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
using System.IO;
using System.Text.RegularExpressions;
using ClasseVivaWPF.Themes.Abs;
using ClasseVivaWPF.Themes.Handling;
using ClasseVivaWPF.Utils;
using System.Resources;

namespace ClasseVivaWPF.Themes.Xaml
{
    public enum ThemeEditorMountModes
    {
        Window,
        InternalWindow
    }

    /// <summary>
    /// Logica di interazione per ThemeEditor.xaml
    /// </summary>
    public partial class ThemeEditor : Canvas
    {
        public static readonly DependencyProperty NewThemeNameProperty;
        public static readonly DependencyProperty SelectedThemeProperty;
        public static readonly DependencyProperty LeftExapandedProperty;
        public static readonly DependencyProperty SelectedViewerProperty;

        public bool EditedFlag = false;
        public bool AllowOverwrite = false;
        private static SemaphoreSlim ShowLock = new SemaphoreSlim(1, 1);
        public static ThemeEditor? INSTANCE { get; private set; }

        public ThemePropertyViewer SelectedViewer
        {
            get => (ThemePropertyViewer)GetValue(SelectedViewerProperty);
            set => SetValue(SelectedViewerProperty, value);
        }

        public BaseTheme SelectedTheme
        {
            get => (BaseTheme)GetValue(SelectedThemeProperty);
            set => SetValue(SelectedThemeProperty, value);
        }

        public string NewThemeName
        {
            get => (string)GetValue(NewThemeNameProperty);
            set => SetValue(NewThemeNameProperty, value);
        }

        public bool LeftExapanded
        {
            get => (bool)GetValue(LeftExapandedProperty);
            set => SetValue(LeftExapandedProperty, value);
        }

        static ThemeEditor()
        {
            SelectedThemeProperty = DependencyProperty.Register("SelectedTheme", typeof(BaseTheme), typeof(ThemeEditor));
            LeftExapandedProperty = DependencyProperty.Register("LeftExapanded", typeof(bool), typeof(ThemeEditor), new PropertyMetadata(false));
            NewThemeNameProperty = DependencyProperty.Register("NewThemeName", typeof(string), typeof(ThemeEditor));
            SelectedViewerProperty = DependencyProperty.Register("SelectedViewer", typeof(ThemePropertyViewer), typeof(ThemeEditor));
        }
        
        private ThemeEditor()
        {
            InitializeComponent();

            this.mobile_wp.DataContext = this;
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

            ((ThemePropertyViewer)INSTANCE.ThemePropertiesWP.Children[0]).Selected = true;

            var files = Directory.GetFiles(Config.THEMES_DIR_PATH, "*.theme.json");

            var last_generic = (from file in files
                                select new Regex(@"^Tema(\d+)\.theme\.json$", RegexOptions.None).Match(System.IO.Path.GetFileName(file)) into q
                                where q.Success select int.Parse(q.Groups[1].Value) into x
                                orderby x select x).LastOrDefault();

            INSTANCE.NewThemeName = $"Tema{last_generic + 1}";

            INSTANCE.SetMountMode(ThemeEditorMountModes.Window);
            return true;
        }

        private void Activate()
        {
            if (this.Parent is Window x)
                x.Activate();
            else if (this.Parent is InternalWindow y)
                y.Collapsed = false;
        }

        private void SetMountMode(ThemeEditorMountModes mode)
        {
            if (this.Parent is Window x)
            {
                x.Closed -= OnWinClose;
                x.Closing -= OnWinClosing;
                x.Content = null;
                x.Close();
            }
            else if (this.Parent is InternalWindow y)
            {
                y.Closed -= OnIntWinClose;
                y.Closing -= OnIntWinClosing;
                y.ContentItem = null;
                y.Unmount();
            }

            if (mode is ThemeEditorMountModes.InternalWindow)
            {
                var in_win = new InternalWindow()
                {
                    Title = "Theme Editor",
                    ContentItem = this,
                    MountOnMainWindow = true,
                };
                in_win.Closed += OnIntWinClose;
                in_win.Closing += OnIntWinClosing;
                in_win.AddCustomAction((FrameworkElement)this.Resources["ExtraIntWinIcon"]);
                in_win.ManualMount();
            }else
            {
                var win = new Window()
                {
                    Title = "Theme Editor",
                    Content = this,
                    ResizeMode = ResizeMode.CanMinimize,
                    Height = 450,
                    Width = 800
                };

                win.Closed += OnWinClose;
                win.Closing += OnWinClosing; // Todo for InternalWindow
                win.Show();
            }
        }

