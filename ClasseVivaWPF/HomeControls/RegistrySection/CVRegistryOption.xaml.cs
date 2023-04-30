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

namespace ClasseVivaWPF.HomeControls.RegistrySection
{
    /// <summary>
    /// Logica di interazione per CVRegistryOption.xaml
    /// </summary>
    public partial class CVRegistryOption : UserControl
    {
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty IconProperty;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ControlTemplate Icon
        {
            get => (ControlTemplate)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        static CVRegistryOption()
        {
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CVRegistryOption));
            IconProperty = DependencyProperty.Register("Icon", typeof(ControlTemplate), typeof(CVRegistryOption));
        }

        public CVRegistryOption()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
