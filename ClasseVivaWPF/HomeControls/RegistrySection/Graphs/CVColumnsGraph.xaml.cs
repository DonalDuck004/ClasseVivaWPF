using ClasseVivaWPF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;


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

        public void Update(IEnumerable<CVColumn> columns,
                           bool persist_empty_fields = true,
                           CVColumnsGraphFilterOperation op = CVColumnsGraphFilterOperation.None)
        {
            this.CurrentValues = (from column in columns 
                                  group column by column.ContentID).ToDictionary(
                                    x => x.Key,
                                    x => x.OrderBy(y => y.Value).ToArray()
                                  );

            this.Filter(op: op, persist_empty_fields: persist_empty_fields);
        }

        private bool AddColumn(CVColumn column, int idx, bool persist_empty_fields)
        {
            if (!persist_empty_fields && column.Value is double.NaN)
                return false;

            this.Grid.Children.Add(column);
            Grid.SetColumn(column, idx);

            if (this.ColumnAdded is not null)
                this.ColumnAdded(column);

            return true;
        }

        public void Filter(CVColumnsGraphFilterOperation op = CVColumnsGraphFilterOperation.None,
                           bool persist_empty_fields = true,
                           params string[] group_names)
        {
            TextBlock header;
            var c = 0;
            CVColumn? column = null;
            bool any_added = false;
            this.Clear();

            foreach (var column_group in CurrentValues)
            {
                if (op is CVColumnsGraphFilterOperation.None)
                {
                    for (int i = 0; i < column_group.Value.Length; i++)
                    {
                        column = column_group.Value[i];

                        if (group_names.Contains(column.SubGroupName))
                            continue;

                        any_added |= this.AddColumn(column, c, persist_empty_fields);
                    }
                }else if (op is CVColumnsGraphFilterOperation.GroupSum)
                {
                    column = column_group.Value.First();
                    column = new() { 
                        Desc = column.Desc,
                        LongDesc = column.LongDesc,
                        Value = column_group.Value.Where(x => !group_names.Contains(x.SubGroupName)).Sum(x => x.Value)
                    };

                    any_added |= this.AddColumn(column, c, persist_empty_fields);
                }
                else
                {
                    column = column_group.Value.First();
                    column = new()
                    {
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

                    any_added |= this.AddColumn(column, c, persist_empty_fields);
                }

                if (!any_added)
                {
                    any_added = false;
                    continue;
                }

                this.Grid.ColumnDefinitions.Add(new());
                any_added = false;

                header = new() { Text = column!.Desc };
                this.Grid.Children.Add(header);
                Grid.SetRow(header, 1);
                Grid.SetColumn(header, c++);
            }
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
                this.Grid.Children.Add(header);
                Grid.SetRow(header, 1);
                Grid.SetColumn(header, c++);
            }
        }

        public void Clear()
        {
            this.Grid.ColumnDefinitions.Clear();
            this.Grid.Children.Clear();
            this.Grid.Children.Capacity = 0;
        }
    }
}
