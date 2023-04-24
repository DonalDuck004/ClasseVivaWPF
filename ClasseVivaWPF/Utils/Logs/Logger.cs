#define LOG_ENABLED

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.WebUI;


namespace ClasseVivaWPF.Utils.Logs
{
    public static class Logger
    {
#if DEBUG
        public static LogLevel LOG_FROM = LogLevel.DEBUG;
#elif TRACE
        public static LogLevel LOG_FROM = LogLevel.TRACE;
#else
        public static LogLevel LOG_FROM = LogLevel.INFO;
#endif

        private static Stream Stream;
        public static Encoding StreamEncoding;
        private static int level_len;
        public static bool Faulted { get; private set; } = false;

        private static object lock_base { get; } = new object();


#if LOG_ENABLED
        static Logger() {
            try
            {
                var c = 0;
                string path;
                StreamEncoding = Encoding.UTF8;
                level_len = Enum.GetNames(typeof(LogLevel)).Max(x => x.Length);

                if (!Directory.Exists(Config.LOG_DIR_PATH))
                    Directory.CreateDirectory(Config.LOG_DIR_PATH);

                while (true)
                {
                    while (File.Exists(path = Path.Join(Config.LOG_DIR_PATH, string.Format(Config.LOG_FILE_TEMPLATE, ++c))))
                        ;

                    try
                    {
                        Stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, Config.LOG_BUFF_SIZE);
                        break;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                        Thread.Sleep(500);
                    }
                }
            }
            catch
            {
                Faulted = true;
            }
        }
#endif

        public static bool CanLog(LogLevel level = LogLevel.INFO) => level >= LOG_FROM;

        public static bool Log(string message, LogLevel level = LogLevel.INFO)
        {
            if (Faulted)
                return false;

#if LOG_ENABLED
            if (!CanLog(level))
                return false;

            try
            {
                var msg = $"|{level + new string(' ', level_len - level.ToString()!.Length)}|{DateTime.Now:dd/MM/yyyy}|";

                if (message.Contains('\n'))
                    msg += $"\n{new string('▼', 12)}\n{message}\n{new string('▲', 12)}\n{msg}\n";
                else
                    msg += message + "\n";

                var buff = StreamEncoding.GetBytes(msg);

                lock (lock_base)
                {
                    Stream.Write(buff, 0, buff.Length);
                    Stream.Flush();
                }
                return true;
            }catch (Exception e)
            {
                return false;
            }
#endif
        }


        public static void SetStream(Stream stream, Encoding encoding, bool CloseOld = true, bool FlushOld = true)
        {
            if (CloseOld)
                Stream.Close();
            else if (FlushOld)
                Stream.Flush();

            Stream = stream;
            StreamEncoding = encoding;
        }
    }
}
