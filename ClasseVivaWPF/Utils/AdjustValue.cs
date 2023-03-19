using System;
using System.Windows.Data;

namespace ClasseVivaWPF
{
    class AdjustValue : IValueConverter
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
