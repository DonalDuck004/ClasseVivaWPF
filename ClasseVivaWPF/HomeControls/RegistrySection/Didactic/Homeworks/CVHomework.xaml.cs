using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Windows.Media.Audio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic.Homeworks
{
    /// <summary>
    /// Logica di interazione per CVHomework.xaml
    /// </summary>
    public partial class CVHomework : Injectable
    {
        private int src_idx;
        private Homework homework => (Homework)this.DataContext;

        private CVHomework()
        {
            InitializeComponent();
        }

        public CVHomework(Homework homework, int src_idx) // TODO Reload
        {
            InitializeComponent();
            
            this.DataContext = homework;
            this.src_idx = src_idx;
        }

        private void OnClose(object sender, MouseButtonEventArgs e) => Close();

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Update(Homework homework)
        {
            this.DataContext = homework;
            CVHomeworks.INSTANCE!.Homeworks[this.src_idx] = homework;
        }

        private async void OnUploadHomework(object sender, MouseButtonEventArgs e)
        {
            var fd = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                DereferenceLinks = true,
            };

            if (fd.ShowDialog() is true)
            {
                var tmp = new FileStream(fd.FileName, FileMode.Open, FileAccess.Read);
                var hash = tmp.GetMD5();
                var cached_id = SessionHandler.INSTANCE!.GetMappedHomework(tmp.GetMD5());
                if (cached_id is null)
                {
                    var header = await Client.INSTANCE.GetHeaderS3(this.homework.EvtId);
                    await Client.INSTANCE.UploadToS3(header, tmp);
                    cached_id = header.FileID;
                    SessionHandler.INSTANCE.AddMappedHomework(hash, cached_id.Value);
                }

                var upd = await Client.INSTANCE.AddS3Homework(cached_id!.Value, System.IO.Path.GetFileName(fd.FileName), this.homework.EvtId);
                Update(upd);
            }
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            try
            {
                var upd = await Client.INSTANCE.SetTeacherMsgStatus(this.homework.EvtId);
                Update(upd);
            }
            catch
            {

            }
        }
    }
}
