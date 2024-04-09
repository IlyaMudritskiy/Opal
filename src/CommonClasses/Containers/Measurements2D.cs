using ProcessDashboard.src.TTL.Containers.FileContent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.CommonClasses.Containers
{
    public class Measurements2D
    {
        public List<double> X { get; set; }
        public List<double> Y { get; set; }

        public int Count { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Measurements2D()
        {
            X = new List<double>();
            Y = new List<double>();
        }

        public Measurements2D(List<Measurement> measurements)
        {
            if (measurements == null)
                throw new ArgumentNullException(nameof(measurements));

             List<string> dateTime = measurements.Select(m => m.DateTime).ToList();
            List<string> values = measurements.Select(v => v.MeasurementValue).ToList();

            List<double> resultOffset = new List<double>(dateTime.Count).Select(x => .0).ToList();
            List<double> resultValues = new List<double>(values.Count).Select(x => .0).ToList();

            for (int i = 0; i < values.Count; i++)
            {
                try
                {
                    resultOffset.Add((DateTime.Parse(dateTime[i]) - DateTime.Parse(dateTime[0])).TotalSeconds);
                    resultValues.Add(double.Parse(values[i]));
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
            X = resultOffset;
            Y = resultValues;
        }

        public static Measurements2D operator +(Measurements2D a, Measurements2D b)
        {
            if (a == null || b == null) return null;

            Measurements2D result = new Measurements2D();

            for (int i = 0; i < a.X.Count; i++)
            {
                result.X.Add(a.X[i] + b.X[i]);
                result.Y.Add(a.Y[i] + b.Y[i]);
            }

            return result;
        }

        public static Measurements2D operator /(Measurements2D a, double div)
        {
            if (a == null || div == 0) return null;

            Measurements2D result = new Measurements2D();

            for (int i = 0; i < a.X.Count; i++)
            {
                result.X.Add(a.X[i] / div);
                result.Y.Add(a.Y[i] / div);
            }

            return result;
        }

        public void FromMeasurements(List<string> XValues, List<string> YValues)
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
            Count = X.Count;
        }

        public Point FindPointByX_old(double XValue)
        {
            int index = X.IndexOf(XValue);
            if (index != -1)
                return new Point { X = X[index], Y = X[index] };

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

        public Point FindPointByX(double XValue)
        {
            int index = X.IndexOf(XValue);
            if (index != -1)
            {
                return new Point { X = X[index], Y = Y[index] };
            }
            else
            {
                int rInd = 0;
                int lInd = 0;

                // Find point on the right of X
                for (int i = 0; i < X.Count; i++)
                {
                    if (X[i] > XValue)
                    {
                        rInd = i;
                        break;
                    }
                }

                // Find point on the left of X
                for (int j = X.Count-1; j >= 0; j--)
                {
                    if (X[j] < XValue)
                    {
                        lInd = j;
                        break;
                    }
                }

                // Build line function through 2 points
                double x1 = X[lInd];
                double y1 = Y[lInd];
                double x2 = X[rInd];
                double y2 = Y[rInd];

                double slope = (y2 - y1) / (x2 - x1);
                double intercept = y1 - slope * x1;

                double exactValue = slope * XValue + intercept;

                return new Point { X = XValue, Y = exactValue };
            }
        }

        public Point FindPointByY(double YValue)
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

        public double MaxPointX()
        {
            int index = MaxValueIndex();
            return X[index];
        }

        public double MaxPointY()
        {
            int index = MaxValueIndex();
            return Y[index];
        }

        public Point MaxPoint()
        {
            int index = MaxValueIndex();
            return new Point
            {
                X = X[index],
                Y = Y[index]
            };
        }

        public double MaxX()
        {
            return X.Max();
        }
    }
}