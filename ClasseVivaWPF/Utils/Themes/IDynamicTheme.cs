namespace ClasseVivaWPF.Utils.Themes
{
    public interface IDynamicTheme : ITheme
    {
        new string Name { get; set; }
    }
}
