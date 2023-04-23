using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    /// <summary>
    /// Logica di interazione per CVHome.xaml
    /// </summary>
    public partial class CVHome : UserControl, IOnSwitch, IOnKeyDown, IOnFullReload
    {
        public static CVHome INSTANCE { get; private set; } = new();
        public Dictionary<DateTime, List<Content>>? Contents { get; set; } = null;

        private CVHome()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static void GlobDispose()
        {
            CVDay.GlobDispose();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.Init();

            this.days_scroller.SizeChanged += (s, e) =>
            {
                if (CVDay.SelectedDay is not null)
                    CVDay.SelectedDay.Parent.BringIntoView();
            };

            this.Loaded -= OnLoad;
        }

        public void UpdateDayLabel()
        {
            this.CurrentDayLbl.Text = CVDay.SelectedDay!.Date.ToString("dddd dd MMMM yyyy");
        }

        public void Init()
        {
            var now = DateTime.Now.Date;
            var item = CVWeek.NewUnsafe(from: now);

            var idx = this.days_wp.Children.Count - 1;
            this.days_wp.Children.Insert(idx == -1 ? 0 : idx, item);
            item.SelectChild(now.DayOfWeek);
        }

        public int IndexOfWeek(CVWeek week) => this.days_wp.Children.ReferenceIndexOf(week);

        public void AddWeek(CVWeek week, int? idx = null)
        {
            if (idx is null)
            {
                idx = 0;
                foreach (CVWeek item in this.days_wp.Children)
                {
                    if (item.From < week.From)
                        idx++;
                    else
                        break;
                }
            }
            if (idx < 0)
                idx = 0;

            this.days_wp.Children.Insert(idx.Value, week);
            if (this.days_wp.Children.Count > 5)
            {
                CVWeek? to_delete = null;
                for (var i = this.days_wp.Children.Count - 1; i >= 0; i--)
                {
                    to_delete = (CVWeek)this.days_wp.Children[i];

                    if (ReferenceEquals(to_delete, CVDay.SelectedDay!.Parent) ||
                        ReferenceEquals(to_delete, week) ||
                        to_delete.To.AddDays(1) == week.From ||
                        to_delete.From.AddDays(-7) == week.From ||
                        to_delete.IsUserVisible(this.days_scroller))
                        continue;

                    this.days_wp.Children.RemoveAt(i);
                    to_delete.BeginDestroy();
                }

                if (to_delete is not null)
                {
                    to_delete = null;
                    GC.Collect();
                }
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Left || e.Key is Key.Right)
                DaysOnKeyDown(sender, e);
            else if (e.Key is Key.F5)
                this.UpdateSelected();
        }

        private void DaysOnKeyDown(object sender, KeyEventArgs e)
        {
            if (CVDay.SelectedDay is null || (e.Key is not Key.Left && e.Key is not Key.Right))
                return;

            if (e.KeyboardDevice.Modifiers is ModifierKeys.Control)
            {
                var idx = this.days_scroller.HorizontalOffset / CVDay.SelectedDay.Parent.ActualWidth;

                if (e.Key is Key.Left)
                    idx--;
                else
                    idx++;

                if ((idx %= this.days_wp.Children.Count) == -1)
                    idx = 0;
                ((CVWeek)this.days_wp.Children[(int)idx]).Scroll();
            }
            else
            {
                var idx = CVDay.SelectedDay.ParentIdx + (e.Key is Key.Left ? -1 : 1);

                if (idx == 7)
                    idx = 0;
                else if (idx == -1)
                    idx = 6;

                CVDay.SelectedDay.Parent.SelectChild(idx);
            }
        }

        private void OnOpenCalendar(object sender, MouseButtonEventArgs e)
        {
            var calendar = new CVCalendar();

            calendar.Inject();
        }

        private Point? mouse_first_snap = null;

        private void OnSetScrollerOffestFromContent(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Run)
                return;

            this.mouse_first_snap = e.GetPosition(this.homework_scroller);
        }

        private void UpdateSelected()
        {
            this.Loader.Show();
            CVDay.SelectedDay!.Update(require_new_call: true);
        }

        private void OnSnapScrollerFromContent(object sender, MouseButtonEventArgs e)
        {
            if (this.days_wp.Children.Count == 0 || this.mouse_first_snap is null)
                return;

            var pos = e.GetPosition(this.homework_scroller);
            if (this.mouse_first_snap.Value.Y <= this.homework_scroller.ActualHeight / 30 && pos.Y - this.mouse_first_snap.Value.Y >= this.homework_scroller.ActualHeight / 2)
            {
                this.UpdateSelected();
                return;
            }

            var required = this.head_wp.ActualWidth / 3;

            var idx = CVDay.SelectedDay!.ParentIdx;

            if (this.mouse_first_snap.Value.X - pos.X > required)
            {
                if (++idx == 7) idx = 0;
                CVDay.SelectedDay.Parent.SelectChild(idx);
            }
            else if (pos.X - this.mouse_first_snap.Value.X > required)
            {
                if (--idx == -1) idx = 6;
                CVDay.SelectedDay.Parent.SelectChild(idx);
            }

            this.mouse_first_snap = null;
        }

        private void OnUpdateContent(object sender, MouseButtonEventArgs e)
        {
            if (CVDay.SelectedDay is not null)
                CVDay.SelectedDay.Update(require_new_call: true);
        }

        public void OnSwitch()
        {
        }

        private void OnSnapScroller(object sender, SnapEventArgs e)
        {
            ((CVWeek)e.SnappendElement).Scroll();
        }

        public void OnFullReload()
        {
            CVDay.DestroyCache();
            this.UpdateSelected();
        }
    }
}
