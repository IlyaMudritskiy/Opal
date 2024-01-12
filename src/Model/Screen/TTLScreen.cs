using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ProcessDashboard.Controller.FileDataProcessors.TTL;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.Model.Data.TTLine;
using ProcessDashboard.Model.Misc;
using ProcessDashboard.Model.Screen.Tabs;
using ProcessDashboard.Model.TTL.DataContainers;

namespace ProcessDashboard.Model.Screen
{
    internal partial class TTLScreen : IScreen
    {
        private static readonly Lazy<TTLScreen> Lazy = new Lazy<TTLScreen>(() => new TTLScreen());
        public static TTLScreen Instance => Lazy.Value;

        private Config Config = Config.Instance;

        private TabControl Tabs { get; set; }

        private ProcessTab Temperature { get; set; }
        private ProcessTab Pressure { get; set; }

        private AcousticTab FR  { get; set; }
        private AcousticTab THD { get; set; }
        private AcousticTab RNB { get; set; }
        private AcousticTab IMP { get; set; }

        // Tab with other information

        private TTLData TTLData { get; set; }

        public void Create(ref Panel panel)
        {
            Tabs = new TabControl() { Dock = DockStyle.Fill };

            Temperature = new ProcessTab("Temperature", ProcessStep.Temperature);
            Pressure = new ProcessTab("Pressure", ProcessStep.HighPressure);

            Tabs.Controls.Add(Temperature.Tab);
            Tabs.Controls.Add(Pressure.Tab);

            if (Config.Acoustic.Enabled)
            {
                FR = new AcousticTab("FR", "Hz", "dB SPL", ProcessStep.FR);
                THD = new AcousticTab("THD", "Hz", "%", ProcessStep.THD);
                RNB = new AcousticTab("RNB", "Hz", "dB SPL", ProcessStep.RNB);
                IMP = new AcousticTab("IMP", "Hz", "Ω", ProcessStep.IMP);

                Tabs.TabPages.Add(FR.Tab);
                Tabs.TabPages.Add(THD.Tab);
                Tabs.TabPages.Add(RNB.Tab);
                Tabs.TabPages.Add(IMP.Tab);
            }

            panel.SuspendLayout();
            panel.Controls.Add(Tabs);
            panel.ResumeLayout();
        }

        public void Update(ref List<JObject> data)
        {
            Clear();
            LoadData(ref data);
        }

        public void LoadData(ref List<JObject> data)
        {
            List<TTLUnit> processedData = TTLDataProcessor.LoadFiles(data);
            if (TTLData != null)
                TTLData = null;

            TTLData = new TTLData(processedData);

            Temperature.AddData(TTLData, ProcessStep.Temperature);
            Pressure.AddData(TTLData, ProcessStep.HighPressure);

            if (Config.Acoustic.Enabled)
            {
                FR.AddData(TTLData);
                THD.AddData(TTLData);
                RNB.AddData(TTLData);
                IMP.AddData(TTLData);
            }
        }

        public void Clear()
        {
            Temperature.Clear();
            Pressure.Clear();

            if (Config.Acoustic.Enabled)
            {
                FR.Clear();
                THD.Clear();
                RNB.Clear();
                IMP.Clear();
            }

            TTLData = null;
        }
    }
}
