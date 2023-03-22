using ClasseVivaWPF.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF
{
    public class CVMainMenuIcon : ContentControl
    {
        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty IconValueProperty;
        public static readonly DependencyProperty IsSelectedProperty;

        public static Dictionary<CVMenuIconValues, CVMainMenuIcon> INSTANCES = new();
        public static CVMainMenuIcon? Selected = null;
        public int ParentIdx => (int)IconValue;

        static CVMainMenuIcon()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVMainMenuIcon), new PropertyMetadata(false));
            IconProperty = DependencyProperty.Register("Icon", typeof(ControlTemplate), typeof(CVMainMenuIcon));
            IconValueProperty = DependencyProperty.Register("IconValue", typeof(CVMenuIconValues), typeof(CVMainMenuIcon));
        }

        public CVMainMenuIcon()
        {
            this.MouseLeftButtonDown += (s, e) => this.IsSelected = !this.IsSelected;
            this.Loaded += (s, e) => {
                if (INSTANCES.ContainsKey(this.IconValue))
                    throw new Exception();

                CVMainMenuIcon.INSTANCES[this.IconValue] = this;

                if (this.IconValue is CVMenuIconValues.Home)
                    this.IsSelected = true;

            };
        }

        public ControlTemplate Icon
        {
            get => (ControlTemplate)base.GetValue(IconProperty);
            set => base.SetValue(IconProperty, value);
        }

        public CVMenuIconValues IconValue
        {
            get => (CVMenuIconValues)base.GetValue(IconValueProperty);
            set {
                base.SetValue(IconValueProperty, value);
            }
        }

        public bool IsSelected
        {
            get => (bool)base.GetValue(IsSelectedProperty);
            set
            {
                if (value)
                {
                    if (CVMainMenuIcon.Selected is not null)
                        CVMainMenuIcon.Selected.IsSelected = false;

                    CVMainMenuIcon.Selected = this;
                    CVMainNavigation.INSTANCE!.SelectVoice(this.ParentIdx);
                }

                base.SetValue(IsSelectedProperty, value);
            }
        }
    }
}
