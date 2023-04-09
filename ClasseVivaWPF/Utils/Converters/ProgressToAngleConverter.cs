using ClasseVivaWPF.SharedControls;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{

    public class ProgressToAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double.NaN)
                return 0D;

            var progress = (double)values[0];
            var bar = (CVProgressEllipse)values[1];

            return 359.999 * (progress / (bar.Max - bar.Min));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
