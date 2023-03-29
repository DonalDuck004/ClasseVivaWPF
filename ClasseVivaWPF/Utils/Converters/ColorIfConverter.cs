using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Converters
{
    public class ColorIfConverter : MarkupExtension, IValueConverter
    {
        public Color FalseColor { get; set; } = Colors.Transparent;
        public Color TrueColor { get; set; } = Colors.White;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool swap = (string)parameter == "swap";

            bool cond;
            if (swap)
                cond = !(bool)value;
            else
                cond = (bool)value;

            return new SolidColorBrush(cond ? TrueColor : FalseColor);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
