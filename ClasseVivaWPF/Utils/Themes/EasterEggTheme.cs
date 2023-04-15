using System;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public class EasterEggTheme : BaseTheme // For debug lol
    {
        private static Random random = new Random();
        
        public override Color CV_MAIN_MENU_ICON_SELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_MAIN_MENU_ICON_UNSELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_NOTE { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_INSUFFICIENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_SUFFICIENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_INSUFFICIENT_BG { get; } = Color.FromArgb(0xAF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_SLIGHTLY_INSUFFICIENT_BG { get; } = Color.FromArgb(0xAF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_SUFFICIENT_BG { get; } = Color.FromArgb(0xAF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_RED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_GRAY { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_BACKGROUND { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_OPAQUE_BACKGROUND { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_URI { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_HOME_CURRENT_DAY { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_GRAY_FONT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_HEADER { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_SPINNER { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_TEXT_SELECTION { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_CHECK_BOX_ELLIPSE_SELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_CHECK_BOX_ELLIPSE_UNSELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_BUTTON { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_CALENDAR { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_PERCENTAGE_BACKGROUND { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_ABSENCES_ABSENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_ABSENCES_PARTIALLY_ABSENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_ABSENCES_EARlY_EXIT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_ABSENCES_LATE { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADES_FILTER { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_MULTI_MENU_FONT_SELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_MULTI_MENU_FONT_UNSELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_MULTI_MENU_FONT_SLIDER { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_BACK_ICON { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_HR { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_SETTINGS_SECTION_HEADER { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_DAY_BG_UNSELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_DAY_BG_SELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_DAY_TEXT_SELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_DAY_TEXT_UNSELECTED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_FONT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_TEXT_BOX_BACKGROUND { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_HEADER_FONT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_DAY_HOME_CONTAINER { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_SETTINGS_TEXT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_EXTRA_HEADER_ELLIPSE { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_EXTRA_INTERACT_ICONS { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
    
        public EasterEggTheme() : base(false){

        }
    }
}
