using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.SharedControls;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsence.xaml
    /// </summary>
    public partial class CVAbsence : UserControl
    {
        private SemaphoreSlim PreventOverlap { get; } = new SemaphoreSlim(1, 1);

        private CVAbsence()
        {
            InitializeComponent();
        }

        public CVAbsence(Event absence)
        {
            InitializeComponent();
            this.DataContext = absence;
        }

        private async void OnJustify(object sender, RoutedEventArgs e)
        {
            if (PreventOverlap.CurrentCount == 0)
                return;

            await PreventOverlap.WaitAsync();

            try
            {
                var popup = new CVJustifyPopup((Event)this.DataContext);
                popup.Inject();
                await popup.WaitForExit();

                if (popup.Result is not null)
                {
                    var ok = await Client.INSTANCE.BridgedGiustifyAbsence((Event)this.DataContext, popup.Result.JCode, popup.Result.Desc);

                    if (ok)
                        await CVAbsencesViewer.INSTANCE!.Reload();
                    else
                        new CVMessageBox("Errore sconosciuto", "Consulta i log per maggiori dettagli").Inject();
                }
            }
            finally
            {
                PreventOverlap.Release();
            }
        }
    }
}
