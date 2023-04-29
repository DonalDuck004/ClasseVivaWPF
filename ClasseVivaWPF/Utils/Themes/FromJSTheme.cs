using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public class FromJSTheme : ITheme
    {
        public required string Name { get; init; }
        public bool Hidden { get; } = false;

        public required Color CV_GRADE_INSUFFICIENT { get; init; } 
        public required Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; init; } 
        public required Color CV_GRADE_SUFFICIENT { get; init; } 
        public required Color CV_GRADE_INSUFFICIENT_BG { get; init; } 
        public required Color CV_GRADE_SLIGHTLY_INSUFFICIENT_BG { get; init; } 
        public required Color CV_GRADE_SUFFICIENT_BG { get; init; } 
        public required Color CV_MAIN_MENU_ICON_SELECTED { get; init; } 
        public required Color CV_MAIN_MENU_ICON_UNSELECTED { get; init; } 
        public required Color CV_GENERIC_RED { get; init; } 
        public required Color CV_TEXT_BOX_BACKGROUND { get; init; } 
        public required Color CV_CALENDAR { get; init; } 
        public required Color CV_BUTTON { get; init; } 
        public required Color CV_HEADER { get; init; } 
        public required Color CV_GENERIC_GRAY { get; init; } 
        public required Color CV_GENERIC_BACKGROUND { get; init; } 
        public required Color CV_GENERIC_OPAQUE_BACKGROUND { get; init; } 
        public required Color CV_URI { get; init; } 
        public required Color CV_HOME_CURRENT_DAY { get; init; } 
        public required Color CV_GENERIC_GRAY_FONT { get; init; } 
        public required Color CV_SPINNER { get; init; } 
        public required Color CV_GENERIC_TEXT_SELECTION { get; init; } 
        public required Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED { get; init; } 
        public required Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED { get; init; } 
        public required Color CV_CHECK_BOX_ELLIPSE_SELECTED { get; init; } 
        public required Color CV_CHECK_BOX_ELLIPSE_UNSELECTED { get; init; } 
        public required Color CV_PERCENTAGE_BACKGROUND { get; init; } 
        public required Color CV_ABSENCES_ABSENT { get; init; } 
        public required Color CV_ABSENCES_PARTIALLY_ABSENT { get; init; } 
        public required Color CV_ABSENCES_EARlY_EXIT { get; init; } 
        public required Color CV_ABSENCES_LATE { get; init; } 
        public required Color CV_GRADES_FILTER { get; init; } 
        public required Color CV_MULTI_MENU_FONT_SELECTED { get; init; } 
        public required Color CV_MULTI_MENU_FONT_UNSELECTED { get; init; } 
        public required Color CV_MULTI_MENU_FONT_SLIDER { get; init; } 
        public required Color CV_BACK_ICON { get; init; } 
        public required Color CV_SETTINGS_SECTION_HEADER { get; init; } 
        public required Color CV_HR { get; init; } 
        public required Color CV_DAY_TEXT_UNSELECTED { get; init; } 
        public required Color CV_DAY_TEXT_SELECTED { get; init; } 
        public required Color CV_DAY_BG_UNSELECTED { get; init; } 
        public required Color CV_DAY_BG_SELECTED { get; init; } 
        public required Color CV_GENERIC_FONT { get; init; } 
        public required Color CV_GENERIC_HEADER_FONT { get; init; } 
        public required Color CV_DAY_HOME_CONTAINER { get; init; } 
        public required Color CV_SETTINGS_TEXT { get; init; } 
        public required Color CV_EXTRA_HEADER_ELLIPSE { get; init; } 
        public required Color CV_EXTRA_INTERACT_ICONS { get; init; } 
        public required Color CV_GRADE_NOTE { get; init; } 
        public required Color CV_GRADE_FONT { get; init; } 
        public required Color CV_GRADE_GRV2 { get; init; } 
        public required Color CV_ABSENCES_FONT { get; init; } 
        public required Color CV_ACCOUNT_BUBBLE_FONT { get; init; } 
        public required Color CV_ACCOUNT_BUBBLE { get; init; } 
        public required Color CV_ABSENCES_PRESENT { get; init; } 
        public required Color CV_ABSENCES_CALENDAR_HAS_EVENT_FONT { get; init; } 
        public required Color CV_ABSENCES_CALENDAR_NO_EVENT_FONT { get; init; } 
    }
}
