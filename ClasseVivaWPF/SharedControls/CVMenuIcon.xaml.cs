using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per cvmENUiCON.xaml
    /// </summary>
    /// 
    public enum CVMenuIconValues
    {
        Home,
        Registro,
        Camera,
        Badge,
        Menu
    }

    public partial class CVMenuIcon : UserControl
    {
        public static Dictionary<CVMenuIconValues, CVMenuIcon> INSTANCES = new();
        public static CVMenuIcon? Selected = null;
        private CVMainNavigation Navigation;
        public CVMenuIconValues Type { get; private init; }
        public int ParentIdx => (int)Type;

        public bool IsSelected
        {
            get => ReferenceEquals(this, Selected);
            set
            {
                if (value)
                {
                    if (Selected is not null)
                        Selected.IsSelected = false;

                    this.Desc.Foreground = this.Top.Fill = new SolidColorBrush(Colors.Red);
                    Selected = this;
                    this.Desc.BeginAnimation(Label.FontSizeProperty, new DoubleAnimation(10, 15, new Duration(TimeSpan.FromMilliseconds(150))));
                    this.Navigation.SelectVoice(this.ParentIdx);
                }
                else
                {
                    this.Desc.BeginAnimation(Label.FontSizeProperty, new DoubleAnimation(15, 10, new Duration(TimeSpan.FromMilliseconds(150))));
                    this.Top.Fill = new SolidColorBrush(Colors.Transparent);
                    this.Desc.Foreground = new SolidColorBrush(Colors.Gray);
                }

            }

        }

#if DEBUG
        private CVMenuIcon()
        {

        }
#endif

        private CVMenuIcon(CVMenuIconValues type, CVMainNavigation parent)
        {
            InitializeComponent();
            this.Type = type;
            this.Desc.Content = type.ToString();
            this.Navigation = parent;
        }

        public static CVMenuIcon New(CVMenuIconValues type, CVMainNavigation parent)
        {
            if (!INSTANCES.ContainsKey(type))
                INSTANCES[type] = new(type, parent);

            return INSTANCES[type];
        }

        private void OnSelect(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = true;
        }
    }
}
