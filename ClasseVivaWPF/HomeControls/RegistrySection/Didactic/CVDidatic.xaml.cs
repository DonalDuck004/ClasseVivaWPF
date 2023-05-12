using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.RegistrySection.Absences;
using ClasseVivaWPF.HomeControls.RegistrySection.Didactic.Homeworks;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using ClasseVivaWPF.Utils.Logs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    /// <summary>
    /// Logica di interazione per CVDidatic.xaml
    /// </summary>
    /// 
    // todo handle spam on reload button (reload lock) 
    public partial class CVDidatic : DFInjectable, ICVMeta
    {
        public bool CountsInStack { get; } = false;
        private TeacherDidactic[]? DidicaticsContent = null;
        private int? highlight;

        public CVDidatic(int? highlight = null)
        {
            InitializeComponent();
            this.DataContext = this;
            this.highlight = highlight;
        }

        private void OnClose(object sender, MouseButtonEventArgs e) => Close();

        private void Search()
        {
            var q = this.SearchBox.Text.ToLower();
            var items = this.TreeDisplayer.Items.OfType<TreeViewItem>().Concat(this.TreeDisplayer.Items.OfType<TreeViewItem>().Select(x => x.Items.OfType<TreeViewItem>()).Merge());
            var folders = this.FolderRoot.Children.OfType<CVFolder>().Concat(this.FolderRoot.Children.OfType<CVFolder>().Select(x => x.SubFolders).Merge());
            var files = folders.Select(x => x.Files).Merge();
            CVFolder? tmp;
            TreeViewItem? ttmp;

            if (this.SearchBox.Text == "")
            {
                foreach (var item in items.Cast<FrameworkElement>().Concat(folders).Concat(files))
                    item.Visibility = Visibility.Visible;
            } else {
                foreach (var item in items.Cast<FrameworkElement>().Concat(folders).Concat(files))
                    item.Visibility = Visibility.Collapsed;

                foreach (var item in items)
                {
                    if (item.Header.ToString()!.ToLower().Contains(q))
                    {
                        ttmp = item;
                        while (true)
                        {
                            if (ttmp.IsExpanded)
                                ttmp.IsExpanded = false;

                            if (ttmp.Visibility is Visibility.Collapsed)
                                ttmp.Visibility = Visibility.Visible;

                            if (ttmp.Parent is TreeView)
                                break;

                            ttmp = (TreeViewItem)ttmp.Parent;
                        }
                    }
                }

                foreach (var folder in folders)
                {
                    if (folder.EffectiveText.ToLower().Contains(q))
                    {
                        tmp = folder;
                        while (true)
                        {
                            if (tmp.IsExpanded)
                                tmp.IsExpanded = false;

                            if (tmp.Visibility is Visibility.Collapsed)
                                tmp.Visibility = Visibility.Visible;

                            tmp = tmp.FindAncestor<CVFolder>();
                            if (tmp is null)
                                break;

                            tmp.IsExpanded = true;
                        }
                    }
                }


                foreach (var file in files)
                {
                    if (file.EffectiveText.ToLower().Contains(q))
                    {
                        file.Visibility = Visibility.Visible;
                        tmp = file.FindAncestor<CVFolder>()!;

                        if (!tmp.IsExpanded && tmp.Visibility is Visibility.Collapsed)
                        {
                            file.Visibility = Visibility.Visible;
                            tmp = file.FindAncestor<CVFolder>()!;
                            if (tmp.Visibility is Visibility.Collapsed)
                                file.Visibility = Visibility.Visible;
                        }
                        else
                            tmp.IsExpanded = true;
                    }
                    else if ((tmp = file.FindAncestor<CVFolder>()) is not null && tmp.Visibility is Visibility.Visible)
                        file.Visibility = Visibility.Visible;
                }
            }
        }

        public void Update()
        {
            this.TreeDisplayer.Items.Clear();
            this.FolderRoot.Children.Clear();

            if (this.DidicaticsContent!.Length == 0)
                this.no_content.Visibility = Visibility.Visible;
            else
                this.no_content.Visibility = Visibility.Collapsed;

            CVFolder parent_folder;
            CVFolder item_folder;

            TreeViewItem parent;
            TreeViewItem item;

            foreach (var teacher in this.DidicaticsContent!)
            {
                parent = new TreeViewItem()
                {
                    Header = "Cartelle di " + teacher.TeacherName.ToTitle(),
                    Tag = teacher.TeacherID
                };
                this.TreeDisplayer.Items.Add(parent);

                parent_folder = new CVFolder()
                {
                    Teacher = teacher,
                    Tag = teacher.TeacherID,
                    DirType = DirType.Teacher
                };

                parent_folder.IsExpandedChanged += OnExpandFromFolder;

                this.FolderRoot.Children.Add(parent_folder);


                foreach (var folder in teacher.Folders.OrderByDescending(y => y.LastShareDT))
                {
                    if (folder.Contents.Length == 0)
                        continue;

                    item = new TreeViewItem()
                    {
                        Header = folder.FolderName,
                        Tag = folder.FolderID,
                    };
                    parent.Items.Add(item);

                    item_folder = new CVFolder()
                    {
                        Folder = folder,
                        Tag = folder.FolderID,
                        DirType = DirType.Folder
                    };

                    parent_folder.AddFolder(item_folder);

                    item_folder.IsExpandedChanged += OnExpandFromFolder;

                    foreach (var content in folder.Contents.OrderByDescending(z => z.ShareDT))
                    {
                        item.Items.Add(new TreeViewItem()
                        {
                            Header = content.ContentName,
                        });

                        if (content.ObjectType is FolderContentType.Link)
                        {
                            item_folder.AddMedia(new CVLink()
                            {
                                Media = content
                            });
                        }
                        else if (content.ObjectType is FolderContentType.File)
                        {
                            item_folder.AddMedia(new CVFile()
                            {
                                Media = content
                            });
                        }
                        else if (content.ObjectType is FolderContentType.Text)
                        {
                            item_folder.AddMedia(new CVText()
                            {
                                Media = content
                            });
                        }else
                            Debugger.Break();

                        if (content.ContentID == highlight)
                        {
                            item_folder.IsExpanded = true;
                            highlight = null;
                        }
                    }
                }
            }

            if (this.SearchBox.Text != "")
                this.Search();
        }

        public async Task ApiUpdate()
        {
            try
            {
                this.DataFetched = false;

                this.DidicaticsContent = (await Client.INSTANCE.Didatics()).DidacticsContent;
                this.Update();
            }
            catch(Exception ex)
            {
                CVMessageBox.Show("Errore", "Errore imprevisto, consulta i log per ulteriori informazioni");
                Logger.Log($"Failed to load/update CVDidatic due: {ex.Message}", LogLevel.ERROR);
                this.Close();
            }
            finally
            {
                this.DataFetched = true;
            }
        }

        private async void OnUpdateBtn(object sender, MouseButtonEventArgs e) => await ApiUpdate();

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            await ApiUpdate();
        }

        private void OnExpandFromTree(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            if (!item.HasItems)
                return;

            CVFolder folder;
            if (item.Parent is TreeView) // Teacher
                folder = this.FolderRoot.Children.OfType<CVFolder>().Where(y => (string)y.Tag == (string)item.Tag).First();
            else
                folder = this.FolderRoot.Children.OfType<CVFolder>().Select(x => x.SubFolders).Merge().Where(y => (int)y.Tag == (int)item.Tag).First();


            if (folder.IsExpanded != item.IsExpanded)
                folder.IsExpanded = item.IsExpanded;
        }

        private void OnExpandFromFolder(CVFolder folder)
        {
            TreeViewItem item;

            if (folder.DirType is DirType.Teacher)
                item = this.TreeDisplayer.Items.OfType<TreeViewItem>().Where(y => (string)y.Tag == (string)folder.Tag).First();
            else
                item = this.TreeDisplayer.Items.OfType<TreeViewItem>().Select(x => x.Items.OfType<TreeViewItem>()).Merge().Where(y => (int)y.Tag == (int)folder.Tag).First();

            if (item.IsExpanded != folder.IsExpanded)
                item.IsExpanded = folder.IsExpanded;
        }

        private void OnSearch(object sender, MouseButtonEventArgs e) => Search();

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
                Search();
        }

        private void OnOpenHomeworks(object sender, MouseButtonEventArgs e)
        {
            new CVHomeworks().Inject();
        }
    }
}
