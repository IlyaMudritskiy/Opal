using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcessDashboard.src.Model.Data.Acoustic
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AcousticTest
    {
        [JsonProperty(PropertyName = "DUT")]
        public AcousticDUT DUT { get; set; }

        [JsonProperty(PropertyName = "Steps")]
        public List<AcousticStep> Steps { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class AcousticDUT
    {
        [JsonProperty(PropertyName = "dutpass")]
        public bool Pass { get; set; }

        [JsonProperty(PropertyName = "typeid")]
        public string TypeID { get; set; }

        [JsonProperty(PropertyName = "serialnr")]
        public string Serial { get; set; }

        [JsonProperty(PropertyName = "duttime")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "nestnumber")]
        public int Nest { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class AcousticStep
    {
        [JsonProperty(PropertyName = "stepname")]
        public string StepName { get; set; }

        [JsonProperty(PropertyName = "measurement")]
        public List<double[]> Measurement { get; set; }
    }
}
