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
using ClasseVivaWPF.Api.Types;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsence.xaml
    /// </summary>
    public partial class CVAbsence : UserControl
    {
        private CVAbsence()
        {
            InitializeComponent();
        }

        public CVAbsence(Event absence)
        {
            InitializeComponent();
            this.DataContext = absence;
        }
    }
}
