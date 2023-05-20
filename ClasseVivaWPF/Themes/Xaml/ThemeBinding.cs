using ClasseVivaWPF.Themes.Handling;
using System.Windows.Data;

namespace ClasseVivaWPF.Themes.Xaml
{
    public class ThemeBinding : Binding
    {
        public ThemeBinding()
        {
            this.Source = ThemeProperties.INSTANCE;
        }
    }
}
