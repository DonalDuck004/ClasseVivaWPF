using Newtonsoft.Json;
using System;
using System.Windows.Controls;

namespace ClasseVivaWPF.Api.Types
{
    public class ApiObject
    {
        public DateTime XCreationDate { get; private set; } = DateTime.Now;

        public bool IsError()
        {
            return this.GetType().IsSubclassOf(typeof(ApiErrorObject));
        }

        public override string ToString() => (string)this;

        public static explicit operator string (ApiObject self)
        {
            return JsonConvert.SerializeObject(self);
        }
    }
}
