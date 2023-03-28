using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
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
using System.Windows.Input;
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
         
        public static string GetMD5(this byte[] buff)
        {
            return Convert.ToHexString(cacher.ComputeHash(buff));
        }

        private static bool Downloaded(string url, out string save_path)
        {
            var md5 = Encoding.UTF8.GetBytes(url).GetMD5() + Path.GetExtension(url);
            save_path = Path.Join(Path.GetTempPath(), md5);

            return File.Exists(save_path);
        }

        public static Task? AsyncLoading(this Image img, string url, Action? OnDone = null, int? DecodePixelHeight = null, int? DecodePixelWidth = null)
        {
            if (Downloaded(url, out string save_path))
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
                        if (DecodePixelHeight is not null || DecodePixelWidth is not null)
                        {
                            var tmp = new BitmapImage();
                            tmp.BeginInit();
                            if (DecodePixelWidth is not null)
                                tmp.DecodePixelWidth = DecodePixelWidth.Value;
                            if (DecodePixelHeight is not null)  
                                tmp.DecodePixelHeight = DecodePixelHeight.Value;

                            tmp.StreamSource = new FileStream(img_path.AbsolutePath, FileMode.Open, FileAccess.Read);
                            tmp.EndInit();
                            img.Source = tmp;
                        }else
                            img.Source = new BitmapImage(img_path);


                        if (done && OnDone is not null)
                            OnDone.Invoke();
                    }
                    );
            }
        }

        public static Task? AsyncImageLoading(this FrameworkElement img, string url, Action? OnDone = null, int? DecodePixelHeight = null, int? DecodePixelWidth = null)
        {

            if (Downloaded(url, out string save_path))
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
                        BitmapImage bg;
                        if (DecodePixelHeight is not null || DecodePixelWidth is not null)
                        {
                            bg = new();
                            bg.BeginInit();
                            if (DecodePixelWidth is not null)
                                bg.DecodePixelWidth = DecodePixelWidth.Value;
                            if (DecodePixelHeight is not null)
                                bg.DecodePixelHeight = DecodePixelHeight.Value;

                            bg.StreamSource = new FileStream(img_path.AbsolutePath, FileMode.Open, FileAccess.Read);
                            bg.EndInit();
                        }
                        else
                            bg = new(img_path);


                        img.SetValue(Panel.BackgroundProperty, new ImageBrush(bg)
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

        public static void SetOpener(this FrameworkElement img, bool on_release = false)
        {
            if (on_release)
                img.MouseLeftButtonUp += OpenPage;
            else
                img.MouseLeftButtonDown += OpenPage;
        }

        public static void UnSetOpener(this FrameworkElement img, bool on_release = false)
        {
            if (on_release)
                img.MouseLeftButtonUp -= OpenPage;
            else
                img.MouseLeftButtonDown -= OpenPage;
        }

        private static void OpenPage(object sender, MouseButtonEventArgs e)
        {
            var content = (Content)((FrameworkElement)sender).Tag;
            // new OpenUriInfo(content.OpensExternally, content.Link is null ? null : new Uri(content.Link), content.ContentID, content.Type)
            if (content.OpensExternally)
                new Uri(content.Link!).SystemOpening();
            else if (content.Type == Content.TYPE_POPFESSORI)
            {
                new CVPopfessoriOpener(content.ContentID)
                {
                    Uri = new Uri(content.Link!)
                }.Inject();
            }
            else if (content.Type == Content.TYPE_PILLOLE)
            {
                new CVPilloleOpener(content).Inject();
            }
            else if (content.Type == Content.TYPE_MINIGAMES)
            {
                new CVMinigamesOpener(content).Inject();
            }
            else
                throw new Exception();
            // {{"id_contenuto":2014,"App":"CntExt","ordine":1,"Tags":null,"inizio":"2022-10-07T00:00:00","fine":"2022-10-07T23:59:00","scadenza":null,"tipo":"Home top","titolo":"Gioca e Impara","Link":"https://web.spaggiari.eu/gek/vimeoPlayer/683335479/5f8d999481","media_type":"video","PanoramicaImg":"https://web.spaggiari.eu/gek/storage/contenuti/contenuto-1665069889.jpg","PanoramicaPos":"top","Gallery":null,"content_detail":null,"Related":[],"opens_externally":false,"accessibility_label":null,"XCreationDate":"2023-03-27T18:42:39.8080766+02:00"}}
        }

        public static int ReferenceIndexOf<T>(this IEnumerable<T> container, T needle)
        {
            int i = 0;

            foreach (var item in container)
            {
                if (object.ReferenceEquals(item, needle))
                    return i;

                i++;
            }

            return -1;
        }
    }
}
