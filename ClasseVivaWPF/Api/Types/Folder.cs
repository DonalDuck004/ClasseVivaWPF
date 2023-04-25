using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class Folder : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int FolderID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FolderName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime LastShareDT { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required FolderContent[] Contents { get; init; }
    }
}
