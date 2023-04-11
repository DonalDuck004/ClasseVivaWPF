using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ClasseVivaWPF.Utils.Converters
{
    public class CounterConverter : MarkupExtension, IValueConverter
    {
        public double StartFrom { get; set; }
        public double Increment { get; set; }
        private double? Counter { get; set; } = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is false)
            {
                this.Counter = this.StartFrom;
                return 0D;
            }

            if (this.Counter is null)
                this.Counter = this.StartFrom;

            this.Counter += this.Increment;

            return this.Counter;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
