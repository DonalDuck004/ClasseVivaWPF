using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVTextBox.xaml
    /// </summary>
    public partial class CVTextBox : UserControl
    {

        public static readonly DependencyProperty PlaceHolderProperty;
        private bool warn;

        static CVTextBox()
        {
            PlaceHolderProperty = DependencyProperty.Register("PlaceHolder", typeof(string), typeof(CVTextBox));
        }

        public CVTextBox()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public void HideContent()
        {
            var asm = Assembly.GetExecutingAssembly().GetName().Name!;
            this.Input.FontFamily = new FontFamily(new Uri($"pack://application:,,,/{asm};component/Assets/Fonts/"), "./#password");
        }

        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }
        public bool Warn
        {
            get => warn;
            set
            {
                warn = value;
                this.warn_symbol.Visibility = this.warn_box.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                this.warn_box.BeginStoryboard(this.FindResource("show_popup") as Storyboard);
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Warn = false;
        }

        public bool IsEmpty() => this.Text == "";
        public string Text { get => this.Input.Text; set => this.Input.Text = value; }
    }
}
