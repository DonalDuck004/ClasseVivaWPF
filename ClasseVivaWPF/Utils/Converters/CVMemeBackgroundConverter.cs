using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace ClasseVivaWPF.Utils.Converters
{
    class CVMemeBackgroundConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null)
                return null;

            if (value is BitmapImage img) ;
            else if (value is Uri)
                img = new((Uri)value);
            else
                throw new Exception();

            // 31 0 192 255

            var rect = new Int32Rect(/*32, 24*/0, 0, 1, 1);

            var stride = (rect.Width * img.Format.BitsPerPixel + 7) / 8; // here

            var buffer = new byte[stride * rect.Height];

            img.CopyPixels(rect, buffer, stride, 0);
            return new SolidColorBrush(new Color()
            {
                B = buffer[0],
                G = buffer[1],
                R = buffer[2],
                A = buffer[3]
            });
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }

    }
}
