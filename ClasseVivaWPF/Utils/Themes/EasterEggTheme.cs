using System;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils.Themes
{
    public class EasterEggTheme : BaseTheme // For debug lol
    {
        private static Random random = new Random();

        public override Color CV_GRADE_NOTE { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_INSUFFICIENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_SLIGHTLY_INSUFFICIENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GRADE_SUFFICIENT { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_RED { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_GRAY { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_BACKGROUND { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_GENERIC_OPAQUE_BACKGROUND { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        public override Color CV_URI_COLOR { get; } = Color.FromArgb(0xFF, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
    }
}
