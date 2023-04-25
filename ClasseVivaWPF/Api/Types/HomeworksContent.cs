using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class HomeworksContent : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required Homework[] Items { get; init; }
    }
}
