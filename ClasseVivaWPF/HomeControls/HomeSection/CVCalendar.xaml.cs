using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    /// <summary>
    /// Logica di interazione per CVCalendar.xaml
    /// </summary>
    public partial class CVCalendar : Injectable
    {
        public CVCalendar() : base()
        {
            this.DataContext = this;
            InitializeComponent();
            SetToday();
        }

        private void SetToday()
        {
            this.calendar.SelectedDate = this.calendar.DisplayDate = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => SetToday();

        private void Button_Click_1(object sender, RoutedEventArgs e) => Close();

        private void PART_YearView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var childs = ((Grid)sender).Children;
            for (int i = 0; i < childs.Count; i++)
                Grid.SetRow(childs[i], i);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            var date = this.calendar.SelectedDate!.Value.Date;

            var week = await CVWeek.New(date);
            if (week.Parent is null)
                CVHome.INSTANCE.AddWeek(week);

            Debug.Assert(!week.Destroyed);
            week.SelectChild(date.DayOfWeek);
            this.Close();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this.wp);
            if (pos.X < 0 || pos.Y < 0)
                this.Close();
        }
    }
}
