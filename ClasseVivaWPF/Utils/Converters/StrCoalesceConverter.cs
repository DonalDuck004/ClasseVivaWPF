using ClasseVivaWPF.SharedControls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ClasseVivaWPF.Utils.Converters
{
    public class StrCoalesceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null || value is double.NaN ? parameter : value;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
