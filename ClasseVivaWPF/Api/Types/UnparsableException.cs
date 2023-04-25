using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClasseVivaWPF.Api.Types
{
    public class UnparsableException : Exception
    {
        public required Response response;
    }
}
