using System;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsencesViewer.xaml
    /// </summary>
    /// 
    public record class DayStatus(DateTime Date, bool IsPresent, bool IsAbsent, bool IsLate, bool IsEarlyExit, bool IsPartiallyAbsent)
    {
        public bool AllFalse => !(IsPresent || IsAbsent || IsLate || IsEarlyExit || IsPartiallyAbsent);
    }
}
