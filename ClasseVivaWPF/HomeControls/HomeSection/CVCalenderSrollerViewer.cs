using ClasseVivaWPF.SharedControls;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    class CVCalenderSrollerViewer : ScrollViewer
    {
        public CVCalenderSrollerViewer() : base()
        {
            SetValue(CVScollerView.IsEnabledProperty, true);
            SetValue(CVScollerView.CatchHeightProperty, false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = false;
        }
    }
}
