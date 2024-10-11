using Newtonsoft.Json.Linq;
using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.Model.Data.Acoustic;
using Opal.src.CommonClasses.Processing;
using Opal.src.TTL.Containers.FileContent;
using Opal.src.TTL.UI.UIElements;
using Opal.src.Utils;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Opal.src.TTL.Screen
{
    internal class AcousticOpenerScreen : IScreen, IDisposable
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly Lazy<AcousticOpenerScreen> Lazy = new Lazy<AcousticOpenerScreen>(() => new AcousticOpenerScreen());
        public static AcousticOpenerScreen Instance => Lazy.Value;

        private Config Config = Config.Instance;

        private ComboBox StepsSelector { get; set; }
        private TableView StatisticsTable { get; set; }
        private PlotView StepsPlot { get; set; }
        private TableLayoutPanel Layout { get; set; }

        private List<AcousticFile> Files { get; set; }

        Dictionary<string, AcousticStepStatistics> StepsStatistics { get; set; }
        private int PassCount { get; set; }
        private int FailCount { get; set; }

        public void Show(Panel panel)
        {
            StepsStatistics = new Dictionary<string, AcousticStepStatistics>();
            CreateLayout();
            panel.SuspendLayout();
            panel.Controls.Add(Layout);
            panel.ResumeLayout();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            StepsSelector.TextChanged += StepsSelector_SelectedIndexChanged;
        }

        public void Update(List<JObject> data, MainForm form)
        {
            Clear();
            Files = GetAcousticFiles(data);
            AggregateData();
            PopulateStepsList();
            ShowStatistics();
        }

        public void Update(JObject data, MainForm form)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            StepsSelector.Items.Clear();
            StatisticsTable.ClearAll();
            StepsPlot.Clear();
            if (Files != null) Files.Clear();
            StepsStatistics.Clear();
            StepsSelector.Text = string.Empty;
        }

        public void ClearAll()
        {
            Clear();
        }

        private void CreateLayout()
        {
            // Loi insisted on this
            var rnd = new Random();
            var color = Colors.Blue;
            int chance = rnd.Next(0, 100);
            if (chance == 1) color = Colors.HOTPINK;

            StepsSelector = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = Fonts.Sennheiser.SM
            };

            StatisticsTable = new TableView();
            StepsPlot = new PlotView(GetPlotHeader(), color);

            StatisticsTable.SetColor(color);

            TableLayoutPanel baseLayout = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, 350),
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100)
                },
            };

            baseLayout.SuspendLayout();
            baseLayout.Controls.Add(StepsSelector, 0, 0);
            baseLayout.Controls.Add(StatisticsTable.Layout, 0, 1);
            baseLayout.Controls.Add(StepsPlot.Layout, 1, 1);
            baseLayout.ResumeLayout();

            Layout = baseLayout;
        }

        private string GetPlotHeader()
        {
            return "";
        }

        private List<AcousticFile> GetAcousticFiles(List<JObject> jsonFiles)
        {
            List<AcousticFile> result = new List<AcousticFile>();

            foreach (var jFile in jsonFiles)
            {
                try
                {
                    var f = CommonFileManager.OpenTo<AcousticFile>(jFile);
                    result.Add(f);
                }
                catch (Exception ex)
                {
                    Log.Warn($"Unable to open file. {ex}");
                }
            }

            return result;
        }

        private void AggregateData()
        {
            PassCount = 0;
            FailCount = 0;

            foreach (var file in Files)
            {
                if (file.DUT.Pass) PassCount++;
                if (!file.DUT.Pass) FailCount++;

                foreach (var step in file.Steps)
                {
                    if (!StepsStatistics.ContainsKey(step.StepName))
                    {
                        StepsStatistics.Add(step.StepName, new AcousticStepStatistics());
                        StepsStatistics[step.StepName].Measurements = new List<StepStat>();
                    }

                    var stepStat = StepsStatistics[step.StepName];

                    // Increase count
                    stepStat.Count++;

                    // Increase Pass count if step Passed
                    if (step.StepPass)
                        stepStat.Pass++;

                    // Increase Fail count if step Failed
                    if (!step.StepPass)
                        stepStat.Fail++;

                    // Add measurements
                    stepStat.Measurements.Add(new StepStat
                    {
                        Pass = step.StepPass,
                        Measurements = step.Measurement
                    });

                    if (stepStat.UpperLimit == null)
                    {
                        stepStat.UpperLimit = step.UpperLimit;
                    }

                    if (stepStat.LowerLimit == null)
                    {
                        stepStat.LowerLimit = step.LowerLimit;
                    }
                }
            }
        }

        private void PopulateStepsList()
        {
            var steps = StepsStatistics.Keys;
            StepsSelector.Items.Clear();
            StepsSelector.Items.AddRange(steps.ToArray());
        }

        private void ShowStatistics()
        {
            StatisticsTable.ClearAll();

            StatisticsTable.SetTitle("Statistics");

            StatisticsTable.Table.Columns.Add("Name", "Name");
            StatisticsTable.Table.Columns.Add("Pass", "P");
            StatisticsTable.Table.Columns.Add("Fail", "F");
            StatisticsTable.Table.Columns.Add("Total", "T");
            StatisticsTable.Table.Columns.Add("Yield", "Yield");

            foreach (var stepPair in StepsStatistics)
            {
                if (stepPair.Key == string.Empty) continue;

                string name = stepPair.Key;
                int pass = stepPair.Value.Pass;
                int fail = stepPair.Value.Fail;
                int total = stepPair.Value.Count;
                double yield = pass == 0 ? 0 : Math.Round(((double)pass / total) * 100, 2);
                string yieldPercent = $"{yield}%";
                StatisticsTable.Table.Rows.Add(name, pass, fail, total, yieldPercent);
            }
            double yieldTotal = PassCount == 0 ? 0 : Math.Round(((double)PassCount / Files.Count) * 100, 2);
            StatisticsTable.Table.Rows.Add("TOTAL:", PassCount, FailCount, Files.Count, $"{yieldTotal}%");
            StatisticsTable.Table.AutoResizeColumns();
        }

        private void StepsSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StepsSelector.SelectedItem == null) return;

            StepsPlot.Clear();

            var yeild = Math.Round(((double)PassCount / Files.Count)*100, 2);

            StepsPlot.SetText($"{Files[0].DUT.TypeName}, {Files[0].DUT.TypeID} | {Files[0].DUT.System} | Total: {Files.Count}, Yield: {yeild}%");

            string selectedStep = StepsSelector.SelectedItem.ToString();

            var stepProp = StepsStatistics[selectedStep];

            var plots = new List<ScatterPlot>();

            foreach (var stepStat in stepProp.Measurements)
            {
                if (stepStat.Measurements[0] == null || stepStat.Measurements[0].Length == 0) continue;
                if (stepStat.Measurements[1] == null || stepStat.Measurements[1].Length == 0) continue;

                StepsPlot.ToLog();
                ScatterPlot p;

                if (stepStat.Measurements[0].Length == 1 || stepStat.Measurements[1].Length == 1)
                {
                    p = GetPlot(stepStat.Measurements, stepStat.Color, markerSize: 10);
                    StepsPlot.ToNormal();
                }
                else
                {
                    p = GetPlot(stepStat.Measurements, stepStat.Color, true);
                }

                plots.Add(p);
            }

            if (stepProp.UpperLimit != null && stepProp.UpperLimit.Count > 0)
            {
                var p = GetPlot(stepProp.UpperLimit, Colors.Blue, true, lineWidth: 2);
                plots.Add(p);
            }

            if (stepProp.LowerLimit != null && stepProp.LowerLimit.Count > 0)
            {
                var p = GetPlot(stepProp.LowerLimit, Colors.Blue, true, lineWidth: 2);
                plots.Add(p);
            }

            StepsPlot.AddScatter(plots);
        }

        private double[] ToLog(double[] array)
        {
            return array.Select(x =>
            {
                if (x > 0)
                    return Math.Log10(x);
                else
                    return 0;
            }).ToArray();
        }

        private ScatterPlot GetPlot(List<double[]> xy, Color color, bool log = false, int markerSize = 0, int lineWidth = 1)
        {
            double[] x = xy[0];
            double[] y = xy[1];

            if (x.Length == 1 && y.Length == 1) markerSize = 10;

            if (x[0] == 0 && y[0] == 0) return null;

            if (log)
                x = ToLog(x);

            return new ScatterPlot(x, y)
            {
                Color = color,
                MarkerSize = markerSize,
                LineWidth = lineWidth
            };
        }

        public void Dispose()
        {

        }
    }
}
