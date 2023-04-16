using ClasseVivaWPF.HomeControls.RegistrySection;
using System.Windows.Media;
using System.Xml.Linq;

namespace ClasseVivaWPF.Utils.Themes
{
    public class WhiteTheme : BaseTheme
    {
        public override Color CV_GRADE_NOTE { get; } = Color.FromArgb(0xFF, 0x5D, 0x97, 0xB1);
        public override Color CV_GRADE_INSUFFICIENT { get; } = Color.FromArgb(0xFF, 0xD0, 0x5A, 0x50);
        public override Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; } = Color.FromArgb(0xFF, 0xEB, 0x98, 0x60);
        public override Color CV_GRADE_SUFFICIENT { get; } = Color.FromArgb(0xFF, 0x83, 0xB5, 0x88);
        public override Color CV_GRADE_INSUFFICIENT_BG { get; } = Color.FromArgb(0xAF, 0xD0, 0x5A, 0x50);
        public override Color CV_GRADE_SLIGHTLY_INSUFFICIENT_BG { get; } = Color.FromArgb(0xAF, 0xEB, 0x98, 0x60);
        public override Color CV_GRADE_SUFFICIENT_BG { get; } = Color.FromArgb(0xAF, 0x83, 0xB5, 0x88);
        public override Color CV_MAIN_MENU_ICON_SELECTED { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_MAIN_MENU_ICON_UNSELECTED { get; } = Color.FromArgb(0xFF, 0x80, 0x80, 0x80);
        public override Color CV_GENERIC_RED { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_TEXT_BOX_BACKGROUND { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_CALENDAR { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_BUTTON { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_HEADER { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_GENERIC_GRAY { get; } = Color.FromArgb(0xFF, 0x80, 0x80, 0x80);
        public override Color CV_GENERIC_BACKGROUND { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_GENERIC_OPAQUE_BACKGROUND { get; } = Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0);
        public override Color CV_URI { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_HOME_CURRENT_DAY { get; } = Color.FromArgb(0xFF, 0x6A, 0x96, 0xAE);
        public override Color CV_GENERIC_GRAY_FONT { get; } = Color.FromArgb(0xFF, 0x6E, 0x6E, 0x6E);
        public override Color CV_SPINNER { get; } = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
        public override Color CV_GENERIC_TEXT_SELECTION { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_SELECTED { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_CHECK_BOX_ELLIPSE_BACKGROUND_UNSELECTED { get; } = Color.FromArgb(0xFF, 0x7C, 0x7C, 0x7C);
        public override Color CV_CHECK_BOX_ELLIPSE_SELECTED { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_CHECK_BOX_ELLIPSE_UNSELECTED { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_PERCENTAGE_BACKGROUND { get; } = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);
        public override Color CV_ABSENCES_ABSENT { get; } = Color.FromArgb(0xFF, 0xD0, 0x5A, 0x50);
        public override Color CV_ABSENCES_PARTIALLY_ABSENT { get; } = Color.FromArgb(0xFF, 0x00, 0x80, 0xB8);
        public override Color CV_ABSENCES_EARlY_EXIT { get; } = Color.FromArgb(0xFF, 0xDB, 0xB7, 0x3B);
        public override Color CV_ABSENCES_LATE { get; } = Color.FromArgb(0xFF, 0xEB, 0x98, 0x50);
        public override Color CV_GRADES_FILTER { get; } = Color.FromArgb(0xFF, 0XF0, 0XF0, 0XF0);
        public override Color CV_MULTI_MENU_FONT_SELECTED { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_MULTI_MENU_FONT_UNSELECTED { get; } = Color.FromArgb(0xAF, 0xFF, 0xFF, 0xFF);
        public override Color CV_MULTI_MENU_FONT_SLIDER { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_BACK_ICON { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_SETTINGS_SECTION_HEADER { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_HR { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_DAY_TEXT_UNSELECTED { get; } = Color.FromArgb(0xFF, 0x6E, 0x6E, 0x6E);
        public override Color CV_DAY_TEXT_SELECTED { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_DAY_BG_UNSELECTED { get; } = Color.FromArgb(0x00, 0x00, 0x00, 0x00);
        public override Color CV_DAY_BG_SELECTED { get; } = Color.FromArgb(0xFF, 0x6A, 0x96, 0xAE);
        public override Color CV_GENERIC_FONT { get; } = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
        public override Color CV_GENERIC_HEADER_FONT { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_DAY_HOME_CONTAINER { get; } = Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0);
        public override Color CV_SETTINGS_TEXT { get; } = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
        public override Color CV_EXTRA_HEADER_ELLIPSE { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_EXTRA_INTERACT_ICONS { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_GRADE_FONT { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_GRADE_GRV2 { get; } = Color.FromArgb(0xFF, 0xC3, 0x7F, 0X45);
        public override Color CV_ABSENCES_FONT { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_ACCOUNT_BUBBLE_FONT { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_ACCOUNT_BUBBLE { get; } = Color.FromArgb(0xFF, 0x16, 0xA0, 0x85);

        public WhiteTheme() : base()
        {
        }
    }
}
