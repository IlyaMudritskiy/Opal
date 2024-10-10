using Newtonsoft.Json;
using System.Collections.Generic;

namespace Opal.src.TTL.Containers.FileContent
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProductLimits
    {
        [JsonProperty(PropertyName = "typeid")]
        public string TypeID { get; set; }

        [JsonProperty(PropertyName = "mean_limits")]
        public Dictionary<string, MeanLimit> MeanLimits { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MeanLimit
    {
        [JsonProperty(PropertyName = "min")]
        public float Min { get; set; }

        [JsonProperty(PropertyName = "max")]
        public float Max { get; set; }
    }
}
