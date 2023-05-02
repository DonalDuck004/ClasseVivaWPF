using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public interface ITheme
    {
        string Name { get; }
        bool Hidden { get; }

        Color CV_ACCOUNT_BUBBLE { get; }
        Color CV_ACCOUNT_BUBBLE_FONT { get; }
        Color CV_EXTRA_HEADER_ELLIPSE { get; }
        Color CV_SETTINGS_TEXT { get; }
        Color CV_TEXT_BOX_BACKGROUND { get; }
        Color CV_ABSENCES_ABSENT { get; }
        Color CV_ABSENCES_PRESENT { get; }
        Color CV_ABSENCES_PARTIALLY_ABSENT { get; }
        Color CV_ABSENCES_EARlY_EXIT { get; }
        Color CV_ABSENCES_LATE { get; }

        Color CV_GRADES_FILTER { get; }
        Color CV_GRADE_NOTE { get; }

        Color CV_GRADE_INSUFFICIENT { get; }
        Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; }
        Color CV_GRADE_SUFFICIENT { get; }
        Color CV_GRADE_INSUFFICIENT_BG { get; }
        Color CV_GRADE_SLIGHTLY_INSUFFICIENT_BG { get; }
        Color CV_GRADE_SUFFICIENT_BG { get; }

        Color CV_MAIN_MENU_ICON_SELECTED { get; }
        Color CV_MAIN_MENU_ICON_UNSELECTED { get; }

        Color CV_URI { get; }
        Color CV_HEADER { get; }
        Color CV_GENERIC_HEADER_FONT { get; }
        Color CV_CALENDAR { get; }
        Color CV_BUTTON { get; }
        Color CV_GENERIC_RED { get; }
        Color CV_GENERIC_GRAY { get; }
        Color CV_GENERIC_BACKGROUND { get; }
        Color CV_GENERIC_OPAQUE_BACKGROUND { get; }
        Color CV_GENERIC_GRAY_FONT { get; }
        Color CV_PERCENTAGE_BACKGROUND { get; }

        Color CV_HOME_CURRENT_DAY { get; }
        Color CV_SPINNER { get; }
        Color CV_GENERIC_TEXT_SELECTION { get; }

        Color CV_HR { get; }
        Color CV_SETTINGS_SECTION_HEADER { get; }

        Color CV_CHECK_BOX_ELLIPSE_SELECTED { get; }
        Color CV_CHECK_BOX_ELLIPSE_UNSELECTED { get; }
        Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED { get; }
        Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED { get; }

        Color CV_MULTI_MENU_FONT_SELECTED { get; }
        Color CV_MULTI_MENU_FONT_UNSELECTED { get; }
        Color CV_MULTI_MENU_FONT_SLIDER { get; }

        Color CV_BACK_ICON { get; }

        Color CV_DAY_TEXT_UNSELECTED { get; }
        Color CV_DAY_TEXT_SELECTED { get; }
        Color CV_DAY_BG_UNSELECTED { get; }
        Color CV_DAY_BG_SELECTED { get; }

        Color CV_DAY_HOME_CONTAINER { get; }

        Color CV_GENERIC_FONT { get; }
        Color CV_EXTRA_INTERACT_ICONS { get; }
        Color CV_GRADE_FONT { get; }
        Color CV_GRADE_GRV2 { get; }
        Color CV_ABSENCES_FONT { get; }
        Color CV_ABSENCES_CALENDAR_HAS_EVENT_FONT { get; }
        Color CV_ABSENCES_CALENDAR_NO_EVENT_FONT { get; }

        Color CV_DIDATICS_ICONS { get; }
        Color CV_DIDATICS_FOLDER { get; }
        Color CV_DIDATICS_TEACHERS { get; }
    }
}
