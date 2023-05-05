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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic.Homeworks
{
    /// <summary>
    /// Logica di interazione per CVHomeworkPreview.xaml
    /// </summary>
    public partial class CVHomeworkPreview : UserControl
    {
        public Homework Homework { get; private set; }
        private int src_idx;

        private CVHomeworkPreview()
        {
            InitializeComponent();
        }

        public CVHomeworkPreview(Homework homework, int src_idx)
        {
            InitializeComponent();

            this.DataContext = homework;
            this.src_idx = src_idx;
        }

        private void OnOpenHomework(object sender, MouseButtonEventArgs e)
        {
            new CVHomework(this.Homework, this.src_idx).Inject();
        }
    }
}
