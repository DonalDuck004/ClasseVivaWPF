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
    public partial class CVRegistryAbsence : UserControl
    {
        public static readonly DependencyProperty CountProperty;
        public static readonly DependencyProperty DescProperty;
        public static readonly DependencyProperty ExtraDescProperty;
        public static readonly DependencyProperty RectColorProperty;

        public required string ExtraDesc
        {
            get => (string)GetValue(ExtraDescProperty);
            set => SetValue(ExtraDescProperty, value);
        }

        public required string Desc
        {
            get => (string)GetValue(DescProperty);
            set => SetValue(DescProperty, value);
        }

        public required Color RectColor
        {
            get => (Color)GetValue(RectColorProperty);
            set => SetValue(RectColorProperty, value);
        }

        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        static CVRegistryAbsence()
        {
            DescProperty = DependencyProperty.Register("Desc", typeof(string), typeof(CVRegistryAbsence));
            ExtraDescProperty = DependencyProperty.Register("ExtraDesc", typeof(string), typeof(CVRegistryAbsence));
            CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(CVRegistryAbsence), new PropertyMetadata(0));
            RectColorProperty = DependencyProperty.Register("RectColor", typeof(Color), typeof(CVRegistryAbsence));
        }

        public CVRegistryAbsence()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
