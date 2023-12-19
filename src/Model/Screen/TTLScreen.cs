using Newtonsoft.Json.Linq;
using ProcessDashboard.src.Controller.FileDataProcessors.TTL;
using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.AppConfiguration;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen.Acoustic;
using ProcessDashboard.src.Model.Screen.TTLine;
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

        private ProcessTab TemperatureTab { get; set; }
        private ProcessTab PressureTab { get; set; }

        private AcousticTab FR  { get; set; }
        private AcousticTab THD { get; set; }
        private AcousticTab RNB { get; set; }
        private AcousticTab IMP { get; set; }

        public void Create(ref Panel panel)
        {
            Tabs = new TabControl() { Dock = DockStyle.Fill };

            TemperatureTab = new ProcessTab();
            PressureTab = new ProcessTab();

            Tabs.Controls.Add(TemperatureTab.Tab);
            Tabs.Controls.Add(PressureTab.Tab);

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

        public void LoadData(ref List<JObject> files)
        {
            List<ProcessFile> jsonFiles = TTLDataProcessor.LoadFiles(files);
            // Get process files
            // Get acoustic files
            // Process them and get list of unified ibjects with all data
        }

        public void Update(ref List<ProcessFile> data)
        {
            throw new NotImplementedException();
        }
    }
}
