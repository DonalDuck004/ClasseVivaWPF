using System.Windows;

namespace ClasseVivaWPF.SharedControls
{
    public class DFInjectable : Injectable
    {
        public static readonly DependencyProperty DataFetchedProperty;

        public bool DataFetched
        {
            get => (bool)GetValue(DataFetchedProperty);
            set => SetValue(DataFetchedProperty, value);
        }

        static DFInjectable()
        {
            DataFetchedProperty = DependencyProperty.Register("DataFetched", typeof(bool), typeof(DFInjectable), new PropertyMetadata(false));
        }
    }
}
