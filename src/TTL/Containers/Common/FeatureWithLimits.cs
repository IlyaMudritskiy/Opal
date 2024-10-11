namespace Opal.src.TTL.Containers.Common
{
    internal class FeatureWithLimits : Feature
    {
        public double? Min { get; set; }
        public double? Max { get; set; }

        public bool HasLimits => Min.HasValue && Max.HasValue;
    }
}
