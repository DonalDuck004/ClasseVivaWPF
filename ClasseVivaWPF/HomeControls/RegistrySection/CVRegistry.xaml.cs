using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ClasseVivaWPF.HomeControls.RegistrySection
{
    /// <summary>
    /// Logica di interazione per CVRegistry.xaml
    /// </summary>
    public partial class CVRegistry : UserControl, IOnSwitch
    {
        
        public static CVRegistry? INSTANCE { get; private set; }

        protected static DependencyProperty DataFetchedProperty;

        private bool GradesUpdateRequired = true;
        private bool AbsencesRequired = true;

        public Grade[] _CachedGrades;
        public Grade[] CachedGrades { get => _CachedGrades; set { GradesUpdateRequired = true; _CachedGrades = value; } }

        public Event[] _CachedAbsences;
        public Event[] CachedAbsences { get => _CachedAbsences; set { AbsencesRequired = true; _CachedAbsences = value; } }

        public bool DataFetched
        {
            get => (bool)base.GetValue(DataFetchedProperty);
            set => base.SetValue(DataFetchedProperty, value);
        }

        static CVRegistry()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVRegistry), new PropertyMetadata(false));
        }

        private SemaphoreSlim ReloadLock = new SemaphoreSlim(1, 1);

        public CVRegistry()
        {
            InitializeComponent();
            this.DataContext = this;
            CVRegistry.INSTANCE = this;
        }

        private void SetAVG(double avg, BaseCVPercentage target)
        {
            if (avg is double.NaN)
            {
                this.AvgArc.Value = avg;
                return;
            }

            var st = new Storyboard();

            AnimationTimeline animation = new DoubleAnimation()
            {
                From = 0,
                To = avg,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.ValueProperty));
            st.Children.Add(animation);


            target.PercentageColor = Color.FromRgb(0xD0, 0x5A, 0x50);

            if (avg >= 5)
            {
                animation = new ColorAnimation()
                {
                    To = Color.FromRgb(0xEB, 0x98, 0x60),
                    AccelerationRatio = 0.1,
                    Duration = new Duration(TimeSpan.FromSeconds(avg >= 6 ? 0.5 : 1)),
                };

                Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.PercentageColorProperty));
                st.Children.Add(animation);
            }

            if (avg >= 6)
            {
                animation = new ColorAnimation()
                {
                    To = Color.FromRgb(0x83, 0xB5, 0x88),
                    Duration = new Duration(TimeSpan.FromSeconds(0.35)),
                    BeginTime = TimeSpan.FromSeconds(0.65),
                };

                Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.PercentageColorProperty));
                st.Children.Add(animation);
            }

            st.Begin(target);
        }

        public void OnSwitch()
        {
            CVRegistry.INSTANCE = null;
        }

        private void UpdateControls(Subject[]? subjects = null)
        {
            this.SetAVG(SafeAVG(CachedGrades.Where(x => x.DecimalValue is not null && !x.IsNote).Select(x => x.DecimalValue!.Value)), this.AvgArc);

            pps.Children.Clear();
            var g = (from grade in CachedGrades
                     where grade.DecimalValue is not null && !grade.IsNote
                     group grade by grade.PeriodDesc into tmp
                     orderby tmp.Key descending
                     select tmp).ToList();
            // TODO Sistemare ordinamento
            CVPeriodPercentage p;

            foreach (var item in g)
            {
                p = new CVPeriodPercentage()
                {
                    Desc = item.Key
                };

                pps.Children.Add(p);

                this.SetAVG(target: p,
                            avg: SafeAVG(item.Where(x => x.DecimalValue is not null && !x.IsNote).Select(x => x.DecimalValue!.Value)));
            }

            if (g.Count != 2)
            {
                p = new CVPeriodPercentage()
                {
                    Value = double.NaN
                };

                pps.Children.Add(p);

                switch (g[0].Key)
                {
                    case "Trimestre":
                        p.Desc = "Pentamestre";
                        break;
                    case "1° Quadrimestre":
                        p.Desc = "2° Quadrimestre";
                        break;
                    case "Pentamestre":
                        p.Desc = "Trimestre";
                        break;
                    default:
                        throw new Exception();
                }

            }

            if (subjects is not null){
                var columns = (from subject in subjects
                               join grade in g.Last() on subject.Id equals grade.SubjectId into sg
                               from grade in sg.DefaultIfEmpty()
                               group grade by subject into gr
                               select new CVColumn()
                               {
                                   ContentID = gr.Key.Id,
                                   Desc = gr.Key.ShortName,
                                   LongDesc = gr.Key.Description.ToTitle(),
                                   Value = SafeAVG(gr.Select(x => x is null || x.DecimalValue is null ? double.NaN : x.DecimalValue.Value))
                               }).ToList();

                foreach (var item in columns)
                    this.SetAVG(item.Value, item);

                this.Graph.Update(columns);
            }
            
            this.GradesWrapper.Children.Clear();
            foreach (var grade in CachedGrades.OrderByDescending(x => x.EvtDate))
            {
                this.GradesWrapper.Children.Add(new CVGradeEllipse()
                {
                    Grade = grade,
                });
            }

            var groups = _CachedAbsences.GroupBy(x => x.EvtCode);

            this.abs.Count = groups.Where(x => x.Key == BaseEvent.ABSENCE).Select(x => x.Count()).Sum();
            this.pabs.Count = groups.Where(x => x.Key.StartsWith(BaseEvent.LATE_START_STRING)).Select(x => x.Count()).Sum();
            var epabs = groups.Where(x => x.Key == BaseEvent.SHORT_LATE).Select(x => x.Count()).Sum();
            var lpabs = groups.Where(x => x.Key == BaseEvent.LATE).Select(x => x.Count()).Sum();
            this.pabs.ExtraDesc = $"Brevi: {epabs}; Lunghi: {lpabs}";
            this.ee.Count = groups.Where(x => x.Key.StartsWith(BaseEvent.EARLY_EXIT_START_STRING)).Select(x => x.Count()).Sum();

            return;

            double SafeAVG(IEnumerable<double> s)
            {
                try
                {
                    return s.Average();
                }
                catch (InvalidOperationException)
                {
                    return double.NaN;
                }
            };
        }

        private async Task Reload()
        {
            if (ReloadLock.CurrentCount == 0)
                return;

            await ReloadLock.WaitAsync();
            this.DataFetched = false;

            var subjects = (await Client.INSTANCE.GetSubjects()).ContentSubjects;
            CachedGrades = (await Client.INSTANCE.GetGrades()).ContentGrades;
            CachedAbsences = (await Client.INSTANCE.GetAbsences()).ContentEvents;

            this.DataFetched = true;

            try
            {
                this.UpdateControls(subjects: subjects);
            }
            finally
            {
                ReloadLock.Release();
            }
        }

        private record SubjectAVG(string Subject, double AVG);

        private async void OnReload(object sender, MouseButtonEventArgs e)
        {
            await Reload();
        }
        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            await Reload();
            graph_loading.Visibility = Visibility.Hidden;
        }
    }
}
