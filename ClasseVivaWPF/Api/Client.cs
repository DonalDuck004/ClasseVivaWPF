using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClasseVivaWPF.Api
{
    public partial class Client
    {
        public static Client INSTANCE { get; private set; } = new Client();
        public const string ENDPOINT = "https://web.spaggiari.eu/";

        public int UserID => SessionHandler.Me!.Id;
        public Me Me => SessionHandler.Me!;

        public HttpClient CVClient { get; private set; }

        public Client(bool set_instance = true)
        {
            if (Config.USE_PROXY)
                HttpClient.DefaultProxy = new WebProxy(Config.PROXY_HOST, Config.PROXY_PORT);

            this.CVClient = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
            });

            this.CVClient.DefaultRequestHeaders.Clear();

            this.CVClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            this.CVClient.DefaultRequestHeaders.Add("z-dev-apikey", "Tg1NWEwNGIgIC0K");
            this.CVClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

            this.CVClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "CVVS/std/4.2.2 C# App/7.1.2");
            
            if (set_instance)
                Api.Client.INSTANCE = this;
        }

        private HttpRequestMessage BuildMessage(HttpMethod method, 
                                                string path,
                                                JObject? data,
                                                string? cached_etag,
                                                DateTime? cached_date,
                                                bool path_is_full_url)
        {
            var message = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = new Uri(path_is_full_url ? path : $"{ENDPOINT}{path}"),
            };

            if (data is not null)
                message.Content = new StringContent(data.ToString(Formatting.None), Encoding.UTF8, "application/json");

            if (cached_date is not null)
                message.Headers.IfModifiedSince = cached_date.Value;

            if (cached_etag is not null)
                message.Headers.Add("z-if-none-match", cached_etag);

            return message;
        }

        private async Task<Response> Send(HttpMethod method, string path, JObject? data, bool allow_cache, bool allow_redirect = true, bool path_is_full_url = false)
        {
            allow_cache = allow_cache && SessionHandler.INSTANCE is not null;

            DateTime? cached_date = null;
            string? cached_json_response = null;
            string? cached_etag = null;
            if (allow_cache)
                cached_date = SessionHandler.INSTANCE!.CheckCache(path, out cached_json_response, out cached_etag);

            var tmp = allow_cache ? "Cache allowed" : "Cache Not Allowed";
            Logger.Log($"Calling {method.Method} {path} - {tmp}");

            var message = BuildMessage(method, path, data, cached_etag, cached_date, path_is_full_url);
            HttpResponseMessage raw_response;
            try
            {
                raw_response = await this.CVClient.SendAsync(message).ConfigureAwait(false);
            }catch (TaskCanceledException e) {
                throw new ApiError(e);
            }
            catch (HttpRequestException e)
            {
                throw new ApiError(e);
            }
            
            if (Logger.CanLog(LogLevel.DEBUG))
                Logger.Log($"{method.Method} {path} replied with {raw_response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult()}", LogLevel.DEBUG);

            Response response;

            if (raw_response.StatusCode is HttpStatusCode.Unauthorized && SessionHandler.INSTANCE is not null)
            {
                SessionHandler.INSTANCE.RenewToken();
                message = BuildMessage(method, path, data, cached_etag, cached_date, path_is_full_url);
                raw_response = await this.CVClient.SendAsync(message).ConfigureAwait(false);
            }

            if (raw_response.StatusCode is HttpStatusCode.NotModified && cached_json_response is not null)
            {
                response = new(cached_json_response);
                if (Logger.CanLog(LogLevel.DEBUG))
                    Logger.Log($"{method.Method} {path} cache hit {cached_json_response}", LogLevel.DEBUG);
                else if (Logger.CanLog(LogLevel.INFO))
                    Logger.Log($"{method.Method} {path} cache hit", LogLevel.INFO);
            }
            else
            {
                response = new(raw_response);
                if (allow_cache)
                {
                    Logger.Log($"{method.Method} {path} cache miss", LogLevel.DEBUG);
                    var new_etag = raw_response.Headers.ETag;
                    SessionHandler.INSTANCE!.SetCache(path, response.Text, new_etag is null ? null : new_etag.ToString());
                }
            }


            return response;
        }

        public async Task<ApiObject> Login(string uid, string pass, string? ident = null)
        {
            var req = new JObject()
            {
                { "uid", uid },
                { "ident", ident },
                { "pass", pass },
            };

            var response = await this.Send(HttpMethod.Post, "rest/v1/auth/login", req, allow_cache: false).ConfigureAwait(false);
            ApiObject? result;
            try
            {
                result = response.GetObject<Me>();
            }catch (JsonSerializationException)
            {
                result = response.GetObject<LoginMultipleChoice>();
            }

            if (result is null)
                response.GetError();

            return result!;
        }

        public void UnSetLoginToken()
        {
            this.CVClient.DefaultRequestHeaders.Remove("z-auth-token");
        }

        public void SetLoginToken(Me me) => SetLoginToken(me.Token);

        public void SetLoginToken(string z_auth_token)
        {
            if (this.CVClient.DefaultRequestHeaders.Contains("z-auth-token"))
                UnSetLoginToken();

            this.CVClient.DefaultRequestHeaders.Add("z-auth-token", z_auth_token);
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
            using (var r = await (await this.CVClient.GetAsync(url)).Content.ReadAsStreamAsync())
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
            var builder = HttpUtility.ParseQueryString("");
            builder.Add("u", u);
            var uri_string = $"https://web.spaggiari.eu/repx/app/default/restbridge.php?t={ticket.TicketString}&" + builder.ToString()!;
            return new Uri(uri_string);
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

        public async Task<Events> GetAbsences()
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/absences/details", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<Events>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<CardSingle> Card(int? id = null)
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{id ?? UserID}/card", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<CardSingle>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Cards> Cards(int? id = null)
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{id ?? UserID}/cards", null, allow_cache: id is null).ConfigureAwait(false);
            var content = response.GetObject<Cards>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Calendar> Calendar()
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/calendar/all", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<Calendar>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public Task<TeachersFrames> TeachersFrames(DateTime from, int? fc = null) => this.TeachersFrames(from.AddDays(-6), from.AddDays(6), fc);
        public Task<TeachersFrames> TeachersFrames(DateTime from, int Range, int? fc = null) => this.TeachersFrames(from.AddDays(-Range), from.AddDays(Range), fc);
        public Task<TeachersFrames> TeachersFrames(DateTime from, DateTime to, int? fc = null) => this.TeachersFrames(from.ToString("yyyyMMdd"), to.ToString("yyyyMMdd"), fc);
        public async Task<TeachersFrames> TeachersFrames(string from, string to, int? fc = null)
        {
            if (fc is not null)
                throw new NotImplementedException(); // ???

            var response = await this.Send(HttpMethod.Get, $"rest/v1/parents/{UserID}/talks/teachersframes2/{from}/{to}", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<TeachersFrames>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<Didactics> Didatics()
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/didactics", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<Didactics>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<FolderContentContentItem> GetDidaticitem(int ItemID)
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/didactics/item/{ItemID}", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<FolderContentContentItem>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<HomeworksContent> Homeworks()
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/homeworks", null, allow_cache: true).ConfigureAwait(false);
            var content = response.GetObject<HomeworksContent>();

            if (content is null)
                response.GetError();

            return content!; // Click on component requires another call
        }

        public async Task<Homework> SetTeacherMsgStatus(int evt_id)
        {
            var data = new JObject() {
                { "messageRead", true }
            };
            var response = await this.Send(HttpMethod.Post, $"rest/v1/students/{UserID}/homeworks/setTeacherMsgStatus/NEWDC/{evt_id}", data, allow_cache: false).ConfigureAwait(false);
            var content = response.GetObject<Homework>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<S3FileHeader> GetHeaderS3(int evt_id)
        {
            var response = await this.Send(HttpMethod.Get, $"rest/v1/students/{UserID}/homeworks/getHeaderS3/NEWDC/{evt_id}", null, allow_cache: false).ConfigureAwait(false);
            var content = response.GetObject<S3FileHeader>();

            if (content is null)
                response.GetError();

            return content!;
        }

        public async Task<bool> UploadToS3(S3FileHeader header, Uri path)
        {
            var url = $"https://s3.eu-south-1.amazonaws.com/{header.Bucket}";
            var fname = Path.GetFileName(path.LocalPath);
            MultipartFormDataContent form = new();
            
            AddBinaryContent(header.ACL, "acl");
            AddStrContent(header.Key, "key");
            AddStrContent($"attachment; filename=\"{fname}\"", "Content-Disposition");
            AddStrContent(header.SAS.ToString(), "success_action_status", "application/json; charset=utf-8");
            AddStrContent(header.ContentType, "Content-Type");
            AddStrContent(header.Tags, "Tagging");
            AddStrContent(header.XAC, "X-Amz-Credential");
            AddStrContent(header.XAA, "X-Amz-Algorithm");
            AddStrContent(header.XAD, "X-Amz-Date");
            AddStrContent(header.Policy, "Policy");
            AddStrContent(header.XAS, "X-Amz-Signature");

            var tmp = new StreamContent(new FileStream(path.LocalPath, FileMode.Open, FileAccess.Read));
            tmp.Headers.Add("Content-Type", "text/plain");
            tmp.Headers.Add("Content-Length", "0");
            tmp.Headers.Add("Content-Disposition", $"form-data; name=\"file\"; filename=\"{fname}\"");
            form.Add(tmp);

            var reply = await this.CVClient.SendAsync(new HttpRequestMessage(header.EffectiveMethod, url)
            {
                Content = form
            });

            return reply.StatusCode == (HttpStatusCode)header.SAS;

            void AddBinaryContent(string data, string key)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var tmp = new ByteArrayContent(bytes, 0, bytes.Length);
                form.Add(tmp, $"\"{key}\"");
            }

            void AddStrContent(string text, string key, string content_type = "text/plain; charset=utf-8")
            {
                var l = Encoding.UTF8.GetBytes(text);
                var content = new StringContent(text);
                content.Headers.Add("Content-Transfer-Encoding", "binary");
                content.Headers.Add("Content-Length", l.Length.ToString());
                content.Headers.TryAddWithoutValidation("Content-Type", content_type);
                form.Add(content, $"\"{key}\"");
            }
        }

        public async Task<Homework> AddS3Homework(int file_id, string filename, int evt_id)
        {
            var req = new JObject()
            {
                { "fileId", file_id },
                { "filename", filename },
            };

            var response = await this.Send(HttpMethod.Post, $"rest/v1/students/{UserID}/homeworks/addFileS3/NEWDC/{evt_id}", req, allow_cache: false).ConfigureAwait(false);
            var content = response.GetObject<Homework>();

            if (content is null)
                response.GetError();

            return content!;
        }
    }
}
