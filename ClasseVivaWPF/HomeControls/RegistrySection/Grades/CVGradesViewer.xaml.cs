using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Grades
{

    /*<ComboBox SelectedIndex = "0" >
        < ComboBoxItem Tag="{x:Static local:GroupGradesBy.Period}" Content="Periodo"></ComboBoxItem>
        <ComboBoxItem Tag = "{x:Static local:GroupGradesBy.Date}"  Content="Data"></ComboBoxItem>
    </ComboBox>
    public enum GroupGradesBy
    {
        Period,
        Date
    }*/

    /// <summary>
    /// Logica di interazione per CVGradesViewer.xaml
    /// </summary>
    /// 
    public partial class CVGradesViewer : UserControl
    {
        public static readonly DependencyProperty SelectedSubjectProperty;

        public CVGrade? SelectedGrade
        {
            get => (CVGrade?)base.GetValue(SelectedSubjectProperty);
            set {
                base.SetValue(SelectedSubjectProperty, value);
                this.s_fp_wp.Children.Clear();
                this.s_lp_wp.Children.Clear();

                if (value is not null)
                {
                    foreach (var grade in CVRegistry.INSTANCE!.CachedGrades.Where(x => x.SubjectId == this.SelectedGrade.Grade.SubjectId))
                        (grade.PeriodDesc == CVRegistry.INSTANCE!.FirstPeriodName ? s_fp_wp : s_lp_wp).Children.Add(new CVGrade(grade));
                }
            }
        }

        static CVGradesViewer()
        {
            SelectedSubjectProperty = DependencyProperty.Register("SelectedSubject", typeof(CVGrade), typeof(CVGradesViewer), new PropertyMetadata(null));
        }

        public CVGradesViewer()
        {
            InitializeComponent();

            this.Update();
            this.DataContext = this;

            this.Scroller.SizeChanged += (s, e) => {
                if (this.Scroller.HorizontalOffset != 0)
                {
                    var idx = this.labels.Children.ReferenceIndexOf(GetSelectedLabel()) - 1;
                    Scroller.ScrollToHorizontalOffset(this.Scroller.ActualWidth * idx);
                }
            };
        }

        private void Update()
        {
            UpdateBlock1();
        }

        private void UpdateBlock1()
        {
            CVGrade tmp;
            Button btn;
            string h;

            foreach (var grade in CVRegistry.INSTANCE!.CachedGrades)
            {
                this.GradeStack.Children.Add(tmp = new(grade));

                h = grade.SubjectDesc.ToTitle();
                if (h.Length > 32)
                    h = grade.SubjectAcronym;

                btn = new Button()
                {
                    Margin = new Thickness(10, 0, 0, 15),
                    Content = $"Visualizza tutti i voti in {h}",
                    VerticalAlignment = VerticalAlignment.Top,
                    Tag = tmp,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                btn.Click += OnButtonClick;
                this.GradeStack.Children.Add(btn);
            }

            this.s_p1.Content = this.p1.Content = CVRegistry.INSTANCE!.FirstPeriodName;
            this.s_p2.Content = this.p2.Content = CVRegistry.INSTANCE!.LastPeriodName;
        }


        public void OnClose(object sender, RoutedEventArgs e) => this.Close();

        public void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }

        public void OnButtonClick(object sender, RoutedEventArgs e)
        {
            this.SelectedGrade = (CVGrade)((Button)sender).Tag;
        }

        private void Scroller_OnSnap(object sender, SnapEventArgs e)
        {
            if (e.OldIndex == e.Index)
                return;

            var labels = this.labels.Children.OfType<Label>().ToArray();
            labels[e.OldIndex].Foreground = new SolidColorBrush(Color.FromArgb(0xAF, 0xFF, 0xFF, 0xFF));
            labels[e.Index].Foreground = new SolidColorBrush(Colors.White);
        }

        private Label GetSelectedLabel()
        {
            return this.labels.Children.OfType<Label>().Where(x => ((SolidColorBrush)x.Foreground).Color == Colors.White).First();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var labels = this.labels.Children.OfType<Label>().ToArray();
            labels.Where(x => ((SolidColorBrush)x.Foreground).Color == Colors.White).First().Foreground = new SolidColorBrush(Color.FromArgb(0xAF, 0xFF, 0xFF, 0xFF));
            ((Label)sender).Foreground = new SolidColorBrush(Colors.White);
            var idx = labels.ReferenceIndexOf(sender);

            this.Scroller.ScrollToHorizontalOffset(this.Scroller.ActualWidth * idx);
        }
    }
}
