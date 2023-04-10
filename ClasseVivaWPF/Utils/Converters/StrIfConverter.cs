using ClasseVivaWPF.SharedControls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ClasseVivaWPF.Utils.Converters
{
    public class StrIfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // |x|Text1|Text2|
            //  ^ sep
            if (value is not bool cond || parameter is not string str)
                return "";

            var sep = str[0];
            var r = str.Substring(1).Split(sep);

            return cond ? r[0] : r[1] ;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
