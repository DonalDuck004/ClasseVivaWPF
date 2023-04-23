using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;


namespace ClasseVivaWPF.SharedControls
{
    public class CVScollerView : DependencyObject
    {
        public static bool GetSnapIncludeMargins(DependencyObject obj) => (bool)obj.GetValue(SnapIncludeMarginsProperty);

        public static void SetSnapIncludeMargins(DependencyObject obj, bool value) => obj.SetValue(SnapIncludeMarginsProperty, value);

        public static double GetSnapSensibility(DependencyObject obj) => (double)obj.GetValue(SnapSensibilityProperty);

        public static void SetSnapSensibility(DependencyObject obj, double value) => obj.SetValue(SnapSensibilityProperty, value);

        public static bool GetSnapFrame(DependencyObject obj) => (bool)obj.GetValue(SnapFrameProperty);

        public static void SetSnapFrame(DependencyObject obj, bool value) => obj.SetValue(SnapFrameProperty, value);

        public static bool GetSnap(DependencyObject obj) => (bool)obj.GetValue(SnapProperty);

        public static void SetSnap(DependencyObject obj, bool value) => obj.SetValue(SnapProperty, value);

        public static SnapDirections GetSnapDirection(DependencyObject obj) => (SnapDirections)obj.GetValue(SnapDirectionProperty);

        public static void SetSnapDirection(DependencyObject obj, SnapDirections value) => obj.SetValue(SnapDirectionProperty, value);

        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

        public static bool GetCatchWidthProperty(DependencyObject obj) => (bool)obj.GetValue(CatchWidthProperty);

        public static void SetCatchWidthProperty(DependencyObject obj, bool value) => obj.SetValue(CatchWidthProperty, value);

        public static bool GetCatchHeightProperty(DependencyObject obj) => (bool)obj.GetValue(CatchHeightProperty);

        public static void SetCatchHeightProperty(DependencyObject obj, bool value) => obj.SetValue(CatchHeightProperty, value);

        public static double GetSpeed(DependencyObject obj) => (double)obj.GetValue(SpeedProperty);

        public static void SetSpeed(DependencyObject obj, double value) => obj.SetValue(SpeedProperty, value);

        public static ScrollDirections GetScrollDirection(DependencyObject obj) => (ScrollDirections)obj.GetValue(ScrollDirectionProperty);

        public static void SetScrollDirection(DependencyObject obj, ScrollDirections value) => obj.SetValue(ScrollDirectionProperty, value);

        public static double GetMaxVerticalOffset(DependencyObject obj) => (double)obj.GetValue(MaxVerticalOffsetProperty);

        public static void SetMaxVerticalOffset(DependencyObject obj, double value) => obj.SetValue(MaxVerticalOffsetProperty, value);

        public static double GetMaxHorizontalOffset(DependencyObject obj) => (double)obj.GetValue(MaxHorizontalOffsetProperty);

        public static void SetMaxHorizontalOffset(DependencyObject obj, double value) => obj.SetValue(MaxHorizontalOffsetProperty, value);
        
        public static bool GetMaxHorizontalOffsetSubstractSelf(DependencyObject obj) => (bool)obj.GetValue(MaxHorizontalOffsetSubstractSelfProperty);
        
        public static void SetGetMaxHorizontalOffsetSubstractSelf(DependencyObject obj, bool value) => obj.SetValue(MaxHorizontalOffsetSubstractSelfProperty, value);
        
        public static bool GetMaxVerticalOffsetSubstractSelf(DependencyObject obj) => (bool)obj.GetValue(MaxVerticalOffsetSubstractSelfProperty);
        
        public static void SetMaxVerticalOffsetSubstractSelf(DependencyObject obj, bool value) => obj.SetValue(MaxVerticalOffsetSubstractSelfProperty, value);


        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public double MaxVerticalOffset
        {
            get => (double)GetValue(MaxVerticalOffsetProperty);
            set => SetValue(MaxVerticalOffsetProperty, value);
        }

