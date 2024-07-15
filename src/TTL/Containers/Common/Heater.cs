using ProcessDashboard.src.TTL.Containers.FileContent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class Heater
    {
        public double On { get; set; }
        public double Off { get; set; }

        public DateTime _on { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Heater(List<Measurement> measurements)
        {
            try
            {
                DateTime on = DateTime.Parse(measurements.FirstOrDefault(x => x.MeasurementValue == "True").DateTime);
                DateTime off = DateTime.Parse(measurements.FirstOrDefault(x => x.MeasurementValue == "False").DateTime);
                _on = on;

                On = 0;
                Off = (off - on).TotalSeconds;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}
