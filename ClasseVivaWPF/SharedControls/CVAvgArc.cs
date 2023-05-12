using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClasseVivaWPF.SharedControls
{
    internal class CVAvgArc : Shape
    {
        public static readonly DependencyProperty StartAngleProperty;
        public static readonly DependencyProperty EndAngleProperty;
        public static readonly DependencyProperty DirectionProperty;
        public static readonly DependencyProperty OriginRotationDegreesProperty;

        static CVAvgArc()
        {
            StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(CVAvgArc), new UIPropertyMetadata(0.0, new PropertyChangedCallback(UpdateArc)));
            EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(CVAvgArc), new UIPropertyMetadata(90.0, new PropertyChangedCallback(UpdateArc)));
            DirectionProperty = DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(CVAvgArc), new UIPropertyMetadata(SweepDirection.Clockwise));
            OriginRotationDegreesProperty = DependencyProperty.Register("OriginRotationDegrees", typeof(double), typeof(CVAvgArc), new UIPropertyMetadata(270.0, new PropertyChangedCallback(UpdateArc)));
        }

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        public SweepDirection Direction
        {
            get => (SweepDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public double OriginRotationDegrees
        {
            get => (double)GetValue(OriginRotationDegreesProperty);
            set => SetValue(OriginRotationDegreesProperty, value);
        }

        protected static void UpdateArc(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CVAvgArc)d).InvalidateVisual();
        }

        protected override Geometry DefiningGeometry => GetArcGeometry();

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(null, new Pen(Stroke, StrokeThickness), GetArcGeometry());
        }

        private Geometry GetArcGeometry()
        {
            Point startPoint = PointAtAngle(Math.Min(StartAngle, EndAngle), Direction);
            Point endPoint = PointAtAngle(Math.Max(StartAngle, EndAngle), Direction);

            Size arcSize = new Size(Math.Max(0, (RenderSize.Width - StrokeThickness) / 2),
                                    Math.Max(0, (RenderSize.Height - StrokeThickness) / 2));
            bool isLargeArc = Math.Abs(EndAngle - StartAngle) > 180;

            StreamGeometry geom = new StreamGeometry();
            using (StreamGeometryContext context = geom.Open())
            {
                context.BeginFigure(startPoint, false, false);
                context.ArcTo(endPoint, arcSize, 0, isLargeArc, Direction, true, false);
            }
            geom.Transform = new TranslateTransform(StrokeThickness / 2, StrokeThickness / 2);
            return geom;
        }

        private Point PointAtAngle(double angle, SweepDirection sweep)
        {
            double translatedAngle = angle + OriginRotationDegrees;
            double radAngle = translatedAngle * (Math.PI / 180);
            double xr = (RenderSize.Width - StrokeThickness) / 2;
            double yr = (RenderSize.Height - StrokeThickness) / 2;

            double x = xr + xr * Math.Cos(radAngle);
            double y = yr * Math.Sin(radAngle);

            if (sweep is SweepDirection.Counterclockwise)
                y = yr - y;
            else
                y = yr + y;

            return new(x, y);
        }
    }
}
