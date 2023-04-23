using ClasseVivaWPF.Utils.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace ClasseVivaWPF.Utils.Themes
{
    /* Plane for dynamic themes (from files)
     * Structure:
     * [{"name": ..., "theme": {}}]
     * In db:
     * Themes(ID, name, theme)
     */
    public abstract class BaseTheme
    {
        public static List<Type> StaticThemes = new List<Type>();

        public const string CV_GRADE_NOTE_PATH = "CV_GRADE_NOTE";

        public const string CV_GRADE_INSUFFICIENT_PATH = "CV_GRADE_INSUFFICIENT";
        public const string CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH = "CV_GRADE_SLIGHTLY_INSUFFICIENT";
        public const string CV_GRADE_SUFFICIENT_PATH = "CV_GRADE_SUFFICIENT";

        public const string CV_GRADE_INSUFFICIENT_BG_PATH = "CV_GRADE_INSUFFICIENT_BG";
        public const string CV_GRADE_SLIGHTLY_INSUFFICIENT_BG_PATH = "CV_GRADE_SLIGHTLY_INSUFFICIENT_BG";
        public const string CV_GRADE_SUFFICIENT_BG_PATH = "CV_GRADE_SUFFICIENT_BG";

        public const string CV_GENERIC_RED_PATH = "CV_GENERIC_RED";
        public const string CV_GENERIC_GRAY_PATH = "CV_GENERIC_GRAY";
        public const string CV_GENERIC_BACKGROUND_PATH = "CV_GENERIC_BACKGROUND";
        public const string CV_GENERIC_OPAQUE_BACKGROUND_PATH = "CV_GENERIC_OPAQUE_BACKGROUND";
        public const string CV_URI_PATH = "CV_URI";
        public const string CV_GENERIC_FONT_PATH = "CV_GENERIC_FONT";
        public const string CV_GENERIC_HEADER_FONT_PATH = "CV_GENERIC_HEADER_FONT";
        public const string CV_HOME_CURRENT_DAY_PATH = "CV_HOME_CURRENT_DAY";
        public const string CV_HEADER_PATH = "CV_HEADER";
        public const string CV_PERCENTAGE_BACKGROUND_PATH = "CV_PERCENTAGE_BACKGROUND";
        public const string CV_EXTRA_INTERACT_ICONS_PATH = "CV_EXTRA_INTERACT_ICONS";
        
        public const string CV_GRADES_FILTER_PATH = "CV_GRADES_FILTER";

        public const string CV_BUTTON_PATH = "CV_BUTTON";
        public const string CV_CALENDAR_PATH = "CV_CALENDAR";

        public const string CV_GENERIC_GRAY_FONT_PATH = "CV_GENERIC_GRAY_FONT";

        public const string CV_DAY_TEXT_UNSELECTED_PATH = "CV_DAY_TEXT_UNSELECTED";
        public const string CV_DAY_TEXT_SELECTED_PATH = "CV_DAY_TEXT_SELECTED";
        public const string CV_DAY_BG_UNSELECTED_PATH = "CV_DAY_BG_UNSELECTED";
        public const string CV_DAY_BG_SELECTED_PATH = "CV_DAY_BG_SELECTED";

        public const string CV_MAIN_MENU_ICON_SELECTED_PATH = "CV_MAIN_MENU_ICON_SELECTED";
        public const string CV_MAIN_MENU_ICON_UNSELECTED_PATH = "CV_MAIN_MENU_ICON_UNSELECTED";

        public const string CV_SPINNER_PATH = "CV_SPINNER";
        public const string CV_GENERIC_TEXT_SELECTION_PATH = "CV_GENERIC_TEXT_SELECTION";

        public const string CV_CHECK_BOX_ELLIPSE_SELECTED_PATH = "CV_CHECK_BOX_ELLIPSE_SELECTED";
        public const string CV_CHECK_BOX_ELLIPSE_UNSELECTED_PATH = "CV_CHECK_BOX_ELLIPSE_UNSELECTED";
        public const string CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED_PATH = "CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED";
        public const string CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED_PATH = "CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED";

        public const string CV_ABSENCES_ABSENT_PATH = "CV_ABSENCES_ABSENT";
        public const string CV_ABSENCES_PARTIALLY_ABSENT_PATH = "CV_ABSENCES_PARTIALLY_ABSENT";
        public const string CV_ABSENCES_EARLY_EXIT_PATH = "CV_ABSENCES_EARlY_EXIT";
        public const string CV_ABSENCES_LATE_PATH = "CV_ABSENCES_LATE";
        public const string CV_ABSENCES_PRESENT_PATH = "CV_ABSENCES_PRESENT";
        public const string CV_ABSENCES_CALENDAR_HAS_EVENT_FONT_PATH = "CV_ABSENCES_CALENDAR_HAS_EVENT_FONT";
        public const string CV_ABSENCES_CALENDAR_NO_EVENT_FONT_PATH = "CV_ABSENCES_CALENDAR_NO_EVENT_FONT";
        
        public const string CV_MULTI_MENU_FONT_SELECTED_PATH = "CV_MULTI_MENU_FONT_SELECTED";
        public const string CV_MULTI_MENU_FONT_UNSELECTED_PATH = "CV_MULTI_MENU_FONT_UNSELECTED";
        public const string CV_MULTI_MENU_FONT_SLIDER_PATH = "CV_MULTI_MENU_FONT_SLIDER";

        public const string CV_BACK_ICON_PATH = "CV_BACK_ICON";
        public const string CV_HR_PATH = "CV_HR";
        public const string CV_SETTINGS_SECTION_HEADER_PATH = "CV_SETTINGS_SECTION_HEADER";
        public const string CV_TEXT_BOX_BACKGROUND_PATH = "CV_TEXT_BOX_BACKGROUND";
        public const string CV_DAY_HOME_CONTAINER_PATH = "CV_DAY_HOME_CONTAINER";
        public const string CV_SETTINGS_TEXT_PATH = "CV_SETTINGS_TEXT";
        public const string CV_EXTRA_HEADER_ELLIPSE_PATH = "CV_EXTRA_HEADER_ELLIPSE";
        public const string CV_GRADE_FONT_PATH = "CV_GRADE_FONT";
        public const string CV_GRADE_GRV2_PATH = "CV_GRADE_GRV2";
        public const string CV_ABSENCES_FONT_PATH = "CV_ABSENCES_FONT";
        public const string CV_ACCOUNT_BUBBLE_FONT_PATH = "CV_ACCOUNT_BUBBLE_FONT";
        public const string CV_ACCOUNT_BUBBLE_PATH = "CV_ACCOUNT_BUBBLE";

        public abstract Color CV_ACCOUNT_BUBBLE { get; }
        public abstract Color CV_ACCOUNT_BUBBLE_FONT { get; }
        public abstract Color CV_EXTRA_HEADER_ELLIPSE { get; }
        public abstract Color CV_SETTINGS_TEXT { get; }
        public abstract Color CV_TEXT_BOX_BACKGROUND { get; }
        public abstract Color CV_ABSENCES_ABSENT { get; }
        public abstract Color CV_ABSENCES_PRESENT { get; }
        public abstract Color CV_ABSENCES_PARTIALLY_ABSENT { get; }
        public abstract Color CV_ABSENCES_EARlY_EXIT { get; }
        public abstract Color CV_ABSENCES_LATE { get; }
        
        public abstract Color CV_GRADES_FILTER { get; }
        public abstract Color CV_GRADE_NOTE { get; }

        public abstract Color CV_GRADE_INSUFFICIENT { get; }
        public abstract Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; }
        public abstract Color CV_GRADE_SUFFICIENT { get; }
        public abstract Color CV_GRADE_INSUFFICIENT_BG { get; }
        public abstract Color CV_GRADE_SLIGHTLY_INSUFFICIENT_BG { get; }
        public abstract Color CV_GRADE_SUFFICIENT_BG { get; }
        
        public abstract Color CV_MAIN_MENU_ICON_SELECTED { get; }
        public abstract Color CV_MAIN_MENU_ICON_UNSELECTED { get; }

        public abstract Color CV_URI { get; }
        public abstract Color CV_HEADER { get; }
        public abstract Color CV_GENERIC_HEADER_FONT { get; }
        public abstract Color CV_CALENDAR { get; }
        public abstract Color CV_BUTTON { get; }
        public abstract Color CV_GENERIC_RED { get; }
        public abstract Color CV_GENERIC_GRAY { get; }
        public abstract Color CV_GENERIC_BACKGROUND { get; }
        public abstract Color CV_GENERIC_OPAQUE_BACKGROUND { get; }
        public abstract Color CV_GENERIC_GRAY_FONT { get; }
        public abstract Color CV_PERCENTAGE_BACKGROUND { get; }
        
        public abstract Color CV_HOME_CURRENT_DAY { get; }
        public abstract Color CV_SPINNER { get; }
        public abstract Color CV_GENERIC_TEXT_SELECTION { get; }

        public abstract Color CV_HR { get; }
        public abstract Color CV_SETTINGS_SECTION_HEADER { get; }

        public abstract Color CV_CHECK_BOX_ELLIPSE_SELECTED { get; }
        public abstract Color CV_CHECK_BOX_ELLIPSE_UNSELECTED { get; }
        public abstract Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED { get; }
        public abstract Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED { get; }

        public abstract Color CV_MULTI_MENU_FONT_SELECTED { get; }
        public abstract Color CV_MULTI_MENU_FONT_UNSELECTED { get; }
        public abstract Color CV_MULTI_MENU_FONT_SLIDER { get; }

        public abstract Color CV_BACK_ICON { get; }

        public abstract Color CV_DAY_TEXT_UNSELECTED { get; }
        public abstract Color CV_DAY_TEXT_SELECTED { get; }
        public abstract Color CV_DAY_BG_UNSELECTED { get; }
        public abstract Color CV_DAY_BG_SELECTED { get; }

        public abstract Color CV_DAY_HOME_CONTAINER { get; }

        public abstract Color CV_GENERIC_FONT { get; }
        public abstract Color CV_EXTRA_INTERACT_ICONS { get; }
        public abstract Color CV_GRADE_FONT { get; }
        public abstract Color CV_GRADE_GRV2 { get; }
        public abstract Color CV_ABSENCES_FONT { get; }
        public abstract Color CV_ABSENCES_CALENDAR_HAS_EVENT_FONT { get; }
        public abstract Color CV_ABSENCES_CALENDAR_NO_EVENT_FONT { get; }

        public BaseTheme(bool reg = true)
        {
            if (reg && !StaticThemes.Contains(this.GetType()))
                StaticThemes.Add(this.GetType());
        }
    }
    
}
