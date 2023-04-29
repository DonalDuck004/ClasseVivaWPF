using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClasseVivaWPF.Api.Types
{
    public class Event : BaseEvent
    {
        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDate { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required int? EvtHPos { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required int? EvtValue { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool IsJustified { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? JustifReasonCode { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? JustifReasonDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int[] HoursAbsence { get; init; }

        [JsonIgnore]
        public string FormattedDate => this.EvtDate.ToString("dd MMMM yyyy");

        [JsonIgnore]
        public string FormattedHoursAbsence => HoursAbsence.Length == 0 ? "" : HoursAbsence.Length == 2 ? $"{HoursAbsence[0]} - {HoursAbsence[1]}" : string.Join("; ", HoursAbsence);

        [JsonIgnore]
        public string? JustifReasonCodeBasedDesc => JustifReasonCode is null ? null : AllowedGiustifications[this.JustifReasonCode];

        public static readonly Dictionary<string, string> AllowedGiustifications = new() {
            { "", "Nessuno" },
            { "A", "Salute" },
            { "AC", "Certificato Medico" },
            { "B", "Famiglia" },
            { "C", "Altro" },
            { "D", "Trasporto" },
            { "E", "Sciopero" },
        };

        public static string[] AllowedGiustificationsCodes => AllowedGiustifications.Keys.ToArray();


        public override void BuildNotifyText(ToastContentBuilder toast)
        {
            toast.AddText(GetHeader() + "_TODO_evt");  // TODO
        }

        public override DateTime GetGotoDate() => this.EvtDate;
    }
}
