using ClasseVivaWPF.Api;
using ClasseVivaWPF.Sessions;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static MainWindow INSTANCE => (MainWindow)Application.Current.MainWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddFieldOverlap(FrameworkElement element)
        {
            this.wrapper.Children.Add(element);
        }

        public void ReplaceMainContent(FrameworkElement element)
        {
            if (this.wrapper.Children.Count != 0)
                this.wrapper.Children.RemoveAt(0);

            this.wrapper.Children.Insert(0, element);
            element.Focus();
        }

        public void RemoveField(FrameworkElement element)
        {
            this.wrapper.Children.Remove(element);
        }


        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SessionHandler.TryInit())
                LoginPage.EndLogin();
            else
                this.ReplaceMainContent(new LoginPage());
        }
    }


    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString((string)parameter);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }

    class ToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }

    }

}
