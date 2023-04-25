using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class NoticeboardAttachment : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string FileName { get; init; }

        [JsonProperty("attachNum", Required = Required.Always)]
        public required bool AttachCount { get; init; }
    }
}
}
