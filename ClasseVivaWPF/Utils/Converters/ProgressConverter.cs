using ClasseVivaWPF.SharedControls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ClasseVivaWPF.Utils.Converters
{
    public class ProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var w = (double)values[1];

            // W : x = 100 : P 
            if (w == 0 || w is double.NaN)
                return 0D;

            var divisor = values[2] is CVPeriodPercentage bar ? (bar.Max - bar.Min) : ((double)values[4] - (double)values[3]);

            var progress = (double)values[0] * w / divisor;

            return progress;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
