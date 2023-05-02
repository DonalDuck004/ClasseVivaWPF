using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils.Interfaces;
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
    public partial class CVLoginPage : Injectable, ICVMeta
    {
        public bool CountsInStack { get; }
        public Task? LoginFinalizer { get; private set; } = null;


        public CVLoginPage(bool injectable_mode = false)
        {
            InitializeComponent();
            this.password.HideContent();

            this.CountsInStack = injectable_mode;
            if (!injectable_mode)
            {
                this.BackIcon.Width = 0;
                this.BackIcon.Visibility = Visibility.Hidden;
            }

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

                    Client client;

                    if (this.CountsInStack)
                        client = new(set_instance: false);
                    else
                        client = Client.INSTANCE; 

                    var login = await client.Login(pass: pass, uid: uid);
                    
                    Me me;
                    if (login is LoginMultipleChoice mc)
                    {
                        var choicer = new CVLoginAccountSelector(mc);
                        choicer.Inject();
                        await choicer.WaitForExit();
                        if (choicer.Result is null)
                            return;

                        me = (Me)await client.Login(pass: pass, uid: uid, ident: choicer.Result);
                        client.SetLoginToken(me.Token);

                        this.LoginFinalizer = new Task(async () =>
                        {

                            var cards = await client.Cards(id: me.Id);
                            Me other;

                            foreach (var card in cards.ContentCards)
                            {

                                SessionMetaController.AddAccount(card, card.Ident == me.Ident && !this.CountsInStack);

                                if (card.Ident != me.Ident)
                                {
                                    other = (Me)await client.Login(pass: pass, uid: uid, ident: card.Ident);
                                    SessionHandler.InitConn(card.Ident).SetMe(other, pass: pass, uid: uid, just_register: true);
                                }
                            }
                        });

                        this.LoginFinalizer.Start();
                    }
                    else
                    {
                        me = (Me)login;

                        client.SetLoginToken(me.Token);

                        SessionMetaController.AddAccount((await client.Card(id: me.Id)).ContentCard, !this.CountsInStack);
                    }


                    SessionHandler.InitConn(me.Ident).SetMe(me, pass: this.password.Text, uid: this.username.Text, just_register: this.CountsInStack);

                    if (this.CountsInStack)
                        this.Close();
                    else
                        CVLoginPage.EndLogin(src: this);
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

        public static void EndLogin(bool set_content = true, CVLoginPage? src = null)
        {
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

            if (src is not null && src.LoginFinalizer is Task x)
                x.Wait();

            if (set_content)
            {
                var navigation = CVMainNavigation.New();
                MainWindow.INSTANCE.ReplaceMainContent(navigation);
            }

            MainWindow.INSTANCE.RaisePostLogin();
        }

        private void OnClose(object sender, System.Windows.Input.MouseButtonEventArgs e) => Close();
    }
}
