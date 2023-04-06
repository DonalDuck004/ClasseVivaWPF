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

        public bool DataFetched
        {
            get => (bool)base.GetValue(DataFetchedProperty);
            set => base.SetValue(DataFetchedProperty, value);
        }
        static CVRegistry()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVRegistry), new PropertyMetadata(false));
        }

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
                Duration = new Duration(TimeSpan.FromSeconds(1))
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
                    BeginTime = TimeSpan.FromSeconds(0.65)
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

        private async Task Reload()
        {
            this.DataFetched = false;

            var subjects = (await Client.INSTANCE.GetSubjects()).ContentSubjects;
            var grades = (await Client.INSTANCE.GetGrades()).ContentGrades;

            await Task.Delay(2000);
            this.DataFetched = true;
            try
            {
                this.SetAVG(grades.Where(x => x.DecimalValue is not null).Select(x => x.DecimalValue!.Value).Average(), this.AvgArc);
            }
            catch (InvalidOperationException)
            {
                this.SetAVG(double.NaN, this.AvgArc);
            }

            pps.Children.Clear();
            var g = (from grade in grades
                     where grade.DecimalValue is not null
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

                try
                {
                    this.SetAVG(target: p,
                                avg: item.Where(x => x.DecimalValue is not null).Select(x => x.DecimalValue).Average()!.Value);
                }
                catch (InvalidOperationException)
                {
                    this.SetAVG(double.NaN, p);
                }
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
                    case "Quadrimestre":
                        p.Desc = "Quadrimestre";
                        break;
                    case "Pentamestre":
                        p.Desc = "Trimestre";
                        break;
                    default:
                        throw new Exception();
                }

            }

            var x = (from grade in g.Last()
                     where grade.DecimalValue is not null
                     group grade by grade.ShortSubjectName into a
                     where a.Count() != 0
                     select new SubjectAVG(a.Key, Math.Ceiling(a.Average(x => x.DecimalValue)!.Value))).ToList();

            x.AddRange(subjects.Select(y => new SubjectAVG(y.ShortName, double.NaN)).Where(y => !x.Where(z => z.Subject != y.Subject).Any()));

            CVColumn col;
            List<CVColumn> cols = new();

            foreach (var item in x)
            {
                col = new()
                {
                    Desc = item.Subject,
                    Value = item.AVG
                };
                this.SetAVG(item.AVG, col);
                cols.Add(col);
            }

            this.Graph.Update(cols);
        }

        private record SubjectAVG(string Subject, double AVG);


        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            await Reload();
        }
    }
}
