using System;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{
    class DivisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var divisor = System.Convert.ToDouble(parameter);
            if (divisor == 0)
                return "";

            return (double)value / divisor;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
