using ClasseVivaWPF.HomeControls.RegistrySection.Grades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    /// <summary>
    /// Logica di interazione per CVFolder.xaml
    /// </summary>
    public partial class CVFolder : UserControl
    {
        public static readonly DependencyProperty ExpandedProperty;

        public bool Expanded
        {
            get => (bool)GetValue(ExpandedProperty);
            set => SetValue(ExpandedProperty, value);
        }

        static CVFolder()
        {
            ExpandedProperty = DependencyProperty.Register("Expanded", typeof(bool), typeof(CVFolder), new PropertyMetadata(false));
        }

        public CVFolder()
        {
            InitializeComponent();
        }

        private void OnExpand(object sender, MouseButtonEventArgs e)
        {
            this.Expanded = !this.Expanded;
        }
    }
}
