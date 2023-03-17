using System;
using System.Windows.Controls;

namespace ClasseVivaWPF.Api.Types
{
    public record class ApiObject()
    {
        public DateTime XCreationDate { get; private set; } = DateTime.Now;

        public bool IsError()
        {
            return this.GetType().IsSubclassOf(typeof(ApiErrorObject));
        }
    }
}
