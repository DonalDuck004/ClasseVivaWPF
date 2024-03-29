﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ClasseVivaWPF.Api.Types;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Grades
{
    /// <summary>
    /// Logica di interazione per CVGrade.xaml
    /// </summary>
    public partial class CVGrade : CVGradeBase
    {
        // For vs
        private CVGrade() : base()
        {
            InitializeComponent();
        }

        public CVGrade(Grade grade) : base(grade)
        {
            InitializeComponent();
            Debug.Assert(grade.EvtCode != Grade.GRADE_GRADE_UNKNOW2);

            if (grade.OldskillDesc == "")
            {
                this.Grid.Children.Remove(this.OldSkill);
                this.Grid.RowDefinitions.RemoveAt(3);
                Grid.SetRow(Notes, 2);
            }
        }
    }
}
