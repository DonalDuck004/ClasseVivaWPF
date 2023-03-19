using ClasseVivaWPF.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVMainNavigation.xaml
    /// </summary>
    public partial class CVMainNavigation : UserControl
    {
        public CVMainNavigation()
        {
            InitializeComponent();
            CVMenuIcon tmp;

            var c = 0;
            foreach (CVMenuIconValues item in Enum.GetValues(typeof(CVMenuIconValues)))
            {
                tmp = CVMenuIcon.New(item, parent: this);
                this.wp_buttons.ColumnDefinitions.Add(new());
                this.wp_buttons.Children.Add(tmp);
                Grid.SetColumn(tmp, c++);
            }
        }

        internal void SelectVoice(int idx)
        {
            if (idx == 0)
            {
                Current.Children.Clear();
                Current.Children.Add(CVHome.INSTANCE);
            }
        }
    }
}
