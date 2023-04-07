using Newtonsoft.Json;


namespace ClasseVivaWPF.Api.Types
{
    public class MinigameCredentials : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string MinigameToken;
        [JsonProperty(Required = Required.Always)]
        public required string @For;
    }
}
