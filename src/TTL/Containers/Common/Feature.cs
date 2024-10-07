using System;
using System.Collections.Generic;

namespace Opal.src.TTL.Containers.Common
{
    public class Feature: IValueDescription
    {

        private double _value;
        public double Value { get { return _value; } set { _value = Math.Round(value, 3); Available = true; } }
        public List<DataPoint> RelatedDataPoints { get; set; }
        //public bool Available { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Feature()
        {
            Name = "_NAME_";
            Description = "__DESCRIPTION__";
            Value = double.NaN;
            Available = false;
            RelatedDataPoints = new List<DataPoint>();
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public static Feature operator +(Feature a, Feature b)
        {
            if (a == null || b == null)
            {
                Log.Error("One of the features is null!");
                return null;
            }

            //if (a.Name != b.Name)
                //Log.Warn($"Attempting to sum different features. Feature 1: ({a.Name}), Feature 2: ({b.Name}). Using Feature 1 as template.");
            
            var result = new Feature()
            {
                Name = a.Name == "_NAME_" ? b.Name : a.Name,
                Description = a.Description == "__DESCRIPTION__" ? b.Description : a.Description,
                Value = double.IsNaN(a.Value) ? 0.0 + b.Value : a.Value + b.Value
            };

            return result;
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
                Name = a.Name,
                Description = a.Description,
                Value = a.Value / div
            };
        }
    }
}
