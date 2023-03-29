using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVImage.xaml
    /// </summary>
    public partial class CVImage : UserControl
    {
#if DEBUG
        private CVImage()
        {
            InitializeComponent();
        }
#endif
        public static readonly RoutedEvent ClickEvent;

        static CVImage()
        {
            ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CVImage));
        }


        private RelatedContentDetail content;
        private SemaphoreSlim sem = new SemaphoreSlim(1, 1);

        public CVImage(RelatedContentDetail content)
        {
            InitializeComponent();

            PreviewMouseLeftButtonUp += (sender, args) => RaiseClickEvent();

            this.content = content;
            this.wp.AsyncImageLoading(content.Img);

            this.Click += ShowMeme;
        }

        private async void ShowMeme(object sender, RoutedEventArgs e)
        {
            await sem.WaitAsync();

            int idx;
            Content? target_content = null;
            foreach (var item in CVHome.INSTANCE.Contents!.Values)
            {
                for (idx = 0; idx < item.Count; idx++)
                {
                    if (item[idx].ContentID == content.Id && item[idx].Type == Api.Types.Content.TYPE_PILLOLE)
                    {
                        target_content = item[idx];
                        break;
                    }
                }
            }
            Debug.Assert(target_content is not null);

            new CVPilloleOpener(target_content).Inject();

            sem.Release();
        }

        private void RaiseClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(CVImage.ClickEvent);
            RaiseEvent(newEventArgs);
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

    }
}
