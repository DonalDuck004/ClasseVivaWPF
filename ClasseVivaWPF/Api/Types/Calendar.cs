using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Api.Types
{
    public class Calendar : ApiObject
    {
        [JsonProperty("calendar", Required = Required.Always)]
        public required CalendarDay[] ContentCalendar;
    }
}
