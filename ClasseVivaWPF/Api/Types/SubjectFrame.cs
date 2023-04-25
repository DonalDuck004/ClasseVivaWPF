using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class SubjectFrame : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int SubjectId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Description { get; init; }
    }
}
