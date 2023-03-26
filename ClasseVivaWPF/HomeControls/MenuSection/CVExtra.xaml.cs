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

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVExtra.xaml
    /// </summary>
    public partial class CVExtra : UserControl
    {
        public CVExtra()
        {
            InitializeComponent();
        }

        public void Destroy()
        {
            CVExtraHeader.DestroyAll();
        }

        public void OnCloseRequested()
        {
            this.Destroy();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
