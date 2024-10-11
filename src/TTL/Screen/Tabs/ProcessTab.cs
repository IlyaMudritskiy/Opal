using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Opal.Model.AppConfiguration;
using Opal.Model.Data.Acoustic;
using Opal.src.CommonClasses.DataProvider;
using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.Containers.FileContent;
using Opal.src.TTL.Containers.ScreenData;
using Opal.src.TTL.UI.UIElements;
using Opal.src.Utils;
using ScottPlot;
using ScottPlot.Plottable;

namespace Opal.Model.Screen.Tabs
{
    public class ProcessTab
    {
        public TabPage Tab { get; set; }
        public PlotView PlotView { get; set; }
        public DSContainer<TableView> FeatureTables { get; set; }
        public DSContainer<PlotView> FeaturesDistributions { get; set; }
        public DSContainer<List<List<MarkerPlot>>> MarkerPlots { get; set; }
        private DSContainer<bool> Visibility { get; set; }
        private DSContainer<List<Bracket>> Brackets { get; set; }
        private DSContainer<bool> IsFeatureSelected { get; set; }

        private Config _config = Config.Instance;
        private string Title = string.Empty;

        private ProcessData Data { get; set; }

        private Dictionary<TableView, int?> _selectedFeatureIndices;
        private Dictionary<TableView, bool> _checkboxStates;
        private DSContainer<FixedSizeBuffer<List<Feature>>> _accumulatedFeatures;

        public ProcessTab(string title)
        {
            FeatureTables = new DSContainer<TableView>();
            FeaturesDistributions = new DSContainer<PlotView>();
            MarkerPlots = new DSContainer<List<List<MarkerPlot>>>();
            Visibility = new DSContainer<bool>();
            Brackets = new DSContainer<List<Bracket>>();
            IsFeatureSelected = new DSContainer<bool>(false);

            CreateLayout(title);
            registerEvents();
            Title = title;
            SetVisibility();

            _selectedFeatureIndices = new Dictionary<TableView, int?>
            {
                { FeatureTables.DS11, null },
                { FeatureTables.DS12, null },
                { FeatureTables.DS21, null },
                { FeatureTables.DS22, null }
            };

            _checkboxStates = new Dictionary<TableView, bool>
            {
                { FeatureTables.DS11, false },
                { FeatureTables.DS12, false },
                { FeatureTables.DS21, false },
                { FeatureTables.DS22, false }
            };

            FeatureTables.Apply(table =>
            {
                _checkboxStates[table] = table.CheckBox.Checked;
            });

            _accumulatedFeatures = new DSContainer<FixedSizeBuffer<List<Feature>>>(
                new FixedSizeBuffer<List<Feature>>(_config.HubBufferSize),
                new FixedSizeBuffer<List<Feature>>(_config.HubBufferSize),
                new FixedSizeBuffer<List<Feature>>(_config.HubBufferSize),
                new FixedSizeBuffer<List<Feature>>(_config.HubBufferSize)
                );
        }

        public void AddData(ProcessData data)
        {
            SaveCheckboxStates();
            Clear();
            Data = data;
            FillScreen();
            AccumulateFeatures();
            RestoreCheckboxStates();
            RestoreFeaturesState();
        }

        public void Clear()
        {
            PlotView.Clear();
            FeatureTables.DS11.Clear();
            FeatureTables.DS12.Clear();
            FeatureTables.DS21.Clear();
            FeatureTables.DS22.Clear();
            Data = null;
        }

