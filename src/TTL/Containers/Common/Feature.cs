using System;

namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class Feature
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private double _value;
        public double Value { get { return _value; } set { _value = Math.Round(value, 3); } }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public override string ToString()
        {
            return $"Feature: {ID}, Name: {Name}, Value: {Value}";
        }

        public static Feature operator +(Feature a, Feature b)
        {
            if (a == null || b == null)
            {
                Log.Error("One of the features is null!");
                return null;
            }

            if (a.ID != b.ID)
                Log.Warn($"Attempting to sum different features. Feature 1: ({a.ID} - {a.Name}), Feature 2: ({b.ID} - {b.Name}). Using Feature 1 as template.");

            return new Feature()
            {
                ID = a.ID,
                Name = a.Name,
                Description = a.Description,
                Value = a.Value + b.Value
            };
        }

        public static Feature operator /(Feature a, double div)
        {
            if (div == 0)
            {
                Log.Error("Division by zero! Using the feature before division.");
                return a;
            }

            return new Feature()
            {
                ID = a.ID,
                Name = a.Name,
                Description = a.Description,
                Value = a.Value / div
            };
        }
    }
}
