using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.HomeSection;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsemcesCalendatDay.xaml
    /// </summary>
    public partial class CVAbsencesCalendarDay : UserControl
    {
        public CVAbsencesCalendarDay(DayStatus day)
        {
            this.DataContext = day;
            InitializeComponent();
        }
    }
}
