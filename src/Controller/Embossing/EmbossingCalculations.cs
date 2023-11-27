using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Data.Embossing;
using System;

namespace ProcessDashboard.src.Controller.Embossing
{
    public static class EmbossingCalculations
    {
        public static void CalculateFeatures(TransducerData data)
        {
            if (data == null) return;

            double Tp = 135.0;
            double t9_const = 0;
            double P1 = 0;
            double tc = 0;

            double t2 = data.Heater.On;
            //double t3 = data.Temperature.ValueToTime(Tp);
            double t4 = data.Heater.Off;

            double T0 = data.Temperature.TimeToValue(t2);
            double T1 = data.Temperature.TimeToValue(t4);

            int maxTempIdx = data.Temperature.MaxValueIndex();
            double Tmax = data.Temperature.Values[maxTempIdx];
            double tmax = data.Temperature.TimeOffset[maxTempIdx];

            // PIR - Pressure Intensifier Release
            // t7p - PIR in HighPressure data array
            double t7p = data.HighPressure.TimeOffset[data.HighPressure.MaxValueIndex()];
            // t7t - PIR in TemperatureActual data array
            double t7t = data.Temperature.FindClosestValue(t7p, data.Temperature.TimeOffset);

            double t8 = t7p + tc;
            double t9 = data.Temperature.ValueToTime(t9_const);
            double t10 = data.HighPressure.ValueToTime(P1);


            data.TempFeatures.Add(new Feature()
            {
                ID = "tT2",
                Name = "F1",
                Description = "Time value of Heater turning OFF - Time value of Heater turning ON",
                Value = Math.Round(t4 - t2, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT3p",
                Name = "F2",
                Description = "Time of pressure intensifier released - Time value of Heater turning OFF",
                Value = Math.Round(t7p - t4, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT3t",
                Name = "F2",
                Description = "Time of pressure intensifier released - Time value of Heater turning OFF",
                Value = Math.Round(t7t - t4, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT4",
                Name = "F3",
                Description = "t7 + tc - Time of pressure intensifier released",
                Value = Math.Round(t8 - t7p, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT5",
                Name = "F4",
                Description = "Time value of measured temperature equals the pre-defined set-value - t7 + tc",
                Value = Math.Round(t9 - t8, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT6",
                Name = "F5",
                Description = "Time value of measured pressure equals the pre-defined pressure value, P1 - Time value of measured temperature equals \r\nthe pre-defined set-value",
                Value = Math.Round(t10 - t9, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "Tmax",
                Name = "F6",
                Description = "Maximum Temperature",
                Value = Math.Round(Tmax, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "T0",
                Name = "F7",
                Description = "Temperature value of Heater turning ON",
                Value = Math.Round(T0, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "T1",
                Name = "F8",
                Description = "Temperature value of Heater turning OFF (rising slope)",
                Value = Math.Round(T1, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff1",
                Name = "F9",
                Description = "T1 - T0",
                Value = Math.Round(T1 - T0, 5)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff2",
                Name = "F10",
                Description = "Tmax - T1",
                Value = Math.Round(Tmax - T1, 5)
            });
        }

        private static void temperatureFeatures(TransducerData data)
        {

        }
    }
}