        private void OnWinClose(object sender, EventArgs e) => OnClose();
        private void OnIntWinClose(InternalWindow sender) => OnClose();
        private void OnWinClosing(object sender, CancelEventArgs e) => e.Cancel = OnClosing();
        private void OnIntWinClosing(object sender, IntWinClosingEventArgs e) => e.Cancel = OnClosing();

        private void OnClose()
        {
            ThemeProperties.EndThemeEditing();
            ThemePropertyViewer.SelectedInstance = null;
            ShowLock.Release();
        }

        private bool OnClosing()
        {
            if (this.EditedFlag)
            {
                var confirm = MessageBox.Show("Non hai salvato il tema modificato, vuoi continuare?", "Tema non salvato", MessageBoxButton.OKCancel);

                if (confirm is MessageBoxResult.Cancel)
                {
                    this.LeftExapanded = true;
                    return true;
                }
            }
            else if (ThemeOperations.ThemeFileExists(this.NewThemeName))
            {
                if (ThemeOperations.TryGetFromFile(this.NewThemeName, out ITheme? theme))
                {
                    ThemeOperations.Register(ThemeInitializer.NewFromFile(this.NewThemeName));
                    MainWindow.INSTANCE.CurrentTheme = theme!;
                }
                else
                    MessageBox.Show("Tema non salvato correttamente", "Errore", MessageBoxButton.OK);
            }

            return false;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var filename = this.NewThemeName + ".theme.json";
            try
            {
                System.IO.Path.GetFullPath(filename);
            }catch (ArgumentException)
            {
                MessageBox.Show("Nome scelto per il tema non valido!", "Errore", MessageBoxButton.OK);
                return;
            }

            var initializer = ThemeOperations.GetCreator(this.NewThemeName);
            if (initializer is not null)
            {
                if (initializer.FromFile && !this.AllowOverwrite)
                {
                    var r = MessageBox.Show("Errore esiste già un tema con questo nome!\nSovrascrivere?", "Errore", MessageBoxButton.OKCancel);
                    if (r is MessageBoxResult.Cancel)
                        return;

                    this.AllowOverwrite = true;
                }
                else if (initializer.FromFile is false)
                {
                    MessageBox.Show("Errore esiste già un tema con questo nome!", "Errore", MessageBoxButton.OK);
                    return;
                }
            }

            var js = ThemeProperties.FreezeAsJson();
            var file = System.IO.Path.Join(Config.THEMES_DIR_PATH, filename);

            try
            {
                File.WriteAllText(file, js);
            }
            catch
            {
                MessageBox.Show("Errore imprevisto non è stato possibile salvare il file!", "Errore", MessageBoxButton.OK);
                return;
            }

            this.NewThemeInput.IsReadOnly = true;

            this.EditedFlag = false;
        }

        private void OnExpand(object sender, MouseButtonEventArgs e)
        {
            this.LeftExapanded = !this.LeftExapanded;
        }

        private void OnOpenWindow(object sender, MouseButtonEventArgs e)
        {

        }

        /*private void OnPicker(object sender, RoutedEventArgs e)
        {
            MainWindow.INSTANCE.ForceCursor = true;
            MainWindow.INSTANCE.Cursor = Cursors.Cross;
            MainWindow.INSTANCE.Activate();
            MainWindow.INSTANCE.PreviewMouseLeftButtonDown += OnPicked;
        }

        private void OnPicked(object sender, RoutedEventArgs e)
        {
            MainWindow.INSTANCE.ForceCursor = false;
            MainWindow.INSTANCE.Cursor = Cursors.Arrow;

            var x = BindingOperations.GetBinding((DependencyObject)e.Source, Panel.BackgroundProperty);

            e.Handled = true;
            MainWindow.INSTANCE.PreviewMouseLeftButtonDown -= OnPicked;
        }*/
    }
}
