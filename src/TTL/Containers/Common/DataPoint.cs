namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class DataPoint: IValueDescription
    {
        public double X { get; set; }
        public double Y { get; set; }

        public string UnitX { get; set; }
        public string UnitY { get; set; }

        //public bool Available { get; set; }

        public override string ToString()
        {
            return $"{X}{UnitX}, {Y}{UnitY}";
        }
    }
}
