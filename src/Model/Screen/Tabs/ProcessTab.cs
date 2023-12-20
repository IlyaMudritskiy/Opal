using ProcessDashboard.src.Model.AppConfiguration;
using ProcessDashboard.src.Model.Data.TTLine.Process;
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

namespace ProcessDashboard.src.Model.TTL
{
    public class ProcessTab
    {
        public TabPage Tab { get; set; }
        public PlotView PlotView { get; set; }
        public TableView DS11 { get; set; }
        public TableView DS12 { get; set; }
        public TableView DS21 { get; set; }
        public TableView DS22 { get; set; }

        private ProcessSteps StepType { get; set; }

        // Make new data structure that will have all this stuff divided in DS

        private ConcurrentBag<TTLUnit> DS11Units {  get; set; } = new ConcurrentBag<TTLUnit>();
        private ConcurrentBag<TTLUnit> DS12Units { get; set; } = new ConcurrentBag<TTLUnit>();
        private ConcurrentBag<TTLUnit> DS21Units { get; set; } = new ConcurrentBag<TTLUnit>();
        private ConcurrentBag<TTLUnit> DS22Units { get; set; } = new ConcurrentBag<TTLUnit>();

        private ConcurrentBag<ScatterPlot> DS11Curves { get; set; } = new ConcurrentBag<ScatterPlot>();
        private ConcurrentBag<ScatterPlot> DS12Curves { get; set; } = new ConcurrentBag<ScatterPlot>();
        private ConcurrentBag<ScatterPlot> DS21Curves { get; set; } = new ConcurrentBag<ScatterPlot>();
        private ConcurrentBag<ScatterPlot> DS22Curves { get; set; } = new ConcurrentBag<ScatterPlot>();

        private Config Config = Config.Instance;

        public ProcessTab(string title, ProcessSteps stepType)
        {
            this.StepType = stepType;
            CreateLayout(title);
            RegisterCurveVisibilityEvents();
        }

        public void AddData(List<TTLUnit> data)
        {
            Parallel.ForEach(data, unit =>
            {
                if (unit.TrackNumber == 1 && unit.PressNumber == 1)
                {
                    DS11Units.Add(unit);
                    addToScatter(unit, DS11Curves, )
                    
                }
                    
                if (unit.TrackNumber == 1 && unit.PressNumber == 2)
                    DS12Units.Add(unit);
                if (unit.TrackNumber == 2 && unit.PressNumber == 1)
                    DS21Units.Add(unit);
                if (unit.TrackNumber == 2 && unit.PressNumber == 2)
                    DS22Units.Add(unit);
            });


        }

        private void addToScatter(TTLUnit unit, ConcurrentBag<ScatterPlot> bag, Color color)
        {
            if (StepType == ProcessSteps.Temperature)
                bag.Add(PlotView.AddGetScatter(unit.Process.Temperature.X.ToArray(), unit.Process.Temperature.Y.ToArray(), color));
            if (StepType == ProcessSteps.HighPressure)
                bag.Add(PlotView.AddGetScatter(unit.Process.HighPressure.X.ToArray(), unit.Process.HighPressure.Y.ToArray(), color));
        }



        private void CreateLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            PlotView = new PlotView("", Colors.Black);
            DS11 = new TableView("Die-Side 1-1");
            DS12 = new TableView("Die-Side 1-2");
            DS21 = new TableView("Die-Side 2-1");
            DS22 = new TableView("Die-Side 2-2");

            // Basic layout for plot and tables
            TableLayoutPanel tabBase = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 40),
                    new RowStyle(SizeType.Percent, 60)
                }
            };

            // Layout for tables with features 
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
            tabBase.Controls.Add(PlotView.Layout, 0, 0);
            tabBase.Controls.Add(tableArea, 0, 1);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }

        #region Toggle Curves Visibility

        private void RegisterCurveVisibilityEvents()
        {
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
                PlotView.Fit();
                PlotView.Refresh();
            }
        }

        #endregion
    }
}
