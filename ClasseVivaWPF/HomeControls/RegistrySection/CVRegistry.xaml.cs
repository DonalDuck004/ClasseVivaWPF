using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.RegistrySection.Grades;
using ClasseVivaWPF.HomeControls.RegistrySection.Graphs;
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
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

        protected static DependencyProperty FirstPeriodNameProperty;
        protected static DependencyProperty LastPeriodNameProperty;
        protected static DependencyProperty DataFetchedProperty;
        protected static DependencyProperty ShowGraphSettingsProperty;

        private bool UpdateGradesRequired = true;
        private bool UpdateAbsencesRequired = true;

        public Grade[] _CachedGrades;
        public Grade[] CachedGrades { 
            get => _CachedGrades;
            set { 
                UpdateGradesRequired = true;
                _CachedGrades = value.OrderByDescending(x => x.EvtDate).ToArray();
            }
        }

        public Event[] _CachedAbsences;
        public Event[] CachedAbsences { get => _CachedAbsences; set { UpdateAbsencesRequired = true; _CachedAbsences = value; } }

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

        public string? FirstPeriodName
        {
            get => (string?)base.GetValue(FirstPeriodNameProperty);
            set => base.SetValue(FirstPeriodNameProperty, value);
        }

        public string? LastPeriodName
        {
            get => (string?)base.GetValue(LastPeriodNameProperty);
            set => base.SetValue(LastPeriodNameProperty, value);
        }


        static CVRegistry()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVRegistry), new PropertyMetadata(false));
            ShowGraphSettingsProperty = DependencyProperty.Register("ShowGraphSettings", typeof(bool), typeof(CVRegistry), new PropertyMetadata(false));
            FirstPeriodNameProperty = DependencyProperty.Register("FirstPeriodName", typeof(string), typeof(CVRegistry), new PropertyMetadata(null));
            LastPeriodNameProperty = DependencyProperty.Register("LastPeriodName", typeof(string), typeof(CVRegistry), new PropertyMetadata(null));
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
                Duration = new(TimeSpan.FromSeconds(1)),
                FillBehavior = FillBehavior.Stop
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.ValueProperty));
            st.Children.Add(animation);


            var dest = BaseTheme.CV_GRADE_INSUFFICIENT_PATH;
            target.PercentageColor = MainWindow.INSTANCE.CurrentTheme.CV_GRADE_INSUFFICIENT;

            if (avg >= 5)
            {
                dest = BaseTheme.CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH;
                animation = new ColorAnimation()
                {
                    To = target.PercentageColor = MainWindow.INSTANCE.CurrentTheme.CV_GRADE_SLIGHTLY_INSUFFICIENT,
                    AccelerationRatio = 0.1,
                    Duration = new(TimeSpan.FromSeconds(avg >= 6 ? 0.5 : 1)),
                    FillBehavior = FillBehavior.Stop
                };

                Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.PercentageColorProperty));
                st.Children.Add(animation);
            }

            if (avg >= 6)
            {
                dest = BaseTheme.CV_GRADE_SUFFICIENT_PATH;
                animation = new ColorAnimation()
                {
                    To = target.PercentageColor = MainWindow.INSTANCE.CurrentTheme.CV_GRADE_SUFFICIENT,
                    Duration = new(TimeSpan.FromSeconds(0.35)),
                    BeginTime = TimeSpan.FromSeconds(0.65),
                    FillBehavior = FillBehavior.Stop
                };

                Storyboard.SetTargetProperty(animation, new PropertyPath(CVProgressEllipse.PercentageColorProperty));
                st.Children.Add(animation);
            }


            st.Completed += (s, e) =>
            {
                target.SetThemeBinding(BaseCVPercentage.PercentageColorProperty, dest);
            };
            st.Begin(target);
        }

        public void OnSwitch()
        {
            CVRegistry.INSTANCE = null;
        }

        private void UpdateControls(Subject[]? subjects = null)
        {
            // CachedGrades = new Grade[0];

            if (CachedGrades.Length != 0)
            {
                this.SetAVG(SafeAVG(CachedGrades.WhereCountsInAVG().Select(x => x.DecimalValue!.Value)), this.AvgArc);

                var g = (from grade in CachedGrades
                         where grade.DecimalValue is not null && !grade.IsNote
                         group grade by grade.PeriodDesc into tmp
                         orderby tmp.First().PeriodPos
                         select tmp).ToList();

                for (int i = 0; i < g.Count; i++)
                {
                    (i == 0 ? sb_avg_1 : sb_avg_2).Desc = g[i].Key;
                    this.SetAVG(target: i == 0 ? sb_avg_1 : sb_avg_2,
                                avg: SafeAVG(g[i].WhereCountsInAVG().Select(x => x.DecimalValue!.Value)));
                }

                this.FirstPeriodCB.Check();
                this.FirstPeriodCB.CanAlterCheck = true;
                this.FirstPeriodName = sb_avg_1.Desc!;

                if (g.Count == 1)
                {
                    this.FirstPeriodCB.Uncheck();
                    this.FirstPeriodCB.CanAlterCheck = false;

                    sb_avg_2.Value = double.NaN;

                    switch (g[0].Key)
                    {
                        case "Trimestre":
                            this.LastPeriodName = sb_avg_2.Desc = "Pentamestre";
                            break;
                        case "1° Quadrimestre":
                            this.LastPeriodName = sb_avg_2.Desc = "2° Quadrimestre";
                            break;
                        case "Pentamestre":
                            this.LastPeriodName = sb_avg_2.Desc = "Trimestre";
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    this.LastPeriodName = sb_avg_2.Desc!;
                    this.LastPeriodCB.Check();
                    this.LastPeriodCB.CanAlterCheck = true;
                }

                this.ShowNaNCB.CanAlterCheck = true;

                if (subjects is not null)
                {
                    var filtered_cols = g.Merge().OnlyDisplayable();

                    var columns = (from subject in subjects
                                   join grade in filtered_cols
                                   on subject.Id equals grade.SubjectId into sg
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
                    foreach (var grade in CachedGrades)
                    {
                        this.GradesWrapper.Children.Add(new CVGradeEllipse(grade));
                    }
                }
            }
            else
            {
                this.FirstPeriodName = this.LastPeriodName = "???";
                this.grades_placeholder.Visibility = Visibility.Visible;
                this.grades_placeholder.Content = "Nessun voto disponibile";
                this.GradesWrapper.Children.Clear();
               
                if (subjects is not null)
                    this.Graph.Update(subjects.Select(x => x.ShortName));
                else
                    this.Graph.Clear();

                this.ShowNaNCB.CanAlterCheck = this.FirstPeriodCB.CanAlterCheck = this.LastPeriodCB.CanAlterCheck = false;
                this.FirstPeriodCB.Uncheck();
                this.LastPeriodCB.Uncheck();
                this.ShowNaNCB.Check();
                
                this.sb_avg_1.Desc = this.sb_avg_2.Desc = "???";
                this.AvgArc.Value = this.sb_avg_1.Value = this.sb_avg_2.Value = double.NaN;
            }

            if (_CachedAbsences.Length != 0)
            {
                var groups = _CachedAbsences.GroupBy(x => x.EvtCode);

                this.abs.Count = groups.Where(x => x.Key == BaseEvent.ABSENCE_ABSENCE).Select(x => x.Count()).Sum();
                this.l.Count = groups.Where(x => x.Key.StartsWith(BaseEvent.ABSENCE_LATE_START_STRING)).Select(x => x.Count()).Sum();
                var epabs = groups.Where(x => x.Key == BaseEvent.ABSENCE_SHORT_LATE).Select(x => x.Count()).Sum();
                var lpabs = groups.Where(x => x.Key == BaseEvent.ABSENCE_LATE).Select(x => x.Count()).Sum();
                this.l.ExtraDesc = $"Brevi: {epabs}; Lunghi: {lpabs}";
                this.ee.Count = groups.Where(x => x.Key.StartsWith(BaseEvent.ABSENCE_EARLY_EXIT_START_STRING)).Select(x => x.Count()).Sum();
            }
            else
            {
                this.ee.Count = this.l.Count = this.abs.Count = 0;
                this.l.ExtraDesc = "";
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

        private void ShakeGraphSettings()
        {
            var v = Canvas.GetLeft(this.GraphWrapper);

            var animation = new DoubleAnimation()
            {
                To = v - 10,
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(4),
                Duration = new Duration(TimeSpan.FromSeconds(0.08)),
                FillBehavior = FillBehavior.Stop
            };

            this.GraphWrapper.BeginAnimation(Canvas.LeftProperty, animation);
        }

        private void GraphFilterUpdated(CVCheckBox sender, CheckedStateChangedEventArgs e)
        {
            if (e.Handled)
                return;

            var FPChecked = object.ReferenceEquals(sender, this.FirstPeriodCB) ? e.NewState : this.FirstPeriodCB.IsChecked;
            var LPChecked = object.ReferenceEquals(sender, this.LastPeriodCB) ? e.NewState : this.LastPeriodCB.IsChecked;
            var ShowNaN = object.ReferenceEquals(sender, this.ShowNaNCB) ? e.NewState : this.ShowNaNCB.IsChecked;

            if (FPChecked && LPChecked)
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, ShowNaN);
            else if (FPChecked)
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, ShowNaN, "Pentamestre");
            else if (LPChecked)
                this.Graph.Filter(CVColumnsGraphFilterOperation.GroupAVG, ShowNaN, "Trimestre");
            else
                (object.ReferenceEquals(sender, this.FirstPeriodCB) ? this.LastPeriodCB : this.FirstPeriodCB).IsChecked = true;
        }

        private void OnTriedChangeStateCB(CVCheckBox sender, EventArgs e)
        {
            this.ShakeGraphSettings();
        }

        private void OpenGradesViewer(object sender, MouseButtonEventArgs e)
        {
            if (this.CachedGrades.Length == 0)
            {
                new CVMessageBox("Errore!", "Impossibile aprire questa schermata, poichè al momento non hai valutazioni").Inject();
                return;
            }

            new CVGradesViewer().Inject();
        }
    }

}
