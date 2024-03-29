﻿using Microsoft.Toolkit.Uwp.Notifications;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Forms = System.Windows.Forms;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex RunLock; // prevent from get destructed

        public App() : base() {
            if (!Debugger.IsAttached)
            {
#if DEBUG
                if (!Debugger.Launch())
#endif
                    this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            RunLock = new Mutex(true, "ClasseVivaDesktop", out bool was_running);
            Forms.Application.EnableVisualStyles();

            if (!was_running)
            {
                Application.Current.Shutdown();
                return;
            }

            var cult = new CultureInfo("it-IT");

            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {

                // if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
                //    MessageBox.Show("Chiusa ed avviato da notifica");

                var args = ToastArguments.Parse(toastArgs.Argument);


                Application.Current.Dispatcher.Invoke(delegate
                {

                    ClasseVivaWPF.MainWindow.INSTANCE.Goto(args);
                });
            };

            cult.DateTimeFormat.MonthNames = cult.DateTimeFormat.MonthNames.Select(cult.TextInfo.ToTitleCase).ToArray();
            cult.DateTimeFormat.MonthGenitiveNames = cult.DateTimeFormat.MonthGenitiveNames.Select(cult.TextInfo.ToTitleCase).ToArray();
            cult.DateTimeFormat.DayNames = cult.DateTimeFormat.DayNames.Select(cult.TextInfo.ToTitleCase).ToArray();
            cult.DateTimeFormat.ShortestDayNames = cult.DateTimeFormat.ShortestDayNames.Select(cult.TextInfo.ToTitleCase).ToArray();
            cult.DateTimeFormat.AbbreviatedMonthNames = cult.DateTimeFormat.AbbreviatedMonthNames.Select(cult.TextInfo.ToTitleCase).ToArray();
            cult.DateTimeFormat.AbbreviatedMonthGenitiveNames = cult.DateTimeFormat.AbbreviatedMonthGenitiveNames.Select(cult.TextInfo.ToTitleCase).ToArray();
            cult.DateTimeFormat.AbbreviatedDayNames = cult.DateTimeFormat.AbbreviatedDayNames.Select(cult.TextInfo.ToTitleCase).ToArray();


            Thread.CurrentThread.CurrentCulture = cult;
            Thread.CurrentThread.CurrentUICulture = cult;
            CultureInfo.DefaultThreadCurrentCulture = cult;
            CultureInfo.DefaultThreadCurrentUICulture = cult;

            CultureInfo.CurrentCulture = cult;
            CultureInfo.CurrentUICulture = cult;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)
                )
            );

            base.OnStartup(e);
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled exception occurred: \n" + e.Exception.Message + "\n---Stack Trace---\n" + e.Exception.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