        public double MaxHorizontalOffset
        {
            get => (double)GetValue(MaxHorizontalOffsetProperty);
            set => SetValue(MaxHorizontalOffsetProperty, value);
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

        public ScrollDirections ScrollDirection
        {
            get => (ScrollDirections)GetValue(ScrollDirectionProperty);
            set => SetValue(ScrollDirectionProperty, value);
        }

        public bool SnapIncludeMargins
        {
            get => (bool)GetValue(SnapIncludeMarginsProperty);
            set => SetValue(SnapIncludeMarginsProperty, value);
        }

        public bool Snap
        {
            get => (bool)GetValue(SnapProperty);
            set => SetValue(SnapProperty, value);
        }

        public bool SnapFrame
        {
            get => (bool)GetValue(SnapFrameProperty);
            set => SetValue(SnapFrameProperty, value);
        }

        public bool MaxHorizontalOffsetSubstractSelf
        {
            get => (bool)GetValue(MaxHorizontalOffsetSubstractSelfProperty);
            set => SetValue(MaxHorizontalOffsetSubstractSelfProperty, value);
        }

        public bool MaxVerticalOffsetSubstractSelf
        {
            get => (bool)GetValue(MaxVerticalOffsetSubstractSelfProperty);
            set => SetValue(MaxVerticalOffsetSubstractSelfProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false, IsEnabledChanged));
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.RegisterAttached("Speed", typeof(double), typeof(CVScollerView), new UIPropertyMetadata(1D));
        public static readonly DependencyProperty CatchWidthProperty = DependencyProperty.RegisterAttached("CatchWidth", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty CatchHeightProperty = DependencyProperty.RegisterAttached("CatchHeight", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty SnapDirectionProperty = DependencyProperty.RegisterAttached("SnapDirection", typeof(SnapDirections), typeof(CVScollerView), new UIPropertyMetadata(SnapDirections.Horizontal));
        public static readonly DependencyProperty SnapProperty = DependencyProperty.RegisterAttached("Snap", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty SnapFrameProperty = DependencyProperty.RegisterAttached("SnapFrame", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty SnapSensibilityProperty = DependencyProperty.RegisterAttached("SnapSensibility", typeof(double), typeof(CVScollerView), new UIPropertyMetadata(4D));
        public static readonly DependencyProperty ScrollDirectionProperty = DependencyProperty.RegisterAttached("ScrollDirection", typeof(ScrollDirections), typeof(CVScollerView), new UIPropertyMetadata(ScrollDirections.Vertical));
        public static readonly DependencyProperty SnapIncludeMarginsProperty = DependencyProperty.RegisterAttached("SnapIncludeMargins", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty MaxVerticalOffsetProperty = DependencyProperty.RegisterAttached("MaxVerticalOffset", typeof(double), typeof(CVScollerView), new UIPropertyMetadata(double.NaN, OnMaxVerticalOffsetChanged));
        public static readonly DependencyProperty MaxHorizontalOffsetProperty = DependencyProperty.RegisterAttached("MaxHorizontalOffset", typeof(double), typeof(CVScollerView), new UIPropertyMetadata(double.NaN, OnMaxHorizontalOffsetChanged));
        public static readonly DependencyProperty MaxHorizontalOffsetSubstractSelfProperty = DependencyProperty.RegisterAttached("MaxHorizontalOffsetSubstractSelf", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty MaxVerticalOffsetSubstractSelfProperty = DependencyProperty.RegisterAttached("MaxVerticalOffsetSubstractSelf", typeof(bool), typeof(CVScollerView), new UIPropertyMetadata(false));
        
        public static readonly RoutedEvent OnSnapFrameEvent = EventManager.RegisterRoutedEvent("OnSnapFrame", RoutingStrategy.Bubble, typeof(OnSnapFrameHandler), typeof(CVScollerView));
        public static readonly RoutedEvent OnSnapEvent = EventManager.RegisterRoutedEvent("OnSnap", RoutingStrategy.Bubble, typeof(OnSnapHandler), typeof(CVScollerView));
        public delegate void OnSnapFrameHandler(object sender, SnapFrameEventArgs e);
        public delegate void OnSnapHandler(object sender, SnapEventArgs e);

        static Dictionary<int, MouseCapture> _captures = new Dictionary<int, MouseCapture>();
        static Dictionary<int, int> _last_snap = new Dictionary<int, int>();

        public static void OnMaxVerticalOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is not ScrollViewer target)
                return;

            if (GetMaxVerticalOffsetSubstractSelf(target))
            {
                if (target.VerticalOffset > (double)e.NewValue - target.ActualHeight)
                    target.ScrollToVerticalOffset((double)e.NewValue - target.ActualHeight);
                else if (target.VerticalOffset == (double)e.OldValue - target.ActualHeight)
                    target.ScrollToVerticalOffset((double)e.NewValue - target.ActualHeight);
            }
            else if (target.VerticalOffset > (double)e.NewValue)
                target.ScrollToVerticalOffset((double)e.NewValue);
            else if (target.VerticalOffset == (double)e.OldValue)
                target.ScrollToVerticalOffset((double)e.NewValue);

        }

        public static void OnMaxHorizontalOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is not ScrollViewer target)
                return;

            if (GetMaxHorizontalOffsetSubstractSelf(target))
            {
                if (target.HorizontalOffset > (double)e.NewValue - target.ActualWidth)
                    target.ScrollToHorizontalOffset((double)e.NewValue - target.ActualWidth);
                else if (target.ActualWidth == (double)e.OldValue - target.ActualWidth)
                    target.ScrollToHorizontalOffset((double)e.NewValue - target.ActualWidth);
            }
            else if (target.HorizontalOffset > (double)e.NewValue)
                target.ScrollToHorizontalOffset((double)e.NewValue);
            else if (target.HorizontalOffset == (double)e.OldValue)
                target.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public static void AddOnSnapFrameHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is not ScrollViewer target)
                return;

            target.AddHandler(OnSnapFrameEvent, handler);
        }

        public static void RemoveOnSnapFrameHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is not ScrollViewer target)
                return;

            target.RemoveHandler(OnSnapFrameEvent, handler);
        }

