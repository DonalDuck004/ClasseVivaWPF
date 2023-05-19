using ClasseVivaWPF.Utils.Themes;
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

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVReload.xaml
    /// </summary>
    public partial class CVSpinner : UserControl
    {
        public CVSpinner()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void Show()
        {
            this.Loader.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Loader.Visibility = Visibility.Hidden;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
