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

namespace ClasseVivaWPF.LoginControls
{
    /// <summary>
    /// Logica di interazione per CVLoginAccountOption.xaml
    /// </summary>
    public partial class CVLoginAccountOption : UserControl
    {
        // For vs
        public CVLoginAccountOption()
        {
            InitializeComponent();
        }

        public CVLoginAccountOption(LoginChoice Choice)
        {
            InitializeComponent();

            this.DataContext = Choice;
        }
    }
}
