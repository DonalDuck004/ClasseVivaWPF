using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using ClasseVivaWPF.Utils.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClasseVivaWPF.Utils
{
    public class NotificationSystem
    {
        public const string GOTO_HOME = "goto_home_date";
        public const string GOTO_DIDATIC = "goto_ditatic_ditatic";
        public const string GOTO_HOMEWORKS = "goto_ditatic_homeworks";

        public static NotificationSystem INSTANCE { get; private set; } = new();

        private Task? task;

        private bool quiteNext = false;
        private bool run = false;
        public bool IsRunning => this.task is null ? false : !this.task.IsFaulted;
        public bool IsActive => this.run;
        private int Range;

        private NotificationSystem()
        {
            MainWindow.INSTANCE.PostLogin += () =>
            {
                this.Stop();
                if (SessionHandler.INSTANCE!.GetNotificationsFlag())
                    this.SpawnTask();

                this.Range = SessionHandler.INSTANCE!.GetNotificationsRange();
                SessionHandler.INSTANCE!.NotificationsFlagChanged += OnNotificationsFlagChanged;
                SessionHandler.INSTANCE!.NotificationsRangeChanged += OnNotificationsRangeChanged;
            };
        }

        public void SpawnTask()
        {
            if (this.task is not null && this.IsRunning)
                return;

            this.run = true;
           
            this.task = Task.Run(Listener);
            this.task.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully is false)
                {
                    MessageBox.Show("F"); // TODO Logging
                    SpawnTask();
                }
            });
        }

        private async Task<IEnumerable<IBuildNotify>> Fetch()
        {
            IEnumerable<IBuildNotify> events = (await Client.INSTANCE.Overview(DateTime.Now, this.Range)).GetBaseEvents(Grades: false, Absences: false).Cast<IBuildNotify>();
            events = events.Concat((await Client.INSTANCE.GetGrades()).ContentGrades); // Grades
            events = events.Concat((await Client.INSTANCE.GetAbsences()).ContentEvents); // Absences
            events = events.Concat((await Client.INSTANCE.Didatics()).DidacticsContent.Select(x => x.Folders.Select(x => x.Contents).Merge()).Merge()); // Didatics

#if DEBUG
            var rnd = new Random();
            return events.Where(x => rnd.Next(0, 51) != 0);
#else
            return events;
#endif
        }

        private async Task Listener()
        {
            var displayed_ids = (await this.Fetch()).Select(x => x.EffectiveID).ToList();

            ToastContentBuilder builder;

            DateTime BeginSleep;

            while (this.run)
            {
                foreach (var item in (await this.Fetch()).Where(x => !displayed_ids.Contains(x.EffectiveID)))
                {
                    Debug.Assert(!displayed_ids.Contains(item.EffectiveID));
                    displayed_ids.Add(item.EffectiveID);
                    if (!quiteNext)
                    {
                        builder = new ToastContentBuilder();
                        if (item is IBuildNotifyCalendar x)
                        {
#if DEBUG
                            var t = x.GetGotoDate();
                            var msg = $"Invalid date returned ({t}) in type {item.GetType().Name}";
                            Debug.Assert(t == t.Date, msg);
#endif
                            builder.AddArgument(GOTO_HOME, x.GetGotoDate().ToString());
                        }
                        else if (item is IBuildNotifyDidatic y)
                            builder.AddArgument(y.GetSection(), y.GetHighlightID().ToString());

                        item.BuildNotify(builder);
                        builder.Show();
                    }
                }

                quiteNext = false;
                BeginSleep = DateTime.Now;
                while (DateTime.Now - BeginSleep < TimeSpan.FromMilliseconds(Config.NOTIFY_UPDATE_DELAY))
                {
                    await Task.Delay(200);
                    if (!this.run)
                        break;
                }
            }
        }

        public void Stop()
        {
            this.run = false;

            try
            {
                if (this.task is not null)
                    this.task.Wait();
            }catch (Exception)
            {
                ;
            }

            this.task = null;
        }

        private void OnNotificationsFlagChanged(SessionHandler sender, bool Flag)
        {
            if (Flag == this.IsActive)
                return;

            if (Flag)
                this.SpawnTask();
            else
                this.Stop();
        }

        private void OnNotificationsRangeChanged(SessionHandler sender, int Range)
        {
            this.Range = Range;
            this.quiteNext = true;
        }

        public static void AvoidCompilationExcelusion()
        {
            INSTANCE = INSTANCE;
        }
    }
}
