using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Threading;
using System.Diagnostics;
using ClasseVivaWPF.Utils;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVLoginPage.xaml
    /// </summary>
    public partial class CVLoginPage : UserControl
    {
        public CVLoginPage()
        {
            InitializeComponent();
            this.password.HideContent();
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
                    var me = await Client.INSTANCE.Login(pass: this.password.Text, uid: this.username.Text);
                    SessionHandler.InitConn(me.Id).SetMe(me, pass: this.password.Text, uid: this.username.Text);
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

            foreach (var item in raw_content.OrderBy(x => x.Order))
            {
                if (item.BeginDate < item.ExpireDate)
                    continue;

                if (!CVHome.INSTANCE.Contents.ContainsKey(item.BeginDate))
                    CVHome.INSTANCE.Contents[item.BeginDate] = new() { item };
                else
                    CVHome.INSTANCE.Contents[item.BeginDate].Add(item);
            }

            NotificationSystem.INSTANCE.SpawnTask();

            Debug.Assert(CVHome.INSTANCE.Contents.Count > 0);
            CVMenuIcon.INSTANCES[CVMenuIconValues.Home].IsSelected = true;

            if (MainWindow.INSTANCE.PostLogin is not null)
                MainWindow.INSTANCE.PostLogin.Invoke();
        }
    }
}
