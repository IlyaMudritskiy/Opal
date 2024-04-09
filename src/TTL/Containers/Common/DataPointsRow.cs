using System.Collections.Generic;

namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class DataPointsRow
    {
        public string Serial { get; set; }
        public List<DataPoint> Values { get; set; }
    }
}
