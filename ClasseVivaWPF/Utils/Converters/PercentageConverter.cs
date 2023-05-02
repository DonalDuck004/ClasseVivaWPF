using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{
    public class PercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[0] is double.NaN || values[1] is double.NaN || values[1] == (object)0)
                return 0D;

            return System.Convert.ToDouble(values[0]) / System.Convert.ToDouble(values[1]) * 100;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
