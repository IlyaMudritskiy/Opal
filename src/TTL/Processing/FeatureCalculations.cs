using System;
using System.Collections.Generic;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;

namespace ProcessDashboard.src.TTL.Processing
{
    public static class FeatureCalculations
    {
        private static Config config = Config.Instance;

        private static List<DataPoint> lastDataPoints;

        public static void Calculate(TTLProcess unit)
        {
            if (unit == null) return;
            if (lastDataPoints == null) lastDataPoints = new List<DataPoint>();

            int round = config.EmbossingConstants.RoundTo;
            unit.JsonPoints.SetOffset();

            #region DataPoints

            DataPoint t2 = new DataPoint()
            {
                Name = "t2",
                Description = "Heater turning ON\nTimestamp, (s)",
                X = Math.Round(unit.Heater.On, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.Heater.On).Y, round),
                UnitX = "s",
                UnitY = "°C"
            };

            DataPoint t3 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t3.Available,
                Name = "t3",
                Description = "Temperature setpoint reached\nTimestamp, (s)",
                X = unit.JsonPoints.PS01_t3.Available ? Math.Round(unit.JsonPoints.PS01_t3.DateOffset, round) : 0.0,
                Y = Math.Round(unit.Temperature.FindPointByX(unit.JsonPoints.PS01_t3.DateOffset).Y, round),
                UnitX = "s",
                UnitY = "°C"
            };

            DataPoint t4 = new DataPoint()
            {
                Name = "t4",
                Description = "Heater turning OFF\nTimestamp, (s)",
                X = Math.Round(unit.Heater.Off, round),
                Y = Math.Round(unit.Temperature.FindPointByX(unit.Heater.Off).Y, round),
                UnitX = "s",
                UnitY = "°C"
            };

