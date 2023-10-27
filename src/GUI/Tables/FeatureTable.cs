using System.Windows.Forms;

namespace ProcessDashboard.src.GUI.Tables
{
    public class FeatureTable
    {
        private TableLayoutPanel Table { get; set; }

        public TableLayoutPanel Get(int featuresNumber)
        {
            Table = new TableLayoutPanel()
            {
                ColumnCount = 4,
                RowCount = featuresNumber + 1
            };

            return Table;
        }
    }
}
