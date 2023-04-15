﻿using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Grades
{
    public class CVGradeBase : UserControl
    {
        public static readonly DependencyProperty GradeProperty;
        public static readonly DependencyProperty EllipseColorProperty;
        public Grade Grade
        {
            get => (Grade)GetValue(GradeProperty);
            init
            {
                SetValue(GradeProperty, value);
                this.SetThemeBinding(CVGradeEllipse.EllipseColorProperty, value.InternalColorPath);
            }
        }

        public SolidColorBrush EllipseColor
        {
            get => (SolidColorBrush)GetValue(EllipseColorProperty);
            init => SetValue(EllipseColorProperty, value);
        }


        static CVGradeBase()
        {
            GradeProperty = DependencyProperty.Register("Grade", typeof(Grade), typeof(CVGradeBase));
            EllipseColorProperty = DependencyProperty.Register("EllipseColor", typeof(SolidColorBrush), typeof(CVGradeBase));
        }

        internal CVGradeBase()
        {

        }

        public CVGradeBase(Grade grade)
        {
            this.Grade = grade;
            this.DataContext = this;
        }
    }
}