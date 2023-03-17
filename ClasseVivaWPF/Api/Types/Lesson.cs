using System;
using System.Collections.Generic;
using System.Linq;

namespace ClasseVivaWPF.Api.Types
{
    public record class Lesson : BaseEvent
    {
        private static List<int> ColorsIDS = new List<int>();
        private int? _color_id = null;

        public DateTime EvtDate {get; internal set;}
        public int EvtHPos {get; internal set;}
        public int EvtDuration {get; internal set;}
        public string ClassDesc {get; internal set;}
        public string AuthorName {get; internal set;}
        public int SubjectId {get; internal set;}
        public string SubjectCode {get; internal set;}
        public string SubjectDesc {get; internal set;}
        public string LessonType {get; internal set;}
        public Lesson MainTeacher { get; internal set; }
        public string LessonArg { get; internal set; }

        public int ColorID
        {
            get => _color_id ??= (ColorsIDS.IndexOf(this.SubjectId) % DayOverview.COLORS.Length);
            set => _color_id = value;
        }

        internal Tuple<int, string, string, string, string, DateTime> Identifiers => new(SubjectId, LessonType, LessonArg, AuthorName, ClassDesc, EvtDate);


        public Lesson(int evtId,
                      DateTime evtDate,
                      string evtCode,
                      int evtHPos,
                      int evtDuration,
                      string classDesc,
                      string authorName,
                      int subjectId,
                      string subjectCode,
                      string subjectDesc,
                      string lessonType,
                      string lessonArg) : base(evtId, evtCode)
        {
            if (!ColorsIDS.Contains(subjectId))
                ColorsIDS.Add(subjectId);

            EvtDate = evtDate;
            EvtHPos = evtHPos;
            EvtDuration = evtDuration;
            ClassDesc = classDesc;
            AuthorName = authorName;
            SubjectId = subjectId;
            SubjectCode = subjectCode;
            SubjectDesc = subjectDesc;
            LessonType = lessonType;
            LessonArg = lessonArg;
            MainTeacher = this;
        }
    }
}
