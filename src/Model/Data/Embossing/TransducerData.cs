using ProcessDashboard.src.Controller.Embossing;
using ProcessDashboard.src.Model.Data.Embossing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data
{
    public class TransducerData
    {
        public int Track { get; set; }
        public int Press { get; set; }
        public Measurements Temperature { get; set; }
        public Measurements HighPressure { get; set; }
        public Measurements HoldPressure { get; set; }
        public Heater Heater { get; set; }
        public List<Feature> TempFeatures { get; set; }
        public List<Feature> PressFeatures { get; set; }

        public TransducerData(JsonFile file)
        {
            TempFeatures = new List<Feature>();
            PressFeatures = new List<Feature>();
            Track = int.Parse(file.DUT.TrackNumber);
            Press = int.Parse(file.DUT.PS01PressNumber);

            Temperature = new Measurements(file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements);
            HighPressure = new Measurements(file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements);
            HoldPressure = new Measurements(file.Steps.Where(x => x.StepName == "ps01_hold_pressure_actual").FirstOrDefault().Measurements);
            Heater = new Heater(file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault().Measurements);
            EmbossingCalculations.CalculateFeatures(this);
        }
    }

    public class Heater
    {
        public double On { get; set; }
        public double Off { get; set; }

        public Heater(List<Measurement> measurements)
        {
            DateTime on = DateTime.Parse(measurements.FirstOrDefault(x => x.MeasurementValue == "True").DateTime);
            DateTime off = DateTime.Parse(measurements.FirstOrDefault(x => x.MeasurementValue == "False").DateTime);

            On = 0;
            Off = (off - on).TotalSeconds;
        }
    }
}
