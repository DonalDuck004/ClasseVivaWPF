using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using System.Diagnostics;
using System.Linq;
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

            this.username.Text = "S7319056Z";
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
            var navigation = CVMainNavigation.New();
            MainWindow.INSTANCE.ReplaceMainContent(navigation);
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

            if (SessionHandler.INSTANCE!.GetNotificationsFlag())
                NotificationSystem.INSTANCE.SpawnTask();

            Debug.Assert(CVHome.INSTANCE.Contents.Count > 0);

            MainWindow.INSTANCE.OnPostLogin();
        }
    }
}
