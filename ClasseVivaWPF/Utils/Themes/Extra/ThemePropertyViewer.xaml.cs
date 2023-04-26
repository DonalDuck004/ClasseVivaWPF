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
using Forms = System.Windows.Forms;

namespace ClasseVivaWPF.Utils.Themes.Extra
{
    /// <summary>
    /// Logica di interazione per ThemePropertyViewer.xaml
    /// </summary>
    public partial class ThemePropertyViewer : UserControl
    {
        public static readonly DependencyProperty ColorProperty;
        public DependencyProperty Property { get; init; }

        public SolidColorBrush Color
        {
            get => (SolidColorBrush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        static ThemePropertyViewer()
        {
            ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(ThemePropertyViewer));
        }

        private ThemePropertyViewer()
        {
            InitializeComponent();
        }

        public ThemePropertyViewer(DependencyProperty property)
        {
            InitializeComponent();

            var path = ThemeProperties.GetTargetThemePath(property);
            this.DataContext = this;
            this.Property = property;
            var binding = new Binding()
            {
                Source = ThemeEditor.INSTANCE,
                Path = new($"SelectedTheme.{path}"),
                Converter = new ToBrushConverter()
            };

            BindingOperations.SetBinding(this, ThemePropertyViewer.ColorProperty, binding);


            BindingOperations.SetBinding(this.hex, TextBlock.TextProperty, binding);
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(((SolidColorBrush)this.ColorViewer.Background).Color.ToString());
        }

        private void OpenColorPicker(object sender, MouseButtonEventArgs e)
        {
            var dialog = new Forms.ColorDialog();
            dialog.ShowDialog();
            var color = System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            var path = ThemeProperties.GetTargetThemePath(this.Property);
            var field = ThemeEditor.INSTANCE!.CurrentCopy.GetType().GetProperty(path, BindingFlags.Public | BindingFlags.Instance)!;
            field.SetValue(ThemeEditor.INSTANCE!.CurrentCopy, color);
            ThemeEditor.INSTANCE!.SelectedTheme = ThemeEditor.INSTANCE.CurrentCopy;
        }
    }
}
