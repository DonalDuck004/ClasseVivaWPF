using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace ClasseVivaWPF.SharedControls
{
    public class CVScollerView : DependencyObject
    {

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

        static Dictionary<object, MouseCapture> _captures = new Dictionary<object, MouseCapture>();

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
            System.Diagnostics.Debug.WriteLine("Target Unloaded");

            var target = sender as ScrollViewer;
            if (target is null)
                return;

            _captures.Remove(sender);

            target.Loaded -= target_Loaded;
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

            System.Diagnostics.Debug.WriteLine("Target Loaded");

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