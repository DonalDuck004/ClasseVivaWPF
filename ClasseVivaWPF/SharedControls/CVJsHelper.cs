using ClasseVivaWPF.Api;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

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

        public CVJsHelper(WebView2 provvider)
        {
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

            this.provvider.ExecuteScriptAsync($"globalThis.nsgame.setApiCredentials({js_obj})");
        }
    }
}
