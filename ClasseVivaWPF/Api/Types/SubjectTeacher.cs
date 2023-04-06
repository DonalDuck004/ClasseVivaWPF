using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class SubjectTeacher : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string TeacherId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string TeacherName { get; init; }
    }
}
