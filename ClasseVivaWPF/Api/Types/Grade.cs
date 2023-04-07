using ClasseVivaWPF.Utils;
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
        public required int SubjectId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SubjectCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SubjectDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime EvtDate { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required double? DecimalValue { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string DisplayValue { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int DisplaPos { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string NotesForFamily { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Color { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool Canceled { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool Underlined { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int PeriodPos { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string PeriodDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int ComponentPos { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string ComponentDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int WeightFactor { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int SkillId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int GradeMasterId { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SkillDesc { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SkillCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int SkillMasterId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SkillValueDesc { get; init; }

        [JsonProperty(Required = Required.AllowNull)]
        public required string? SkillValueShortDesc { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int OldskillId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string OldskillDesc { get; init; }

        public string ShortSubjectName => this.SubjectDesc.Substring(0, 3).ToUpper();


        private static Dictionary<string, Color> CColor = new Dictionary<string, Color>()
        {
            {"green", System.Windows.Media.Color.FromRgb(0x83, 0xB5, 0x88) },
            {"red", Config.CV_RED },
            {"blue", System.Windows.Media.Color.FromRgb(0x5D, 0x97, 0xB1) }
        };

        public Color ApiGivenColor
        {
            get
            {
                if (!CColor.ContainsKey(this.Color))
                {
                    var c = typeof(Colors).GetProperty(this.Color, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                    CColor[this.Color] = (Color)(c ?? throw new Exception($"Invalid color {this.Color}")).GetValue(null)!;
                }

                return CColor[this.Color];
            }
        }

        public Color InternalColor => 
            this.DecimalValue is null ? System.Windows.Media.Color.FromRgb(0x5D, 0x97, 0xB1) :
            this.DecimalValue < 5 ? System.Windows.Media.Color.FromRgb(0xD0, 0x5A, 0x50) : 
            this.DecimalValue < 6 ? System.Windows.Media.Color.FromRgb(0xEB, 0x98, 0x60) :
            System.Windows.Media.Color.FromRgb(0x83, 0xB5, 0x88);

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
