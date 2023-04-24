using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Threading;

namespace ClasseVivaWPF.Utils
{
    public class AsyncEvent
    {
        private bool value = false;
        private List<SemaphoreSlim> tasks = new();

        public bool IsSet() => value;

        public void Clear() => value = false;

        public void Set()
        {
            if (value is true)
                return;

            value = true;

            foreach (var sem in tasks) { 
                sem.Release();
            }
        }

        public async Task<bool> WaitAsync()
        {
            if (value)
                return true;

            var sem = new SemaphoreSlim(1, 1);
            await sem.WaitAsync();
            tasks.Add(sem);

            try
            {
                await sem.WaitAsync();
                return true;
            }
            finally
            {
                tasks.Remove(sem);
            }
        }
    }
}
