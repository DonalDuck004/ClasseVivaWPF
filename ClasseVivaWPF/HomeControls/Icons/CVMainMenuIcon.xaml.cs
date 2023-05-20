using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Themes;
using ClasseVivaWPF.Themes.Handling;
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

namespace ClasseVivaWPF.HomeControls.Icons
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
                    this.Top.SetThemeBinding(Rectangle.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMainMenuIconSelectedProperty);

                    this.Path1.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path1.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);

                    this.Path2.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path2.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);

                    this.Path3.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path3.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                }
                else
                {
                    this.Top.SetThemeBinding(Rectangle.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);

                    this.Path1.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);
                    this.Path1.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);

                    this.Path2.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);
                    this.Path2.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);

                    this.Path3.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);
                    this.Path3.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                }

            }
        }

        public CVMainMenuIcon() : base(CVMainMenuIconValues.Menu)
        {
            InitializeComponent();
        }
    }
}
