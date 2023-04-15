using ClasseVivaWPF.Api.Types;
using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class ContentDetail : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string Img { get; init; }
    }
}
