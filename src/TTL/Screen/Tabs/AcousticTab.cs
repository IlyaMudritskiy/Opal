using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using ProcessDashboard.src.TTL.Processing;
using ProcessDashboard.src.TTL.Screen;
using ProcessDashboard.src.TTL.UI.UIElements;
using ProcessDashboard.src.Utils;
using ScottPlot.Plottable;

namespace ProcessDashboard.Model.Screen.Tabs
{
    public class AcousticTab
    {
        public TabPage Tab { get; set; }
        public Label Title { get; set; }

        //public DSContainer<PlotView> Plots { get; set; }
        public PlotViewDSContainer Plots { get; set; }

        public PlotView ComparisonPlot { get; set; }

        public string UnitX { get; set; }
        public string UnitY { get; set; }

        public event EventHandler DSNestToggleButtonClick;
        public Button DSNestSortToggleButton { get; set; }
        public CheckBox ShowHideFailsChk { get; set; }
        public CheckBox ShowHidePassChk { get; set; }

        private AcousticData Data { get; set; }
        private string TabType { get; set; } = string.Empty;
        private bool DSPlotView { get; set; } = true;

        #region Constructor

        public AcousticTab(string title, string unitX, string unitY)
        {
            UnitX = unitX;
            UnitY = unitY;
            //Plots = new DSContainer<PlotView>();
            Plots = new PlotViewDSContainer();
            TabType = title;
            createLayout(title);
            ShowHideFailsChk.CheckedChanged += OnShowHideFailsChkCheckedChanged;
            ShowHidePassChk.CheckedChanged += OnShowHidePassChkCheckedChanged;
        }

        private void createLayout(string title)
        {
            Tab = new TabPage() { Text = title };
            Title = CommonElements.Header(title);
            Plots.DS11 = new PlotView("DS 1-1", Colors.DS11C, UnitX, UnitY, true);
            Plots.DS12 = new PlotView("DS 1-2", Colors.DS12C, UnitX, UnitY, true);
            Plots.DS21 = new PlotView("DS 2-1", Colors.DS21C, UnitX, UnitY, true);
            Plots.DS22 = new PlotView("DS 2-2", Colors.DS22C, UnitX, UnitY, true);
            ComparisonPlot = new PlotView("Mean Plots", Colors.Black, UnitX, UnitY, true);

            TableLayoutPanel generalLayoutPanel = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 40),
                    new RowStyle(SizeType.Absolute, 40),
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            TableLayoutPanel plotPanel = new TableLayoutPanel()
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

