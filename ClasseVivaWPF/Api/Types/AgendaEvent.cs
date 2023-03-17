using System;

namespace ClasseVivaWPF.Api.Types
{
    public record class AgendaEvent(int EvtId,
                                    string EvtCode,
                                    DateTime EvtDatetimeBegin,
                                    DateTime EvtDatetimeEnd,
                                    bool IsFullDay,
                                    string Notes,
                                    string AuthorName,
                                    string ClassDesc,
                                    int? SubjectId,
                                    string SubjectDesc,
                                    object HomeworkId) : BaseEvent(EvtId, EvtCode);
}
