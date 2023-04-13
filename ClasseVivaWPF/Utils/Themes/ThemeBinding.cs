using System.Windows.Data;

namespace ClasseVivaWPF.Utils.Themes
{
    public class ThemeBinding : Binding
    {
        public ThemeBinding()
        {
            this.Source = ThemeProperties.INSTANCE;
        }
    }
}