            TableLayoutPanel titlePanel = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 100)
                },
                BackColor = Colors.Black,
                Font = Fonts.Sennheiser.M
            };

            TableLayoutPanel controlsPanel = new TableLayoutPanel()
            {
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, 150),
                    new ColumnStyle(SizeType.Absolute, 150),
                    new ColumnStyle(SizeType.Absolute, 150),
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 100)
                },
                BackColor= Colors.Black
            };

            DSNestSortToggleButton = new Button()
            {
                Text = "BUTTON",
                Dock = DockStyle.Fill,
                BackColor = Colors.Default.Grey,
                Font = Fonts.Sennheiser.M
            };

            ShowHideFailsChk = new CheckBox()
            {
                Text = "Show Fail Units",
                Dock = DockStyle.Fill,
                BackColor = Colors.Black,
                ForeColor = Colors.White,
                Font = Fonts.Sennheiser.ML,
                Checked = true
            };

            ShowHidePassChk = new CheckBox()
            {
                Text = "Show Pass Units",
                Dock = DockStyle.Fill,
                BackColor = Colors.Black,
                ForeColor = Colors.White,
                Font = Fonts.Sennheiser.ML,
                Checked = true
            };

            DSNestSortToggleButton.Click += OnDSNestToggleButtonClick;

            titlePanel.SuspendLayout();
            titlePanel.Controls.Add(Title, 0, 0);
            titlePanel.ResumeLayout();

            controlsPanel.SuspendLayout();
            controlsPanel.Controls.Add(DSNestSortToggleButton, 0, 0);
            controlsPanel.Controls.Add(ShowHideFailsChk, 1, 0);
            controlsPanel.Controls.Add(ShowHidePassChk, 2, 0);
            controlsPanel.ResumeLayout();

            plotPanel.SuspendLayout();
            plotPanel.Controls.Add(Plots.DS11.Layout, 0, 0);
            plotPanel.Controls.Add(Plots.DS12.Layout, 1, 0);
            plotPanel.Controls.Add(Plots.DS21.Layout, 0, 1);
            plotPanel.Controls.Add(Plots.DS22.Layout, 1, 1);
            plotPanel.Controls.Add(ComparisonPlot.Layout, 0, 2);
            plotPanel.ResumeLayout();

            generalLayoutPanel.SuspendLayout();
            generalLayoutPanel.Controls.Add(titlePanel, 0, 0);
            generalLayoutPanel.Controls.Add(controlsPanel, 0, 1);
            generalLayoutPanel.Controls.Add(plotPanel, 0, 2);
            generalLayoutPanel.ResumeLayout();

            Tab.Controls.Add(generalLayoutPanel);
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
            ShowHideFailsChk.Checked = true;
            ShowHidePassChk.Checked = true;
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

        #region Show Hide Fail curves

        public void ShowHideFailCurves(List<ScatterPlot> target)
        {
            if (target != null && target.Count > 0)
                foreach (var curve in target) curve.IsVisible = ShowHideFailsChk.Checked;
        }

        public void toggleFailVisibility()
        {
            Data.DSCurvesFail.Apply(ShowHideFailCurves);
            Data.NestCurvesFail.Apply(ShowHideFailCurves);
            FitPlots();
            Refresh();
        }

        private void OnShowHideFailsChkCheckedChanged(object sender, EventArgs e)
        {
            toggleFailVisibility();
        }

        #endregion

        #region Show Hide Pass curves

        public void ShowHidePassCurves(List<ScatterPlot> target)
        {
            if (target != null && target.Count > 0)
                foreach (var curve in target) curve.IsVisible = ShowHidePassChk.Checked;
        }

        public void togglePassVisibility()
        {
            Data.DSCurvesPass.Apply(ShowHidePassCurves);
            Data.NestCurvesPass.Apply(ShowHidePassCurves);
            FitPlots();
            Refresh();
        }

        private void OnShowHidePassChkCheckedChanged(object sender, EventArgs e)
        {
            togglePassVisibility();
        }

        #endregion

        #region Toggle Plots view (DS or Nest)

        private void ShowDSPlots()
        {
            Plots.AddScatter(Data.DSCurvesPass, Data.DSCurvesFail);
            ComparisonPlot.AddScatter(Data.MeanDSCurves);
            AddLimits();
            Refresh();
        }

        private void ShowNestPlots()
        {
        Plots.AddScatter(Data.NestCurvesPass, Data.NestCurvesFail);
            ComparisonPlot.AddScatter(Data.MeanNestCurves);
            AddLimits();
            Refresh();
        }

        #endregion

        #region Limits methods

        private void AddLimits()
        {
            var limits = AcousticDataProcessor.OpenLimitFiles();
            if (limits ==  null) return;

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
            DSNestSortToggleButton.Text = "Sort by TestBox";
        }

        private void SetNestTitles()
        {
            Plots.DS11.Title.Text = "TestBox 1";
            Plots.DS12.Title.Text = "TestBox 2";
            Plots.DS21.Title.Text = "TestBox 3";
            Plots.DS22.Title.Text = "TestBox 4";
            ComparisonPlot.Title.Text = "Mean Plots";
            Title.Text = $"{TabType} | {Data.MachineID} - {Data.ProductID}";
            DSNestSortToggleButton.Text = "Sort by Die-Side";
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
