using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    /// <summary>
    /// Logica di interazione per CVUrl.xaml
    /// </summary>
    public partial class CVLink : CVBaseMedia
    {
        public CVLink()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async Task<Uri?> GetUri()
        {
            if (this.HyperLink.NavigateUri.OriginalString == "file://placeholder")
            {
                try
                {
                    await this.Media.GetItem();
                }
                catch (Exception ex)
                {
                    CVMessageBox.Show("Errore", "Impossibile aprire questo url, a causa di un errore di rete");
                    Logger.Log($"Failed to get item of {this.Media.ObjectId}\n{ex.Message}", LogLevel.ERROR);
                    return null;
                }

                this.HyperLink.NavigateUri = new(this.Media.CachedItem!.Item.Link);
            }

            return this.HyperLink.NavigateUri;
        }

        private async void OpenUrl(object sender, RequestNavigateEventArgs e)
        {
            if (await GetUri() is Uri uri)
                new CVWebView() { Uri = uri }.Inject();
        }


        private async void OpenBrowser(object sender, RoutedEventArgs e)
        {
            if (await GetUri() is Uri uri)
                uri.SystemOpening();
        }
    }
}
