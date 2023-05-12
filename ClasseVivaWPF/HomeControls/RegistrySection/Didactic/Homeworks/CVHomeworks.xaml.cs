using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils.Interfaces;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic.Homeworks
{
    /// <summary>
    /// Logica di interazione per CVHomerworks.xaml
    /// </summary>
    public partial class CVHomeworks : DFInjectable, ICVMeta
    {
        public static CVHomeworks? INSTANCE = null;
        public bool CountsInStack => false;

        public Homework[] Homeworks { get; private set; }

        public CVHomeworks()
        {
            InitializeComponent();

            this.DataContext = this;
            INSTANCE = this;
        }

        public void Update()
        {
            this.HomeworksWP.Children.Clear();

            if (this.Homeworks.Length == 0)
            {
                no_content.Visibility = Visibility.Visible;
                return;
            }
            
            no_content.Visibility = Visibility.Collapsed;

            var c = 0;
            foreach (var homework in this.Homeworks)
                this.HomeworksWP.Children.Add(new CVHomeworkPreview(homework, c++));
        }

        public async Task ApiUpdate()
        {
            try
            {
                this.DataFetched = false;

                this.Homeworks = (await Client.INSTANCE.Homeworks()).Homeworks.OrderByDescending(x => x.HomeworkDone ? 0 : 1).ThenBy(x => x.ExpiryDate).ToArray();
                this.Update();
            }
            catch (Exception ex)
            {
                CVMessageBox.Show("Errore", "Errore imprevisto, consulta i log per ulteriori informazioni");
                Logger.Log($"Failed to load/update CVHomeworks due: {ex.Message}", LogLevel.ERROR);
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

        private void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public override void WhenInjectableIsClosed()
        {
            INSTANCE = null;
        }
    }
}
