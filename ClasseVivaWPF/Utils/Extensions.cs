using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClasseVivaWPF.Utils
{
    static class Extensions
    {
        public static void Shuffle<T>(this List<T> src, int cycles = 2)
        {
            T item;
            int idx;

            for (int _ = 0; _ < cycles; _++)
            {
                for (int i = 0; i < src.Count; i++)
                {
                    item = src[i];
                    idx = Random.Shared.Next(src.Count);
                    src[i] = src[idx];
                    src[idx] = item;
                }
            }
        }

        public static void Shuffle<T>(this T[] src, int cycles = 2)
        {
            T item;
            int idx;

            for (int _ = 0; _ < cycles; _++)
            {
                for (int i = 0; i < src.Length; i++)
                {
                    item = src[i];
                    idx = Random.Shared.Next(src.Length);
                    src[i] = src[idx];
                    src[idx] = item;
                }
            }
        }

        public static bool IsUserVisible(this UIElement element)
        {
            if (!element.IsVisible)
                return false;

            var container = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (container is null) throw new ArgumentNullException("container");

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.RenderSize.Width, element.RenderSize.Height));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.IntersectsWith(bounds);
        }

        public static int AsInt32(this DayOfWeek self)
        {
            var result = ((int)self) - 1;
            return result == -1 ? 6 : result;
        }

        public static bool ContainsReference<T>(this IEnumerable<T> obj, T Needle)
        {
            foreach (var item in obj)
                if (ReferenceEquals(item, Needle))
                    return true;

            return false;
        }

        public static void SystemOpening(this Uri uri)
        {
            Process.Start(new ProcessStartInfo{
                    FileName = uri.ToString(),
                    UseShellExecute = true,
                    WorkingDirectory = Directory.GetCurrentDirectory()
            });
        }

        public static string ToTitle(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static Task AsyncLoading(this Image img, string url, Action? OnDone = null)
        {
            var task = Task.Run(() => Client.INSTANCE.Download(url).ContinueWith(
                                t => img.Dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        img.Source = new BitmapImage(t.Result);

                                        if (OnDone is not null)
                                            OnDone.Invoke();
                                    }
                                )
                            )
                        );
            return task;
        }

        public static Task AsyncImageLoading(this FrameworkElement img, string url, Action? OnDone = null)
        {
            var task = Task.Run(() => Client.INSTANCE.Download(url).ContinueWith(
                                t => img.Dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        var b = new BitmapImage(t.Result);
                                        img.SetValue(Panel.BackgroundProperty, new ImageBrush(b)
                                        {
                                            Stretch = Stretch.UniformToFill,
                                        });
                                        img.SetValue(Panel.SnapsToDevicePixelsProperty, true);
                                        

                                        if (OnDone is not null)
                                            OnDone.Invoke();
                                    }
                                )
                            )
                        );
            return task;
        }
    }
}
