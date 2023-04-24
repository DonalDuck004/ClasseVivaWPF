using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
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

        private async Task<IEnumerable<BaseEvent>> Fetch()
        {
            IEnumerable<BaseEvent> events = (await Client.INSTANCE.Overview(DateTime.Now, this.Range)).GetBaseEvents(Grades: false, Absences: false);
            events = events.Concat((await Client.INSTANCE.GetGrades()).ContentGrades); // Grades
            events = events.Concat((await Client.INSTANCE.GetAbsences()).ContentEvents); // Absences
            return events;
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
                        builder.AddArgument("goto_home_date", item.GetGotoDate().ToString());
                        item.BuildNotifyText(builder);
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
