using Newtonsoft.Json;
using System;

namespace ClasseVivaWPF.Api.Types
{
    public class Frame : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int FrameId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int FrameHPos { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime FrameDate { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FramePlace { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FrameTimeRange { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FrameTimeFrom { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FrameTimeTo { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int OverallSlots { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int BusySlotMask { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int BusySlotFlags { get; init; } // TODO: Check from smali for an enum(?)

        [JsonProperty(Required = Required.Always)]
        public required object FreeSlotTimeRange { get => new(); init => throw new NotImplementedException(value.ToString()!); }
    }
}