        private void FillScreen()
        {
            PreFillCurvesVisibility();

            PlotView.AddScatter(
               Data.Curves.DS11,
               Data.Curves.DS12,
               Data.Curves.DS21,
               Data.Curves.DS22
            );

            string title = $"{Title} | {Data.LineID} - {Data.ProductID}";

            if (_config.DataProvider.Type == DataProviderType.Hub)
                title += $" ({_config.HubBufferSize} last DUTs)";

            PlotView.Title.Text = title;

            if (Data.MeanFeatures.DS11 != null)
                FeatureTables.DS11.AddData(Data.MeanFeatures.DS11, Data.ProductLimits, Colors.DS11C, Data.Features.DS11.Count);

            if (Data.MeanFeatures.DS12 != null)
                FeatureTables.DS12.AddData(Data.MeanFeatures.DS12, Data.ProductLimits, Colors.DS12C, Data.Features.DS12.Count);

            if (Data.MeanFeatures.DS21 != null)
                FeatureTables.DS21.AddData(Data.MeanFeatures.DS21, Data.ProductLimits, Colors.DS21C, Data.Features.DS21.Count);

            if (Data.MeanFeatures.DS22 != null)
                FeatureTables.DS22.AddData(Data.MeanFeatures.DS22, Data.ProductLimits, Colors.DS22C, Data.Features.DS22.Count);
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

            FeaturesDistributions.DS11 = new PlotView("DS 1-1", Colors.DS11C, padding);
            FeaturesDistributions.DS12 = new PlotView("DS 1-2", Colors.DS12C, padding);
            FeaturesDistributions.DS21 = new PlotView("DS 2-1", Colors.DS21C, padding);
            FeaturesDistributions.DS22 = new PlotView("DS 2-2", Colors.DS22C, padding);

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
                    new RowStyle(SizeType.Absolute, 340),
                    new RowStyle(SizeType.Percent, 25)
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
            distributionArea.Controls.Add(FeaturesDistributions.DS11.Layout, 0, 0);
            distributionArea.Controls.Add(FeaturesDistributions.DS12.Layout, 1, 0);
            distributionArea.Controls.Add(FeaturesDistributions.DS21.Layout, 2, 0);
            distributionArea.Controls.Add(FeaturesDistributions.DS22.Layout, 3, 0);
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
            var tableView = FeatureTables.Get(dsIdx);
            var table = tableView.Table;
            var rowCells = table.Rows[e.RowIndex].Cells;

            var savedFeatureIndex = GetSavedFeatureIndex(tableView);

            // If no feature was selected before, plot Feature properties
            if (!IsFeatureSelected.Get(dsIdx))
            {
                PlotFeaturesProperties(e, dsIdx);
                IsFeatureSelected.Set(dsIdx, true);
                return;
            }

            // If any of the features was selected in table, save the state
            foreach (DataGridViewCell cell in rowCells)
            {
                if (cell.Selected)
                {
                    IsFeatureSelected.Set(dsIdx, true);
                    break;
                }
            }

            // If feature was selected, and double click happened, deselect it and unplot properties
            if (savedFeatureIndex != null && savedFeatureIndex == e.RowIndex)
            {
                if (IsFeatureSelected.Get(dsIdx))
                {
                    UnplotPoints(dsIdx);
                    DeselectFeature(e, dsIdx);
                    IsFeatureSelected.Set(dsIdx, false);
                    return;
                }
            }
            if (savedFeatureIndex != null && savedFeatureIndex != e.RowIndex)
            {
                if (IsFeatureSelected.Get(dsIdx))
                {
                    PlotFeaturesProperties(e, dsIdx);
                    IsFeatureSelected.Set(dsIdx, true);
                    return;
                }
            }

        }

        private void PlotFeaturesProperties(DataGridViewCellEventArgs e, int dsIdx)
        {
            // Clear the corresponding plot
            UnplotPoints(dsIdx);

            // Get DS data for further use
            DataGridView table = FeatureTables.Get(dsIdx).Table; // DS Table containing mean features

            List<List<Feature>> accumulatedFeatures = new List<List<Feature>>();
            List<Feature> selectedAccumFeature = new List<Feature>();

            if (_config.DataProvider.Type == DataProviderType.Hub)
            {
                accumulatedFeatures = _accumulatedFeatures.Get(dsIdx).Buffer;
            }

            List<List<Feature>> features = Data.Features.Get(dsIdx);

            FeatureWithLimits featureWithLimits = getSelectedFeature(e, table); // Now retrieves FeatureWithLimits from cell values
                                                                                // Convert FeatureWithLimits back to Feature if necessary
            Feature feature = new Feature
            {
                Name = featureWithLimits.Name,
                Value = featureWithLimits.Value,
                Description = featureWithLimits.Description
                // Add other properties if needed
            };

            if (feature == null) return;
            if (accumulatedFeatures != null)
            {
                selectedAccumFeature = getCorrespondingFeatures(feature, accumulatedFeatures);
            }

            List<Feature> selectedFeature = getCorrespondingFeatures(feature, features); // List of all features that have the same name

            PlotView distributionPlot = FeaturesDistributions.Get(dsIdx); // Distribution plot related to a specific DS
            bool visibility = Visibility.Get(dsIdx); // Visibility of the distribution plot related to a specific DS
            Color color = Colors.GetDSColor(dsIdx);

            distributionPlot.Clear();
            distributionPlot.SetText(feature.Name);

            if (_config.DataProvider.Type == DataProviderType.Hub)
            {
                plotDistributionHistogram(selectedAccumFeature, feature, distributionPlot, color);
            }
            else
            {
                plotDistributionHistogram(selectedFeature, feature, distributionPlot, color);
            }

            List<List<MarkerPlot>> dataPointsMarkers = plotDataPoints(selectedFeature, color, visibility);
            List<Bracket> brackets = plotClusterID(selectedFeature, visibility);

            MarkerPlots.Set(dsIdx, dataPointsMarkers);
            Brackets.Set(dsIdx, brackets);

            distributionPlot.Refresh();
            distributionPlot.Fit();

            SaveSelectedFeature(FeatureTables.Get(dsIdx), e.RowIndex);
        }

