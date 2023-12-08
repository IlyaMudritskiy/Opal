using ProcessDashboard.src.Model.Screen.Elements;
using ProcessDashboard.src.Utils.Design;
using ScottPlot;
using ScottPlot.Plottable;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.TTLine
{
    public class TTLineTab : AbsTab
    {
        public FormsPlot Plot { get; set; }
        public TableView DS11 { get; set; }
        public TableView DS12 { get; set; }
        public TableView DS21 { get; set; }
        public TableView DS22 { get; set; }

        private Label XLabel { get; set; }
        private Label YLabel { get; set; }
        private Label XCoordLabel { get; set; }
        private Label YCoordLabel { get; set; }

        private Crosshair Crosshair;

        public TTLineTab(string title)
        {
            createLayout(title);
        }

        public override void AddScatter(double[] x, double[] y, Color color, string flag = "")
        {
            Plot.Plot.AddScatter(
                xs: x,
                ys: y,
                color: color,
                markerSize: 0,
                lineWidth: 1
                );
            Plot.Refresh();
            Plot.Plot.AxisAuto();
            Plot.Refresh();
        }

        public override void Clear()
        {

        }

        protected override void createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Title = CommonElements.Header(title);
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
            tabBase.Controls.Add(Title, 0, 0);
            tabBase.Controls.Add(Plot, 0, 1);
            tabBase.Controls.Add(tableArea, 0, 2);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);

            Crosshair = Plot.Plot.AddCrosshair(0, 0);

            XLabel = new Label()
            {
                AutoSize = true,
                Font = Fonts.Sennheiser.S,
                Location = new System.Drawing.Point(0, 0),
                Name = "XLabel"
            };

            YLabel = new Label()
            {
                AutoSize = true,
                Font = Fonts.Sennheiser.S,
                Location = new System.Drawing.Point(0, 0),
                Name = "XLabel"
            };

            XCoordLabel = new Label()
            {
                AutoSize = true,
                Font = Fonts.Sennheiser.S,
                Location = new System.Drawing.Point(0, 0),
                Name = "XCoordLabel"
            };

            YCoordLabel = new Label()
            {
                AutoSize = true,
                Font = Fonts.Sennheiser.S,
                Location = new System.Drawing.Point(0, 0),
                Name = "YCoordLabel"
            };

            Plot.MouseMove += Plot_MouseMoved;

            Plot.Refresh();
            Plot.Plot.AxisAuto();
        }

        private void Plot_MouseMoved(object sender, MouseEventArgs e)
        {
            (double coordinateX, double coordinateY) = Plot.GetMouseCoordinates();

            XLabel.Text = $"{e.X:0.000}";
            YLabel.Text = $"{e.Y:0.000}";

            XCoordLabel.Text = $"{coordinateX:0.00000000}";
            YCoordLabel.Text = $"{coordinateY:0.00000000}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            Plot.Refresh(lowQuality: false, skipIfCurrentlyRendering: true);
        }
    }
}

