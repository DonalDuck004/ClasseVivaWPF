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

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVExtra.xaml
    /// </summary>
    public partial class CVExtra : UserControl, IOnKeyDown, ICloseRequested, IOnChildClosed
    {
        public static CVExtra? INSTANCE = null;

        public CVExtra()
        {
            CVExtra.INSTANCE = this;
            InitializeComponent();
        }

        public void Destroy()
        {
            CVExtraHeader.DestroyAll();
        }

        public void OnCloseRequested()
        {
            this.Destroy();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }

        public void SetContent(Grid grid)
        {
            this.content_wp.Content = grid;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.SetContent(((CVExtraHeader)h_grid.Children[0]).GContent);

            this.Loaded -= OnLoad;
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            content_wp.RaiseEvent(e);
        }

        public void OnChildClosed()
        {
            if (CVExtraHeader.SavedUpdated && CVExtraHeader.SelectedH!.HeaderText == CVExtraHeader.NAMES[CVExtraHeader.NAMES.Length - 1])
            {
                CVExtraHeader.SelectedH!.IsSelected = true;
            }
        }
    }
}
