using ClasseVivaWPF.Api;
using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class CVPopfessoriOpener : CVExtraBase, ICloseRequested
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

        public void OnCloseRequested()
        {
            this.WebView.Dispose();
            GC.Collect();
        }
    }
}
