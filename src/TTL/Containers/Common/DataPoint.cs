namespace Opal.src.TTL.Containers.Common
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

        public bool IsNaN()
        {
            return (double.IsNaN(X) || double.IsNaN(Y));
        }

        public bool Equals(DataPoint other)
        {
            if (other == null) return false;
            if (other.Name == this.Name && other.X == this.X && other.Y == this.Y) return true;
            return false;
        }
    }
}
