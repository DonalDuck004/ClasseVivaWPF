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

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVSettings.xaml
    /// </summary>
    public partial class CVSettings : UserControl, ICloseRequested, ICVMeta
    {
        public bool CountsInStack { get; } = false;
       
        public static readonly DependencyProperty TitlesColorProperty;
        public static readonly DependencyProperty ParagraphColorProperty;

        public SolidColorBrush TitlesColor
        {
            get => (SolidColorBrush)GetValue(TitlesColorProperty);
            set => SetValue(TitlesColorProperty, value);
        }

        public SolidColorBrush ParagraphColor
        {
            get => (SolidColorBrush)GetValue(ParagraphColorProperty);
            set => SetValue(ParagraphColorProperty, value);
        }

        static CVSettings()
        {
            TitlesColorProperty = DependencyProperty.Register("TitlesColor", typeof(SolidColorBrush), typeof(CVSettings));
            ParagraphColorProperty = DependencyProperty.Register("ParagraphColor", typeof(SolidColorBrush), typeof(CVSettings));
        }

        public static string MediaDirSize => new DirectoryInfo(Config.MEDIA_DIR_PATH).GetFiles().Sum(x => x.Length).SizeSuffix();
        public static string? SessionCacheSize => SessionHandler.INSTANCE?.GetCacheSize().SizeSuffix();
        public static string? DBSize => SessionHandler.INSTANCE is null ? null : new FileInfo(SessionHandler.INSTANCE?.FileName!).Length.SizeSuffix();
        private FileSystemWatcher MediaDirChanged = new FileSystemWatcher(Config.MEDIA_DIR_PATH)
        {
            EnableRaisingEvents = true,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
        };

        public CVSettings()
        {
            InitializeComponent();
            
            this.Header.SetThemeBinding(DockPanel.BackgroundProperty, BaseTheme.CV_HEADER_PATH);
            this.BackIcon.SetThemeBinding(ContentControl.BackgroundProperty, BaseTheme.CV_BACK_ICON_PATH);
            this.Scroller.SetThemeBinding(ScrollViewer.BackgroundProperty, BaseTheme.CV_GENERIC_BACKGROUND_PATH);
            this.SetThemeBinding(CVSettings.TitlesColorProperty, BaseTheme.CV_SETTINGS_SECTION_HEADER_PATH);
            this.SetThemeBinding(CVSettings.ParagraphColorProperty, BaseTheme.CV_GENERIC_GRAY_FONT_PATH);
            this.DataContext = this;
            MediaDirChanged.Deleted += FSUpdate;
            MediaDirChanged.Created += FSUpdate;
            MediaDirChanged.Changed += FSUpdate;
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
            this.ShowCache.Text = CVSettings.SessionCacheSize;
            this.ShowDBSize.Text = CVSettings.DBSize;
            this.ShowMediaDirSize.Text = CVSettings.MediaDirSize;
            this.NotificationsCB.IsChecked = NotificationSystem.INSTANCE.IsActive;
            this.NotificationsRangeSlider.Value = SessionHandler.INSTANCE!.GetNotificationsRange();
            this.PagesStackSlider.Value = SessionHandler.INSTANCE!.GetPagesStackSize();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }

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

        private void DropSessionCache(object sender, RoutedEventArgs e)
        {
            SessionHandler.INSTANCE!.DropCache();
            this.Update();
        }

        public void OnCloseRequested()
        {
            SessionHandler.INSTANCE!.NotificationsFlagChanged -= OnNotificationsFlagChanged;
            this.MediaDirChanged.Dispose();
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
    }
}
