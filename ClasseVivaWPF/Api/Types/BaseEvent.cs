using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public abstract class BaseEvent : ApiObject
    {
        public const string AGENDA_HOMEWORK = "AGHW";
        public const string AGENDA_NOTE = "AGNT";
        public const string EARLY_EXIT = "ABU0";
        public const string ABSANCE = "ABA0";
        public const string LATE = "ABR0";
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
        public bool IsAbsence => this.EvtCode == ABSANCE;
        public bool IsLate => this.EvtCode == LATE;
        public bool IsPresence => this.EvtCode == PRESENCE;
        public bool IsCoPresence => this.EvtCode == CO_PRESENCE;
        public bool IsSupport => this.EvtCode == SUPPORT;
        public bool IsInAbsenceSection => IsAbsence || IsEarlyExit || IsLate;
        public bool IsLesson => IsPresence || IsCoPresence || IsSupport;
        public bool IsGenericCoPresence => IsCoPresence || IsSupport;
        public bool IsGrade => this.EvtCode == GRADE;

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
