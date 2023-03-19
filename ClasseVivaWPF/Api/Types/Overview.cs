using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Overview : ApiObject {

        [JsonProperty(Required = Required.Always)]
        public required object[] VirtualClassesAgenda { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required Lesson[] Lessons { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required AgendaEvent[] Agenda { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required Event[] Events { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required object[] Grades { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required Note Notes { get; init; }


        public BaseEvent[] GetBaseEvents()
        {
            var result = new BaseEvent[Lessons.Length + Agenda.Length + Events.Length];
            Lessons.CopyTo(result, 0);
            Agenda.CopyTo(result, Lessons.Length);
            Events.CopyTo(result, Lessons.Length + Agenda.Length);

            return result;
        }

        public Dictionary<DateTime, DayOverview> GetDayOverviews()
        {
            var result = new Dictionary<DateTime, DayOverview>();
            var iterator = this.Lessons.DistinctBy(x => x.Identifiers).OrderBy(x => x.EvtHPos).ThenByDescending(x => x.IsPresence).ToList();
            Lesson lesson;

            for (int i = 0; i < iterator.Count; i++)
            {
                lesson = iterator[i];
                Debug.Assert(lesson.IsLesson);
                if (!result.ContainsKey(lesson.EvtDate.Date))
                    result[lesson.EvtDate.Date] = new(new(), new(), new(), new(), new());

                result[lesson.EvtDate.Date].Lessons.Add(lesson);
                lesson.EvtDuration = this.Lessons.Where(x => x.Identifiers.Equals(lesson.Identifiers)).Count();
                if (lesson.IsPresence)
                {
                    for (; i < iterator.Count && iterator[i].IsGenericCoPresence; i++)
                    {
                        iterator[i].MainTeacher = lesson;
                        iterator[i].EvtDuration = lesson.EvtDuration;
                        result[lesson.EvtDate.Date].Lessons.Add(iterator[i]);
                    }
                }

            }


            foreach (var e in this.Agenda.OrderBy(x => x.EvtDatetimeBegin))
            {
                for (DateTime from = e.EvtDatetimeBegin; from.Date <= e.EvtDatetimeEnd.Date; from = from.AddDays(1))
                {
                    if (!result.ContainsKey(from.Date))
                        result[from.Date] = new(new(), new(), new(), new(), new());
                    if (e.IsNote)
                        result[from.Date].Notes.Add(e);
                    else if (e.IsHomework)
                        result[from.Date].Homeworks.Add(e);
                    else
                    {
                        ;
                    }

                }
            }

            foreach (var e in this.Events.Where(x => x.IsInAbsenceSection).OrderBy(x => x.EvtDate))
            {
                if (!result.ContainsKey(e.EvtDate))
                    result[e.EvtDate] = new(new(), new(), new(), new(), new());
                result[e.EvtDate].Absances.Add(e);
            }

            foreach (var e in this.Events.OrderBy(x => x.EvtDate))
            {
                if (!result.ContainsKey(e.EvtDate))
                    result[e.EvtDate] = new(new(), new(), new(), new(), new());
                result[e.EvtDate].Events.Add(e);
            }

            new Thread(CompressAll).Start(result);
            return result;
        }

        private void CompressAll(object? _items)
        {
            var items = (Dictionary<DateTime, DayOverview>)_items!;


            foreach (var item in items.Values)
                item.Compress();
        }
    }
}
