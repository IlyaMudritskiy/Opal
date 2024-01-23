using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Screen;
using ProcessDashboard.src.Utils;
using ScottPlot;
using ScottPlot.Plottable;

namespace ProcessDashboard.src.TTL.UI.UIElements
{
    public class PlotView
    {
        public Label Title { get; set; }
        public FormsPlot Plot { get; set; }
        public TableLayoutPanel Layout { get; set; }

        public PlotView(string title, Color color, string unitx = "", string unity = "", bool log = false)
        {
            createLayout(title, color, unitx, unity, log);
        }

        public void AddScatter(params List<ScatterPlot>[] data)
        {
            if (data == null || data.Length == 0) return;

            foreach (var plotSet in data)
                foreach (var plot in plotSet)
                    Plot.Plot.Add(plot);

            Refresh();
            Fit();
        }

        public void AddScatter(params ScatterPlot[] data)
        {
            if (data == null || data.Length == 0) return;

            foreach (var plot in data)
                Plot.Plot.Add(plot);

            Refresh();
            Fit();
        }

        public void AddScatter(DSContainer<ScatterPlot> data)
        {
            if (data == null) return;

            if (data.DS11 != null) Plot.Plot.Add(data.DS11);
            if (data.DS12 != null) Plot.Plot.Add(data.DS12);
            if (data.DS21 != null) Plot.Plot.Add(data.DS21);
            if (data.DS22 != null) Plot.Plot.Add(data.DS22);

            Refresh();
            Fit();
        }

        public void AddScatter(Color color, params Measurements2D[] data)
        {
            if (data == null) return;

            foreach (var acoustic in data)
                Plot.Plot.AddScatter(acoustic.X.ToArray(), acoustic.Y.ToArray(), color, 2);

            Refresh();
            Fit();
        }

        public void Clear()
        {
            Plot.Plot.Clear();
            Plot.Refresh();
            Title.Text = "";
        }

        public void Fit()
        {
            Plot.Plot.AxisAuto();
        }

        public void Refresh()
        {
            Plot.Refresh();
        }

        private void createLayout(string title, Color color, string unitx = "", string unity = "", bool log = false)
        {
            Title = CommonElements.Header(title);
            Title.BackColor = color;

            Plot = CommonElements.Plot();
            Plot.Plot.ManualDataArea(new PixelPadding(54, 7, 44, 7));
            Plot.BackColor = Colors.Default.Grey;
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
            };

            panel.BackColor = color;

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
