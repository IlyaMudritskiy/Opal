using Newtonsoft.Json;
using System;
using System.IO;

namespace ProcessDashboard.src.Model.AppConfiguration
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Config
    {
        private static readonly Lazy<Config> lazy = new Lazy<Config>(() => new Config());
        public static Config Instance => lazy.Value;

        private string path = $"{Directory.GetCurrentDirectory()}\\Config\\config.json";

        [JsonProperty(PropertyName = "LimitsFolder")]
        public string LimitsFolder { get; set; }

        [JsonProperty(PropertyName = "Acoustic")]
        public Acoustic Acoustic { get; set; }

        private Config()
        {
            try
            {
                string content = File.ReadAllText(path);
                JsonConvert.PopulateObject(content, this);
            }
            catch (Exception ex)
            {
                // Log config file does not exist
            }
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception ex)
            {
                // Log
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Acoustic
    {
        [JsonProperty(PropertyName = "Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "ManualSelection")]
        public bool ManualSelection { get; set; }
    }
}
