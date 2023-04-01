using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClasseVivaWPF.Utils.Converters
{
    class CVMemeBackgroundConverter : IValueConverter
    {
        private static Dictionary<string, SolidColorBrush> cache = new Dictionary<string, SolidColorBrush>();

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
            var rect = new Int32Rect(0, 0, img.PixelWidth, img.PixelHeight / 6);
            var stride = (rect.Width * 32 + 7) / 8;

            var buffer = new byte[stride * rect.Height];
            img.CopyPixels(rect, buffer, stride, 0);

            var md5 = buffer.GetMD5();
            if (cache.ContainsKey(md5))
                return cache[md5];


            var buff = new byte[buffer.Length / 4][];

            for (int i = 0, j = 0; i < buffer.Length; i += 4, j++)
            {
                buff[j] = new byte[4];
                Array.Copy(buffer, i, buff[j], 0, buff[j].Length);
            }

            var x = buff.GroupBy(x => x, g => 1, (k, g) => (k, g.Count())).MaxBy(x => x.Item2)!.k;

            return cache[md5] = new SolidColorBrush(new Color()
            {
                B = x[0],
                G = x[1],
                R = x[2],
                A = x[3]
            });
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }

    }
}
