using ClasseVivaWPF.Api.Types;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace ClasseVivaWPF.Api
{
    public class Response
    {
        private static JsonSerializerSettings settings;

        static Response()
        {
            settings = new() {
#if DEBUG
                MissingMemberHandling = MissingMemberHandling.Error
#endif
            };
        }

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

            return JsonConvert.DeserializeObject<T>(this.Text, settings);
        }

        public T[]? GetObjectList<T>() where T : ApiObject
        {
            if (this.Text.Contains("error"))
                return null;

            if (this.Text == "") // Bug in backend
                return new T[0];

            return JsonConvert.DeserializeObject<T[]>(this.Text, settings);
        }

        public void GetError()
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<ApiErrorObject>(this.Text)!;
                throw new ApiError(obj);
            }
            catch (JsonReaderException)
            {
                throw new ApiError(new UnparsableException() { response = this });
            }
        }
    }
}
