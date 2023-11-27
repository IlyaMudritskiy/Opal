﻿using ProcessDashboard.src.Controller.TTLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data.TTLine
{
    public class TTLUnitData
    {
        public int TypeID { get; set; }
        public int Track { get; set; }
        public int Press { get; set; }
        public string LineID { get; set; }
        public Measurements Temperature { get; set; }
        public Measurements HighPressure { get; set; }
        public double HoldPressure { get; set; }
        public double PrePressure { get; set; }
        public double HeaterCurrent { get; set; }
        public Heater Heater { get; set; }
        public List<Feature> TempFeatures { get; set; }
        public List<Feature> PressFeatures { get; set; }
        public List<DataPoint> DataPoints { get; set; }
        

        public TTLUnitData(JsonFile file)
        {
            TempFeatures = new List<Feature>();
            PressFeatures = new List<Feature>();
            TypeID = int.Parse(file.DUT.TypeID);
            Track = int.Parse(file.DUT.TrackNumber);
            Press = int.Parse(file.DUT.PS01PressNumber);
            LineID = file.DUT.MachineID;
            DataPoints = new List<DataPoint>();

            Temperature = new Measurements(file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements);
            HighPressure = new Measurements(file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements);
            HoldPressure = file.Steps.Where(x => x.StepName == "ps01_hold_pressure_actual")
                .FirstOrDefault().Measurements
                .Average(item => double.Parse(item.MeasurementValue));
            PrePressure = file.Steps.Where(x => x.StepName == "ps01_pre_pressure_actual")
                .FirstOrDefault().Measurements
                .Average(item => double.Parse(item.MeasurementValue));
            HeaterCurrent = file.Steps.Where(x => x.StepName == "ps01_heater_current_actual")
                .FirstOrDefault().Measurements
                .Average(item => double.Parse(item.MeasurementValue));
            Heater = new Heater(file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault().Measurements);
            FeatureCalculations.Calculate(this);
        }
    }

    
}