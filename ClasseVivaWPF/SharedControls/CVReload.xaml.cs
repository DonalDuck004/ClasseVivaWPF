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
    public partial class CVReload : UserControl
    {
        public static readonly DependencyProperty EllipseColorBrushProperty;
        public static readonly DependencyProperty SpinnerColorBrushProperty;

        public SolidColorBrush EllipseColorBrush
        {
            get => (SolidColorBrush)GetValue(EllipseColorBrushProperty);
            set => SetValue(EllipseColorBrushProperty, value);
        }
        public SolidColorBrush SpinnerColorBrush
        {
            get => (SolidColorBrush)GetValue(SpinnerColorBrushProperty);
            set => SetValue(SpinnerColorBrushProperty, value);
        }

        static CVReload()
        {
            EllipseColorBrushProperty = DependencyProperty.Register("EllipseColorBrush", typeof(SolidColorBrush), typeof(CVReload), new PropertyMetadata(new SolidColorBrush(Colors.White)));
            SpinnerColorBrushProperty = DependencyProperty.Register("SpinnerColorBrush", typeof(SolidColorBrush), typeof(CVReload), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));
        }

        public CVReload()
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
