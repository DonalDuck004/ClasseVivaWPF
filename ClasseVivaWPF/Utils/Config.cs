using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace ClasseVivaWPF.Utils
{
    public static class Config
    {
        public const double HEADER_HEIGHT = 48.0D;
        public static readonly GridLength HEADER_HEIGHT_GL = new GridLength(HEADER_HEIGHT);

        public const int NOTIFY_UPDATE_DELAY = 60000;

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

        public const string REPO_URL = "https://github.com/DonalDuck004/ClasseVivaWPF";

        public static readonly string INSTALL_PATH = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;

        public static readonly string MEDIA_DIR_PATH = Path.Join(INSTALL_PATH, "media");
        public static readonly string SESSIONS_DIR_PATH = Path.Join(INSTALL_PATH, "Sessions");

        public static readonly (int, int, string, string) VERSION = (1, 9, "development version", "dev");
        public static string VERSION_SHORTEST_STRING => $"{VERSION.Item1}.{VERSION.Item2}";
        public static string VERSION_SHORT_STRING => $"{VERSION_SHORTEST_STRING} {VERSION.Item4}";
        public static string VERSION_LONG_STRING => $"{VERSION_SHORTEST_STRING} {VERSION.Item3}";
        public static string VERSION_LONGEST_STRING => $"{VERSION_LONG_STRING} ({VERSION.Item4})";

        static Config()
        {
            if (!Directory.Exists(MEDIA_DIR_PATH))
                Directory.CreateDirectory(MEDIA_DIR_PATH);

            if (!Directory.Exists(SESSIONS_DIR_PATH))
                Directory.CreateDirectory(SESSIONS_DIR_PATH);
        }

    }
}
