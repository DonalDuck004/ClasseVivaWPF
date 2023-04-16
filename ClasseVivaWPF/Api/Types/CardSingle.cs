using Newtonsoft.Json;


namespace ClasseVivaWPF.Api.Types
{
    public class CardSingle : ApiObject
    {
        [JsonProperty("card", Required = Required.Always)]
        public required Card ContentCard { get; init; }
    }
}
