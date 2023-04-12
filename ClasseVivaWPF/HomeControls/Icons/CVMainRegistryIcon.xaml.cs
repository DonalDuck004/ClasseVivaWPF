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
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;

namespace ClasseVivaWPF.HomeControls.Icons
{
    /// <summary>
    /// Logica di interazione per CVMainRegistryIcon.xaml
    /// </summary>
    public partial class CVMainRegistryIcon : CVBaseIcon
    {

        public override bool IsSelected
        {
            set
            {
                base.IsSelected = value;

                if (value)
                {
                    this.Top.SetThemeBinding(Rectangle.FillProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
                    this.Path1.SetThemeBinding(Path.FillProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
                    this.Path1.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
                    this.Path2.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
                    this.Path3.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
                }
                else
                {
                    this.Top.SetThemeBinding(Rectangle.FillProperty, BaseTheme.CV_GENERIC_OPAQUE_BACKGROUND_PATH);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MAIN_MENU_ICON_UNSELECTED_PATH);
                    this.Path1.SetThemeBinding(Path.FillProperty, BaseTheme.CV_MAIN_MENU_ICON_UNSELECTED_PATH);
                    this.Path1.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_MAIN_MENU_ICON_UNSELECTED_PATH);
                    this.Path2.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_MAIN_MENU_ICON_UNSELECTED_PATH);
                    this.Path3.SetThemeBinding(Path.StrokeProperty, BaseTheme.CV_MAIN_MENU_ICON_UNSELECTED_PATH);
                }

            }
        }

        public CVMainRegistryIcon() : base(CVMainMenuIconValues.Registro)
        {
            InitializeComponent();

            this.Path2.SetThemeBinding(Path.FillProperty, BaseTheme.CV_GENERIC_OPAQUE_BACKGROUND_PATH);
            this.Path3.SetThemeBinding(Path.FillProperty, BaseTheme.CV_GENERIC_OPAQUE_BACKGROUND_PATH);
        }
    }
}
