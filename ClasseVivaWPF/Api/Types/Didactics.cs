using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Didactics : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required TeacherDidactic[] DidacticsContent { get; init; }
    }
}
