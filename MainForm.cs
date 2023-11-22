using ProcessDashboard.src.Data;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen;
using ProcessDashboard.src.Utils.Design;
using ProcessDashboard.src.View.Embossing;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard
{
    public partial class MainForm : Form
    {
        private EmbossingScreen screen;
        public MainForm()
        {
            InitializeComponent();
            _createTableLayout();
        }

        private void _createTableLayout()
        {
            screen = new EmbossingScreen(ref MainFormPanel);
        }

        private void SelectJsonFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rawData = EmbossingDataProcessor.OpenFiles(JsonFileDialog);

            List<TransducerData> DS11 = rawData.Where(x => x.Track == 1 && x.Press == 1).ToList();
            List<TransducerData> DS12 = rawData.Where(x => x.Track == 1 && x.Press == 2).ToList();
            List<TransducerData> DS21 = rawData.Where(x => x.Track == 2 && x.Press == 1).ToList();
            List<TransducerData> DS22 = rawData.Where(x => x.Track == 2 && x.Press == 2).ToList();

            if (DS11.Count != 0)
                foreach (var feature in DS11[0].TempFeatures)
                    screen.Temperature.DS11.DataSource.Add(feature);

            if (DS12.Count != 0)
                foreach (var feature in DS12[0].TempFeatures)
                    screen.Temperature.DS12.DataSource.Add(feature);

            if (DS21.Count != 0)
                foreach (var feature in DS21[0].TempFeatures)
                    screen.Temperature.DS21.DataSource.Add(feature);

            if (DS22.Count != 0)
                foreach (var feature in DS22[0].TempFeatures)
                    screen.Temperature.DS22.DataSource.Add(feature);

            screen.Temperature.Plot.Plot.AddHorizontalLine(135.0, color: Colors.Black);

            if (DS11.Count != 0)
            {
                screen.Temperature.Plot.Plot.AddScatter(
                    DS11[0].Temperature.TimeOffset,
                    DS11[0].Temperature.Values,
                    Colors.Orange,
                    markerSize: 5,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS11");
                screen.Temperature.Plot.Plot.AddVerticalLine(DS11[0].TempFeatures[0].Value, color: Colors.Orange, width: 2, LineStyle.Dot);

                screen.Pressure.Plot.Plot.AddScatter(
                    DS11[0].HighPressure.TimeOffset,
                    DS11[0].HighPressure.Values,
                    Colors.Orange,
                    markerSize: 4,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS11");
            }

            if (DS12.Count != 0)
            {
                screen.Temperature.Plot.Plot.AddScatter(
                    DS12[0].Temperature.TimeOffset,
                    DS12[0].Temperature.Values,
                    Colors.Cyan,
                    markerSize: 5,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS12");
                screen.Temperature.Plot.Plot.AddVerticalLine(DS12[0].TempFeatures[0].Value, color: Colors.Cyan, width: 2, LineStyle.Dot);

                screen.Pressure.Plot.Plot.AddScatter(
                    DS12[0].HighPressure.TimeOffset,
                    DS12[0].HighPressure.Values,
                    Colors.Cyan,
                    markerSize: 4,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS12");
            }

            if (DS21.Count != 0)
            {
                screen.Temperature.Plot.Plot.AddScatter(
                    DS21[0].Temperature.TimeOffset,
                    DS21[0].Temperature.Values,
                    Colors.Blue,
                    markerSize: 5,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS21");
                screen.Temperature.Plot.Plot.AddVerticalLine(DS21[0].TempFeatures[0].Value, color: Colors.Blue, width: 2, LineStyle.Dot);

                screen.Pressure.Plot.Plot.AddScatter(
                    DS21[0].HighPressure.TimeOffset,
                    DS21[0].HighPressure.Values,
                    Colors.Blue,
                    markerSize: 4,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS21");
            }

            if (DS22.Count != 0)
            {
                screen.Temperature.Plot.Plot.AddScatter(
                    DS22[0].Temperature.TimeOffset,
                    DS22[0].Temperature.Values,
                    Colors.Purple,
                    markerSize: 5,
                    //lineStyle: ScottPlot.LineStyle.Solid,
                    lineWidth: 2,
                    label: "DS22");
                screen.Temperature.Plot.Plot.AddVerticalLine(DS22[0].TempFeatures[0].Value, color: Colors.Purple, width: 2, LineStyle.Dot);

                screen.Pressure.Plot.Plot.AddScatter(
                   DS22[0].HighPressure.TimeOffset,
                   DS22[0].HighPressure.Values,
                   Colors.Purple,
                   markerSize: 4,
                   //lineStyle: ScottPlot.LineStyle.Solid,
                   lineWidth: 2,
                   label: "DS22");
            }

            var legend = screen.Temperature.Plot.Plot.Legend();
            legend.Location = ScottPlot.Alignment.UpperRight;
            screen.Temperature.Plot.Refresh();

            screen.Temperature.DS11.Title.BackColor = Colors.Orange;
            screen.Temperature.DS12.Title.BackColor = Colors.Cyan;
            screen.Temperature.DS21.Title.BackColor = Colors.Blue;
            screen.Temperature.DS22.Title.BackColor = Colors.Purple;

            // ===========================================================================

            var legend2 = screen.Pressure.Plot.Plot.Legend();
            legend2.Location = ScottPlot.Alignment.UpperRight;
            screen.Pressure.Plot.Refresh();

            screen.Pressure.DS11.Title.BackColor = Colors.Orange;
            screen.Pressure.DS12.Title.BackColor = Colors.Cyan;
            screen.Pressure.DS21.Title.BackColor = Colors.Blue;
            screen.Pressure.DS22.Title.BackColor = Colors.Purple;

        }

        private void selectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select a folder
        }
    }
}