            DataPoint t5 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t5.Available,
                Name = "t5",
                Description = "Pressure intensifier ON\nTimestamp, (s)",
                X = unit.JsonPoints.PS01_t5.Available ? Math.Round(unit.JsonPoints.PS01_t5.DateOffset, round) : 0.0,
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t5.DateOffset).Y, round),
                UnitX = "s",
                UnitY = "bar"
            };

            DataPoint t6 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t6.Available,
                Name = "t6",
                Description = "Maximum Pressure\n"
                            + "Pressure intensifier fully loaded, before pressure valve is released\n"
                            + "Timestamp, (s)",
                //X = unit.JsonPoints.PS01_t6.Available ? Math.Round(unit.JsonPoints.PS01_t6.DateOffset, round) : 0.0,
                X = Math.Round(unit.HighPressure.MaxPointX(), round),
                //Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t6.DateOffset).Y, round),
                Y = Math.Round(unit.HighPressure.MaxPointY(), round),
                UnitX = "s",
                UnitY = "bar"
            };

            DataPoint t7 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t7.Available,
                Name = "t7",
                Description = "Pressure valve released\nTimestamp, (s)",
                X = unit.JsonPoints.PS01_t7.Available ? Math.Round(unit.JsonPoints.PS01_t7.DateOffset, round) : 0.0,
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t7.DateOffset).Y, round),
                UnitX = "s",
                UnitY = "bar"
            };

            DataPoint t8 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t8.Available,
                Name = "t8",
                Description = "Cooling ON\nTimestamp, (s)",
                X = unit.JsonPoints.PS01_t8.Available ? Math.Round(unit.JsonPoints.PS01_t8.DateOffset, round) : 0.0,
                Y = Math.Round(unit.Temperature.FindPointByX(unit.JsonPoints.PS01_t8.DateOffset).Y, round),
                UnitX = "s",
                UnitY = "°C"
            };

            DataPoint t9 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t9.Available,
                Name = "t9",
                Description = "Temperature (cooling) reaches the die cover open temperature\nCooling slope\nTimestamp, (s)",
                X = unit.JsonPoints.PS01_t9.Available ? Math.Round(unit.JsonPoints.PS01_t9.DateOffset, round) : 0.0,
                Y = Math.Round(unit.Temperature.FindPointByX(unit.JsonPoints.PS01_t9.DateOffset).Y, round),
                UnitX = "s",
                UnitY = "°C"
            };

            DataPoint t10 = new DataPoint()
            {
                Available = unit.JsonPoints.PS01_t10.Available,
                Name = "t10",
                Description = "Measured pressure equals the pre-defined pressure value, P1\nTimestamp, (s)",
                X = unit.JsonPoints.PS01_t10.Available ? Math.Round(unit.JsonPoints.PS01_t10.DateOffset, round) : 0.0,
                Y = Math.Round(unit.HighPressure.FindPointByX(unit.JsonPoints.PS01_t10.DateOffset).Y, round),
                UnitX = "s",
                UnitY = "bar"
            };

            DataPoint Tmax = new DataPoint()
            {
                Name = "Tmax",
                Description = "Maximum temperature\nTemperature, (°C)",
                X = Math.Round(unit.Temperature.MaxPointX(), round),
                Y = Math.Round(unit.Temperature.MaxPointY(), round),
                UnitX = "s",
                UnitY = "°C"
            };

            DataPoint P3 = new DataPoint()
            {
                Name = "P3",
                Description = "Pressure at the start of embossing process\nPressure, (bar)",
                X = Math.Round(t6.X + 0.5, round),
                Y = Math.Round(unit.HighPressure.FindPointByX(t6.X + config.EmbossingConstants.tsettle).Y, round),
                UnitX = "s",
                UnitY = "bar"
            };

            DataPoint P4 = new DataPoint()
            {
                Name = "P4",
                Description = "Pressure at the end of embossing process\nPressure, (bar)",
                X = t9.X,
                Y = Math.Round(unit.HighPressure.FindPointByX(t9.X).Y, round),
                UnitX = "s",
                UnitY = "bar"
            };

            // Value data points used to calculate features
            unit.DataPoints = new List<DataPoint> { t2, t3, t4, t5, t6, t7, t8, t9, t10, Tmax, P3, P4 };

            #endregion

            #region TemperatureFeatures

            unit.TempFeatures.Add(new Feature()
            {
                Available = unit.JsonPoints.PS01_HeaterRuntime.Available,
                Name = "HR",
                Description = "Heater Runtime\n" +
                              "Loaded from .json file\n" +
                              "Duration, (s)",
                Value = unit.JsonPoints.PS01_HeaterRuntime.Value / 1000
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = true,
                Name = "HRC",
                Description = "Heater runtime calculated\n" +
                              "t4.X - t2.X\n" +
                              "Duration, (s)",
                Value = Math.Round(t4.X - t2.X, round),
                RelatedDataPoints = new List<DataPoint> { t2, t4 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = t7.Available,
                Name = "tT3",
                Description = "Time duration between Heater OFF and Pressure intensifier released\n" +
                              "(P)t7.X - t4.X\n" +
                              "Duration, (s)",
                Value = Math.Round(t7.X - t4.X, round),
                RelatedDataPoints = new List<DataPoint> { t4 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = t7.Available && t8.Available,
                Name = "tT4",
                Description = "Time duration (actual) between slider opening and cooling On \n" +
                              "t8.X - (P)t7.X\n" +
                              "Duration, (s)",
                Value = Math.Round(t8.X - t7.X, round),
                RelatedDataPoints = new List<DataPoint> { t8 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = t8.Available && t9.Available,
                Name = "tT5",
                Description = "Time duration of cooling\n" +
                              "t8.X - t9.X\n" +
                              "Duration, (s)",
                Value = Math.Round(t9.X - t8.X, round),
                RelatedDataPoints = new List<DataPoint> { t8, t9 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = t9.Available && t10.Available,
                Name = "tT6",
                Description = "(In rework) t10.X - t9.X",
                Value = Math.Round(t10.X - t9.X, round),
                RelatedDataPoints = new List<DataPoint> { t9, t10 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = true,
                Name = "Tmax",
                Description = "Maximum die temperature value\n" +
                              "Temperature, (°C)",
                Value = Math.Round(Tmax.Y, round),
                RelatedDataPoints = new List<DataPoint> { Tmax }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = true,
                Name = "T0",
                Description = "Die temperature value of Heater turning ON\n" +
                              "t2.Y\n" +
                              "Temperature, (°C)",
                Value = Math.Round(t2.Y, round),
                RelatedDataPoints = new List<DataPoint> { t2 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = true,
                Name = "T1",
                Description = "Die temperature value of Heater turning OFF\n" +
                              "t4.Y\n" +
                              "Temperature, (°C)",
                Value = Math.Round(t4.Y, round),
                RelatedDataPoints = new List<DataPoint> { t4 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = true,
                Name = "TempDiff1",
                Description = "Die temperature difference during heating\n" +
                              "t4.Y - t2.Y or T1 - T0\n" +
                              "Temperature, (°C)",
                Value = Math.Round(t4.Y - t2.Y, round),
                RelatedDataPoints = new List<DataPoint> { t2, t4 }
            });

            unit.TempFeatures.Add(new Feature()
            {
                Available = true,
                Name = "TempDiff2",
                Description = "Die temperature lag value between heater OFF and Max Temperature\n" +
                              "Tmax - t4.Y or Tmax - T1\n" +
                              "Temperature, (°C)",
                Value = Math.Round(Tmax.Y - t4.Y, round),
                RelatedDataPoints = new List<DataPoint> { Tmax, t4 }
            });
            #endregion

            #region PressureFeatures
            // Pressure features
            unit.PressFeatures.Add(new Feature()
            {
                Available = t5.Available && t6.Available,
                Name = "tp1",
                Description = "Time duration of pressure buildup in intensifier\nDuration, (s)",
                Value = Math.Round(t6.X - t5.X, round),
                RelatedDataPoints = new List<DataPoint> { t5, t6 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t6.Available && t9.Available,
                Name = "tp2",
                Description = "Time duration of applying pressure during embossing\nDuration, (s)",
                Value = Math.Round(t9.X - t6.X, round),
                RelatedDataPoints = new List<DataPoint> { t6, t9 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t6.Available,
                Name = "T2?",
                Description = "!!!t6 + 2",
                Value = Math.Round(t6.X + 2, round),
                RelatedDataPoints = new List<DataPoint> { t6 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t5.Available,
                Name = "P1",
                Description = "Pre-defined pressure value at t5\nPressure, (bar)",
                Value = Math.Round(t5.Y, round),
                RelatedDataPoints = new List<DataPoint> { t5 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t6.Available,
                Name = "P2",
                Description = "Maximum pressure\nPressure, (bar)",
                Value = Math.Round(t6.Y, round),
                RelatedDataPoints = new List<DataPoint> { t6 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Name = "P3",
                Description = "Pressure Value at (t6 + tsettle)\nPressure, (bar)",
                Value = Math.Round(P3.Y, round),
                RelatedDataPoints = new List<DataPoint> { P3 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t9.Available,
                Name = "P4",
                Description = "Pressure Value at t9\nPressure, (bar)",
                Value = P4.Y,
                RelatedDataPoints = new List<DataPoint> { P4 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t6.Available,
                Name = "PresDiff1",
                Description = "P2 - P1\nPressure difference, (bar)",
                Value = Math.Round(t6.Y - config.EmbossingConstants.P1, round),
                RelatedDataPoints = new List<DataPoint> { t6 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Available = t6.Available,
                Name = "PresDiff2",
                Description = "P2 – P3\nPressure difference, (bar)",
                Value = Math.Round(P3.Y - t6.Y, round),
                RelatedDataPoints = new List<DataPoint> { t6, P3 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Name = "PresDiff3",
                Description = "P3 – P4\nPressure difference, (bar)",
                Value = Math.Round(P3.Y - P4.Y),
                RelatedDataPoints = new List<DataPoint> { P3, P4 }
            });

            unit.PressFeatures.Add(new Feature()
            {
                Name = "PresDiff4",
                Description = "P4 – P1\nPressure difference, (bar)",
                Value = Math.Round(P4.Y - config.EmbossingConstants.P1),
                RelatedDataPoints = new List<DataPoint> { P4 }
            });
            #endregion
        }
        
        /*
        public static void Calculate(TTLProcess unit)
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
        */
    }
}
