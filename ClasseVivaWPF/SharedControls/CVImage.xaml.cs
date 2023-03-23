using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.HomeControls.HomeSection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

            new CVMemeViewer(target_content).Inject();

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
