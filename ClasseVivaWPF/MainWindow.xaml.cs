using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.LoginControls;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Forms = System.Windows.Forms;

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
        Forms.ToolStripMenuItem? NotifyIcon = null;

        public delegate void PostLoginEventHandler();
        public event PostLoginEventHandler? PostLogin = null;

        public MainWindow()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
            InitializeComponent();

            this.PostLogin += () => SessionHandler.INSTANCE!.NotificationsFlagChanged += OnNotificationsFlagChanged;
        }

        public bool HasOnlyMainField => this.wrapper.Children.Count == 1;

        public void AddFieldOverlap(FrameworkElement element)
        {
            if (this.wrapper.Children.Count - 1 >= Config.MAX_OVERLAPPED_WIN)
                this.RemoveField((FrameworkElement)this.wrapper.Children[1]);

            this.wrapper.Children.Add(element);
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
            if (element is ICloseRequested sub_win)
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

                NotifyIcon = new Forms.ToolStripMenuItem()
                {
                    Text = "Notifiche",
                    Checked = true,
                    CheckOnClick = true,
                };
                NotifyIcon.CheckedChanged += (s, e) =>
                {
                    SessionHandler.INSTANCE!.SetNotificationsFlag(NotifyIcon.Checked);
                };
                icon.ContextMenuStrip.Items.Add(NotifyIcon);

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
            if (MainWindow.INSTANCE.PostLogin is not null)
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
    }
}
