using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ProcessDashboard.src.Model.Data.Acoustic
{
    public class Limit
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public List<double> X { get; set; } = new List<double>();
        public List<double> Y { get; set; } = new List<double>();

        public Limit(string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) return;

            try
            {
                string[] lines = File.ReadAllLines(filepath);

                foreach (string line in lines)
                {
                    string[] values = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (values.Length == 2)
                    {
                        this.X.Add(double.Parse(values[0], CultureInfo.InvariantCulture));
                        this.Y.Add(double.Parse(values[1], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        Console.WriteLine($"Line problem: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
