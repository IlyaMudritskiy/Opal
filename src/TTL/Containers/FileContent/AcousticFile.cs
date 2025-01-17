﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Opal.Model.Data.Acoustic
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AcousticFile
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
        public string Serial { 
            get { return _serial; } 
            set 
            {
                if (value.Length >= 2)
                {
                    Track = int.Parse(value[value.Length - 2].ToString());
                    Press = int.Parse(value[value.Length - 1].ToString());
                }
                _serial = value;
            } 
        }
        private string _serial;

        [JsonProperty(PropertyName = "duttime")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "typename")]
        public string TypeName { get; set; }

        [JsonProperty(PropertyName = "system")]
        public string System { get; set; }

        [JsonProperty(PropertyName = "workorder")]
        public string WorkOrder { get; set; }

        [JsonProperty(PropertyName = "executiontime(s)")]
        public float ExecutionTime { get; set; }

        [JsonProperty(PropertyName = "nestnumber")]
        public int Nest { get; set; }

        public int Track { get; set; }
        public int Press { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class AcousticStep
    {
        [JsonProperty(PropertyName = "stepname")]
        public string StepName { get; set; }

        [JsonProperty(PropertyName = "steppass")]
        public bool StepPass { get; set; }

        [JsonProperty(PropertyName = "measurement")]
        public List<double[]> Measurement { get; set; }

        [JsonProperty(PropertyName = "upperlimit")]
        public List<double[]> UpperLimit { get; set; }

        [JsonProperty(PropertyName = "lowerlimit")]
        public List<double[]> LowerLimit { get; set; }
    }
}
