using ClasseVivaWPF.SharedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVPeriodPercentage.xaml
    /// </summary>
    public partial class CVPeriodPercentage : BaseCVPercentage
    {
        public CVPeriodPercentage() : base()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
