using ProcessDashboard.src.Model.Data.TTLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProcessDashboard.src.Controller.TTLine
{
    public static class FeatureCalculations
    {
        public static void Calculate(TTLUnitData data)
        {
            if (data == null) return;

            // Constants
            double Tp = 135.0; // Value (temperature)
            double t9_const = 0; // Pressure const
            double P1 = 0; // Value (pressure)
            double tc = 0; // Time 
            int round = 3;

            // Time data points with X and Y values that are used to calculate Features
            DataPoint t2 = new DataPoint() {
                Name = "t2",
                Descritpion = "Time value of Heater turning ON",
                X = Math.Round(data.Heater.On, round),
                Y = Math.Round(data.Temperature.FindPointByTime(data.Heater.On).Y, round)
            };

            DataPoint t4 = new DataPoint()
            {
                Name = "t4",
                Descritpion = "Time value of Heater turning OFF",
                X = Math.Round(data.Heater.Off, round),
                Y = Math.Round(data.Temperature.FindPointByTime(data.Heater.Off).Y, round)
            };

            DataPoint t7 = new DataPoint()
            {
                Name = "t7",
                Descritpion = "Time of pressure intensifier released",
                X = Math.Round(data.HighPressure.TimeOffset[data.HighPressure.MaxValueIndex()], round),
                Y = Math.Round(data.HighPressure.Values[data.HighPressure.MaxValueIndex()], round)
            };

            DataPoint t8 = new DataPoint()
            {
                Name = "t8",
                Descritpion = "t7 + tc",
                X = Math.Round(t7.X + tc, round),
                Y = Math.Round(data.Temperature.FindPointByTime(t7.X + tc).Y, round)
            };

            DataPoint t9 = new DataPoint()
            {
                Name = "t9",
                Descritpion = "Time value of measured temperature equals the pre-defined set-value",
                X = Math.Round(data.Temperature.FindPointByValue(t9_const).X, round),
                Y = Math.Round(t9_const, round)
            };

            DataPoint t10 = new DataPoint()
            {
                Name = "t10",
                Descritpion = "Time value of measured pressure equals the pre-defined pressure value, P1",
                X = Math.Round(data.HighPressure.FindPointByValue(P1).X, round),
                Y = Math.Round(P1, round)
            };

            // Value data points used to calculate features
            DataPoint Tmax = new DataPoint()
            {
                Name = "Tmax",
                Descritpion = "Maximum temperature",
                X = Math.Round(data.Temperature.TimeOffset[data.Temperature.MaxValueIndex()], round),
                Y = Math.Round(data.Temperature.Values[data.Temperature.MaxValueIndex()], round)
            };

            data.DataPoints = new List<DataPoint> { t2, t4, t7, t8, t9, t10, Tmax };


            data.TempFeatures.Add(new Feature()
            {
                ID = "tT2",
                Name = "F1",
                Description = "Time value of Heater turning OFF - Time value of Heater turning ON",
                Value = Math.Round(t4.X - t2.X, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT3",
                Name = "F2",
                Description = "Time of pressure intensifier released - Time value of Heater turning OFF",
                Value = Math.Round(t7.X - t4.X, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT4",
                Name = "F3",
                Description = "t7 + tc - Time of pressure intensifier released",
                Value = Math.Round(t8.X - t7.X, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT5",
                Name = "F4",
                Description = "Time value of measured temperature equals the pre-defined set-value - t7 + tc",
                Value = Math.Round(t9.X - t8.X, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "tT6",
                Name = "F5",
                Description = "Time value of measured pressure equals the pre-defined pressure value, P1 - Time value of measured temperature equals \r\nthe pre-defined set-value",
                Value = Math.Round(t10.X - t9.X, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "Tmax",
                Name = "F6",
                Description = "Maximum Temperature",
                Value = Math.Round(Tmax.Y, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "T0",
                Name = "F7",
                Description = "Temperature value of Heater turning ON",
                Value = Math.Round(t2.Y, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "T1",
                Name = "F8",
                Description = "Temperature value of Heater turning OFF (rising slope)",
                Value = Math.Round(t4.Y, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff1",
                Name = "F9",
                Description = "T1 - T0",
                Value = Math.Round(t4.Y - t2.Y, round)
            });

            data.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff2",
                Name = "F10",
                Description = "Tmax - T1",
                Value = Math.Round(Tmax.Y - t4.Y, round)
            });
        }
    }
}
