using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xaml.Schema;

namespace ClasseVivaWPF.Utils.Converters
{
    class ThicknessConverter : IValueConverter
    {
        private readonly string[] Allowed = { "Top", "Left", "Right", "Bottom" };

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is null)
                return null;

            var @params = parameter.ToString()!.Split("|");
            Debug.Assert(@params.Where(x => Allowed.Contains(x)).Any());
            var t = new Thickness();

            if (@params.Contains("Left"))
                t.Left = (double)value;
            
            if (@params.Contains("Top"))
                t.Top = (double)value;

            if (@params.Contains("Right"))
                t.Right = (double)value;

            if (@params.Contains("Right"))
                t.Right = (double)value;

            return t;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}
