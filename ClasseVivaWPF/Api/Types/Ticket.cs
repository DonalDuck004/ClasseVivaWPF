using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Ticket : ApiObject
    {
        [JsonProperty("ticket", Required = Required.Always)]
        public required string TicketString { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int Len { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int ULen { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string MD5 { get; init; }
    }
}
