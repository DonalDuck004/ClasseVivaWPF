using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVWebView.xaml
    /// </summary>
    /// 

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class CVJsHelper
    {
        // This was veryyyyy funny to emulate :)
        public const string NAME = "cvv";

        public class CVJSCredentials
        {
            public required string token;
            public required string playerId;
        }

        private WebView2 provvider;

        public CVJsHelper(WebView2 provvider) {
            this.provvider = provvider;
            this.provvider.ExecuteScriptAsync("globalThis.cvv = chrome.webview.hostObjects.cvv");
        }

        public void refreshToken()
        {
            var c = Client.INSTANCE.GetMinigameCredentials().Result;

            var obj = new CVJSCredentials()
            {
                token = c.MinigameToken,
                playerId = c.For
            };

            var js_obj = JsonConvert.SerializeObject(obj)!;

            provvider.ExecuteScriptAsync($"globalThis.nsgame.setApiCredentials({js_obj})");
        }

        public void getAPIData()
        {
            provvider.ExecuteScriptAsync($"alert(\"getAPIData\")");
        }
    }

    public partial class CVMinigamesOpener : CVExtraBase, ICloseRequested
    {
        private static DependencyProperty UriProperty;

        static CVMinigamesOpener()
        {
            UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(CVMinigamesOpener));
        }

#if DEBUG
        private CVMinigamesOpener() // For vs editor
        {
            InitializeComponent();
            this.DataContext = this;
        }
#endif

        public CVMinigamesOpener(Content content)
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

        public void OnCloseRequested()
        {
            this.WebView.Dispose();
            GC.Collect();
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
    }
}
