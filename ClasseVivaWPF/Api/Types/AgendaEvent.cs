using ClasseVivaWPF.Utils;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class AgendaEvent : BaseEvent
    {
        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDatetimeBegin { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDatetimeEnd { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool IsFullDay { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Notes { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string AuthorName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string ClassDesc { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required int? SubjectId { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SubjectDesc { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required object? HomeworkId { get; init; }

        public override void BuildNotifyText(ToastContentBuilder toast)
        {
            var when = IsFullDay ? "tutto il giorno" : $"{EvtDatetimeBegin:dddd d}";
            var header = (SubjectDesc is null ? AuthorName : SubjectDesc).ToTitle();
            toast.AddText($"{header} per {when}");
            toast.AddText(Notes);  // TODO
        }

        public override DateTime GetGotoDate() => this.EvtDatetimeBegin;
    }
}
