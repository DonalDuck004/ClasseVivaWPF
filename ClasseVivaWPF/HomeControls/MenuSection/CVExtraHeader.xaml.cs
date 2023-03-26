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

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVExtraHeader.xaml
    /// </summary>
    public partial class CVExtraHeader : UserControl
    {
        public static string[] NAMES = { "In 1 Minuto", "Popfessori", "Minigame", "Salvati" };

        private Dictionary<string, StackPanel> CachedContents = new Dictionary<string, StackPanel>();

        public static CVExtraHeader? SelectedH { get; private set; } = null;
        
        public static DependencyProperty SelectedProperty;
        public static DependencyProperty HeaderTextProperty;


        static CVExtraHeader()
        {
            SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(CVExtraHeader), new PropertyMetadata(false));
            HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(CVExtraHeader));
        }

        public StackPanel ContentPanel
        {
            get
            {
                if (CachedContents.ContainsKey(this.HeaderText))
                    return CachedContents[this.HeaderText];

                return CachedContents[this.HeaderText] = GetContent();
            }
        }

        private StackPanel GetContent()
        {
            // TODO
            return new();
        }

        public bool Selected
        {
            get => (bool)GetValue(CVExtraHeader.SelectedProperty);
            set {
                
                if (value)
                {
                    if (CVExtraHeader.SelectedH is not null)
                        CVExtraHeader.SelectedH.Selected = false;

                    CVExtraHeader.SelectedH = this;
                }


                SetValue(CVExtraHeader.SelectedProperty, value);
            }
        }

        public required string HeaderText
        {
            get => (string)GetValue(CVExtraHeader.HeaderTextProperty);
            set => SetValue(CVExtraHeader.HeaderTextProperty, value);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.Selected)
                CVExtraHeader.SelectedH = this;

            this.Loaded -= OnLoad;
        }

        public CVExtraHeader()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += OnLoad;
        }

        public static void DestroyAll()
        {
            CVExtraHeader.SelectedH = null;
        }

        private void OnSelected(object sender, MouseButtonEventArgs e)
        {
            this.Selected = true;
        }
    }
}