        private FeatureWithLimits getSelectedFeature(DataGridViewCellEventArgs e, DataGridView table)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return null;

            var row = table.Rows[e.RowIndex];
            return row.DataBoundItem as FeatureWithLimits;
        }

        private void DeselectFeature(DataGridViewCellEventArgs e, int dsIdx)
        {
            var table = FeatureTables.Get(dsIdx).Table;
            table.Rows[e.RowIndex].Selected = false;
        }

        private void UnplotPoints(int dsIdx)
        {
            unplotDataPoints(MarkerPlots.Get(dsIdx));
            unplotBrackets(Brackets.Get(dsIdx));
            MarkerPlots.Set(dsIdx, null);
            Brackets.Set(dsIdx, null);

            var distributionPlot = FeaturesDistributions.Get(dsIdx);
            distributionPlot.Clear();
            distributionPlot.Refresh();
            distributionPlot.Fit();
            PlotView.Refresh();
        }

        private int? GetSavedFeatureIndex(TableView tableView)
        {
            var dsIdx = GetDSIndex(tableView);
            var index = (dsIdx / 10 - 1) * 2 + (dsIdx % 10) - 1;
            var kvp = _selectedFeatureIndices.ElementAt(index);

            if (kvp.Value.HasValue && kvp.Value >= 0)
            {
                return kvp.Value;
            }
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

            // Calculate the range of features
            var min = values.Min();
            var max = values.Max();
            var diff = max - min;
            var range = diff == 0 ? 1 : diff;

            // Number of bins (bars) and their width
            var binCount = (int)Math.Ceiling(Math.Sqrt(features.Count));
            var binWidth = range / binCount;

            // count the amount of values that got into certain bin
            double[] binSizes = new double[binCount];

            for (int i = 0; i <= binCount; i++)
                edges.Add(min + i * binWidth);

            var maxEdge = edges[edges.Count - 1];

            if (maxEdge <= max)
                edges[edges.Count - 1] = maxEdge * 1.01;

            // For each bin (bar)
            for (int i = 0; i < binCount; i++)
            {
                // for each value in list of values for specific feature
                for (int j = 0; j < values.Count; j++)
                {
                    /*
                     * 1. Check if value is greater than the lower edge of the bin
                     * 2. If it is the last bin: i == binCount - 1
                     *  1. Check the value with the edge including the edge: values[j] <= edges[i + 1]
                     *  2. If not last bin, check the value excluding the edge: values[j] < edges[i + 1]
                     */
                    bool isInBin = (values[j] >= edges[i])
                        && (i == binCount - 1 ? values[j] <= edges[i + 1] : values[j] < edges[i + 1]);

                    if (isInBin)
                    {
                        binSizes[i]++;
                    }
                }
            }

            for (int i = 0; i < binCount; i++)
                positions.Add(edges[i] + binWidth / 2);

            plot.AddBar(binSizes, positions.ToArray(), color, binWidth * 0.8);
            plot.AddVerticalLine(meanFeature.Value, Colors.Green, 2);

            if (Data.ProductLimits != null && Data.ProductLimits.MeanLimits.TryGetValue(meanFeature.Name, out MeanLimit meanLimit))
            {
                plot.AddVerticalLine(meanLimit.Min, Colors.Red, 2);
                plot.AddVerticalLine(meanLimit.Max, Colors.Red, 2);
            }
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

        private void AccumulateFeatures()
        {
            if (_config.DataProvider.Type != DataProviderType.Hub) return;
            AccumulateFeature(11);
            AccumulateFeature(12);
            AccumulateFeature(21);
            AccumulateFeature(22);
        }

        private void AccumulateFeature(int dsIdx)
        {
            if (Data.MeanFeatures.Get(dsIdx) == null) return;

            if (!_accumulatedFeatures.Get(dsIdx).Contains(Data.MeanFeatures.Get(dsIdx)))
            {
                _accumulatedFeatures.Get(dsIdx).Add(Data.MeanFeatures.Get(dsIdx));
            }
        }

        #endregion

        #region Save and Restore selected features

        private void SaveSelectedFeature(TableView tableView, int rowIndex)
        {
            _selectedFeatureIndices[tableView] = rowIndex;
        }

        private void RestoreFeaturesState()
        {
            if (_config.DataProvider.Type != DataProviderType.Hub)
                return;

            for (int i = 0; i < _selectedFeatureIndices.Count; i++)
            {
                var kvp = _selectedFeatureIndices.ElementAt(i);

                if (kvp.Value.HasValue && kvp.Value >= 0 && IsFeatureSelected.Elements[i])
                {
                    RestoreFeature(kvp.Key, kvp.Value.Value, i);
                }
            }
        }

        private void RestoreFeature(TableView tableView, int rowIndex, int tableIndex)
        {
            if (rowIndex < tableView.Table.Rows.Count && IsFeatureSelected.Get(tableIndex))
            {
                // Select the feature in the table
                tableView.Table.CurrentCell = tableView.Table.Rows[rowIndex].Cells[0];

                // Plot the feature on the main plot
                //cellDoubleClickHandler(new DataGridViewCellEventArgs(0, rowIndex), GetDSIndex(tableView));
                PlotFeaturesProperties(new DataGridViewCellEventArgs(0, rowIndex), GetDSIndex(tableView));
            }
        }

        private int GetDSIndex(TableView tableView)
        {
            switch (tableView)
            {
                case TableView DS11 when ReferenceEquals(DS11, FeatureTables.DS11): return 11;
                case TableView DS12 when ReferenceEquals(DS12, FeatureTables.DS12): return 12;
                case TableView DS21 when ReferenceEquals(DS21, FeatureTables.DS21): return 21;
                case TableView DS22 when ReferenceEquals(DS22, FeatureTables.DS22): return 22;
                default: throw new ArgumentException("Invalid TableView");
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

        private void SetVisibility()
        {
            Visibility.DS11 = FeatureTables.DS11.CheckBox.Checked;
            Visibility.DS12 = FeatureTables.DS12.CheckBox.Checked;
            Visibility.DS21 = FeatureTables.DS21.CheckBox.Checked;
            Visibility.DS22 = FeatureTables.DS22.CheckBox.Checked;
        }

        private void SaveCheckboxStates()
        {
            FeatureTables.Apply(table =>
            {
                _checkboxStates[table] = table.CheckBox.Checked;
            });
        }

        private void RestoreCheckboxStates()
        {
            FeatureTables.Apply(table =>
            {
                table.CheckBox.Checked = _checkboxStates[table];
                UpdatePlotVisibility(table);
            });
        }

        private void UpdatePlotVisibility(TableView tableView)
        {
            int dsIndex = GetDSIndex(tableView);
            List<ScatterPlot> curves = Data.Curves.Get(dsIndex);
            List<List<MarkerPlot>> markers = MarkerPlots.Get(dsIndex);
            List<Bracket> brackets = Brackets.Get(dsIndex);

            bool isVisible = tableView.CheckBox.Checked;

            if (curves != null)
            {
                foreach (var curve in curves)
                    curve.IsVisible = isVisible;
            }

            if (markers != null)
            {
                foreach (var markerList in markers)
                {
                    foreach (var marker in markerList)
                        marker.IsVisible = isVisible;
                }
            }

            if (brackets != null)
            {
                foreach (var bracket in brackets)
                    bracket.IsVisible = isVisible;
            }

            PlotView.Fit();
            PlotView.Refresh();
        }

        private void PreFillCurvesVisibility()
        {
            var visibility = _checkboxStates.Values.ToList();

            for (int i = 0; i < visibility.Count; i++)
            {
                var curves = Data.Curves.Elements[i];

                if (curves == null) return;

                foreach (var curve in curves)
                {
                    curve.IsVisible = visibility[i];
                }
            }
        }

        #endregion
    }
}
