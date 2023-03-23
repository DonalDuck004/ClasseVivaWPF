using ClasseVivaWPF.Api;
using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVWebView.xaml
    /// </summary>
    public partial class CVWebView : CVExtraBase, ICloseRequested
    {
        private static DependencyProperty UriProperty;

        static CVWebView()
        {
            UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(CVWebView));
        }

#if DEBUG
        private CVWebView() : base() // For vs editor
        {
            InitializeComponent();
            this.DataContext = this;
        }
#endif

        public CVWebView(int ID) : base(ID)
        {
            InitializeComponent();
            var Options = new CoreWebView2EnvironmentOptions();
            if (Config.USE_PROXY)
                Options.AdditionalBrowserArguments = $"--proxy-server={Config.PROXY_HOST}:{Config.PROXY_PORT}";

            var env = CoreWebView2Environment.CreateAsync(null, null, Options).Result;
            this.WebView.EnsureCoreWebView2Async(env);

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
        }

    }
}
