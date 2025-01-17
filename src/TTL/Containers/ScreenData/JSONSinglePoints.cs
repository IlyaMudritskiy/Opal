﻿using Opal.src.TTL.Containers.FileContent;
using System.Linq;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class JSONSinglePoints
    {
        // PS01_t2 is Heater = True (on) from old Json
        public SinglePointMeasurement PS01_t3 { get; set; }
        // PS01_t4 is Heater = False (off) from old Json
        public SinglePointMeasurement PS01_t5 { get; set; }
        public SinglePointMeasurement PS01_t6 { get; set; }
        public SinglePointMeasurement PS01_t7 { get; set; }
        public SinglePointMeasurement PS01_t8 { get; set; }
        public SinglePointMeasurement PS01_t9 { get; set; }
        public SinglePointMeasurement PS01_t10 { get; set; }
        public SinglePointMeasurement PS01_HeaterRuntime { get; set; }
        public SinglePointMeasurement PressureStart { get; set; }
        public SinglePointMeasurement TemperatureStart { get; set; }

        public JSONSinglePoints(ProcessFile file)
        {
            PS01_t3 = getPoint(file, "ps01_t3");
            PS01_t5 = getPoint(file, "ps01_t5");
            PS01_t6 = getPoint(file, "ps01_t6");
            PS01_t7 = getPoint(file, "ps01_t7");
            PS01_t8 = getPoint(file, "ps01_t8");
            PS01_t9 = getPoint(file, "ps01_t9");
            PS01_t10 = getPoint(file, "ps01_t10");
            PS01_HeaterRuntime = getPoint(file, "ps01_heater_runtime");
            PressureStart = getPoint(file, "ps01_high_pressure_actual");
            TemperatureStart = getPoint(file, "ps01_temperature_actual");
        }

        public void SetOffset()
        {
            PS01_t3.Offset(TemperatureStart.Date);
            PS01_t5.Offset(PressureStart.Date);
            PS01_t6.Offset(PressureStart.Date);
            PS01_t7.Offset(PressureStart.Date);
            PS01_t8.Offset(TemperatureStart.Date);
            PS01_t9.Offset(TemperatureStart.Date);
            PS01_t10.Offset(PressureStart.Date);
            PS01_HeaterRuntime.Offset(TemperatureStart.Date);
        }

        private SinglePointMeasurement getPoint(ProcessFile file, string stepname)
        {
            var step = file.Steps.Where(x => x.StepName == stepname).FirstOrDefault();

            if (step == null)
                return new SinglePointMeasurement();

            return new SinglePointMeasurement(step);
        }
    }
}
