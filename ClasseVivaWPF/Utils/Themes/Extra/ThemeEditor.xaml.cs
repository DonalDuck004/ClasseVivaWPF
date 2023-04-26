using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClasseVivaWPF.SharedControls;

namespace ClasseVivaWPF.Utils.Themes.Extra
{
    /// <summary>
    /// Logica di interazione per ThemeEditor.xaml
    /// </summary>
    public partial class ThemeEditor : Window
    {
        public static readonly DependencyProperty SelectedThemeProperty;

        private static SemaphoreSlim ShowLock = new SemaphoreSlim(1, 1);
        public static ThemeEditor? INSTANCE { get; private set; }
        private BaseTheme Current;
        public BaseTheme CurrentCopy;

        public BaseTheme SelectedTheme
        {
            get => (BaseTheme)GetValue(SelectedThemeProperty);
            set => SetValue(SelectedThemeProperty, value);
        }

        static ThemeEditor()
        {
            SelectedThemeProperty = DependencyProperty.Register("SelectedTheme", typeof(BaseTheme), typeof(ThemeEditor));
        }

        private ThemeEditor()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public static async Task<bool> New()
        {
            if (ShowLock.CurrentCount == 0)
            {
                INSTANCE!.Activate();
                return false;
            }

            await ShowLock.WaitAsync();
            INSTANCE = new ThemeEditor();
            INSTANCE.Closed += OnClose!;
            INSTANCE.SelectedTheme = INSTANCE.Current = MainWindow.INSTANCE.CurrentTheme;
            INSTANCE.CurrentCopy = (BaseTheme)MainWindow.INSTANCE.CurrentTheme.Clone();


            foreach (var item in ThemeProperties.GetProperties())
                INSTANCE.ThemePropertiesWP.Children.Add(new ThemePropertyViewer(item));

            INSTANCE.Show();
            return true;
        }

        public static void OnClose(object sender, EventArgs e)
        {
            if (ReferenceEquals(sender, INSTANCE))
                INSTANCE = null;

            ShowLock.Release();
        }

        private void CheckStateChanged(CVCheckBox sender, CheckedStateChangedEventArgs e)
        {
            if (sender.IsChecked)
                this.SelectedTheme = MainWindow.INSTANCE.CurrentTheme = this.Current;
            else
                this.SelectedTheme = MainWindow.INSTANCE.CurrentTheme = this.CurrentCopy;
        }
    }
}
