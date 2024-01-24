﻿using System;
using System.Windows.Forms;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using ProcessDashboard.src.TTL.Processing;
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

        public event EventHandler DSNestToggleButtonClick;
        public Button DSNestToggleButton { get; set; }

        private AcousticData Data { get; set; }
        private string TabType { get; set; } = string.Empty;
        private bool DSPlotView { get; set; } = true;

        #region Constructor

        public AcousticTab(string title, string unitX, string unitY)
        {
            UnitX = unitX;
            UnitY = unitY;
            Plots = new DSContainer<PlotView>();
            TabType = title;
            createLayout(title);
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
                    new RowStyle(SizeType.Absolute, 40),
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

            TableLayoutPanel titleArea = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, 150),
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 100)
                },
                BackColor = Colors.Black,
                Font = Fonts.Sennheiser.M
            };

            DSNestToggleButton = new Button()
            {
                Text = "BUTTON",
                Dock = DockStyle.Fill,
                BackColor = Colors.Default.Grey
            };

            DSNestToggleButton.Click += OnDSNestToggleButtonClick;

            titleArea.SuspendLayout();
            titleArea.Controls.Add(DSNestToggleButton, 0, 0);
            titleArea.Controls.Add(Title, 1, 0);
            titleArea.ResumeLayout();

            plotArea.SuspendLayout();
            plotArea.Controls.Add(Plots.DS11.Layout, 0, 0);
            plotArea.Controls.Add(Plots.DS12.Layout, 1, 0);
            plotArea.Controls.Add(Plots.DS21.Layout, 0, 1);
            plotArea.Controls.Add(Plots.DS22.Layout, 1, 1);
            plotArea.Controls.Add(ComparisonPlot.Layout, 0, 2);
            plotArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(titleArea, 0, 0);
            tabBase.Controls.Add(plotArea, 0, 1);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }

        #endregion

        #region Public methods
        public void AddData(AcousticData data)
        {
            if (data == null) return;
            Data = data;
            DSPlotView = true;
            ShowPlots();
        }

        public void Clear()
        {
            Plots.DS11.Clear();
            Plots.DS12.Clear();
            Plots.DS21.Clear();
            Plots.DS22.Clear();
            ComparisonPlot.Clear();
            Title.Text = string.Empty;
        }

        public void ShowPlots()
        {
            Clear();
            if (DSPlotView)
            {
                SetDSTitles();
                DSPlotView = !DSPlotView;
                ShowDSPlots();
                return;
            }
            if (!DSPlotView)
            {
                SetNestTitles();
                DSPlotView = !DSPlotView;
                ShowNestPlots();
                return;
            }
        }

        #endregion

        #region Toggle Plots view (DS or Nest)

        private void ShowDSPlots()
        {
            Plots.DS11.AddScatter(Data.DSCurves.DS11);
            Plots.DS12.AddScatter(Data.DSCurves.DS12);
            Plots.DS21.AddScatter(Data.DSCurves.DS21);
            Plots.DS22.AddScatter(Data.DSCurves.DS22);
            ComparisonPlot.AddScatter(Data.MeanDSCurves);
            AddLimits();
            Refresh();
        }

        private void ShowNestPlots()
        {
            Plots.DS11.AddScatter(Data.NestCurves.DS11);
            Plots.DS12.AddScatter(Data.NestCurves.DS12);
            Plots.DS21.AddScatter(Data.NestCurves.DS21);
            Plots.DS22.AddScatter(Data.NestCurves.DS22);
            ComparisonPlot.AddScatter(Data.MeanNestCurves);
            AddLimits();
            Refresh();
        }

        #endregion

        #region Limits methods

        private void AddLimits()
        {
            var limits = AcousticDataProcessor.OpenLimitFiles();
            if (TabType.Contains("FR"))
                AddLimit(limits["FRUpper"], limits["FRLower"], limits["FRReference"]);
            if (TabType.Contains("THD"))
                AddLimit(limits["THDUpper"], limits["THDLower"], limits["THDReference"]);
            if (TabType.Contains("RNB"))
                AddLimit(limits["RNBUpper"], limits["RNBLower"], limits["RNBReference"]);
            if (TabType.Contains("IMP"))
                AddLimit(limits["IMPUpper"], limits["IMPLower"], limits["IMPReference"]);
        }

        private void AddLimit(params Limit[] limits)
        {
            foreach (var limit in limits) AddLimitToAllPlots(limit);
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

        #endregion

        #region Service methods

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

        private void SetDSTitles()
        {
            Plots.DS11.Title.Text = "DS 1-1";
            Plots.DS12.Title.Text = "DS 1-2";
            Plots.DS21.Title.Text = "DS 2-1";
            Plots.DS22.Title.Text = "DS 2-2";
            ComparisonPlot.Title.Text = "Mean Plots";
            Title.Text = $"{TabType} | {Data.MachineID} - {Data.ProductID}";
            DSNestToggleButton.Text = "Sort by TestBox";
        }

        private void SetNestTitles()
        {
            Plots.DS11.Title.Text = "TestBox 1";
            Plots.DS12.Title.Text = "TestBox 2";
            Plots.DS21.Title.Text = "TestBox 3";
            Plots.DS22.Title.Text = "TestBox 4";
            ComparisonPlot.Title.Text = "Mean Plots";
            Title.Text = $"{TabType} | {Data.MachineID} - {Data.ProductID}";
            DSNestToggleButton.Text = "Sort by Die-Side";
        }

        #endregion

        #region Event to toggle DS and Nest separation

        private void OnDSNestToggleButtonClick(object sender, EventArgs e)
        {
            DSNestToggleButtonClick?.Invoke(this, EventArgs.Empty);
        }

        public void SubscribeToDSNestToggleButtonClick(EventHandler handler)
        {
            DSNestToggleButtonClick += handler;
        }

        #endregion
    }
}