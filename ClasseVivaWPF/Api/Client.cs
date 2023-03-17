using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Cryptography;
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
            HttpClient.DefaultProxy = new WebProxy("http://localhost", 8000);
            
            
            this.client = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
            });


            this.client.DefaultRequestHeaders.Clear();

            this.client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            this.client.DefaultRequestHeaders.Add("z-dev-apikey", "Tg1NWEwNGIgIC0K");
            this.client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            this.client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "CVVS/std/4.2.2 Android/7.1.2");

            Client.INSTANCE = this;
        }

        private void _NotImplementedCheck(object? v)
        {
            Debug.Assert(v is null);
        }

        private async Task<Response> Send(HttpMethod method, string path, JObject? data, DateTime? cache_date = null)
        {
            var message = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = new Uri($"{ENDPOINT}{path}"),
            };

            if (data is not null)
                message.Content = new StringContent(data.ToString(Formatting.None), Encoding.UTF8, "application/json");

            if (cache_date is not null)
                message.Headers.IfModifiedSince = cache_date.Value; // TODO Fixxare bug, non cotiene orario, ma solo data

            var response = await this.client.SendAsync(message).ConfigureAwait(false);

            // DumpMsg(response);
            var result = new Response(response);
            return result;
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

        public async Task<Me> Login(string uid, string password, object? indent = null)
        {
            _NotImplementedCheck(indent);
            var req = new JObject()
            {
                { "uid", uid },
                { "indent", null },
                { "pass", password },
            };

            var response = await this.Send(HttpMethod.Post, "rest/v1/auth/login", req).ConfigureAwait(false);
            var me = response.GetObject<Me>();
            if (me is null)
                response.GetError();

            return me!;
        }

        public void SetLoginToken(Me me) => SetLoginToken(me.Token);

        public void SetLoginToken(string z_auth_token)
        {
            this.client.DefaultRequestHeaders.Add("z-auth-token", z_auth_token);
        }

        public Task<Overview> Overview(DateTime from) => this.Overview(from, from.AddDays(6));
        public Task<Overview> Overview(DateTime from, DateTime to) => this.Overview(from.ToString("yyyyMMdd"), to.ToString("yyyyMMdd"));

        public async Task<Overview> Overview(string from, string to)
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/overview/all/{from}/{to}", null).ConfigureAwait(false);
            var overview = response.GetObject<Overview>();
            if (overview is null)
                response.GetError();

            return overview!;
        }

        public async Task<Content[]> Contents()
        {
            const string path = "auc/api/v2/contents";
            var cache_date = SessionHandler.INSTANCE!.CheckCache(path, out string? json_response);
            var response = await this.Send(HttpMethod.Get, path, null, cache_date: cache_date).ConfigureAwait(false);

            if (response.RawResponse.StatusCode is HttpStatusCode.NotModified)
                return JsonConvert.DeserializeObject<Content[]>(json_response!)!;

            var content = response.GetObjectList<Content>();
            SessionHandler.INSTANCE!.SetCache(path, response.Text);

            if (content is null)
                response.GetError();

            return content!;
        }

        private MD5 cacher = MD5.Create();

        public async Task<Uri> Download(string url, int buffer_size = 1024 * 1024)
        {
            var md5 = Convert.ToHexString(cacher.ComputeHash(Encoding.UTF8.GetBytes(url))) + Path.GetExtension(url);
            var path = Path.Join(Path.GetTempPath(), md5);
            if (!File.Exists(path))
            {
                var buff = new byte[buffer_size];
                using (var r = await (await this.client.GetAsync(url)).Content.ReadAsStreamAsync()){
                    using (var w = File.OpenWrite(path))
                    {
                        while ((await r.ReadAsync(buff, 0, buff.Length)) != 0)
                            await w.WriteAsync(buff, 0, buff.Length);
                    }
                }
            }

            return new Uri(path);
        }
    }
}
