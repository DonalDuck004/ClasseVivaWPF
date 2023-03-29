using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace ClasseVivaWPF.Api.Types
{
    public class Grade : BaseEvent
    {
        [JsonProperty(Required = Required.Always)]
        public required int SubjectId;

        [JsonProperty(Required = Required.Always)]
        public required string SubjectCode;

        [JsonProperty(Required = Required.Always)]
        public required string SubjectDesc;

        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDate;

        [JsonProperty(Required = Required.AllowNull)]
        public required double? DecimalValue;

        [JsonProperty(Required = Required.Always)]
        public required string DisplayValue;

        [JsonProperty(Required = Required.Always)]
        public required int DisplaPos;

        [JsonProperty(Required = Required.Always)]
        public required string NotesForFamily;

        [JsonProperty(Required = Required.Always)]
        public required string Color;

        [JsonProperty(Required = Required.Always)]
        public required bool Canceled;

        [JsonProperty(Required = Required.Always)]
        public required bool Underlined;

        [JsonProperty(Required = Required.Always)]
        public required int PeriodPos;

        [JsonProperty(Required = Required.Always)]
        public required string PeriodDesc;

        [JsonProperty(Required = Required.Always)]
        public required int ComponentPos;

        [JsonProperty(Required = Required.Always)]
        public required string ComponentDesc;

        [JsonProperty(Required = Required.Always)]
        public required int WeightFactor;

        [JsonProperty(Required = Required.Always)]
        public required int SkillId;

        [JsonProperty(Required = Required.Always)]
        public required int GradeMasterId;

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SkillDesc;

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SkillCode;

        [JsonProperty(Required = Required.Always)]
        public required int SkillMasterId;

        [JsonProperty(Required = Required.Always)]
        public required string SkillValueDesc;

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SkillValueShortDesc;

        [JsonProperty(Required = Required.Always)]
        public required int OldskillId;

        [JsonProperty(Required = Required.Always)]
        public required string OldskillDesc;

        private static Dictionary<string, Color> DYNAMIC_OBTAINED_COLOR = new Dictionary<string, Color>()
        {
            {"green", System.Windows.Media.Color.FromRgb(0x83, 0xb4, 0x88) },
            {"blue", System.Windows.Media.Color.FromRgb(0x5D, 0x97, 0xB1) }
        };

        public Color GetColor()
        {
            if (!DYNAMIC_OBTAINED_COLOR.ContainsKey(this.Color))
            {
                var c = typeof(Colors).GetProperty(this.Color, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                DYNAMIC_OBTAINED_COLOR[this.Color] = (Color)(c ?? throw new Exception($"Invalid color {this.Color}")).GetValue(null)!;
            }

            return DYNAMIC_OBTAINED_COLOR[this.Color];
        }

        public override void BuildNotifyText(ToastContentBuilder toast)
        {
            string s;
            if (this.SubjectCode != "" && this.SubjectDesc.Length > 24)
                s = $"{this.SubjectCode} ({this.SubjectDesc.Substring(0, 24 - this.SubjectCode.Length - 6)}...)";
            else
                s = this.SubjectDesc;

            var ext = this.DecimalValue is null ? "" : $"({this.DecimalValue:0.00})";
            toast.AddText($"Nuovo voto in {s}");
            toast.AddText($"{this.ComponentDesc} {this.DisplayValue}{ext}");
            toast.AddText(this.NotesForFamily);
        }

        public override DateTime GetGotoDate() => this.EvtDate;
    }
}
