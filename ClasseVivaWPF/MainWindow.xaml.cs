using ClasseVivaWPF.Api;
using ClasseVivaWPF.Sessions;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Input;
using ClasseVivaWPF.Utils;

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

        public void RemoveField(UIElement element) => RemoveField((FrameworkElement)element);


        public void RemoveField(FrameworkElement element)
        {
            this.wrapper.Children.Remove(element);
        }


        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SessionHandler.TryInit())
                CVLoginPage.EndLogin();
            else
                this.ReplaceMainContent(new CVLoginPage());
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape && this.wrapper.Children.Count > 1)
            {
                var child = this.wrapper.Children[this.wrapper.Children.Count - 1];

                if (child is IOnEscKey sub_win)
                    sub_win.OnEscKey();
                else
                    this.RemoveField(child);
            }
        }
    }
}
