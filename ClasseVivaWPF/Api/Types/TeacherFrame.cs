﻿using Newtonsoft.Json;
using System.Net.Mail;

namespace ClasseVivaWPF.Api.Types
{
    public class TeacherFrame : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int TeacherID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string TeacherName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required SubjectFrame[] Subjects { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool HasOpenTalks { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required Frame[] Frames { get; init; }
    }
}
