using System.Windows;


namespace ClasseVivaWPF.SharedControls
{
    public class SnapEventArgs : RoutedEventArgs
    {
        public SnapEventArgs() : base(CVScollerView.OnSnapEvent)
        {

        }

        protected SnapEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {

        }

        public required int OldIndex { get; init; }
        public required int Index { get; init; }
        public required FrameworkElement SnappendElement { get; init; }
    }
}