using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClasseVivaWPF.Api.Types
{
    public class Lesson : BaseEvent
    {
        private static List<int> ColorsIDS = new List<int>();
        private int? _color_id = null;

        [JsonProperty(Required = Required.Always)] 
        public required DateTime EvtDate {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required int EvtHPos {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required int EvtDuration {get; set;}

        [JsonProperty(Required = Required.Always)]
        public required string ClassDesc {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required string AuthorName {get; init;}

        private int _SubjectId;

        [JsonProperty(Required = Required.Always)]
        public required int SubjectId {
            get => _SubjectId; 
            init {
                if (!ColorsIDS.Contains(value))
                    ColorsIDS.Add(value);
                _SubjectId = value;
            }
        }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SubjectCode {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required string SubjectDesc {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required string LessonType {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required string LessonArg { get; init; }

        public Lesson MainTeacher { get; internal set; }

        public int ColorID
        {
            get => _color_id ??= (ColorsIDS.IndexOf(this.SubjectId) % DayOverview.COLORS.Length);
            set => _color_id = value;
        }

        internal Tuple<int, string, string, string, string, DateTime> Identifiers => new(SubjectId, LessonType, LessonArg, AuthorName, ClassDesc, EvtDate);

        [JsonConstructor]
        public Lesson()
        {
            MainTeacher = this;
        }

        public override void BuildNotifyText(ToastContentBuilder toast)
        {
            toast.AddText($"{AuthorName} a {EvtHPos}°");
            toast.AddText(LessonArg);
            toast.AddText(LessonType);
        }

        public override DateTime GetGotoDate() => this.EvtDate;
    }
}
