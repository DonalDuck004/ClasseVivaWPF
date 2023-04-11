using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public class WhiteTheme : BaseTheme
    {
        public override Color CV_GRADE_NOTE { get; } = Color.FromArgb(0xFF, 0x5D, 0x97, 0xB1);
        public override Color CV_GRADE_INSUFFICIENT { get; } = Color.FromArgb(0xFF, 0xD0, 0x5A, 0x50);
        public override Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; } = Color.FromArgb(0xFF, 0xEB, 0x98, 0x60);
        public override Color CV_GRADE_SUFFICIENT { get; } = Color.FromArgb(0xFF, 0x83, 0xB5, 0x88);
        public override Color CV_GENERIC_RED { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
        public override Color CV_GENERIC_GRAY { get; } = Colors.Gray;
        public override Color CV_GENERIC_BACKGROUND { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public override Color CV_GENERIC_OPAQUE_BACKGROUND { get; } = Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0);
        public override Color CV_URI_COLOR { get; } = Color.FromArgb(0xFF, 0xC6, 0x28, 0x28);
    }
}
