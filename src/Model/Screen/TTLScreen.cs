using Newtonsoft.Json.Linq;
using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.AppConfiguration;
using ProcessDashboard.src.Model.Data.TTLine.General;
using ProcessDashboard.src.Model.Data.TTLine.Process;
using ProcessDashboard.src.Model.Misc;
using ProcessDashboard.src.Model.Screen.Acoustic;
using ProcessDashboard.src.Model.TTL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    internal partial class TTLScreen : IScreen
    {
        private static readonly Lazy<TTLScreen> lazy = new Lazy<TTLScreen>(() => new TTLScreen());
        public static TTLScreen Instance => lazy.Value;

        private Config Config = Config.Instance;

        private TabControl Tabs { get; set; }

        private ProcessTab Temperature { get; set; }
        private ProcessTab Pressure { get; set; }

        private AcousticTab FR  { get; set; }
        private AcousticTab THD { get; set; }
        private AcousticTab RNB { get; set; }
        private AcousticTab IMP { get; set; }

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
                FR = new AcousticTab("FR", "Hz", "dB SPL");
                THD = new AcousticTab("THD", "Hz", "%");
                RNB = new AcousticTab("RNB", "Hz", "dB SPL");
                IMP = new AcousticTab("IMP", "Hz", "Ω");

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

            Temperature.AddData(TTLData);
            Pressure.AddData(TTLData);

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
