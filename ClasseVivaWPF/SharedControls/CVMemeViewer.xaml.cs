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
using ClasseVivaWPF;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Api;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVMemeViewer.xaml
    /// </summary>
    public partial class CVMemeViewer : CVExtraBase
    {
#if DEBUG
        private CVMemeViewer()
        {
            InitializeComponent();
        }
#endif

        static CVMemeViewer()
        {

        }

        public CVMemeViewer(Content content) : base(content.ContentID)
        {
            InitializeComponent();
            this.DataContext = this;

            foreach (var x in content.Related)
                this.ExtraWp.Children.Add(new CVImage(x));

            new Task(async () => await Client.INSTANCE.SetInteraction(content.ContentID, Interaction.REACTION_CLICK));
            // Todo Gestire .Detail 13 feb.
            this.main_img.AsyncLoading(content.Gallery!);
        }
    }
}
