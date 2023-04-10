using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Logica di interazione per CVCheckBox.xaml
    /// </summary>
    public class CheckedStateChangedEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
        public required bool NewState { get; init; }

        public bool Invalidated { get; set; } = false;
    }

    public partial class CVCheckBox : UserControl
    {
        public static DependencyProperty IsCheckedProperty;
        public static DependencyProperty CanAlterCheckProperty;

        public delegate void CheckedStateChangedEventHandler(CVCheckBox sender, CheckedStateChangedEventArgs e);
        public event CheckedStateChangedEventHandler? CheckStateChanged = null;

        public delegate void TriedChangeStateEventHandler(CVCheckBox sender, EventArgs e);
        public event TriedChangeStateEventHandler? TriedChangeState = null;

        static CVCheckBox()
        {
            IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(CVCheckBox), new PropertyMetadata(false));
            CanAlterCheckProperty = DependencyProperty.Register("CanAlterCheck", typeof(bool), typeof(CVCheckBox), new PropertyMetadata(true));
        }

        public CVCheckBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public required bool IsChecked
        {
            get => (bool)base.GetValue(IsCheckedProperty);
            set {
                if (!this.CanAlterCheck)
                {
                    this.OnTriedChangeState();
                    return;
                }

                if (!this.OnCheckStateChanged(value))
                    base.SetValue(IsCheckedProperty, value);
            }
        }

        public void Uncheck()
        {
            base.SetValue(IsCheckedProperty, false);
        }
        public void Check()
        {
            base.SetValue(IsCheckedProperty, true);
        }

        public required bool CanAlterCheck
        {
            get => (bool)base.GetValue(CanAlterCheckProperty);
            set => base.SetValue(CanAlterCheckProperty, value);
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            this.IsChecked = !this.IsChecked;
        }

        protected virtual bool OnCheckStateChanged(bool NewState)
        {
            if (this.CheckStateChanged is not null)
            {
                var e = new CheckedStateChangedEventArgs()
                {
                    NewState = NewState,
                };
                this.CheckStateChanged(this, e);
                return e.Invalidated;
            }

            return false;
        }

        protected virtual void OnTriedChangeState()
        {
            if (this.TriedChangeState is not null)
                this.TriedChangeState(this, EventArgs.Empty);
        }

        private void OnFirstLoad(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(this.Point, 64);
            this.Loaded -= OnFirstLoad;
        }
    }
}
