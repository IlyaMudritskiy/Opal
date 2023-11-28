using ScottPlot;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.Acoustic
{
    public class AcousticTab
    {
        public TabPage Tab { get; set; }
        public Label Header { get; set; }
        public FormsPlot DS11 { get; set; }
        public FormsPlot DS12 { get; set; }
        public FormsPlot DS21 { get; set; }
        public FormsPlot DS22 { get; set; }

        public AcousticTab(string title)
        {
            _createLayout(title);
        }

        public void AddScatter(double[] x, double[] y, Color color, string label, string DS)
        {
            switch (DS)
            {
                case "DS11":
                    addScatter(x, y, color, label, DS11);
                    break;
                case "DS12":
                    addScatter(x, y, color, label, DS12);
                    break;
                case "DS21":
                    addScatter(x, y, color, label, DS21);
                    break;
                case "DS22":
                    addScatter(x, y, color, label, DS22);
                    break;
            }
        }

        private void addScatter(double[] x, double[] y, Color color, string label, FormsPlot plot)
        {
            plot.Plot.AddScatter(
                xs: x,
                ys: y,
                color: color,
                markerSize: 3,
                lineWidth: 1,
                label: label
                );
            plot.Refresh();
            plot.Plot.AxisAuto();
        }

        private void _createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Header = CommonElements.Header(title);
            DS11 = CommonElements.Plot("Die-Side 1-1");
            DS12 = CommonElements.Plot("Die-Side 1-2");
            DS21 = CommonElements.Plot("Die-Side 2-1");
            DS22 = CommonElements.Plot("Die-Side 2-2");

            TableLayoutPanel tabBase = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F)},
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            TableLayoutPanel plotArea = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 50F),
                    new ColumnStyle(SizeType.Percent, 50F)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 33.3F),
                    new RowStyle(SizeType.Percent, 33.3F),
                    new RowStyle(SizeType.Percent, 33.3F)
                }
            };

            plotArea.SuspendLayout();
            plotArea.Controls.Add(DS11, 0, 0);
            plotArea.Controls.Add(DS12, 1, 0);
            plotArea.Controls.Add(DS21, 0, 1);
            plotArea.Controls.Add(DS22, 1, 1);
            plotArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(Header, 0, 0);
            tabBase.Controls.Add(plotArea, 0, 1);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }
    }
}
