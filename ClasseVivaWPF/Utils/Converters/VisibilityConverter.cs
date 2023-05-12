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
            var c = System.Convert.ToBoolean(value);
            switch (parameter?.ToString())
            {
                case null:
                case "":
                    return c ? Visibility.Visible : Visibility.Hidden;
                case "swap":
                    return c ? Visibility.Hidden : Visibility.Visible;
                case "collapse":
                    return c ? Visibility.Visible : Visibility.Collapsed;
                case "collapse|swap":
                    return c ? Visibility.Collapsed : Visibility.Visible;
            }

            throw new NotImplementedException(parameter?.ToString());
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
