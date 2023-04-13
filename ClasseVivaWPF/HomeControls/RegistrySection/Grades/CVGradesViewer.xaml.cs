using ClasseVivaWPF.SharedControls;
using ClasseVivaWPF.Utils.Themes;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
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
        public static readonly DependencyProperty SelectedSectionProperty;

        public CVGrade? SelectedGrade
        {
            get => (CVGrade?)base.GetValue(SelectedSubjectProperty);
            set
            {
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

        public FrameworkElement SelectedSection
        {
            get => (FrameworkElement)base.GetValue(SelectedSectionProperty);
            set => base.SetValue(SelectedSectionProperty, value);
        }

        static CVGradesViewer()
        {
            SelectedSubjectProperty = DependencyProperty.Register("SelectedSubject", typeof(CVGrade), typeof(CVGradesViewer), new PropertyMetadata(null));
            SelectedSectionProperty = DependencyProperty.Register("SelectedSection", typeof(FrameworkElement), typeof(CVGradesViewer), new PropertyMetadata(null));
        }

        public CVGradesViewer()
        {
            InitializeComponent();

            this.Update();
            this.DataContext = this;
            this.InitLabels();
            this.SelectedSection = (FrameworkElement)this.SectionsWP.Children[0];

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
            UpdateBlock2();
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

                btn = new CVButton()
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

        private void UpdateBlock2()
        {
            this.FirstPeriodStack.Children.Add(CVSubjectGrades.New(CVRegistry.INSTANCE!.CachedGrades));
            this.FirstPeriodStack.Children.Add(CVSubjectGrades.New(CVRegistry.INSTANCE!.CachedGrades));
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

            this.Dispatcher.BeginInvoke(() => this.Scroller.ScrollToVerticalOffset(0));
            this.SelectedSection = (FrameworkElement)((StackPanel)e.SnappendElement).Children[0];
            var labels = this.labels.Children.OfType<Label>().ToArray();
            SelectLabel(labels[e.Index], labels[e.OldIndex]);
        }

        private Label GetSelectedLabel()
        {
            return this.labels.Children.OfType<Label>().Where(x => ((SolidColorBrush)x.Foreground).Color == Colors.White).First();
        }

        private void SelectLabel(Label @new, Label old)
        {
            @new.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MULTI_MENU_FONT_SELECTED_PATH);
            old.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MULTI_MENU_FONT_UNSELECTED_PATH);
        }

        private void InitLabels()
        {
            var iterator = this.labels.Children.OfType<Label>().GetEnumerator();
            iterator.MoveNext();
            iterator.Current.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MULTI_MENU_FONT_SELECTED_PATH);

            while (iterator.MoveNext())
                iterator.Current.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_MULTI_MENU_FONT_UNSELECTED_PATH);
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var labels = this.labels.Children.OfType<Label>().ToArray();
            var old = labels.Where(x => ((SolidColorBrush)x.Foreground).Color == MainWindow.INSTANCE.CurrentTheme.CV_MULTI_MENU_FONT_SELECTED).First();
            var idx = labels.ReferenceIndexOf(sender);
            this.SelectedSection = (FrameworkElement)((StackPanel)this.SectionsWP.Children[idx]).Children[0];
            SelectLabel((Label)sender, old);

            this.Scroller.ScrollToVerticalOffset(0);
            this.Scroller.ScrollToHorizontalOffset(this.Scroller.ActualWidth * idx);
        }
    }
}
