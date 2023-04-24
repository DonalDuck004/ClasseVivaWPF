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

namespace ClasseVivaWPF.HomeControls.RegistrySection.Absences
{
    /// <summary>
    /// Logica di interazione per CVAbsenceCategory.xaml
    /// </summary>
    public partial class CVAbsenceCategory : UserControl
    {
        public static readonly DependencyProperty HeaderNameProperty;
        public string HeaderName
        {
            get => (string)GetValue(HeaderNameProperty);
            set => SetValue(HeaderNameProperty, value);
        }

        static CVAbsenceCategory()
        {
            HeaderNameProperty = DependencyProperty.Register("HeaderName", typeof(string), typeof(CVAbsenceCategory));
        }

        public CVAbsenceCategory()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void AddChild(CVAbsence abs)
        {
            this.ContentWrapper.Children.Add(abs);
            this.Counter.Text = this.ContentWrapper.Children.Count.ToString();
        }

        public void ClearAll()
        {
            this.ContentWrapper.Children.Clear();
            this.Counter.Text = "0";
        }
    }
}
