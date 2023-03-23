using ClasseVivaWPF.SharedControls;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    class CVHomeworksViewer : ScrollViewer
    {
        public CVHomeworksViewer() : base()
        {
            SetValue(CVScollerView.IsEnabledProperty, true);
            SetValue(CVScollerView.CatchWidthProperty, false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key is Key.Down || e.Key is Key.Up)
                base.OnKeyDown(e);
        }
    }
}
