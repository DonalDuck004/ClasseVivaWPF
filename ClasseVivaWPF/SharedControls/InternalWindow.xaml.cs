using ClasseVivaWPF.HomeControls;
using ClasseVivaWPF.Utils.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ClasseVivaWPF.SharedControls
{
    public class IntWinClosingEventArgs : EventArgs
    {
        public bool Cancel = false;
    }

    /// <summary>
    /// Logica di interazione per InternalWindow.xaml
    /// </summary>
    public partial class InternalWindow : UserControl
    {
        public static readonly DependencyProperty ContentItemProperty;
        public static readonly DependencyProperty TitleProperty;
        public static readonly DependencyProperty AllowVMoveProperty;
        public static readonly DependencyProperty AllowHMoveProperty;
        public static readonly DependencyProperty MountOnMainWindowProperty;
        public static readonly DependencyProperty ExpandedProperty;
        private static readonly DependencyProperty MoveLockedProperty;
        private static readonly DependencyProperty RestoreToProperty;
        public static readonly DependencyProperty StayInRectProperty;
        public static readonly DependencyProperty CollapsedProperty;

        public FrameworkElement? ContentItem
        {
            get => (FrameworkElement?)GetValue(ContentItemProperty);
            set => SetValue(ContentItemProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        private bool MoveLocked
        {
            get => (bool)GetValue(MoveLockedProperty);
            set => SetValue(MoveLockedProperty, value);
        }

        public bool AllowVMove
        {
            get => (bool)GetValue(AllowVMoveProperty);
            set => SetValue(AllowVMoveProperty, value);
        }

        public bool AllowHMove
        {
            get => (bool)GetValue(AllowHMoveProperty);
            set => SetValue(AllowHMoveProperty, value);
        }

        public bool MountOnMainWindow
        {
            get => (bool)GetValue(MountOnMainWindowProperty);
            set => SetValue(MountOnMainWindowProperty, value);
        }

        public bool Expanded
        {
            get => (bool)GetValue(ExpandedProperty);
            set => SetValue(ExpandedProperty, value);
        }

        private Point? RestoreTo
        {
            get => (Point?)GetValue(RestoreToProperty);
            set => SetValue(RestoreToProperty, value);
        }

        public Rect? StayInRect
        {
            get => (Rect?)GetValue(StayInRectProperty);
            set => SetValue(StayInRectProperty, value);
        }

        public bool Collapsed
        {
            get => (bool)GetValue(CollapsedProperty);
            set => SetValue(CollapsedProperty, value);
        }

        private bool Moving = false;
        public delegate void ClosedHandler(InternalWindow sender);
        public delegate void ClosingHandler(InternalWindow sender, IntWinClosingEventArgs e);
        public event ClosedHandler Closed;
        public event ClosingHandler Closing;

        static InternalWindow()
        {
            ContentItemProperty = DependencyProperty.Register("ContentItem", typeof(FrameworkElement), typeof(InternalWindow));
            TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(InternalWindow));
            AllowVMoveProperty = DependencyProperty.Register("AllowVMove", typeof(bool), typeof(InternalWindow), new PropertyMetadata(true));
            AllowHMoveProperty = DependencyProperty.Register("AllowHMove", typeof(bool), typeof(InternalWindow), new PropertyMetadata(true));
            MoveLockedProperty = DependencyProperty.Register("MoveLocked", typeof(bool), typeof(InternalWindow));
            MountOnMainWindowProperty = DependencyProperty.Register("MountOnMainWindow", typeof(bool), typeof(InternalWindow), new PropertyMetadata(true));
            ExpandedProperty = DependencyProperty.Register("Expanded", typeof(bool), typeof(InternalWindow), new PropertyMetadata(false, OnExpandedPropertyChanged));
            RestoreToProperty = DependencyProperty.Register("RestoreTo", typeof(Point?), typeof(InternalWindow));
            StayInRectProperty = DependencyProperty.Register("StayInRect", typeof(Rect?), typeof(InternalWindow), new PropertyMetadata(new Rect(0, 0, double.PositiveInfinity, double.PositiveInfinity), OnStayInRectPropertyChanged));
            CollapsedProperty = DependencyProperty.Register("Collapsed", typeof(bool), typeof(InternalWindow), new PropertyMetadata(OnCollapsedChanged));
        }

        private static void OnCollapsedChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is not InternalWindow sender)
                return;

            sender.content_wp.Visibility = e.NewValue is true ? Visibility.Collapsed : Visibility.Visible;
        }

        private static void OnStayInRectPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is not InternalWindow sender)
                return;

            if (e.NewValue is not Rect rect)
            {
                if (e.NewValue is null)
                {
                    sender.wp.MaxHeight = double.PositiveInfinity;
                    sender.wp.MaxWidth = double.PositiveInfinity;
                }

                return;
            }
            sender.wp.MaxHeight = rect.Height;
            sender.wp.MaxWidth = rect.Width;

            var l = Canvas.GetLeft(sender.wp);
            var t = Canvas.GetTop(sender.wp);

            if (l < rect.Left)
            {
                Canvas.SetLeft(sender.wp, rect.Left);
                l = rect.Left;
            }

            if (t < rect.Top)
            {
                Canvas.SetTop(sender.wp, rect.Top);
                t = rect.Top;
            }

            if (rect.Left + rect.Width < l + sender.wp.ActualWidth)
                Canvas.SetLeft(sender.wp, rect.Left + rect.Width - sender.wp.ActualWidth);

            if (rect.Top + rect.Height < t + sender.wp.ActualHeight)
                Canvas.SetTop(sender.wp, rect.Top + rect.Height - sender.wp.ActualHeight);
        }

        private static void OnExpandedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is not InternalWindow sender)
                return;
            
            if (e.NewValue is true)
            {
                if (sender.Parent is null)
                    throw new InvalidOperationException("This windows doesn't have a parent");

                var binding = new Binding()
                {
                    Path = new(FrameworkElement.ActualWidthProperty),
                    Mode = BindingMode.OneWay,
                    Source = sender.Parent,
                    Converter = new ActionConverter(),
                    ConverterParameter = (object v) => (double)v > sender.wp.MaxWidth ? sender.wp.MaxWidth : v
                };

                BindingOperations.SetBinding(sender.grid, FrameworkElement.WidthProperty, binding);


                binding = new Binding()
                {
                    Path = new(FrameworkElement.ActualHeightProperty),
                    Mode = BindingMode.OneWay,
                    Source = sender.Parent,
                    Converter = new ActionConverter(),
                    ConverterParameter = (object v) => (double)v > sender.wp.MaxHeight ? sender.wp.MaxHeight : v
                }; 
                
                BindingOperations.SetBinding(sender.grid, FrameworkElement.HeightProperty, binding);

                sender.MoveLocked = true;
                sender.RestoreTo = new Point(Canvas.GetLeft(sender.wp), Canvas.GetTop(sender.wp));

                Canvas.SetLeft(sender.wp, sender.StayInRect.HasValue ? sender.StayInRect.Value.Left : 0);
                Canvas.SetTop(sender.wp, sender.StayInRect.HasValue ? sender.StayInRect.Value.Top : 0);
            }
            else
            {
                BindingOperations.ClearBinding(sender.grid, FrameworkElement.WidthProperty);
                BindingOperations.ClearBinding(sender.grid, FrameworkElement.HeightProperty);

                sender.MoveLocked = false;
                if (sender.RestoreTo is not null)
                {
                    Canvas.SetLeft(sender.wp, sender.RestoreTo.Value.X);
                    Canvas.SetTop(sender.wp, sender.RestoreTo.Value.Y);


                    sender.RestoreTo = null;
                }
            }

        }

        public InternalWindow()
        {
            InitializeComponent();

            MainWindow.INSTANCE.SizeChanged += OnSizeChanged;
        }

        public void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
                Canvas.SetLeft(this.wp, Canvas.GetLeft(this.wp) * e.NewSize.Width / e.PreviousSize.Width);
            if (e.HeightChanged)
                Canvas.SetTop(this.wp, Canvas.GetTop(this.wp) * e.NewSize.Height / e.PreviousSize.Height);
        }

        private Point from;

        private void BeginMove(object sender, MouseButtonEventArgs e)
        {
            this.Moving = true;
            this.from = Mouse.GetPosition(this.header_wp);
        }

        private void EndMove(object sender, MouseButtonEventArgs e)
        {
            this.Moving = false;
        }

        private void Move(object sender, MouseEventArgs e)
        {
            if (!this.Moving || this.MoveLocked)
                return;

            var pos = Mouse.GetPosition((FrameworkElement)this.Parent);

            if (this.AllowHMove)
            {
                var tmp = pos.X - this.from.X;
                tmp = this.StayInRect.HasValue && tmp < this.StayInRect.Value.Left ? this.StayInRect.Value.Left : tmp;
                tmp = this.StayInRect.HasValue && this.StayInRect.Value.Left + this.StayInRect.Value.Width < tmp + this.wp.ActualWidth ? this.StayInRect.Value.Left + this.StayInRect.Value.Width - this.wp.ActualWidth : tmp;

                Canvas.SetLeft(this.wp, tmp);
            }

            if (this.AllowVMove)
            {
                var tmp = pos.Y - this.from.Y;
                tmp = this.StayInRect.HasValue && tmp < this.StayInRect.Value.Top ? this.StayInRect.Value.Top : tmp;
                tmp = this.StayInRect.HasValue && this.StayInRect.Value.Top + this.StayInRect.Value.Height < tmp + this.wp.ActualHeight ? this.StayInRect.Value.Top + this.StayInRect.Value.Height - this.wp.ActualHeight : tmp;
               
                Canvas.SetTop(this.wp, tmp);
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            if (this.Parent is null || (this.MountOnMainWindow && this.Parent is not MainWindow) || (!this.MountOnMainWindow && this.Parent != CVMainNavigation.INSTANCE!.Current)) 
                ManualMount();
        }

        private void LeaveMove(object sender, MouseEventArgs e)
        {
            this.Moving = false;
        }

        private void OnChangeExpand(object sender, MouseButtonEventArgs e)
        {
            this.Expanded = !this.Expanded;
        }

        private void OnCollapse(object sender, MouseButtonEventArgs e)
        {
            this.Collapsed = !this.Collapsed;
        }

        private void OnClose(object sender, MouseButtonEventArgs e) => Unmount();

        public void Unmount(bool fire_event = true)
        {
            if (this.Parent is not Panel x)
                throw new InvalidOperationException("Parent is not valid");

            if (fire_event && !RaiseClosing())
                return;

            x.Children.Remove(this);

            if (fire_event)
                RaiseClosed();
        }

        private void RaiseClosed()
        {
            if (this.Closed is not null)
                this.Closed(this);
        }
        private bool RaiseClosing()
        {
            if (this.Closing is not null)
            {
                var e = new IntWinClosingEventArgs();
                this.Closing(this, e);
                return e.Cancel;
            }

            return false;
        }
        public void MoveTo(Point point) => MoveTo(point.X, point.Y);

        public void MoveTo(double? x, double? y)
        {
            if (x is not null)
                Canvas.SetLeft(this.wp, this.StayInRect.HasValue && x.Value < this.StayInRect.Value.Left ? this.StayInRect.Value.Left : x.Value);

            if (y is not null)
                Canvas.SetTop(this.wp, this.StayInRect.HasValue && y.Value < this.StayInRect.Value.Top ? this.StayInRect.Value.Top : y.Value);
        }

        public void ManualMount()
        {
            if (this.Parent is not null)
                this.Unmount(fire_event: false);

            if (this.MountOnMainWindow)
                MainWindow.INSTANCE.AddFieldOverlap(this);
            else
                CVMainNavigation.INSTANCE!.Current.Children.Add(this);
        }

        public void AddCustomAction(FrameworkElement element)
        {
            this.header_wp.ColumnDefinitions.Add(new() { Width = GridLength.Auto });
            this.header_wp.Children.Insert(1, element);
            Grid.SetColumn(element, 1);
            for (int i = 2; i < this.header_wp.Children.Count; i++)
                Grid.SetColumn(this.header_wp.Children[i], i);
        }
    }
}
