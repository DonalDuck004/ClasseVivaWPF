using System.Windows.Input;
using System.Windows.Documents;

namespace ClasseVivaWPF.Utils
{
    public interface IOnKeyDown
    {
        void OnKeyDown(object sender, KeyEventArgs e);
    }
}
