using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected static DependencyProperty ShowGraphSettingsProperty;

        private bool UpdateGradesRequired = true;
        private bool UpdateAbsencesRequired = true;

        public Grade[] _CachedGrades;
        public Grade[] CachedGrades { get => _CachedGrades; set { UpdateGradesRequired = true; _CachedGrades = value; } }

        public Event[] _CachedAbsences;
        public Event[] CachedAbsences { get => _CachedAbsences; set { UpdateAbsencesRequired = true; _CachedAbsences = value; } }

#pragma warning disable CS0169
        private string FirstPeriodName; // TODO
        private string LastPeriodName;
#pragma warning restore CS0169

        public bool DataFetched
        {
            get => (bool)base.GetValue(DataFetchedProperty);
            set => base.SetValue(DataFetchedProperty, value);
        }

        public bool ShowGraphSettings
        {
            get => (bool)base.GetValue(ShowGraphSettingsProperty);
            set => base.SetValue(ShowGraphSettingsProperty, value);
        }

        static CVRegistry()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVRegistry), new PropertyMetadata(false));
            ShowGraphSettingsProperty = DependencyProperty.Register("ShowGraphSettings", typeof(bool), typeof(CVRegistry), new PropertyMetadata(false));
        }

        private SemaphoreSlim ReloadLock = new SemaphoreSlim(1, 1);

        public CVRegistry()
        {
            InitializeComponent();
            this.DataContext = this;
            CVRegistry.INSTANCE = this;
            this.Graph.ColumnAdded += (c) => this.SetAVG(c.Value, c);
        }

        private void SetAVG(double avg, BaseCVPercentage target)
        {
            if (avg is double.NaN)
            {
                target.Value = avg;
                return;
            }

            var st = new Storyboard();

            AnimationTimeline animation = new DoubleAnimation()
            {
                From = 0,
                To = target.Value = avg,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                FillBehavior = FillBehavior.Stop
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.ValueProperty));
            st.Children.Add(animation);


            target.PercentageColor = Color.FromRgb(0xD0, 0x5A, 0x50);

            if (avg >= 5)
            {
                animation = new ColorAnimation()
                {
                    To = target.PercentageColor = Color.FromRgb(0xEB, 0x98, 0x60),
                    AccelerationRatio = 0.1,
                    Duration = new Duration(TimeSpan.FromSeconds(avg >= 6 ? 0.5 : 1)),
                    FillBehavior = FillBehavior.Stop
                };

                Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.PercentageColorProperty));
                st.Children.Add(animation);
            }

            if (avg >= 6)
            {
                animation = new ColorAnimation()
                {
                    To = target.PercentageColor = Color.FromRgb(0x83, 0xB5, 0x88),
                    Duration = new Duration(TimeSpan.FromSeconds(0.35)),
                    BeginTime = TimeSpan.FromSeconds(0.65),
                    FillBehavior = FillBehavior.Stop
                };
                
                Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.PercentageColorProperty));
                st.Children.Add(animation);
            }

            st.Completed += (s, e) =>
            {
                st.Remove(target);
                target.Value = target.Value;
            };
            st.Begin(target);
        }

        public void OnSwitch()
        {
            CVRegistry.INSTANCE = null;
        }

        private void UpdateControls(Subject[]? subjects = null)
        {
            /*var afs = this.CachedGrades.ToList();
            afs.Clear();
            this.CachedGrades = afs.ToArray();
            */

            if (CachedGrades.Length != 0)
            {
                this.SetAVG(SafeAVG(CachedGrades.Where(x => x.DecimalValue is not null && !x.IsNote).Select(x => x.DecimalValue!.Value)), this.AvgArc);

                var g = (from grade in CachedGrades
                         where grade.DecimalValue is not null && !grade.IsNote
                         group grade by grade.PeriodDesc into tmp
                         orderby tmp.First().PeriodPos
                         select tmp).ToList();

                for (int i = 0; i < g.Count; i++)
                {
                    (i == 0 ? sb_avg_1 : sb_avg_2).Desc = g[i].Key;
                    this.SetAVG(target: i == 0 ? sb_avg_1 : sb_avg_2,
                                avg: SafeAVG(g[i].Where(x => x.DecimalValue is not null && !x.IsNote).Select(x => x.DecimalValue!.Value)));
                }

                this.FirstPeriodCB.IsChecked = this.FirstPeriodCB.IsEnabled = true;

                if (g.Count == 1)
                {
                    this.LastPeriodCB.IsChecked = this.LastPeriodCB.IsEnabled = false;

                    sb_avg_2.Value = double.NaN;

                    switch (g[0].Key)
                    {
                        case "Trimestre":
                            sb_avg_2.Desc = "Pentamestre";
                            break;
                        case "1° Quadrimestre":
                            sb_avg_2.Desc = "2° Quadrimestre";
                            break;
                        case "Pentamestre":
                            sb_avg_2.Desc = "Trimestre";
                            break;
                        default:
                            throw new Exception();
                    }
                }else
                    this.LastPeriodCB.IsChecked = this.LastPeriodCB.IsEnabled = true;

                if (subjects is not null)
                {
                    var columns = (from subject in subjects
                                   join grade in g.Merge() on subject.Id equals grade.SubjectId into sg
                                   from grade in sg.DefaultIfEmpty()
                                   group grade by (subject, grade?.PeriodDesc) into gr
                                   let values = gr.Select(x => x is null || x.DecimalValue is null ? double.NaN : x.DecimalValue.Value)
                                   select new CVColumn()
                                   {
                                       ContentID = gr.Key.subject.Id,
                                       Desc = gr.Key.subject.ShortName,
                                       SubGroupName = gr.Key.PeriodDesc,
                                       LongDesc = gr.Key.subject.Description.ToTitle(),
                                       Values = values,
                                       Value = SafeAVG(values)
                                   }).ToList();

                    this.grades_placeholder.Visibility = Visibility.Hidden;
                    this.Graph.Update(columns, this.ShowNaNCB.IsChecked, op: CVColumnsGraphFilterOperation.GroupAVG);
                    this.GradesWrapper.Children.Clear();
                    foreach (var grade in CachedGrades.OrderByDescending(x => x.EvtDate))
                    {
                        this.GradesWrapper.Children.Add(new CVGradeEllipse()
                        {
                            Grade = grade,
                        });
                    }
                }
            }
            else
            {
                this.grades_placeholder.Visibility = Visibility.Visible;
                this.grades_placeholder.Content = "Nessun voto disponibile";
                this.GradesWrapper.Children.Clear();

                if (subjects is not null && !this.ShowNaNCB.IsChecked)
                    this.Graph.Update(subjects.Select(x => x.ShortName));
                else
                    this.Graph.Clear();

                this.FirstPeriodCB.IsEnabled = this.LastPeriodCB.IsEnabled = false;
                this.FirstPeriodCB.IsChecked = this.LastPeriodCB.IsChecked = false;
                this.sb_avg_1.Desc = this.sb_avg_2.Desc = "???";
                AvgArc.Value = this.sb_avg_1.Value = this.sb_avg_2.Value = double.NaN;
            }
            
            if (_CachedAbsences.Length != 0)
            {
                var groups = _CachedAbsences.GroupBy(x => x.EvtCode);

                this.abs.Count = groups.Where(x => x.Key == BaseEvent.ABSENCE).Select(x => x.Count()).Sum();
                this.pabs.Count = groups.Where(x => x.Key.StartsWith(BaseEvent.LATE_START_STRING)).Select(x => x.Count()).Sum();
                var epabs = groups.Where(x => x.Key == BaseEvent.SHORT_LATE).Select(x => x.Count()).Sum();
                var lpabs = groups.Where(x => x.Key == BaseEvent.LATE).Select(x => x.Count()).Sum();
                this.pabs.ExtraDesc = $"Brevi: {epabs}; Lunghi: {lpabs}";
                this.ee.Count = groups.Where(x => x.Key.StartsWith(BaseEvent.EARLY_EXIT_START_STRING)).Select(x => x.Count()).Sum();
            }
            else
            {
                this.ee.Count = this.pabs.Count = this.abs.Count = 0;
                this.pabs.ExtraDesc = "";
            }

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

        private void GraphSettingsClick(object sender, MouseButtonEventArgs e)
        {
            this.ShowGraphSettings = !this.ShowGraphSettings;
        }

        private void GraphFilterUpdated(object sender, EventArgs e)
        {
            if (!((CVCheckBox)sender).IsEnabled)
                return;

            if (this.FirstPeriodCB.IsChecked && this.LastPeriodCB.IsChecked)
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, this.ShowNaNCB.IsChecked);
            else if (this.FirstPeriodCB.IsChecked)
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, this.ShowNaNCB.IsChecked, "Pentamestre");
            else if (this.LastPeriodCB.IsChecked)
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, this.ShowNaNCB.IsChecked, "Trimestre");
            else
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, this.ShowNaNCB.IsChecked, "Trimestre", "Pentamestre");
        }
    }
}
