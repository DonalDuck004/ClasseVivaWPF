using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Sessions;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClasseVivaWPF.Utils
{
    public class NotificationSystem
    {
        public static NotificationSystem INSTANCE { get; } = new NotificationSystem();

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
        private int deb = 40;
        private async Task<IEnumerable<BaseEvent>> Fetch()
        {
            var events = (await Client.INSTANCE.Overview(DateTime.Now, this.Range)).GetBaseEvents(Grades: false);
            return events.Concat((await Client.INSTANCE.GetGrades()).ContentGrades.Take(++deb));
        }

        private async Task Listener()
        {
            var displayed_ids = (await this.Fetch()).Select(x => x.EffectiveID).ToList();

            ToastContentBuilder builder;

            while (this.run)
            {
                foreach (var item in (await this.Fetch()).Where(x => !displayed_ids.Contains(x.EffectiveID)))
                {
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

                await Task.Delay(Config.NOTIFY_UPDATE_DELAY);
            }
        }

        public void Stop()
        {
            this.run = false;
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
    }
}
