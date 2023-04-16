using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF.SharedControls
{

    public partial class CVMinigamesOpener : Injectable
    {
        private static DependencyProperty UriProperty;

        static CVMinigamesOpener()
        {
            UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(CVMinigamesOpener));
        }

#if DEBUG
        private CVMinigamesOpener() : base() // For vs editor
        {
            InitializeComponent();
        }
#endif

        public CVMinigamesOpener(Content content) : base()
        {
            InitializeComponent();
            var Options = new CoreWebView2EnvironmentOptions();
            if (Config.USE_PROXY)
                Options.AdditionalBrowserArguments = $"--proxy-server={Config.PROXY_HOST}:{Config.PROXY_PORT}";

            var env = CoreWebView2Environment.CreateAsync(null, null, Options).Result;
#if !DEBUG
            this.WebView.Initialized += (s, e) => this.WebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
#else
            this.WebView.SourceChanged += (s, e) => {
                this.WebView.CoreWebView2.OpenDevToolsWindow();
                this.WebView.CoreWebView2.AddHostObjectToScript("cvv", new CVJsHelper(this.WebView));
            };
#endif
            this.WebView.NavigationStarting += (s, e) =>
                this.WebView.CoreWebView2.AddHostObjectToScript(CVJsHelper.NAME, new CVJsHelper(this.WebView));

            this.WebView.EnsureCoreWebView2Async(env);
            new Task(async () =>
                await Client.INSTANCE.SetInteraction(content.ContentID, Interaction.REACTION_CLICK)
            ).Start();



            this.Uri = new(content.Link!);

            this.DataContext = this;
        }

        public Uri Uri
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

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();
    }
}
