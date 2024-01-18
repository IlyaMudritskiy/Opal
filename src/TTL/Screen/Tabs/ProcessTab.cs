using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using ProcessDashboard.src.TTL.Misc;
using ProcessDashboard.src.TTL.UI.UIElements;
using ProcessDashboard.src.Utils;
using ScottPlot.Plottable;

namespace ProcessDashboard.Model.Screen.Tabs
{
    public class ProcessTab
    {
        public TabPage Tab { get; set; }
        public PlotView PlotView { get; set; }
        public DSContainer<TableView> FeatureTables { get; set; }

        private Config Config = Config.Instance;

        private ProcessData Data { get; set; }
        private ProcessStep Step { get; set; }

        public ProcessTab(string title, ProcessStep step)
        {
            Step = step;
            FeatureTables = new DSContainer<TableView>();
            CreateLayout(title);
            RegisterCurveVisibilityEvents();
        }

        public void AddData(ProcessData data)
        {
            Clear();
            Data = data;
            FillScreen();
        }

        public void Clear()
        {
            PlotView.Clear();
            FeatureTables.DS11.Clear();
            FeatureTables.DS12.Clear();
            FeatureTables.DS21.Clear();
            FeatureTables.DS22.Clear();
            Data = null;
            Step = ProcessStep.None;
            ResetCheckBoxes();
        }

        private void FillScreen()
        {
            PlotView.AddScatter(
               Data.Curves.DS11,
               Data.Curves.DS12,
               Data.Curves.DS21,
               Data.Curves.DS22
            );

            FeatureTables.DS11.AddData(Data.MeanFeatures.DS11, Colors.DS11C, Data.Features.DS11.Count);
            FeatureTables.DS12.AddData(Data.MeanFeatures.DS12, Colors.DS12C, Data.Features.DS12.Count);
            FeatureTables.DS21.AddData(Data.MeanFeatures.DS21, Colors.DS21C, Data.Features.DS21.Count);
            FeatureTables.DS22.AddData(Data.MeanFeatures.DS22, Colors.DS22C, Data.Features.DS22.Count);
        }


        private void CreateLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            PlotView = new PlotView("", Colors.Black);
            FeatureTables.DS11 = new TableView("Die-Side 1-1");
            FeatureTables.DS12 = new TableView("Die-Side 1-2");
            FeatureTables.DS21 = new TableView("Die-Side 2-1");
            FeatureTables.DS22 = new TableView("Die-Side 2-2");

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
            tableArea.Controls.Add(FeatureTables.DS11.Layout, 0, 0);
            tableArea.Controls.Add(FeatureTables.DS12.Layout, 1, 0);
            tableArea.Controls.Add(FeatureTables.DS21.Layout, 2, 0);
            tableArea.Controls.Add(FeatureTables.DS22.Layout, 3, 0);
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
            FeatureTables.DS11.CheckboxStateChanged += TableView_CheckboxStateChanged_DS11;
            FeatureTables.DS12.CheckboxStateChanged += TableView_CheckboxStateChanged_DS12;
            FeatureTables.DS21.CheckboxStateChanged += TableView_CheckboxStateChanged_DS21;
            FeatureTables.DS22.CheckboxStateChanged += TableView_CheckboxStateChanged_DS22;
        }

        private void TableView_CheckboxStateChanged_DS11(object sender, EventArgs e)
        {
            toggleCurvesVisibility(Data.Curves.DS11, sender);
        }

        private void TableView_CheckboxStateChanged_DS12(object sender, EventArgs e)
        {
            toggleCurvesVisibility(Data.Curves.DS12, sender);
        }

        private void TableView_CheckboxStateChanged_DS21(object sender, EventArgs e)
        {
            toggleCurvesVisibility(Data.Curves.DS21, sender);
        }

        private void TableView_CheckboxStateChanged_DS22(object sender, EventArgs e)
        {
            toggleCurvesVisibility(Data.Curves.DS22, sender);
        }

        private void toggleCurvesVisibility(List<ScatterPlot> list, object sender)
        {
            if (list == null && list.Count == 0) return;
            if (sender is TableView)
            {
                foreach (var curve in list)
                    curve.IsVisible = !curve.IsVisible;

                PlotView.Fit();
                PlotView.Refresh();
            }
        }

        private void ResetCheckBoxes()
        {
            FeatureTables.DS11.CheckBox.Checked = true;
            FeatureTables.DS12.CheckBox.Checked = true;
            FeatureTables.DS21.CheckBox.Checked = true;
            FeatureTables.DS22.CheckBox.Checked = true;
        }

        #endregion
    }
}
