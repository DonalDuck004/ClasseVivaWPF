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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Grades
{
    /// <summary>
    /// Logica di interazione per CVGrade.xaml
    /// </summary>
    public partial class CVGrade : UserControl
    {
        public Grade Grade { get; init; }

#if DEBUG
        public CVGrade()
        {
            InitializeComponent();
        }
#endif

        public CVGrade(Grade grade)
        {
            InitializeComponent();
            this.DataContext = this.Grade = grade;
        }
    }
}
