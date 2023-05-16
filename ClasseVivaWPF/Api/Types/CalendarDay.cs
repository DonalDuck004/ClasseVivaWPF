using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{

    public class CalendarDay : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required DateTime DayDate;

        [JsonProperty("DayOfWeek", Required = Required.Always)]
        public required int DayOfWeek;
        // DayOfWeek type uses 0 as sunday

        [JsonProperty(Required = Required.Always)]
        public required string DayStatus;


        #region \smali\eu\spaggiari\classevivastudente\models\students\students\Day$EventCode.smali
        public const string DAY_UNKNOW1 = "ND";
        public const string DAY_UNKNOW2 = "US";

        public const string HOLY_DAY_STATUS = "HD";
        public const string SCHOOL_DAY_STATUS = "SD";
        public const string NO_WORK_STATUS = "NW";
        #endregion

        [JsonIgnore]
        public int NumberDay => this.DayDate.Day;

        public bool IsHolyday => HOLY_DAY_STATUS == DayStatus;
        public bool IsSchoolDay => SCHOOL_DAY_STATUS == DayStatus;
        public bool IsNoWork => NO_WORK_STATUS == DayStatus;
    }
}
