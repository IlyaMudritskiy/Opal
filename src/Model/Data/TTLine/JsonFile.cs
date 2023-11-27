﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcessDashboard.src.Model.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonFile
    {
        [JsonProperty(PropertyName = "DUT")]
        public DUT DUT { get; set; }

        [JsonProperty(PropertyName = "Steps")]
        public List<Step> Steps { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DUT
    {
        [JsonProperty(PropertyName = "type_id")]
        public string TypeID { get; set; }

        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "system_type")]
        public string SystemType { get; set; }

        [JsonProperty(PropertyName = "serial_nr")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "track_nr")]
        public string TrackNumber { get; set; }

        [JsonProperty(PropertyName = "ps01_press_nr")]
        public string PS01PressNumber { get; set; }

        [JsonProperty(PropertyName = "wpc_number")]
        public string WPCNumber { get; set; }

        [JsonProperty(PropertyName = "wpc_height")]
        public string WPCHeight { get; set; }

        [JsonProperty(PropertyName = "machine_id")]
        public string MachineID { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Step
    {
        [JsonProperty(PropertyName = "stepname")]
        public string StepName { get; set; }

        [JsonProperty(PropertyName = "unitx")]
        public string UnitX { get; set; }

        [JsonProperty(PropertyName = "unity")]
        public string UnitY { get; set; }

        [JsonProperty(PropertyName = "Measurements")]
        public List<Measurement> Measurements;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Measurement
    {
        [JsonProperty(PropertyName = "Date")]
        public string DateTime { get; set; }

        [JsonProperty(PropertyName = "MeasurementValue")]
        public string MeasurementValue { get; set; }
    }
}