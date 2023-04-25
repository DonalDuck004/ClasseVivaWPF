using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class FolderContent : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int ContentID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string ContentName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int ObjectId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string ObjectType { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime ShareDT { get; init; }
    }
}
