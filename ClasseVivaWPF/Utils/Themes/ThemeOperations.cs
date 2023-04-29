﻿using System.Collections.Generic;
using System.Linq;

namespace ClasseVivaWPF.Utils.Themes
{
    public static class ThemeOperations
    {
        public static List<ThemeCreator> THEMES = new List<ThemeCreator>();
        
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

        public static void Register(ThemeCreator creator)
        {
            THEMES.Add(creator);
        }

        public static ITheme Get(string name) => THEMES.Where(x => x.Name == name).First().Create();
        public static ITheme Get(ThemeCreator creator) => creator.Create();
    }
}
