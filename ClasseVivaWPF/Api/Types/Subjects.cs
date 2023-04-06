using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Subjects : ApiObject
    {
        [JsonProperty("subjects", Required = Required.Always)]
        public required Subject[] ContentSubjects { get; init; }
    }
}
