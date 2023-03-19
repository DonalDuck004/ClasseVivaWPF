using ClasseVivaWPF.Api;
using ClasseVivaWPF.Utils;
using Microsoft.Web.WebView2.Core;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClasseVivaWPF
{
    /// <summary>
    /// Logica di interazione per CVWebView.xaml
    /// </summary>
    public partial class CVWebView : UserControl, IOnEscKey
    {
        private static DependencyProperty UriProperty;
        private static DependencyProperty CounterProperty;
        private static DependencyProperty SavedProperty;
        private static DependencyProperty LikedProperty;

        private SemaphoreSlim sem = new SemaphoreSlim(0, 1);
        private int ID;

        static CVWebView()
        {
            UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(CVWebView));
            SavedProperty = DependencyProperty.Register("Saved", typeof(bool), typeof(CVWebView), new PropertyMetadata(false));
            LikedProperty = DependencyProperty.Register("Liked", typeof(bool), typeof(CVWebView), new PropertyMetadata(false));
            CounterProperty = DependencyProperty.Register("Counter", typeof(int), typeof(CVWebView));
        }

#if DEBUG
        private CVWebView() // For vs editor
        {
            InitializeComponent();
            this.DataContext = this;
            this.ID = 0;
        }
#endif

        public CVWebView(int ID)
        {
            InitializeComponent();
            var Options = new CoreWebView2EnvironmentOptions();
#if DEBUG
            Options.AdditionalBrowserArguments = "--proxy-server=http://localhost:8000";
#endif
            var env = CoreWebView2Environment.CreateAsync(null, null, Options).Result;
            this.WebView.EnsureCoreWebView2Async(env);

            this.DataContext = this;
            this.ID = ID;
        }

        public Uri Uri
        {
            get => (Uri)base.GetValue(UriProperty);
            set => base.SetValue(UriProperty, value);
        }

        public bool Saved
        {
            get => (bool)base.GetValue(SavedProperty);
            set => base.SetValue(SavedProperty, value);
        }

        public bool Liked
        {
            get => (bool)base.GetValue(LikedProperty);
            set => base.SetValue(LikedProperty, value);
        }

        public int Counter
        {
            get => (int)base.GetValue(CounterProperty);
            set => base.SetValue(CounterProperty, value);
        }

        private async void OnLikeBtnClick(object sender, MouseButtonEventArgs e)
        {
            await sem.WaitAsync();
            if (this.Liked)
            {
                this.Counter--;
                await Client.INSTANCE.DeleteInteraction(this.ID, Api.Types.Interaction.REACTION_LIKE);
            }
            else
            {
                this.Counter++;
                await Client.INSTANCE.SetInteraction(this.ID, Api.Types.Interaction.REACTION_LIKE);
            }

            this.Liked = !this.Liked;

            sem.Release();
        }

        private async void OnSaveBtnClick(object sender, MouseButtonEventArgs e)
        {
            await sem.WaitAsync();

            if (this.Saved)
                await Client.INSTANCE.DeleteInteraction(this.ID, Api.Types.Interaction.REACTION_BOOKMARK);
            else
                await Client.INSTANCE.SetInteraction(this.ID, Api.Types.Interaction.REACTION_BOOKMARK);

            this.Saved = !this.Saved;
            sem.Release();
        }

        private void CloseView(object sender, MouseButtonEventArgs e) => OnEscKey();

        public void OnEscKey() => Close();

        public void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            var result = await Client.INSTANCE.GetInteractions(this.ID);
            this.Liked = result.IsLiked;
            this.Saved = result.IsSaved;
            this.Counter = result.LikesTo;

            sem.Release();
        }
    }
}
