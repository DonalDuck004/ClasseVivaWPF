using ClasseVivaWPF.SharedControls;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class ApiError : Exception
    {
        public ApiErrorObject? Error { get; init; }
        public Exception? WrappedExc { get; init; }

        public ApiError(ApiErrorObject? Error) : base(Error?.Message)
        {
            this.Error = Error;
            this.WrappedExc = null;
        }

        public ApiError(Exception? exc) : base(exc?.Message)
        {
            this.WrappedExc = exc;
            this.Error = null;
        }


        public void ApplyStdProcedure(string header = "")
        {
#if DEBUG
            // throw this;
#endif
            new CVMessageBox(header, this.Message).Inject();
        }
    }
}
