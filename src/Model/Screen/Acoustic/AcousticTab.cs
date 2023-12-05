using ProcessDashboard.src.Model.Data.Acoustic;
using ProcessDashboard.src.Model.Screen.Elements;
using ProcessDashboard.src.Utils.Design;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.Acoustic
{
    public class AcousticTab : AbsTab
    {
        //public TabPage Tab { get; set; }
        //public Label Header { get; set; }
        public PlotView DS11 { get; set; }
        public PlotView DS12 { get; set; }
        public PlotView DS21 { get; set; }
        public PlotView DS22 { get; set; }

        public string UnitX { get; set; }
        public string UnitY { get; set; }

        public AcousticTab(string title, string unitX, string unitY)
        {
            UnitX = unitX;
            UnitY = unitY;
            createLayout(title);
        }

        public override void AddScatter(double[] x, double[] y, Color color, string flag = "")
        {
            double[] xs = x.Select(xx => Math.Log10(xx)).ToArray();
            switch (flag)
            {
                case "DS 1-1":
                    DS11.AddScatter(xs, y, color);
                    break;
                case "DS 1-2":
                    DS12.AddScatter(xs, y, color);
                    break;
                case "DS 2-1":
                    DS21.AddScatter(xs, y, color);
                    break;
                case "DS 2-2":
                    DS22.AddScatter(xs, y, color);
                    break;
            }
            FitPlots();
        }

        public void AddLimits(Limit upper, Limit lower, Limit reference)
        {
            addLimit(upper, Colors.Green);
            addLimit(lower, Colors.Green);
            addLimit(reference, Colors.Red, 1, 1);
            FitPlots();
        }

        private void addLimit(Limit limit, Color color, int line = 2, int marker = 2)
        {
            if (limit == null) { return; }

            double[] xs = limit.X.Select(i => Math.Log10(i)).ToArray();
            DS11.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            DS12.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            DS21.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            DS22.AddScatter(xs, limit.Y.ToArray(), color, line, marker);
            FitPlots();
        }

        public override void Clear()
        {
            DS11.Clear();
            DS12.Clear();
            DS21.Clear();
            DS22.Clear();
        }

        public void FitPlots()
        {
            DS11.Plot.Refresh();
            DS11.Plot.Plot.AxisAuto();
            DS12.Plot.Refresh();
            DS12.Plot.Plot.AxisAuto();
            DS21.Plot.Refresh();
            DS21.Plot.Plot.AxisAuto();
            DS22.Plot.Refresh();
            DS22.Plot.Plot.AxisAuto();
        }

        protected override void createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Title = CommonElements.Header(title);
            DS11 = new PlotView("Die-Side 1-1", Colors.DS11C, UnitX, UnitY, true);
            DS12 = new PlotView("Die-Side 1-2", Colors.DS12C, UnitX, UnitY, true);
            DS21 = new PlotView("Die-Side 2-1", Colors.DS21C, UnitX, UnitY, true);
            DS22 = new PlotView("Die-Side 2-2", Colors.DS22C, UnitX, UnitY, true);

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
            plotArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(Title, 0, 0);
            tabBase.Controls.Add(plotArea, 0, 1);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }
    }
}
