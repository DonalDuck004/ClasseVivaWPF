﻿using ClasseVivaWPF.Utils.Converters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace ClasseVivaWPF.Themes.Abs
{

    public abstract class BaseTheme : ITheme
    {
        public virtual string Name { get => this.GetType().Name; }
        public virtual bool Hidden { get; } = false;

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
        public abstract Color CV_DIDATICS_ICONS { get; }
        public abstract Color CV_DIDATICS_FOLDER { get; }
        public abstract Color CV_DIDATICS_TEACHERS { get; }

        public abstract Color CV_HOMEWORK_DONE { get; }

        public abstract Color CV_CARET_BRUSH { get; }
        public abstract Color CV_SELECTION_BRUSH { get; }
        public abstract Color CV_REGISTRY_OPTION_BACKGROUND { get; }
        public abstract Color CV_RELOAD_BACKGROUND { get; }
        public abstract Color CV_RELOAD_BORDER { get; }
        public abstract Color CV_RELOAD { get; }
        public abstract Color CV_SPINNER_BACKGROUND { get; }

        protected BaseTheme()
        {

        }
    }
}
