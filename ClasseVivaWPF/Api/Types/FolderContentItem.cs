using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class FolderContentItem : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string Link { get; init; }
    }
}
