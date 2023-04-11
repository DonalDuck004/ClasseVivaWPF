using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF.HomeControls
{
    public class CVBaseIcon : UserControl
    {
        public static readonly DependencyProperty IconValueProperty;
        public static readonly DependencyProperty IsSelectedProperty;

        public static Dictionary<CVMainMenuIconValues, CVBaseIcon> INSTANCES = new();
        public static CVBaseIcon? Selected = null;

        static CVBaseIcon()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVBaseIcon), new PropertyMetadata(false));
            IconValueProperty = DependencyProperty.Register("IconValue", typeof(CVMainMenuIconValues), typeof(CVBaseIcon));
        }

        private CVBaseIcon()
        {

            /*
            Loaded += (s, e) =>
            {
                if (INSTANCES.ContainsKey(IconValue))
                    throw new Exception();

                INSTANCES[IconValue] = this;

                if (IconValue is CVMainMenuIconValues.Home)
                    IsSelected = true;
            };*/
        }

        internal CVBaseIcon(CVMainMenuIconValues Icon)
        {
            this.IconValue = Icon;
            this.MouseLeftButtonDown += (s, e) =>
            {
                if (!this.IsSelected)
                    this.IsSelected = !this.IsSelected;
            };
            this.Loaded += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.IsSelected = this.IsSelected;
            this.Loaded -= OnLoad;
        }

        public CVMainMenuIconValues IconValue
        {
            get => (CVMainMenuIconValues)GetValue(IconValueProperty);
            set
            {
                SetValue(IconValueProperty, value);
            }
        }

        public virtual bool IsSelected
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
