using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class ContentNoticeboard : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required Noticeboard[] Items { get; init; }
    }
}
}
