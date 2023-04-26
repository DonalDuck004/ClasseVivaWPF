using System;

namespace ClasseVivaWPF.Utils.Themes
{
    public class ThemePropertyMeta : Attribute {
        public required string BindsTo { get; init; }
    }
}
