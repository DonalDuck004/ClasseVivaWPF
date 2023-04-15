using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class LoginChoice : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string CID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Ident { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Name { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string School { get; init; }
    }
}
