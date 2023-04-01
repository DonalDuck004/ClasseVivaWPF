using System;
using System.Globalization;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{
    class AddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var n2 = double.Parse(parameter.ToString()!, CultureInfo.InvariantCulture);

            return (double)value + n2;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
