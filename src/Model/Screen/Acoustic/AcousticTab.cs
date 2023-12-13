using ProcessDashboard.src.Model.Data.Acoustic;
using ProcessDashboard.src.Model.Screen.Elements;
using ProcessDashboard.src.Utils.Design;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.Acoustic
{
    public class AcousticTab : AbsTab
    {
        public PlotView DS11 { get; set; }
        public PlotView DS12 { get; set; }
        public PlotView DS21 { get; set; }
        public PlotView DS22 { get; set; }
        public PlotView AcousticMean { get; set; }

        public string UnitX { get; set; }
        public string UnitY { get; set; }
        private List<AcousticMeasurement> ds11Mean = new List<AcousticMeasurement>();
        private List<AcousticMeasurement> ds12Mean = new List<AcousticMeasurement>();
        private List<AcousticMeasurement> ds21Mean = new List<AcousticMeasurement>();
        private List<AcousticMeasurement> ds22Mean = new List<AcousticMeasurement>();

        public AcousticTab(string title, string unitX, string unitY)
        {
            UnitX = unitX;
            UnitY = unitY;
            createLayout(title);
        }

        public override void AddScatter(int track, int press, double[] x, double[] y, Color color, string flag = "")
        {
            double[] xs = x.Select(xx => Math.Log10(xx)).ToArray();
            if (track == 1 && press == 1)
            {
                DS11.AddScatter(xs, y, color);
                ds11Mean.Add(new AcousticMeasurement() { X = xs, Y = y });
            }
                    
            if (track == 1 && press == 2)
            {
                DS12.AddScatter(xs, y, color);
                ds12Mean.Add(new AcousticMeasurement() { X = xs, Y = y });
            }
                
            if (track == 2 && press == 1)
            {
                DS21.AddScatter(xs, y, color);
                ds21Mean.Add(new AcousticMeasurement() { X = xs, Y = y });
            }
                
            if (track == 2 && press == 2)
            {
                DS22.AddScatter(xs, y, color);
                ds22Mean.Add(new AcousticMeasurement() { X = xs, Y = y });
            }
        }

        public void AddLimits(Limit upper, Limit lower, Limit reference)
        {
            addLimit(upper, Colors.Green);
            addLimit(lower, Colors.Green);
            addLimit(reference, Colors.Red, 1, 1);
        }

        public void PlotMean()
        {
            var ds11 = getMean(ds11Mean);
            var ds12 = getMean(ds12Mean);
            var ds21 = getMean(ds21Mean);
            var ds22 = getMean(ds22Mean);

            if (ds11 != null) 
                AcousticMean.AddScatter(ds11.X, ds11.Y, Colors.DS11C, 2);

            if (ds12 != null)
                AcousticMean.AddScatter(ds12.X, ds12.Y, Colors.DS12C, 2);

            if (ds21 != null)
                AcousticMean.AddScatter(ds21.X, ds21.Y, Colors.DS21C, 2);

            if (ds22 != null)
                AcousticMean.AddScatter(ds22.X, ds22.Y, Colors.DS22C, 2);
            FitPlots();
            Refresh();
        }

        public void Refresh()
        {
            DS11.Plot.Refresh();
            DS12.Plot.Refresh();
            DS21.Plot.Refresh();
            DS22.Plot.Refresh();
        }

        private AcousticMeasurement getMean(List<AcousticMeasurement> dsMeasurements)
        {
            if (dsMeasurements.Count == 0 || dsMeasurements == null) return null;

            int counter = 0;

            List<double> xmean = new List<double>(dsMeasurements[0].X.Length);
            List<double> ymean = new List<double>(dsMeasurements[0].Y.Length);

            for (int i = 0; i < dsMeasurements[0].X.Length; i++)
            {
                xmean.Add(0);
                ymean.Add(0);
            }

            foreach (var ds in dsMeasurements)
            {
                for (int i = 0; i < ds.X.Length; i++)
                {
                    xmean[i] = xmean[i] + dsMeasurements[0].X[i];
                    ymean[i] = ymean[i] + dsMeasurements[0].Y[i];
                }
                counter++;
            }
            return new AcousticMeasurement()
            {
                X = xmean.Select(e => e / counter).ToArray(),
                Y = ymean.Select(e => e / counter).ToArray()
            };
        }

        private void addLimit(Limit limit, Color color, int line = 2, int marker = 2)
        {
            if (limit == null) { return; }

            double[] xs = limit.X.Select(i => Math.Log10(i)).ToArray();
            DS11.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            DS12.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            DS21.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            DS22.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            AcousticMean.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            FitPlots();
        }

        public override void Clear()
        {
            DS11.Clear();
            DS12.Clear();
            DS21.Clear();
            DS22.Clear();
            AcousticMean.Clear();
        }

        public void FitPlots()
        {
            DS11.Fit();
            DS12.Fit();
            DS21.Fit();
            DS22.Fit();
            AcousticMean.Fit();
            Refresh();
        }

        protected override void createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Title = CommonElements.Header(title);
            DS11 = new PlotView("Die-Side 1-1", Colors.DS11C, UnitX, UnitY, true);
            DS12 = new PlotView("Die-Side 1-2", Colors.DS12C, UnitX, UnitY, true);
            DS21 = new PlotView("Die-Side 2-1", Colors.DS21C, UnitX, UnitY, true);
            DS22 = new PlotView("Die-Side 2-2", Colors.DS22C, UnitX, UnitY, true);
            AcousticMean = new PlotView("Mean Plots", Colors.Black, UnitX, UnitY, true);

            TableLayoutPanel tabBase = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
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
            plotArea.Controls.Add(DS11.Layout, 0, 0);
            plotArea.Controls.Add(DS12.Layout, 1, 0);
            plotArea.Controls.Add(DS21.Layout, 0, 1);
            plotArea.Controls.Add(DS22.Layout, 1, 1);
            plotArea.Controls.Add(AcousticMean.Layout, 0, 2);
            plotArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(Title, 0, 0);
            tabBase.Controls.Add(plotArea, 0, 1);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }
    }
}
