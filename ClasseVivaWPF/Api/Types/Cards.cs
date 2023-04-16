using Newtonsoft.Json;


namespace ClasseVivaWPF.Api.Types
{
    public class Cards : ApiObject
    {
        [JsonProperty("cards", Required = Required.Always)]
        public required Card[] ContentCards { get; init; }
    }
}
