using NLog.Fluent;
using ProcessDashboard.src.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data.TTLine.Process
{
    public class TTLProcess
    {
        public Measurements2D HighPressure { get; set; }
        public Measurements2D Temperature { get; set; }
        public Heater Heater { get; set; }

        public List<Feature> TempFeatures { get; set; }
        public List<Feature> PressFeatures { get; set; }
        public List<DataPoint> DataPoints { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public TTLProcess(ProcessFile file)
        {
            TempFeatures = new List<Feature>();
            PressFeatures = new List<Feature>();
            DataPoints = new List<DataPoint>();
            Temperature = new Measurements2D();
            HighPressure = new Measurements2D();

            var temp = file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements;
            var press = file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements;

            Temperature.FromTimeSeries(temp.Select(x => x.DateTime).ToList(), temp.Select(x => x.MeasurementValue).ToList());
            HighPressure.FromTimeSeries(press.Select(x => x.DateTime).ToList(), temp.Select(x => x.MeasurementValue).ToList());
            Heater = new Heater(file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault().Measurements);
        }
    }
}
