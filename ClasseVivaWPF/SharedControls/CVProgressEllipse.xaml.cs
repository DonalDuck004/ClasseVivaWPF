﻿using System;
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
    /// Logica di interazione per CVEllipseAVG.xaml
    /// </summary>
    public partial class CVProgressEllipse : BaseCVPercentage
    {

        public static readonly DependencyProperty CenterColorProperty;

        public SolidColorBrush CenterColor
        {
            get => (SolidColorBrush)GetValue(CenterColorProperty);
            set => SetValue(CenterColorProperty, value);
        }

        public override string? Desc
        {
            get => (string)base.GetValue(DescProperty);
            set{
                base.SetValue(DescProperty, value);
                var x = (TextBlock)this.Text;

                if (value is null)
                {
                    if (x.Inlines.Count != 1) {
                        x.Inlines.Remove(x.Inlines.Last());
                        x.Inlines.Remove(x.Inlines.Last());
                    }
                }
                else
                {
                    x.Inlines.Add(new LineBreak());
                    x.Inlines.Add(new Run(text: value) { FontSize = 24 });
                }
            }
        }

        static CVProgressEllipse()
        {
            CenterColorProperty = DependencyProperty.Register("CenterColor", typeof(SolidColorBrush), typeof(CVProgressEllipse), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        }

        public CVProgressEllipse()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoad;

            if (this.Desc is not null)
                this.Desc = this.Desc; // Directly set by xaml
        }
    }
}
