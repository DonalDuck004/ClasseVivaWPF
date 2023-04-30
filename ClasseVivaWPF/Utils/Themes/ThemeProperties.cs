using ClasseVivaWPF.Utils.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ClasseVivaWPF.Utils.Themes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ThemeProperties : FrameworkElement
    {
        private static Dictionary<DependencyProperty, object>? FrozenStatus = null;
        
        public static readonly ThemeProperties INSTANCE;

        public static readonly DependencyProperty EditingProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_ABSENT_PATH)]
        public static readonly DependencyProperty CVAbsencesAbsentProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_TEXT_BOX_BACKGROUND_PATH)]
        public static readonly DependencyProperty CVTextBoxBackgroundProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_PARTIALLY_ABSENT_PATH)]
        public static readonly DependencyProperty CVAbsencesPartiallyAbsentProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_EARLY_EXIT_PATH)]
        public static readonly DependencyProperty CVAbsencesEarlyExitProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_LATE_PATH)]
        public static readonly DependencyProperty CVAbsencesLateProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADES_FILTER_PATH)]
        public static readonly DependencyProperty CVGradesFilterProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_NOTE_PATH)]
        public static readonly DependencyProperty CVGradeNoteProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_INSUFFICIENT_PATH)]
        public static readonly DependencyProperty CVGradeInsufficientProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_HEADER_FONT_PATH)]
        public static readonly DependencyProperty CVGenericHeaderFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_DAY_HOME_CONTAINER_PATH)]
        public static readonly DependencyProperty CVDayHomeContainerProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH)]
        public static readonly DependencyProperty CVGradeSlightlyInsufficientProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_SUFFICIENT_PATH)]
        public static readonly DependencyProperty CVGradeSufficientProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_MAIN_MENU_ICON_SELECTED_PATH)]
        public static readonly DependencyProperty CVMainMenuIconSelectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_MAIN_MENU_ICON_UNSELECTED_PATH)]
        public static readonly DependencyProperty CVMainMenuIconUnselectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_URI_PATH)]
        public static readonly DependencyProperty CVUriProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_HEADER_PATH)]
        public static readonly DependencyProperty CVHeaderProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_CALENDAR_PATH)]
        public static readonly DependencyProperty CVCalendarProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_BUTTON_PATH)]
        public static readonly DependencyProperty CVButtonProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_RED_PATH)]
        public static readonly DependencyProperty CVGenericRedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_GRAY_PATH)]
        public static readonly DependencyProperty CVGenericGrayProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_BACKGROUND_PATH)]
        public static readonly DependencyProperty CVGenericBackgroundProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_OPAQUE_BACKGROUND_PATH)]
        public static readonly DependencyProperty CVGenericOpaqueBackgroundProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_GRAY_FONT_PATH)]
        public static readonly DependencyProperty CVGenericGrayFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_PERCENTAGE_BACKGROUND_PATH)]
        public static readonly DependencyProperty CVPercentageBackgroundProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_HOME_CURRENT_DAY_PATH)]
        public static readonly DependencyProperty CVHomeCurrentDayProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_SPINNER_PATH)]
        public static readonly DependencyProperty CVSpinnerProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_TEXT_SELECTION_PATH)]
        public static readonly DependencyProperty CVGenericTextSelectionProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_HR_PATH)]
        public static readonly DependencyProperty CVHrProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_SETTINGS_SECTION_HEADER_PATH)]
        public static readonly DependencyProperty CVSettingsSectionHeaderProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_CHECK_BOX_ELLIPSE_SELECTED_PATH)]
        public static readonly DependencyProperty CVCheckBoxEllipseSelectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_CHECK_BOX_ELLIPSE_UNSELECTED_PATH)]
        public static readonly DependencyProperty CVCheckBoxEllipseUnselectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED_PATH)]
        public static readonly DependencyProperty CVCheckBoxEllipseBackgroundSelectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED_PATH)]
        public static readonly DependencyProperty CVCheckBoxEllipseBackgroundUnselectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_MULTI_MENU_FONT_SELECTED_PATH)]
        public static readonly DependencyProperty CVMultiMenuFontSelectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_MULTI_MENU_FONT_UNSELECTED_PATH)]
        public static readonly DependencyProperty CVMultiMenuFontUnselectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_MULTI_MENU_FONT_SLIDER_PATH)]
        public static readonly DependencyProperty CVMultiMenuFontSliderProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_BACK_ICON_PATH)]
        public static readonly DependencyProperty CVBackIconProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_DAY_BG_UNSELECTED_PATH)]
        public static readonly DependencyProperty CVDayBgUnselectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_DAY_BG_SELECTED_PATH)]
        public static readonly DependencyProperty CVDayBgSelectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_DAY_TEXT_UNSELECTED_PATH)]
        public static readonly DependencyProperty CVDayTextUnselectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_DAY_TEXT_SELECTED_PATH)]
        public static readonly DependencyProperty CVDayTextSelectedProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GENERIC_FONT_PATH)]
        public static readonly DependencyProperty CVGenericFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_SETTINGS_TEXT_PATH)]
        public static readonly DependencyProperty CVSettingsTextProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_EXTRA_HEADER_ELLIPSE_PATH)]
        public static readonly DependencyProperty CVExtraHeaderEllipseProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_FONT_PATH)]
        public static readonly DependencyProperty CVGradeFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_GRV2_PATH)]
        public static readonly DependencyProperty CVGradeGRV2Property;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_FONT_PATH)]
        public static readonly DependencyProperty CVAbsencesFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ACCOUNT_BUBBLE_FONT_PATH)]
        public static readonly DependencyProperty CVAccountBubbleFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ACCOUNT_BUBBLE_PATH)]
        public static readonly DependencyProperty CVAccountBubbleProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_PRESENT_PATH)]
        public static readonly DependencyProperty CVAbsencesPresentProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_CALENDAR_NO_EVENT_FONT_PATH)]
        public static readonly DependencyProperty CVAbsencesCalendarNoEventFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_ABSENCES_CALENDAR_HAS_EVENT_FONT_PATH)]
        public static readonly DependencyProperty CVAbsencesCalendarHasEventFontProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_INSUFFICIENT_BG_PATH)]
        public static readonly DependencyProperty CVGradeInsufficientBgProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_SLIGHTLY_INSUFFICIENT_BG_PATH)]
        public static readonly DependencyProperty CVGradeSlightlyInsufficientBgProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_GRADE_SUFFICIENT_BG_PATH)]
        public static readonly DependencyProperty CVGradeSufficientBgProperty;

        [ThemePropertyMeta(BindsTo = ThemeOperations.CV_EXTRA_INTERACT_ICONS_PATH)]
        public static readonly DependencyProperty CVExtraInteractIconsProperty;        
        public SolidColorBrush CVAbsencesCalendarNoEventFont
        {
            get => (SolidColorBrush)GetValue(CVAbsencesCalendarNoEventFontProperty);
            set => SetValue(CVAbsencesCalendarNoEventFontProperty, value);
        }

        public bool Editing
        {
            get => (bool)GetValue(EditingProperty);
            set => SetValue(EditingProperty, value);
        }

        public SolidColorBrush CVAbsencesCalendarHasEventFont
        {
            get => (SolidColorBrush)GetValue(CVAbsencesCalendarHasEventFontProperty);
            set => SetValue(CVAbsencesCalendarHasEventFontProperty, value);
        }
        public SolidColorBrush CVAbsencesPresent
        {
            get => (SolidColorBrush)GetValue(CVAbsencesPresentProperty);
            set => SetValue(CVAbsencesPresentProperty, value);
        }
        public SolidColorBrush CVAccountBubbleFont
        {
            get => (SolidColorBrush)GetValue(CVAccountBubbleFontProperty);
            set => SetValue(CVAccountBubbleFontProperty, value);
        }
        public SolidColorBrush CVAccountBubble
        {
            get => (SolidColorBrush)GetValue(CVAccountBubbleProperty);
            set => SetValue(CVAccountBubbleProperty, value);
        }
        public SolidColorBrush CVAbsencesFont
        {
            get => (SolidColorBrush)GetValue(CVAbsencesFontProperty);
            set => SetValue(CVAbsencesFontProperty, value);
        }
        public SolidColorBrush CVGradeFont
        {
            get => (SolidColorBrush)GetValue(CVGradeFontProperty);
            set => SetValue(CVGradeFontProperty, value);
        }
        public SolidColorBrush CVGradeGRV2
        {
            get => (SolidColorBrush)GetValue(CVGradeGRV2Property);
            set => SetValue(CVGradeGRV2Property, value);
        }
        public SolidColorBrush CVExtraInteractIcons
        {
            get => (SolidColorBrush)GetValue(CVExtraInteractIconsProperty);
            set => SetValue(CVExtraInteractIconsProperty, value);
        }
        public SolidColorBrush CVExtraHeaderEllipse
        {
            get => (SolidColorBrush)GetValue(CVExtraHeaderEllipseProperty);
            set => SetValue(CVExtraHeaderEllipseProperty, value);
        }
        public SolidColorBrush CVSettingsText
        {
            get => (SolidColorBrush)GetValue(CVSettingsTextProperty);
            set => SetValue(CVSettingsTextProperty, value);
        }
        public SolidColorBrush CVAbsencesAbsent
        {
            get => (SolidColorBrush)GetValue(CVAbsencesAbsentProperty);
            set => SetValue(CVAbsencesAbsentProperty, value);
        }
        public SolidColorBrush CVTextBoxBackground
        {
            get => (SolidColorBrush)GetValue(CVTextBoxBackgroundProperty);
            set => SetValue(CVTextBoxBackgroundProperty, value);
        }
        public SolidColorBrush CVAbsencesPartiallyAbsent
        {
            get => (SolidColorBrush)GetValue(CVAbsencesPartiallyAbsentProperty);
            set => SetValue(CVAbsencesPartiallyAbsentProperty, value);
        }
        public SolidColorBrush CVAbsencesEarlyExit
        {
            get => (SolidColorBrush)GetValue(CVAbsencesEarlyExitProperty);
            set => SetValue(CVAbsencesEarlyExitProperty, value);
        }
        public SolidColorBrush CVDayHomeContainer
        {
            get => (SolidColorBrush)GetValue(CVDayHomeContainerProperty);
            set => SetValue(CVDayHomeContainerProperty, value);
        }
        public SolidColorBrush CVGenericHeaderFont
        {
            get => (SolidColorBrush)GetValue(CVGenericHeaderFontProperty);
            set => SetValue(CVGenericHeaderFontProperty, value);
        }
        public SolidColorBrush CVAbsencesLate
        {
            get => (SolidColorBrush)GetValue(CVAbsencesLateProperty);
            set => SetValue(CVAbsencesLateProperty, value);
        }
        public SolidColorBrush CVGradesFilter
        {
            get => (SolidColorBrush)GetValue(CVGradesFilterProperty);
            set => SetValue(CVGradesFilterProperty, value);
        }
        public SolidColorBrush CVGradeNote
        {
            get => (SolidColorBrush)GetValue(CVGradeNoteProperty);
            set => SetValue(CVGradeNoteProperty, value);
        }
        public SolidColorBrush CVGradeInsufficient
        {
            get => (SolidColorBrush)GetValue(CVGradeInsufficientProperty);
            set => SetValue(CVGradeInsufficientProperty, value);
        }
        public SolidColorBrush CVGradeSlightlyInsufficient
        {
            get => (SolidColorBrush)GetValue(CVGradeSlightlyInsufficientProperty);
            set => SetValue(CVGradeSlightlyInsufficientProperty, value);
        }
        public SolidColorBrush CVGradeSufficient
        {
            get => (SolidColorBrush)GetValue(CVGradeSufficientProperty);
            set => SetValue(CVGradeSufficientProperty, value);
        }
        public SolidColorBrush CVMainMenuIconSelected
        {
            get => (SolidColorBrush)GetValue(CVMainMenuIconSelectedProperty);
            set => SetValue(CVMainMenuIconSelectedProperty, value);
        }
        public SolidColorBrush CVMainMenuIconUnselected
        {
            get => (SolidColorBrush)GetValue(CVMainMenuIconUnselectedProperty);
            set => SetValue(CVMainMenuIconUnselectedProperty, value);
        }
        public SolidColorBrush CVUri
        {
            get => (SolidColorBrush)GetValue(CVUriProperty);
            set => SetValue(CVUriProperty, value);
        }
        public SolidColorBrush CVHeader
        {
            get => (SolidColorBrush)GetValue(CVHeaderProperty);
            set => SetValue(CVHeaderProperty, value);
        }
        public SolidColorBrush CVCalendar
        {
            get => (SolidColorBrush)GetValue(CVCalendarProperty);
            set => SetValue(CVCalendarProperty, value);
        }
        public SolidColorBrush CVButton
        {
            get => (SolidColorBrush)GetValue(CVButtonProperty);
            set => SetValue(CVButtonProperty, value);
        }
        public SolidColorBrush CVGenericRed
        {
            get => (SolidColorBrush)GetValue(CVGenericRedProperty);
            set => SetValue(CVGenericRedProperty, value);
        }
        public SolidColorBrush CVGenericGray
        {
            get => (SolidColorBrush)GetValue(CVGenericGrayProperty);
            set => SetValue(CVGenericGrayProperty, value);
        }
        public SolidColorBrush CVGenericBackground
        {
            get => (SolidColorBrush)GetValue(CVGenericBackgroundProperty);
            set => SetValue(CVGenericBackgroundProperty, value);
        }
        public SolidColorBrush CVGenericOpaqueBackground
        {
            get => (SolidColorBrush)GetValue(CVGenericOpaqueBackgroundProperty);
            set => SetValue(CVGenericOpaqueBackgroundProperty, value);
        }
        public SolidColorBrush CVGenericGrayFont
        {
            get => (SolidColorBrush)GetValue(CVGenericGrayFontProperty);
            set => SetValue(CVGenericGrayFontProperty, value);
        }
        public SolidColorBrush CVPercentageBackground
        {
            get => (SolidColorBrush)GetValue(CVPercentageBackgroundProperty);
            set => SetValue(CVPercentageBackgroundProperty, value);
        }
        public SolidColorBrush CVHomeCurrentDay
        {
            get => (SolidColorBrush)GetValue(CVHomeCurrentDayProperty);
            set => SetValue(CVHomeCurrentDayProperty, value);
        }
        public SolidColorBrush CVSpinner
        {
            get => (SolidColorBrush)GetValue(CVSpinnerProperty);
            set => SetValue(CVSpinnerProperty, value);
        }
        public SolidColorBrush CVGenericTextSelection
        {
            get => (SolidColorBrush)GetValue(CVGenericTextSelectionProperty);
            set => SetValue(CVGenericTextSelectionProperty, value);
        }
        public SolidColorBrush CVHr
        {
            get => (SolidColorBrush)GetValue(CVHrProperty);
            set => SetValue(CVHrProperty, value);
        }
        public SolidColorBrush CVSettingsSectionHeader
        {
            get => (SolidColorBrush)GetValue(CVSettingsSectionHeaderProperty);
            set => SetValue(CVSettingsSectionHeaderProperty, value);
        }
        public SolidColorBrush CVCheckBoxEllipseSelected
        {
            get => (SolidColorBrush)GetValue(CVCheckBoxEllipseSelectedProperty);
            set => SetValue(CVCheckBoxEllipseSelectedProperty, value);
        }
        public SolidColorBrush CVCheckBoxEllipseUnselected
        {
            get => (SolidColorBrush)GetValue(CVCheckBoxEllipseUnselectedProperty);
            set => SetValue(CVCheckBoxEllipseUnselectedProperty, value);
        }
        public SolidColorBrush CVCheckBoxEllipseBackgroundSelected
        {
            get => (SolidColorBrush)GetValue(CVCheckBoxEllipseBackgroundSelectedProperty);
            set => SetValue(CVCheckBoxEllipseBackgroundSelectedProperty, value);
        }
        public SolidColorBrush CVCheckBoxEllipseBackgroundUnselected
        {
            get => (SolidColorBrush)GetValue(CVCheckBoxEllipseBackgroundUnselectedProperty);
            set => SetValue(CVCheckBoxEllipseBackgroundUnselectedProperty, value);
        }
        public SolidColorBrush CVMultiMenuFontSelected
        {
            get => (SolidColorBrush)GetValue(CVMultiMenuFontSelectedProperty);
            set => SetValue(CVMultiMenuFontSelectedProperty, value);
        }
        public SolidColorBrush CVMultiMenuFontUnselected
        {
            get => (SolidColorBrush)GetValue(CVMultiMenuFontUnselectedProperty);
            set => SetValue(CVMultiMenuFontUnselectedProperty, value);
        }
        public SolidColorBrush CVMultiMenuFontSlider
        {
            get => (SolidColorBrush)GetValue(CVMultiMenuFontSliderProperty);
            set => SetValue(CVMultiMenuFontSliderProperty, value);
        }
        public SolidColorBrush CVBackIcon
        {
            get => (SolidColorBrush)GetValue(CVBackIconProperty);
            set => SetValue(CVBackIconProperty, value);
        }
        public SolidColorBrush CVDayBgUnselected
        {
            get => (SolidColorBrush)GetValue(CVDayBgUnselectedProperty);
            set => SetValue(CVDayBgUnselectedProperty, value);
        }
        public SolidColorBrush CVDayBgSelected
        {
            get => (SolidColorBrush)GetValue(CVDayBgSelectedProperty);
            set => SetValue(CVDayBgSelectedProperty, value);
        }
        public SolidColorBrush CVDayTextUnselected
        {
            get => (SolidColorBrush)GetValue(CVDayTextUnselectedProperty);
            set => SetValue(CVDayTextUnselectedProperty, value);
        }
        public SolidColorBrush CVDayTextSelected
        {
            get => (SolidColorBrush)GetValue(CVDayTextUnselectedProperty);
            set => SetValue(CVDayTextUnselectedProperty, value);
        }
        public SolidColorBrush CVGenericFont
        {
            get => (SolidColorBrush)GetValue(CVGenericFontProperty);
            set => SetValue(CVGenericFontProperty, value);
        }
        public SolidColorBrush CVGradeInsufficientBg
        {
            get => (SolidColorBrush)GetValue(CVGradeInsufficientBgProperty);
            set => SetValue(CVGradeInsufficientBgProperty, value);
        }
        public SolidColorBrush CVGradeSlightlyInsufficientBg
        {
            get => (SolidColorBrush)GetValue(CVGradeSlightlyInsufficientBgProperty);
            set => SetValue(CVGradeSlightlyInsufficientBgProperty, value);
        }
        public SolidColorBrush CVGradeSufficientBg
        {
            get => (SolidColorBrush)GetValue(CVGradeSufficientBgProperty);
            set => SetValue(CVGradeSufficientBgProperty, value);
        }


        static ThemeProperties()
        {
            ThemeProperties.EditingProperty = DependencyProperty.Register("Editing", typeof(bool), typeof(ThemeProperties), new PropertyMetadata(false));

            ThemeProperties.CVGradeInsufficientBgProperty = DependencyProperty.Register("CVGradeInsufficientBg", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeSlightlyInsufficientBgProperty = DependencyProperty.Register("CVGradeSlightlyInsufficientBg", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeSufficientBgProperty = DependencyProperty.Register("CVGradeSufficientBg", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesAbsentProperty = DependencyProperty.Register("CVAbsencesAbsent", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVTextBoxBackgroundProperty = DependencyProperty.Register("CVTextBoxBackground", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesPartiallyAbsentProperty = DependencyProperty.Register("CVAbsencesPartiallyAbsent", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesEarlyExitProperty = DependencyProperty.Register("CVAbsencesEarlyExit", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesLateProperty = DependencyProperty.Register("CVAbsencesLate", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradesFilterProperty = DependencyProperty.Register("CVGradesFilter", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeNoteProperty = DependencyProperty.Register("CVGradeNote", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeInsufficientProperty = DependencyProperty.Register("CVGradeInsufficient", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeSlightlyInsufficientProperty = DependencyProperty.Register("CVGradeSlightlyInsufficient", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeSufficientProperty = DependencyProperty.Register("CVGradeSufficient", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVMainMenuIconSelectedProperty = DependencyProperty.Register("CVMainMenuIconSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVMainMenuIconUnselectedProperty = DependencyProperty.Register("CVMainMenuIconUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVUriProperty = DependencyProperty.Register("CVUri", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVHeaderProperty = DependencyProperty.Register("CVHeader", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVCalendarProperty = DependencyProperty.Register("CVCalendar", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVButtonProperty = DependencyProperty.Register("CVButton", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericRedProperty = DependencyProperty.Register("CVGenericRed", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericGrayProperty = DependencyProperty.Register("CVGenericGray", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericBackgroundProperty = DependencyProperty.Register("CVGenericBackground", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericOpaqueBackgroundProperty = DependencyProperty.Register("CVGenericOpaqueBackground", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericGrayFontProperty = DependencyProperty.Register("CVGenericGrayFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVPercentageBackgroundProperty = DependencyProperty.Register("CVPercentageBackground", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVHomeCurrentDayProperty = DependencyProperty.Register("CVHomeCurrentDay", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVSpinnerProperty = DependencyProperty.Register("CVSpinner", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericTextSelectionProperty = DependencyProperty.Register("CVGenericTextSelection", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVHrProperty = DependencyProperty.Register("CVHr", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVSettingsSectionHeaderProperty = DependencyProperty.Register("CVSettingsSectionHeader", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVCheckBoxEllipseSelectedProperty = DependencyProperty.Register("CVCheckBoxEllipseSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVCheckBoxEllipseUnselectedProperty = DependencyProperty.Register("CVCheckBoxEllipseUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVCheckBoxEllipseBackgroundSelectedProperty = DependencyProperty.Register("CVCheckBoxEllipseBackgroundSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVCheckBoxEllipseBackgroundUnselectedProperty = DependencyProperty.Register("CVCheckBoxEllipseBackgroundUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVMultiMenuFontSelectedProperty = DependencyProperty.Register("CVMultiMenuFontSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVMultiMenuFontUnselectedProperty = DependencyProperty.Register("CVMultiMenuFontUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVMultiMenuFontSliderProperty = DependencyProperty.Register("CVMultiMenuFontSlider", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVBackIconProperty = DependencyProperty.Register("CVBackIcon", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayBgUnselectedProperty = DependencyProperty.Register("CVDayBgUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayBgSelectedProperty = DependencyProperty.Register("CVDayBgSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayTextUnselectedProperty = DependencyProperty.Register("CVDayTextUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayTextSelectedProperty = DependencyProperty.Register("CVDayTextSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericFontProperty = DependencyProperty.Register("CVGenericFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericHeaderFontProperty = DependencyProperty.Register("CVGenericHeaderFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayHomeContainerProperty = DependencyProperty.Register("CVDayHomeContainer", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVSettingsTextProperty = DependencyProperty.Register("CVSettingsText", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVExtraHeaderEllipseProperty = DependencyProperty.Register("CVExtraHeaderEllipse", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVExtraInteractIconsProperty = DependencyProperty.Register("CVExtraInteractIcons", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeFontProperty = DependencyProperty.Register("CVGradeFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGradeGRV2Property = DependencyProperty.Register("CVGradeGRV2", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesFontProperty = DependencyProperty.Register("CVAbsencesFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAccountBubbleProperty = DependencyProperty.Register("CVAccountBubble", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAccountBubbleFontProperty = DependencyProperty.Register("CVAccountBubbleFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesPresentProperty = DependencyProperty.Register("CVAbsencesPresent", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesCalendarNoEventFontProperty = DependencyProperty.Register("CVAbsencesCalendarNoEventFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVAbsencesCalendarHasEventFontProperty = DependencyProperty.Register("CVAbsencesCalendarHasEventFont", typeof(SolidColorBrush), typeof(ThemeProperties));

            ThemeProperties.INSTANCE = new();
        }

        private ThemeProperties()
        {
            this.SetThemeBinding(ThemeProperties.CVGradeInsufficientBgProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeSlightlyInsufficientBgProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeSufficientBgProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesCalendarNoEventFontProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesCalendarHasEventFontProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesPresentProperty);
            this.SetThemeBinding(ThemeProperties.CVAccountBubbleProperty);
            this.SetThemeBinding(ThemeProperties.CVAccountBubbleFontProperty);
            this.SetThemeBinding(ThemeProperties.CVExtraInteractIconsProperty);
            this.SetThemeBinding(ThemeProperties.CVExtraHeaderEllipseProperty);
            this.SetThemeBinding(ThemeProperties.CVTextBoxBackgroundProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesAbsentProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesPartiallyAbsentProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesEarlyExitProperty);
            this.SetThemeBinding(ThemeProperties.CVAbsencesLateProperty);
            this.SetThemeBinding(ThemeProperties.CVGradesFilterProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeNoteProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeInsufficientProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeSlightlyInsufficientProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeSufficientProperty);
            this.SetThemeBinding(ThemeProperties.CVMainMenuIconSelectedProperty);
            this.SetThemeBinding(ThemeProperties.CVMainMenuIconUnselectedProperty);
            this.SetThemeBinding(ThemeProperties.CVUriProperty);
            this.SetThemeBinding(ThemeProperties.CVHeaderProperty);
            this.SetThemeBinding(ThemeProperties.CVCalendarProperty);
            this.SetThemeBinding(ThemeProperties.CVButtonProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericRedProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericGrayProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericBackgroundProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericOpaqueBackgroundProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericGrayFontProperty);
            this.SetThemeBinding(ThemeProperties.CVPercentageBackgroundProperty);
            this.SetThemeBinding(ThemeProperties.CVHomeCurrentDayProperty);
            this.SetThemeBinding(ThemeProperties.CVSpinnerProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericTextSelectionProperty);
            this.SetThemeBinding(ThemeProperties.CVHrProperty);
            this.SetThemeBinding(ThemeProperties.CVSettingsSectionHeaderProperty);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseSelectedProperty);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseUnselectedProperty);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseBackgroundSelectedProperty);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseBackgroundUnselectedProperty);
            this.SetThemeBinding(ThemeProperties.CVMultiMenuFontSelectedProperty);
            this.SetThemeBinding(ThemeProperties.CVMultiMenuFontUnselectedProperty);
            this.SetThemeBinding(ThemeProperties.CVMultiMenuFontSliderProperty);
            this.SetThemeBinding(ThemeProperties.CVBackIconProperty);
            this.SetThemeBinding(ThemeProperties.CVDayBgSelectedProperty);
            this.SetThemeBinding(ThemeProperties.CVDayBgUnselectedProperty);
            this.SetThemeBinding(ThemeProperties.CVDayTextSelectedProperty);
            this.SetThemeBinding(ThemeProperties.CVDayTextUnselectedProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericFontProperty);
            this.SetThemeBinding(ThemeProperties.CVGenericHeaderFontProperty);
            this.SetThemeBinding(ThemeProperties.CVDayHomeContainerProperty);
            this.SetThemeBinding(ThemeProperties.CVSettingsTextProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeFontProperty);
            this.SetThemeBinding(ThemeProperties.CVGradeGRV2Property);
            this.SetThemeBinding(ThemeProperties.CVAbsencesFontProperty);
        }

        public static IEnumerable<DependencyProperty> GetProperties()
        {
            return from field in typeof(ThemeProperties).GetFields(BindingFlags.Static | BindingFlags.Public)
                   where field.FieldType == typeof(DependencyProperty) && field.GetCustomAttribute<ThemePropertyMeta>() is not null 
                   select (DependencyProperty)field.GetValue(null)! into d orderby d.Name select d;
        }

        public static string GetTargetThemePath(DependencyProperty property)
        {
            return GetTargetThemePath(typeof(ThemeProperties).GetField(property.Name + "Property", BindingFlags.Static | BindingFlags.Public)!);
        }

        public static string GetTargetThemePath(FieldInfo field)
        {
            return field.GetCustomAttribute<ThemePropertyMeta>()!.BindsTo;
        }

        private void SetThemeBinding(DependencyProperty property, bool run_animation = false)
        {
            var path = GetTargetThemePath(property);
            
            bool initialized = run_animation;
            var binding = new Binding()
            {
                Path = new("CurrentTheme." + path),
                Converter = new ActionConverter(),
                ConverterParameter = (object v) =>
                {
                    var rt = property.PropertyType == typeof(Color) ? v : new SolidColorBrush((Color)v);

                    if (initialized is true)
                    {
                        var st = new Storyboard();
                        var path = property.PropertyType == typeof(Color) ?
                                        new PropertyPath(property.Name) :
                                        new PropertyPath($"({property.Name}).(SolidColorBrush.Color)");
                        var current = this.GetValue(property);

                        var animation = new ColorAnimation()
                        {
                            Duration = new(TimeSpan.FromSeconds(0.5)),
                            From = (Color)(current is Color ? current : ((SolidColorBrush)current).Color),
                            To = (Color)v,
                            FillBehavior = FillBehavior.Stop
                        };

                        Storyboard.SetTargetProperty(animation, path);
                        st.Children.Add(animation);

                        this.Dispatcher.BeginInvoke(() =>
                        {
                            st.Begin(this);
                        });

                    }
                    else
                        initialized = true;

                    return rt;
                },
                Source = MainWindow.INSTANCE
            };

            BindingOperations.SetBinding(this, property, binding);
        }



        public static void BeginThemeEditing()
        {
            if (FrozenStatus is not null)
                throw new InvalidOperationException("BeginThemeEditing called twice");

            FrozenStatus = new();
            INSTANCE.Editing = true;

            foreach (var item in ThemeProperties.GetProperties())
                FrozenStatus[item] = INSTANCE.GetValue(item);

            FrozenStatus.EnsureCapacity(FrozenStatus.Count);
        }

        public static void EndThemeEditing(bool CommitCurrent)
        {
            if (FrozenStatus is null)
                throw new InvalidOperationException("BeginThemeEditing not called");

            INSTANCE.Editing = false;

            if (!CommitCurrent)
            {
                foreach (var item in FrozenStatus)
                    INSTANCE.SetValue(item.Key, item.Value);
            }

            FrozenStatus = null;
        }

        public static string FreezeAsJson()
        {
            var sw = new StringWriter();

            var writer = new JsonTextWriter(sw);

            var obj = new JObject();
            foreach (var item in ThemeProperties.GetProperties())
                obj.Add(item.Name/*ThemeProperties.GetTargetThemePath(item)*/, ThemeProperties.INSTANCE.GetValue(item).ToString());

            obj.WriteTo(writer);
            return sw.ToString()!;
        }
    }
}
