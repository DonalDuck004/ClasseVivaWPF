using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public abstract class BaseEvent : ApiObject
    {
        #region \smali\eu\spaggiari\classevivastudente\models\students\students\Agenda$EventCode.smali
        public const string AGENDA_START_STRING = "AG";
        public const string AGENDA_HOMEWORK = "AGHW";
        public const string AGENDA_NOTE = "AGNT";
        public const string AGENDA_UNKNOW1 = "AGCR";
        public const string AGENDA_UNKNOW2 = "AGDD";
        #endregion

        #region \smali\eu\spaggiari\classevivastudente\models\students\students\Absence$EventCode.smali
        public const string ABSENCE_ABSENCE_START_STRING = "AB";

        public const string ABSENCE_EARLY_EXIT_START_STRING = "ABU";
        public const string ABSENCE_EARLY_EXIT = "ABU0";

        public const string ABSENCE_START_STRING = "ABA";
        public const string ABSENCE_ABSENCE = "ABA0";

        public const string ABSENCE_LATE_START_STRING = "ABR";
        public const string ABSENCE_LATE = "ABR0";
        public const string ABSENCE_SHORT_LATE = "ABR1";
        #endregion

        #region \smali\eu\spaggiari\classevivastudente\models\students\students\Lesson$EventCode.smali
        public const string LESSON_START_STRING = "LS";
        public const string LESSON_PRESENCE = "LSF0";
        public const string LESSON_CO_PRESENCE = "LSC0";
        public const string LESSON_SUPPORT = "LSS0";
        #endregion

        #region \smali\eu\spaggiari\classevivastudente\models\students\students\Day$EventCode.smali
        public const string DAY_UNKNOW1 = "SD";
        public const string DAY_UNKNOW2 = "ND";
        public const string DAY_UNKNOW3 = "HD";
        public const string DAY_UNKNOW4 = "NW";
        public const string DAY_UNKNOW5 = "US";
        #endregion

        #region \smali\eu\spaggiari\classevivastudente\models\students\students\Grade$EventCode.smali
        public const string GRADE_GRADE_START_STRING = "GR";

        public const string GRADE_GRADE = "GRV0";
        public const string GRADE_GRADE_UNKNOW1 = "GRV1"; // Only this counts in grades periods app sections
        
        public const string GRADE_GRADE_UNKNOW2 = "GRV2"; // https://pasteboard.co/Jvu8mZckip60.png
        // Doesn't count in average

        public const string GRADE_GRADE_UNKNOW3 = "GRT1";

        public const string GRADE_GRADE_UNKNOW4 = "GRA1";
        #endregion

        [JsonProperty(Required = Required.Always)]
        public required int EvtId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string EvtCode { get; init; }

        public bool IsHomework => this.EvtCode == AGENDA_HOMEWORK;
        public bool IsNote => this.EvtCode == AGENDA_NOTE;
        public bool IsEarlyExit => this.EvtCode == ABSENCE_EARLY_EXIT;
        public bool IsAbsence => this.EvtCode == ABSENCE_ABSENCE;
        public bool IsLate => this.EvtCode == ABSENCE_LATE;
        public bool IsShortLate => this.EvtCode == ABSENCE_SHORT_LATE;
        public bool IsPresence => this.EvtCode == LESSON_PRESENCE;
        public bool IsCoPresence => this.EvtCode == LESSON_CO_PRESENCE;
        public bool IsSupport => this.EvtCode == LESSON_SUPPORT;
        public bool IsGrade => this.EvtCode == GRADE_GRADE;


        public bool IsInAbsenceSection => this.EvtCode.StartsWith(ABSENCE_ABSENCE_START_STRING);
        public bool IsLesson => this.EvtCode.StartsWith(LESSON_START_STRING);
        public bool IsGenericCoPresence => IsCoPresence || IsSupport;

        public int EffectiveID => this.GetHashCode();

        public string GetHeader()
        {
            if (IsHomework)
                return "Compiti";

            if (IsNote)
                return "Agenda";

            if (IsInAbsenceSection)
                return "Assenze";

            if (IsLesson)
                return "Lezioni";

            if (IsGrade)
                return "Voti";

            throw new NotImplementedException();
        }

        public abstract void BuildNotifyText(ToastContentBuilder toast);
        public abstract DateTime GetGotoDate();

        public override int GetHashCode()
        {
            return HashCode.Combine(this.EvtId, this.EvtCode);
        }
    }
}
