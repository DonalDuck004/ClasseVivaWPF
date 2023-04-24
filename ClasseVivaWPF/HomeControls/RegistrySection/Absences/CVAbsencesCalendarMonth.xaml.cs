using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsencesCalendarMonth.xaml
    /// </summary>
    public partial class CVAbsencesCalendarMonth : UserControl
    {
        private SemaphoreSlim loader_check;
        private DayStatus[] days;
        public DateTime Date { get; private set; }

        private CVAbsencesCalendarMonth()
        {
            InitializeComponent();
        }

        public CVAbsencesCalendarMonth(DayStatus[] days, SemaphoreSlim loader_check)
        {
            InitializeComponent();
            this.days = days;
            this.Date = days[0].Date;
            this.loader_check = loader_check;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;
            int row_idx = 1;
            CVAbsencesCalendarDay tmp;

            foreach (var day in this.days)
            {
                this.Grid.Children.Add(tmp = new(day));
                Grid.SetColumn(tmp, day.Date.DayOfWeek.AsInt32());
                Grid.SetRow(tmp, row_idx);


                if (day.Date.DayOfWeek is DayOfWeek.Sunday)
                    row_idx++;
            }

            this.loader_check.Release();
        }
    }
}
