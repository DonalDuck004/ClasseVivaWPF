using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
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

namespace ClasseVivaWPF.HomeControls.RegistrySection
{
    /// <summary>
    /// Logica di interazione per CVGradeEllipse.xaml
    /// </summary>
    public partial class CVGradeEllipse : UserControl
    {
        public static readonly DependencyProperty GradeProperty;
        public static readonly DependencyProperty BackgroundEllipseColorProperty;

        public Grade Grade
        {
            get => (Grade)GetValue(GradeProperty);
            init => SetValue(GradeProperty, value);
        }

        public Color BackgroundEllipseColor
        {
            get => (Color)GetValue(BackgroundEllipseColorProperty);
            init => SetValue(BackgroundEllipseColorProperty, value);
        }

        static CVGradeEllipse()
        {
            GradeProperty = DependencyProperty.Register("Grade", typeof(Grade), typeof(CVGradeEllipse));
            BackgroundEllipseColorProperty = DependencyProperty.Register("BackgroundEllipseColor", typeof(Color), typeof(CVGradeEllipse), new PropertyMetadata(Colors.Transparent));
        }

        public string ToolTipText
        {
            get
            {
                var text = $"Data: {this.Grade.EvtDate:d}\n";
                text += $"Materia: {this.Grade.SubjectDesc.ToTitle()}\n";

                if (this.Grade.DecimalValue is not null)
                    text += $"Valore in decimali: {this.Grade.DecimalValue:0.00}\n";
                
                if (!string.IsNullOrEmpty(this.Grade.NotesForFamily))
                    text += $"Note: {this.Grade.NotesForFamily}\n";
                
                return text.Substring(0, text.Length - 1);
            }
        }

        public CVGradeEllipse()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
