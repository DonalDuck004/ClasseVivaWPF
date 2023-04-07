using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{
    class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter?.ToString() == "swap")
                return (bool)value ? Visibility.Hidden : Visibility.Visible;

            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
