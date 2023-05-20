using ClasseVivaWPF.Utils.Converters;
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
using System.Reflection;
using ColorPicker;
using Forms = System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ClasseVivaWPF.Themes.Handling;

namespace ClasseVivaWPF.Themes.Xaml
{
    /// <summary>
    /// Logica di interazione per ThemePropertyViewer.xaml
    /// </summary>
    public partial class ThemePropertyViewer : UserControl
    {
        public static readonly DependencyProperty SelectedProperty;
        public static readonly DependencyProperty BindendColorProperty;
        public static ThemePropertyViewer? SelectedInstance = null;

        public DependencyProperty Property { get; init; }
        private SolidColorBrush SourceColor;

        public SolidColorBrush BindendColor
        {
            get => (SolidColorBrush)GetValue(BindendColorProperty);
            set => SetValue(BindendColorProperty, value); 
        }

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set {
                if (value)
                {
                    ThemeEditor.INSTANCE!.SelectedViewer = this;

                    if (SelectedInstance is not null)
                        SelectedInstance.Selected = false;

                    SelectedInstance = this;
                }

                SetValue(SelectedProperty, value);
            }
        }

        static ThemePropertyViewer()
        {
            BindendColorProperty = DependencyProperty.Register("BindendColor", typeof(SolidColorBrush), typeof(ThemePropertyViewer), new PropertyMetadata(), new ValidateValueCallback((t) => {
                if (t is null || ThemeEditor.INSTANCE is null || ThemePropertyViewer.SelectedInstance is null)
                    return true;

                if (ThemeEditor.INSTANCE!.EditedFlag is false && ThemePropertyViewer.SelectedInstance!.SourceColor != t)
                    ThemeEditor.INSTANCE!.EditedFlag = true;

                return true; 
            }));
            SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(ThemePropertyViewer), new PropertyMetadata(false));
        }

        private ThemePropertyViewer()
        {
            InitializeComponent();
        }

        public ThemePropertyViewer(DependencyProperty property)
        {
            InitializeComponent();

            this.DataContext = this;
            this.Property = property;

            var binding = new ThemeBinding()
            {
                Path = new(property),
                Mode = BindingMode.TwoWay
            };

            this.property_name.Text = ThemeProperties.GetTargetThemePath(property);

            this.SourceColor = (SolidColorBrush)ThemeProperties.INSTANCE.GetValue(property);
            BindingOperations.SetBinding(this.hex, TextBlock.TextProperty, binding);
            BindingOperations.SetBinding(this, ThemePropertyViewer.BindendColorProperty, binding);
        }
        
        private void OnCopy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(((SolidColorBrush)this.ColorViewer.Background).Color.ToString());
        }

        private void OnReset(object sender, RoutedEventArgs e)
        {
            ThemeProperties.INSTANCE.SetValue(this.Property, this.SourceColor);
        }

        private void OnSelect(object sender, MouseButtonEventArgs e)
        {
            this.Selected = true;
        }
    }
}
