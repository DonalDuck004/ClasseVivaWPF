﻿using ClasseVivaWPF.Api.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    public class CVBaseMedia : UserControl
    {
        public static readonly DependencyProperty MediaProperty;

        public virtual string EffectiveText => Media.ContentName.ToString();

        public FolderContent Media
        {
            get => (FolderContent)GetValue(MediaProperty);
            set => SetValue(MediaProperty, value);
        }

        static CVBaseMedia()
        {
            MediaProperty = DependencyProperty.Register("Media", typeof(FolderContent), typeof(CVBaseMedia));
        }
    }
}
