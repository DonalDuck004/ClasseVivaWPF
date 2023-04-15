using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;

namespace ClasseVivaWPF.LoginControls
{
    /// <summary>
    /// Logica di interazione per CVLoginAccountSelector.xaml
    /// </summary>
    public partial class CVLoginAccountSelector : Injectable
    {
        public string? Result = null;

        public CVLoginAccountSelector()
        {
            InitializeComponent();
        }

        public CVLoginAccountSelector(LoginMultipleChoice C)
        {
            InitializeComponent();

            CVLoginAccountOption opt;

            foreach (var choice in C.Choices)
            {
                this.Choices.Children.Add(opt = new(choice));
                opt.MouseLeftButtonDown += OnSelected;
            }
        }

        public void OnSelected(object sender, EventArgs e)
        {
            Result = ((LoginChoice)((CVLoginAccountOption)sender).DataContext).Ident;

            this.Close();
        }
    }
}
