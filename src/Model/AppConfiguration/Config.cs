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

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private string path = $"{Directory.GetCurrentDirectory()}\\config.json";

        [JsonProperty(PropertyName = "DataDriveLetter")]
        public string DataDriveLetter { get; set; }

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
                Log.Error(ex);
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
                Log.Error($"Failed to save config to {path}, {ex}");
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
