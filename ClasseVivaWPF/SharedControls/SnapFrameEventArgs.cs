namespace ClasseVivaWPF.SharedControls
{
    public class SnapFrameEventArgs : SnapEventArgs
    {
        public SnapFrameEventArgs() : base(CVScollerView.OnSnapFrameEvent)
        {

        }

        public required double offset { get; init; }
    }
}