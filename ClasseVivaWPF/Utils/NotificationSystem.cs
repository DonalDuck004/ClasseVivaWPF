﻿using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClasseVivaWPF.Utils
{
    public class NotificationSystem
    {
        public static NotificationSystem INSTANCE { get; } = new NotificationSystem();

        private Task? task;

        private bool run = true;
        public bool IsRunning => task is null ? false : !task.IsFaulted;

        private NotificationSystem()
        {
        }

        public void SpawnTask()
        {
            if (this.task is not null && this.IsRunning)
                return;

            this.task = Task.Run(Listener);
            this.task.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully is false)
                {
                    MessageBox.Show("F"); // TODO Logging
                    SpawnTask();
                }
            });

            this.run = true;
        }

        private async Task Listener()
        {
            var displayed_ids = (await Client.INSTANCE.Overview(DateTime.Now)).GetBaseEvents().Select(x => x.EffectiveID).ToList();

            BaseEvent[] overview;
            ToastContentBuilder builder;

            while (this.run)
            {
                overview = (await Client.INSTANCE.Overview(DateTime.Now)).GetBaseEvents();

                foreach (var item in overview.Where(x => !displayed_ids.Contains(x.EffectiveID)))
                {
                    displayed_ids.Add(item.EffectiveID);

                    builder = new ToastContentBuilder();
                    builder.AddArgument("goto_home_date", item.GetGotoDate().ToString());
                    item.BuildNotifyText(builder);
                    builder.Show();
                }

                await Task.Delay(Config.NOTIFY_UPDATE_DELAY);
            }
        }

        internal void Stop()
        {
            this.run = false;
        }
    }
}
