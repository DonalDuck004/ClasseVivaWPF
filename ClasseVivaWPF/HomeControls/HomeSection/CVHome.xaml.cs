using ClasseVivaWPF.Api.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    /// <summary>
    /// Logica di interazione per CVHome.xaml
    /// </summary>
    public partial class CVHome : UserControl
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

            Application.Current.MainWindow.KeyDown += OnKeyDown;
            this.days_scroller.SizeChanged += (s, e) => {
                if (CVDay.SelectedDay is not null)
                    CVDay.SelectedDay.Parent.BringIntoView();
            };

            this.Loaded -= OnLoad;
        }

        public void UpdateDayLabel()
        {
            this.day_lbl.Text = CVDay.SelectedDay!.Date.ToString("dddd dd MMMM yyyy");
        }

        public void Init()
        {
            var item = new CVWeek(from: DateTime.Now.Date);
            var idx = this.days_wp.Children.Count - 1;
            this.days_wp.Children.Insert(idx == -1 ? 0 : idx, item);
        }

        private double? scroll_horizontal_offset = null;

        private void OnSnapScroller(object sender, MouseButtonEventArgs e)
        {
            if (this.days_wp.Children.Count == 0 || scroll_horizontal_offset is null)
                return;

            var required = this.head_wp.ActualWidth / 20;

            if (this.days_scroller.HorizontalOffset - scroll_horizontal_offset > required) // Next
            {
                var r = this.days_scroller.HorizontalOffset / this.head_wp.ActualWidth;
                ((CVWeek)this.days_wp.Children[(int)r + 1]).SelectChild(0);
            }
            else if (scroll_horizontal_offset - this.days_scroller.HorizontalOffset > required)// Undo
            {
                var r = this.days_scroller.HorizontalOffset / this.head_wp.ActualWidth;
                ((CVWeek)this.days_wp.Children[(int)r]).SelectChild(6);
            }
            else if (CVDay.SelectedDay is not null)
                CVDay.SelectedDay.Parent.BringIntoView();

            scroll_horizontal_offset = null;
        }

        private void OnSetScrollerOffest(object sender, MouseButtonEventArgs e)
        {
            scroll_horizontal_offset = this.days_scroller.HorizontalOffset;
        }

        public void AddWeek(CVWeek week, int? idx = null)
        {
            if (idx is null)
                this.days_wp.Children.Add(week);
            else
                this.days_wp.Children.Insert(idx.Value, week);

            CVWeek? to_delete;
            while (this.days_wp.Children.Count > 5)
            {
                if (idx is null)
                {
                    to_delete = (CVWeek)this.days_wp.Children[0];
                    this.days_wp.Children.RemoveAt(0);
                }
                else
                {
                    to_delete = (CVWeek)this.days_wp.Children[this.days_wp.Children.Count - 1];
                    this.days_wp.Children.RemoveAt(this.days_wp.Children.Count - 1);
                }
                to_delete.BeginDestroy();
            }

            to_delete = null;
            GC.Collect();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Left || e.Key is Key.Right)
                this.days_scroller.RaiseEvent(e);
            else if(e.Key is Key.Down || e.Key is Key.Up)
                this.homework_scroller.RaiseEvent(e);
        }

        private void DaysOnKeyDown(object sender, KeyEventArgs e)
        {
            if (CVDay.SelectedDay is null || (e.Key is not Key.Left && e.Key is not Key.Right))
            {
                e.Handled = false;
                return;
            }

            int idx;
            if (e.KeyboardDevice.Modifiers is ModifierKeys.Control)
            {
                idx = this.days_wp.Children.IndexOf(CVDay.SelectedDay.Parent);
                int d_idx;

                if (e.Key is Key.Left)
                {
                    idx--;
                    d_idx = 6;
                }
                else
                {
                    idx++;
                    d_idx = 0;
                }

                ((CVWeek)this.days_wp.Children[idx]).SelectChild(d_idx);
            }
            else
            {
                idx = CVDay.SelectedDay.ParentIdx + (e.Key is Key.Left ? -1 : 1);

                if (idx == 7)
                    idx = 0;
                else if(idx == -1)
                    idx = 6;

                CVDay.SelectedDay.Parent.SelectChild(idx);
            }

            e.Handled = true;
        }

        private void OnOpenCalendar(object sender, MouseButtonEventArgs e)
        {
            MainWindow.INSTANCE.AddFieldOverlap(new CVCalendar());
        }

        private Point? mouse_fist_snap = null;

        private void OnSetScrollerOffestFromContent(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Run)
                return;

            this.mouse_fist_snap = e.GetPosition(this.homework_scroller);
        }

        private void OnSnapScrollerFromContent(object sender, MouseButtonEventArgs e)
        {
            if (this.days_wp.Children.Count == 0 || this.mouse_fist_snap is null)
                return;

            Point pos = e.GetPosition(this.homework_scroller);
            if (this.mouse_fist_snap.Value.Y <= this.homework_scroller.ActualHeight / 30 && pos.Y - this.mouse_fist_snap.Value.Y >= this.homework_scroller.ActualHeight / 2)
            {
                this.Loader.Visibility = Visibility.Visible;
                CVDay.SelectedDay!.Update(require_new_call: true);
                return;
            }


            var required = this.head_wp.ActualWidth / 3;

            var idx = CVDay.SelectedDay!.ParentIdx;

            if (this.mouse_fist_snap.Value.X - pos.X > required)
            {
                if (++idx == 7) idx = 0;
                CVDay.SelectedDay.Parent.SelectChild(idx);
            }
            else if (pos.X - this.mouse_fist_snap.Value.X > required)
            {
                if (--idx == -1) idx = 6;
                CVDay.SelectedDay.Parent.SelectChild(idx);
            }

            this.mouse_fist_snap = null;
        }

        private void OnUpdateContent(object sender, MouseButtonEventArgs e)
        {
            if (CVDay.SelectedDay is not null)
                CVDay.SelectedDay.Update(require_new_call: true);
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Loader.Visibility = Visibility.Hidden;
        }
    }
}
