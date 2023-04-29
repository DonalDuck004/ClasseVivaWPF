using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    public record CVJustifyPopupResult(string JCode, string Desc);

    /// <summary>
    /// Logica di interazione per CVJustifyPopup.xaml
    /// </summary>
    public partial class CVJustifyPopup : Injectable
    {
        public CVJustifyPopupResult? Result { get; private set; } = null;

        private CVJustifyPopup()
        {
            InitializeComponent();
        }

        public CVJustifyPopup(Event @event)
        {
            InitializeComponent();
            this.DataContext = @event;

            foreach (var item in Client.AllowedGiustifications)
            {
                this.wp.Children.Add(new RadioButton()
                {
                    Content = item.Desc,
                    Tag = item.Code,
                });
                this.wp.Children.Add(new Line());
            }

            ((RadioButton)this.wp.Children[0]).IsChecked = true;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            this.Close();

            var code = this.wp.Children.OfType<RadioButton>().Where(x => x.IsChecked is true).First().Tag.ToString()!;

            this.Result = new(code, this.Desc.Text);
        }
    }
}
