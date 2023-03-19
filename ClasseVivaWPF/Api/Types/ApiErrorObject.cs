using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class ApiErrorObject : ApiObject
    {
       
        [JsonProperty(Required = Required.Always)]
        public required int StatusCode { get; init; }
        [JsonProperty(Required = Required.Always)]
        public required string Error { get; init; }
        [JsonProperty(Required = Required.Always)]
        public required string Info { get; init; }
        [JsonProperty(Required = Required.Always)]
        public required string Message { get; init; }
    }
}
