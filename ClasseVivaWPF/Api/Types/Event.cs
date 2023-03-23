using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;


namespace ClasseVivaWPF.Api.Types
{
    public class Event : BaseEvent
    {
        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDate { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required int? EvtHPos { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required object? EvtValue { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool IsJustified { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required char? JustifReasonCode { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? JustifReasonDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int[] HoursAbsence { get; init; }

        public override void BuildNotifyText(ToastContentBuilder toast)
        {
            toast.AddText(GetHeader() + "evt");  // TODO
        }

        public override DateTime GetGotoDate() => this.EvtDate;
    }
}
