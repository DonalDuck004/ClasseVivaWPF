using System.Diagnostics;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public abstract class BaseTheme
    {
        public const string CV_GRADE_NOTE_PATH = "CV_GRADE_NOTE";
        public const string CV_GRADE_INSUFFICIENT_PATH = "CV_GRADE_INSUFFICIENT";
        public const string CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH = "CV_GRADE_SLIGHTLY_INSUFFICIENT";
        public const string CV_GRADE_SUFFICIENT_PATH = "CV_GRADE_SUFFICIENT";
        public const string CV_GENERIC_RED_PATH = "CV_GENERIC_RED";
        public const string CV_GENERIC_GRAY_PATH = "CV_GENERIC_GRAY";
        public const string CV_GENERIC_BACKGROUND_PATH = "CV_GENERIC_BACKGROUND";
        public const string CV_GENERIC_OPAQUE_BACKGROUND_PATH = "CV_GENERIC_OPAQUE_BACKGROUND";
        public const string CV_URI_COLOR_PATH = "CV_URI_COLOR";

        public abstract Color CV_GRADE_NOTE { get; }
        public abstract Color CV_GRADE_INSUFFICIENT { get; }
        public abstract Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; }
        public abstract Color CV_GRADE_SUFFICIENT { get; }

        public abstract Color CV_URI_COLOR { get; }

        public abstract Color CV_GENERIC_RED { get; }
        public abstract Color CV_GENERIC_GRAY { get; }
        public abstract Color CV_GENERIC_BACKGROUND { get; }
        public abstract Color CV_GENERIC_OPAQUE_BACKGROUND { get; }


        public SolidColorBrush CV_GRADE_NOTE_BRUSH { get; }
        public SolidColorBrush CV_GRADE_INSUFFICIENT_BRUSH { get; }
        public SolidColorBrush CV_GRADE_SLIGHTLY_INSUFFICIENT_BRUSH { get; }
        public SolidColorBrush CV_GRADE_SUFFICIENT_BRUSH { get; }
        public SolidColorBrush CV_URI_COLOR_BRUSH { get; }
        public SolidColorBrush CV_GENERIC_RED_BRUSH { get; }
        public SolidColorBrush CV_GENERIC_GRAY_BRUSH { get; }
        public SolidColorBrush CV_GENERIC_BACKGROUND_BRUSH { get; }
        public SolidColorBrush CV_GENERIC_OPAQUE_BACKGROUND_BRUSH { get; }


        public BaseTheme()
        {
            this.CV_GRADE_NOTE_BRUSH = new SolidColorBrush(this.CV_GRADE_NOTE);
            this.CV_GRADE_INSUFFICIENT_BRUSH = new SolidColorBrush(this.CV_GRADE_INSUFFICIENT);
            this.CV_GRADE_SLIGHTLY_INSUFFICIENT_BRUSH = new SolidColorBrush(this.CV_GRADE_SLIGHTLY_INSUFFICIENT);
            this.CV_GRADE_SUFFICIENT_BRUSH = new SolidColorBrush(this.CV_GRADE_SUFFICIENT);
            this.CV_URI_COLOR_BRUSH = new SolidColorBrush(this.CV_URI_COLOR);
            this.CV_GENERIC_RED_BRUSH = new SolidColorBrush(this.CV_GENERIC_RED);
            this.CV_GENERIC_GRAY_BRUSH = new SolidColorBrush(this.CV_GENERIC_GRAY);
            this.CV_GENERIC_BACKGROUND_BRUSH = new SolidColorBrush(this.CV_GENERIC_BACKGROUND);
            this.CV_GENERIC_OPAQUE_BACKGROUND_BRUSH = new SolidColorBrush(this.CV_GENERIC_OPAQUE_BACKGROUND);
        }
    }
}
