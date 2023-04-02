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
    /// Logica di interazione per CVCheckBox.xaml
    /// </summary>
    public partial class CVCheckBox : UserControl
    {
        public static DependencyProperty IsCheckedProperty;
        public event EventHandler? CheckStateChanged = null;

        static CVCheckBox()
        {
            IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(CVCheckBox), new PropertyMetadata(false));
        }

        public CVCheckBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public required bool IsChecked
        {
            get => (bool)base.GetValue(IsCheckedProperty);
            set {
                base.SetValue(IsCheckedProperty, value);
                this.OnCheckStateChanged(EventArgs.Empty);
            }
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            this.IsChecked = !this.IsChecked;
        }

        protected virtual void OnCheckStateChanged(EventArgs e)
        {
            if (this.CheckStateChanged is not null)
                this.CheckStateChanged(this, e);
        }

        private void OnFirstLoad(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(this.Point, 64);
            this.Loaded -= OnFirstLoad;
        }
    }
}
