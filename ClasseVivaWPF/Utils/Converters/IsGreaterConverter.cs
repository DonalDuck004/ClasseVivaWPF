using ClasseVivaWPF.SharedControls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ClasseVivaWPF.Utils.Converters
{
    public class IsGreaterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(values[0]) > System.Convert.ToDouble(values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
