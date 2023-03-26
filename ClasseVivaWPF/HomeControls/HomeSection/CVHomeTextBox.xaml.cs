using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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

        static CVHomeTextBox()
        {
            SubjectProperty = DependencyProperty.Register("Title", typeof(string), typeof(CVHomeTextBox));
            HourProperty = DependencyProperty.Register("Hour", typeof(int?), typeof(CVHomeTextBox));
            HoursProperty = DependencyProperty.Register("Hours", typeof(int?), typeof(CVHomeTextBox));
            Row2Property = DependencyProperty.Register("Row2", typeof(string), typeof(CVHomeTextBox), new PropertyMetadata(""));
            FillerColorProperty = DependencyProperty.Register("FillerColor", typeof(Color), typeof(CVHomeTextBox), new PropertyMetadata(Config.OPAQUE_WHITE));
            BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(CVHomeTextBox), new PropertyMetadata(Config.OPAQUE_WHITE));
            FontColorProperty = DependencyProperty.Register("FontColor", typeof(SolidColorBrush), typeof(CVHomeTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        }

        internal CVHomeTextBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void ParseText(string text, InlineCollection target, string init_value = "")
        {
            Hyperlink uri;

            foreach (var block in Regex.Split(text, "(?<=[\n ])"))
            {
                if (URL_REGEX.Match(block).Success)
                {
                    if (init_value != "")
                    {
                        target.Add(new Run() { Text = init_value, Foreground = this.FontColor });
                        init_value = "";
                    }

                    uri = new Hyperlink(new Run() { Text = block })
                    {
                        NavigateUri = new Uri(block),
                        TextDecorations = null,
                        Foreground = Config.CV_RED_BRUSH,
                    };
                    uri.RequestNavigate += OpenUrl;
                    target.Add(uri);
                }
                else
                    init_value += block;
            }

            if (init_value != "")
                target.Add(new Run() { Text = init_value, Foreground = this.FontColor });
        }

        private static void OpenUrl(object sender, RequestNavigateEventArgs e)
        {
            e.Uri.SystemOpening();
        }

        public static CVHomeTextBox FromLesson(Lesson lesson)
        {
            CVHomeTextBox @this = new();
            @this.Title = lesson.SubjectDesc;
            @this.Hour = lesson.EvtHPos;
            @this.Hours = lesson.EvtDuration;
            @this.Row2 = lesson.AuthorName.ToTitle();
            @this.FillerColor = DayOverview.COLORS[lesson.ColorID];

            if (lesson.LessonArg == "")
            {
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
                @this.line.Visibility = Visibility.Hidden;
                @this.line.Height = 0;
                @this.last_wp.Visibility = Visibility.Hidden;
                @this.last_wp.Height = 0;

                @this.Row2Control.VerticalAlignment = VerticalAlignment.Bottom;
            }
            else
            {
                @this.lesson_txt.Inlines.Add(new Run()
                {
                    Text = lesson.LessonType,
                    Foreground = new SolidColorBrush(@this.FillerColor)
                });
                @this.ParseText(lesson.LessonArg, @this.lesson_txt.Inlines, ": ");
            }
            @this.LowerImgWP.Height = 0;
            return @this;
        }

        public static CVHomeTextBox FromAgendaEvent(AgendaEvent e, EventAppCategory category)
        {
            CVHomeTextBox @this = new();
            @this.Title = category is EventAppCategory.Agenda ? e.AuthorName : e.SubjectDesc!;
            if (category is EventAppCategory.Homework && !e.IsFullDay)
                @this.Row2 = $"{e.EvtDatetimeBegin:HH:mm} - {e.EvtDatetimeEnd:HH:mm}";
            else
            {
                Grid.SetRowSpan(@this.TitleControl, 2);
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
            }

            @this.FillerColor = Colors.WhiteSmoke;
            
            @this.ParseText(e.Notes, @this.lesson_txt.Inlines);
            @this.LowerImgWP.Height = 0;
            return @this;
        }

        public static CVHomeTextBox FromEvent(Event e)
        {
            CVHomeTextBox @this = new();
            if (e.IsAbsence)
            {
                @this.FillerColor = @this.BackgroundColor = Color.FromRgb(0xD0, 0x5A, 0x50);
                @this.Title = "Assenze";
            }
            else if (e.IsEarlyExit)
            {
                @this.FillerColor = @this.BackgroundColor = Color.FromRgb(0xDB, 0xB6, 0x3B);
                // 0xC5, 0xA5, 0x35
                @this.Title = "Uscita anticipata";
            }
            else if (e.IsLate || e.IsShortLate)
            {
                @this.Title = "Ritardi";
                @this.FillerColor = @this.BackgroundColor = Color.FromRgb(0xEB, 0x98, 50);
            }
            else
                throw new Exception();

            @this.FontColor = new SolidColorBrush(Colors.White);
           
            @this.lesson_txt.Inlines.Add(new Run() { Text = e.IsJustified ? e.JustifReasonDesc : "Da giustificare", Foreground = @this.FontColor });
            
            if (e.EvtHPos.HasValue)
                @this.Row2 = $"A {e.EvtHPos}° ora";
            else
                Grid.SetRowSpan(@this.TitleControl, 2);

            @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
            @this.LowerImgWP.Height = 0;

            return @this;
        }

        public static CVHomeTextBox FromGrade(Grade evt)
        {
            CVHomeTextBox @this = new();
            @this.TitleControl.Foreground = new SolidColorBrush(Colors.White);

            @this.FillerColor = @this.BackgroundColor = evt.GetColor();
            @this.Title = evt.SubjectDesc.ToTitle();

            if (evt.NotesForFamily == "")
            {
                Grid.SetRowSpan(@this.TitleControl, 2);
                @this.TitleControl.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                @this.Row2 = evt.NotesForFamily;
                @this.Row2Control.Foreground = @this.TitleControl.Foreground;
            }

            @this.lesson_txt.Inlines.Add(new Run() { FontSize = 24, Foreground = @this.TitleControl.Foreground, Text = evt.DisplayValue, FontWeight = FontWeights.SemiBold });
            @this.lesson_txt.Inlines.Add(new Run() { Foreground = @this.TitleControl.Foreground, Text = $"\t{evt.ComponentDesc}", BaselineAlignment = BaselineAlignment.Center, FontSize = 16 });
            @this.UpperImgWP.Visibility = Visibility.Hidden;

            return @this;
        }

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

        public Color FillerColor
        {
            get => (Color)base.GetValue(FillerColorProperty);
            set => base.SetValue(FillerColorProperty, value);
        }
        public Color BackgroundColor
        {
            get => (Color)base.GetValue(BackgroundColorProperty);
            set => base.SetValue(BackgroundColorProperty, value);
        }

        public SolidColorBrush FontColor
        {
            get => (SolidColorBrush)base.GetValue(FontColorProperty);
            set => base.SetValue(FontColorProperty, value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(this.lesson_txt);
        }
    }
}
