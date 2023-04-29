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
        private static int[] CustomColors = new int[0];

        public static readonly DependencyProperty BindendColorProperty;
        public DependencyProperty Property { get; init; }

        public SolidColorBrush BindendColor
        {
            get => (SolidColorBrush)GetValue(BindendColorProperty);
            set => SetValue(BindendColorProperty, value);
        }

        static ThemePropertyViewer()
        {
            BindendColorProperty = DependencyProperty.Register("BindendColor", typeof(SolidColorBrush), typeof(ThemePropertyViewer));
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
                Path = new(property)
            };

            this.property_name.Text = ThemeProperties.GetTargetThemePath(property);

            BindingOperations.SetBinding(this.hex, TextBlock.TextProperty, binding);
            BindingOperations.SetBinding(this, ThemePropertyViewer.BindendColorProperty, binding);
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(((SolidColorBrush)this.ColorViewer.Background).Color.ToString());
        }

        private void OpenColorPicker(object sender, MouseButtonEventArgs e)
        {
            var dialog = new Forms.ColorDialog()
            {
                FullOpen = true,
                CustomColors = CustomColors
            };
            var result = dialog.ShowDialog();
            CustomColors = dialog.CustomColors;
            if (result is Forms.DialogResult.Cancel)
                return;

            var color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            ThemeProperties.INSTANCE.SetValue(this.Property, new SolidColorBrush(color));
            ThemeEditor.INSTANCE!.EditedFlag = true;
        }
    }
}
