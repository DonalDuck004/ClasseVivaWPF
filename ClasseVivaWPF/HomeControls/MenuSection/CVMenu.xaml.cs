using ClasseVivaWPF.Api;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.SharedControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVMenu.xaml
    /// </summary>
    public partial class CVMenu : UserControl
    {
        private const string BRIDGE = "https://web.spaggiari.eu/repx/app/default/restbridge.php";

        private SemaphoreSlim PreventOverlap { get; } = new SemaphoreSlim(1, 1);
        private bool CanPress => PreventOverlap.CurrentCount == 1;

        public CVMenu()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        private async Task<Uri> GetUri(string u)
        {
            var ticket = await Client.INSTANCE.Ticket();
            var query = "t=" + ticket.TicketString + "&u=" + u;
            return new UriBuilder(BRIDGE) { Query = query.ToString() }.Uri;
        }

        private async void OpenWebviewer(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();
            var t = new CVRestBridgeViewer() { Uri = await this.GetUri((string)((FrameworkElement)sender).Tag) };
            t.Inject();
            
            PreventOverlap.Release();
        }

        private async void OpenWebviewerBw(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();
            (await this.GetUri((string)((FrameworkElement)sender).Tag)).SystemOpening();
            PreventOverlap.Release();

        }


        private async void BeforeYear(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();
            var t = new CVRestBridgeViewer() { Uri = await this.GetUri("https://web.spaggiari.eu/") };
            t.WebView.ContentLoading += (s, e) =>
            {
                t.Uri = new Uri("https://web.spaggiari.eu/home/app/default/xasapi.php?a=lap&bu=https://web21.spaggiari.eu&ru=/home/&fu=xasapi-ERROR.php");
            };
            t.Inject();

            PreventOverlap.Release();
        }
    }
}
