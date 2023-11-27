using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data.TTLine
{
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
