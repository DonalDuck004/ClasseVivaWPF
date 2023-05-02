using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using ClasseVivaWPF.SharedControls;
using System.Windows.Media;
using ClasseVivaWPF.Utils.Themes;
using ClasseVivaWPF.Utils.Themes.Extra;
using ClasseVivaWPF.Utils.Interfaces;

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVSettings.xaml
    /// </summary>
    public partial class CVSettings : Injectable, IOnCloseRequested, ICVMeta
    {
        public bool CountsInStack { get; } = false;

        public static string MediaDirSize => new DirectoryInfo(Config.MEDIA_DIR_PATH).GetFiles().Sum(x => x.Length).SizeSuffix();
        public static string LogsDirSize => new DirectoryInfo(Config.LOGS_DIR_PATH).GetFiles().Sum(x => x.Length).SizeSuffix();
        public static string ThemesDirSize => new DirectoryInfo(Config.THEMES_DIR_PATH).GetFiles().Sum(x => x.Length).SizeSuffix();
        public static string? SessionCacheSize => SessionHandler.INSTANCE?.GetCacheSize().SizeSuffix();
        public static string? DBSize => SessionHandler.INSTANCE is null ? null : new FileInfo(SessionHandler.INSTANCE?.FileName!).Length.SizeSuffix();
        private FileSystemWatcher MediaDirChanged = new FileSystemWatcher(Config.MEDIA_DIR_PATH)
        {
            EnableRaisingEvents = true,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
        };


        private FileSystemWatcher LogsDirChanged = new FileSystemWatcher(Config.LOGS_DIR_PATH)
        {
            EnableRaisingEvents = true,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
        };

        private FileSystemWatcher ThemesDirChanged = new FileSystemWatcher(Config.THEMES_DIR_PATH)
        {
            EnableRaisingEvents = true,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
        };

        public CVSettings() : base()
        {
            InitializeComponent();

            this.DataContext = this;
            MediaDirChanged.Deleted += FSUpdate;
            MediaDirChanged.Created += FSUpdate;
            MediaDirChanged.Changed += FSUpdate;

            LogsDirChanged.Deleted += FSUpdate;
            LogsDirChanged.Created += FSUpdate;
            LogsDirChanged.Changed += FSUpdate;

            ThemesDirChanged.Deleted += FSUpdate;
            ThemesDirChanged.Created += FSUpdate;
            ThemesDirChanged.Changed += FSUpdate;

            Update();
            SessionHandler.INSTANCE!.NotificationsFlagChanged += OnNotificationsFlagChanged;
            this.NotificationsCB.CheckStateChanged += OnCheckStateChanged!;
            this.NotificationsRangeSlider.ValueChanged += OnNotifyRangeChanged!;
            this.PagesStackSlider.ValueChanged += OnPagesStackSizeChanged!;
        }

        private void OnNotificationsFlagChanged(SessionHandler sender, bool Flag)
        {
            this.NotificationsCB.IsChecked = Flag;
        }

        private void FSUpdate(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(Update);
        }

        private void Update()
        {
            this.ShowCacheSize.Text = CVSettings.SessionCacheSize;
            this.ShowDBSize.Text = CVSettings.DBSize;
            this.ShowMediaDirSize.Text = CVSettings.MediaDirSize;
            this.ShowLogsDirSize.Text = CVSettings.LogsDirSize;
            this.ShowThemesDirSize.Text = CVSettings.ThemesDirSize;

            this.NotificationsCB.IsChecked = NotificationSystem.INSTANCE.IsActive;
            this.NotificationsRangeSlider.Value = SessionHandler.INSTANCE!.GetNotificationsRange();
            this.PagesStackSlider.Value = SessionHandler.INSTANCE!.GetPagesStackSize();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        private void OpenRepo(object sender, MouseButtonEventArgs e)
        {
            new Uri(Config.REPO_URL).SystemOpening();
        }

        private void OpenMediaDir(object sender, RoutedEventArgs e)
        {
            new Uri(Config.MEDIA_DIR_PATH).SystemOpening();
        }

        private void OpenSessionDir(object sender, RoutedEventArgs e)
        {
            new Uri(Config.SESSIONS_DIR_PATH).SystemOpening();
        }

        private void OpenLogsDir(object sender, RoutedEventArgs e)
        {
            new Uri(Config.LOGS_DIR_PATH).SystemOpening();
        }

        private void OpenThemesDir(object sender, RoutedEventArgs e)
        {
            new Uri(Config.THEMES_DIR_PATH).SystemOpening();
        }

        private void DropSessionCache(object sender, RoutedEventArgs e)
        {
            SessionHandler.INSTANCE!.DropCache();
            this.Update();
        }

        public override void OnCloseRequested()
        {
            SessionHandler.INSTANCE!.NotificationsFlagChanged -= OnNotificationsFlagChanged;
            this.MediaDirChanged.Dispose();
            base.OnCloseRequested();
        }

        private void OnCheckStateChanged(object sender, EventArgs e)
        {
            SessionHandler.INSTANCE!.NotificationsFlagChanged -= OnNotificationsFlagChanged;
            SessionHandler.INSTANCE!.SetNotificationsFlag(this.NotificationsCB.IsChecked);
            SessionHandler.INSTANCE!.NotificationsFlagChanged += OnNotificationsFlagChanged;
        }

        private void OnNotifyRangeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SessionHandler.INSTANCE!.SetNotificationsRange((int)e.NewValue);
        }

        private void OnPagesStackSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SessionHandler.INSTANCE!.SetPagesStackSize((int)e.NewValue);
        }

        private async void OpenThemeEditor(object sender, RoutedEventArgs e)
        {
            if (!await ThemeEditor.New())
                new CVMessageBox("Errore!", "Editor temi già aperto").Inject();
        }

        private void OnPreviusTheme(object sender, RoutedEventArgs e)
        {
            var current = MainWindow.INSTANCE.CurrentTheme.Name;
            var theme = ThemeOperations.THEMES.TakeWhile(x => x.Name != current).LastOrDefault(ThemeOperations.THEMES.Last());

            SetTheme(theme);
        }

        private void OnNextTheme(object sender, RoutedEventArgs e)
        {
            var current = MainWindow.INSTANCE.CurrentTheme.Name;
            var theme = ThemeOperations.THEMES.SkipWhile(x => x.Name != current).ElementAtOrDefault(1) ?? ThemeOperations.THEMES.First();

            SetTheme(theme);
        }

        private void SetTheme(ThemeInitializer creator)
        {
            try
            {
                MainWindow.INSTANCE.CurrentTheme = ThemeOperations.Get(creator);
            }catch (FileNotFoundException) {
                ThemeOperations.Unregister(creator);
            }
        }
    }
}
