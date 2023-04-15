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
using ClasseVivaWPF.Api.Types;
using System.Threading;
using ClasseVivaWPF.Api;

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
    public partial class CVGradesViewer : Injectable
    {
        public static readonly DependencyProperty SelectedGradeProperty;
        public static readonly DependencyProperty SelectedSectionProperty;
        public static readonly DependencyProperty DataFetchedProperty;

        private SemaphoreSlim ReloadLock = new SemaphoreSlim(1, 1);

        public CVGrade? SelectedGrade
        {
            get => (CVGrade?)base.GetValue(SelectedGradeProperty);
            set
            {
                base.SetValue(SelectedGradeProperty, value);
                this.s_fp_wp.Children.Clear();
                this.s_lp_wp.Children.Clear();

                if (value is not null)
                {
                    foreach (var grade in CVRegistry.INSTANCE!.CachedGrades.Where(x => x.SubjectId == value.Grade.SubjectId).OnlyDisplayable())
                        (grade.PeriodDesc == CVRegistry.INSTANCE!.FirstPeriodName ? s_fp_wp : s_lp_wp).Children.Add(new CVGrade(grade));
                }
            }
        }
        public bool DataFetched
        {
            get => (bool)base.GetValue(DataFetchedProperty);
            set => base.SetValue(DataFetchedProperty, value);
        }

        public FrameworkElement SelectedSection
        {
            get => (FrameworkElement)base.GetValue(SelectedSectionProperty);
            set => base.SetValue(SelectedSectionProperty, value);
        }

        static CVGradesViewer()
        {
            SelectedGradeProperty = DependencyProperty.Register("SelectedGrade", typeof(CVGrade), typeof(CVGradesViewer), new PropertyMetadata(null));
            SelectedSectionProperty = DependencyProperty.Register("SelectedSection", typeof(FrameworkElement), typeof(CVGradesViewer), new PropertyMetadata(null));
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVGradesViewer), new PropertyMetadata(true));
        }

        public CVGradesViewer() : base()
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
            this.GradeStack.Children.Clear();

            foreach (var grade in CVRegistry.INSTANCE!.CachedGrades)
            {
                if (grade.EvtCode == BaseEvent.GRADE_GRADE_UNKNOW2)
                {
                    this.GradeStack.Children.Add(new CVGradeGRV2(grade));
                    continue;
                }

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
            CVSubjectGrades? tmp;
            var block2empty = true;
            this.FirstPeriodStack.Children.Clear();
            this.LastPeriodStack.Children.Clear();

            foreach (var grade in from subject in CVRegistry.INSTANCE!.CachedSubjects
                                  join grade in CVRegistry.INSTANCE!.CachedGrades.OnlyDisplayable() on subject.Id equals grade.SubjectId
                                  group grade by (grade.PeriodDesc, subject))
            {
                tmp = CVSubjectGrades.New(grade.ToArray(), grade.Key.subject);
                if (tmp is null)
                    continue;

                if (grade.Key.PeriodDesc == this.p1.Content.ToString())
                    this.FirstPeriodStack.Children.Add(tmp);
                else
                {
                    block2empty = false;
                    this.LastPeriodStack.Children.Add(tmp);
                }
            }

            if (block2empty)
            {
                Label lbl;

                this.LastPeriodStack.Children.Add(lbl = new()
                {
                    Content = "Nessun voto disponibile",
                    FontSize = 48,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                });

                lbl.SetThemeBinding(Label.ForegroundProperty, BaseTheme.CV_GENERIC_GRAY_FONT_PATH);
                BindingOperations.SetBinding(lbl, Label.HeightProperty, new Binding()
                {
                    Source = this.Scroller,
                    Path = new("ActualHeight")
                });
            }
        }

        public void OnClose(object sender, RoutedEventArgs e) => this.Close();

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
            return this.labels.Children.OfType<Label>().Where(
                x => ((SolidColorBrush)x.Foreground).Color == MainWindow.INSTANCE!.CurrentTheme.CV_MULTI_MENU_FONT_SELECTED
            ).First();
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
            if (ReferenceEquals(sender, old))
                return;

            var idx = labels.ReferenceIndexOf(sender);
            this.SelectedSection = (FrameworkElement)((StackPanel)this.SectionsWP.Children[idx]).Children[0];
            SelectLabel((Label)sender, old);

            this.Scroller.ScrollToVerticalOffset(0);
            this.Scroller.ScrollToHorizontalOffset(this.Scroller.ActualWidth * idx);
        }

        private async void CVReloadButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ReloadLock.CurrentCount == 0)
                return;

            try
            {
                await ReloadLock.WaitAsync();

                this.DataFetched = false;

                try
                {
                    CVRegistry.INSTANCE!.CachedSubjects = (await Client.INSTANCE.GetSubjects()).ContentSubjects;
                    CVRegistry.INSTANCE!.CachedGrades = (await Client.INSTANCE.GetGrades()).ContentGrades;
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
            this.DataFetched = true;
        }
    }
}
