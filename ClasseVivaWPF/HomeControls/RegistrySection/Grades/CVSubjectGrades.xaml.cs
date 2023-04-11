using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Grades
{
    /// <summary>
    /// Logica di interazione per CVSubjectGrades.xaml
    /// </summary>
    public partial class CVSubjectGrades : UserControl
    {
        public static readonly DependencyProperty ExpandedProperty;


        public bool Expanded
        {
            get => (bool)GetValue(ExpandedProperty);
            set {
                SetValue(ExpandedProperty, value);
                Storyboard ExpandCTAnimation = new();
                Storyboard ExpandHAnimation = new();
                DoubleAnimation animationCT;
                DoubleAnimation animationH;
                var duration = new Duration(TimeSpan.FromSeconds(0.5));

                if (value)
                {
                    animationH = new()
                    {
                        Duration = duration,
                        To = this.GradeStackWP.Height = this.GradeStack.ActualHeight
                    };

                    animationCT = new()
                    {
                        From = -this.GradeStack.ActualHeight,
                        Duration = duration,
                        To = 0
                    };

                }else
                {
                    animationH = new()
                    {
                        Duration = duration,
                        To = this.GradeStackWP.Height = 0
                    };

                    animationCT = new()
                    {
                        To = -this.GradeStack.ActualHeight,
                        Duration = duration,
                        From = 0
                    };
                }

                Storyboard.SetTargetProperty(animationH, new(Canvas.HeightProperty));
                Storyboard.SetTargetProperty(animationCT, new(Canvas.TopProperty));

                ExpandHAnimation.Children.Add(animationH);
                ExpandCTAnimation.Children.Add(animationCT);
                ExpandHAnimation.Begin(this.GradeStackWP);
                ExpandCTAnimation.Begin(this.GradeStack);

            }
        }

        static CVSubjectGrades()
        {



            ExpandedProperty = DependencyProperty.Register("Expanded", typeof(bool), typeof(CVSubjectGrades), new PropertyMetadata(false));
        }


        // For vs
        private CVSubjectGrades()
        {
            InitializeComponent();
        }

        public static CVSubjectGrades? New(Grade[] grades)
        {
            grades = grades.OnlyDisplayable().ToArray();
            if (grades.Length == 0)
                return null;
            
            var @this = new CVSubjectGrades();
            @this.DataContext = @this;

            foreach (var g in grades)
                @this.GradeStack.Children.Add(new CVGrade(g));

            try
            {
                @this.avg.Value = grades.WhereCountsInAVG().Average(x => x.DecimalValue)!.Value;
            }catch(InvalidOperationException)
            {
                @this.avg.Value = double.NaN;
            }

            
            return @this;
        }

        private void OnExpand(object sender, MouseButtonEventArgs e)
        {
            this.Expanded = !this.Expanded;
        }
    }
}
