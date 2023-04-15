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
using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;

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
                        To = this.GradeStack.ActualHeight
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
                        To = -this.GradeStack.ActualHeight + 5,
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

        public static CVSubjectGrades? New(Grade[] grades, Subject subject)
        {
            var avg_grades = grades.OnlyDisplayable().ToArray();
            if (avg_grades.Length == 0)
                return null;
            
            var @this = new CVSubjectGrades();

            try
            {
                @this.avg.Value = avg_grades.WhereCountsInAVG().Average(x => x.DecimalValue)!.Value;
            }catch(InvalidOperationException)
            {
                return null;
            }

            @this.DataContext = @this;

            @this.Subject.Text = subject.Description;
            @this.Teachers.Text = subject.TeachersString.ToTitle();

            foreach (var g in grades)
                @this.GradeStack.Children.Add(new CVGrade(g));

            if (@this.avg.Value < 5)
            {
                @this.avg.SetThemeBinding(CVProgressEllipse.PercentageColorProperty, BaseTheme.CV_GRADE_INSUFFICIENT_PATH);
                @this.avg.SetThemeBinding(CVProgressEllipse.BackgroundColorProperty, BaseTheme.CV_GRADE_INSUFFICIENT_BG_PATH);
            }
            else if (@this.avg.Value < 6)
            {
                @this.avg.SetThemeBinding(CVProgressEllipse.PercentageColorProperty, BaseTheme.CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH);
                @this.avg.SetThemeBinding(CVProgressEllipse.BackgroundColorProperty, BaseTheme.CV_GRADE_SLIGHTLY_INSUFFICIENT_BG_PATH);
            }
            else
            {
                @this.avg.SetThemeBinding(CVProgressEllipse.PercentageColorProperty, BaseTheme.CV_GRADE_SUFFICIENT_PATH);
                @this.avg.SetThemeBinding(CVProgressEllipse.BackgroundColorProperty, BaseTheme.CV_GRADE_SUFFICIENT_BG_PATH);
            }

            if (avg_grades.Length > 1)
            {
                var g1 = avg_grades[avg_grades.Length - 2];
                var g2 = avg_grades[avg_grades.Length - 1];

                var control = new ContentControl()
                {
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(0, 0, 5, 0)
                };
                @this.grid.Children.Add(control);
                Grid.SetColumn(control, 3);
                Grid.SetRowSpan(control, 2);

                if (g1.DecimalValue == g2.DecimalValue)
                {
                    control.Template = (ControlTemplate)Application.Current.FindResource("TrendStableIcon");
                    control.SetThemeBinding(ContentControl.BackgroundProperty, g1.InternalColorPath);
                }
                else
                {
                    control.Template = (ControlTemplate)Application.Current.FindResource("TrendDownIcon");

                    if (g1.DecimalValue > g2.DecimalValue)
                    {
                        control.LayoutTransform = new RotateTransform(180, 0.5, 0.5);
                        control.SetThemeBinding(ContentControl.BackgroundProperty, BaseTheme.CV_GRADE_SUFFICIENT_PATH);
                    }else
                        control.SetThemeBinding(ContentControl.BackgroundProperty, BaseTheme.CV_GRADE_INSUFFICIENT_PATH);
                }
            }

            @this.SubjectColor.Fill = new SolidColorBrush(DayOverview.COLORS[subject.ColorID]);

            return @this;
        }

        private void OnExpand(object sender, MouseButtonEventArgs e)
        {
            this.Expanded = !this.Expanded;
        }
    }
}
