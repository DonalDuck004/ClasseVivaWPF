using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
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

namespace ClasseVivaWPF.HomeControls
{
    /// <summary>
    /// Logica di interazione per CVMainMenuIcon.xaml
    /// </summary>
    public partial class CVMainMenuIcon : CVBaseIcon
    {
        public override bool IsSelected
        {
            set
            {
                base.IsSelected = value;

                if (value)
                {
                    this.Top.SetThemeBinding(Rectangle.FillProperty, BaseTheme.CV_GENERIC_RED_PATH);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_GENERIC_RED_PATH);
                    this.Path1.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_GENERIC_RED_PATH);
                    this.Path1.SetThemeBinding(Path.FillProperty, BaseTheme.CV_GENERIC_RED_PATH);
                }
                else
                {
                    this.Top.SetThemeBinding(Rectangle.FillProperty, BaseTheme.CV_GENERIC_OPAQUE_BACKGROUND_PATH);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_GENERIC_GRAY_PATH);
                    this.Path1.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_GENERIC_GRAY_PATH);
                    this.Path1.SetThemeBinding(Path.FillProperty, BaseTheme.CV_GENERIC_OPAQUE_BACKGROUND_PATH);
                }

            }
        }

        public CVMainMenuIcon() : base(CVMainMenuIconValues.Home)
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
