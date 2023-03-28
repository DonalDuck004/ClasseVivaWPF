using ClasseVivaWPF.Api.Types;
using Newtonsoft.Json;
using System.Net.Http;
using System;

namespace ClasseVivaWPF.Api
{
    public class Response
    {
        public HttpResponseMessage? RawResponse { get; private init; }
        private string? _text = null;
        public string Text
        {
            get
            {
                if (_text is null)
                    _text = (RawResponse ?? throw new Exception()).Content.ReadAsStringAsync().Result;

                return _text;
            }
        }
        public bool IsCached { get; private set; }

        public Response(HttpResponseMessage Response)
        {
            this.IsCached = false;
            this.RawResponse = Response;
        }

        public Response(string Text)
        {
            this.IsCached = true;
            this._text = Text;
        }

        public T? GetObject<T>() where T : ApiObject
        {
            if (this.Text.Contains("error"))
                return null;

            return JsonConvert.DeserializeObject<T>(this.Text, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error });
        }

        public T[]? GetObjectList<T>() where T : ApiObject
        {
            if (this.Text.Contains("error"))
                return new T[0];

            if (this.Text == "") // Bug in backend
                return new T[0];

            return JsonConvert.DeserializeObject<T[]>(this.Text, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error });
        }

        public void GetError()
        {
            var obj = JsonConvert.DeserializeObject<ApiErrorObject>(this.Text)!;
            throw new ApiError(obj);
        }
    }
}
