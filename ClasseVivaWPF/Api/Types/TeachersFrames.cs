using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class TeachersFrames : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required TeacherFrame[] Teacher { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required Talk[] Talks { get; init; }
    }
}
