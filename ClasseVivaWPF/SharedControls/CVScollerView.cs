using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace ClasseVivaWPF.SharedControls
{
    public class SnapEventArgs : RoutedEventArgs
    {
        public SnapEventArgs() : base(CVScollerView.OnSnapEvent)
        {

        }

        public required int Index { get; init; }
        public required FrameworkElement SnappendElement { get; init; }
    }

    public enum SnapDirections
    {
        Horizontal,
        Vertical
    }

    public class CVScollerView : DependencyObject
    {
        public static double GetSnapSensibility(DependencyObject obj)
        {
            return (double)obj.GetValue(SnapSensibilityProperty);
        }

        public static void SetSnapSensibility(DependencyObject obj, double value)
        {
            obj.SetValue(SnapSensibilityProperty, value);
        }

        public static bool GetSnap(DependencyObject obj)
        {
            return (bool)obj.GetValue(SnapProperty);
        }

        public static void SetSnap(DependencyObject obj, bool value)
        {
            obj.SetValue(SnapProperty, value);
        }

        public static SnapDirections GetSnapDirection(DependencyObject obj)
        {
            return (SnapDirections)obj.GetValue(SnapDirectionProperty);
        }

        public static void SetSnapDirection(DependencyObject obj, SnapDirections value)
        {
            obj.SetValue(SnapDirectionProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }
        public static bool GetCatchWidthProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(CatchWidthProperty);
        }

        public static void SetCatchWidthProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(CatchWidthProperty, value);
        }

        public static bool GetCatchHeightProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(CatchHeightProperty);
        }

        public static void SetCatchHeightProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(CatchHeightProperty, value);
        }


        public static double GetSpeed(DependencyObject obj)
        {
            return (double)obj.GetValue(SpeedProperty);
        }

        public static void SetSpeed(DependencyObject obj, double value)
        {
            obj.SetValue(SpeedProperty, value);
        }

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public bool CatchWidth
        {
            get => (bool)GetValue(CatchWidthProperty);
            set => SetValue(CatchWidthProperty, value);
        }


        public bool CatchHeight
        {
            get => (bool)GetValue(CatchHeightProperty);
            set => SetValue(CatchHeightProperty, value);
        }
        
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false, IsEnabledChanged));
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.RegisterAttached("Speed", typeof(double), typeof(CVScollerView), new UIPropertyMetadata(1D));
        public static readonly DependencyProperty CatchWidthProperty = DependencyProperty.RegisterAttached("CatchWidth", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty CatchHeightProperty = DependencyProperty.RegisterAttached("CatchHeight", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty SnapDirectionProperty = DependencyProperty.RegisterAttached("SnapDirection", typeof(SnapDirections), typeof(CVScollerView), new UIPropertyMetadata(SnapDirections.Horizontal));
        public static readonly DependencyProperty SnapProperty = DependencyProperty.RegisterAttached("Snap", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty SnapSensibilityProperty = DependencyProperty.RegisterAttached("SnapSensibility", typeof(double), typeof(CVScollerView), new UIPropertyMetadata(4D));
        public static readonly RoutedEvent OnSnapEvent = EventManager.RegisterRoutedEvent("OnSnap", RoutingStrategy.Bubble, typeof(OnSnapHandler), typeof(CVScollerView));
        public delegate void OnSnapHandler(object sender, SnapEventArgs e);

        static Dictionary<object, MouseCapture> _captures = new Dictionary<object, MouseCapture>();
        static Dictionary<ScrollViewer, int> _indexes = new Dictionary<ScrollViewer, int>();

        public static void AddOnSnapHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.AddHandler(OnSnapEvent, handler);
        }

        public static void RemoveOnSnapHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is not UIElement uiElement)
                return;

            uiElement.RemoveHandler(OnSnapEvent, handler);
        }

        public static int GetCurrentSnapIndex(ScrollViewer scroller)
        {
            if (!_indexes.ContainsKey(scroller))
                return 0;
            return _indexes[scroller];
        }

        static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ScrollViewer;
            if (target is null)
                return;

            if ((bool)e.NewValue)
                target.Loaded += target_Loaded;
            else
                target_Unloaded(target, new RoutedEventArgs());
        }

        static void target_Unloaded(object sender, RoutedEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;

            _captures.Remove(sender);

            // target.Loaded -= target_Loaded;
            target.Unloaded -= target_Unloaded;
            target.PreviewMouseLeftButtonDown -= target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove -= target_PreviewMouseMove;

            target.PreviewMouseLeftButtonUp -= target_PreviewMouseLeftButtonUp;
        }

        static void target_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;
            if (e.OriginalSource is Run)
                return;

            _captures[sender] = new MouseCapture()
            {
                VerticalOffset = target.VerticalOffset,
                HorizontalOffset = target.HorizontalOffset,
                Point = e.GetPosition(target)
            };
        }

        static void target_Loaded(object sender, RoutedEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;

            target.Unloaded += target_Unloaded;
            target.PreviewMouseLeftButtonDown += target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove += target_PreviewMouseMove;

            target.PreviewMouseLeftButtonUp += target_PreviewMouseLeftButtonUp;
        }

        static void target_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;

            if (GetSnap(target) && _captures.ContainsKey(sender))
            {
                var i = 0;
                var c = ((Panel)target.Content).Children;
                var h = 0D;
                var d = GetSnapDirection(target);
                double check;
                double o_check;

                double p_s;
                double req;
                double[] sizes;

                if (d is SnapDirections.Horizontal)
                {
                    p_s = target.ActualWidth;
                    check = target.HorizontalOffset;
                    o_check = _captures[target].HorizontalOffset;
                    sizes = c.OfType<FrameworkElement>().Select(x => x.ActualWidth).ToArray();
                    req = ((FrameworkElement)target.Parent).ActualWidth / GetSnapSensibility(target);
                    
                    for (; i < c.Count; i++)
                    {
                        h += ((FrameworkElement)c[i]).ActualWidth;
                        if (target.HorizontalOffset < h)
                        {
                            i++;
                            break;
                        }
                    }
                }
                else if (d is SnapDirections.Vertical)
                {
                    p_s = target.ActualHeight;
                    check = target.VerticalOffset;
                    o_check = _captures[target].VerticalOffset;
                    sizes = c.OfType<FrameworkElement>().Select(x => x.ActualHeight).ToArray();
                    req = ((FrameworkElement)target.Parent).ActualHeight / GetSnapSensibility(target);
                    
                    for (; i < c.Count; i++)
                    {
                        h += ((FrameworkElement)c[i]).ActualHeight;
                        if (target.VerticalOffset < h)
                        {
                            i++;
                            break;
                        }
                    }
                }
                else
                    throw new Exception();

                if (check - o_check > req) // ->
                {}
                else if (o_check - check > req) // <-
                    i -= 2;
                else
                    i--;

                i = i < 0 ? 0 : i % c.Count;
                var to = sizes.Take(i).Sum();
                //    to -= ((p_s - sizes[i]) / 2);

                target.ScrollToHorizontalOffset(to);
                _indexes[target] = i;
                var t = (FrameworkElement)c[i];
                target.RaiseEvent(new SnapEventArgs() { Index = i, SnappendElement = t });

                // OnSnap?.Invoke(target, new(i, (FrameworkElement)c[i], (Panel)target.Content));
            }

            target.ReleaseMouseCapture();
        }

        static void target_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_captures.ContainsKey(sender))
                return;

            if (e.LeftButton is not MouseButtonState.Pressed)
            {
                _captures.Remove(sender);
                return;
            }

            var target = sender as ScrollViewer;
            if (target is null)
                return;

            var capture = _captures[sender];

            var point = e.GetPosition(target);

            var dy = point.Y - capture.Point.Y;
            var dx = point.X - capture.Point.X;
            var catch_height = GetCatchHeightProperty(target);
            var catch_width = GetCatchWidthProperty(target);


            if (dy == 0 && dx == 0)
                return;

            e.Handled = true;


            if ((Math.Abs(dy) > 5 && catch_height) || (Math.Abs(dx) > 5 && catch_width))
                target.CaptureMouse();
            var speed = GetSpeed(target);


            if (catch_height && capture.VerticalOffset != dy)
                target.ScrollToVerticalOffset((capture.VerticalOffset - dy) * speed);

            if (catch_width && capture.HorizontalOffset != dx)
                target.ScrollToHorizontalOffset((capture.HorizontalOffset - dx) * speed);
        }
    }
}