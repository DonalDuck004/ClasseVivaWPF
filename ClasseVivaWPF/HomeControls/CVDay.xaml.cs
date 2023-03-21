using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClasseVivaWPF.Api;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Microsoft.Web.WebView2.Wpf;
using System.Reflection.Metadata;
using Microsoft.Web.WebView2.Core;
using Windows.ApplicationModel.Contacts;
using Windows.UI.WebUI;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVDay.xaml
    /// </summary>
    public partial class CVDay : UserControl
    {
        public static CVDay? SelectedDay { get; private set; } = null;
        private static DependencyProperty IsSelectedProperty;
        public DateTime Date { get; private set; }
        public new CVWeek Parent { get; private set; }
        public int ParentIdx => this.Date.DayOfWeek.AsInt32();

        private static Dictionary<DateTime, CVDay> INSTANCES = new Dictionary<DateTime, CVDay>();
        public static Dictionary<DateTime, DayOverview> READY_CONTENT = new Dictionary<DateTime, DayOverview>();
        private static List<DateTime> Retries = new List<DateTime>();

        private StackPanel? _content = null;
        public static readonly RoutedEvent ClickEvent;

        private CVDay? Tomorrow => this.ParentIdx == 6 ? null : this.Parent.GetChild(this.ParentIdx + 1);
        private CVDay? Yesterday => this.ParentIdx == 0 ? null : this.Parent.GetChild(this.ParentIdx - 1);

        private bool _destroyed = false;

        static CVDay()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVDay), new PropertyMetadata(false));
            ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CVDay));
        }

        public CVDay(DateTime date, CVWeek parent)
        {
            InitializeComponent();
            PreviewMouseLeftButtonUp += (sender, args) => RaiseClickEvent();

            this.DataContext = this;
            this.Date = date;
            this.Parent = parent;
            this.Click += OnSelected;

            INSTANCES.Add(this.Date, this);
        }

        public bool IsSelected
        {
            get => (bool)base.GetValue(IsSelectedProperty);
            set
            {
                if (value)
                {
                    var content = this.PrepareContent();
                    if (content is null)
                        return;

                    if (SelectedDay is not null)
                        SelectedDay.IsSelected = false;

                    SelectedDay = this;
                    if (this.Tomorrow is not null)
                        this.Tomorrow.Dispatcher.BeginInvoke(this.Tomorrow._PrepareContent);
                    else if(this.Yesterday is not null)
                        this.Yesterday.Dispatcher.BeginInvoke(this.Yesterday._PrepareContent);
                    CVHome.INSTANCE!.last_update_date.Content = READY_CONTENT[this.Date].XCreationDate;

                    this.Parent.View(content);
                }

                base.SetValue(IsSelectedProperty, value);
            }
        }

        public string Day => Date.ToString("ddd");
        public string NumericDay => Date.ToString("dd");

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            this.IsSelected = true;
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        private void RaiseClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(CVDay.ClickEvent);
            RaiseEvent(newEventArgs);
        }

        private void _PrepareContent()
        {
            PrepareContent();
        }

        private StackPanel? PrepareContent()
        {
            if (Retries.Contains(this.Date))
            {
                this._content = null;
                Retries.Remove(this.Date);
            }

            if (this._content is null)
            {
                if (this._destroyed)
                    return null;

                if (!READY_CONTENT.ContainsKey(this.Date))
                {
                    try
                    {
                        try
                        {
                            UpdateCachedData(Client.INSTANCE.Overview(this.Date).Result);
                        }
                        catch (AggregateException e)
                        {
                            throw e.InnerExceptions[0];
                        }

                        if (!READY_CONTENT.ContainsKey(this.Date))
                            READY_CONTENT[this.Date] = new(new(), new(), new(), new(), new());

                    }
                    catch (ApiError e)
                    {
                        e.ApplyStdProcedure();
                        Retries.Add(this.Date);
                        return null;
                    }
                }

                this._content = FillContent();
            }

            return this._content;
        }


        private static void UpdateCachedData(Overview overview)
        {
            foreach (var item in overview.GetDayOverviews())
            {
                READY_CONTENT[item.Key] = item.Value;
                if (INSTANCES.ContainsKey(item.Key))
                    INSTANCES[item.Key].Update(require_new_call: false);
            }
        }

        private StackPanel FillContent()
        {
            _content = new StackPanel();

            StackPanel sub_content;
            IEnumerable<Content> iterator;


            if (CVHome.INSTANCE.Contents!.TryGetValue(this.Date, out var ext_contents) &&
                (iterator = ext_contents.Where(x => (x.ExpireDate is null || x.ExpireDate < DateTime.Now) && x.PanoramicImg is not null && x.PanoramicaPos == Api.Types.Content.PANORAMIC_BANNER)).Any())
            {
                _content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label()
                {
                    Content = "Contenuti",
                    FontWeight = FontWeights.SemiBold
                });

                sub_content.Children.Add(sub_content = new StackPanel() { Orientation = Orientation.Horizontal });
                sub_content.HorizontalAlignment = HorizontalAlignment.Center;
                foreach (var content in iterator)
                {
                    Image tmp;

                    sub_content.Children.Add(tmp = new() { SnapsToDevicePixels = true });
                    RenderOptions.SetBitmapScalingMode(tmp, BitmapScalingMode.NearestNeighbor);
                    
                    tmp.AsyncLoading(content.PanoramicImg!, () => {
                        tmp.MaxWidth = 1000;
                        tmp.MaxHeight = 250;
                        tmp.Cursor = Cursors.Hand;
                        tmp.Tag = content;
                        tmp.MouseLeftButtonDown += OpenPage;
                    });
                    
                    // Todo Tag ti open Extr
                }
            }

            if (READY_CONTENT[this.Date].Events.Count != 0)
            {
                var contents = new Dictionary<string, StackPanel>();
                var headers = READY_CONTENT[this.Date].Events.Select(x => x.GetHeader()).Distinct();
                foreach (var item in headers)
                {
                    _content.Children.Add(contents[item] = new());
                    contents[item].Children.Add(new Label() { Content = item, FontWeight = FontWeights.SemiBold });
                }

                foreach (var evt in READY_CONTENT[this.Date].Events)
                    contents[evt.GetHeader()].Children.Add(CVHomeTextBox.FromEvent(evt));
            }

            if (READY_CONTENT[this.Date].Lessons.Count != 0)
            {
                _content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label() { Content = READY_CONTENT[this.Date].Lessons[0].GetHeader(),
                                                       FontWeight = FontWeights.SemiBold });
                foreach (var evt in READY_CONTENT[this.Date].Lessons)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromLesson(evt));
                }
            }


            if (READY_CONTENT[this.Date].Notes.Count != 0)
            {
                _content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label() { Content = READY_CONTENT[this.Date].Notes[0].GetHeader(),
                                                    FontWeight = FontWeights.SemiBold });
                foreach (var evt in READY_CONTENT[this.Date].Notes)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromAgendaEvent(evt, EventAppCategory.Agenda));
                }
            }


            if (READY_CONTENT[this.Date].Homeworks.Count != 0)
            {
                _content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label() { Content = READY_CONTENT[this.Date].Homeworks[0].GetHeader(),
                    FontWeight = FontWeights.SemiBold });
                foreach (var evt in READY_CONTENT[this.Date].Homeworks)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromAgendaEvent(evt, EventAppCategory.Homework));
                }
            }

            return _content;
        }

        private void OpenPage(object sender, MouseButtonEventArgs e)
        {
            var content = (Content)((FrameworkElement)sender).Tag;
            // new OpenUriInfo(content.OpensExternally, content.Link is null ? null : new Uri(content.Link), content.ContentID, content.Type)
            if (content.OpensExternally)
                new Uri(content.Link!).SystemOpening();
            else if (content.Type == Api.Types.Content.TYPE_POPFESSORI)
            {
                MainWindow.INSTANCE.AddFieldOverlap(new CVWebView(content.ContentID)
                {
                    Uri = new Uri(content.Link!)
                });
            }
            else if (content.Type == Api.Types.Content.TYPE_PILLOLE)
            {
                MainWindow.INSTANCE.AddFieldOverlap(new CVMemeViewer(content)); /* (uri_info.ContentID)
                {
                    Uri = uri_info.Uri
                });*/
            }

        }

        internal void BeginDestroy()
        {
            INSTANCES.Remove(this.Date);
            _destroyed = true;
            this._content = null;
            READY_CONTENT.Remove(this.Date);
        }

        internal void Update(bool require_new_call)
        {
            this._content = null;

            if (require_new_call)
                READY_CONTENT.Remove(this.Date); 
            
            if (this.IsSelected)
                this.IsSelected = true;
        }
    }
}
