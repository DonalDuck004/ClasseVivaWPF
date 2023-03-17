using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVMessageBox.xaml
    /// </summary>
    public partial class CVMessageBox : UserControl
    {
        private static DependencyProperty TitleProperty;
        private static DependencyProperty DescriptionProperty;
        static CVMessageBox()
        {
            TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CVComboBoxItem));
            DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(CVComboBoxItem));
        }


        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        public string Description
        {
            get => (string)base.GetValue(DescriptionProperty);
            set => base.SetValue(DescriptionProperty, value);
        }

        public CVMessageBox(string title, string description)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Title = title;
            this.Description = description;
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.INSTANCE.RemoveField(this);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Description = "Todo";
        }
    }
}
