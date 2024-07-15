using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public PlotView(string title, Color color, PixelPadding padding)
        {
            createLayout(title, color, padding);
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
        /*
        public void AddScatter(Color color, params Measurements2D[] data)
        {
            if (data == null) return;

            foreach (var acoustic in data)
                Control.Plot.AddScatter(acoustic.X.ToArray(), acoustic.Y.ToArray(), color, 2);

            Refresh();
            Fit();
        }
        */

        public Bracket AddGetBracket(DataPoint start, DataPoint end, bool visibility = true)
        {
            if (start.Name != end.Name) return null;

            if (start.IsNaN() || end.IsNaN()) return null;

            var bracket = Control.Plot.AddBracket(start.X, start.Y, end.X, end.Y, start.Name);
            bracket.IsVisible = visibility;

            Refresh();
            Fit();

            return bracket;
        }

        public void AddBar(double[] values, double[] positions, Color color, double width)
        {
            if (values == null || values.Length == 0) return;
            if (positions == null || positions.Length == 0) return;
            if (values.Length != positions.Length) return;

            var bar = Control.Plot.AddBar(values, positions, color);
            bar.BarWidth = width;
            bar.ShowValuesAboveBars = true;
            Control.Plot.XTicks(positions, positions.Select(x => Math.Round(x, 3).ToString()).ToArray());
            Refresh();
            Fit();
        }

        public void AddVerticalLine(double value, Color color, int width = 1, string label = "")
        {
            Control.Plot.AddVerticalLine(value, color, width);
            Refresh();
            Fit();
        }

        public VLine AddGetVerticalLine(double value, Color color, bool visibility, int width = 1, string label = "")
        {
            var line = Control.Plot.AddVerticalLine(value, color, width);
            if (string.IsNullOrEmpty(label)) return null;

            line.Label = label;
            line.IsVisible = visibility;

            Refresh();
            Fit();

            return line;
        }
        /*
        public void AddHorizontalLine(double value, Color color)
        {
            Control.Plot.AddHorizontalLine(value, color);
            Refresh();
            Fit();
        }
        public void AddPoint(DataPoint point, Color color, int markerSize = 7)
        {
            Control.Plot.AddMarker(point.X, point.Y, MarkerShape.filledCircle, markerSize, color);
            Refresh();
            Fit();
        }

        public void AddPoints(List<DataPoint> points, Color color, int markerSize = 7)
        {
            if (points == null || points == null) return;
            foreach (var point in points) 
            {
                Control.Plot.AddMarker(point.X, point.Y, MarkerShape.filledCircle, markerSize, color);
            }
            Refresh();
            Fit();
        }

        public void AddPoints(List<double> xs, List<double> ys, Color color, int markerSize = 7)
        {
            if (xs == null || ys == null) return;
            if (xs.Count != ys.Count) return;
            for (int i = 0; i < xs.Count; i++)
            {
                Control.Plot.AddMarker(xs[i], ys[i], MarkerShape.filledCircle, markerSize, color);
            }
            Refresh();
            Fit();
        }
        */

        /// <summary>
        /// Adds a list of data points to the plot and returns a list of plottable objects. 
        /// List contains same data point from multiple process files (t3_file1, t3_file2, ...).
        /// After creating plottable objects, refreshes and fits the plot.
        /// </summary>
        /// <param name="points">A list of DataPoint objects to be added to the plot.</param>
        /// <param name="color">The color in which the data points should be plotted. If not specified, the default color is used.</param>
        /// <param name="markerSize">The size of the markers used to represent the data points. Default is 7.</param>
        /// <returns>A list of MarkerPlot objects representing the added data points.</returns>
        public List<MarkerPlot> AddGetPoints(List<DataPoint> points, List<Color> colors, bool visibility = true, int markerSize = 7)
        {
            if (points == null) return null;

            List<MarkerPlot> result = new List<MarkerPlot>();

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].IsNaN()) 
                    continue;

                var marker = Control.Plot.AddMarker(points[i].X, points[i].Y, MarkerShape.filledCircle, markerSize, colors[i]);
                marker.IsVisible = visibility;
                result.Add(marker);
            }

            Refresh();
            Fit();
            return result;
        }

        public List<MarkerPlot> AddGetPoints(List<DataPoint> points, Color color, bool visibility = true, int markerSize = 7)
        {
            if (points == null) return null;

            List<MarkerPlot> result = new List<MarkerPlot>();

            foreach (var point in points)
            {
                if (point.IsNaN())
                    continue;

                var marker = Control.Plot.AddMarker(point.X, point.Y, MarkerShape.filledCircle, markerSize, color);
                marker.IsVisible = visibility;
                result.Add(marker);
            }

            Refresh();
            Fit();
            return result;
        }

        public void Clear()
        {
            Control.Plot.Clear();
            Control.Refresh();
            Title.Text = "";
        }

        public void Clear(string title)
        {
            Control.Plot.Clear();
            Control.Refresh();
            Title.Text = title;
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

        private void createLayout(string title, Color color, PixelPadding padding)
        {
            SetText(title);
            AddControl(CommonElements.Plot());
            SetColor(color);
            Control.Plot.ManualDataArea(padding);
            Control.BackColor = Colors.Default.Grey;
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
