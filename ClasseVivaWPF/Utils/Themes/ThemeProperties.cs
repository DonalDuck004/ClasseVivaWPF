using System.Windows;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public class ThemeProperties : FrameworkElement
    {
        public static readonly ThemeProperties INSTANCE;
        
        public static readonly DependencyProperty CVExtraInteractIconsProperty;
        public static readonly DependencyProperty CVAbsencesAbsentProperty;
        public static readonly DependencyProperty CVTextBoxBackgroundProperty;
        public static readonly DependencyProperty CVAbsencesPartiallyAbsentProperty;
        public static readonly DependencyProperty CVAbsencesEarlyExitProperty;
        public static readonly DependencyProperty CVAbsencesLateProperty;
        public static readonly DependencyProperty CVGradesFilterProperty;
        public static readonly DependencyProperty CVGradeNoteProperty; 
        public static readonly DependencyProperty CVGradeInsufficientProperty;
        public static readonly DependencyProperty CVGenericHeaderFontProperty;
        public static readonly DependencyProperty CVDayHomeContainerProperty;
        public static readonly DependencyProperty CVGradeSlightlyInsufficientProperty;
        public static readonly DependencyProperty CVGradeSufficientProperty;
        public static readonly DependencyProperty CVMainMenuIconSelectedProperty;
        public static readonly DependencyProperty CVMainMenuIconUnselectedProperty;
        public static readonly DependencyProperty CVUriProperty;
        public static readonly DependencyProperty CVHeaderProperty;
        public static readonly DependencyProperty CVCalendarProperty;
        public static readonly DependencyProperty CVButtonProperty;
        public static readonly DependencyProperty CVGenericRedProperty;
        public static readonly DependencyProperty CVGenericGrayProperty;
        public static readonly DependencyProperty CVGenericBackgroundProperty;
        public static readonly DependencyProperty CVGenericOpaqueBackgroundProperty;
        public static readonly DependencyProperty CVGenericGrayFontProperty;
        public static readonly DependencyProperty CVPercentageBackgroundProperty;
        public static readonly DependencyProperty CVHomeCurrentDayProperty;
        public static readonly DependencyProperty CVSpinnerProperty;
        public static readonly DependencyProperty CVGenericTextSelectionProperty;
        public static readonly DependencyProperty CVHrProperty;
        public static readonly DependencyProperty CVSettingsSectionHeaderProperty;
        public static readonly DependencyProperty CVCheckBoxEllipseSelectedProperty;
        public static readonly DependencyProperty CVCheckBoxEllipseUnselectedProperty;
        public static readonly DependencyProperty CVCheckBoxEllipseBackgroundSelectedProperty;
        public static readonly DependencyProperty CVCheckBoxEllipseBackgroundUnselectedProperty;
        public static readonly DependencyProperty CVMultiMenuFontSelectedProperty;
        public static readonly DependencyProperty CVMultiMenuFontUnselectedProperty;
        public static readonly DependencyProperty CVMultiMenuFontSliderProperty;
        public static readonly DependencyProperty CVBackIconProperty;
        public static readonly DependencyProperty CVDayBGUnselectedProperty;
        public static readonly DependencyProperty CVDayBGSelectedProperty;
        public static readonly DependencyProperty CVDayTextUnselectedProperty;
        public static readonly DependencyProperty CVDayTextSelectedProperty; 
        public static readonly DependencyProperty CVGenericFontProperty;
        public static readonly DependencyProperty CVSettingsTextProperty;
        public static readonly DependencyProperty CVExtraHeaderEllipseProperty;
        
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

        public SolidColorBrush CVDayBGUnselected
        {
            get => (SolidColorBrush)GetValue(CVDayBGUnselectedProperty);
            set => SetValue(CVDayBGUnselectedProperty, value);
        }

        public SolidColorBrush CVDayBGSelected
        {
            get => (SolidColorBrush)GetValue(CVDayBGSelectedProperty);
            set => SetValue(CVDayBGSelectedProperty, value);
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


        static ThemeProperties()
        {
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
            ThemeProperties.CVDayBGUnselectedProperty = DependencyProperty.Register("CVDayBGUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayBGSelectedProperty = DependencyProperty.Register("CVDayBGSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayTextUnselectedProperty = DependencyProperty.Register("CVDayTextUnselected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayTextSelectedProperty = DependencyProperty.Register("CVDayTextSelected", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericFontProperty = DependencyProperty.Register("CVGenericFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVGenericHeaderFontProperty = DependencyProperty.Register("CVGenericHeaderFont", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVDayHomeContainerProperty = DependencyProperty.Register("CVDayHomeContainer", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVSettingsTextProperty = DependencyProperty.Register("CVSettingsText", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVExtraHeaderEllipseProperty = DependencyProperty.Register("CVExtraHeaderEllipse", typeof(SolidColorBrush), typeof(ThemeProperties));
            ThemeProperties.CVExtraInteractIconsProperty = DependencyProperty.Register("CVExtraInteractIcons", typeof(SolidColorBrush), typeof(ThemeProperties));
            
            ThemeProperties.INSTANCE = new();
        }


        private ThemeProperties()
        {
            this.SetThemeBinding(ThemeProperties.CVExtraInteractIconsProperty, BaseTheme.CV_EXTRA_INTERACT_ICONS_PATH);
            this.SetThemeBinding(ThemeProperties.CVExtraHeaderEllipseProperty, BaseTheme.CV_EXTRA_HEADER_ELLIPSE_PATH);
            this.SetThemeBinding(ThemeProperties.CVTextBoxBackgroundProperty, BaseTheme.CV_TEXT_BOX_BACKGROUND_PATH);
            this.SetThemeBinding(ThemeProperties.CVAbsencesAbsentProperty, BaseTheme.CV_ABSENCES_ABSENT_PATH);
            this.SetThemeBinding(ThemeProperties.CVAbsencesPartiallyAbsentProperty, BaseTheme.CV_ABSENCES_PARTIALLY_ABSENT_PATH);
            this.SetThemeBinding(ThemeProperties.CVAbsencesEarlyExitProperty, BaseTheme.CV_ABSENCES_EARLY_EXIT_PATH);
            this.SetThemeBinding(ThemeProperties.CVAbsencesLateProperty, BaseTheme.CV_ABSENCES_LATE_PATH);
            this.SetThemeBinding(ThemeProperties.CVGradesFilterProperty, BaseTheme.CV_GRADES_FILTER_PATH);
            this.SetThemeBinding(ThemeProperties.CVGradeNoteProperty, BaseTheme.CV_GRADE_NOTE_PATH);
            this.SetThemeBinding(ThemeProperties.CVGradeInsufficientProperty, BaseTheme.CV_GRADE_INSUFFICIENT_PATH);
            this.SetThemeBinding(ThemeProperties.CVGradeSlightlyInsufficientProperty, BaseTheme.CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH);
            this.SetThemeBinding(ThemeProperties.CVGradeSufficientProperty, BaseTheme.CV_GRADE_SUFFICIENT_PATH);
            this.SetThemeBinding(ThemeProperties.CVMainMenuIconSelectedProperty, BaseTheme.CV_MAIN_MENU_ICON_SELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVMainMenuIconUnselectedProperty, BaseTheme.CV_MAIN_MENU_ICON_UNSELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVUriProperty, BaseTheme.CV_URI_PATH);
            this.SetThemeBinding(ThemeProperties.CVHeaderProperty, BaseTheme.CV_HEADER_PATH);
            this.SetThemeBinding(ThemeProperties.CVCalendarProperty, BaseTheme.CV_CALENDAR_PATH);
            this.SetThemeBinding(ThemeProperties.CVButtonProperty, BaseTheme.CV_BUTTON_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericRedProperty, BaseTheme.CV_GENERIC_RED_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericGrayProperty, BaseTheme.CV_GENERIC_GRAY_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericBackgroundProperty, BaseTheme.CV_GENERIC_BACKGROUND_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericOpaqueBackgroundProperty, BaseTheme.CV_GENERIC_OPAQUE_BACKGROUND_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericGrayFontProperty, BaseTheme.CV_GENERIC_GRAY_FONT_PATH);
            this.SetThemeBinding(ThemeProperties.CVPercentageBackgroundProperty, BaseTheme.CV_PERCENTAGE_BACKGROUND_PATH);
            this.SetThemeBinding(ThemeProperties.CVHomeCurrentDayProperty, BaseTheme.CV_HOME_CURRENT_DAY_PATH);
            this.SetThemeBinding(ThemeProperties.CVSpinnerProperty, BaseTheme.CV_SPINNER_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericTextSelectionProperty, BaseTheme.CV_GENERIC_TEXT_SELECTION_PATH);
            this.SetThemeBinding(ThemeProperties.CVHrProperty, BaseTheme.CV_HR_PATH);
            this.SetThemeBinding(ThemeProperties.CVSettingsSectionHeaderProperty, BaseTheme.CV_SETTINGS_SECTION_HEADER_PATH);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseSelectedProperty, BaseTheme.CV_CHECK_BOX_ELLIPSE_SELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseUnselectedProperty, BaseTheme.CV_CHECK_BOX_ELLIPSE_UNSELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseBackgroundSelectedProperty, BaseTheme.CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVCheckBoxEllipseBackgroundUnselectedProperty, BaseTheme.CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVMultiMenuFontSelectedProperty, BaseTheme.CV_MULTI_MENU_FONT_SELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVMultiMenuFontUnselectedProperty, BaseTheme.CV_MULTI_MENU_FONT_UNSELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVMultiMenuFontSliderProperty, BaseTheme.CV_MULTI_MENU_FONT_SLIDER_PATH);
            this.SetThemeBinding(ThemeProperties.CVBackIconProperty, BaseTheme.CV_BACK_ICON_PATH);
            this.SetThemeBinding(ThemeProperties.CVDayBGSelectedProperty, BaseTheme.CV_DAY_BG_SELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVDayBGUnselectedProperty, BaseTheme.CV_DAY_BG_UNSELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVDayTextSelectedProperty, BaseTheme.CV_DAY_TEXT_SELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVDayTextUnselectedProperty, BaseTheme.CV_DAY_TEXT_UNSELECTED_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericFontProperty, BaseTheme.CV_GENERIC_FONT_PATH);
            this.SetThemeBinding(ThemeProperties.CVGenericHeaderFontProperty, BaseTheme.CV_GENERIC_HEADER_FONT_PATH);
            this.SetThemeBinding(ThemeProperties.CVDayHomeContainerProperty, BaseTheme.CV_DAY_HOME_CONTAINER_PATH);
            this.SetThemeBinding(ThemeProperties.CVSettingsTextProperty, BaseTheme.CV_SETTINGS_TEXT_PATH);
        }
    }
}
