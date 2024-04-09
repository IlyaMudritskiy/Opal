namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class DataPoint
    {
        public string Name { get; set; }
        public string Descritpion { get; set; }

        /// <summary>
        /// Time offset from the start of the process
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Value at time X
        /// </summary>
        public double Y { get; set; }

        public string UnitX { get; set; }
        public string UnitY { get; set; }

        public string ToString()
        {
            return $"{X}{UnitX}, {Y}{UnitY}";
        }
    }
}
