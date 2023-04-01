using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVSettings.xaml
    /// </summary>
    public partial class CVSettings : UserControl
    {
        public static string MediaDirSize => new DirectoryInfo(Config.MEDIA_DIR_PATH).GetFiles().Sum(x => x.Length).SizeSuffix();
        public static string? SessionCacheSize => SessionHandler.INSTANCE?.GetCacheSize().SizeSuffix();
        public static string? DBSize => SessionHandler.INSTANCE is null ? null : new FileInfo(SessionHandler.INSTANCE?.FileName!).Length.SizeSuffix();
     
        public CVSettings()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }

        private void OpenRepo(object sender, MouseButtonEventArgs e)
        {
            new Uri(Config.REPO_URL).SystemOpening();
        }

        private void OpenMediaDir(object sender, RoutedEventArgs e)
        {
            new Uri(Config.MEDIA_DIR_PATH).SystemOpening();
        }

        private void OpenSessionDir(object sender, RoutedEventArgs e)
        {
            new Uri(Config.SESSIONS_DIR_PATH).SystemOpening();
        }
    }
}
