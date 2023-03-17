using System;

namespace ClasseVivaWPF.Api.Types
{
    public record class Event(int EvtId,
                              string EvtCode,
                              DateTime EvtDate,
                              int? EvtHPos,
                              object? EvtValue,
                              bool IsJustified,
                              char? JustifReasonCode,
                              string JustifReasonDesc,
                              object[] hoursAbsence) : BaseEvent(EvtId, EvtCode);
}
