using System;
using System.Collections.Generic;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Misc;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class TTLData
    {
        private static TTLData instance = null;

        public ProcessData Temperature { get; set; }
        public ProcessData Pressure { get; set; }

        public AcousticData FR { get; set; }
        public AcousticData THD { get; set; }
        public AcousticData RNB { get; set; }
        public AcousticData IMP { get; set; }

        public List<TTLUnit> Units { get; set; }
        public PassFailUnits PassFailUnits { get; set; }
        public List<DataPointsRow> DataPoints { get; set; }

        private Config config = Config.Instance;

        private TTLData(List<TTLUnit> units)
        {
            Units = units;
            PassFailUnits = new PassFailUnits(units);
            Temperature = new ProcessData(units, ProcessStep.Temperature);
            Pressure = new ProcessData(units, ProcessStep.HighPressure);
            DataPoints = new List<DataPointsRow>();
            if (config.Acoustic.Enabled)
            {
                FR = new AcousticData(units, ProcessStep.FR);
                THD = new AcousticData(units, ProcessStep.THD);
                RNB = new AcousticData(units, ProcessStep.RNB);
                IMP = new AcousticData(units, ProcessStep.IMP);
            }

            foreach (var unit in Units)
                DataPoints.Add(new DataPointsRow
                {
                    Serial = unit.SerialNumber,
                    Values = unit.Process.DataPoints
                });
        }

        public static TTLData GetInstance(List<TTLUnit> units)
        {
            if (instance == null)
                instance = new TTLData(units);
            return instance;
        }

        public static TTLData GetInstance()
        {
            return instance;
        }
    }
}
