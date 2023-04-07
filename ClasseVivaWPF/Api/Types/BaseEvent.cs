using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public abstract class BaseEvent : ApiObject
    {
        public const string AGENDA_START_STRING = "AG";
        public const string AGENDA_HOMEWORK = "AGHW";
        public const string AGENDA_NOTE = "AGNT";

        public const string GENERIC_ABSENCE_START_STRING = "AB";
        
        public const string EARLY_EXIT_START_STRING = "ABU";
        public const string EARLY_EXIT = "ABU0";

        public const string ABSENCE_START_STRING = "ABA";
        public const string ABSENCE = "ABA0";

        public const string LATE_START_STRING = "ABR";
        public const string LATE = "ABR0";
        public const string SHORT_LATE = "ABR1";

        public const string PRESENCE_STRING = "LS";
        public const string PRESENCE = "LSF0";
        public const string CO_PRESENCE = "LSC0";
        public const string SUPPORT = "LSS0";

        public const string GRADE = "GRV0";

        [JsonProperty(Required = Required.Always)]
        public required int EvtId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string EvtCode { get; init; }

        public bool IsHomework => this.EvtCode == AGENDA_HOMEWORK;
        public bool IsNote => this.EvtCode == AGENDA_NOTE;
        public bool IsEarlyExit => this.EvtCode == EARLY_EXIT;
        public bool IsAbsence => this.EvtCode == ABSENCE;
        public bool IsLate => this.EvtCode == LATE;
        public bool IsShortLate => this.EvtCode == SHORT_LATE;
        public bool IsPresence => this.EvtCode == PRESENCE;
        public bool IsCoPresence => this.EvtCode == CO_PRESENCE;
        public bool IsSupport => this.EvtCode == SUPPORT;
        public bool IsGrade => this.EvtCode == GRADE;


        public bool IsInAbsenceSection => this.EvtCode.StartsWith(GENERIC_ABSENCE_START_STRING);
        public bool IsLesson => this.EvtCode.StartsWith(PRESENCE_STRING);
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
