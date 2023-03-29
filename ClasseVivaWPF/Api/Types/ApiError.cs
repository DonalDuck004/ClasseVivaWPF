using ClasseVivaWPF.SharedControls;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class ApiError : Exception
    {
        public ApiErrorObject? Error { get; init; }

        public ApiError(ApiErrorObject? Error) : base(Error?.Message)
        {
            this.Error = Error;
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
