using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace ClasseVivaWPF.Api.Types
{
    public class Homework : BaseEvent
    {
        [JsonProperty(Required = Required.Always)]
        public required string TeacherName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int TeacherID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string HomeworkDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool HomeworkDone { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime ExpiryDate { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int SubjectId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SubjectDesc { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? LastStudentMsg { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? LastTeacherMsg { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool NewMessages { get; init; }

        private HomeworkFile[] _not_implemented = new HomeworkFile[0];

        [JsonProperty(Required = Required.Always)]
        public required object[] TeacherFiles { get => _not_implemented; init
            {
                if (value.Length != 0)
                    Debugger.Break();
            }
        }

        [JsonProperty(Required = Required.Always)]
        public required object[] TeacherLinks
        {
            get => _not_implemented; init
            {
                if (value.Length != 0)
                    Debugger.Break();
            }
        }

        [JsonProperty(Required = Required.Always)]
        public required HomeworkFile[] StudentFiles { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required object[] CorrectedFiles
        {
            get => _not_implemented; init
            {
                if (value.Length != 0)
                    Debugger.Break();
            }
        }
    }
}
