using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVRestBridgeViewer.xaml
    /// </summary>
    public partial class CVRestBridgeViewer : UserControl, ICloseRequested
    {
        private static DependencyProperty UriProperty;

        static CVRestBridgeViewer()
        {
            UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(CVRestBridgeViewer));
        }

        public CVRestBridgeViewer()
        {
            InitializeComponent();
            var Options = new CoreWebView2EnvironmentOptions();
            if (Config.USE_PROXY)
                Options.AdditionalBrowserArguments = $"--proxy-server={Config.PROXY_HOST}:{Config.PROXY_PORT}";

            var env = CoreWebView2Environment.CreateAsync(null, null, Options).Result;
            this.WebView.EnsureCoreWebView2Async(env);

            this.DataContext = this;
        }

        public void OnCloseRequested()
        {
            this.WebView.Dispose();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public required Uri Uri
        {
            get => (Uri)base.GetValue(UriProperty);
            set => base.SetValue(UriProperty, value);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }
    }
}
