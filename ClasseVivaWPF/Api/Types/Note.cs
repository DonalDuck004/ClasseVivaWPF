using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Note : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required object[] NTTE { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required object[] NTCL { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required object[] NTWN { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required object[] NTST { get; init; }
    }
}
