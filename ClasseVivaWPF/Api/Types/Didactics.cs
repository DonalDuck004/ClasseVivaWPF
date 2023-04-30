using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Didactics : ApiObject
    {
        [JsonProperty("didacticts", Required = Required.Always)]
        public required TeacherDidactic[] DidacticsContent { get; init; }
    }
}
