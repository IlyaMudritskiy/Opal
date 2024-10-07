using System;
using System.Collections.Generic;
using System.Linq;
using Opal.Model.AppConfiguration;
using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.Misc;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class TTLData
    {
        private static readonly Lazy<TTLData> _instance = new Lazy<TTLData>(() => new TTLData());
        public static TTLData Instance => _instance.Value;

        private Config _config = Config.Instance;

        public ProcessData Temperature { get; set; }
        public ProcessData Pressure { get; set; }

        public AcousticData FR { get; set; }
        public AcousticData THD { get; set; }
        public AcousticData RNB { get; set; }
        public AcousticData IMP { get; set; }

        //public List<TTLUnit> Units { get; set; }

        public List<DataPointsRow<IValueDescription>> DataPoints { get; set; }
        public List<DataPointsRow<IValueDescription>> TempFeatures { get; set; }
        public List<DataPointsRow<IValueDescription>> PressFeatures { get; set; }
        public List<DataPointsRow<IValueDescription>> StepsStatus { get; set; }

        //private static readonly object _lock = new object();

        private TTLData() {
            Temperature = new ProcessData();
            Pressure = new ProcessData();
            DataPoints = new List<DataPointsRow<IValueDescription>>();
            PressFeatures = new List<DataPointsRow<IValueDescription>>();
            TempFeatures = new List<DataPointsRow<IValueDescription>>();
            StepsStatus = new List<DataPointsRow<IValueDescription>>();
        }

        public void AddData(List<TTLUnit> units)
        {
            //Units = units;
            Temperature.AddData(units, ProcessStep.Temperature);

            Pressure.AddData(units, ProcessStep.HighPressure);

            if (_config.Acoustic.Enabled)
            {
                FR = new AcousticData(units, ProcessStep.FR);
                THD = new AcousticData(units, ProcessStep.THD);
                RNB = new AcousticData(units, ProcessStep.RNB);
                IMP = new AcousticData(units, ProcessStep.IMP);
            }

            PackDataPointsRows(units);
        }

        public void UpdateUnit(TTLUnit unit)
        {
            Temperature.UpdateData(unit, ProcessStep.Temperature);
            Pressure.UpdateData(unit, ProcessStep.HighPressure);
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

        private void PackDataPointsRows(List<TTLUnit> units)
        {
            if (units == null || units.Count == 0) return;

            foreach (var unit in units)
            {
                DataPoints.Add(GetDataPointObj(unit));
                TempFeatures.Add(GetFeatureObj(unit, unit.Process.TempFeatures));
                PressFeatures.Add(GetFeatureObj(unit, unit.Process.PressFeatures));
                StepsStatus.Add(GetStepsStatusObj(unit));
            }
        }
    }
}
