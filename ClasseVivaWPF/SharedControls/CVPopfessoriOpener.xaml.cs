using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVWebView.xaml
    /// </summary>
    public partial class CVPopfessoriOpener : CVExtraBase, IOnCloseRequested
    {
        private static DependencyProperty UriProperty;

        static CVPopfessoriOpener()
        {
            UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(CVPopfessoriOpener));
        }

#if DEBUG
        private CVPopfessoriOpener() : base() // For vs editor
        {
            InitializeComponent();
            this.DataContext = this;
        }
#endif

        public CVPopfessoriOpener(int ID) : base(ID)
        {
            InitializeComponent();
            var Options = new CoreWebView2EnvironmentOptions();
            if (Config.USE_PROXY)
                Options.AdditionalBrowserArguments = $"--proxy-server={Config.PROXY_HOST}:{Config.PROXY_PORT}";

            var env = CoreWebView2Environment.CreateAsync(null, null, Options).Result;
#if !DEBUG
            this.WebView.Initialized += (s, e) => this.WebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
#endif
            this.WebView.EnsureCoreWebView2Async(env);

            this.DataContext = this;
        }

        public required Uri Uri
        {
            get => (Uri)base.GetValue(UriProperty);
            set => base.SetValue(UriProperty, value);
        }

        public override void OnCloseRequested()
        {
            this.WebView.Dispose();
            GC.Collect();
            base.OnCloseRequested();
        }
    }
}
