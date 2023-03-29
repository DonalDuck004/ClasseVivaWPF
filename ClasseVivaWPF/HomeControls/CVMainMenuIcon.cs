using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF.HomeControls
{
    public class CVMainMenuIcon : ContentControl
    {
        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty IconValueProperty;
        public static readonly DependencyProperty IsSelectedProperty;

        public static Dictionary<CVMainMenuIconValues, CVMainMenuIcon> INSTANCES = new();
        public static CVMainMenuIcon? Selected = null;

        static CVMainMenuIcon()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVMainMenuIcon), new PropertyMetadata(false));
            IconProperty = DependencyProperty.Register("Icon", typeof(ControlTemplate), typeof(CVMainMenuIcon));
            IconValueProperty = DependencyProperty.Register("IconValue", typeof(CVMainMenuIconValues), typeof(CVMainMenuIcon));
        }

        public CVMainMenuIcon()
        {
            MouseLeftButtonDown += (s, e) =>
            {
                if (!IsSelected)
                    IsSelected = !IsSelected;
            };
            Loaded += (s, e) =>
            {
                if (INSTANCES.ContainsKey(IconValue))
                    throw new Exception();

                INSTANCES[IconValue] = this;

                if (IconValue is CVMainMenuIconValues.Home)
                    IsSelected = true;

            };
        }

        public ControlTemplate Icon
        {
            get => (ControlTemplate)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public CVMainMenuIconValues IconValue
        {
            get => (CVMainMenuIconValues)GetValue(IconValueProperty);
            set
            {
                SetValue(IconValueProperty, value);
            }
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set
            {
                if (value)
                {
                    if (Selected is not null)
                        Selected.IsSelected = false;

                    Selected = this;
                    CVMainNavigation.INSTANCE!.SelectVoice(IconValue);
                }

                SetValue(IsSelectedProperty, value);
            }
        }
    }
}
