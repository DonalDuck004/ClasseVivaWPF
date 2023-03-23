using System.Windows;


namespace ClasseVivaWPF.SharedControls
{
    internal class MouseCapture
    {
        internal required double HorizontalOffset;
        internal required double VerticalOffset;
        internal required Point Point;

        public bool Equals(DependencyObject target, MouseCapture other)
        {
            bool result = true;

            if (CVScollerView.GetCatchWidthProperty(target))
                result = result && other.HorizontalOffset == this.HorizontalOffset;

            if (CVScollerView.GetCatchHeightProperty(target))
                result = result && other.VerticalOffset == this.VerticalOffset;

            return result;
        }
    }
}