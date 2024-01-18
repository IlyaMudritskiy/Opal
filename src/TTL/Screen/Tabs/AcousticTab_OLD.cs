using System.Windows.Forms;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using ProcessDashboard.src.TTL.Misc;
using ProcessDashboard.src.TTL.Screen;
using ProcessDashboard.src.TTL.UI.UIElements;
using ProcessDashboard.src.Utils;

namespace ProcessDashboard.Model.Screen.Tabs
{
    public class AcousticTab
    {
        public TabPage Tab { get; set; }
        public Label Title { get; set; }

        public DSContainer<PlotView> Plots { get; set; }

        public PlotView ComparisonPlot { get; set; }

        public string UnitX { get; set; }
        public string UnitY { get; set; }

        private AcousticData Data { get; set; }
        private ProcessStep Step { get; set; }

        public AcousticTab(string title, string unitX, string unitY, ProcessStep step)
        {
            UnitX = unitX;
            UnitY = unitY;
            Step = step;
            Plots = new DSContainer<PlotView>();
            createLayout(title);
        }

        public void AddData(AcousticData data)
        {
            if (data == null) return;
            Clear();
            Data = data;
            FillScreen();
        }

        public void Clear()
        {
            Plots.DS11.Clear();
            Plots.DS12.Clear();
            Plots.DS21.Clear();
            Plots.DS22.Clear();
            ComparisonPlot.Clear();
        }

        private void FillScreen()
        {
            Plots.DS11.AddScatter(Data.Curves.DS11);
            Plots.DS12.AddScatter(Data.Curves.DS12);
            Plots.DS21.AddScatter(Data.Curves.DS21);
            Plots.DS22.AddScatter(Data.Curves.DS22);
            ComparisonPlot.AddScatter(Data.MeanCurves.DS11, Data.MeanCurves.DS12, Data.MeanCurves.DS21, Data.MeanCurves.DS22);
            Refresh();
        }

        public void AddLimits(Limit upper, Limit lower, Limit reference)
        {
            AddLimitToAllPlots(upper);
            AddLimitToAllPlots(lower);
            AddLimitToAllPlots(reference);
            Refresh();
            FitPlots();
        }

        private void AddLimitToAllPlots(Limit limit)
        {
            if (limit == null) return;
            Plots.DS11.AddScatter(limit.Curve);
            Plots.DS12.AddScatter(limit.Curve);
            Plots.DS21.AddScatter(limit.Curve);
            Plots.DS22.AddScatter(limit.Curve);
            ComparisonPlot.AddScatter(limit.Curve);
        }

        private void FitPlots()
        {
            Plots.DS11.Fit();
            Plots.DS12.Fit();
            Plots.DS21.Fit();
            Plots.DS22.Fit();
            ComparisonPlot.Fit();
            Refresh();
        }

        private void Refresh()
        {
            Plots.DS11.Refresh();
            Plots.DS12.Refresh();
            Plots.DS21.Refresh();
            Plots.DS22.Refresh();
            ComparisonPlot.Refresh();
        }

        private void createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Title = CommonElements.Header(title);
            Plots.DS11 = new PlotView("Die-Side 1-1", Colors.DS11C, UnitX, UnitY, true);
            Plots.DS12 = new PlotView("Die-Side 1-2", Colors.DS12C, UnitX, UnitY, true);
            Plots.DS21 = new PlotView("Die-Side 2-1", Colors.DS21C, UnitX, UnitY, true);
            Plots.DS22 = new PlotView("Die-Side 2-2", Colors.DS22C, UnitX, UnitY, true);
            ComparisonPlot = new PlotView("Mean Plots", Colors.Black, UnitX, UnitY, true);

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
            plotArea.Controls.Add(Plots.DS11.Layout, 0, 0);
            plotArea.Controls.Add(Plots.DS12.Layout, 1, 0);
            plotArea.Controls.Add(Plots.DS21.Layout, 0, 1);
            plotArea.Controls.Add(Plots.DS22.Layout, 1, 1);
            plotArea.Controls.Add(ComparisonPlot.Layout, 0, 2);
            plotArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(Title, 0, 0);
            tabBase.Controls.Add(plotArea, 0, 1);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }
    }
}
