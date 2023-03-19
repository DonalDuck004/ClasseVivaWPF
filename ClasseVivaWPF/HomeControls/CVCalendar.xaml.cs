using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVCalendar.xaml
    /// </summary>
    public partial class CVCalendar : UserControl, IOnEscKey
    {
        public CVCalendar()
        {
            InitializeComponent();
            this.DataContext = this;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var date = this.calendar.SelectedDate!.Value;
            var week = CVDay.SelectedDay!.Parent;

            if (date < week.From){
                while (date < week.From)
                {
                    if (week.Previus is null)
                        CVHome.INSTANCE.AddWeek(week.Previus = new(date, next: week), 0);
                    week = week.Previus;
                }
            } else if (date > week.To)
            {
                while (date > week.To)
                {
                    if (week.Next is null)
                        CVHome.INSTANCE.AddWeek(week.Next = new(date, previus: week));
                    week = week.Next;
                }
            }

            week.SelectChild(date.DayOfWeek);
            this.Close();
        }

        public void OnEscKey() => Close();

        public void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }
    }
}
