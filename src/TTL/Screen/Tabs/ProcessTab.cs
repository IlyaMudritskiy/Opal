using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using ProcessDashboard.src.TTL.UI.UIElements;
using ProcessDashboard.src.Utils;
using ScottPlot;
using ScottPlot.Plottable;

namespace ProcessDashboard.Model.Screen.Tabs
{
    public class ProcessTab
    {
        public TabPage Tab { get; set; }
        public PlotView PlotView { get; set; }
        public DSContainer<TableView> FeatureTables { get; set; }
        public DSContainer<PlotView> FeaturesDistributiuons { get; set; }
        public DSContainer<List<List<MarkerPlot>>> MarkerPlots { get; set; }
        private DSContainer<bool> Visibility { get; set; }
        private DSContainer<List<Bracket>> Brackets { get; set; }

        private Config Config = Config.Instance;
        private string Title = string.Empty;

        private ProcessData Data { get; set; }

        public ProcessTab(string title)
        {
            FeatureTables = new DSContainer<TableView>();
            FeaturesDistributiuons = new DSContainer<PlotView>();
            MarkerPlots = new DSContainer<List<List<MarkerPlot>>>();
            Visibility = new DSContainer<bool>();
            Brackets = new DSContainer<List<Bracket>>();

            CreateLayout(title);
            registerEvents();
            Title = title;
            SetVisibility();
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

            PlotView.Title.Text = $"{Title} | {Data.MachineID} - {Data.ProductID}";

            FeatureTables.DS11.AddData(Data.MeanFeatures.DS11, Colors.DS11C, Data.Features.DS11.Count);
            FeatureTables.DS12.AddData(Data.MeanFeatures.DS12, Colors.DS12C, Data.Features.DS12.Count);
            FeatureTables.DS21.AddData(Data.MeanFeatures.DS21, Colors.DS21C, Data.Features.DS21.Count);
            FeatureTables.DS22.AddData(Data.MeanFeatures.DS22, Colors.DS22C, Data.Features.DS22.Count);
        }

        private void CreateLayout(string title)
        {
            PixelPadding padding = new PixelPadding(30, 7, 25, 7);

            Tab = new TabPage() { Text = title };
            PlotView = new PlotView(title, Colors.Black, padding);
            FeatureTables.DS11 = new TableView("DS 1-1");
            FeatureTables.DS12 = new TableView("DS 1-2");
            FeatureTables.DS21 = new TableView("DS 2-1");
            FeatureTables.DS22 = new TableView("DS 2-2");

            FeaturesDistributiuons.DS11 = new PlotView("DS 1-1", Colors.DS11C, padding);
            FeaturesDistributiuons.DS12 = new PlotView("DS 1-2", Colors.DS12C, padding);
            FeaturesDistributiuons.DS21 = new PlotView("DS 2-1", Colors.DS21C, padding);
            FeaturesDistributiuons.DS22 = new PlotView("DS 2-2", Colors.DS22C, padding);

            TableLayoutPanel tabBase = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 35),
                    new RowStyle(SizeType.Absolute, 318),
                    new RowStyle(SizeType.Percent, 29.7f)
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
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            TableLayoutPanel distributionArea = new TableLayoutPanel()
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
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            distributionArea.SuspendLayout();
            distributionArea.Controls.Add(FeaturesDistributiuons.DS11.Layout, 0, 0);
            distributionArea.Controls.Add(FeaturesDistributiuons.DS12.Layout, 1, 0);
            distributionArea.Controls.Add(FeaturesDistributiuons.DS21.Layout, 2, 0);
            distributionArea.Controls.Add(FeaturesDistributiuons.DS22.Layout, 3, 0);
            distributionArea.ResumeLayout();

            tableArea.SuspendLayout();
            tableArea.Controls.Add(FeatureTables.DS11.Layout, 0, 0);
            tableArea.Controls.Add(FeatureTables.DS12.Layout, 1, 0);
            tableArea.Controls.Add(FeatureTables.DS21.Layout, 2, 0);
            tableArea.Controls.Add(FeatureTables.DS22.Layout, 3, 0);
            tableArea.ResumeLayout();

            tabBase.SuspendLayout();
            tabBase.Controls.Add(PlotView.Layout, 0, 0);
            tabBase.Controls.Add(tableArea, 0, 1);
            tabBase.Controls.Add(distributionArea, 0, 2);
            tabBase.ResumeLayout();

            Tab.Controls.Add(tabBase);
        }

