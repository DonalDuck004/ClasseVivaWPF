using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Sessions;
using System.IO;
using System;
using System.Reflection;
using Microsoft.Toolkit.Uwp.Notifications;
using Forms = System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using Windows.UI.Notifications;
using ClasseVivaWPF.LoginControls;
using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.HomeControls.HomeSection;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static MainWindow INSTANCE => (MainWindow)Application.Current.MainWindow;
        public Forms.NotifyIcon icon = new Forms.NotifyIcon();

        public delegate void PostLoginEventHandler();
        public event PostLoginEventHandler PostLogin;

        public MainWindow()
        {
            InitializeComponent();

            PostLogin = () => { };
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
        }

        public bool HasOnlyMainField => this.wrapper.Children.Count == 1;

        public void AddFieldOverlap(FrameworkElement element)
        {
            if (this.wrapper.Children.Count - 1 >= Config.MAX_OVERLAPPED_WIN)
                this.RemoveField((FrameworkElement)this.wrapper.Children[1]);

            this.wrapper.Children.Add(element);
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
            if (element is ICloseRequested sub_win)
                sub_win.OnCloseRequested();

            this.wrapper.Children.Remove(element);

            if (this.wrapper.Children.Count != 0 && this.wrapper.Children[this.wrapper.Children.Count - 1] is IOnChildClosed occ)
                occ.OnChildClosed();
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
            if (SessionHandler.TryInit())
                CVLoginPage.EndLogin();
            else
                this.ReplaceMainContent(new CVLoginPage());

            if (icon.Visible is false)
            {
                icon.Icon = new(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/Images/icon.ico")).Stream);
                icon.ContextMenuStrip = new();
                icon.ContextMenuStrip.Items.Add("Apri", null, (s, e) => this.Show());

                var notify_btn = new Forms.ToolStripMenuItem()
                {
                    Text = "Notifiche",
                    Checked = true,
                    CheckOnClick = true,
                };
                notify_btn.CheckedChanged += (s, e) => {
                    if (notify_btn.Checked)
                        NotificationSystem.INSTANCE.SpawnTask();
                    else
                        NotificationSystem.INSTANCE.Stop();
                };
                icon.ContextMenuStrip.Items.Add(notify_btn);

                icon.ContextMenuStrip.Items.Add("Chiudi", null, (s, e) => Application.Current.Shutdown());
                icon.Visible = true;
            }
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
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

            if (FocusOnMain)
                this.wrapper.Children[0].Focus();
        }

        public void OnPostLogin()
        {
            MainWindow.INSTANCE.PostLogin();
        }

        internal void Goto(ToastArguments args)
        {
            if (args.TryGetValue("goto_home_date", out string v))
            {
                this.Show();

                var date = DateTime.Parse(v);


                if (!SessionHandler.Logged)
                {
                    PostLoginEventHandler? fn = null;
                    this.PostLogin += fn = () =>
                    {
                        RemoveFields();
                        
                        if (CVMainMenuIcon.Selected!.IconValue is not CVMainMenuIconValues.Home)
                            CVMainMenuIcon.INSTANCES[CVMainMenuIconValues.Home].IsSelected = true;


                        if (CVHome.INSTANCE.IsLoaded)
                            CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek);
                        else
                            CVHome.INSTANCE.Loaded += (s, e) =>
                            {
                                CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek);
                            };

                        this.PostLogin -= fn!;
                    };
                }
                else
                {
                    RemoveFields();

                    if (CVMainMenuIcon.Selected!.IconValue is not CVMainMenuIconValues.Home)
                        CVMainMenuIcon.INSTANCES[CVMainMenuIconValues.Home].IsSelected = true;


                    if (CVHome.INSTANCE.IsLoaded)
                    {
                        CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek, update: true);
                    }
                    else
                        CVHome.INSTANCE.Loaded += (s, e) =>
                        {
                            CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek, update: true);
                        };
                }
                return;
            }
            return;
        }

        private void window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
