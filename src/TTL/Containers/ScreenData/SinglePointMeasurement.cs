using ProcessDashboard.src.TTL.Containers.FileContent;
using System;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class SinglePointMeasurement
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double DateOffset { get; set; }
        public bool BoolValue { get; set; }
        public double Value { get; set; }

        public SinglePointMeasurement(Step step)
        {
            Name = step.StepName;
            Date = DateTime.Parse(step.Measurements[0].DateTime);

            try { BoolValue = bool.Parse(step.Measurements[0].MeasurementValue); }
            catch { BoolValue = false; }

            try { Value = double.Parse(step.Measurements[0].MeasurementValue); }
            catch { Value = double.NaN; }
        }

        public double Offset(DateTime processStart)
        {
            DateOffset = (Date - processStart).TotalSeconds;
            return DateOffset;
        }
    }
}
