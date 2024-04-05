using ProcessDashboard.src.TTL.Containers.FileContent;
using System;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class JSONPoint
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double DateOffset { get; set; }
        public bool Value { get; set; }

        public JSONPoint(Step step)
        {
            Name = step.StepName;
            Date = DateTime.Parse(step.Measurements[0].DateTime);
            Value = bool.Parse(step.Measurements[0].MeasurementValue);
        }

        public double Offset(DateTime processStart)
        {
            DateOffset = (Date - processStart).TotalSeconds;
            return DateOffset;
        }
    }
}
