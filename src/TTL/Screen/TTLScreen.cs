using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Opal.Model.AppConfiguration;
using Opal.Model.Screen.Tabs;
using Opal.src.TTL.Containers.ScreenData;
using Opal.src.TTL.Processing;
using ProcessDashboard.src.CommonClasses.SreenProvider;

namespace Opal.src.TTL.Screen
{
    public partial class TTLScreen : IScreen
    {
        private static readonly Lazy<TTLScreen> Lazy = new Lazy<TTLScreen>(() => new TTLScreen());
        public static TTLScreen Instance => Lazy.Value;

        private Config Config = Config.Instance;

        private TabControl Tabs { get; set; }

        private ProcessTab Temperature { get; set; }
        private ProcessTab Pressure { get; set; }

        private AcousticTab FR { get; set; }
        private AcousticTab THD { get; set; }
        private AcousticTab RNB { get; set; }
        private AcousticTab IMP { get; set; }

        private TTLData TTLData { get; set; }

        public void Show(ref Panel panel)
        {
            Tabs = new TabControl() { Dock = DockStyle.Fill };

            Temperature = new ProcessTab("Temperature");
            Pressure = new ProcessTab("Pressure");

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

                FR.SubscribeToDSNestToggleButtonClick(FRToggleView);
                THD.SubscribeToDSNestToggleButtonClick(THDToggleView);
                RNB.SubscribeToDSNestToggleButtonClick(RNBToggleView);
                IMP.SubscribeToDSNestToggleButtonClick(IMPToggleView);
            }

            panel.SuspendLayout();
            panel.Controls.Add(Tabs);
            panel.ResumeLayout();
        }

        public void Update(List<JObject> data)
        {
            List<TTLUnit> processedData = new List<TTLUnit>();

            Task.Run(async () =>
            {
                processedData = await TTLDataProcessor.LoadFiles(data);
            }).Wait();

            if (processedData == null) return;

            Clear();

            TTLData = TTLData.GetInstance(processedData, true);

            if (TTLData == null) return;

            Temperature.AddData(TTLData.Temperature);
            Pressure.AddData(TTLData.Pressure);

            if (Config.Acoustic.Enabled)
            {
                FR.AddData(TTLData.FR);
                THD.AddData(TTLData.THD);
                RNB.AddData(TTLData.RNB);
                IMP.AddData(TTLData.IMP);
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

        private void FRToggleView(object sender, EventArgs e)
        {
            FR.ShowPlots();
        }

        private void THDToggleView(object sender, EventArgs e)
        {
            THD.ShowPlots();
        }

        private void RNBToggleView(object sender, EventArgs e)
        {
            RNB.ShowPlots();
        }

        private void IMPToggleView(object sender, EventArgs e)
        {
            IMP.ShowPlots();
        }
    }
}
