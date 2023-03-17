using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
            this.password.HideContent();

            this.username.Text = "S7319056Z";
            this.password.Text = "bl32848n";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;

            try
            {
                bool ret = false;

                if (this.password.IsEmpty())
                    ret = this.password.Warn = true;

                if (this.username.IsEmpty())
                    ret = this.username.Warn = true;

                if (ret)
                    return;

                try
                {
                    var me = await Client.INSTANCE.Login(password: this.password.Text, uid: this.username.Text);
                    SessionHandler.InitConn(me.Id).SetMe(me);
                    EndLogin();
                }
                catch (ApiError exc)
                {
                    exc.ApplyStdProcedure("Errore di Login");
                }
            }
            finally
            {
                btn.IsEnabled = true;
            }
        }

        public static void EndLogin()
        {
            var navigation = new CVMainNavigation();
            MainWindow.INSTANCE.ReplaceMainContent(navigation);
            var raw_content = Client.INSTANCE.Contents().ConfigureAwait(false).GetAwaiter().GetResult();
            CVHome.INSTANCE.Contents = new();

            foreach (var item in raw_content.OrderBy(x => x.Ordine))
            {
                for (var from = item.Inizio; from <= item.Fine; from = from.AddDays(1))
                {
                    if (!CVHome.INSTANCE.Contents.ContainsKey(from))
                        CVHome.INSTANCE.Contents[from] = new() { item };
                    else
                        CVHome.INSTANCE.Contents[from].Add(item);
                }
            }

            navigation.SelectVoice(0);

        }
    }
}
