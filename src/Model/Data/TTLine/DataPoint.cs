namespace ProcessDashboard.src.Model.Data.TTLine
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

        public override string ToString()
        {
            return $"DataPoint: {Name}, X: {X}, Y: {Y}";
        }
    }
}
