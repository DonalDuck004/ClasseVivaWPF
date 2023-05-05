using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class HomeworksContent : ApiObject
    {
        [JsonProperty("Items", Required = Required.Always)]
        public required Homework[] Homeworks { get; init; }
    }
}
