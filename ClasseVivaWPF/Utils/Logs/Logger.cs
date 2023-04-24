using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Utils.Logs
{
    public class Logger
    {
        public static Logger INSTANCE { get; } = new();
        public Stream OutStrem { get; set; }

        private Logger() { 

        }

    }
}
