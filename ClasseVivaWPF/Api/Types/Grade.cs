using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using ClasseVivaWPF.Themes;
using ClasseVivaWPF.Themes.Handling;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace ClasseVivaWPF.Api.Types
{
    public class Grade : BaseEvent, IBuildNotifyCalendar
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

        private string? acronym = null;

        public string SubjectAcronym => acronym ??= string.Join("", this.SubjectDesc.ToTitle(false).Split().Where(x => x.Length > 2).Select(x => x[0]));


        private static Dictionary<string, Color> CColor = new Dictionary<string, Color>()
        {
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

        public string InternalColorPath => 
            this.DecimalValue is null ? ThemeOperations.CV_GRADE_NOTE_PATH :
            this.DecimalValue < 5 ? ThemeOperations.CV_GRADE_INSUFFICIENT_PATH : 
            this.DecimalValue < 6 ? ThemeOperations.CV_GRADE_SLIGHTLY_INSUFFICIENT_PATH :
            ThemeOperations.CV_GRADE_SUFFICIENT_PATH;

        public DependencyProperty InternalColorProperty =>
            this.DecimalValue is null ? ThemeProperties.CVGradeNoteProperty :
            this.DecimalValue < 5 ? ThemeProperties.CVGradeInsufficientProperty :
            this.DecimalValue < 6 ? ThemeProperties.CVGradeSlightlyInsufficientProperty :
            ThemeProperties.CVGradeSufficientProperty;

        public void BuildNotify(ToastContentBuilder toast)
        {
            string s;
            if (this.SubjectCode != "" && this.SubjectDesc.Length > 24)
                s = $"{this.SubjectCode} ({this.SubjectDesc.Substring(0, 24 - this.SubjectCode.Length - 6)}...)";
            else
                s = this.SubjectDesc.ToTitle();

            var ext = this.DecimalValue is null ? "" : $" - {this.DecimalValue:0.00} decimi";
            toast.AddText($"Nuovo voto in {s}");
            toast.AddText($"{this.ComponentDesc} {this.DisplayValue}{ext}");
            toast.AddText(this.NotesForFamily);
        }

        public DateTime GetGotoDate() => this.EvtDate.Date;
    }
}
