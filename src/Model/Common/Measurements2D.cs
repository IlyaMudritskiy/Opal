using ProcessDashboard.src.Model.Data.TTLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Common
{
    public class Measurements2D
    {
        public List<double> X { get; set; }
        public List<double> Y { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public void FromTimeSeries(List<string> XValues, List<string> YValues)
        {
            if (XValues == null || YValues == null) return;
            if (XValues.Count == 0 || YValues.Count == 0) return;

            List<double> XResult = new List<double>();
            List<double> YResult = new List<double>();

            for (int i = 0; i < YValues.Count(); i++)
            {
                try
                {
                    XResult.Add((DateTime.Parse(XValues[i]) - DateTime.Parse(XValues[0])).TotalSeconds);
                    YResult.Add(double.Parse(YValues[i]));
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
            X = XResult;
            Y = YResult;
        }

        public Point FindPointByTime(double XValue)
        {
            int index = X.IndexOf(XValue);
            if (index != -1)
                return new Point { X = this.X[index], Y = this.X[index] };

            else
            {
                var nearestPoints = X.Select((x, i) => new { Index = i, Distance = Math.Abs(x - XValue) })
                                  .OrderBy(p => p.Distance)
                                  .Take(2)
                                  .OrderBy(p => p.Index)
                                  .ToArray();

                double x1 = X[nearestPoints[0].Index];
                double y1 = Y[nearestPoints[0].Index];
                double x2 = X[nearestPoints[1].Index];
                double y2 = Y[nearestPoints[1].Index];

                double slope = (y2 - y1) / (x2 - x1);
                double intercept = y1 - slope * x1;

                double exactValue = slope * XValue + intercept;

                return new Point { X = XValue, Y = exactValue };
            }
        }

        public Point FindPointByValue(double YValue)
        {
            int index = Y.IndexOf(YValue);
            if (index != -1)
                return new Point { X = X[index], Y = Y[index] };

            else
            {
                var nearestPoints = Y.Select((x, i) => new { Index = i, Distance = Math.Abs(x - YValue) })
                                  .OrderBy(p => p.Distance)
                                  .Take(2)
                                  .OrderBy(p => p.Index)
                                  .ToArray();

                double x1 = X[nearestPoints[0].Index];
                double y1 = Y[nearestPoints[0].Index];
                double x2 = X[nearestPoints[1].Index];
                double y2 = Y[nearestPoints[1].Index];

                double slope = (x2 - x1) / (y2 - y1);
                double intercept = x1 - slope * y1;

                double exactValue = slope * YValue + intercept;

                return new Point { X = exactValue, Y = YValue };
            }
        }

        public int MaxValueIndex()
        {
            return Enumerable.Range(0, Y.Count)
                .OrderByDescending(i => Y[i])
                .FirstOrDefault();
        }

        public double MaxValue()
        {
            int index = MaxValueIndex();
            return Y[index];
        }

        public double MaxValueTime()
        {
            int index = MaxValueIndex();
            return X[index];
        }

        public double MaxTime()
        {
            return X.Max();
        }
    }
}
