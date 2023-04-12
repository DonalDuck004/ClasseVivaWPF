using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace ClasseVivaWPF.SharedControls
{
    public class BaseCVPercentage : UserControl
    {
        public static readonly DependencyProperty DescProperty;
        public static readonly DependencyProperty BackgroundColorProperty;
        public static readonly DependencyProperty FontColorProperty;
        public static readonly DependencyProperty PercentageColorProperty;
        public static readonly DependencyProperty MaxProperty;
        public static readonly DependencyProperty MinProperty;
        public static readonly DependencyProperty ValueProperty;

        public virtual string? Desc
        {
            get => (string)GetValue(DescProperty);
            set => SetValue(DescProperty, value);
        }
        

        public virtual SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public virtual Color FontColor
        {
            get => (Color)GetValue(FontColorProperty);
            set => SetValue(FontColorProperty, value);
        }

        public virtual Color PercentageColor
        {
            get => (Color)GetValue(PercentageColorProperty);
            set => SetValue(PercentageColorProperty, value);
        }
        public virtual double Min
        {
            get => (double)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public virtual double Max
        {
            get => (double)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public virtual double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        static BaseCVPercentage()
        {
            FontColorProperty = DependencyProperty.Register("FontColor", typeof(Color), typeof(BaseCVPercentage), new PropertyMetadata(Colors.Black));
            DescProperty = DependencyProperty.Register("Desc", typeof(string), typeof(BaseCVPercentage));

            BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(BaseCVPercentage), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC))));

            PercentageColorProperty = DependencyProperty.Register("PercentageColor", typeof(Color), typeof(BaseCVPercentage), new PropertyMetadata(Colors.Green));

            MaxProperty = DependencyProperty.Register("Max", typeof(double), typeof(BaseCVPercentage), new PropertyMetadata(100D));
            MinProperty = DependencyProperty.Register("Min", typeof(double), typeof(BaseCVPercentage), new PropertyMetadata(0D));
            ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(BaseCVPercentage), new PropertyMetadata(0D));
        }

    }
}
