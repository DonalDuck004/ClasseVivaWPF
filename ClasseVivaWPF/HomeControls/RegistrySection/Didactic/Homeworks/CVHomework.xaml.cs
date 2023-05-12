using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic.Homeworks
{
    /// <summary>
    /// Logica di interazione per CVHomework.xaml
    /// </summary>
    public partial class CVHomework : Injectable
    {
        private CVHomework()
        {
            InitializeComponent();
        }

        public CVHomework(Homework homework, int src_idx) // TODO Reload
        {
            InitializeComponent();

            this.DataContext = homework;
        }

        private void OnClose(object sender, MouseButtonEventArgs e) => Close();
    }
}
