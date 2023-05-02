using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClasseVivaWPF.SharedControls
{
    public class Injectable : UserControl, IOnCloseRequested
    {
        private AsyncEvent @event = new();
        private bool Opened = false;

        public Injectable()
        {

        }

        public virtual void Inject()
        {
            if (@event.IsSet() && this.Opened)
                throw new InvalidOperationException();
            
            if (!this.Opened)
                @event.Clear();

            this.Opened = true;

            MainWindow.INSTANCE!.AddFieldOverlap(this);
        }

        public virtual void Close()
        {
            this.WhenInjectableIsClosed();
            MainWindow.INSTANCE.RemoveField(this);
            this.Opened = false;
            @event.Set();
        }

        public async Task WaitForExit()
        {
            await @event.WaitAsync();
        }

        public virtual void OnCloseRequested()
        {
            this.WhenInjectableIsClosed();
            this.Opened = false;
            @event.Set();
        }

        public virtual void WhenInjectableIsClosed()
        {

        }
    }
}
