using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ClasseVivaWPF.SharedControls
{
    /// <summary>
    /// Logica di interazione per CVReloadButton.xaml
    /// </summary>
    public partial class CVReloadButton : Viewbox
    {
        public Storyboard st { get; set; }
        private DependencyPropertyDescriptor desc;
        private bool DataFetched => (bool)desc.GetValue(ctx);
        private FrameworkElement ctx;

        static CVReloadButton()
        {

        }

        public CVReloadButton()
        {
            InitializeComponent();

            var tmp = new DoubleAnimation()
            {
                To = -360,
                FillBehavior = FillBehavior.Stop,
                Duration = new(TimeSpan.FromSeconds(1))
            };
            tmp.Completed += OnAnimationCompleted;
            st = new();
            st.Children.Add(tmp);
            Storyboard.SetTargetProperty(tmp, new("(Canvas.RenderTransform).(RotateTransform.Angle)"));
        }

        ~CVReloadButton(){
            desc.RemoveValueChanged(this, OnDataFetchedChanged);
        }

        private void RunAnimation()
        {
            if (!this.DataFetched)
                st.Begin(this.cv);
        }

        private void OnAnimationCompleted(object? sender, EventArgs e) => RunAnimation();
        private void OnDataFetchedChanged(object? sender, EventArgs e) => RunAnimation();

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;
            try
            {
                ctx = (FrameworkElement)this.DataContext;
                desc = DependencyPropertyDescriptor.FromName("DataFetched", ctx.GetType(), ctx.GetType());
                desc.AddValueChanged(ctx, OnDataFetchedChanged);

                RunAnimation();
            }
            catch
            {

            }
        }
    }
}
