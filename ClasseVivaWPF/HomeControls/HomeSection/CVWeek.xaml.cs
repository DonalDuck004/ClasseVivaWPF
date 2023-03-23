using ClasseVivaWPF.Utils;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    /// <summary>
    /// Logica di interazione per CVWeek.xaml
    /// </summary>
    public partial class CVWeek : UserControl
    {
        public CVWeek? Previus = null;
        public CVWeek? Next = null;
        public DateTime From { get; private set; }
        public DateTime To => From.AddDays(6);

        public CVWeek(DateTime from, CVWeek? previus = null, CVWeek? next = null)
        {
            InitializeComponent();

            this.Previus = previus;
            this.Next = next;

            this.From = from.AddDays(-from.DayOfWeek.AsInt32());

            for (int i = 0; i < 7; i++)
            {
                this.grid.Children.Add(new CVDay(this.From.AddDays(i), parent: this));
                Grid.SetColumn(grid.Children[i], i);
            }

            this.grid.Children.Capacity = 7; // Clean up memory

            if (from == DateTime.Now.Date)
                this.SelectChild(from.DayOfWeek);
        }
        ~CVWeek()
        {
            ;
        }

        public void SelectChild(DayOfWeek day, bool update = false) => SelectChild(day.AsInt32(), update);

        public void SelectChild(int idx, bool update = false)
        {
            var child = this.GetChild(idx);
            child.IsSelected = true;
            if (update)
                child.Update(require_new_call: true);
        }

        public void GetChild(DayOfWeek day) => GetChild(day.AsInt32());
        public CVDay GetChild(int idx)
        {
            return (CVDay)grid.Children[idx];
        }

        public void View(StackPanel content)
        {
            if (this.Next is null)
                CVHome.INSTANCE.AddWeek(this.Next = new(from: this.To.AddDays(1), previus: this));
            
            if (this.Previus is null)
                 CVHome.INSTANCE.AddWeek(this.Previus = new(from: this.From.AddDays(-7), next: this), idx: 0);

            CVHome.INSTANCE.homework_scroller.Content = content;
            CVHome.INSTANCE.UpdateDayLabel();
            this.Dispatcher.InvokeAsync(this.BringIntoView);
        }

        internal void BeginDestroy()
        {
            if (ReferenceEquals(CVDay.SelectedDay!.Parent, this))
                return;

            if (this.Previus is not null)
                this.Previus.Next = null;
            else if (this.Next is not null)
                this.Next.Previus = null;


            foreach (CVDay item in this.grid.Children)
                item.BeginDestroy();
            
            this.grid.Children.Clear();
        }

        internal static CVWeek GetWeekContaining(DateTime date)
        {
            var week = CVDay.SelectedDay!.Parent;

            if (date < week.From)
            {
                while (date < week.From)
                {
                    if (week.Previus is null)
                        CVHome.INSTANCE.AddWeek(week.Previus = new(date, next: week), 0);
                    week = week.Previus;
                }
            }
            else if (date > week.To)
            {
                while (date > week.To)
                {
                    if (week.Next is null)
                        CVHome.INSTANCE.AddWeek(week.Next = new(date, previus: week));
                    week = week.Next;
                }
            }

            return week;
        }
    }
}
