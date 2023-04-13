using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.HomeControls.HomeSection;
using ClasseVivaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using CVContent = ClasseVivaWPF.Api.Types.Content;

namespace ClasseVivaWPF.HomeControls.MenuSection
{
    /// <summary>
    /// Logica di interazione per CVExtraHeader.xaml
    /// </summary>
    public partial class CVExtraHeader : UserControl
    {
        public static bool SavedUpdated = false;

        public static string[] NAMES = { "In 1 Minuto", "Popfessori", "Minigame", "Salvati" };
        public static string[] CATEGORIES = { CVContent.TYPE_PILLOLE,
                                              CVContent.TYPE_POPFESSORI,
                                              CVContent.TYPE_MINIGAMES };
        private static Dictionary<string, Grid> cache = new Dictionary<string, Grid>();
        public static CVExtraHeader? SelectedH { get; private set; } = null;
        public static DependencyProperty IsSelectedProperty;
        public static DependencyProperty HeaderTextProperty;


        static CVExtraHeader()
        {
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CVExtraHeader), new PropertyMetadata(false));
            HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(CVExtraHeader));
        }

        public List<Border> GetGrid()
        {
            var category = CATEGORIES[Array.IndexOf(NAMES, this.HeaderText)];
            var images = new List<Border>();

            foreach (var contents in CVHome.INSTANCE.Contents!.Values)
            {
                foreach (var content in contents)
                {
                    if (content.Type == category)
                    {
                        var tmp = new Image();
                        RenderOptions.SetBitmapScalingMode(tmp, BitmapScalingMode.HighQuality);

                        tmp.AsyncLoading(content.Gallery!, () =>
                        {
                            tmp.Tag = content;
                            tmp.SetOpener(on_release: true);
                        }, DecodePixelHeight: 256);

                        images.Add(new Border() { Child = tmp });
                    }
                }
            }

            return images;
        }

        private Content GetContentById(int id)
        {
            foreach (var contents in CVHome.INSTANCE.Contents!.Values)
                foreach (var content in contents)
                    if (content.ContentID == id)
                        return content;

            throw new ValueUnavailableException();
        }

        public List<Border> GetGridFromBookmarks()
        {
            var images = new List<Border>();
            var contents = Client.INSTANCE.GetBookmarks().Result;
            foreach (var content in contents)
            {
                var tmp = new Image();
                RenderOptions.SetBitmapScalingMode(tmp, BitmapScalingMode.HighQuality);

                tmp.AsyncLoading(content.Img, () =>
                {
                    tmp.Tag = this.GetContentById(content.Id);
                    tmp.SetOpener(on_release: true);
                }, DecodePixelHeight: 256);

                images.Add(new Border() { Child = tmp });
            }

            return images;
        }

        public void SortGrid(List<Border> images)
        {
            const int MAX_FOR_ROW = 5;
            for (int _ = 0; _ < MAX_FOR_ROW; _++)
                cache[this.HeaderText].ColumnDefinitions.Add(new());

            int r = 0;
            foreach (var chunk in images.Chunk(MAX_FOR_ROW))
            {
                cache[this.HeaderText].RowDefinitions.Add(new());
                for (int i = 0; i < chunk.Length; i++)
                {
                    cache[this.HeaderText].Children.Add(chunk[i]);
                    Grid.SetColumn(chunk[i], i);
                    Grid.SetRow(chunk[i], r);
                }

                r++;
            }
        }

        public Grid GContent
        {
            get
            {
                List<Border>? images = null;

                if (this.HeaderText == NAMES[3])
                {
                    if (CVExtraHeader.SavedUpdated && CVExtraHeader.cache.ContainsKey(this.HeaderText))
                        cache.Remove(this.HeaderText);

                    images = this.GetGridFromBookmarks();
                    CVExtraHeader.SavedUpdated = false;
                }

                if (cache.ContainsKey(this.HeaderText))
                    return cache[this.HeaderText];

                if (images is null)
                    images = this.GetGrid();

                // TODO
                CVExtraHeader.cache[this.HeaderText] = new();
                this.SortGrid(images);

                return CVExtraHeader.cache[this.HeaderText];
            }
        }

        public bool IsSelected
        {
            get => (bool)GetValue(CVExtraHeader.IsSelectedProperty);
            set
            {

                if (value)
                {
                    if (CVExtraHeader.SelectedH is not null)
                        CVExtraHeader.SelectedH.IsSelected = false;

                    CVExtraHeader.SelectedH = this;
                    CVExtra.INSTANCE!.SetContent(this.GContent);
                }

                SetValue(CVExtraHeader.IsSelectedProperty, value);
            }
        }

        public required string HeaderText
        {
            get => (string)GetValue(CVExtraHeader.HeaderTextProperty);
            set => SetValue(CVExtraHeader.HeaderTextProperty, value);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.IsSelected)
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
            CVExtraHeader.cache.Clear();
        }

        private void OnSelected(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = true;
        }

    }
}
