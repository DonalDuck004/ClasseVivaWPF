using ClasseVivaWPF.Sessions;
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

namespace ClasseVivaWPF.HomeControls
{
    /// <summary>
    /// Logica di interazione per CVAccount.xaml
    /// </summary>
    public partial class CVAccount : UserControl
    {
        public static CVAccount? Selected { get; private set; } = null;
        public static readonly DependencyProperty IsSelectedProperty;

        public event EventHandler? OnSelect = null;
        public AccountMeta Account { get; init; }

        public bool IsSelected
        {
            get => (bool)this.GetValue(IsSelectedProperty);
            set
            {
                if (value && !ReferenceEquals(CVAccount.Selected, this)) {
                    if (CVAccount.Selected is not null)
                        CVAccount.Selected.IsSelected = false;

                    CVAccount.Selected = this;
                }

                this.SetValue(IsSelectedProperty, value);

                if (value && this.OnSelect is not null)
                    this.OnSelect(this, EventArgs.Empty);
            }
        }

        static CVAccount()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVAccount), new PropertyMetadata(false));
        }


        public CVAccount()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnSelected(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = true;
        }
    }
}
