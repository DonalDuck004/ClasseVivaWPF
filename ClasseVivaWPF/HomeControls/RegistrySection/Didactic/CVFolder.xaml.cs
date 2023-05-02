using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.RegistrySection.Grades;
using ClasseVivaWPF.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    /// <summary>
    /// Logica di interazione per CVFolder.xaml
    /// </summary>
    public partial class CVFolder : UserControl
    {
        public static readonly DependencyProperty IsExpandedProperty;
        public static readonly DependencyProperty FolderProperty;
        public static readonly DependencyProperty DirTypeProperty;
        public static readonly DependencyProperty TeacherProperty;

        public delegate void IsExpandedHandler(CVFolder sender);
        public event IsExpandedHandler IsExpandedChanged;

        private Point? DownPos = null;

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set
            {
                this.Expand(value); // Order is important
                // SetValue(IsExpandedProperty, value);
            }
        }

        public DirType DirType
        {
            get => (DirType)GetValue(DirTypeProperty);
            set => SetValue(DirTypeProperty, value);
        }

        public Folder? Folder
        {
            get => (Folder?)GetValue(FolderProperty);
            set => SetValue(FolderProperty, value);
        }

        public TeacherDidactic? Teacher
        {
            get => (TeacherDidactic?)GetValue(TeacherProperty);
            set => SetValue(TeacherProperty, value);
        }

        public IEnumerable<CVFolder> SubFolders => this.ItemsStack.Children.OfType<CVFolder>();
        public IEnumerable<CVBaseMedia> Files => this.ItemsStack.Children.OfType<CVBaseMedia>();

        public string EffectiveText => this.DirType is DirType.Teacher ? this.Teacher.TeacherName : this.Folder.FolderName;

        static CVFolder()
        {
            IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(CVFolder), new PropertyMetadata(false));
            DirTypeProperty = DependencyProperty.Register("DirType", typeof(DirType), typeof(CVFolder));
            FolderProperty = DependencyProperty.Register("Folder", typeof(Folder), typeof(CVFolder));
            TeacherProperty = DependencyProperty.Register("Teacher", typeof(TeacherDidactic), typeof(CVFolder));
        }

        public CVFolder()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void AddFolder(CVFolder folder)
        {
            this.ItemsStack.Children.Add(folder);
        }

        public void AddMedia(CVBaseMedia media)
        {
            this.ItemsStack.Children.Add(media);
        }

        private void RaiseIsExpandedChanged()
        {
            if (IsExpandedChanged is not null)
                IsExpandedChanged(this);
        }

        private void Expand(bool expand)
        {
            var parent = this.FindAncestor<CVFolder>();

            if (parent is not null && expand && parent.IsExpanded is false)
                parent.Expand(expand);

            Storyboard ExpandCTAnimation = new();
            Storyboard ExpandHAnimation = new();
            DoubleAnimation animationCT;
            DoubleAnimation animationH;
            var duration = new Duration(TimeSpan.FromSeconds(0.1));

            if (expand)
            {
                animationH = new()
                {
                    Duration = duration,
                    To = this.ItemsStack.ActualHeight,
                    FillBehavior = FillBehavior.Stop
                };

                animationCT = new()
                {
                    From = -this.ItemsStack.ActualHeight,
                    Duration = duration,
                    To = 0,
                    FillBehavior = FillBehavior.Stop
                };

            }
            else
            {
                animationH = new()
                {
                    Duration = duration,
                    To = 0,
                    FillBehavior = FillBehavior.Stop
                };

                animationCT = new()
                {
                    To = -this.ItemsStack.ActualHeight + 5,
                    Duration = duration,
                    From = 0,
                    FillBehavior = FillBehavior.Stop
                };
            }

            Storyboard.SetTargetProperty(animationH, new(Canvas.HeightProperty));
            Storyboard.SetTargetProperty(animationCT, new(Canvas.TopProperty));

            ExpandHAnimation.Children.Add(animationH);
            ExpandCTAnimation.Children.Add(animationCT);

            ExpandCTAnimation.Completed += (s, e) =>
            {
                if (expand)
                {
                    var binding = new Binding()
                    {
                        Source = ItemsStack,
                        Path = new(StackPanel.ActualHeightProperty)
                    };
                    BindingOperations.SetBinding(ItemsStackWP, Canvas.HeightProperty, binding);
                }
                else
                {
                    BindingOperations.ClearBinding(ItemsStackWP, Canvas.HeightProperty);
                    ItemsStackWP.Height = 0;
                }

                SetValue(IsExpandedProperty, expand);
                RaiseIsExpandedChanged();
            };

            ExpandHAnimation.Begin(this.ItemsStackWP);
            ExpandCTAnimation.Begin(this.ItemsStack);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DownPos == e.GetPosition(MainWindow.INSTANCE))
                this.IsExpanded = !this.IsExpanded;

            DownPos = null;
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DownPos = e.GetPosition(MainWindow.INSTANCE);
        }

    }
}
