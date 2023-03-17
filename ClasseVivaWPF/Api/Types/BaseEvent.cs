using System;

namespace ClasseVivaWPF.Api.Types
{
    public record class BaseEvent(int EvtId,
                                  string EvtCode) : ApiObject
    {
        public const string AGENDA_HOMEWORK = "AGHW";
        public const string AGENDA_NOTE = "AGNT";
        public const string EARLY_EXIT = "ABU0";
        public const string ABSANCE = "ABA0";
        public const string LATE = "ABR0";
        public const string PRESENCE = "LSF0";
        public const string CO_PRESENCE = "LSC0";
        public const string SUPPORT = "LSS0";
        
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

            throw new NotImplementedException();
        }
    }
}
