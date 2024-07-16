﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ProcessDashboard.Model.AppConfiguration
{
    /// <summary>
    /// Json props of the Config
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Config
    {
        public string ProductID { get; set; }

        [JsonProperty(PropertyName = "data-drive-letter")]
        public string DataDriveLetter { get; set; }

        [JsonProperty(PropertyName = "asx-compliant-mode")]
        public bool ASxReports { get; set; }

        [JsonProperty(PropertyName = "acoustic")]
        public Acoustic Acoustic { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Acoustic
    {
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

        public bool CustomLocationEnabled
        {
            get
            {
                return string.IsNullOrEmpty(CustomFilesLocation);
            }
        }

        [JsonProperty(PropertyName = "files-custom-location")]
        public string CustomFilesLocation { get; set; }
    }

    /// <summary>
    /// Private props and methods of Config class
    /// </summary>
    public partial class Config
    {
        public List<string> ProcessFilePaths { get; set; } = new List<string>();
        private static readonly Lazy<Config> lazy = new Lazy<Config>(() => new Config());
        public static Config Instance => lazy.Value;

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private string path = $"{Assembly.GetEntryAssembly().Location}\\app-config.json";

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
                Log.Error($"Failed to save config to {path}, exception:\n{ex}");
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
                Log.Error($"Failed to read config at {path}, exception:\n{ex}");
            }
        }
    }
}
