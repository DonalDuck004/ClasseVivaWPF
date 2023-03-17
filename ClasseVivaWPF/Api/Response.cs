using ClasseVivaWPF.Api.Types;
using Newtonsoft.Json;
using System.Net.Http;

namespace ClasseVivaWPF.Api
{
    public class Response
    {
        public HttpResponseMessage RawResponse { get; private set; }
        private string? _cache = null;
        public string Text
        {
            get
            {
                if (_cache is null)
                    _cache = RawResponse.Content.ReadAsStringAsync().Result;

                return _cache;
            }
        }

        public Response(HttpResponseMessage Response)
        {
            this.RawResponse = Response;
        }

        public T? GetObject<T>() where T : ApiObject
        {
            if (this.Text.Contains("error"))
                return null;
            return JsonConvert.DeserializeObject<T>(this.Text);
        }

        public T[]? GetObjectList<T>() where T : ApiObject
        {
            if (this.Text.Contains("error"))
                return new T[0];

            return JsonConvert.DeserializeObject<T[]>(this.Text);
        }

        public void GetError()
        {
            var obj = JsonConvert.DeserializeObject<ApiErrorObject>(this.Text)!;
            throw new ApiError(obj);
        }
    }
}
