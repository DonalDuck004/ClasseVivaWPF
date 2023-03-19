using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class RelatedContentDetail : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int Id { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Img { get; init; }
    }
}
