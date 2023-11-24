using System;

namespace ProcessDashboard.src.Model.Data.TTLine
{
    public class Feature
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private double _value;
        public double Value { get { return _value; } set { _value = Math.Round(value, 3); } }
    }
}
