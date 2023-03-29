using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ClasseVivaWPF.Api.Types
{

    public class DayOverview
    {
        public List<Lesson> Lessons = new();
        public List<AgendaEvent> Notes = new();
        public List<AgendaEvent> Homeworks = new();
        public List<Event> Absances = new();
        public List<Grade> Grades = new();
        public List<Event> Events = new();

        public DateTime XCreationDate { get; private set; } = DateTime.Now;

        public static readonly Color[] COLORS = new Color[] { Colors.Coral, Colors.Red, Colors.Brown, Colors.Gray,
                                                              Colors.DarkCyan, Colors.Orchid, Colors.Teal, Colors.GreenYellow,
                                                              Color.FromRgb(219, 195, 0), Colors.Green, Colors.Black, Colors.DarkOrange,
                                                              Colors.DarkGreen, Colors.DarkOrchid, Colors.DarkRed, Colors.RoyalBlue,
                                                              Colors.Purple, Colors.Blue, Colors.Orange, Colors.Pink };

        public void Compress()
        {
            this.Lessons.Capacity = this.Lessons.Count;
            this.Notes.Capacity = this.Notes.Count;
            this.Homeworks.Capacity = this.Homeworks.Count;
            this.Absances.Capacity = this.Absances.Count;
            this.Events.Capacity = this.Events.Count;
            this.Grades.Capacity = this.Grades.Count;
        }
    }
}
