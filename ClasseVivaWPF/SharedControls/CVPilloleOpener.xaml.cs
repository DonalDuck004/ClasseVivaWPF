using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Converters;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;



namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVMemeViewer.xaml
    /// </summary>
    public partial class CVPilloleOpener : CVExtraBase, ICloseRequested, IOnKeyDown
    {
        public static readonly DependencyProperty SelectedContentProperty;
        public static readonly DependencyProperty MultiProperty;

        private (Image, string)[] Images;
      
        public bool Multi
        {
            get => (bool)base.GetValue(MultiProperty);
            set => base.SetValue(MultiProperty, value);
        }
        public Image SelectedContent
        {
            get => (Image)base.GetValue(SelectedContentProperty);
            set
            {
                base.SetValue(SelectedContentProperty, value);
                var h = Images.TakeWhile(x => !ReferenceEquals(x.Item1, value)).Sum(x => x.Item1.ActualWidth);
                var c = Scroller.HorizontalOffset;
                if (h == c)
                    return;

                const double ANIMATION_DURATION = 0.01;

                Scroller.ScrollToHorizontalOffset(h);
                if (this.Multi)
                {
                    var animation = new DoubleAnimation()
                    {
                        Duration = TimeSpan.FromSeconds(ANIMATION_DURATION * 10),
                        To = GetPointLeft(),
                    };
                    this.Pointer.BeginAnimation(Canvas.LeftProperty, animation);
                    // Scroller.AnimateScrollerH(c, h, ANIMATION_DURATION).IsEnabled = true;
                }
            }
        }

        static CVPilloleOpener()
        {
            SelectedContentProperty = DependencyProperty.Register("SelectedContent", typeof(Image), typeof(CVPilloleOpener));
            MultiProperty = DependencyProperty.Register("Multi", typeof(bool), typeof(CVPilloleOpener));
        }

#if DEBUG
        private CVPilloleOpener()
        {
            InitializeComponent();
        }
#endif

        public CVPilloleOpener(Content content) : base(content.ContentID)
        {
            InitializeComponent();

            foreach (var x in content.Related)
                this.ExtraWp.Children.Add(new CVImage(x));

            this.Multi = content.ContentDetail!.Length != 1;
            this.Images = content.ContentDetail!.Select(x => (new Image(), x.Img)).ToArray();

            StartRendering();

            this.DataContext = this;
            this.SelectedContent = Images[0].Item1;
            this.Loaded += OnLoad;
        }

        private double GetPointLeft(int? idx = null)
        {
            idx ??= this.ImagesWrapper.Children.ReferenceIndexOf(this.SelectedContent);
            if (idx == -1)
                idx = 0;

            var pos = Points.Children.OfType<Border>().ElementAt(idx.Value).TransformToAncestor(Points).Transform(new(0, 0));
            return pos.X;
        }

        private void StartRendering()
        {
            Border tmp;

            foreach ((var wp, var url) in Images)
            {
                Points.Children.Add(tmp = new Border());
                tmp.MouseLeftButtonDown += GotoPoint;
                wp.AsyncLoading(url);

                this.ImagesWrapper.Children.Add(wp);
            }

            if (this.Multi)
            {
                var bf = new Binding()
                {
                    Path = new("ActualWidth"),
                    ElementName = "Scroller",
                    Converter = new ActionConverter(),
                    ConverterParameter = () =>
                    {
                        var l = (Scroller.ActualWidth - Images[0].Item1.ActualWidth) / 2;
                        return (object)new Thickness(l, 0, 0, 0);
                    },
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                BindingOperations.SetBinding(Images[0].Item1, Image.MarginProperty, bf);

                var br = new Binding()
                {
                    Path = new("ActualWidth"),
                    ElementName = "Scroller",
                    Converter = new ActionConverter(),
                    ConverterParameter = () =>
                    {
                        var r = (Scroller.ActualWidth - Images[Images.Length - 1].Item1.ActualWidth) / 2;
                        return (object)new Thickness(0, 0, r, 0);
                    },
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                BindingOperations.SetBinding(Images[Images.Length - 1].Item1, Image.MarginProperty, br);
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            var idx = this.ImagesWrapper.Children.ReferenceIndexOf(this.SelectedContent);

            if (e.Key is Key.Left)
                idx--;
            else if (e.Key is Key.Right)
                idx++;
            else if (e.Key is Key.Up || e.Key is Key.Down)
            {
                this.ExtraScroller.RaiseEvent(e);
                return;
            }
            else
                return;

            if (idx < 0)
                idx = this.Images.Length - 1;
            else if (idx == this.Images.Length)
                idx = 0;

            this.SelectedContent = this.Images[idx].Item1;
            e.Handled = true;
        }

        private void Scroller_OnSnap(object sender, SnapEventArgs e)
        {
            this.SelectedContent = (Image)e.SnappendElement;
        }
        private void GotoPoint(object sender, EventArgs e)
        {
            var i = this.Points.Children.ReferenceIndexOf(sender);
            this.SelectedContent = Images[i].Item1;
        }
        private void OnLoad(object sender, EventArgs e)
        {
            Canvas.SetLeft(this.Pointer, GetPointLeft());

            this.Loaded -= OnLoad;
        }

        public void OnCloseRequested()
        {
            this.Content = null;
            foreach (var item in this.Images)
                item.Item1.Source = null;
            Application.Current.Dispatcher.BeginInvoke(GC.Collect);
        }
    }
}
