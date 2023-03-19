using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Sessions;

using Forms = System.Windows.Forms;
using System.IO;
using System;
using System.Reflection;
using Microsoft.Toolkit.Uwp.Notifications;

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
        public Action? PostLogin = null;

        public MainWindow()
        {
            InitializeComponent();
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
        }

        public void AddFieldOverlap(FrameworkElement element)
        {
            this.wrapper.Children.Add(element);
        }

        public void ReplaceMainContent(FrameworkElement element)
        {
            if (this.wrapper.Children.Count != 0)
                this.wrapper.Children.RemoveAt(0);

            this.wrapper.Children.Insert(0, element);
            element.Focus();
        }

        public void RemoveField(UIElement element) => RemoveField((FrameworkElement)element);


        public void RemoveField(FrameworkElement element)
        {
            this.wrapper.Children.Remove(element);
        }


        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SessionHandler.TryInit())
                CVLoginPage.EndLogin();
            else
                this.ReplaceMainContent(new CVLoginPage());

            if (icon.Visible is false)
            {
                icon.Icon = new("pacman.ico");
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
            if (e.Key is Key.Escape && this.wrapper.Children.Count > 1)
            {
                var child = this.wrapper.Children[this.wrapper.Children.Count - 1];

                if (child is IOnEscKey sub_win)
                    sub_win.OnEscKey();
                else
                    this.RemoveField(child);
            }
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        internal void Goto(ToastArguments args)
        {
            if (args.TryGetValue("goto_home_date", out string v))
            {
                this.Show();

                var fn = () =>
                {
                    if (CVMenuIcon.Selected!.Type is not CVMenuIconValues.Home)
                        CVMenuIcon.INSTANCES[CVMenuIconValues.Home].IsSelected = true;
                    
                    var date = DateTime.Parse(v);

                    if (CVHome.INSTANCE.IsLoaded)
                        CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek);
                    else
                        CVHome.INSTANCE.Loaded += (s, e) =>
                        {
                            CVWeek.GetWeekContaining(date).SelectChild(date.DayOfWeek);
                        };

                    this.PostLogin = null;
                };

                if (!SessionHandler.Logged)
                    this.PostLogin = fn;
                else
                    fn.Invoke();
                return;
            }

            throw new NotImplementedException();
        }
    }
}
