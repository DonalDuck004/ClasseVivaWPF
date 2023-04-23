using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Themes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Diagnostics;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Graphs
{

    /// <summary>
    /// Logica di interazione per CVColumnsGraph.xaml
    /// </summary>
    /// 
    public partial class CVColumnsGraph : UserControl
    {
        private Dictionary<object, CVColumn[]> CurrentValues = new();

        public delegate void ColumnAddedHandler(CVColumn column);
        public event ColumnAddedHandler? ColumnAdded;

        public CVColumnsGraph()
        {
            InitializeComponent();
        }

        private static Func<IGrouping<object, CVColumn>, CVColumn[]> Orderer = x => x.OrderByDescending(y => y.Value).ToArray();

        public void Update(IEnumerable<CVColumn> columns,
                           bool persist_empty_fields = true,
                           bool persist_zero_fields = true,
                           Func<IGrouping<object, CVColumn>, CVColumn[]>? order_func = null,
                           CVColumnsGraphFilterOperation op = CVColumnsGraphFilterOperation.NoneOverlap)
        {
            this.CurrentValues = (from column in columns 
                                  group column by column.ContentID).ToDictionary(
                                    x => x.Key,
                                    order_func ?? Orderer);

            this.Filter(op: op, persist_empty_fields: persist_empty_fields, persist_zero_fields: persist_zero_fields);
        }

        public void Update(IEnumerable<string> column_names)
        {
            TextBlock header;
            var c = 0;
            this.Clear();

            foreach (var column in column_names)
            {
                this.Grid.ColumnDefinitions.Add(new());

                header = new() { Text = column };
                header.SetThemeBinding(TextBlock.ForegroundProperty, ThemeProperties.CVGenericFontProperty);
                this.Grid.Children.Add(header);
                Grid.SetRow(header, 1);
                Grid.SetColumn(header, c++);
            }
        }

        private bool AddColumn(CVColumn column, int idx, bool persist_empty_fields, bool persist_zero_fields)
        {
            if (!persist_empty_fields && column.Value is double.NaN)
                return false;

            if (!persist_zero_fields && column.Value == 0)
                return false;

            this.Grid.Children.Add(column);
            Grid.SetColumn(column, idx);

            if (this.ColumnAdded is not null)
                this.ColumnAdded(column);

            return true;
        }

        public void Filter(CVColumnsGraphFilterOperation op = CVColumnsGraphFilterOperation.NoneOverlap,
                           bool persist_empty_fields = true,
                           bool persist_zero_fields = true,
                           params string[] group_names)
        {
            TextBlock? header = null;
            var col_idx = 0;
            bool tmp;
            int ac;
            CVColumn? column = null;
            this.Clear();

            foreach (var column_group in this.CurrentValues)
            {
                ac = 0;

                if (op is CVColumnsGraphFilterOperation.NoneOverlap)
                {
                    for (int i = 0; i < column_group.Value.Length; i++)
                    {
                        column = column_group.Value[i];

                        if (group_names.Contains(column.SubGroupName))
                            continue;

                        if (tmp = this.AddColumn(column, col_idx, persist_empty_fields, persist_zero_fields))
                            ac++;
                    }
                } 
                else if (op is CVColumnsGraphFilterOperation.NoneNoOverlap)
                {
                    for (int i = 0; i < column_group.Value.Length; i++)
                    {
                        column = column_group.Value[i];

                        if (group_names.Contains(column.SubGroupName))
                            continue;


                        if (tmp = this.AddColumn(column, col_idx, persist_empty_fields, persist_zero_fields))
                        {
                            col_idx++;
                            this.Grid.ColumnDefinitions.Add(new());
                            ac++;
                        }
                    }

                    if (ac != 0)
                    {
                        header = new(){ Text = column!.Desc };
                        Grid.SetColumn(header, col_idx - ac);
                    }

                    column = null;
                }
                else if (op is CVColumnsGraphFilterOperation.GroupSum)
                {
                    column = column_group.Value.First();
                    column = new() { 
                        Min = column!.Min,
                        Max = column!.Max,
                        Desc = column.Desc,
                        LongDesc = column.LongDesc,
                        Value = column_group.Value.Where(x => !group_names.Contains(x.SubGroupName)).Sum(x => x.Value)
                    };

                    if (tmp = this.AddColumn(column, col_idx, persist_empty_fields, persist_zero_fields))
                        ac++;
                }
                else
                {
                    column = column_group.Value.First();
                    column = new()
                    {
                        Max = column.Max,
                        Min = column.Min,
                        Desc = column.Desc,
                        LongDesc = column.LongDesc,
                        Value = double.NaN,
                    };

                    try
                    {
                        var x = column_group.Value.Where(x => !group_names.Contains(x.SubGroupName) && x.Value is not double.NaN).Select(x => x.Values)!.Merge().ToArray();

                        column.Value = column_group.Value.Where(x => !group_names.Contains(x.SubGroupName) && x.Value is not double.NaN).Select(x => x.Values)!.Merge().Average();
                    }
                    catch (Exception)
                    {
                    }

                    if (tmp = this.AddColumn(column, col_idx, persist_empty_fields, persist_zero_fields))
                        ac++;
                }

                if (ac == 0)
                    continue;

                if (op is not CVColumnsGraphFilterOperation.NoneNoOverlap)
                {
                    this.Grid.ColumnDefinitions.Add(new());
                    header = new() { Text = column!.Desc };
                    Grid.SetColumn(header, col_idx++);
                }
                else if (ac != 0)
                {
                    Grid.SetColumnSpan(header, ac);

                    this.Grid.ColumnDefinitions.Add(new());
                    col_idx++;
                }

                Debug.Assert(header is not null);
                header.SetThemeBinding(TextBlock.ForegroundProperty, ThemeProperties.CVGenericFontProperty);
                this.Grid.Children.Add(header);
                Grid.SetRow(header, 1);

                ac = 0;
            }

            if (op is CVColumnsGraphFilterOperation.NoneNoOverlap && this.Grid.Children.Count != 0)
                this.Grid.ColumnDefinitions.RemoveAt(this.Grid.ColumnDefinitions.Count - 1);
        }

        public void Clear()
        {
            this.Grid.ColumnDefinitions.Clear();
            this.Grid.Children.Clear();
            this.Grid.Children.Capacity = 0;
        }
    }
}
