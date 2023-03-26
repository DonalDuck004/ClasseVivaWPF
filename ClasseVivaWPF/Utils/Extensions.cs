using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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

        public static bool IsUserVisible(this FrameworkElement element, FrameworkElement? container = null)
        {
            if (!element.IsVisible)
                return false;

            container ??= VisualTreeHelper.GetParent(element) as FrameworkElement;
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

        private static MD5 cacher = MD5.Create();

        private static bool Donwloaded(string url, out string save_path)
        {
            var md5 = Convert.ToHexString(cacher.ComputeHash(Encoding.UTF8.GetBytes(url))) + Path.GetExtension(url);
            save_path = Path.Join(Path.GetTempPath(), md5);

            return File.Exists(save_path);
        }

        public static Task? AsyncLoading(this Image img, string url, Action? OnDone = null)
        {
            if (Donwloaded(url, out string save_path))
                Set(new Uri(save_path));
            else
            {
                Set(new Uri("pack://application:,,,/Assets/Images/placeholder.jpg"), done: false);
                return Task.Run(() => Client.INSTANCE.Download(url, save_path).ContinueWith(
                        t => img.Dispatcher.BeginInvoke(
                            () => Set(new(save_path))
                        )
                    )
                );
            }

            return null;

            void Set(Uri img_path, bool done = true)
            {
                img.Dispatcher.BeginInvoke(
                    () =>
                    {
                        img.Source = new BitmapImage(img_path);
                        if (done && OnDone is not null)
                            OnDone.Invoke();
                    }
                    );
            }
        }

        public static Task? AsyncImageLoading(this FrameworkElement img, string url, Action? OnDone = null)
        {

            if (Donwloaded(url, out string save_path))
                Set(new Uri(save_path));
            else
            {
                Set(new Uri("pack://application:,,,/Assets/Images/placeholder.jpg"), done: false);
                return Task.Run(
                        () => Client.INSTANCE.Download(url, save_path).ContinueWith(
                            t => img.Dispatcher.BeginInvoke(() => Set(new(save_path))
                            )
                        )
                    );
            }

            return null;

            void Set(Uri img_path, bool done = true)
            {
                img.Dispatcher.BeginInvoke(
                    () =>
                    {
                        var b = new BitmapImage(img_path);
                        img.SetValue(Panel.BackgroundProperty, new ImageBrush(b)
                        {
                            Stretch = Stretch.UniformToFill,
                        });
                        img.SetValue(Panel.SnapsToDevicePixelsProperty, true);


                        if (done && OnDone is not null)
                            OnDone.Invoke();
                    }
                    );
            }
        }

        public static DispatcherTimer AnimateScrollerH(this ScrollViewer sc, double from, double to, double duration)
        {
            
            if (sc.Tag is DispatcherTimer old_dp)
                old_dp.IsEnabled = false;

            DispatcherTimer dp;
            var inc = from < to;

            sc.Tag = dp = new()
            {
                Interval = TimeSpan.FromSeconds(duration / Math.Abs(from - to)) * 2
            };

            dp.Tick += (s, e) => {
                if ((inc && from++ > to) || (!inc && to > --from))
                {
                    ((DispatcherTimer)s!).IsEnabled = false;
                    return;
                }

                if (inc)
                    from++;
                else 
                    from--;
                
                sc.ScrollToHorizontalOffset(from);
            }; // It's not animable

            return dp;
        }
    }
}
