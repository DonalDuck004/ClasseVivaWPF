using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.HomeControls.Icons;
using ClasseVivaWPF.HomeControls.RegistrySection;
using ClasseVivaWPF.LoginControls;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using ClasseVivaWPF.Utils.Themes;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Forms = System.Windows.Forms;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty CurrentThemeProperty;
        public static readonly DependencyProperty DefaultFontColorProperty;

#if DEBUG
        private readonly Key[] KonamiCode = new Key[] { Key.K, Key.Enter };
#else
        private readonly Key[] KonamiCode = new Key[] { Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.B, Key.A, Key.Enter };
#endif
        private int KonamiCodeIndex = 0;

        public static MainWindow INSTANCE => (MainWindow)Application.Current.MainWindow;
        public Forms.NotifyIcon icon = new Forms.NotifyIcon();
        public Forms.ToolStripMenuItem? NotifyIcon = null;
        private int PagesStackSize;

        public delegate void PostLoginEventHandler();
        public event PostLoginEventHandler PostLogin;

        public ITheme LastTheme;

        public ITheme CurrentTheme
        {
            get => (ITheme)GetValue(CurrentThemeProperty);
            set
            {
                this.LastTheme = this.CurrentTheme;
                SetValue(CurrentThemeProperty, value);
            }
        }

        public SolidColorBrush DefaultFontColor
        {
            get => (SolidColorBrush)GetValue(DefaultFontColorProperty);
            set => SetValue(DefaultFontColorProperty, value);
        }


        static MainWindow()
        {
            ThemeOperations.Register(ThemeInitializer.New<WhiteTheme>());
            ThemeOperations.LoadFromThemeDir();

            CurrentThemeProperty = DependencyProperty.Register("CurrentTheme", typeof(ITheme), typeof(MainWindow), new PropertyMetadata(ThemeOperations.Get(Config.DEFAULT_THEME_NAME)));
            DefaultFontColorProperty = DependencyProperty.Register("DefaultFontColor", typeof(SolidColorBrush), typeof(MainWindow));
        }

        public MainWindow()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
            NotificationSystem.AvoidCompilationExcelusion();
            this.DataContext = this;
            this.AddHandler(Keyboard.KeyDownEvent, new KeyEventHandler(window_KeyDown), true);
            InitializeComponent();

            this.PostLogin += OnPostLogin;
        }


        private void OnPostLogin()
        {
            if (this.NotifyIcon is not null)
                this.NotifyIcon!.Checked = SessionHandler.INSTANCE!.GetNotificationsFlag();

   
            if (this.icon.Visible is false)
            {
                this.icon.Icon = new(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/Images/icon.ico")).Stream);
                this.icon.ContextMenuStrip = new();
                this.icon.ContextMenuStrip.Items.Add("Apri", null, (s, e) => this.Show());

                this.NotifyIcon = new Forms.ToolStripMenuItem()
                {
                    Text = "Notifiche",
                    Checked = true,
                    CheckOnClick = true,
                };
                this.NotifyIcon.CheckedChanged += (s, e) =>
                {
                    SessionHandler.INSTANCE!.SetNotificationsFlag(NotifyIcon.Checked);
                };
                this.icon.ContextMenuStrip.Items.Add(NotifyIcon);

                this.icon.ContextMenuStrip.Items.Add("Chiudi", null, (s, e) => Application.Current.Shutdown());
                this.icon.Visible = true;
            }

            this.PagesStackSize = SessionHandler.INSTANCE!.GetPagesStackSize();
            SessionHandler.INSTANCE!.NotificationsFlagChanged += OnNotificationsFlagChanged;
            SessionHandler.INSTANCE!.PagesStackSizeChanged += OnPagesStackSizeChanged;
        }

        private void OnPagesStackSizeChanged(SessionHandler sender, int Size)
        {
            this.PagesStackSize = Size;
        }

        public bool HasOnlyMainField => this.wrapper.Children.Count == 1;

        public void AddFieldOverlap(FrameworkElement element)
        {
            var f = this.wrapper.Children.OfType<FrameworkElement>().Where(x => x is not ICVMeta y || y.CountsInStack is true);

            if (this.PagesStackSize != 0 && f.Count() >= this.PagesStackSize)
                this.RemoveField(f.First());

            this.wrapper.Children.Add(element);
        }

        public FrameworkElement GetLastField()
        {
            return (FrameworkElement)this.wrapper.Children[this.wrapper.Children.Count - 1];
        }

        private void OnNotificationsFlagChanged(SessionHandler sender, bool Flag)
        {
            if (this.NotifyIcon is not null)
                this.NotifyIcon.Checked = Flag;
        }

        public void ReplaceMainContent(FrameworkElement element, bool animate = true)
        {
            if (this.wrapper.Children.Count != 0)
                this.RemoveField((FrameworkElement)this.wrapper.Children[0]);

            if (animate)
            {
                var animation = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(1)),
                };

                element.BeginAnimation(FrameworkElement.OpacityProperty, animation);
            }

            this.wrapper.Children.Insert(0, element);
        }

        public void RemoveField(FrameworkElement element)
        {
            if (element is IOnCloseRequested sub_win)
                sub_win.OnCloseRequested();

            this.wrapper.Children.Remove(element);

            if (this.wrapper.Children.Count != 0 && this.wrapper.Children[this.wrapper.Children.Count - 1] is IOnChildClosed occ)
                occ.OnChildClosed();
        }

        public void RemoveLastField(bool @unsafe = false)
        {
            if (!@unsafe && this.wrapper.Children.Count == 1)
                return;

            this.RemoveField((FrameworkElement)this.wrapper.Children[this.wrapper.Children.Count - 1]);
        }

        public bool UpdateLast()
        {
            if (this.wrapper.Children.Count == 0)
                return false;

            if (this.wrapper.Children[wrapper.Children.Count - 1] is not IUpdate x)
                return false;

            x.Update();
            return true;
        }

        public new void Show()
        {
            if (this.WindowState is WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                this.UpdateLayout();
            }

            base.Show();
            this.Activate();
        }

        private void window_Loaded(object sender, EventArgs e)
        {

            if (SessionMetaController.Current.Accounts.Count == 0)
            {
                this.ReplaceMainContent(new CVLoginPage());
                return;
            }
                
            string? api_error_message;

            if (SessionHandler.TryInit(out api_error_message))
            {
                CVLoginPage.EndLogin();
                return;
            }

            string errors = "";
            while (SessionMetaController.Current.HasAccounts)
            {
                errors += $"{SessionMetaController.Current.CurrentAccount!.Ident}: {api_error_message}\n";
                    
                SessionMetaController.RemoveCurrent();
                if (SessionHandler.TryInit(out api_error_message))
                {
                    CVLoginPage.EndLogin();

                    if (errors != "")
                        CVMessageBox.Show("Errore di Login", errors);

                    return;
                }
            }

            this.ReplaceMainContent(new CVLoginPage());

            CVMessageBox.Show("Errore di Login", errors);

        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (KonamiCode[KonamiCodeIndex] == e.Key && !ThemeProperties.INSTANCE.Editing)
            {
                KonamiCodeIndex++;

                if (KonamiCodeIndex == KonamiCode.Length)
                {
                    this.CurrentTheme = this.CurrentTheme is not EasterEggTheme ? new EasterEggTheme() : this.LastTheme;
                    KonamiCodeIndex = 0;
                    return;
                }
            }
            else
                KonamiCodeIndex = 0;


            if (e.Key is Key.Escape)
            {
                if (this.wrapper.Children.Count > 1)
                {
                    var child = this.wrapper.Children[this.wrapper.Children.Count - 1];

                    this.RemoveField((FrameworkElement)child);
                }

            }
            else if (this.wrapper.Children.Count != 0 && this.wrapper.Children[this.wrapper.Children.Count - 1] is IOnKeyDown kw)
            {
                kw.OnKeyDown(sender, e);
            }
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.RemoveFields(FocusOnMain: false);
        }

        private void RemoveFields(bool FocusOnMain = true)
        {
            while (this.wrapper.Children.Count > 1)
                this.RemoveField((FrameworkElement)this.wrapper.Children[1]);

            if (FocusOnMain && this.wrapper.Children.Count > 0)
                this.wrapper.Children[0].Focus();
        }

        public void RaisePostLogin()
        {
            if (MainWindow.INSTANCE.PostLogin is not null)
                MainWindow.INSTANCE.PostLogin();
        }

        internal void Goto(ToastArguments args)
        {
            this.Show();

            if (args.TryGetValue(NotificationSystem.GOTO_HOME, out string v))
                HandleGotoDate(DateTime.Parse(v));
            else if (args.TryGetValue(NotificationSystem.GOTO_DIDATIC, out v))
                HandleGotoDitatic(int.Parse(v));

            return;

            void SetAppSection(CVMainMenuIconValues section)
            {
                if (CVBaseIcon.Selected is null || CVBaseIcon.Selected.IconValue != section)
                    CVBaseIcon.INSTANCES[section].IsSelected = true;
            }

            void GotoDate(DateTime date)
            {
                if (CVHome.INSTANCE.IsLoaded)
                    CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek, update: true);
                else
                    CVHome.INSTANCE.Loaded += (s, e) => CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek, update: true);
            }

            void HandleGotoDate(DateTime date)
            {

                if (!SessionHandler.Logged)
                {
                    PostLoginEventHandler? fn = null;
                    this.PostLogin += fn = () =>
                    {
                        SetAppSection(CVMainMenuIconValues.Home);

                        GotoDate(date);
                        this.PostLogin -= fn!;
                    };
                }
                else
                {
                    SetAppSection(CVMainMenuIconValues.Home);

                    GotoDate(date);
                }
            }
        
            void HandleGotoDitatic(int id)
            {
                if (!SessionHandler.Logged)
                {
                    PostLoginEventHandler? fn = null;
                    this.PostLogin += fn = () =>
                    {
                        SetAppSection(CVMainMenuIconValues.Registro);
                        CVRegistry.INSTANCE!.OpenDidatic(id);

                        this.PostLogin -= fn!;
                    };
                }
                else
                {
                    SetAppSection(CVMainMenuIconValues.Registro);

                    CVRegistry.INSTANCE!.OpenDidatic(id);
                }
            }
        }

        private void window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus is null)
                return;

            if (e.NewFocus is not MainWindow)
            {
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this);
            }
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton is MouseButton.XButton1)
            {
                this.RemoveLastField(@unsafe: false);
                e.Handled = true;
            }
        }

        public void HideIcon()
        {
            icon.Visible = false;
        }
    }
}
