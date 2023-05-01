using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class FolderContentContentItem : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required FolderContentItem Item { get; init; }
    }
}
