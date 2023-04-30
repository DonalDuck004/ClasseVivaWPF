using ClasseVivaWPF.SharedControls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ClasseVivaWPF.Utils.Converters
{
    public class StrFormatIfNotEmpty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null || value.ToString() == "" ? "" : string.Format((string)parameter, value.ToString());
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
