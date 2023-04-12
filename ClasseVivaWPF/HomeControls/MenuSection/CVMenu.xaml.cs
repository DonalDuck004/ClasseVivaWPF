using ClasseVivaWPF.Api;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVMenu.xaml
    /// </summary>
    public partial class CVMenu : UserControl, IOnSwitch {
        public static readonly DependencyProperty TitlesColorProperty;
        public static readonly DependencyProperty HRColorProperty;

        public SolidColorBrush TitlesColor
        {
            get => (SolidColorBrush)GetValue(TitlesColorProperty);
            set => SetValue(TitlesColorProperty, value);
        }

        public SolidColorBrush HRColor
        {
            get => (SolidColorBrush)GetValue(HRColorProperty);
            set => SetValue(HRColorProperty, value);
        }

        static CVMenu()
        {
            TitlesColorProperty = DependencyProperty.Register("TitlesColor", typeof(SolidColorBrush), typeof(CVMenu));
            HRColorProperty = DependencyProperty.Register("HRColor", typeof(SolidColorBrush), typeof(CVMenu));
        }

        private SemaphoreSlim PreventOverlap { get; } = new SemaphoreSlim(1, 1);
        private bool CanPress => PreventOverlap.CurrentCount == 1;

        public CVMenu()
        {
            this.SetThemeBinding(CVMenu.BackgroundProperty, BaseTheme.CV_GENERIC_BACKGROUND_PATH);
            this.SetThemeBinding(CVMenu.TitlesColorProperty, BaseTheme.CV_SETTINGS_SECTION_HEADER_PATH);
            this.SetThemeBinding(CVMenu.HRColorProperty, BaseTheme.CV_HR_PATH);
            this.DataContext = this;
            InitializeComponent();
        }

        private async void OpenWebviewer(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();
            var t = new CVRestBridgeViewer() { Uri = await Client.INSTANCE.GetUriFromTicket((string)((FrameworkElement)sender).Tag) };
            t.Inject();

            PreventOverlap.Release();
        }

        private async void OpenWebviewerBw(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();
            (await Client.INSTANCE.GetUriFromTicket((string)((FrameworkElement)sender).Tag)).SystemOpening();
            PreventOverlap.Release();

        }

        private async void BeforeYear(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();
            var t = new CVRestBridgeViewer() { Uri = await Client.INSTANCE.GetUriFromTicket("https://web.spaggiari.eu/") };
            t.WebView.ContentLoading += (s, e) =>
            {
                t.Uri = new("https://web.spaggiari.eu/home/app/default/xasapi.php?a=lap&bu=https://web21.spaggiari.eu&ru=/home/&fu=xasapi-ERROR.php");
            };
            t.Inject();

            PreventOverlap.Release();
        }

        private void OpenExtra(object sender, MouseButtonEventArgs e)
        {
            new CVExtra().Inject();
        }

        private void OnSendMail(object sender, MouseButtonEventArgs e)
        {
            
        }

        public void OnSwitch()
        {

        }

        private void OpenSettings(object sender, MouseButtonEventArgs e)
        {
            new CVSettings().Inject();
        }
    }
}
