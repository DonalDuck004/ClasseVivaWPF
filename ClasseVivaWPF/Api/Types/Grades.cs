using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Grades : ApiObject
    {
        [JsonProperty("grades", Required = Required.Always)]
        public required Grade[] ContentGrades;
    }
}
