using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class HomeworkFile : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int FileID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FileName { get; init; }

        [JsonProperty("attachNum", Required = Required.Always)]
        public required int AttachIndex { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? Note { get; init; }
    }
}
