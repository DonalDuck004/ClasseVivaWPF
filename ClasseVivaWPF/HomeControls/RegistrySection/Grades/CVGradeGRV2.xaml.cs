using ClasseVivaWPF.Api.Types;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Grades
{
    /// <summary>
    /// Logica di interazione per CVGradeGRV2.xaml
    /// </summary>
    public partial class CVGradeGRV2 : CVGradeBase
    {
        public CVGradeGRV2(Grade grade) : base(grade)
        {
            InitializeComponent();
        }
    }
}
