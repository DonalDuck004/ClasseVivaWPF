using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ClasseVivaWPF.Api.Types
{
    public class Lesson : BaseEvent, IBuildNotifyCalendar
    {
        private int? _color_id = null;

        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDate { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int EvtHPos { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int EvtDuration { get; set; }

        [JsonProperty(Required = Required.Always)]
        public required string ClassDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string AuthorName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int SubjectId { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SubjectCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SubjectDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string LessonType { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string LessonArg { get; init; }

        public Lesson MainTeacher { get; internal set; }

        public int ColorID
        {
            get => _color_id ??= Subject.ColorIDFor(this.SubjectId); 
            private set => _color_id = value;
        }

        internal Tuple<int, string, string, string, string, DateTime> Identifiers => new(SubjectId, LessonType, LessonArg, AuthorName, ClassDesc, EvtDate);

        [JsonConstructor]
        public Lesson()
        {
            MainTeacher = this;
        }

        public void BuildNotify(ToastContentBuilder toast)
        {
            toast.AddText($"{AuthorName.ToTitle()} a {EvtHPos}°");
            toast.AddText(LessonArg);
            toast.AddText(LessonType);
        }

        public DateTime GetGotoDate() => this.EvtDate.Date;
    }
}
