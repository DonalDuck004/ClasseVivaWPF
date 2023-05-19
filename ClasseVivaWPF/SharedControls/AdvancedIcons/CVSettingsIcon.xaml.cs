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

namespace ClasseVivaWPF.SharedControls.AdvancedIcons
{
    /// <summary>
    /// Logica di interazione per CVSettingsIcon.xaml
    /// </summary>
    public partial class CVSettingsIcon : Viewbox
    {
        public static readonly DependencyProperty BordersColorProperty;
        public static readonly DependencyProperty CenterColorProperty;
        public static readonly DependencyProperty ColorProperty;

        public SolidColorBrush BordersColor
        {
            get => (SolidColorBrush)GetValue(BordersColorProperty);
            set => SetValue(BordersColorProperty, value);
        }
        public SolidColorBrush CenterColor
        {
            get => (SolidColorBrush)GetValue(CenterColorProperty);
            set => SetValue(CenterColorProperty, value);
        }
        public SolidColorBrush Color
        {
            get => (SolidColorBrush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        static CVSettingsIcon()
        {
            BordersColorProperty = DependencyProperty.Register("BordersColor", typeof(SolidColorBrush), typeof(CVSettingsIcon));
            CenterColorProperty = DependencyProperty.Register("CenterColor", typeof(SolidColorBrush), typeof(CVSettingsIcon));
            ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(CVSettingsIcon));
        }

        public CVSettingsIcon()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
