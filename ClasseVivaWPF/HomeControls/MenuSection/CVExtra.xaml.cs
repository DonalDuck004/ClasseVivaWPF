using ClasseVivaWPF.Utils;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using ClasseVivaWPF.SharedControls;

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVExtra.xaml
    /// </summary>
    public partial class CVExtra : Injectable, IOnKeyDown, ICloseRequested, IOnChildClosed, ICVMeta
    {
        public bool CountsInStack { get; } = false;
        public static CVExtra? INSTANCE = null;

        public CVExtra() : base()
        {
            CVExtra.INSTANCE = this;
            InitializeComponent();
        }

        public void Destroy()
        {
            CVExtraHeader.DestroyAll();
        }

        public void OnCloseRequested()
        {
            this.Destroy();
        }

        protected void OnClose(object sender, MouseButtonEventArgs e) => Close();

        public void SetContent(Grid grid)
        {
            this.content_wp.Content = grid;
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is not Key.Left && e.Key is not Key.Right)
                return;

            var pool = this.h_grid.Children.OfType<CVExtraHeader>();

            if (e.Key is not Key.Left)
            {
                try
                {
                    pool.SkipWhile(x => !x.IsSelected).Skip(1).First().IsSelected = true;
                }
                catch (InvalidOperationException)
                {
                    pool.First().IsSelected = true;
                }
                return;
            }
            try
            {
                pool.TakeWhile(x => !x.IsSelected).Last().IsSelected = true;
            } catch (InvalidOperationException)
            {
                pool.Last().IsSelected = true;
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.SetContent(((CVExtraHeader)h_grid.Children[0]).GContent);

            this.Loaded -= OnLoad;
        }

        public void OnChildClosed()
        {
            if (CVExtraHeader.SavedUpdated && CVExtraHeader.SelectedH!.HeaderText == CVExtraHeader.NAMES[CVExtraHeader.NAMES.Length - 1])
            {
                CVExtraHeader.SelectedH!.IsSelected = true;
            }
        }
    }
}
