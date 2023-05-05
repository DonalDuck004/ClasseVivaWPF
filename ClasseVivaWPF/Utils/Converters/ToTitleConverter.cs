using System;
using System.Globalization;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{
    class ToTitleConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString()?.ToTitle();
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
