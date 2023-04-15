using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClasseVivaWPF.SharedControls
{
    public class Injectable : UserControl, ICloseRequested
    {
        private SemaphoreSlim waiter = new SemaphoreSlim(1);

        public Injectable()
        {

        }

        public virtual void Inject()
        {
            if (waiter.CurrentCount == 0)
                throw new InvalidOperationException();

            waiter.Wait();
            MainWindow.INSTANCE!.AddFieldOverlap(this);
        }

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
            waiter.Release();
        }

        public async Task WaitForExit()
        {
            await waiter.WaitAsync();
        }

        public virtual void OnCloseRequested()
        {
            waiter.Release();
        }
    }
}
