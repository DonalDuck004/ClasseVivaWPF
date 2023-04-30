using ColorPicker.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Converters
{
    class ToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Color x)
                return new SolidColorBrush(x);

            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
