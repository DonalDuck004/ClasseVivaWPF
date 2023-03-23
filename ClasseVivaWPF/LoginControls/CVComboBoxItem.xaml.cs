using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF.LoginControls
{
    /// <summary>
    /// Logica di interazione per CVComboBoxItem.xaml
    /// </summary>
    public partial class CVComboBoxItem : UserControl
    {
        private static DependencyProperty LabelContentProperty;

        static CVComboBoxItem()
        {
            LabelContentProperty = DependencyProperty.Register("LabelContent", typeof(string), typeof(CVComboBoxItem));
        }

        public CVComboBoxItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        public string LabelContent
        {
            get => (string)base.GetValue(LabelContentProperty);
            set => base.SetValue(LabelContentProperty, value);
        }
    }
}
