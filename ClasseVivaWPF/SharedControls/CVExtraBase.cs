using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClasseVivaWPF.SharedControls
{
    public class CVExtraBase : UserControl
    {
        protected static DependencyProperty CounterProperty;
        protected static DependencyProperty SavedProperty;
        protected static DependencyProperty LikedProperty;
        protected static DependencyProperty DataFetchedProperty;

        protected SemaphoreSlim PreventOverlap { get; } = new SemaphoreSlim(0, 1);

        protected int ID { get; init; }

        protected bool CanPress => this.DataFetched && PreventOverlap.CurrentCount == 1;

        static CVExtraBase()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(CVExtraBase), new PropertyMetadata(false));
            SavedProperty = DependencyProperty.Register("Saved", typeof(bool), typeof(CVExtraBase), new PropertyMetadata(false));
            LikedProperty = DependencyProperty.Register("Liked", typeof(bool), typeof(CVExtraBase), new PropertyMetadata(false));
            CounterProperty = DependencyProperty.Register("Counter", typeof(int), typeof(CVExtraBase), new PropertyMetadata(0));
        }

#if DEBUG
        protected CVExtraBase()
        {

        }
#endif

        public CVExtraBase(int ID)
        {
            this.ID = ID;
            new Task(async () => await Client.INSTANCE.SetInteraction(ID, Interaction.REACTION_CLICK)).Start();

            this.Loaded += OnLoad;

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
        public bool DataFetched
        {
            get => (bool)base.GetValue(DataFetchedProperty);
            set => base.SetValue(DataFetchedProperty, value);
        }

        public int Counter
        {
            get => (int)base.GetValue(CounterProperty);
            set => base.SetValue(CounterProperty, value);
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Client.INSTANCE.GetInteractions(this.ID);
                
                this.Liked = result.IsLiked;
                this.Saved = result.IsSaved;
                this.Counter = result.LikesTo;
                this.DataFetched = true;
            }
            catch (ApiError exc)
            {
                this.Close();
                exc.ApplyStdProcedure();
                return;
            }

            PreventOverlap.Release();
        }

        protected async void OnLikeBtnClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;

            await PreventOverlap.WaitAsync();

            try
            {
                if (this.Liked)
                {
                    await Client.INSTANCE.DeleteInteraction(this.ID, Interaction.REACTION_LIKE);
                    this.Counter--;
                }
                else
                {
                    await Client.INSTANCE.SetInteraction(this.ID, Interaction.REACTION_LIKE);
                    this.Counter++;
                }
                this.Liked = !this.Liked;
            }catch (ApiError exc)
            {
                exc.ApplyStdProcedure();
            }


            PreventOverlap.Release();
        }

        protected async void OnSaveBtnClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanPress)
                return;
            
            await PreventOverlap.WaitAsync();

            try
            {
                if (this.Saved)
                    await Client.INSTANCE.DeleteInteraction(this.ID, Interaction.REACTION_BOOKMARK);
                else
                    await Client.INSTANCE.SetInteraction(this.ID, Interaction.REACTION_BOOKMARK);
            this.Saved = !this.Saved;
            }
            catch (ApiError exc)
            {
                exc.ApplyStdProcedure();
            }
          
            PreventOverlap.Release();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public virtual void Close()
        {
            MainWindow.INSTANCE.RemoveField(this);
        }

        public void Inject()
        {
            MainWindow.INSTANCE.AddFieldOverlap(this);
        }
    }
}