        public static void AddOnSnapHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is not ScrollViewer target)
                return;

            target.AddHandler(OnSnapEvent, handler);
        }

        public static void RemoveOnSnapHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            if (dependencyObject is not ScrollViewer target)
                return;

            target.RemoveHandler(OnSnapEvent, handler);
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

        static void target_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;
            double tmp;
            double max;

            if (GetScrollDirection(target) is ScrollDirections.Vertical)
            {
                tmp = target.VerticalOffset - e.Delta * GetSpeed(target);
                max = GetMaxVerticalOffset(target);

                if (GetMaxVerticalOffsetSubstractSelf(target))
                    max -= target.ActualHeight;

                if (tmp > max)
                    tmp = max;

                target.ScrollToVerticalOffset(tmp);
            }
            else
            {
                tmp = target.HorizontalOffset - e.Delta * GetSpeed(target);
                max = GetMaxHorizontalOffset(target);

                if (GetMaxHorizontalOffsetSubstractSelf(target))
                    max -= target.ActualWidth;

                if (tmp > max)
                    tmp = max;

                target.ScrollToHorizontalOffset(tmp);
            }
        }

        static void target_Unloaded(object sender, RoutedEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;

            _captures.Remove(sender.GetHashCode());

            // target.Loaded -= target_Loaded;
            target.Unloaded -= target_Unloaded;
            target.PreviewMouseLeftButtonDown -= target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove -= target_PreviewMouseMove;
            target.PreviewMouseWheel -= target_MouseWheel;

            target.PreviewMouseLeftButtonUp -= target_PreviewMouseLeftButtonUp;
        }

        static void target_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (target is null)
                return;
            if (e.OriginalSource is Run)
                return;
            if (e.Source is GridSplitter)
                return;

            _captures[sender.GetHashCode()] = new MouseCapture()
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
            target.PreviewMouseWheel += target_MouseWheel;

            target.PreviewMouseLeftButtonUp += target_PreviewMouseLeftButtonUp;
        }

        private static (FrameworkElement element, double offset, int idx, int old_idx) GetCurrentHorizontal(ScrollViewer target)
        {
            var hash = target.GetHashCode();
            var container = ((Panel)target.Content).Children;
            
            var i = 0;
            var h = 0D;
            double check;
            double o_check;

            double p_s;
            double req;
            double[] sizes;
            // The ActualWidth includes only the padding, not the margin (same with ActualHeight).
            p_s = target.ActualWidth;
            check = target.HorizontalOffset;
            o_check = _captures[hash].HorizontalOffset;

            if (GetSnapIncludeMargins(target))
                sizes = container.OfType<FrameworkElement>().Select(x => x.ActualWidth - x.Margin.Right - x.Margin.Left).ToArray();
            else
                sizes = container.OfType<FrameworkElement>().Select(x => x.ActualWidth).ToArray();

            req = ((FrameworkElement)target.Parent).ActualWidth / GetSnapSensibility(target);

            for (; i < container.Count; i++)
            {
                h += ((FrameworkElement)container[i]).ActualWidth;
                if (target.HorizontalOffset < h)
                {
                    i++;
                    break;
                }
            }

            return Finalize(container: container, check: check, o_check: o_check, req: req, sizes: sizes, i: i);
        }

        private static (FrameworkElement element, double offset, int idx, int old_idx) GetCurrentVertical(ScrollViewer target)
        {
            var hash = target.GetHashCode();
            var container = ((Panel)target.Content).Children;

            var i = 0;
            var h = 0D;
            double check;
            double o_check;

            double p_s;
            double req;
            double[] sizes;
            // The ActualWidth includes only the padding, not the margin (same with ActualHeight).
            p_s = target.ActualHeight;
            check = target.VerticalOffset;
            o_check = _captures[hash].VerticalOffset;
            if (GetSnapIncludeMargins(target))
                sizes = container.OfType<FrameworkElement>().Select(x => x.ActualHeight + x.Margin.Top + x.Margin.Bottom).ToArray();
            else
                sizes = container.OfType<FrameworkElement>().Select(x => x.ActualHeight).ToArray();

            req = ((FrameworkElement)target.Parent).ActualHeight / GetSnapSensibility(target);

            for (; i < container.Count; i++)
            {
                h += ((FrameworkElement)container[i]).ActualHeight;
                if (target.VerticalOffset < h)
                {
                    i++;
                    break;
                }
            }

            return Finalize(container: container, check: check, o_check: o_check, req: req, sizes: sizes, i: i);
        }

        private static (FrameworkElement element, double offset, int idx, int old_idx) Finalize(int i, double check, double o_check, double req, UIElementCollection container, double[] sizes)
        {
            var suff = Math.Abs(check - o_check) > req;
            var old_idx = i;

            if (check > o_check && suff) // ->
                old_idx--;
            else if (check < o_check && suff) // <-
                i--; // i--; // |  -|--- |
            else if (check < o_check)
            { }
            else
                old_idx = --i;

            i = i < 0 ? 0 : i % container.Count;
            var to = sizes.Take(i).Sum();

            return ((FrameworkElement)container[i], to, i, old_idx);
        }

        private static void target_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ScrollViewer target)
                return;

            var hash = sender.GetHashCode();

            try
            {
                if (GetSnap(target) && _captures.ContainsKey(hash))
                {
                    var c = ((Panel)target.Content).Children;
                    var d = GetSnapDirection(target);
                    if (c.Count == 0)
                    {
                        if (d is SnapDirections.Horizontal)
                            target.ScrollToHorizontalOffset(0);
                        else if (d is SnapDirections.Vertical)
                            target.ScrollToVerticalOffset(0);

                        return;
                    }

                    (FrameworkElement element, double offset, int idx, int old_idx) = d is SnapDirections.Horizontal ? GetCurrentHorizontal(target) : GetCurrentVertical(target);

                    if (d is SnapDirections.Horizontal)
                        target.ScrollToHorizontalOffset(offset);
                    else
                        target.ScrollToVerticalOffset(offset);

                    target.RaiseEvent(new SnapEventArgs() { Index = idx, SnappendElement = element, OldIndex = old_idx });
                }
            }
            finally
            {
                target.ReleaseMouseCapture();
            }

        }

        static void target_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var hash = sender.GetHashCode();
            if (!_captures.ContainsKey(hash))
                return;

            if (e.LeftButton is not MouseButtonState.Pressed)
            {
                _captures.Remove(hash);
                return;
            }

            var target = sender as ScrollViewer;
            if (target is null)
                return;

            var capture = _captures[hash];

            var point = e.GetPosition(target);

            var dy = point.Y - capture.Point.Y;
            var dx = point.X - capture.Point.X;
            var ady = Math.Abs(dy);
            var adx = Math.Abs(dx);

            var catch_height = GetCatchHeightProperty(target);
            var catch_width = GetCatchWidthProperty(target);


            if (dy == 0 && dx == 0)
                return;

            if (ady > adx)
                adx = dx = 0;
            else
                ady = dy = 0;

            e.Handled = true;

            if ((ady > 5 && catch_height) || (adx > 5 && catch_width))
                target.CaptureMouse();
            
            var speed = GetSpeed(target);
            double? to = null;
            double max;

            if (catch_height && capture.VerticalOffset != dy)
            {
                to = (capture.VerticalOffset - dy) * speed;

                max = GetMaxVerticalOffset(target);
                if (GetMaxVerticalOffsetSubstractSelf(target))
                    max -= target.ActualWidth;

                if (to > max)
                    to = max;

                target.ScrollToVerticalOffset(to.Value);
            }

            if (catch_width && capture.HorizontalOffset != dx)
            {
                to = (capture.HorizontalOffset - dx) * speed;
                max = GetMaxHorizontalOffset(target);
                if (GetMaxHorizontalOffsetSubstractSelf(target))
                    max -= target.ActualWidth;

                if (to > max)
                    to = max;

                target.ScrollToHorizontalOffset(to.Value);
            }

            if (to is not null && GetSnapFrame(target))
            {
                (FrameworkElement element, double offset, int idx, int old_idx) = GetSnapDirection(target) is SnapDirections.Horizontal ? GetCurrentHorizontal(target) : GetCurrentVertical(target);

                var element_hash = element.GetHashCode();

                if (!_last_snap.ContainsKey(hash))
                {
                    _last_snap[hash] = element_hash;
                    return;
                }

                if (_last_snap[hash] != element_hash)
                {
                    _last_snap[hash] = element_hash;

                    target.RaiseEvent(new SnapFrameEventArgs() { Index = idx, SnappendElement = element, OldIndex = old_idx, offset = offset });
                }

            }

        }
    }
}