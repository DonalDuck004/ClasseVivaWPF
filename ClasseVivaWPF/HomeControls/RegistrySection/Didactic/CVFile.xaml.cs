using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    /// <summary>
    /// Logica di interazione per CVMedia.xaml
    /// </summary>
    public partial class CVFile : CVBaseMedia
    {
        public static readonly DependencyProperty DownloaderProperty;
        public static readonly DependencyProperty CompletedProperty;

        private string downloaded_path;

        public MediaDownloader Downloader
        {
            get => (MediaDownloader)GetValue(DownloaderProperty);
            set => SetValue(DownloaderProperty, value);
        }

        public bool Completed
        {
            get => (bool)GetValue(CompletedProperty);
            set => SetValue(CompletedProperty, value);
        }


        static CVFile()
        {
            DownloaderProperty = DependencyProperty.Register("Downloader", typeof(MediaDownloader), typeof(CVFile));
            CompletedProperty = DependencyProperty.Register("Completed", typeof(bool), typeof(CVFile), new PropertyMetadata(false));
        }

        public CVFile()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private string? _tree = null;
        private string Tree
        {
            get
            {
                if (_tree is null)
                {
                    CVFolder? parent = this.FindAncestor<CVFolder>()!;
                    _tree = "";
                    do
                    {
                        _tree = System.IO.Path.Join(parent.DirType is DirType.User ?
                                            FNameFilter(parent.Teacher!.TeacherName.ToTitle(), $"Teacher{parent.Teacher.TeacherID}") :
                                            FNameFilter(parent.Folder!.FolderName.ToTitle(), $"Teacher{parent.Folder!.FolderID}"), _tree);
                    } while ((parent = parent!.FindAncestor<CVFolder>()) is not null);

                    _tree = System.IO.Path.Join(SessionHandler.Me!.Ident, _tree);
                }

                return _tree;
            }
        }

        private string? FNameFilter(string name, string? fallback = null)
        {
            var regex = new Regex(@"[^a-zA-Z0-9_\-\s\.]");
            var txt = regex.Replace(name, "").TrimEnd(' ');
            return txt == "" ? fallback : txt;
        }

        private string GetPath(string filename, bool dir_required = false, params string[] fragment)
        {
            var path = System.IO.Path.Join(KnownFolders.GetPath(KnownFolder.Downloads), "ClassevivaDesktop") + "\\" + System.IO.Path.Join(fragment);

            if (dir_required && !Directory.Exists(path))
                Directory.CreateDirectory(path);

            return System.IO.Path.Join(path, FNameFilter(filename, $"{this.Media.ObjectType}_{this.Media.ContentID}"));
        }

        private string GetTempPath()
        {
            return System.IO.Path.Join(System.IO.Path.GetTempPath(), $"{this.Media.ObjectType}_{this.Media.ContentID}");
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;
            var t = SessionHandler.INSTANCE!.GetMappedFile(this.Media.ContentID);

            if (SessionHandler.INSTANCE!.GetMappedFile(this.Media.ContentID) is string filename && File.Exists(this.downloaded_path = GetPath(filename, false, this.Tree)))
                this.Completed = true;
            else
                this.Downloader = MediaDownloader.New(this.Media.ContentID);
        }

        private async void OnBeginDownload(object sender, RoutedEventArgs e)
        {
            var temp = GetTempPath();
            this.Downloader.DownloadTaskEnded = (d, t, s) =>
            {
                if (t.IsFaulted)
                {
                    this.Completed = false;
                    d.Reset();
                    CVMessageBox.Show("Errore", "Download fallito!");
                    return;
                }
                Debug.Assert(t.IsCompleted);

                this.Completed = true;
                SessionHandler.INSTANCE!.AddMappedFile(this.Media.ContentID, d.Name);
                downloaded_path = GetPath(d.Name, true, this.Tree);

                s.Close();
                File.Move(temp, downloaded_path, true);
            };

            try
            {
                var writer = new FileStream(temp, FileMode.Create, FileAccess.Write);

                await this.Downloader.Begin(writer);
            }
            catch (IOException)
            {

            }
        }

        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            new Uri(this.downloaded_path).SystemOpening();
        }

        private void OnOpenFolder(object sender, RoutedEventArgs e)
        {
            new Uri(System.IO.Path.GetDirectoryName(this.downloaded_path)!).SystemOpening();
        }
    }
}
