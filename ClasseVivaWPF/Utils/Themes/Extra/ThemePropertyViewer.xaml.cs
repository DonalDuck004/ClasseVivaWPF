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

namespace ClasseVivaWPF.Utils.Themes.Extra
{
    /// <summary>
    /// Logica di interazione per ThemePropertyViewer.xaml
    /// </summary>
    public partial class ThemePropertyViewer : UserControl
    {
        private ThemePropertyViewer()
        {
            InitializeComponent();
        }

        public ThemePropertyViewer(DependencyProperty property)
        {
            InitializeComponent();
        }
    }
}
