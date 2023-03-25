using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Converters;
using System;
using System.Linq;
using System.Threading.Tasks;
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
    public partial class CVMemeViewer : CVExtraBase
    {
#if DEBUG
        private CVMemeViewer()
        {
            InitializeComponent();
        }
#endif
        public static readonly DependencyProperty SelectedContentProperty;
        public static readonly DependencyProperty MultiProperty;

        static CVMemeViewer()
        {
            SelectedContentProperty = DependencyProperty.Register("SelectedContent", typeof(Image), typeof(CVMemeViewer));
            MultiProperty = DependencyProperty.Register("Multi", typeof(bool), typeof(CVMemeViewer));
        }
        
        private (Image, string)[] Images;

        private void StartRendering()
        {
            foreach ((var wp, var url) in Images)
            {
                Points.Children.Add(new Border());
                wp.AsyncLoading(url);

                this.ImagesWrapper.Children.Add(wp);
            }

            if (Images.Length != 1)
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

        public CVMemeViewer(Content content) : base(content.ContentID)
        {
            new Task(async () => await Client.INSTANCE.SetInteraction(content.ContentID, Interaction.REACTION_CLICK));
           
            InitializeComponent();

            foreach (var x in content.Related)
                this.ExtraWp.Children.Add(new CVImage(x));

            if (this.Multi = (content.ContentDetail!.Length != 1))
                Images = content.ContentDetail!.Select(x => (new Image(), x.Img)).ToArray();
            else
                Images = new[] { (new Image(), content.Gallery!) };

            // Todo Gestire .Detail 13 feb.
            StartRendering();

            this.DataContext = this;
            this.SelectedContent = Images[0].Item1;
            this.Loaded += (s, e) => Canvas.SetLeft(this.Pointer, GetPointLeft());
        }

        public bool Multi
        {
            get => (bool)base.GetValue(MultiProperty);
            set => base.SetValue(MultiProperty, value);
        }

        private double GetPointLeft(int? idx = null)
        {
            idx ??= GetImageIndex();
            var pos = Points.Children.OfType<Border>().ElementAt(idx.Value).TransformToAncestor(Points).Transform(new(0, 0));
            return pos.X;
        }

        private DispatcherTimer? last_animator = null;

        public Image SelectedContent
        {
            get => (Image)base.GetValue(SelectedContentProperty);
            set {
                base.SetValue(SelectedContentProperty, value);
                
                if (last_animator is not null)
                    last_animator.IsEnabled = false;

                var h = Images.TakeWhile(x => !ReferenceEquals(x.Item1, value)).Sum(x => x.Item1.ActualWidth);
                var c = Scroller.HorizontalOffset;
                if (h == c)
                    return;

                const double ANIMATION_DURATION = 0.01;

                
                if (this.Multi)
                {
                    var animation = new DoubleAnimation()
                    {
                        Duration = TimeSpan.FromSeconds(ANIMATION_DURATION * 10),
                        To = GetPointLeft(),
                    };
                    this.Pointer.BeginAnimation(Canvas.LeftProperty, animation);
                }

                Scroller.AnimateScrollerH(c, h, ANIMATION_DURATION).IsEnabled = true;
            }
        }

        private void GotoSubImage(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled) 
                return;

            this.SelectedContent = (Image)sender;   
        }

        private int GetImageIndex()
        {
            int i = 0;
            for (; !ReferenceEquals(this.ImagesWrapper.Children[i], this.SelectedContent); i++) ;

            return i;
        }

        private double? scroll_horizontal_offset = null;

        private void OnSnapScroller(object sender, MouseButtonEventArgs e)
        {
            if (scroll_horizontal_offset is null)
                return;

            var required = this.Scroller.ActualWidth / 20;
            int idx = GetImageIndex();

            if (this.Scroller.HorizontalOffset - scroll_horizontal_offset > required) // Next
                idx++;
            else if (scroll_horizontal_offset - this.Scroller.HorizontalOffset > required)// Undo
                idx--;

            if (idx == this.ImagesWrapper.Children.Count)
                idx = this.ImagesWrapper.Children.Count - 1;
            else if (idx == -1)
                idx = 0;

            this.SelectedContent = (Image)this.ImagesWrapper.Children[idx];
            scroll_horizontal_offset = null;
        }
        private void OnSetScrollerOffest(object sender, MouseButtonEventArgs e)
        {
            scroll_horizontal_offset = this.Scroller.HorizontalOffset;
        }
    }
}
