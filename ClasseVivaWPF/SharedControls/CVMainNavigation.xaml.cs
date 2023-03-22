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
        public static CVMainNavigation? INSTANCE = null;

        private CVMainNavigation()
        {
            InitializeComponent();
        }

        public static CVMainNavigation New()
        {
            if (CVMainNavigation.INSTANCE is null)
                CVMainNavigation.INSTANCE = new();

            return CVMainNavigation.INSTANCE;
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
