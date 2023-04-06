using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClasseVivaWPF.Api
{
    public class Client
    {
        public static Client INSTANCE { get; private set; } = new Client();
        private const string ENDPOINT = "https://web.spaggiari.eu/";

        private int UserID => SessionHandler.Me!.Id;
        private HttpClient client;

        public Client()
        {
            if (Config.USE_PROXY)
                HttpClient.DefaultProxy = new WebProxy(Config.PROXY_HOST, Config.PROXY_PORT);

            this.client = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
            });


            this.client.DefaultRequestHeaders.Clear();

            this.client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            this.client.DefaultRequestHeaders.Add("z-dev-apikey", "Tg1NWEwNGIgIC0K");
            this.client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            this.client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "CVVS/std/4.2.2 C# App/7.1.2");

            Client.INSTANCE = this;
        }

        private void _NotImplementedCheck(object? v)
        {
            Debug.Assert(v is null);
        }

        private HttpRequestMessage BuildMessage(HttpMethod method, string path, JObject? data, string? cached_etag, DateTime? cached_date)
        {
            var message = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = new Uri($"{ENDPOINT}{path}"),
            };

            if (data is not null)
                message.Content = new StringContent(data.ToString(Formatting.None), Encoding.UTF8, "application/json");

            if (cached_date is not null)
                message.Headers.IfModifiedSince = cached_date.Value; // TODO Fixxare bug, non cotiene orario, ma solo data

            if (cached_etag is not null)
                message.Headers.Add("z-if-none-match", cached_etag);

            return message;
        }

        private async Task<Response> Send(HttpMethod method, string path, JObject? data, bool allow_cache)
        {
            allow_cache = allow_cache && SessionHandler.INSTANCE is not null;

            DateTime? cached_date = null;
            string? cached_json_response = null;
            string? cached_etag = null;
            if (allow_cache)
                cached_date = SessionHandler.INSTANCE!.CheckCache(path, out cached_json_response, out cached_etag);


            var message = BuildMessage(method, path, data, cached_etag, cached_date);
            var raw_response = await this.client.SendAsync(message).ConfigureAwait(false);

            // DumpMsg(response);
            Response response;

            if (raw_response.StatusCode is HttpStatusCode.Unauthorized)
            {
                Debug.Assert(SessionHandler.INSTANCE is not null);
                SessionHandler.INSTANCE.RenewToken();
                message = BuildMessage(method, path, data, cached_etag, cached_date);
                raw_response = await this.client.SendAsync(message).ConfigureAwait(false);
            }

            if (raw_response.StatusCode is HttpStatusCode.NotModified && cached_json_response is not null)
                response = new(cached_json_response);
            else
            {
                response = new(raw_response);
                if (allow_cache)
                {
                    var new_etag = raw_response.Headers.ETag;
                    SessionHandler.INSTANCE!.SetCache(path, response.Text, new_etag is null ? null : new_etag.ToString());
                }
            }


            return response;
        }

        private void DumpMsg(HttpResponseMessage response)
        {
            var msg = "";

            foreach (var item in response.RequestMessage!.Headers)
                msg += $"{item.Key}: {string.Join("\n", item.Value)}\n";
            foreach (var item in response.RequestMessage.Content!.Headers)
                msg += $"{item.Key}: {string.Join("\n", item.Value)}\n";
            msg += "\n\n";

            foreach (var item in response.Content.Headers)
                msg += $"{item.Key}: {string.Join("\n", item.Value)}\n";
            foreach (var item in response.Headers)
                msg += $"{item.Key}: {string.Join("\n", item.Value)}\n";
            MessageBox.Show(msg);
        }

        public async Task<Me> Login(string uid, string pass, string? ident = null)
        {
            var req = new JObject()
            {
                { "uid", uid },
                { "ident", ident },
                { "pass", pass },
            };

            var response = await this.Send(HttpMethod.Post, "rest/v1/auth/login", req, allow_cache: false).ConfigureAwait(false);
            var me = response.GetObject<Me>();
            if (me is null)
                response.GetError();

            return me!;
        }
        public void UnSetLoginToken()
        {
            this.client.DefaultRequestHeaders.Remove("z-auth-token");
        }

        public void SetLoginToken(Me me) => SetLoginToken(me.Token);

        public void SetLoginToken(string z_auth_token)
        {
            this.client.DefaultRequestHeaders.Add("z-auth-token", z_auth_token);
        }

        public Task<Overview> Overview(DateTime from) => this.Overview(from.AddDays(-6), from.AddDays(6));
        public Task<Overview> Overview(DateTime from, int Range) => this.Overview(from.AddDays(-Range), from.AddDays(Range));
        public Task<Overview> Overview(DateTime from, DateTime to) => this.Overview(from.ToString("yyyyMMdd"), to.ToString("yyyyMMdd"));

        public async Task<Overview> Overview(string from, string to)
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/overview/all/{from}/{to}", null, allow_cache: true).ConfigureAwait(false);
            var overview = response.GetObject<Overview>();
            if (overview is null)
                response.GetError();

            return overview!;
        }

        public async Task<Content[]> Contents()
        {
            var response = await this.Send(HttpMethod.Get, "auc/api/v2/contents", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObjectList<Content>();

            if (content is null)
                response.GetError();

            return content!;
        }


        public async Task Download(string url, string path, int buffer_size = 1024 * 1024)
        {
            var buff = new byte[buffer_size];
            using (var r = await (await this.client.GetAsync(url)).Content.ReadAsStreamAsync())
            {
                using (var w = File.OpenWrite(path))
                {
                    while ((await r.ReadAsync(buff, 0, buff.Length)) != 0)
                        await w.WriteAsync(buff, 0, buff.Length);
                }
            }
        }

        public async Task<Interaction> GetInteractions(int content_id)
        {
            var response = await this.Send(HttpMethod.Get, $"auc/api/v2/getInteractions/{content_id}", null, allow_cache: true).ConfigureAwait(false);

            var content = response.GetObject<Interaction>();

            if (content is null)
                response.GetError();

            return content!;
        }


        public async Task<ApiObject[]> DeleteInteraction(int content_id, string interaction_type)
        {
            var req = new JObject()
            {
                { "content_id", content_id },
                { "interaction_type", interaction_type },
            };


            var response = await this.Send(HttpMethod.Post, "auc/api/v2/deleteInteraction", req, allow_cache: false).ConfigureAwait(false);

            var content = response.GetObjectList<ApiObject>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<ApiObject[]> SetInteraction(int content_id, string interaction_type)
        {
            var req = new JObject()
            {
                { "content_id", content_id },
                { "interaction_type", interaction_type },
            };

            var response = await this.Send(HttpMethod.Post, "auc/api/v2/setInteraction", req, allow_cache: false).ConfigureAwait(false);

            var content = response.GetObjectList<ApiObject>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Ticket> Ticket()
        {
            var response = await this.Send(HttpMethod.Get, "rest/v1/auth/ticket", null, allow_cache: false).ConfigureAwait(false);
            var content = response.GetObject<Ticket>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Uri> GetUriFromTicket(string u)
        {
            var ticket = await this.Ticket();
            var query = "t=" + ticket.TicketString + "&u=" + u;
            return new UriBuilder("https://web.spaggiari.eu/repx/app/default/restbridge.php") { Query = query.ToString() }.Uri;
        }

        public async Task<MinigameCredentials> GetMinigameCredentials()
        {
            var response = await this.Send(HttpMethod.Get, "rest/v1/auth/minigame", null, allow_cache: false).ConfigureAwait(false);
            var content = response.GetObject<MinigameCredentials>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Bookmark[]> GetBookmarks()
        {
            var response = await this.Send(HttpMethod.Get, "auc/api/v2/getBookmarks", null, allow_cache: false).ConfigureAwait(false);
            var content = response.GetObjectList<Bookmark>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Subjects> GetSubjects()
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/subjects", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<Subjects>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Grades> GetGrades()
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/grades2", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<Grades>();

            if (content is null)
                response.GetError();

            return content!;
        }
    }
}
