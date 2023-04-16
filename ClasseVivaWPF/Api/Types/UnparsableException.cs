using System;

namespace ClasseVivaWPF.Api.Types
{
    public class UnparsableException: Exception
    {
        public required Response response;
    }
}
