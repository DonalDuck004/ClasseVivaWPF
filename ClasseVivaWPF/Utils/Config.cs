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


        public const string REPO_URL = "https://github.com/DonalDuck004/ClasseVivaWPF";

        public static readonly string INSTALL_PATH = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;

        public static readonly string MEDIA_DIR_PATH = Path.Join(INSTALL_PATH, "media");
        public static readonly string SESSIONS_DIR_PATH = Path.Join(INSTALL_PATH, "Sessions");

        public static readonly (int, int, string, string) VERSION = (2, 7, "development version", "dev");
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
