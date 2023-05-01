using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ClasseVivaWPF.HomeControls.HomeSection
{

    /// <summary>
    /// Logica di interazione per CVHomeTextBox.xaml
    /// </summary>
    public partial class CVHomeTextBox : UserControl
    {
        private static Regex URL_REGEX = new Regex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);

        private static DependencyProperty SubjectProperty;
        private static DependencyProperty HourProperty;
        private static DependencyProperty HoursProperty;
        private static DependencyProperty Row2Property;
        private static DependencyProperty FillerColorProperty;
        private static DependencyProperty BackgroundColorProperty;
        private static DependencyProperty FontColorProperty;
        private static DependencyProperty IconTemplateProperty;

        public string Title
        {
            get => (string)base.GetValue(SubjectProperty);
            set => base.SetValue(SubjectProperty, value);
        }

        public int? Hour
        {
            get => (int?)base.GetValue(HourProperty);
            set => base.SetValue(HourProperty, value);
        }

        public int? Hours
        {
            get => (int?)base.GetValue(HoursProperty);
            set => base.SetValue(HoursProperty, value);
        }

        public string Row2
        {
            get => (string)base.GetValue(Row2Property);
            set => base.SetValue(Row2Property, value);
        }

        public SolidColorBrush FillerColor
        {
            get => (SolidColorBrush)base.GetValue(FillerColorProperty);
            set => base.SetValue(FillerColorProperty, value);
        }
        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)base.GetValue(BackgroundColorProperty);
            set => base.SetValue(BackgroundColorProperty, value);
        }

        public SolidColorBrush FontColor
        {
            get => (SolidColorBrush)base.GetValue(FontColorProperty);
            set => base.SetValue(FontColorProperty, value);
        }

        public ControlTemplate IconTemplate
        {
            get => (ControlTemplate)base.GetValue(IconTemplateProperty);
            set => base.SetValue(IconTemplateProperty, value);
        }

        static CVHomeTextBox()
        {
            SubjectProperty = DependencyProperty.Register("Title", typeof(string), typeof(CVHomeTextBox));
            HourProperty = DependencyProperty.Register("Hour", typeof(int?), typeof(CVHomeTextBox));
            HoursProperty = DependencyProperty.Register("Hours", typeof(int?), typeof(CVHomeTextBox));
            Row2Property = DependencyProperty.Register("Row2", typeof(string), typeof(CVHomeTextBox), new PropertyMetadata(""));
            FillerColorProperty = DependencyProperty.Register("FillerColor", typeof(SolidColorBrush), typeof(CVHomeTextBox));
            BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(CVHomeTextBox));
            FontColorProperty = DependencyProperty.Register("FontColor", typeof(SolidColorBrush), typeof(CVHomeTextBox));
            IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(ControlTemplate), typeof(CVHomeTextBox));
        }

        internal CVHomeTextBox()
        {
            InitializeComponent();
            this.DataContext = this;

            this.ContentTextBox.SetThemeBinding(RichTextBox.SelectionBrushProperty, ThemeProperties.CVGenericTextSelectionProperty);
        }

        private void ParseText(string text, InlineCollection target, string init_value = "")
        {
            Hyperlink uri;
            Match match;

            foreach (var block in Regex.Split(text, "(?<=[\n ])"))
            {
                if ((match = URL_REGEX.Match(block)).Success)
                {
                    if (init_value != "")
                    {
                        target.Add(new Run() { Text = init_value });
                        init_value = "";
                    }
                    
                    uri = new Hyperlink()
                    {
                        NavigateUri = new Uri(match.Value),
                        TextDecorations = null,
                        Cursor = Cursors.Hand,
                    };

                    uri.Inlines.Add(block);

                    uri.RequestNavigate += OpenUrl;
                    uri.SetThemeBinding(Hyperlink.ForegroundProperty, ThemeProperties.CVGenericRedProperty);
                    target.Add(uri);
                }
                else
                    init_value += block;
            }

            if (init_value != "")
                target.Add(new Run() { Text = init_value });
        }

        public static CVHomeTextBox FromLesson(Lesson lesson)
        {
            CVHomeTextBox @this = new();
            @this.Title = lesson.SubjectDesc;
            @this.Hour = lesson.EvtHPos;
            @this.Hours = lesson.EvtDuration;
            @this.Row2 = lesson.AuthorName.ToTitle();
            @this.FillerColor = new(DayOverview.COLORS[lesson.ColorID]);
            @this.SetThemeBinding(CVHomeTextBox.BackgroundColorProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
            @this.SetThemeBinding(CVHomeTextBox.FontColorProperty, ThemeProperties.CVGenericFontProperty);

            if (lesson.LessonArg == "")
            {
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
                @this.line.Visibility = Visibility.Hidden;
                @this.line.Height = 0;
                @this.last_wp.Visibility = Visibility.Hidden;
                @this.last_wp.Height = 0;

                @this.Row2Control.VerticalAlignment = VerticalAlignment.Bottom;
                @this.main_wp.RowDefinitions.RemoveAt(2);
                @this.main_wp.RowDefinitions.RemoveAt(2);
            }
            else
            {
                @this.lesson_txt.Inlines.Add(new Run()
                {
                    Text = lesson.LessonType,
                    Foreground = @this.FillerColor
                });
                @this.ParseText(lesson.LessonArg, @this.lesson_txt.Inlines, ": ");
            }

            @this.IconTemplate = (ControlTemplate)Application.Current.FindResource("LessonIcon");
            @this.LowerImgWP.Height = 0;

            return @this;
        }

        public static CVHomeTextBox FromAgendaEvent(AgendaEvent e, EventAppCategory category)
        {
            CVHomeTextBox @this = new();
            @this.SetThemeBinding(CVHomeTextBox.BackgroundColorProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
            @this.SetThemeBinding(CVHomeTextBox.FillerColorProperty, ThemeProperties.CVGenericOpaqueBackgroundProperty);
            @this.SetThemeBinding(CVHomeTextBox.FontColorProperty, ThemeProperties.CVGenericFontProperty);
           
            @this.Title = category is EventAppCategory.Agenda ? e.AuthorName : e.SubjectDesc!;
            if (category is EventAppCategory.Homework && !e.IsFullDay)
                @this.Row2 = $"{e.EvtDatetimeBegin:HH:mm} - {e.EvtDatetimeEnd:HH:mm}";
            else
            {
                Grid.SetRowSpan(@this.TitleControl, 2);
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
            }

            @this.IconTemplate = (ControlTemplate)Application.Current.FindResource("CalendarIcon");

            @this.ParseText(e.Notes, @this.lesson_txt.Inlines);
            @this.LowerImgWP.Height = 0;
            return @this;
        }

        public static CVHomeTextBox FromEvent(Event e)
        {
            CVHomeTextBox @this = new();
            if (e.IsAbsence)
            {
                @this.SetThemeBinding(CVHomeTextBox.BackgroundColorProperty, ThemeProperties.CVAbsencesAbsentProperty);
                @this.SetThemeBinding(CVHomeTextBox.FillerColorProperty, ThemeProperties.CVAbsencesAbsentProperty);
                @this.UpperImgWPContainer.SetThemeBinding(WrapPanel.BackgroundProperty, ThemeProperties.CVAbsencesAbsentProperty);
                @this.Title = "Assenze";
            }
            else if (e.IsEarlyExit)
            {
                @this.SetThemeBinding(CVHomeTextBox.BackgroundColorProperty, ThemeProperties.CVAbsencesEarlyExitProperty);
                @this.SetThemeBinding(CVHomeTextBox.FillerColorProperty, ThemeProperties.CVAbsencesEarlyExitProperty);
                @this.UpperImgWPContainer.SetThemeBinding(WrapPanel.BackgroundProperty, ThemeProperties.CVAbsencesEarlyExitProperty);
                @this.Title = "Uscita anticipata";
            }
            else if (e.IsLate || e.IsShortLate)
            {
                @this.SetThemeBinding(CVHomeTextBox.BackgroundColorProperty, ThemeProperties.CVAbsencesLateProperty);
                @this.SetThemeBinding(CVHomeTextBox.FillerColorProperty, ThemeProperties.CVAbsencesLateProperty);
                @this.UpperImgWPContainer.SetThemeBinding(WrapPanel.BackgroundProperty, ThemeProperties.CVAbsencesLateProperty);
                @this.Title = "Ritardi";
            }
            else
                throw new Exception();

            if (e.JustifReasonDesc == "" && e.IsJustified)
            {
                @this.line.Visibility = Visibility.Hidden;
                @this.line.Height = 0;
                @this.last_wp.Height = 0;
                @this.last_wp.Visibility = Visibility.Hidden;
                Grid.SetRowSpan(@this.UpperImgWPContainer, 2);
            }
            else
            {
                @this.lesson_txt.Inlines.Add(new Run() { Text = e.IsJustified ? e.JustifReasonDesc : "Da giustificare" });
                Grid.SetRowSpan(@this.UpperImgWPContainer, 4);
            }

            if (e.EvtHPos.HasValue)
            {
                @this.Row2 = $"A {e.EvtHPos}° ora";
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Bottom;
            }
            else
            {
                if (e.JustifReasonDesc != "" || e.IsJustified)
                    Grid.SetRowSpan(@this.TitleControl, 2);
                else
                    Grid.SetRowSpan(@this.TitleControl, 4);

                Grid.SetRowSpan(@this.UpperImgWPContainer, 4);
                
                @this.Row2Control.Height = 0;
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
            }

            @this.LowerImgWP.Height = 0;

            @this.SetThemeBinding(CVHomeTextBox.FontColorProperty, ThemeProperties.CVAbsencesFontProperty);

            @this.IconTemplate = (ControlTemplate)Application.Current.FindResource("AbsenceIcon");
            Grid.SetZIndex(@this.UpperImgWPContainer, 1);
            @this.UpperImgWP.Margin = new Thickness(0, 10, 0, 10);

            @this.last_wp.VerticalAlignment = VerticalAlignment.Top;

            return @this;
        }

        public static CVHomeTextBox FromGrade(Grade evt)
        {
            CVHomeTextBox @this = new();
            
            @this.SetThemeBinding(CVHomeTextBox.FontColorProperty, ThemeProperties.CVGradeFontProperty);
            @this.SetThemeBinding(CVHomeTextBox.FillerColorProperty, evt.InternalColorProperty);
            @this.SetThemeBinding(CVHomeTextBox.BackgroundColorProperty, evt.InternalColorProperty);


            @this.Title = evt.SubjectDesc.ToTitle();

            if (evt.NotesForFamily == "")
            {
                Grid.SetRowSpan(@this.TitleControl, 2);
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
            }
            else
                @this.Row2 = evt.NotesForFamily;

            @this.lesson_txt.Inlines.Add(new Run() { FontSize = 24, Text = evt.DisplayValue, FontWeight = FontWeights.SemiBold });
            @this.lesson_txt.Inlines.Add(new Run() { Text = $"\t{evt.ComponentDesc}", BaselineAlignment = BaselineAlignment.Center, FontSize = 16 });
            @this.UpperImgWP.Visibility = Visibility.Hidden;
            @this.IconTemplate = (ControlTemplate)Application.Current.FindResource("GradeIcon");

            return @this;
        }

        private static void OpenUrl(object sender, RequestNavigateEventArgs e)
        {
            e.Uri.SystemOpening();
        }
    }
}