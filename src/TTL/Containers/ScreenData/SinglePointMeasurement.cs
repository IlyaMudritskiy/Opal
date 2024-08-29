using Opal.src.TTL.Containers.FileContent;
using System;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class SinglePointMeasurement
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double DateOffset { get; set; }
        public bool Available { get; set; }
        public double Value { get; set; }

        public SinglePointMeasurement(Step step)
        {
            Name = step.StepName;
            Date = DateTime.Parse(step.Measurements[0].DateTime);

            try { Value = double.Parse(step.Measurements[0].MeasurementValue); }
            catch { Value = double.NaN; }
        }

        public SinglePointMeasurement()
        {
            Available = false;
        }

        public double Offset(DateTime processStart)
        {
            DateOffset = (Date - processStart).TotalSeconds;

            if (DateOffset < 0)
            {
                Available = false;
                DateOffset = -1;
            }
            else
            {
                Available = true;
            }

            return DateOffset;
        }
    }
}
