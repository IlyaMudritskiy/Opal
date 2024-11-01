using System;
using System.Collections.Generic;
using System.Linq;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Containers;
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

        private List<TTLUnit> _units;

        private TTLData()
        {
            Temperature = new ProcessData();
            Pressure = new ProcessData();
        }

        public void AddData(List<TTLUnit> units)
        {
            _units = units;
            Temperature.AddData(units, ProcessStep.Temperature);
            Pressure.AddData(units, ProcessStep.HighPressure);

            if (_config.Acoustic.Enabled)
            {
                FR = new AcousticData(units, ProcessStep.FR);
                THD = new AcousticData(units, ProcessStep.THD);
                RNB = new AcousticData(units, ProcessStep.RNB);
                IMP = new AcousticData(units, ProcessStep.IMP);
            }
        }

        public void UpdateUnit(TTLUnit unit)
        {
            Temperature.UpdateData(unit, ProcessStep.Temperature);
            Pressure.UpdateData(unit, ProcessStep.HighPressure);
        }

        public Dictionary<string, TableDataContainer> GetDataViewerFormat()
        {
            if (_units == null || _units.Count == 0) return null;

            var result = new Dictionary<string, TableDataContainer>
            {
                { "Data Points", DataToDataViewer(unit => unit.Process.DataPoints, x => x.Name, x => $"{x.X} {x.UnitX}, {x.Y} {x.UnitY}") },
                { "Temperature Features", DataToDataViewer(unit => unit.Process.TempFeatures, x => x.Name, x => $"{x.Value}") },
                { "Pressure Features", DataToDataViewer(unit => unit.Process.PressFeatures, x => x.Name, x => $"{x.Value}") }
            };

            if (_config.Acoustic.Enabled)
            {
                result.Add("Acoustic Steps", DataToDataViewer(unit => unit.Acoustic.StepsStatus, x => x.StepName, x => x.StepPass ? "PASS" : "FAIL"));
            }

            return result;
        }

        public void Clear()
        {
            Temperature = new ProcessData();
            Pressure = new ProcessData();
            FR = null;
            THD = null;
            RNB = null;
            IMP = null;
            _units = null;
            _config = Config.Instance;
        }

        private TableDataContainer DataToDataViewer<T>(
            Func<TTLUnit, IEnumerable<T>> dataSourceSelector,
            Func<T, string> nameRepr,
            Func<T, string> valueRepr)
        {
            var firstUnit = _units[0];
            var headers = new List<string>
            {
                "Serial",
                "WPC"
            };

            headers.AddRange(dataSourceSelector(firstUnit).Select(nameRepr).ToList());

            var featuresValues = new List<List<string>>();

            foreach (var unit in _units)
            {
                var unitValues = new List<string>
                {
                    unit.SerialNumber,
                    unit.WPC
                };

                unitValues.AddRange(dataSourceSelector(unit).Select(valueRepr));
                featuresValues.Add(unitValues);
            }

            return new TableDataContainer(headers, featuresValues);
        }
    }
}