        private void registerEvents()
        {
            // Feature from the table selected
            FeatureTables.DS11.Table.CellDoubleClick += Table_DS11_CellDoubleClick;
            FeatureTables.DS12.Table.CellDoubleClick += Table_DS12_CellDoubleClick;
            FeatureTables.DS21.Table.CellDoubleClick += Table_DS21_CellDoubleClick;
            FeatureTables.DS22.Table.CellDoubleClick += Table_DS22_CellDoubleClick;

            // Curves and corresponding feature props visibility on the plot
            FeatureTables.DS11.CheckboxStateChanged += TableView_CheckboxStateChanged_DS11;
            FeatureTables.DS12.CheckboxStateChanged += TableView_CheckboxStateChanged_DS12;
            FeatureTables.DS21.CheckboxStateChanged += TableView_CheckboxStateChanged_DS21;
            FeatureTables.DS22.CheckboxStateChanged += TableView_CheckboxStateChanged_DS22;
        }

        #region Features distribution

        private void Table_DS11_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cellDoubleClickHandler(e, 11);
        }
        private void Table_DS12_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cellDoubleClickHandler(e, 12);
        }
        private void Table_DS21_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cellDoubleClickHandler(e, 21);
        }
        private void Table_DS22_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cellDoubleClickHandler(e, 22);
        }

        private void cellDoubleClickHandler(DataGridViewCellEventArgs e, int dsIdx)
        {
            // Clear the corresponding plot
            unplotDataPoints(MarkerPlots.Get(dsIdx));
            unplotBrackets(Brackets.Get(dsIdx));
            MarkerPlots.Set(dsIdx, null);
            Brackets.Set(dsIdx, null);

            // Get DS data for further use
            DataGridView table = FeatureTables.Get(dsIdx).Table;                // DS Table containing mean features
            List<List<Feature>> features = Data.Features.Get(dsIdx);            // All features related to a specific DS
            Feature feature = getSelectedFeature(e, table);                     // Selected mean feature from the table
            if (feature == null) return;
            List<Feature> list = getCorrespondingFeatures(feature, features);   // List of all features that have the same name
            PlotView distributionPlot = FeaturesDistributiuons.Get(dsIdx);      // Distribution plot related to a specific DS
            bool visibility = Visibility.Get(dsIdx);                            // Visibility of the distribution plot related to a specific DS
            Color color = Colors.GetDSColor(dsIdx);
            

            distributionPlot.Clear();
            distributionPlot.SetText(feature.Name);

            plotDistributionHistogram(list, feature, distributionPlot, color);
            List<List<MarkerPlot>> dataPointsMarkers = plotDataPoints(list, color, visibility);
            List<Bracket> brackets = plotClusterID(list, visibility);

            MarkerPlots.Set(dsIdx, dataPointsMarkers);
            Brackets.Set(dsIdx, brackets);

            distributionPlot.Refresh();
            distributionPlot.Fit();
        }

        private Feature getSelectedFeature(DataGridViewCellEventArgs e, DataGridView table)
        {
            if (e.RowIndex < 0 && e.ColumnIndex < 0)
                return null;

            if (table.Rows[e.RowIndex].DataBoundItem is Feature data)
                return data;

            return null;
        }

        private List<Feature> getCorrespondingFeatures(Feature feature, List<List<Feature>> featureLists)
        {
            List<Feature> result = new List<Feature>();

            foreach (var featureList in featureLists)
                foreach (var f in featureList)
                    if (f.Name == feature.Name) 
                        result.Add(f);

            return result;
        }

        private void plotDistributionHistogram(List<Feature> features, Feature meanFeature, PlotView plot, Color color)
        {
            if (features == null || features.Count == 0) return;

            List<Bar> result = new List<Bar>();
            List<double> edges = new List<double>();
            List<double> values = features.Select(f => f.Value).ToList();
            List<double> positions = new List<double>();
            values.Sort();

            var min = values.Min();
            var max = values.Max();
            var range = max - min;

            var binCount = (int)Math.Ceiling(Math.Sqrt(features.Count));
            var binWidth = range/binCount;

            double[] binSizes = new double[binCount];

            for (int i = 0; i <= binCount; i++)
                edges.Add(min + i * binWidth);

            for (int i = 0; i < binCount; i++)
                for (int j = 0; j < values.Count; j++)
                    if (values[j] >= edges[i] && values[j] < edges[i + 1])
                        binSizes[i]++;

            for (int i = 0; i < binCount; i++)
                positions.Add(edges[i] + binWidth / 2);

            plot.AddBar(binSizes, positions.ToArray(), color, binWidth * 0.8);
            plot.AddVerticalLine(meanFeature.Value, Colors.Green, 2);
        }

        private List<List<MarkerPlot>> plotDataPoints(List<Feature> feature, Color color, bool visibility)
        {
            var points = feature.Select(f => f.RelatedDataPoints).ToList();
            List<List<MarkerPlot>> marker = new List<List<MarkerPlot>>();
            List<Color> colors = new List<Color>();

            foreach (var _ in points[0])
                colors.Add(Colors.GetRandomColor());

            foreach (var pointsList in points)
            {
                marker.Add(PlotView.AddGetPoints(pointsList, colors, visibility));
            }

            return marker;
        }

        private List<Bracket> plotClusterID(List<Feature> feature, bool visibility)
        {
            var points = feature.Select(f => f.RelatedDataPoints).ToList();
            List<Bracket> bracketPlots = new List<Bracket>();

            for (int i = 0; i < points[0].Count; i++)
            {
                DataPoint min = new DataPoint()
                {
                    X = 9999,
                    Y = 9999
                };
                DataPoint max = new DataPoint()
                {
                    X = -1,
                    Y = -1
                };

                for (int j = 0; j < points.Count; j++)
                {
                    if (points[j][i].IsNaN()) 
                        continue;

                    if (points[j][i].Name != "t2" && points[j][i].X == 0)
                        continue;

                    if (points[j][i].X == 0)
                        if (points[j][i].Y < min.Y)
                            min = points[j][i];

                    if (points[j][i].X < min.X) 
                        min = points[j][i];

                    if (points[j][i].X > max.X) 
                        max = points[j][i];
                }
                var bracket = PlotView.AddGetBracket(min, max, visibility);

                if (bracket != null)
                    bracketPlots.Add(bracket);
            }
            return bracketPlots;
        }

        private void unplotDataPoints(List<List<MarkerPlot>> markers)
        {
            if (markers == null) return;

            foreach (var markerList in markers)
            {
                foreach (var marker in markerList)
                {
                    marker.IsVisible = false;
                }
            }
        }

        private void unplotBrackets(List<Bracket> brackets)
        {
            if (brackets == null) return;

            foreach (var bracket in brackets)
            {
                bracket.IsVisible = false;
            }
        }

        #endregion

        #region Toggle Curves Visibility

        private void TableView_CheckboxStateChanged_DS11(object sender, EventArgs e)
        {
            Visibility.DS11 = toggleCurvesVisibility(Data.Curves.DS11, MarkerPlots.DS11, Brackets.DS11, sender);
        }

        private void TableView_CheckboxStateChanged_DS12(object sender, EventArgs e)
        {
            Visibility.DS12 = toggleCurvesVisibility(Data.Curves.DS12, MarkerPlots.DS12, Brackets.DS12, sender);
        }

        private void TableView_CheckboxStateChanged_DS21(object sender, EventArgs e)
        {
            Visibility.DS21 = toggleCurvesVisibility(Data.Curves.DS21, MarkerPlots.DS21, Brackets.DS21, sender);
        }

        private void TableView_CheckboxStateChanged_DS22(object sender, EventArgs e)
        {
            Visibility.DS22 = toggleCurvesVisibility(Data.Curves.DS22, MarkerPlots.DS22, Brackets.DS22, sender);
        }

        private bool toggleCurvesVisibility(List<ScatterPlot> list, List<List<MarkerPlot>> markers, List<Bracket> brackets, object sender)
        {
            if (list == null && list.Count == 0) return false;
            if (sender is TableView)
            {
                var s = sender as TableView;

                foreach (var curve in list)
                    curve.IsVisible = s.CheckBox.Checked;

                if (markers != null)
                    foreach (var markerList in markers)
                        foreach (var marker in markerList)
                            marker.IsVisible = s.CheckBox.Checked;

                if (brackets != null)
                    foreach (var bracket in brackets)
                        bracket.IsVisible = s.CheckBox.Checked;

                PlotView.Fit();
                PlotView.Refresh();
                return s.CheckBox.Checked;
            }

            return false;
        }

        private void ResetCheckBoxes()
        {
            FeatureTables.DS11.CheckBox.Checked = true;
            FeatureTables.DS12.CheckBox.Checked = true;
            FeatureTables.DS21.CheckBox.Checked = true;
            FeatureTables.DS22.CheckBox.Checked = true;
        }

        private void SetVisibility()
        {
            Visibility.DS11 = FeatureTables.DS11.CheckBox.Checked;
            Visibility.DS12 = FeatureTables.DS12.CheckBox.Checked;
            Visibility.DS21 = FeatureTables.DS21.CheckBox.Checked;
            Visibility.DS22 = FeatureTables.DS22.CheckBox.Checked;
        }

        #endregion
    }
}
