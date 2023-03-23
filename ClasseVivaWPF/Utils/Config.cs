using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClasseVivaWPF.Utils
{
    public static class Config
    {
        public const double HEADER_HEIGHT = 48.0D;
        public static readonly GridLength HEADER_HEIGHT_GL = new GridLength(48.0D);

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
    }
}
