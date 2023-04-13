using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using QRCoder;
using QRCoder.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

namespace ClasseVivaWPF.HomeControls.BadgeSection
{
    /// <summary>
    /// Logica di interazione per CVBadge.xaml
    /// </summary>
    public partial class CVBadge : UserControl, IOnSwitch
    {
        public CVBadge()
        {
            InitializeComponent();
            var x = new QRCodeGenerator();
            var qr = x.CreateQrCode("Test", QRCodeGenerator.ECCLevel.L);

            this.UpdateHighlight(0);

            this.QR.Background = new ImageBrush(new XamlQRCode(qr).GetGraphic(32))
            {
                Stretch = Stretch.Uniform,
            };
            this.BR.Text = "*" + SessionHandler.Me!.Id.ToString() + "*";

            this.Scroller.SizeChanged += (s, e) => {
                if (this.Scroller.HorizontalOffset != 0)
                    Scroller.ScrollToHorizontalOffset(this.ActualWidth * 2);
            };

        }

        public string Prepare(string Str)
        {

            int start = 104;
            int end = 106;
            int calc = start;
            var Barcode = start.ToString();
            for (var i = 0; i < Str.Length; i++)
            {
                calc += (Convert.ToChar(Str[i]) - 32) * (i + 1);
                Barcode += Str[i];
            }

            double rem = calc % 103;
            Barcode += Convert.ToChar((int)rem + 32).ToString() + end;

            return Barcode;

        }

        public void OnSwitch()
        {

        }

        private void OnSectionClick(object sender, MouseButtonEventArgs e)
        {
            var to = (int)(Grid.GetColumn((UIElement)sender) * ((FrameworkElement)sender).ActualWidth * 2);
            if (to == this.Scroller.HorizontalOffset)
                return;

            this.Scroller.AnimateScrollerH(this.Scroller.HorizontalOffset, to, 0.02).Start();

            var idx = this.labels.Children.ReferenceIndexOf(sender);
            UpdateHighlight(idx);
        }

        private void Scroller_OnSnap(object sender, SnapEventArgs e)
        {
            UpdateHighlight(e.Index);
        }
        
        private void UpdateHighlight(int idx)
        {
            ((Label)labels.Children[idx]).SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MULTI_MENU_FONT_SELECTED_PATH);
            ((Label)labels.Children[++idx % 2]).SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MULTI_MENU_FONT_UNSELECTED_PATH);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
