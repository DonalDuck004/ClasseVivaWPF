using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ClasseVivaWPF.SharedControls;

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
