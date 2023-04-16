using Newtonsoft.Json;

namespace ClasseVivaWPF.Sessions
{
    public class AccountMeta {
        [JsonProperty(Required = Required.Always)]
        public required string Ident { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Name { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string School { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Initials { get; init; }
    }
}
