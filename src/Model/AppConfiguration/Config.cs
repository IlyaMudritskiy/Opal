using Newtonsoft.Json;
using System;
using System.IO;

namespace ProcessDashboard.src.Model.AppConfiguration
{
    /// <summary>
    /// Json props of the Config
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Config
    {
        public string PrductID { get; set; }
        [JsonProperty(PropertyName = "DataDriveLetter")]
        public string DataDriveLetter { get; set; }

        [JsonProperty(PropertyName = "Acoustic")]
        public Acoustic Acoustic { get; set; }

        [JsonProperty(PropertyName = "EmbossingConstants")]
        public EmbossingConstants EmbossingConstants { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Acoustic
    {
        [JsonProperty(PropertyName = "Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "ManualSelection")]
        public bool ManualSelection { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class EmbossingConstants
    {
        [JsonProperty(PropertyName = "TD")]
        public double TD { get; set; }

        [JsonProperty(PropertyName = "t9const")]
        public double t9const { get; set; }

        [JsonProperty(PropertyName = "tc")]
        public double tc { get; set; }

        [JsonProperty(PropertyName = "tHP")]
        public double tHP { get; set; }

        [JsonProperty(PropertyName = "tsettle")]
        public double tsettle { get; set; }

        [JsonProperty(PropertyName = "P1")]
        public double P1 { get; set; }

        [JsonProperty(PropertyName = "RoundTo")]
        public int RoundTo { get; set; }
    }

    /// <summary>
    /// Private props and methods of Config class
    /// </summary>
    public partial class Config
    {
        private static readonly Lazy<Config> lazy = new Lazy<Config>(() => new Config());
        public static Config Instance => lazy.Value;

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private string path = $"{Directory.GetCurrentDirectory()}\\config.json";
        public string ProductID { get; set; }

        private Config()
        {
            Read();
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

        public void Read()
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
    }
}
