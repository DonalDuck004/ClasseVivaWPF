﻿using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ClasseVivaWPF.LoginControls
{
    /// <summary>
    /// Logica di interazione per CVLoginPage.xaml
    /// </summary>
    public partial class CVLoginPage : UserControl, ICVMeta
    {
        public bool CountsInStack { get; } = false;
     
        public CVLoginPage()
        {
            InitializeComponent();
            this.password.HideContent();

            this.username.Text = "leonardo.burla@ittvt.edu.it";
            this.password.Text = "L2004b2007!";
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
                    var pass = this.password.Text;
                    var uid = this.username.Text;

                    var login = await Client.INSTANCE.Login(pass: pass, uid: uid);
                    Me me;
                    if (login is LoginMultipleChoice mc)
                    {
                        var choicer = new CVLoginAccountSelector(mc);
                        choicer.Inject();
                        await choicer.WaitForExit();
                        if (choicer.Result is null)
                            return;

                        me = (Me)await Client.INSTANCE.Login(pass: pass, uid: uid, ident: choicer.Result);
                    }
                    else
                        me = (Me)login;

                    SessionHandler.InitConn(me.Ident).SetMe(me, pass: this.password.Text, uid: this.username.Text);

                    new Task(async () =>
                    {
                        var cards = await Client.INSTANCE.Cards();
                        Me other;

                        foreach (var card in cards.ContentCards)
                        {

                            SessionMetaController.AddAccount(card, card.Ident == me.Ident);

                            if (card.Ident != me.Ident)
                            {
                                other = (Me)await Client.INSTANCE.Login(pass: pass, uid: uid, ident: card.Ident);
                                SessionHandler.InitConn(card.Ident).SetMe(other, pass: pass, uid: uid, just_register: true);
                            }
                        }
                    }).Start();


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

        public static void EndLogin(bool set_content = true)
        {
            if (set_content)
            {
                var navigation = CVMainNavigation.New();
                MainWindow.INSTANCE.ReplaceMainContent(navigation);
            }

            var raw_content = Client.INSTANCE.Contents().ConfigureAwait(false).GetAwaiter().GetResult();
            CVHome.INSTANCE.Contents = new();

            foreach (var item in raw_content.OrderByDescending(x => x.BeginDate))
            {
                if (item.BeginDate < item.ExpireDate)
                    continue;

                if (!CVHome.INSTANCE.Contents.ContainsKey(item.BeginDate))
                    CVHome.INSTANCE.Contents[item.BeginDate] = new() { item };
                else
                    CVHome.INSTANCE.Contents[item.BeginDate].Add(item);
            }

            Debug.Assert(CVHome.INSTANCE.Contents.Count > 0);

            MainWindow.INSTANCE.RaisePostLogin();
        }
    }
}
