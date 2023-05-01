using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.RegistrySection.Absences;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Logs;
using System;
using System.Collections.Generic;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    /// <summary>
    /// Logica di interazione per CVDidatic.xaml
    /// </summary>
    public partial class CVDidatic : DFInjectable, ICVMeta
    {
        public bool CountsInStack { get; } = false;
        private TeacherDidactic[]? DidicaticsContent = null;

        public CVDidatic()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnClose(object sender, MouseButtonEventArgs e) => Close();

        private void OnSearch(object sender, MouseButtonEventArgs e)
        {
            // TODO
        }

        public void Update()
        {
            this.TreeDisplayer.Items.Clear();

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
                    DirType = DirType.User
                };
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


                    foreach (var content in folder.Contents.OrderByDescending(z => z.ShareDT))
                    {
                        item.Items.Add(new TreeViewItem()
                        {
                            Header = content.ContentName,
                            Tag = content.ContentID,
                        });

                        if (content.ObjectType is FolderContentType.Link)
                            item_folder.AddMedia(new CVLink()
                            {
                                Media = content,
                                Tag = content.ContentID
                            });
                    }
                }
            }
        }

        public async Task ApiUpdate()
        {
            try
            {
                this.DataFetched = false;

                this.DidicaticsContent = (await Client.INSTANCE.Didatics()).DidacticsContent;
                // this.DidicaticsContent = new TeacherDidactic[0];
                this.Update();
            }
            catch(Exception ex)
            {
                CVMessageBox.Show("Errore", "Errore imprevisto, consulta i log per ulteriori informazioni");
                Logger.Log($"Failed to load/update CVDidatic due: {ex.Message}", LogLevel.ERROR);
                // this.Close();
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
    }
}
