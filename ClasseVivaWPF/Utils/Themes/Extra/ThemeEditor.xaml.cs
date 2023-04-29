using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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
using Newtonsoft.Json;

namespace ClasseVivaWPF.Utils.Themes.Extra
{
    /// <summary>
    /// Logica di interazione per ThemeEditor.xaml
    /// </summary>
    public partial class ThemeEditor : Window
    {
        public static readonly DependencyProperty SelectedThemeProperty;

        public bool EditedFlag = false;
        private bool ApplyCurrent = false;
        private static SemaphoreSlim ShowLock = new SemaphoreSlim(1, 1);
        public static ThemeEditor? INSTANCE { get; private set; }

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

            ThemeProperties.BeginThemeEditing();

            foreach (var item in ThemeProperties.GetProperties())
                INSTANCE.ThemePropertiesWP.Children.Add(new ThemePropertyViewer(item));

            INSTANCE.Show();
            return true;
        }

        public void OnClose(object sender, EventArgs e)
        {
            ThemeProperties.EndThemeEditing(this.ApplyCurrent);
            ShowLock.Release();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (EditedFlag)
            {
                var confirm = MessageBox.Show("Non hai salvato il tema modificato, vuoi continuare?", "Tema non salvato", MessageBoxButton.OKCancel);

                if (confirm is MessageBoxResult.Cancel)
                {
                    this.ApplyCurrent = false;
                    e.Cancel = true;
                    return;
                }
                else
                    this.ApplyCurrent = true;

            }else
                this.ApplyCurrent = false;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var js = ThemeProperties.FreezeAsJson();
        }
    }
}
