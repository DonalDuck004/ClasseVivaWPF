using ClasseVivaWPF.HomeControls.BadgeSection;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.HomeControls.MenuSection;
using ClasseVivaWPF.HomeControls.RegistrySection;
using ClasseVivaWPF.LoginControls;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Security.Principal;
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

        public bool AccountSectionExpanded
        {
            get => (bool)GetValue(AccountSectionExpandedProperty);
            set => SetValue(AccountSectionExpandedProperty, value);
        }

        static CVMainNavigation()
        {
            AccountSectionExpandedProperty = DependencyProperty.Register("AccountSectionExpanded", typeof(bool), typeof(CVMainNavigation));
        }

        private CVMainNavigation()
        {
            InitializeComponent();
            this.DataContext = this;

            CVAccount tmp;
            for (int i = 0; i < SessionMetaController.Current.Accounts.Count; i++)
            {
                this.accounts_wp.Children.Add(tmp = new() { Account = SessionMetaController.Current.Accounts[i] });

                if (SessionMetaController.Current.LastIdx == i)
                    tmp.IsSelected = true;

                tmp.OnSelect += OnAccountChanged!;
            }
        }
        
        public static CVMainNavigation New()
        {
            if (CVMainNavigation.INSTANCE is null)
                CVMainNavigation.INSTANCE = new();

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
                if (Config.UNLOAD_TABS_ON_SWITCH)
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
        }

        private void OnAccountChanged(object sender, EventArgs e)
        {
            var old_idx = SessionMetaController.Current.LastIdx!.Value;

            var meta = ((CVAccount)sender).Account;
            SessionMetaController.Select(meta);
            SessionHandler.INSTANCE!.Close();
            SessionHandler.TryInit(out string? error_msg);
            
            if (error_msg is not null)
            {
                new CVMessageBox("Errore di login", error_msg).Inject();

                SessionMetaController.RemoveCurrent(old_idx);
            }

            MainWindow.INSTANCE.RemoveField(this);

            CVLoginPage.EndLogin(set_content: false);
            if (Current.Children.Count > 0 && // Camera section
                Current.Children[0] is IOnUpdateRequired x)
                x.OnUpdateRequired();

            MainWindow.INSTANCE.ReplaceMainContent(this);
            this.AccountSectionExpanded = false;
        }
    }
}
