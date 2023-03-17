using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF
{
    class CVCalenderSrollerViewer : ScrollViewer
    {
        public CVCalenderSrollerViewer() : base()
        {
            this.SetValue(CVScollerView.IsEnabledProperty, true);
            this.SetValue(CVScollerView.CatchHeightProperty, false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = false;
        }
    }

    class CVHomeWorkViewer : ScrollViewer
    {
        public CVHomeWorkViewer() : base()
        {
            this.SetValue(CVScollerView.IsEnabledProperty, true);
            this.SetValue(CVScollerView.CatchWidthProperty, false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key is Key.Down || e.Key is Key.Up)
                base.OnKeyDown(e);
        }
    }
}
