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
    public partial class CVGradesViewer : DFInjectable, IOnKeyDown
    {
        public static readonly DependencyProperty SelectedGradeProperty;
        public static readonly DependencyProperty SelectedSectionProperty;

        private SemaphoreSlim ReloadLock = new SemaphoreSlim(1, 1);

        public CVGrade? SelectedGrade
        {
            get => (CVGrade?)GetValue(SelectedGradeProperty);
            set
            {
                SetValue(SelectedGradeProperty, value);
                this.s_fp_wp.Children.Clear();
                this.s_lp_wp.Children.Clear();

                if (value is not null)
                {
                    foreach (var grade in CVRegistry.INSTANCE!.CachedGrades.Where(x => x.SubjectId == value.Grade.SubjectId).OnlyDisplayable())
                        (grade.PeriodDesc == CVRegistry.INSTANCE!.FirstPeriodName ? s_fp_wp : s_lp_wp).Children.Add(new CVGrade(grade));
                }
            }
        }

        public FrameworkElement SelectedSection
        {
            get => (FrameworkElement)GetValue(SelectedSectionProperty);
            set => SetValue(SelectedSectionProperty, value);
        }

        static CVGradesViewer()
        {
            SelectedGradeProperty = DependencyProperty.Register("SelectedGrade", typeof(CVGrade), typeof(CVGradesViewer), new PropertyMetadata(null));
            SelectedSectionProperty = DependencyProperty.Register("SelectedSection", typeof(FrameworkElement), typeof(CVGradesViewer), new PropertyMetadata(null));
        }

        public CVGradesViewer() : base()
        {
            InitializeComponent();
            this.DataFetched = true;

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
            var block2empty = true;
            this.FirstPeriodStack.Children.Clear();
            this.LastPeriodStack.Children.Clear();

            foreach (var grade in from subject in CVRegistry.INSTANCE!.CachedSubjects
                                  join grade in CVRegistry.INSTANCE!.CachedGrades.OnlyDisplayable() on subject.Id equals grade.SubjectId
                                  group grade by (grade.PeriodDesc, subject))
            {
                CVSubjectGrades? tmp = CVSubjectGrades.New(grade.ToArray(), grade.Key.subject);
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

                lbl.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVGenericGrayFontProperty);
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
            @new.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMultiMenuFontSelectedProperty);
            old.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMultiMenuFontUnselectedProperty);
        }

        private void InitLabels()
        {
            var iterator = this.labels.Children.OfType<Label>().GetEnumerator();
            iterator.MoveNext();
            iterator.Current.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMultiMenuFontSelectedProperty);

            while (iterator.MoveNext())
                iterator.Current.SetThemeBinding(Label.ForegroundProperty, ThemeProperties.CVMultiMenuFontUnselectedProperty);
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var labels = this.labels.Children.OfType<Label>().ToArray();
            var old = this.GetSelectedLabel();
            if (ReferenceEquals(sender, old))
                return;

            var idx = labels.ReferenceIndexOf(sender);
            this.SelectedSection = (FrameworkElement)((StackPanel)this.SectionsWP.Children[idx]).Children[0];
            SelectLabel((Label)sender, old);

            this.Scroller.ScrollToVerticalOffset(0);
            this.Scroller.ScrollToHorizontalOffset(this.Scroller.ActualWidth * idx);
        }

        private async Task Reload()
        {
            if (ReloadLock.CurrentCount == 0)
                return;

            this.DataFetched = false;

            try
            {
                await ReloadLock.WaitAsync();

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
            this.DataFetched = true;

            this.Update();
        }

        private async void ReloadBtn(object sender, MouseButtonEventArgs e) => await Reload();

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.F5)
            {
                Task.Run(Reload);
                return;
            }

            if (e.Key is not Key.Left && e.Key is not Key.Right)
                return;

            var labels = this.labels.Children.OfType<Label>().ToArray();
            var old = this.GetSelectedLabel();
            var idx = labels.ReferenceIndexOf(old);

            if (e.Key is Key.Left)
                idx--;
            else 
                idx++;

            idx = idx == -1 ? 2 : idx % 3;

            this.SelectedSection = (FrameworkElement)((StackPanel)this.SectionsWP.Children[idx]).Children[0];
            SelectLabel(labels[idx], old);

            this.Scroller.ScrollToVerticalOffset(0);
            this.Scroller.ScrollToHorizontalOffset(this.Scroller.ActualWidth * idx);
        }
    }
}
