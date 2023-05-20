using ClasseVivaWPF.Api;
using ClasseVivaWPF.HomeControls.BadgeSection;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.HomeControls.MenuSection;
using ClasseVivaWPF.HomeControls.RegistrySection;
using ClasseVivaWPF.LoginControls;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using ClasseVivaWPF.Themes;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF.HomeControls
{
    /// <summary>
    /// Logica di interazione per CVMainNavigation.xaml
    /// </summary>
    public partial class CVMainNavigation : UserControl, IOnKeyDown, ICVMeta, IOnChildClosed
    {
        public bool CountsInStack { get; } = false;
     
        public static CVMainNavigation? INSTANCE = null;
        public static readonly DependencyProperty AccountSectionExpandedProperty;
        public static readonly DependencyProperty AccountSectionShowManagementProperty;

        public bool AccountSectionExpanded
        {
            get => (bool)GetValue(AccountSectionExpandedProperty);
            set => SetValue(AccountSectionExpandedProperty, value);
        }

        public bool AccountSectionShowManagement
        {
            get => (bool)GetValue(AccountSectionShowManagementProperty);
            set => SetValue(AccountSectionShowManagementProperty, value);
        }

        static CVMainNavigation()
        {
            AccountSectionExpandedProperty = DependencyProperty.Register("AccountSectionExpanded", typeof(bool), typeof(CVMainNavigation));
            AccountSectionShowManagementProperty = DependencyProperty.Register("AccountSectionShowManagement", typeof(bool), typeof(CVMainNavigation));
        }

        private CVMainNavigation()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void BuildAccountSection()
        {
            this.accounts_wp.Children.Clear();
            this.logout_wp.Children.Clear();
           
            CVAccount tmp;
            CVAccountLogout tmp1;
            for (int i = 0; i < SessionMetaController.Current.Accounts.Count; i++)
            {
                this.accounts_wp.Children.Add(tmp = new() { Account = SessionMetaController.Current.Accounts[i] });

                if (SessionMetaController.Current.LastIdx == i)
                    tmp.IsSelected = true;

                tmp.OnSelect += OnAccountChanged!;

                this.logout_wp.Children.Add(tmp1 = new () { Account = SessionMetaController.Current.Accounts[i] });
                tmp1.OnLogout += OnAccountLogout!;
            }
        }
        
        public static CVMainNavigation New()
        {
            if (CVMainNavigation.INSTANCE is null)
                CVMainNavigation.INSTANCE = new();
            CVMainNavigation.INSTANCE.BuildAccountSection();

            return CVMainNavigation.INSTANCE;
        }

        internal void SelectVoice(CVMainMenuIconValues idx)
        {
            if (Current.Children.Count != 0)
            {
                ((IOnSwitch)Current.Children[0]).OnSwitch();
                Current.Children.Clear();
            }

            // IOnSwitch
            if (idx is CVMainMenuIconValues.Home)
            {
                Current.Children.Add(CVHome.INSTANCE);
                if (Config.UNLOAD_HOME_ON_SWITCH)
                    CVHome.GlobDispose();
            }
            else if (idx is CVMainMenuIconValues.Menu)
            {
                Current.Children.Add(new CVMenu());
            }
            else if (idx is CVMainMenuIconValues.Badge)
            {
                Current.Children.Add(new CVBadge());
            }
            else if (idx is CVMainMenuIconValues.Registro)
            {
                Current.Children.Add(new CVRegistry());
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Current.Children.Count != 0 && Current.Children[0] is IOnKeyDown kd)
            {
                kd.OnKeyDown(sender, e);
            }
        }

        public void OnChildClosed()
        {
            if (Current.Children[0] is IOnChildClosed kd)
                kd.OnChildClosed();
        }

        private void OnExpandAccounts(object sender, MouseButtonEventArgs e)
        {
            this.AccountSectionExpanded = !AccountSectionExpanded;
            if (this.AccountSectionExpanded is false)
                this.AccountSectionShowManagement = false;
        }

        private bool ChangeAccount(int old_idx = 0)
        {
            SessionHandler old;

            while (true)
            {
                old = SessionHandler.INSTANCE!;
                SessionHandler.TryInit(out string? error_msg);

                if (error_msg is not null)
                {
                    new CVMessageBox("Errore di login", error_msg).Inject();

                    SessionHandler.DestroyFile(SessionHandler.SessionFileFor(SessionMetaController.Current.CurrentAccount!.Ident));
                    SessionMetaController.RemoveCurrent(old_idx);
                }
                else
                {
                    if (!SessionMetaController.Current.HasAccounts)
                    {
                        this.FinalizeAccountSwitching(kill_db: false);
                        old.Close();

                        MainWindow.INSTANCE.ReplaceMainContent(new CVLoginPage());
                        return false;
                    }

                    old.Close();
                    break;
                }
            }

            Client.INSTANCE.SetLoginToken(SessionHandler.Me!.Token);

            MainWindow.INSTANCE.RemoveField(this);
            CVDay.DestroyCache(true);

            CVLoginPage.EndLogin(set_content: false);

            if (Current.Children.Count > 0 && // Camera section
                Current.Children[0] is IOnFullReload x)
                x.OnFullReload();

            MainWindow.INSTANCE.ReplaceMainContent(this);
            return true;
        }

        private void OnAccountChanged(object sender, EventArgs e)
        {
            var old_idx = SessionMetaController.Current.LastIdx!.Value;

            var meta = ((CVAccount)sender).Account;
            SessionMetaController.Select(meta);

            if (ChangeAccount(old_idx))
                this.AccountSectionExpanded = false;
        }

        private async void OnAddAccount(object sender, MouseButtonEventArgs e)
        {
            var window = new CVLoginPage(true);
            window.Inject();
            await window.WaitForExit();

            if (window.LoginFinalizer is Task task)
                task.Wait();

            this.BuildAccountSection();
        }

        private void OnHandleAccounts(object sender, MouseButtonEventArgs e)
        {
            this.AccountSectionShowManagement = !this.AccountSectionShowManagement;

            if (this.AccountSectionShowManagement)
                this.AccountSectionExpanded = true;
        }

        private void OnAccountLogout(object sender, EventArgs e)
        {
            var meta = ((CVAccountLogout)sender).Account;
            bool tmp;
            if (tmp = meta == SessionMetaController.Current.CurrentAccount)
                SessionMetaController.RemoveCurrent();
            else
                SessionMetaController.Remove(meta);

            if (!SessionMetaController.Current.HasAccounts)
            {
                this.FinalizeAccountSwitching();

                MainWindow.INSTANCE.ReplaceMainContent(new CVLoginPage());

                this.AccountSectionExpanded = false;
                return;
            }

            if (tmp)
                SessionHandler.INSTANCE!.Close();
            else
                SessionMetaController.Select(SessionMetaController.Current.CurrentAccount!);

            if (ChangeAccount())
                this.BuildAccountSection();
            SessionHandler.DestroyFile(SessionHandler.SessionFileFor(meta.Ident));
        }

        private void FinalizeAccountSwitching(bool kill_db = true)
        {
            NotificationSystem.INSTANCE.Stop();
            MainWindow.INSTANCE.HideIcon();
            Client.INSTANCE.UnSetLoginToken();

            if (kill_db)
                SessionHandler.INSTANCE!.Close();
        }
    }
}
