using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClasseVivaWPF.HomeControls.HomeSection
{
    /// <summary>
    /// Logica di interazione per CVWeek.xaml
    /// </summary>
    public partial class CVWeek : UserControl
    {
        private CVWeek? _previus = null;
        private CVWeek? _next = null;
        public CVWeek Previus
        {
            get
            {
                if (_previus is not null)
                    return _previus;

                _previus = CVWeek.New(this.From.AddDays(-7), false).ConfigureAwait(false).GetAwaiter().GetResult();
                return _previus;
            }
        }
        public CVWeek Next
        {
            get
            {
                if (_next is not null)
                    return _next;

                _next = CVWeek.New(this.From.AddDays(7), false).ConfigureAwait(false).GetAwaiter().GetResult();
                return _next;
            }
        }

        public DateTime From { get; private set; }
        public DateTime To => From.AddDays(6);

        private static Dictionary<DateTime, CVWeek> cached_objects = new();
        private static SemaphoreSlim create_lock = new(1, 1);
        public bool Destroyed { get; private set; } = false;

        private CVWeek()
        {
            InitializeComponent();
        }

        public static async Task<CVWeek?> CacheGet(DateTime from)
        {
            await create_lock.WaitAsync();

            CVWeek? @this = null;

            try
            {
                @this = CVWeek.CacheGetUnsafe(from);
                return @this;
            }
            finally
            {
                Debug.Assert(@this is not null);

                create_lock.Release();
            }
        }

        public static CVWeek? CacheGetUnsafe(DateTime from)
        {
            Debug.Assert(from.Date == from);
            var monday = from.AddDays(-from.DayOfWeek.AsInt32());
            if (cached_objects.ContainsKey(monday))
                return cached_objects[monday];

            return null;
        }

        public static async Task<CVWeek> New(DateTime from, bool GenChain = true)
        {
            Debug.Assert(from.Date == from);
            await create_lock.WaitAsync();

            CVWeek? @this = null;

            try
            {
                @this = CVWeek.NewUnsafe(from, GenChain);
                return @this;
            }
            finally
            {
                Debug.Assert(@this is not null);

                create_lock.Release();
            }
        }

        public static CVWeek NewUnsafe(DateTime from, bool GenChain = true)
        {
            Debug.Assert(from.Date == from);
            var monday = from.Date.AddDays(-from.DayOfWeek.AsInt32());
            // var debug = monday.AddDays(6).Date;
            // Debug.Assert(debug != DateTime.Now.Date);
            if (cached_objects.ContainsKey(monday))
                return cached_objects[monday];

            var @this = new CVWeek()
            {
                From = monday,
            };


            for (int i = 0; i < 7; i++)
            {
                @this.grid.Children.Add(new CVDay(@this.From.AddDays(i), parent: @this));
                Grid.SetColumn(@this.grid.Children[i], i);
            }

            @this.grid.Children.Capacity = 7; // Clean up memory


            if (GenChain)
            {
                @this._previus = CVWeek.NewUnsafe(@this.From.AddDays(-7), false);
                @this._next = CVWeek.NewUnsafe(@this.From.AddDays(7), false);
                @this._previus._next = @this;
                @this._next._previus = @this;
            }

            return cached_objects[monday] = @this;
        }

        ~CVWeek()
        {
            ;
        }

        public void SelectChild(DayOfWeek day, bool update = false) => SelectChild(day.AsInt32(), update);

        public void SelectChild(int idx, bool update = false)
        {
            var child = this.GetChild(idx);
            child.IsSelected = true;
            if (update)
                child.Update(require_new_call: true);
        }

        public void GetChild(DayOfWeek day) => GetChild(day.AsInt32());
        public CVDay GetChild(int idx)
        {
            return (CVDay)grid.Children[idx];
        }

        public void View(StackPanel content)
        {

            CVHome.INSTANCE.homework_scroller.Content = content;
            CVHome.INSTANCE.UpdateDayLabel();

            this.Scroll();
        }

        internal void BeginDestroy()
        {
            if (ReferenceEquals(CVDay.SelectedDay!.Parent, this))
                throw new Exception();
            cached_objects.Remove(this.From);

            CVWeek? tmp;
            if ((tmp = CVWeek.CacheGetUnsafe(this.To.AddDays(1))) is not null) // TODO Check if instance at date exists
                tmp._previus = null;
            if ((tmp = CVWeek.CacheGetUnsafe(this.From.AddDays(-7))) is not null)
                tmp._next = null;

            foreach (CVDay item in this.grid.Children)
                item.BeginDestroy();

            this.grid.Children.Clear();
            this.Destroyed = true;
        }

        internal static CVWeek GetWeekContaining(DateTime date)
        {
            var week = CVWeek.NewUnsafe(date);
            if (week.Parent is null)
                CVHome.INSTANCE.AddWeek(week);
            return week;
        }

        public void Scroll()
        {
            this.Dispatcher.BeginInvoke(() => this.BringIntoView());
            var idx = CVHome.INSTANCE.IndexOfWeek(this);
            Debug.Assert(idx != -1);

            if (this.Next.Parent is null)
            {
                CVHome.INSTANCE.AddWeek(this.Next, idx + 1);
                idx = CVHome.INSTANCE.IndexOfWeek(this); // IDX may change after an mem - optimization
            }

            Debug.Assert(idx == CVHome.INSTANCE.IndexOfWeek(this));
            var w = this.From;
            if (this.Previus.Parent is null)
                CVHome.INSTANCE.AddWeek(this.Previus, idx);
        }
    }
}
