﻿using ClasseVivaWPF.Themes;
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
using ClasseVivaWPF.Themes.Handling;

namespace ClasseVivaWPF.HomeControls.Icons
{
    /// <summary>
    /// Logica di interazione per CVMainBadgeIcon.xaml
    /// </summary>
    public partial class CVMainBadgeIcon : CVBaseIcon
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

                    this.Path1.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path1.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    
                    this.Path2.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path2.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);

                    this.Path3.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path3.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);

                    this.Path4.SetThemeBinding(Path.FillProperty, ThemeProperties.CVMainMenuIconSelectedProperty);
                    this.Path4.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);

                    this.Path5.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);

                    this.Path6.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                }
                else
                {
                    this.Top.SetThemeBinding(Rectangle.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                    this.Desc.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);

                    this.Path1.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                    this.Path1.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);

                    this.Path2.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                    this.Path2.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);

                    this.Path3.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                    this.Path3.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);

                    this.Path4.SetThemeBinding(Path.FillProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
                    this.Path4.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);
                    
                    this.Path5.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);
                    
                    this.Path6.SetThemeBinding(Path.StrokeProperty, ThemeProperties.CVMainMenuIconUnselectedProperty);
                }

            }
        }

        public CVMainBadgeIcon() : base(CVMainMenuIconValues.Badge)
        {
            InitializeComponent();
        }
    }
}
