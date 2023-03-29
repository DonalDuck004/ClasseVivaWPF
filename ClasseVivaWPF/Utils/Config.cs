using System.Windows;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils
{
    public static class Config
    {
        public const double HEADER_HEIGHT = 48.0D;
        public static readonly GridLength HEADER_HEIGHT_GL = new GridLength(HEADER_HEIGHT);

        public const int NOTIFY_UPDATE_DELAY = 60000;

        public const int MAX_OVERLAPPED_WIN = 5;

#if DEBUG
        public const bool USE_PROXY = true;
#else
        public const bool USE_PROXY = false;
#endif

        public const int PROXY_PORT = 8000;
        public const string PROXY_HOST = "localhost";

        public const bool UNLOAD_TABS_ON_SWITCH = false;

        public static readonly byte CV_RED_A = 0xFF;
        public static readonly byte CV_RED_R = 0xC6;
        public static readonly byte CV_RED_G = 0x28;
        public static readonly byte CV_RED_B = 0x28;

        public static readonly Color CV_RED = Color.FromArgb(CV_RED_A, CV_RED_R, CV_RED_G, CV_RED_B);
        public static readonly SolidColorBrush CV_RED_BRUSH = new SolidColorBrush(CV_RED);

        // f0f0f0
        public static readonly byte CV_OPAQUE_WHITE_A = 0xFF;
        public static readonly byte CV_OPAQUE_WHITE_R = 0xF0;
        public static readonly byte CV_OPAQUE_WHITE_G = 0xF0;
        public static readonly byte CV_OPAQUE_WHITE_B = 0xF0;

        public static readonly Color OPAQUE_WHITE = Color.FromArgb(CV_OPAQUE_WHITE_A,
                                                                   CV_OPAQUE_WHITE_R,
                                                                   CV_OPAQUE_WHITE_G,
                                                                   CV_OPAQUE_WHITE_B);
        public static readonly SolidColorBrush CV_OPAQUE_WHITE_BRUSH = new SolidColorBrush(OPAQUE_WHITE);
    }
}
