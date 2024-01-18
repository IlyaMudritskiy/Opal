using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.TTL.Containers.FileContent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.Model.Data.TTLine
{
    public class Measurements
    {
        public double[] TimeOffset { get; set; }
        public double[] Values { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Measurements(List<Measurement> measurements)
        {
            if (measurements == null)
                throw new ArgumentNullException(nameof(measurements));

            string[] dateTime = measurements.Select(m => m.DateTime).ToArray();
            string[] values = measurements.Select(v => v.MeasurementValue).ToArray();

            double[] resultOffset = new double[dateTime.Length];
            double[] resultValues = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                try
                {
                    resultOffset[i] = (DateTime.Parse(dateTime[i]) - DateTime.Parse(dateTime[0])).TotalSeconds;
                    resultValues[i] = double.Parse(values[i]);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
            TimeOffset = resultOffset;
            Values = resultValues;
        }


        public Point FindPointByTime(double timeOffset)
        {
            int index = Array.IndexOf(TimeOffset, timeOffset);
            if (index != -1)
                return new Point { X = TimeOffset[index], Y = Values[index] };

            else
            {
                var nearestPoints = TimeOffset.Select((x, i) => new { Index = i, Distance = Math.Abs(x - timeOffset) })
                                  .OrderBy(p => p.Distance)
                                  .Take(2)
                                  .OrderBy(p => p.Index)
                                  .ToArray();

                double x1 = TimeOffset[nearestPoints[0].Index];
                double y1 = Values[nearestPoints[0].Index];
                double x2 = TimeOffset[nearestPoints[1].Index];
                double y2 = Values[nearestPoints[1].Index];

                double slope = (y2 - y1) / (x2 - x1);
                double intercept = y1 - slope * x1;

                double exactValue = slope * timeOffset + intercept;

                return new Point { X = timeOffset, Y = exactValue };
            }
        }

        public Point FindPointByValue(double value)
        {
            int index = Array.IndexOf(Values, value);
            if (index != -1)
                return new Point { X = TimeOffset[index], Y = Values[index] };

            else
            {
                var nearestPoints = Values.Select((x, i) => new { Index = i, Distance = Math.Abs(x - value) })
                                  .OrderBy(p => p.Distance)
                                  .Take(2)
                                  .OrderBy(p => p.Index)
                                  .ToArray();

                double x1 = TimeOffset[nearestPoints[0].Index];
                double y1 = Values[nearestPoints[0].Index];
                double x2 = TimeOffset[nearestPoints[1].Index];
                double y2 = Values[nearestPoints[1].Index];

                double slope = (x2 - x1) / (y2 - y1);
                double intercept = x1 - slope * y1;

                double exactValue = slope * value + intercept;

                return new Point { X = exactValue, Y = value };
            }
        }

        /// <summary>
        /// Finds the index of maximum value in array.
        /// </summary>
        /// <returns>Index of maximum value</returns>
        public int MaxValueIndex()
        {
            return Enumerable.Range(0, Values.Length)
                .OrderByDescending(i => Values[i])
                .FirstOrDefault();
        }

        public double MaxValue()
        {
            int index = MaxValueIndex();
            return Values[index];
        }

        public double MaxValueTime()
        {
            int index = MaxValueIndex();
            return TimeOffset[index];
        }

        public double MaxTime()
        {
            return TimeOffset.Max();
        }
    }
}
