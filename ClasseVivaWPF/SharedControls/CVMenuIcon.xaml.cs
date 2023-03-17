using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        public CVMenuIconValues Type { get; private set; }

        private bool _IsSelected = false;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                if (value)
                {
                    this.Desc.Foreground = this.Top.Fill = new SolidColorBrush(Colors.Red);
                    foreach (var item in INSTANCES.Values)
                        if (!ReferenceEquals(item, this) && item.IsSelected)
                            item.IsSelected = false;

                    this.Desc.BeginAnimation(Label.FontSizeProperty, new DoubleAnimation(10, 15, new Duration(TimeSpan.FromMilliseconds(150))));
                }
                else
                {
                    this.Desc.BeginAnimation(Label.FontSizeProperty, new DoubleAnimation(15, 10, new Duration(TimeSpan.FromMilliseconds(150))));
                    this.Top.Fill = new SolidColorBrush(Colors.Transparent);
                    this.Desc.Foreground = new SolidColorBrush(Colors.Gray);
                }

            }

        }

        private CVMenuIcon(CVMenuIconValues type)
        {
            InitializeComponent();
            this.Type = type;
            this.Desc.Content = type.ToString();
        }

        public static CVMenuIcon New(CVMenuIconValues type)
        {
            if (!INSTANCES.ContainsKey(type))
                INSTANCES[type] = new(type);

            return INSTANCES[type];
        }

        private void OnSelect(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = true;
        }
    }
}
