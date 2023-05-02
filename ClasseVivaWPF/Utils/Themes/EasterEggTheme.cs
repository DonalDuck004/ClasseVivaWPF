using System;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public class EasterEggTheme : ITheme // For debug lol
    {
        private static Random rnd = new Random();
        public string Name { get; } = "EasterEgg";
        public bool Hidden { get; } = true;

        public Color CV_MAIN_MENU_ICON_SELECTED { get; } = rnd.NextColor();
        public Color CV_MAIN_MENU_ICON_UNSELECTED { get; } = rnd.NextColor();
        public Color CV_GRADE_NOTE { get; } = rnd.NextColor();
        public Color CV_GRADE_INSUFFICIENT { get; } = rnd.NextColor();
        public Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; } = rnd.NextColor();
        public Color CV_GRADE_SUFFICIENT { get; } = rnd.NextColor();
        public Color CV_GRADE_INSUFFICIENT_BG { get; } = rnd.NextColor();
        public Color CV_GRADE_SLIGHTLY_INSUFFICIENT_BG { get; } = rnd.NextColor();
        public Color CV_GRADE_SUFFICIENT_BG { get; } = rnd.NextColor();
        public Color CV_GENERIC_RED { get; } = rnd.NextColor();
        public Color CV_GENERIC_GRAY { get; } = rnd.NextColor();
        public Color CV_GENERIC_BACKGROUND { get; } = rnd.NextColor();
        public Color CV_GENERIC_OPAQUE_BACKGROUND { get; } = rnd.NextColor();
        public Color CV_URI { get; } = rnd.NextColor();
        public Color CV_HOME_CURRENT_DAY { get; } = rnd.NextColor();
        public Color CV_GENERIC_GRAY_FONT { get; } = rnd.NextColor();
        public Color CV_HEADER { get; } = rnd.NextColor();
        public Color CV_SPINNER { get; } = rnd.NextColor();
        public Color CV_GENERIC_TEXT_SELECTION { get; } = rnd.NextColor();
        public Color CV_CHECK_BOX_ELLIPSE_SELECTED { get; } = rnd.NextColor();
        public Color CV_CHECK_BOX_ELLIPSE_UNSELECTED { get; } = rnd.NextColor();
        public Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED { get; } = rnd.NextColor();
        public Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED { get; } = rnd.NextColor();
        public Color CV_BUTTON { get; } = rnd.NextColor();
        public Color CV_CALENDAR { get; } = rnd.NextColor();
        public Color CV_PERCENTAGE_BACKGROUND { get; } = rnd.NextColor();
        public Color CV_ABSENCES_ABSENT { get; } = rnd.NextColor();
        public Color CV_ABSENCES_PARTIALLY_ABSENT { get; } = rnd.NextColor();
        public Color CV_ABSENCES_EARlY_EXIT { get; } = rnd.NextColor();
        public Color CV_ABSENCES_LATE { get; } = rnd.NextColor();
        public Color CV_GRADES_FILTER { get; } = rnd.NextColor();
        public Color CV_MULTI_MENU_FONT_SELECTED { get; } = rnd.NextColor();
        public Color CV_MULTI_MENU_FONT_UNSELECTED { get; } = rnd.NextColor();
        public Color CV_MULTI_MENU_FONT_SLIDER { get; } = rnd.NextColor();
        public Color CV_BACK_ICON { get; } = rnd.NextColor();
        public Color CV_HR { get; } = rnd.NextColor();
        public Color CV_SETTINGS_SECTION_HEADER { get; } = rnd.NextColor();
        public Color CV_DAY_BG_UNSELECTED { get; } = rnd.NextColor();
        public Color CV_DAY_BG_SELECTED { get; } = rnd.NextColor();
        public Color CV_DAY_TEXT_SELECTED { get; } = rnd.NextColor();
        public Color CV_DAY_TEXT_UNSELECTED { get; } = rnd.NextColor();
        public Color CV_GENERIC_FONT { get; } = rnd.NextColor();
        public Color CV_TEXT_BOX_BACKGROUND { get; } = rnd.NextColor();
        public Color CV_GENERIC_HEADER_FONT { get; } = rnd.NextColor();
        public Color CV_DAY_HOME_CONTAINER { get; } = rnd.NextColor();
        public Color CV_SETTINGS_TEXT { get; } = rnd.NextColor();
        public Color CV_EXTRA_HEADER_ELLIPSE { get; } = rnd.NextColor();
        public Color CV_EXTRA_INTERACT_ICONS { get; } = rnd.NextColor();
        public Color CV_GRADE_FONT { get; } = rnd.NextColor();
        public Color CV_GRADE_GRV2 { get; } = rnd.NextColor();
        public Color CV_ABSENCES_FONT { get; } = rnd.NextColor();
        public Color CV_ACCOUNT_BUBBLE_FONT { get; } = rnd.NextColor();
        public Color CV_ACCOUNT_BUBBLE { get; } = rnd.NextColor();
        public Color CV_ABSENCES_PRESENT { get; } = rnd.NextColor();
        public Color CV_ABSENCES_CALENDAR_HAS_EVENT_FONT { get; } = rnd.NextColor();
        public Color CV_ABSENCES_CALENDAR_NO_EVENT_FONT { get; } = rnd.NextColor();
        public Color CV_DIDATICS_ICONS { get; } = rnd.NextColor();
        public Color CV_DIDATICS_FOLDER { get; } = rnd.NextColor();
        public Color CV_DIDATICS_TEACHERS { get; } = rnd.NextColor();
    }
}
