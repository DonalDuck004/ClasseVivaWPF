using ClasseVivaWPF.SharedControls;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection
{
    /// <summary>
    /// Logica di interazione per CVColum.xaml
    /// </summary>
    public partial class CVColumn : BaseCVPercentage
    {
        public static readonly DependencyProperty LongDescProperty;

        public string? LongDesc
        {
            get => (string)GetValue(LongDescProperty);
            set => SetValue(LongDescProperty, value);
        }

        public string? SubGroupName { get; init; } = null;

        static CVColumn()
        {
            LongDescProperty = DependencyProperty.Register("LongDesc", typeof(string), typeof(CVColumn));
        }

        public IEnumerable<double>? Values = null;
        public object? ContentID { get; init; } = null;

        public CVColumn()
        {
            this.Loaded += OnLoad;
            this.DataContext = this;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            InitializeComponent();

            this.Loaded -= OnLoad;
        }

    }
}
