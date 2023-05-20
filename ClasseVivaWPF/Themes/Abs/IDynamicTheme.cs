namespace ClasseVivaWPF.Themes.Abs
{
    public interface IDynamicTheme : ITheme
    {
        new string Name { get; set; }
    }
}
