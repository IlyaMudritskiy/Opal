using ProcessDashboard.src.View.Embossing;
using ScottPlot;
using System;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.Embossing
{
    public partial class EmbossingTab
    {
        public TabPage Tab { get; set; }
        public Label Header { get; set; }
        public FormsPlot Plot { get; set; }
        public TableView DS11 { get; set; }
        public TableView DS12 { get; set; }
        public TableView DS21 { get; set; }
        public TableView DS22 { get; set; }

        public EmbossingTab(string title)
        {
            _createLayout(title);
        }

        private void _createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Header = CommonElements.Header(title);
            Plot = CommonElements.Plot();
            DS11 = new TableView("Die-Side 1-1");
            DS12 = new TableView("Die-Side 1-2");
            DS21 = new TableView("Die-Side 2-1");
            DS22 = new TableView("Die-Side 2-2");

            TableLayoutPanel tabBase = CommonElements.Base();
            TableLayoutPanel tableArea = new TableLayoutPanel()
            {
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 25F),
                    new ColumnStyle(SizeType.Percent, 25F),
                    new ColumnStyle(SizeType.Percent, 25F),
                    new ColumnStyle(SizeType.Percent, 25F)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 50F)
                }
            };

            tableArea.SuspendLayout();
            tableArea.Controls.Add(DS11.Layout, 0, 0);
            tableArea.Controls.Add(DS12.Layout, 1, 0);
            tableArea.Controls.Add(DS21.Layout, 2, 0);
            tableArea.Controls.Add(DS22.Layout, 3, 0);
            tableArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(Header, 0, 0);
            tabBase.Controls.Add(Plot, 0, 1);
            tabBase.Controls.Add(tableArea, 0, 2);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }
    }
}

