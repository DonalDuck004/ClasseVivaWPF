using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsencesCalendar.xaml
    /// </summary>
    public partial class CVAbsencesCalendar : UserControl
    {
        private Storyboard NextMonth;
        private Storyboard PreviusMonth;

        public static readonly DependencyProperty SelectedDateProperty;
        private SemaphoreSlim? WaitLoader = null;

        private DateTime ActualSelectedDate;

        public DateTime SelectedDate
        {
            get => (DateTime)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        static CVAbsencesCalendar()
        {
            SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(CVAbsencesCalendar));
        }

        public CVAbsencesCalendar()
        {
            InitializeComponent();
            this.DataContext = this;

            var duration = TimeSpan.FromSeconds(0.1);
            DoubleAnimation tmp;


            NextMonth = new();

            NextMonth.Children.Add(tmp = new() { To = 0.8, From = 1, Duration = new(duration) });
            Storyboard.SetTargetProperty(tmp, new(Canvas.OpacityProperty));
            tmp.Completed += (s, e) => SelectedDate = this.ActualSelectedDate;
            NextMonth.Children.Add(tmp = new() { To = -16, From = 0, Duration = new(duration) });
            Storyboard.SetTargetProperty(tmp, new(Canvas.TopProperty));

            NextMonth.Children.Add(tmp = new() { To = 0, From = 16, Duration = new(duration), BeginTime = duration });
            Storyboard.SetTargetProperty(tmp, new(Canvas.TopProperty));
            NextMonth.Children.Add(tmp = new() { To = 1, From = 0.8, Duration = new(duration), BeginTime = duration });
            Storyboard.SetTargetProperty(tmp, new(Canvas.OpacityProperty));

            Storyboard.SetTarget(NextMonth, this.date_header);


            PreviusMonth = new();

            PreviusMonth.Children.Add(tmp = new() { To = 16, From = 0, Duration = new(duration) });
            Storyboard.SetTargetProperty(tmp, new(Canvas.TopProperty));
            tmp.Completed += (s, e) => SelectedDate = this.ActualSelectedDate;
            PreviusMonth.Children.Add(tmp = new() { To = 0, From = 1, Duration = new(duration) });
            Storyboard.SetTargetProperty(tmp, new(Canvas.OpacityProperty));

            PreviusMonth.Children.Add(tmp = new() { To = 0, From = -16, Duration = new(duration), BeginTime = duration });
            Storyboard.SetTargetProperty(tmp, new(Canvas.TopProperty));
            PreviusMonth.Children.Add(tmp = new() { To = 1, From = 0, Duration = new(duration), BeginTime = duration });
            Storyboard.SetTargetProperty(tmp, new(Canvas.OpacityProperty));

            Storyboard.SetTarget(PreviusMonth, this.date_header);
        }

        public void Init(List<DayStatus> days)
        {
            this.months_wrapper.Children.Clear();
            var groups = days.GroupBy(x => x.Date.Month).ToArray();
            this.WaitLoader = new SemaphoreSlim(0, groups.Length);

            this.scroller.SizeChanged += (s, e) => GetMonth(this.ActualSelectedDate).BringIntoView();

            foreach (var month in groups)
                this.months_wrapper.Children.Add(new CVAbsencesCalendarMonth(month.ToArray(), this.WaitLoader));

            var dt = DateTime.Now.Date;
            var mt = GetMonth(dt.AddDays(-dt.Day + 1));
            this.ActualSelectedDate = this.SelectedDate = mt.Date;
            this.scroller.ScrollToHorizontalOffset(OffsetOf(mt));
        }

        public async Task WaitForLoading()
        {
            if (this.WaitLoader is null) throw new Exception();

            while(this.WaitLoader.CurrentCount > 0)
                await Task.Delay(100);
        }

        private CVAbsencesCalendarMonth GetMonth(DateTime date)
        {
            var children = this.months_wrapper.Children.OfType<CVAbsencesCalendarMonth>();
            var deb = children.ToArray();
            try
            {
                return children.Where(x => x.Date == date).First();
            }catch
            {
                return children.Where(x => x.Date == this.ActualSelectedDate).FirstOrDefault(children.Last());
            }
        }

        private double OffsetOf(CVAbsencesCalendarMonth element)
        {
            return this.months_wrapper.Children.OfType<CVAbsencesCalendarMonth>().TakeWhile(x => !ReferenceEquals(x, element)).Select(x => x.ActualWidth).Sum();
        }

        private void scroller_OnSnapFrame(object sender, SnapFrameEventArgs e)
        {
            SetValue(SelectedDateProperty, ((CVAbsencesCalendarMonth)e.SnappendElement).Date);
        }

        private void scroller_OnSnap(object sender, SnapEventArgs e)
        {
            SetValue(SelectedDateProperty, this.ActualSelectedDate = ((CVAbsencesCalendarMonth)e.SnappendElement).Date);
        }

        private void GoBackClick(object sender, MouseButtonEventArgs e)
        {
            var month = GetMonth(this.ActualSelectedDate.AddMonths(-1));
            this.ActualSelectedDate = month.Date;
            this.scroller.AnimateScrollerH(this.scroller.HorizontalOffset, OffsetOf(month), 0.2, do_nothing_if_running: true);
            PreviusMonth.Begin();
        }

        private void GoNextClick(object sender, MouseButtonEventArgs e)
        {
            var month = GetMonth(this.ActualSelectedDate.AddMonths(1));
            this.ActualSelectedDate = month.Date;
            this.scroller.AnimateScrollerH(this.scroller.HorizontalOffset, OffsetOf(month), 0.2, do_nothing_if_running: true);
            NextMonth.Begin();
        }
    }

    public class KineticBehaviour
    {
        #region Friction

        /// <summary>
        /// Friction Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FrictionProperty =
            DependencyProperty.RegisterAttached("Friction",
            typeof(double), typeof(KineticBehaviour),
                new FrameworkPropertyMetadata((double)0.95));

        /// <summary>
        /// Gets the Friction property.  This dependency property
        /// indicates ....
        /// </summary>
        public static double GetFriction(DependencyObject d)
        {
            return (double)d.GetValue(FrictionProperty);
        }

        /// <summary>
        /// Sets the Friction property.
        /// </summary>
        public static void SetFriction(DependencyObject d, double value)
        {
            d.SetValue(FrictionProperty, value);
        }

        #endregion

        #region ScrollStartPoint

        /// <summary>
        /// ScrollStartPoint Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty ScrollStartPointProperty =
            DependencyProperty.RegisterAttached("ScrollStartPoint",
            typeof(Point), typeof(KineticBehaviour),
                new FrameworkPropertyMetadata((Point)new Point()));

        /// <summary>
        /// Gets the ScrollStartPoint property.
        /// </summary>
        private static Point GetScrollStartPoint(DependencyObject d)
        {
            return (Point)d.GetValue(ScrollStartPointProperty);
        }

        /// <summary>
        /// Sets the ScrollStartPoint property.
        /// </summary>
        private static void SetScrollStartPoint(DependencyObject d,
            Point value)
        {
            d.SetValue(ScrollStartPointProperty, value);
        }

        #endregion

        #region ScrollStartOffset

        /// <summary>
        /// ScrollStartOffset Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty ScrollStartOffsetProperty =
            DependencyProperty.RegisterAttached("ScrollStartOffset",
            typeof(Point), typeof(KineticBehaviour),
                new FrameworkPropertyMetadata((Point)new Point()));

        /// <summary>
        /// Gets the ScrollStartOffset property.
        /// </summary>
        private static Point GetScrollStartOffset(DependencyObject d)
        {
            return (Point)d.GetValue(ScrollStartOffsetProperty);
        }

        /// <summary>
        /// Sets the ScrollStartOffset property.
        /// </summary>
        private static void SetScrollStartOffset(DependencyObject d,
            Point value)
        {
            d.SetValue(ScrollStartOffsetProperty, value);
        }

        #endregion

        #region InertiaProcessor

        /// <summary>
        /// InertiaProcessor Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty InertiaProcessorProperty =
            DependencyProperty.RegisterAttached("InertiaProcessor",
            typeof(InertiaHandler), typeof(KineticBehaviour),
                new FrameworkPropertyMetadata((InertiaHandler)null));

        /// <summary>
        /// Gets the InertiaProcessor property.
        /// </summary>
        private static InertiaHandler GetInertiaProcessor(DependencyObject d)
        {
            return (InertiaHandler)d.GetValue(InertiaProcessorProperty);
        }

        /// <summary>
        /// Sets the InertiaProcessor property.
        /// </summary>
        private static void SetInertiaProcessor(DependencyObject d,
            InertiaHandler value)
        {
            d.SetValue(InertiaProcessorProperty, value);
        }

        #endregion

        #region HandleKineticScrolling

        /// <summary>
        /// HandleKineticScrolling Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty
            HandleKineticScrollingProperty =
            DependencyProperty.RegisterAttached("HandleKineticScrolling",
            typeof(bool), typeof(KineticBehaviour),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(
                        OnHandleKineticScrollingChanged)));

        /// <summary>
        /// Gets the HandleKineticScrolling property.
        /// </summary>
        public static bool GetHandleKineticScrolling(DependencyObject d)
        {
            return (bool)d.GetValue(HandleKineticScrollingProperty);
        }

        /// <summary>
        /// Sets the HandleKineticScrolling property.
        /// </summary>
        public static void SetHandleKineticScrolling(DependencyObject d,
            bool value)
        {
            d.SetValue(HandleKineticScrollingProperty, value);
        }

        /// <summary>
        /// Handles changes to the HandleKineticScrolling property.
        /// </summary>
        private static void OnHandleKineticScrollingChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scoller = d as ScrollViewer;
            if ((bool)e.NewValue)
            {
                scoller.PreviewMouseDown += OnPreviewMouseDown;
                scoller.PreviewMouseMove += OnPreviewMouseMove;
                scoller.PreviewMouseUp += OnPreviewMouseUp;
                SetInertiaProcessor(scoller, new InertiaHandler(scoller));
            }
            else
            {
                scoller.PreviewMouseDown -= OnPreviewMouseDown;
                scoller.PreviewMouseMove -= OnPreviewMouseMove;
                scoller.PreviewMouseUp -= OnPreviewMouseUp;
                var inertia = GetInertiaProcessor(scoller);
                if (inertia != null)
                    inertia.Dispose();
            }

        }

        #endregion

        #region Mouse Events
        private static void OnPreviewMouseDown(object sender,
            MouseButtonEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.IsMouseOver)
            {
                // Save starting point, used later when
                //determining how much to scroll.
                SetScrollStartPoint(scrollViewer,
                    e.GetPosition(scrollViewer));
                SetScrollStartOffset(scrollViewer,
                    new Point(scrollViewer.HorizontalOffset,
                        scrollViewer.VerticalOffset));
                scrollViewer.CaptureMouse();
            }
        }


        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.IsMouseCaptured)
            {
                Point currentPoint = e.GetPosition(scrollViewer);

                var scrollStartPoint = GetScrollStartPoint(scrollViewer);
                // Determine the new amount to scroll.
                Point delta = new Point(scrollStartPoint.X -
                    currentPoint.X, scrollStartPoint.Y - currentPoint.Y);

                var scrollStartOffset = GetScrollStartOffset(scrollViewer);
                Point scrollTarget = new Point(scrollStartOffset.X +
                    delta.X, scrollStartOffset.Y + delta.Y);

                var inertiaProcessor = GetInertiaProcessor(scrollViewer);
                if (inertiaProcessor != null)
                    inertiaProcessor.ScrollTarget = scrollTarget;

                // Scroll to the new position.
                scrollViewer.ScrollToHorizontalOffset(scrollTarget.X);
                scrollViewer.ScrollToVerticalOffset(scrollTarget.Y);
            }
        }

        private static void OnPreviewMouseUp(object sender,
            MouseButtonEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.IsMouseCaptured)
            {
                scrollViewer.ReleaseMouseCapture();
            }
        }
        #endregion

        #region Inertia Stuff

        /// <summary>
        /// Handles the inertia
        /// </summary>
        class InertiaHandler : IDisposable
        {
            private Point previousPoint;
            private Vector velocity;
            ScrollViewer scroller;
            DispatcherTimer animationTimer;

            private Point scrollTarget;
            public Point ScrollTarget
            {
                get { return scrollTarget; }
                set { scrollTarget = value; }
            }

            public InertiaHandler(ScrollViewer scroller)
            {
                this.scroller = scroller;
                animationTimer = new DispatcherTimer();
                animationTimer.Interval =
                    new TimeSpan(0, 0, 0, 0, 20);
                animationTimer.Tick +=
                    new EventHandler(HandleWorldTimerTick);
                animationTimer.Start();
            }

            private void HandleWorldTimerTick(object sender,
                EventArgs e)
            {
                if (scroller.IsMouseCaptured)
                {
                    Point currentPoint = Mouse.GetPosition(scroller);
                    velocity = previousPoint - currentPoint;
                    previousPoint = currentPoint;
                }
                else
                {
                    if (velocity.Length > 1)
                    {
                        scroller.ScrollToHorizontalOffset(
                            ScrollTarget.X);
                        scroller.ScrollToVerticalOffset(
                            ScrollTarget.Y);
                        scrollTarget.X += velocity.X;
                        scrollTarget.Y += velocity.Y;
                        velocity *= KineticBehaviour.GetFriction(scroller);
                    }
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                animationTimer.Stop();
            }

            #endregion
        }

        #endregion
    }
}
