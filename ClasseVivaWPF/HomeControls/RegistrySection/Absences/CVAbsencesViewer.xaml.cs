using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Api;
using ClasseVivaWPF.HomeControls.RegistrySection.Grades;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClasseVivaWPF.HomeControls.RegistrySection.Graphs;
using ClasseVivaWPF.Utils.Themes;
using System.Diagnostics;
using System.Drawing;
using ClasseVivaWPF.HomeControls.MenuSection;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{

    public partial class CVAbsencesViewer : Injectable, IUpdate, ICVMeta
    {
        public static CVAbsencesViewer? INSTANCE { get; set; } = null;

        public static readonly DependencyProperty DataFetchedProperty;

        private SemaphoreSlim ReloadLock = new SemaphoreSlim(1, 1);
        private const int MonthOffset = 9;
        private const int MonthOffsetLength = 10;
        private static readonly string[] Months = new string[] {"Set", "Ott", "Nov", "Dic", "Gen", "Feb", "Mar", "Apr", "Mag", "Giu"};
        private static readonly string[] Evts = new string[] {"ABA0", "ABU0", "ABR0"};
        public bool CountsInStack { get; } = false;
        private SemaphoreSlim PreventOverlap { get; } = new SemaphoreSlim(0, 1);
        private CalendarDay[]? Days = null;


        public bool DataFetched
        {
            get => (bool)base.GetValue(DataFetchedProperty);
            set => base.SetValue(DataFetchedProperty, value);
        }

        static CVAbsencesViewer()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVAbsencesViewer), new PropertyMetadata(false));
        }

        public CVAbsencesViewer() : base()
        {
            InitializeComponent();
            this.DataContext = this;
            CVAbsencesViewer.INSTANCE = this;
        }

        public void Update()
        {
            var today = DateTime.Now.Date;
            var year_begin = today;

            year_begin = year_begin.AddDays(-year_begin.Day + 1);

            if (year_begin.Month < MonthOffset)
                year_begin = year_begin.AddYears(-1).AddMonths(-year_begin.Month + MonthOffset);

            var end = year_begin.AddMonths(MonthOffsetLength);

            var values = new List<DateTime>();
            List<DayStatus> DayStates = new();
            Event? abs;
            bool is_school_day;
            int mdays;

            while(year_begin < end)
            {
                mdays = DateTime.DaysInMonth(year_begin.Year, year_begin.Month);

                for (int i = 0; i < mdays; i++)
                {
                    if (is_school_day = this.Days!.Where(x => x.DayDate == year_begin).FirstOrDefault()?.IsSchoolDay is true)
                        abs = CVRegistry.INSTANCE!.CachedAbsences.Where(x => x.EvtDate == year_begin).FirstOrDefault();
                    else
                        abs = null;

                    DayStates.Add(new(Date: year_begin,
                                      IsPresent: abs is null && is_school_day && year_begin < today,
                                      IsAbsent: abs is null ? false : abs.IsAbsence,
                                      IsLate: abs is null ? false : abs.IsLate,
                                      IsEarlyExit: abs is null ? false : abs.IsEarlyExit,
                                      IsPartiallyAbsent: abs is null ? false : abs.IsPartiallyAbsent));
                    year_begin = year_begin.AddDays(1);
                }

                Debug.Assert(1 == year_begin.Day);
            }
            DayStates.Capacity = DayStates.Count;

            this.Calendar.Init(DayStates);

            var q = from evt in Evts
                    from month in Months
                    let block = (month, evt, CVRegistry.INSTANCE!.CachedAbsences.Where(x => x.EvtCode == evt && x.EvtDate.ToString("MMM") == month).ToArray())
                    select new CVColumn(PostLoad: col =>
                    {
                        switch (block.evt)
                        {
                            case BaseEvent.ABSENCE_ABSENCE:
                                col.SetThemeBinding(CVColumn.PercentageColorProperty, ThemeProperties.CVAbsencesAbsentProperty);
                                break;
                            case BaseEvent.ABSENCE_EARLY_EXIT:
                                col.SetThemeBinding(CVColumn.PercentageColorProperty, ThemeProperties.CVAbsencesEarlyExitProperty);
                                break;
                            case BaseEvent.ABSENCE_LATE:
                                col.SetThemeBinding(CVColumn.PercentageColorProperty, ThemeProperties.CVAbsencesLateProperty);
                                break;
                            default:
                                col.SetThemeBinding(CVColumn.PercentageColorProperty, ThemeProperties.CVAbsencesPartiallyAbsentProperty);
                                break;
                        }
                    })
                    {
                        Max = 27,
                        Value = block.Item3.Length,
                        ContentID = block.month,
                        Desc = block.month,
                        StringFormat = "{0}",
                        LongDesc = (block.evt == BaseEvent.ABSENCE_ABSENCE ? "Assenze" :
                                    block.evt == BaseEvent.ABSENCE_EARLY_EXIT ? "Uscite anticipate" :
                                    "Ritardi") + $": {block.Item3.Length}",
                        SubGroupName = block.evt,
                        Content = block.month,
                        Tag = block.evt == BaseEvent.ABSENCE_ABSENCE ? 1 : block.evt == BaseEvent.ABSENCE_EARLY_EXIT ? 2 : block.evt == BaseEvent.ABSENCE_LATE ? 3 : 4
                    };

            var now = DateTime.Now.Date;

            q = q.Concat(from day in this.Days!
                         where day.DayDate < now && day.IsSchoolDay && !CVRegistry.INSTANCE!.CachedAbsences.Where(x => x.EvtDate == day.DayDate && Evts.Contains(x.EvtCode)).Any()
                         group day by day.DayDate.ToString("MMM") into g
                         let c = g.Count()
                         select new CVColumn(PostLoad: col => col.SetThemeBinding(CVColumn.PercentageColorProperty, ThemeProperties.CVAbsencesPresentProperty))
                         {
                            Max = 27,
                            Value = c,
                            ContentID = g.Key,
                            Desc = g.Key,
                            StringFormat = "{0}",
                            LongDesc = $"Presenze: {c}",
                            SubGroupName = "InternalCode-Presence",
                            Content = g.Key,
                            Tag = 0,
                         });

            this.Graph.Update(columns: q,
                              order_func: x => x.OrderBy(y => y.Tag).ToArray(), 
                              persist_zero_fields: false,
                              op: CVColumnsGraphFilterOperation.NoneNoOverlap);

            this.Justified.ClearAll();
            this.NotJustified.ClearAll();

            foreach (var item in CVRegistry.INSTANCE!.CachedAbsences.OrderByDescending(x => x.EvtDate))
            {
                if (item.IsJustified)
                    this.Justified.AddChild(new(item));
                else
                    this.NotJustified.AddChild(new(item));
            }
        }

        public async Task Reload()
        {
            if (ReloadLock.CurrentCount == 0)
                return;

            try
            {
                await ReloadLock.WaitAsync();

                this.DataFetched = false;

                try
                {
                    CVRegistry.INSTANCE!.CachedAbsences = (await Client.INSTANCE.GetAbsences()).ContentEvents;
                }
                catch (ApiError exc)
                {
                    this.DataFetched = true;
                    exc.ApplyStdProcedure();
                    return;
                }
            }
            finally
            {
                ReloadLock.Release();
            }

            this.Update();
            await this.Calendar.WaitForLoading();
            this.DataFetched = true;
        }

        private async void ReloadBtn(object sender, MouseButtonEventArgs e) => await Reload();

        private void OnClose(object sender, MouseButtonEventArgs e) => this.Close();

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            try
            {
                this.Days = (await Client.INSTANCE.Calendar()).ContentCalendar;
                this.Update();
                await this.Calendar.WaitForLoading();

                this.DataFetched = true;
            }
            catch (ApiError exc)
            {
                this.Close();
                exc.ApplyStdProcedure();
                return;
            }

            PreventOverlap.Release();
        }

        public override void WhenInjectableIsClosed()
        {
            if (ReferenceEquals(CVAbsencesViewer.INSTANCE, this))
                CVAbsencesViewer.INSTANCE = null;
        }
    }
}
