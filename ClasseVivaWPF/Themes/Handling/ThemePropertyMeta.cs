using System;

namespace ClasseVivaWPF.Themes.Handling
{
    public class ThemePropertyMeta : Attribute {
        public required string BindsTo { get; init; }
    }
}
