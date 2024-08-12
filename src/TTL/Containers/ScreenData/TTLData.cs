using System.Collections.Generic;
using System.Linq;
using Opal.Model.AppConfiguration;
using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.Misc;

namespace Opal.src.TTL.Containers.ScreenData
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
        public List<DataPointsRow<IValueDescription>> DataPoints { get; set; }
        public List<DataPointsRow<IValueDescription>> TempFeatures { get; set; }
        public List<DataPointsRow<IValueDescription>> PressFeatures { get; set; }
        public List<DataPointsRow<IValueDescription>> StepsStatus { get; set; }

        private Config config = Config.Instance;
        private static readonly object _lock = new object();

        private TTLData(List<TTLUnit> units)
        {
            Units = units;
            PassFailUnits = new PassFailUnits(units);
            Temperature = new ProcessData(units, ProcessStep.Temperature);
            Pressure = new ProcessData(units, ProcessStep.HighPressure);
            DataPoints = new List<DataPointsRow<IValueDescription>>();
            PressFeatures = new List<DataPointsRow<IValueDescription>>();
            TempFeatures = new List<DataPointsRow<IValueDescription>>();
            StepsStatus = new List<DataPointsRow<IValueDescription>>();

            if (config.Acoustic.Enabled)
            {
                FR = new AcousticData(units, ProcessStep.FR);
                THD = new AcousticData(units, ProcessStep.THD);
                RNB = new AcousticData(units, ProcessStep.RNB);
                IMP = new AcousticData(units, ProcessStep.IMP);
            }

            PackDataPointsRows();
        }

        public static TTLData GetInstance(List<TTLUnit> units, bool reload)
        {
            if (!reload)
                if (instance == null)
                    lock (_lock)
                        if (instance == null)
                            instance = new TTLData(units);
            if (reload)
                lock (_lock)
                    instance = new TTLData(units);

            return instance;
        }

        public static TTLData GetInstance()
        {
            return instance;
        }

        private DataPointsRow<IValueDescription> GetDataPointObj(TTLUnit unit)
        {
            return new DataPointsRow<IValueDescription>
            {
                Serial = unit.SerialNumber,
                WPC = unit.WPC,
                Values = unit.Process.DataPoints.Select(
                        x => new IValueDescription
                        {
                            Name = x.Name,
                            Description = x.Description,
                            sValue = x.ToString(),
                        }).ToList()
            };
        }

        private DataPointsRow<IValueDescription> GetFeatureObj(TTLUnit unit, List<Feature> features)
        {
            return new DataPointsRow<IValueDescription>
            {
                Serial = unit.SerialNumber,
                WPC = unit.WPC,
                Values = features.Select(
                        x => new IValueDescription
                        {
                            Name = x.Name,
                            Description = x.Description,
                            sValue = x.ToString(),
                        }).ToList()
            };
        }

        private DataPointsRow<IValueDescription> GetStepsStatusObj(TTLUnit unit)
        {
            if (unit.Acoustic == null) return null;
            var unitSteps = unit.Acoustic.StepsStatus;
            var values = new List<IValueDescription>();

            foreach (var step in unitSteps)
            {
                values.Add(new IValueDescription
                {
                    Name = step.StepName,
                    Description = "",
                    sValue = step.StepPass ? "PASS" : "FAIL",
                    Available = true,
                });
            }
            return new DataPointsRow<IValueDescription>
            {
                Serial = unit.SerialNumber,
                Values = values,
                WPC = unit.WPC
            };
        }

        private void PackDataPointsRows()
        {
            foreach (var unit in Units)
            {
                DataPoints.Add(GetDataPointObj(unit));
                TempFeatures.Add(GetFeatureObj(unit, unit.Process.TempFeatures));
                PressFeatures.Add(GetFeatureObj(unit, unit.Process.PressFeatures));
                StepsStatus.Add(GetStepsStatusObj(unit));
            }
        }

    }
}
