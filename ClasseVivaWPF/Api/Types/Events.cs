using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Events : ApiObject
    {
        [JsonProperty("events", Required = Required.Always)]
        public required Event[] ContentEvents { get; init; }
    }
}
