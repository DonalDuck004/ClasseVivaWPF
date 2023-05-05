using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Utils.Interfaces
{
    public interface IBuildNotify
    {
        int EffectiveID { get; }
        void BuildNotify(ToastContentBuilder toast);
    }

    public interface IBuildNotifyCalendar : IBuildNotify
    {
        DateTime GetGotoDate();
    }

    public interface IBuildNotifyDidatic : IBuildNotify
    {
        string GetSection();
        int GetHighlightID();
    }
}
