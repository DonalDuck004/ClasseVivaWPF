using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClasseVivaWPF.HomeControls.HomeSection
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

        private CVDay? Tomorrow => this.ParentIdx == 6 ? null : this.Parent.GetChild(this.ParentIdx + 1);
        private CVDay? Yesterday => this.ParentIdx == 0 ? null : this.Parent.GetChild(this.ParentIdx - 1);

        private SemaphoreSlim loader = new SemaphoreSlim(1, 1);
        private bool _destroyed = false;

        static CVDay()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVDay), new PropertyMetadata(false));
        }

#if DEBUG
        private CVDay()
        {

        }
#endif
        public CVDay(DateTime date, CVWeek parent)
        {
            InitializeComponent();

            this.DataContext = this;
            this.Date = date;
            this.Parent = parent;
            this.PreviewMouseLeftButtonUp += OnSelected;

            INSTANCES.Add(this.Date, this);
        }

        public bool IsSelected
        {
            get => (bool)base.GetValue(IsSelectedProperty);
            set
            {
                if (value)
                {
                    var content = this.PrepareContent().ConfigureAwait(false).GetAwaiter().GetResult();
                    if (content is null)
                        return;

                    if (SelectedDay is not null)
                        SelectedDay.IsSelected = false;

                    SelectedDay = this;
                    if (this.Tomorrow is not null)
                        this.Tomorrow.Dispatcher.BeginInvoke(this.Tomorrow._PrepareContent);
                    if (this.Yesterday is not null)
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

        internal async void _PrepareContent()
        {
            await PrepareContent();
        }

        private async Task<StackPanel?> PrepareContent()
        {
            await loader.WaitAsync();
            try
            {
                if (Retries.Contains(this.Date))
                {
                    this._content = null;
                    Retries.Remove(this.Date);
                }

                if (this._content is null || !READY_CONTENT.ContainsKey(this.Date))
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
                                READY_CONTENT[this.Date] = new();

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
            finally
            {
                loader.Release();
            }
        }


        private static void UpdateCachedData(Overview overview)
        {
            foreach (var item in overview.GetDayOverviews())
            {

                if (item.Value is null)
                    INSTANCES[item.Key].Update(require_new_call: false);

                if (INSTANCES.ContainsKey(item.Key) &&
                    READY_CONTENT.ContainsKey(item.Key) &&
                    item.Value is not null &&
                    READY_CONTENT[item.Key].XCreationDate - item.Value.XCreationDate > TimeSpan.FromMinutes(5))
                    INSTANCES[item.Key].Update(require_new_call: false);

                if (item.Value is not null)
                    READY_CONTENT[item.Key] = item.Value;
            }
        }

        private StackPanel FillContent()
        {
            this._content = new StackPanel();

            StackPanel sub_content;
            IEnumerable<Content> iterator;

            if (CVHome.INSTANCE.Contents!.TryGetValue(this.Date, out var ext_contents) &&
                (iterator = ext_contents.Where(x =>
                    (x.ExpireDate is null || x.ExpireDate < DateTime.Now) && x.PanoramicImg is not null && x.PanoramicaPos == Api.Types.Content.PANORAMIC_BANNER)).Any())
            {
                this._content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label()
                {
                    Content = "Contenuti",
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                });

                sub_content.Children.Add(sub_content = new StackPanel() { Orientation = Orientation.Horizontal });
                sub_content.HorizontalAlignment = HorizontalAlignment.Center;
                var style = (Style)Application.Current.FindResource("BDImg");

                foreach (var content in iterator)
                {
                    Image tmp = new() { SnapsToDevicePixels = true };

                    sub_content.Children.Add(new Border()
                    {
                        Child = tmp,
                        Style = style
                    });

                    tmp.AsyncLoading(content.PanoramicImg!, () =>
                    {
                        tmp.Height = 200;
                        tmp.Cursor = Cursors.Hand;
                        tmp.Tag = content;
                        tmp.SetOpener();
                    }, DecodePixelHeight: 200);

                    // Todo Tag ti open Extr
                }
            }


            if (READY_CONTENT[this.Date].Events.Count != 0)
            {
                var contents = new Dictionary<string, StackPanel>();
                var headers = READY_CONTENT[this.Date].Events.Select(x => x.GetHeader()).Distinct();
                foreach (var item in headers)
                {
                    this._content.Children.Add(contents[item] = new());
                    contents[item].Children.Add(new Label()
                    {
                        Content = item,
                        FontSize = 14,
                        FontWeight = FontWeights.Bold
                    });
                }

                foreach (var evt in READY_CONTENT[this.Date].Events)
                    contents[evt.GetHeader()].Children.Add(CVHomeTextBox.FromEvent(evt));
            }

            if (READY_CONTENT[this.Date].Grades.Count != 0)
            {
                this._content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label()
                {
                    Content = READY_CONTENT[this.Date].Grades[0].GetHeader(),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                });
                foreach (var evt in READY_CONTENT[this.Date].Grades)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromGrade(evt));
                }
            }

            if (READY_CONTENT[this.Date].Lessons.Count != 0)
            {
                this._content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label()
                {
                    Content = READY_CONTENT[this.Date].Lessons[0].GetHeader(),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                });
                foreach (var evt in READY_CONTENT[this.Date].Lessons)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromLesson(evt));
                }
            }


            if (READY_CONTENT[this.Date].Notes.Count != 0)
            {
                this._content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label()
                {
                    Content = READY_CONTENT[this.Date].Notes[0].GetHeader(),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                });
                foreach (var evt in READY_CONTENT[this.Date].Notes)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromAgendaEvent(evt, EventAppCategory.Agenda));
                }
            }


            if (READY_CONTENT[this.Date].Homeworks.Count != 0)
            {
                this._content.Children.Add(sub_content = new StackPanel());
                sub_content.Children.Add(new Label()
                {
                    Content = READY_CONTENT[this.Date].Homeworks[0].GetHeader(),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                });
                foreach (var evt in READY_CONTENT[this.Date].Homeworks)
                {
                    sub_content.Children.Add(CVHomeTextBox.FromAgendaEvent(evt, EventAppCategory.Homework));
                }
            }

            return this._content;
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

        internal static void GlobDispose()
        {
            INSTANCES.Clear();
            SelectedDay = null;
        }

        public static void DestroyCache()
        {
            READY_CONTENT.Clear();
        }
    }
}
