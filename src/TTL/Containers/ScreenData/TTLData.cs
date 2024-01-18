using System.Collections.Generic;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Misc;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class TTLData
    {
        public ProcessData Temperature { get; set; }
        public ProcessData Pressure { get; set; }

        public AcousticData FR { get; set; }
        public AcousticData THD { get; set; }
        public AcousticData RNB { get; set; }
        public AcousticData IMP { get; set; }

        public List<TTLUnit> Units { get; set; }
        public PassFailUnits PassFailUnits { get; set; }

        private Config config = Config.Instance;

        public TTLData(List<TTLUnit> units)
        {
            Units = units;
            PassFailUnits = new PassFailUnits(units);
            Temperature = new ProcessData(units, ProcessStep.Temperature);
            Pressure = new ProcessData(units, ProcessStep.HighPressure);
            if (config.Acoustic.Enabled)
            {
                FR = new AcousticData(units, ProcessStep.FR);
                THD = new AcousticData(units, ProcessStep.THD);
                RNB = new AcousticData(units, ProcessStep.RNB);
                IMP = new AcousticData(units, ProcessStep.IMP);
            }
        }
    }
}
