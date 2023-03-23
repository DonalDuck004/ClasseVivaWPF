using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Converters
{
    class ToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
