using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils.Converters;
using Newtonsoft.Json.Linq;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Graphs
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

        private string _StringFormat = "0.0";

        public string StringFormat
        {
            get => this._StringFormat;
            set
            {
                if (value == this._StringFormat)
                    return;

                this._StringFormat = value;
                if (!this.IsLoaded)
                    this.Loaded += SetStringFormat;
                else
                    this.UpdateTextBinding();
            }
        }

        private void UpdateTextBinding()
        {
            var binding = new Binding()
            {
                Path = new(CVColumn.ValueProperty),
                Converter = new StrCoalesceConverter(),
                ConverterParameter = "",
                StringFormat = this.StringFormat
            };

            BindingOperations.SetBinding(this.Header, TextBlock.TextProperty, binding);

        }

        public string? SubGroupName { get; init; } = null;

        static CVColumn()
        {
            LongDescProperty = DependencyProperty.Register("LongDesc", typeof(string), typeof(CVColumn));
        }

        public IEnumerable<double>? Values = null;
        public object? ContentID { get; init; } = null;
        private Action<CVColumn>? PostLoad = null;

        public CVColumn(Action<CVColumn>? PostLoad = null)
        {
            this.Loaded += OnLoad;
            this.DataContext = this;
            this.PostLoad = PostLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            InitializeComponent();
            // this.Background = new SolidColorBrush(Color.FromRgb((byte)new Random().Next(256), (byte)new Random().Next(256), (byte)new Random().Next(256)));
            if (this.PostLoad is not null)
            {
                this.PostLoad.Invoke(this);
                this.PostLoad = null;
            }

            this.Loaded -= OnLoad;
        }

        private void SetStringFormat(object sender, EventArgs e)
        {
            this.UpdateTextBinding();

            this.Loaded -= SetStringFormat;
        }
    }
}
