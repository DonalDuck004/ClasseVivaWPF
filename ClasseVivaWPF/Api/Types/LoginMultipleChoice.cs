using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class LoginMultipleChoice : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string RequestedAction { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required LoginChoice[] Choices { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Message { get; init; }
    }
}
