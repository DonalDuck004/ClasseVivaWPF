using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup;

namespace ClasseVivaWPF
{
    class ColorIfConverter : MarkupExtension, IValueConverter
    {
        public Color FalseColor { get; set; } = Colors.Transparent;
        public Color TrueColor { get; set; } = Colors.White;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((bool)value ? TrueColor : FalseColor);
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
