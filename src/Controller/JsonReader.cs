using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessDashboard.src.Controller
{
    public static class JsonReader
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static T Read<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Log.Error($"The file '{filePath}' does not exist");
                return default(T);
            }
                
            try
            {
                string json = File.ReadAllText(filePath);
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
            catch (JsonSerializationException ex)
            {
                Log.Error($"Error deserializing JSON: {ex.Message}");
            }
            return default(T);
        }

        public static List<T> Read<T>(IEnumerable<string> filePaths)
        {
            ConcurrentBag<T> result = new ConcurrentBag<T>();

            Parallel.ForEach(filePaths, f => {
                result.Add(Read<T>(f));
            });

            return result.ToList();
        }

        public static T ReadFromZip<T>(string zipFilePath)
        {
            using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read))
            using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                if (zipArchive.Entries.Count != 1)
                { 
                    Log.Error("The zip file must contain exactly one entry (JSON file).");
                }

                ZipArchiveEntry entry = zipArchive.Entries[0];

                using (Stream entryStream = entry.Open())
                using (StreamReader reader = new StreamReader(entryStream))
                {
                    string json = reader.ReadToEnd();
                    T result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
            }
        }

        public static List<T> ReadFromZip<T>(IEnumerable<string> zipFilePaths)
        {
            ConcurrentBag<T> result = new ConcurrentBag<T>();

            Parallel.ForEach(zipFilePaths, f => {
                result.Add(ReadFromZip<T>(f));
            });

            return result.ToList();
        }

        public static void Write<T>(string filePath, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Data to write cannot be null.");

            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (JsonSerializationException ex)
            {
                Log.Error($"Error serializing data to JSON: {ex.Message}", ex);
            }
        }
    }
}
