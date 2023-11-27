using ProcessDashboard.src.View.Embossing;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.Embossing
{
    public class TableView
    {
        public Label Title { get; set; }
        public DataGridView Table { get; set; }
        public BindingSource DataSource { get; set; }
        public TableLayoutPanel Layout { get; set; }

        public TableView(string title)
        {
            _createLayout(title);
        }

        private void _createLayout(string title)
        {
            Title = CommonElements.Header(title);
            Table = CommonElements.DataGridView();
            DataSource = new BindingSource();

            Layout = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 1,
                Dock = DockStyle.Fill,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            Table.AutoGenerateColumns = false;

            var cell1 = new DataGridViewTextBoxColumn();
            cell1.DataPropertyName = "ID";
            cell1.HeaderText = "Feature";

            var cell2 = new DataGridViewTextBoxColumn();
            cell2.DataPropertyName = "Value";
            cell2.HeaderText = "Val";

            Table.Columns.Add(cell1);

            Table.Columns.Add(cell2);

            Table.DataSource = DataSource;

            Layout.SuspendLayout();
            Layout.Controls.Add(Title, 0, 0);
            Layout.Controls.Add(Table, 0, 1);
            Layout.ResumeLayout();
        }
    }
}