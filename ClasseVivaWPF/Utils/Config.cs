using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Windows.UI.WebUI;

namespace ClasseVivaWPF.Utils
{

    public static class Config
    {
        public const double HEADER_HEIGHT = 48.0D;
        public static readonly GridLength HEADER_HEIGHT_GL = new GridLength(HEADER_HEIGHT);

        public const string DEFAULT_THEME_NAME = "WhiteTheme";

        public const int NOTIFY_UPDATE_DELAY = 60000;

#if DEBUG
        public const bool USE_PROXY = true;
#else
        public const bool USE_PROXY = false;
#endif

        public const int PROXY_PORT = 8000;
        public const string PROXY_HOST = "localhost";

        public const bool UNLOAD_TABS_ON_SWITCH = false;


        public const string REPO_URL = "https://github.com/DonalDuck004/ClasseVivaWPF";

        public static readonly string INSTALL_PATH = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;

        public static readonly string MEDIA_DIR_PATH = Path.Join(INSTALL_PATH, "Media");
        public static readonly string SESSIONS_DIR_PATH = Path.Join(INSTALL_PATH, "Sessions");
        public static readonly string THEMES_DIR_PATH = Path.Join(INSTALL_PATH, "Themes");

        public static readonly (int, int, string, string) VERSION = (3, 9, "development version", "dev");
        public static string VERSION_SHORTEST_STRING => $"{VERSION.Item1}.{VERSION.Item2}";
        public static string VERSION_SHORT_STRING => $"{VERSION_SHORTEST_STRING} {VERSION.Item4}";
        public static string VERSION_LONG_STRING => $"{VERSION_SHORTEST_STRING} {VERSION.Item3}";
        public static string VERSION_LONGEST_STRING => $"{VERSION_LONG_STRING} ({VERSION.Item4})";

        public static readonly string LOG_DIR_PATH = Path.Join(INSTALL_PATH, "Logs");
        public static readonly string LOG_FILE_TEMPLATE = $"Log_{DateTime.Now:dd_MM_yyyy}_{{0}}.log";
        public static readonly long MAX_LOG_SIZE = 0xFFFFFFFF; // 1 = 1 byte
        public static readonly int LOG_BUFF_SIZE = 0xFFF;

        static Config()
        {
            if (!Directory.Exists(MEDIA_DIR_PATH))
                Directory.CreateDirectory(MEDIA_DIR_PATH);

            if (!Directory.Exists(SESSIONS_DIR_PATH))
                Directory.CreateDirectory(SESSIONS_DIR_PATH);

            if (!Directory.Exists(LOG_DIR_PATH))
                Directory.CreateDirectory(LOG_DIR_PATH);

            if (!Directory.Exists(THEMES_DIR_PATH))
                Directory.CreateDirectory(THEMES_DIR_PATH);
        }
    }
}
