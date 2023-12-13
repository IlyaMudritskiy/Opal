using ProcessDashboard.src.Model.Screen.Elements;
using ProcessDashboard.src.Utils.Design;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
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

        private ConcurrentBag<ScatterPlot> DS11Curves { get; set; } = new ConcurrentBag<ScatterPlot>();
        private ConcurrentBag<ScatterPlot> DS12Curves { get; set; } = new ConcurrentBag<ScatterPlot>();
        private ConcurrentBag<ScatterPlot> DS21Curves { get; set; } = new ConcurrentBag<ScatterPlot>();
        private ConcurrentBag<ScatterPlot> DS22Curves { get; set; } = new ConcurrentBag<ScatterPlot>();

        private Label XLabel { get; set; }
        private Label YLabel { get; set; }
        private Label XCoordLabel { get; set; }
        private Label YCoordLabel { get; set; }

        private Crosshair Crosshair;

        public TTLineTab(string title)
        {
            createLayout(title);
            WireUpEvents();
            Fit();
        }

        public override void AddScatter(int track, int press, double[] x, double[] y, Color color, string flag = "")
        {
            if (track == 1 && press == 1)
                addCurve(DS11Curves, x, y, color);
            if (track == 1 && press == 2)
                addCurve(DS12Curves, x, y, color);
            if (track == 2 && press == 1)
                addCurve(DS21Curves, x, y, color);
            if (track == 2 && press == 2)
                addCurve(DS22Curves, x, y, color);
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

            Fit();
        }

        private void WireUpEvents()
        {
            // Subscribe to the CheckboxStateChanged event for each GenericControl
            DS11.CheckboxStateChanged += TableView_CheckboxStateChanged_DS11;
            DS12.CheckboxStateChanged += TableView_CheckboxStateChanged_DS12;
            DS21.CheckboxStateChanged += TableView_CheckboxStateChanged_DS21;
            DS22.CheckboxStateChanged += TableView_CheckboxStateChanged_DS22;
        }
        
        private void TableView_CheckboxStateChanged_DS11(object sender, EventArgs e)
        {
            toggleCurvesVisibility(DS11Curves, sender);
        }

        private void TableView_CheckboxStateChanged_DS12(object sender, EventArgs e)
        {
            toggleCurvesVisibility(DS12Curves, sender);
        }

        private void TableView_CheckboxStateChanged_DS21(object sender, EventArgs e)
        {
            toggleCurvesVisibility(DS21Curves, sender);
        }

        private void TableView_CheckboxStateChanged_DS22(object sender, EventArgs e)
        {
            toggleCurvesVisibility(DS22Curves, sender);
        }

        private void toggleCurvesVisibility(ConcurrentBag<ScatterPlot> list, object sender)
        {
            if (list == null && list.Count == 0) return;

            if (sender is TableView control)
            {
                Parallel.ForEach(list, curve =>
                {
                    curve.IsVisible = !curve.IsVisible;
                });
                Fit();
                Refresh();
            }
        }

        private void addCurve(ConcurrentBag<ScatterPlot> list, double[] x, double[] y, Color color)
        {
            list.Add(
                   Plot.Plot.AddScatter(
                       xs: x,
                       ys: y,
                       color: color,
                       markerSize: 0,
                       lineWidth: 1));
        }

        private void Plot_MouseMoved(object sender, MouseEventArgs e)
        {
            (double coordinateX, double coordinateY) = Plot.GetMouseCoordinates();

            XLabel.Text = $"{e.X:70.000}";
            YLabel.Text = $"{e.Y:70.000}";

            XCoordLabel.Text = $"{coordinateX:70.00000000}";
            YCoordLabel.Text = $"{coordinateY:70.00000000}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            Plot.Refresh(lowQuality: false, skipIfCurrentlyRendering: true);
        }

        public void Fit()
        {
            Plot.Plot.AxisAuto();
        }

        public void Refresh()
        {
            Plot.Refresh();
        }
    }
}

