using ClasseVivaWPF.Sessions;
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

namespace ClasseVivaWPF.HomeControls
{
    /// <summary>
    /// Logica di interazione per CVAccountLogout.xaml
    /// </summary>
    public partial class CVAccountLogout : UserControl
    {
        public AccountMeta Account { get; init; }
        public event EventHandler? OnLogout;

        public CVAccountLogout()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void CVButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnLogout is not null)
                OnLogout(this, EventArgs.Empty);
        }
    }
}
