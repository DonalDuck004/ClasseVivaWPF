using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Logica di interazione per CVColumnGraph.xaml
    /// </summary>
    /// 


    public partial class CVColumnsGraph : UserControl
    {
        public CVColumnsGraph()
        {
            InitializeComponent();
        }

        public void Update(IEnumerable<CVColumn> columns)
        {
            this.Grid.ColumnDefinitions.Clear();
            this.Grid.Children.Clear();
            TextBlock header;
            var c = 0;

            foreach (var column_group in from column in columns group column by column.ContentID)
            {
                this.Grid.ColumnDefinitions.Add(new());
                
                foreach (var column in column_group.OrderByDescending(x => x.Value))
                {
                    var d = column.Value;
                    this.Grid.Children.Add(column);
                    Grid.SetColumn(column, c);
                    Grid.SetRow(column, 0);
                }

                header = new() { Text = column_group.First().Desc };
                this.Grid.Children.Add(header);
                Grid.SetRow(header, 1);
                Grid.SetColumn(header, c++);
            }
        }


    }
}
