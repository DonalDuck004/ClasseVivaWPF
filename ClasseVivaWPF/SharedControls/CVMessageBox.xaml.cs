using ClasseVivaWPF.Utils;
using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVMessageBox.xaml
    /// </summary>
    public partial class CVMessageBox : Injectable
    {
        private static DependencyProperty TitleProperty;
        private static DependencyProperty DescriptionProperty;

        static CVMessageBox()
        {
            TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CVMessageBox));
            DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(CVMessageBox));
        }

        public CVMessageBox(string title, string description) : base()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Title = title;
            this.Description = description;
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


        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Description = "Todo";
        }
    }
}
