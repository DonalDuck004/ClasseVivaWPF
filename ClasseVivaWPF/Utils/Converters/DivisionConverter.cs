﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Converters
{
    class DivisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var divisor = double.Parse(parameter.ToString()!, CultureInfo.InvariantCulture);
            if (divisor == 0)
                return 0;

            return (double)value / divisor;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
