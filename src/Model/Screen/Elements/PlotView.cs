using ProcessDashboard.src.Utils.Design;
using ScottPlot;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.Elements
{
    public class PlotView
    {
        public Label Title { get; set; }
        public FormsPlot Plot { get; set; }
        public TableLayoutPanel Layout { get; set; }

        private string title;
        private int amount;
        private Color grey = Color.FromArgb(20, Color.Black);

        public PlotView(string title, Color color, string unitx = "", string unity = "", bool log = false)
        {
            createLayout(title, color, unitx, unity, log);
        }

        public void AddScatter(double[] x, double[] y, Color color, int lineWidth = 1, int markerSize = 0)
        {
            Plot.Plot.AddScatter(x, y, color, lineWidth, markerSize);
            Plot.Refresh();
        }

        public void Clear()
        {
            Plot.Plot.Clear();
            Plot.Refresh();
        }

        public void AddHorizontalLine(double value, float width)
        {
            Plot.Plot.AddHorizontalLine(value, color: Colors.Black, width);
        }

        private void createLayout(string title, Color color, string unitx = "", string unity = "", bool log = false)
        {
            Title = CommonElements.Header(title);
            this.title = title;
            Title.BackColor = color;

            Plot = CommonElements.Plot();
            Plot.Plot.ManualDataArea(new PixelPadding(52, 1, 42, 1));
            Plot.Refresh();

            TableLayoutPanel panel = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100)
                },
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            Plot.Plot.XLabel(unitx);
            Plot.Plot.YLabel(unity);

            if (log)
                toLog(Plot);

            Plot.Refresh();

            panel.SuspendLayout();
            panel.Controls.Add(Title, 0, 0);
            panel.Controls.Add(Plot, 0, 1);
            panel.ResumeLayout();
            Layout = panel;

            
        }

        private void toLog(FormsPlot plot)
        {
            if (plot == null) return;
            double[] positions = { 20, 30, 70, 200, 300, 500, 700, 2000, 3000, 4000, 5000, 7000, 20000 };
            positions = positions.Select(x => Math.Log10(x)).ToArray();
            string[] labels = { "20", "30", "70", "200", "300", "500", "700", "2k", "3k", "4k", "5k", "7k", "20k" };

            plot.Plot.XAxis.TickLabelFormat(logTickLabels);

            plot.Plot.XAxis.MinorLogScale(true);
            plot.Plot.XAxis.MajorGrid(true, Color.FromArgb(20, Color.Black));
            plot.Plot.XAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            plot.Plot.YAxis.MajorGrid(true, Color.FromArgb(20, Color.Black));
            plot.Plot.XAxis.Ticks(major: true, minor: true);
            plot.Plot.XAxis.AutomaticTickPositions(positions, labels);
        }

        private static string logTickLabels(double y)
        {
            var value = Math.Pow(10, y);
            string result;
            if (value >= 1000)
                result = (value / 1000).ToString("N0") + "k";
            else
                result = value.ToString();
            return result;
        }
    }
}
