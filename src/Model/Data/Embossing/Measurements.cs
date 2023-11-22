using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ProcessDashboard.src.Model.Data.Embossing
{
    public class Measurements
    {
        public double[] TimeOffset { get; set; }
        public double[] Values { get; set; }

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
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            TimeOffset = resultOffset;
            Values = resultValues;
        }

        /// <summary>
        /// Finds the index of the closest value in specified array.
        /// </summary>
        /// <param name="value">Value to find.</param>
        /// <param name="array">Where to look for the value.</param>
        /// <returns>Index of the value.</returns>
        public int FindValueIndex(double value, double[] array)
        {
            return Enumerable.Range(0, array.Length)
                .OrderBy(i => Math.Abs(array[i] - value))
                .FirstOrDefault();
        }

        /// <summary>
        /// Finds the closest value in array to the value in argument.
        /// </summary>
        /// <param name="value">Value to find.</param>
        /// <param name="array">Where to look for the closest value.</param>
        /// <returns>Closest value to specified value.</returns>
        public double FindClosestValue(double value, double[] array)
        {
            int idx = FindValueIndex(value, array);
            return array[idx];
        }

        /// <summary>
        /// Finds the corresponding MeasurementValue to specified TimeOffset.
        /// </summary>
        /// <param name="time">TimeOffset value</param>
        /// <returns>Corresponding MeasurementValue</returns>
        public double TimeToValue(double time)
        {
            int pointIdx = FindValueIndex(time, TimeOffset);
            return Values[pointIdx];
        }

        /// <summary>
        /// Finds the corresponding TimeOffset value to specified MeasurementValue.
        /// </summary>
        /// <param name="value">Measurement value</param>
        /// <returns>Corresponding TimeOffset</returns>
        public double ValueToTime(double value)
        {
            int pointIdx = FindValueIndex(value, Values);
            return TimeOffset[pointIdx];
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

        /// <summary>
        /// Finds the maximum value.
        /// </summary>
        /// <returns>Maximum value</returns>
        public double MaxValue()
        {
            return Values[MaxValueIndex()];
        }
    }
}
