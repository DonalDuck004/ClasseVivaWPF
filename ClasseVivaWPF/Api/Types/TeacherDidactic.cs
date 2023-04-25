using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class TeacherDidactic : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int TeacherID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string TeacherName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string TeacherFirstName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string TeacherLastName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required Folder[] Folders { get; init; }
    }
}
