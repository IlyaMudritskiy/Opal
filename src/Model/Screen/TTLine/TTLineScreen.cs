using ProcessDashboard.src.Controller.Acoustic;
using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Data.Acoustic;
using ProcessDashboard.src.Model.Data.TTLine;
using ProcessDashboard.src.Model.Screen.Acoustic;
using ProcessDashboard.src.Model.Screen.Elements;
using ProcessDashboard.src.Utils.Design;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.TTLine
{
    public class TTLineScreen : IScreen
    {
        private static readonly Lazy<TTLineScreen> lazy =
        new Lazy<TTLineScreen>(() => new TTLineScreen());
        public static TTLineScreen Instance => lazy.Value;

        private TabControl Tabs { get; set; }

        // Process data tabs
        private TTLineTab Temperature { get; set; }
        private TTLineTab Pressure { get; set; }

        // Acoustic data tabs
        private AcousticTab FR { get; set; }
        private AcousticTab THD { get; set; }
        private AcousticTab RNB { get; set; }
        private AcousticTab IMP { get; set; }

        // Data
        private ScreenData ScreenData { get; set; }

        private string lineID;
        private string typeID;

        public void Create(ref Panel panel)
        {
            Tabs = new TabControl() { Dock = DockStyle.Fill };

            createTabs();
            addTabs();

            panel.SuspendLayout();
            panel.Controls.Add(Tabs);
            panel.ResumeLayout();
        }

        public void Update(ref List<JsonFile> files)
        {
            clear(Temperature);
            clear(Pressure);
            FR.Clear();
            THD.Clear();
            RNB.Clear();
            IMP.Clear();
            LoadData(ref files);
        }

        public void LoadData(ref List<JsonFile> files)
        {
            // Transform JsonFile to ready-to-use line object
            List<TTLUnitData> processData = TTLineDataProcessor.LoadFiles(files);
            // Find and open acoustic files
            List<AcousticFile> acousticData = AcousticDataProcessor.OpenFiles(ref files, files[0].DUT.TypeID);

            ScreenData = new ScreenData(processData, acousticData);

            fillData();
            renameHeaders();
            addLimits();
        }

        private void addLimits()
        {
            var limits = AcousticDataProcessor.OpenLimitFiles();

            FR.AddLimits(
                upper: limits["FRUpper"],
                lower: limits["FRLower"],
                reference: limits["FRReference"]
                );

            THD.AddLimits(
                upper: limits["THDUpper"],
                lower: limits["THDLower"],
                reference: limits["THDReference"]
                );

            RNB.AddLimits(
                upper: limits["RNBUpper"],
                lower: limits["RNBLower"],
                reference: limits["RNBReference"]
                );

            IMP.AddLimits(
                upper: limits["IMPUpper"],
                lower: limits["IMPLower"],
                reference: limits["IMPReference"]
                );
        }

        private void createTabs()
        {
            Temperature = new TTLineTab("Temperature Details");
            Pressure = new TTLineTab("Pressure Details");
            FR = new AcousticTab("FR", "Hz", "dB SPL");
            THD = new AcousticTab("THD", "Hz", "%");
            RNB = new AcousticTab("RNB", "Hz", "dB SPL");
            IMP = new AcousticTab("IMP", "Hz", "Ω");
        }

        private void addTabs()
        {
            Tabs.TabPages.Add(Temperature.Tab);
            Tabs.TabPages.Add(Pressure.Tab);
            Tabs.TabPages.Add(FR.Tab);
            Tabs.TabPages.Add(THD.Tab);
            Tabs.TabPages.Add(RNB.Tab);
            Tabs.TabPages.Add(IMP.Tab);
        }

        private void renameHeaders()
        {
            Temperature.Title.Text = $"Temperature  |  {lineID} - {typeID}";
            Pressure.Title.Text = $"Pressure  |  {lineID} - {typeID}";
            FR.Title.Text = $"Frequency Response  |  {lineID} - {typeID}";
            THD.Title.Text = $"Total Harmonic Distortion  |  {lineID} - {typeID}";
            RNB.Title.Text = $"Rub and Buzz  |  {lineID} - {typeID}";
            IMP.Title.Text = $"Impedance  |  {lineID} - {typeID}";
        }

        private void fillData()
        {
            addTempData(ScreenData.DS11, Temperature.DS11, Colors.DS11C, "DS 1-1");
            addTempData(ScreenData.DS12, Temperature.DS12, Colors.DS12C, "DS 1-2");
            addTempData(ScreenData.DS21, Temperature.DS21, Colors.DS21C, "DS 2-1");
            addTempData(ScreenData.DS22, Temperature.DS22, Colors.DS22C, "DS 2-2");

            addPressData(ScreenData.DS11, Pressure.DS11, Colors.DS11C, "DS 1-1");
            addPressData(ScreenData.DS12, Pressure.DS12, Colors.DS12C, "DS 1-2");
            addPressData(ScreenData.DS21, Pressure.DS21, Colors.DS21C, "DS 2-1");
            addPressData(ScreenData.DS22, Pressure.DS22, Colors.DS22C, "DS 2-2");

            addAcousticData(FR, "freq");
            addAcousticData(THD, "thd");
            addAcousticData(RNB, "rnb");
            addAcousticData(IMP, "imp");
        }

        private void addTempData(DSXXData ds, TableView tv, Color color, string label)
        {
            if (ds == null) return;

            if (ds.Temperature.Count != 0)
            {
                tv.AddData(ds.TempFeaturesMean, color, ds.Amount);

                foreach (var item in ds.Temperature)
                    Temperature.AddScatter(item.TimeOffset, item.Values, color, label);
            }

            lineID = ds.LineID;
            typeID = ds.TypeID.ToString();
        }

        private void addPressData(DSXXData ds, TableView tv, Color color, string label)
        {
            if (ds == null) return;

            if (ds.Pressure.Count != 0)
            {
                tv.AddData(ds.PressFeaturesMean, color, ds.Amount);

                foreach (var item in ds.Pressure)
                    Pressure.AddScatter(item.TimeOffset, item.Values, color, label);
            }

            lineID = ds.LineID;
            typeID = ds.TypeID.ToString();
        }

        private void addAcousticData(AcousticTab tab, string name)
        {
            setAcousticData(tab, ScreenData.DS11, Colors.DS11C, name, "DS 1-1");
            setAcousticData(tab, ScreenData.DS12, Colors.DS12C, name, "DS 1-2");
            setAcousticData(tab, ScreenData.DS21, Colors.DS21C, name, "DS 2-1");
            setAcousticData(tab, ScreenData.DS22, Colors.DS22C, name, "DS 2-2");
        }

        private void setAcousticData(AcousticTab tab, DSXXData data, Color color, string stepname, string label)
        {
            if (data == null || data.AcousticFiles == null) return;
            foreach (var file in data.AcousticFiles)
            {
                string ds = $"DS{data.Track}{data.Press}";
                var step = file.Steps.Where(x => x.StepName == stepname).FirstOrDefault();
                tab.AddScatter(step.Measurement[0], step.Measurement[1], Colors.Grey, label);
            }
        }

        private void clear(TTLineTab tab)
        {
            tab.Plot.Plot.Clear(typeof(ScatterPlot));
            clearTable(tab.DS11);
            clearTable(tab.DS12);
            clearTable(tab.DS21);
            clearTable(tab.DS22);
        }

        private void clearTable(TableView table)
        {
            table.DataSource.Clear();
            table.Table.Refresh();
        }
    }
}
