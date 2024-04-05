using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Screen;
using ProcessDashboard.src.Utils;
using ScottPlot;
using ScottPlot.Plottable;

namespace ProcessDashboard.src.TTL.UI.UIElements
{
    public class PlotView : HeaderView<FormsPlot>
    {
        public PlotView(string title, Color color, string unitx = "", string unity = "", bool log = false)
        {
            createLayout(title, color, unitx, unity, log);
        }

        public void AddScatter(params List<ScatterPlot>[] data)
        {
            if (data == null || data.Length == 0) return;

                foreach (var plotSet in data)
                    foreach (var plot in plotSet)
                        if (plot != null)
                            Control.Plot.Add(plot);

            Refresh();
            Fit();
        }

        public void AddScatter(params ScatterPlot[] data)
        {
            if (data == null || data.Length == 0) return;

                foreach (var plot in data)
                if (plot != null)
                    Control.Plot.Add(plot);

            Refresh();
            Fit();
        }

        public void AddScatter(DSContainer<ScatterPlot> data)
        {
            if (data == null) return;

            if (data.DS11 != null) Control.Plot.Add(data.DS11);
            if (data.DS12 != null) Control.Plot.Add(data.DS12);
            if (data.DS21 != null) Control.Plot.Add(data.DS21);
            if (data.DS22 != null) Control.Plot.Add(data.DS22);

            Refresh();
            Fit();
        }

        public void AddScatter(Color color, params Measurements2D[] data)
        {
            if (data == null) return;

            foreach (var acoustic in data)
                Control.Plot.AddScatter(acoustic.X.ToArray(), acoustic.Y.ToArray(), color, 2);

            Refresh();
            Fit();
        }

        public void Clear()
        {
            Control.Plot.Clear();
            Control.Refresh();
            Title.Text = "";
        }

        public void Fit()
        {
            Control.Plot.AxisAuto();
        }

        public void Refresh()
        {
            Control.Refresh();
        }

        private void createLayout(string title, Color color, string unitx = "", string unity = "", bool log = false)
        {
            SetText(title);
            AddControl(CommonElements.Plot());
            SetColor(color);
            Control.Plot.ManualDataArea(new PixelPadding(54, 7, 44, 7));
            Control.BackColor = Colors.Default.Grey;
            Control.Refresh();
            Control.Plot.XLabel(unitx);
            Control.Plot.YLabel(unity);

            if (log)
                toLog(Control);

            Control.Refresh();
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
