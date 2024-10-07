using System.Collections.Generic;

namespace Opal.src.TTL.Containers.Common
{
    public class DataPointsRow<T>
    {
        public string Serial { get; set; }
        public string WPC { get; set; }
        public List<T> Values { get; set; }
    }
}
