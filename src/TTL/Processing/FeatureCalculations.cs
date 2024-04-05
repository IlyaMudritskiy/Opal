using System;
using System.Collections.Generic;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;

namespace ProcessDashboard.src.TTL.Processing
{
    public static class FeatureCalculations
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private static Config config = Config.Instance;

        public static void Calculate(TTLProcess unit)
        {
            if (unit == null) return;

            // Constants
            int round = config.EmbossingConstants.RoundTo;
            unit.JsonPoints.SetOffset(unit.Heater._on);

            #region DataPoints

            DataPoint t2 = new DataPoint() // Correct
            {
                Name = "t2",
                Descritpion = "Time value of Heater turning ON",
                X = Math.Round(unit.Heater.On, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.Heater.On).Y, round)
            };

            DataPoint t3 = new DataPoint() // Reworked
            {
                Name = "t3",
                Descritpion = "Time value of measured temperature equals the pre-defined set-value, TD.",
                X = Math.Round(unit.JsonPoints.PS01_t3.DateOffset, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.JsonPoints.PS01_t3.DateOffset).Y, round)
            };

            DataPoint t4 = new DataPoint() // Correct
            {
                Name = "t4",
                Descritpion = "Time value of Heater turning OFF",
                X = Math.Round(unit.Heater.Off, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.Heater.Off).Y, round)
            };

            DataPoint t5 = new DataPoint()
            {
                Name = "t5",
                Descritpion = "t3 + tHP",
                X = Math.Round(unit.JsonPoints.PS01_t5.DateOffset, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t5.DateOffset).Y, round)
            };

            DataPoint t6 = new DataPoint()
            {
                Name = "t6",
                Descritpion = "Maximum Pressure",
                X = Math.Round(unit.JsonPoints.PS01_t6.DateOffset, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t6.DateOffset).Y, round)
            };

            DataPoint t7 = new DataPoint()
            {
                Name = "t7",
                Descritpion = "Time of pressure intensifier released",
                X = Math.Round(unit.JsonPoints.PS01_t7.DateOffset, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t7.DateOffset).Y, round)
            };

            DataPoint t8 = new DataPoint()
            {
                Name = "t8",
                Descritpion = "t7 + tc",
                X = Math.Round(unit.JsonPoints.PS01_t8.DateOffset, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.JsonPoints.PS01_t8.DateOffset).Y, round)
            };

            DataPoint t9 = new DataPoint()
            {
                Name = "t9",
                Descritpion = "Time value of measured temperature equals the pre-defined set-value",
                X = Math.Round(unit.JsonPoints.PS01_t9.DateOffset, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.JsonPoints.PS01_t9.DateOffset).Y, round)
            };

            DataPoint t10 = new DataPoint()
            {
                Name = "t10",
                Descritpion = "Time value of measured pressure equals the pre-defined pressure value, P1",
                X = Math.Round(unit.JsonPoints.PS01_t10.DateOffset, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t10.DateOffset).Y, round)
            };

            DataPoint Tmax = new DataPoint()
            {
                Name = "Tmax",
                Descritpion = "Maximum temperature",
                X = Math.Round(unit.Temperature.MaxPointX(), round),
                Y = Math.Round(unit.Temperature.MaxPointY(), round)
            };

            DataPoint P3 = new DataPoint()
            {
                Name = "P3",
                Descritpion = "Pressure Value at (t6 + tsettle)",
                X = Math.Round(t6.X + config.EmbossingConstants.tsettle, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(t6.X + config.EmbossingConstants.tsettle).Y, round)
            };

            DataPoint P4 = new DataPoint()
            {
                Name = "P4",
                Descritpion = "Pressure Value at t9",
                X = t9.X,
                Y = Math.Round(unit.HighPressure.FindPointByX(t9.X).Y, round)
            };

            // Value data points used to calculate features
            unit.DataPoints = new List<DataPoint> { t2, t3, t4, t5, t6, t7, t8, t9, t10, Tmax, P3, P4 };

            #endregion

            #region TemperatureFeatures
            // Features
            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT2",
                Name = "TF1",
                Description = "Time value of Heater turning OFF - Time value of Heater turning ON",
                Value = Math.Round(t4.X - t2.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT3",
                Name = "TF2",
                Description = "Time of pressure intensifier released - Time value of Heater turning OFF",
                Value = Math.Round(t7.X - t4.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT4",
                Name = "TF3",
                Description = "t7 + tc - Time of pressure intensifier released",
                Value = Math.Round(t8.X - t7.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT5",
                Name = "TF4",
                Description = "Time value of measured temperature equals the pre-defined set-value - t7 + tc",
                Value = Math.Round(t9.X - t8.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT6",
                Name = "TF5",
                Description = "Time value of measured pressure equals the pre-defined pressure value, P1 - Time value of measured temperature equals \r\nthe pre-defined set-value",
                Value = Math.Round(t10.X - t9.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "Tmax",
                Name = "TF6",
                Description = "Maximum Temperature",
                Value = Math.Round(Tmax.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "T0",
                Name = "TF7",
                Description = "Temperature value of Heater turning ON",
                Value = Math.Round(t2.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "T1",
                Name = "TF8",
                Description = "Temperature value of Heater turning OFF (rising slope)",
                Value = Math.Round(t4.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff1",
                Name = "TF9",
                Description = "T1 - T0",
                Value = Math.Round(t4.Y - t2.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff2",
                Name = "TF10",
                Description = "Tmax - T1",
                Value = Math.Round(Tmax.Y - t4.Y, round)
            });
            #endregion

            #region PressureFeatures
            // Pressure features
            unit.PressFeatures.Add(new Feature()
            {
                ID = "tp1",
                Name = "PF1",
                Description = "(t6 + tsettle) - t5",
                Value = Math.Round(t6.X + config.EmbossingConstants.tsettle - t5.X, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "tp2",
                Name = "PF2",
                Description = "t9 - (t6 + tsettle)",
                Value = Math.Round(t9.X - (t6.X + config.EmbossingConstants.tsettle))
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "T2",
                Name = "PF3",
                Description = "t6 + 2",
                Value = Math.Round(t6.X + 2)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P1",
                Name = "PF4",
                Description = "pre-defined pressure value at t5",
                Value = Math.Round(t5.Y)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P2",
                Name = "PF5",
                Description = "Maximum pressure",
                Value = Math.Round(t6.Y)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P3",
                Name = "PF6",
                Description = "Pressure Value at (t6 + tsettle)",
                Value = Math.Round(P3.Y, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P4",
                Name = "PF7",
                Description = "Pressure Value at t9",
                Value = Math.Round(t9.Y, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff1",
                Name = "PF8",
                Description = "P2 - P1",
                Value = Math.Round(t6.Y - config.EmbossingConstants.P1)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff2",
                Name = "PF9",
                Description = "P2 – P3",
                Value = Math.Round(t6.Y - P3.Y, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff3",
                Name = "PF10",
                Description = "P3 – P4",
                Value = Math.Round(P3.Y - P4.Y)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff4",
                Name = "PF11",
                Description = "P4 – P1",
                Value = Math.Round(P4.Y - config.EmbossingConstants.P1)
            });
            #endregion
        }

        public static void CalculateOld(TTLProcess unit)
        {
            if (unit == null) return;

            // Constants
            int round = config.EmbossingConstants.RoundTo;

            #region DataPoints

            // Time data points with X and Y values that are used to calculate Features
            DataPoint t2 = new DataPoint()
            {
                Name = "t2",
                Descritpion = "Time value of Heater turning ON",
                X = Math.Round(unit.Heater.On, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.Heater.On).Y, round)
            };

            DataPoint t3 = new DataPoint()
            {
                Name = "t3",
                Descritpion = "Time value of measured temperature equals the pre-defined set-value, TD.",
                X = Math.Round(unit.Temperature.FindPointByY(config.EmbossingConstants.TD).X, round),
                Y = config.EmbossingConstants.TD
            };

            DataPoint t4 = new DataPoint()
            {
                Name = "t4",
                Descritpion = "Time value of Heater turning OFF",
                X = Math.Round(unit.Heater.Off, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.Heater.Off).Y, round)
            };

            DataPoint t5 = new DataPoint()
            {
                Name = "t5",
                Descritpion = "t3 + tHP",
                X = Math.Round(t3.X + config.EmbossingConstants.tHP),
                Y = Math.Round(unit.HighPressure.FindPointByX(t3.X + config.EmbossingConstants.tHP).Y, round)
            };

            DataPoint t6 = new DataPoint()
            {
                Name = "t6",
                Descritpion = "Maximum Pressure",
                X = Math.Round(unit.HighPressure.MaxPointY(), round),
                Y = Math.Round(unit.HighPressure.MaxPointX(), round)
            };

            DataPoint t7 = new DataPoint()
            {
                Name = "t7",
                Descritpion = "Time of pressure intensifier released",
                X = Math.Round(unit.HighPressure.MaxPointX(), round),
                Y = Math.Round(unit.HighPressure.MaxPointY(), round)
            };

            DataPoint t8 = new DataPoint()
            {
                Name = "t8",
                Descritpion = "t7 + tc",
                X = Math.Round(t7.X + config.EmbossingConstants.tc, round),
                Y = Math.Round(unit.Temperature.FindPointByY(t7.X + config.EmbossingConstants.tc).Y, round)
            };

            DataPoint t9 = new DataPoint()
            {
                Name = "t9",
                Descritpion = "Time value of measured temperature equals the pre-defined set-value",
                X = Math.Round(unit.Temperature.FindPointByY(config.EmbossingConstants.t9const).X, round),
                Y = Math.Round(config.EmbossingConstants.t9const, round)
            };

            DataPoint t10 = new DataPoint()
            {
                Name = "t10",
                Descritpion = "Time value of measured pressure equals the pre-defined pressure value, P1",
                X = Math.Round(unit.HighPressure.FindPointByY(config.EmbossingConstants.P1).X, round),
                Y = Math.Round(config.EmbossingConstants.P1, round)
            };

            DataPoint Tmax = new DataPoint()
            {
                Name = "Tmax",
                Descritpion = "Maximum temperature",
                X = Math.Round(unit.Temperature.MaxPointX(), round),
                Y = Math.Round(unit.Temperature.MaxPointY(), round)
            };

            DataPoint P3 = new DataPoint()
            {
                Name = "P3",
                Descritpion = "Pressure Value at (t6 + tsettle)",
                X = Math.Round(t6.X + config.EmbossingConstants.tsettle, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(t6.X + config.EmbossingConstants.tsettle).Y, round)
            };

            DataPoint P4 = new DataPoint()
            {
                Name = "P4",
                Descritpion = "Pressure Value at t9",
                X = t9.X,
                Y = Math.Round(unit.HighPressure.FindPointByX(t9.X).Y, round)
            };

            // Value data points used to calculate features
            unit.DataPoints = new List<DataPoint> { t2, t3, t4, t5, t6, t7, t8, t9, t10, Tmax, P3, P4 };

            #endregion

            #region TemperatureFeatures
            // Features
            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT2",
                Name = "TF1",
                Description = "Time value of Heater turning OFF - Time value of Heater turning ON",
                Value = Math.Round(t4.X - t2.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT3",
                Name = "TF2",
                Description = "Time of pressure intensifier released - Time value of Heater turning OFF",
                Value = Math.Round(t7.X - t4.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT4",
                Name = "TF3",
                Description = "t7 + tc - Time of pressure intensifier released",
                Value = Math.Round(t8.X - t7.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT5",
                Name = "TF4",
                Description = "Time value of measured temperature equals the pre-defined set-value - t7 + tc",
                Value = Math.Round(t9.X - t8.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "tT6",
                Name = "TF5",
                Description = "Time value of measured pressure equals the pre-defined pressure value, P1 - Time value of measured temperature equals \r\nthe pre-defined set-value",
                Value = Math.Round(t10.X - t9.X, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "Tmax",
                Name = "TF6",
                Description = "Maximum Temperature",
                Value = Math.Round(Tmax.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "T0",
                Name = "TF7",
                Description = "Temperature value of Heater turning ON",
                Value = Math.Round(t2.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "T1",
                Name = "TF8",
                Description = "Temperature value of Heater turning OFF (rising slope)",
                Value = Math.Round(t4.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff1",
                Name = "TF9",
                Description = "T1 - T0",
                Value = Math.Round(t4.Y - t2.Y, round)
            });

            unit.TempFeatures.Add(new Feature()
            {
                ID = "TempDiff2",
                Name = "TF10",
                Description = "Tmax - T1",
                Value = Math.Round(Tmax.Y - t4.Y, round)
            });
            #endregion

            #region PressureFeatures
            // Pressure features
            unit.PressFeatures.Add(new Feature()
            {
                ID = "tp1",
                Name = "PF1",
                Description = "(t6 + tsettle) - t5",
                Value = Math.Round(t6.X + config.EmbossingConstants.tsettle - t5.X, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "tp2",
                Name = "PF2",
                Description = "t9 - (t6 + tsettle)",
                Value = Math.Round(t9.X - (t6.X + config.EmbossingConstants.tsettle))
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "T2",
                Name = "PF3",
                Description = "t6 + 2",
                Value = Math.Round(t6.X + 2)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P1",
                Name = "PF4",
                Description = "pre-defined pressure value at t5",
                Value = Math.Round(t5.Y)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P2",
                Name = "PF5",
                Description = "Maximum pressure",
                Value = Math.Round(t6.Y)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P3",
                Name = "PF6",
                Description = "Pressure Value at (t6 + tsettle)",
                Value = Math.Round(P3.Y, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "P4",
                Name = "PF7",
                Description = "Pressure Value at t9",
                Value = Math.Round(t9.Y, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff1",
                Name = "PF8",
                Description = "P2 - P1",
                Value = Math.Round(t6.Y - config.EmbossingConstants.P1)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff2",
                Name = "PF9",
                Description = "P2 – P3",
                Value = Math.Round(t6.Y - P3.Y, round)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff3",
                Name = "PF10",
                Description = "P3 – P4",
                Value = Math.Round(P3.Y - P4.Y)
            });

            unit.PressFeatures.Add(new Feature()
            {
                ID = "PresDiff4",
                Name = "PF11",
                Description = "P4 – P1",
                Value = Math.Round(P4.Y - config.EmbossingConstants.P1)
            });
            #endregion
        }
    }
}
