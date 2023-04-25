using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class Noticeboard : BaseEvent
    {
        [JsonProperty(Required = Required.Always)]
        public required int PubId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime PubDT { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool ReadStatus { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int CntId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime CntValidFrom { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime CntValidTo { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool CntValidInRange { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string CntStatus { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string CntTitle { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string CntCategory { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool CntHasChanged { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool CntHasAttach { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool NeedJoin { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool NeedReply { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool NeedFile { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool NeedSign { get; init; } // TODO Play with official app, in order to see how some attrs are handled/displayed :D

        [JsonProperty("evento_id", Required = Required.Always)]
        public required override int EvtId { get; init; }

        [JsonProperty("dinsert_allegato", Required = Required.Always)]
        public required bool AttachmentInsertDT { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required NoticeboardAttachment[] Attachments { get; init; }

        public override DateTime GetGotoDate() => PubDT;

        public override void BuildNotifyText(ToastContentBuilder toast)
        {
            toast.AddText(GetHeader() + "_TODO_evt");  // TODO
        }

    }
}
}
