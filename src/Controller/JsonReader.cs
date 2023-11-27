using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProcessDashboard.src.Controller
{
    public static class JsonReader
    {
        public static T Read<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            try
            {
                string json = File.ReadAllText(filePath);
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
            catch (JsonSerializationException ex)
            {
                throw new JsonSerializationException($"Error deserializing JSON: {ex.Message}", ex);
            }
        }

        public static List<T> Read<T>(string[] filePaths)
        {
            List<T> result = new List<T>();
            foreach (string filePath in filePaths)
            {
                result.Add(Read<T>(filePath));
            }
            return result;
        }

        public static void Write<T>(string filePath, T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data to write cannot be null.");
            }

            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (JsonSerializationException ex)
            {
                throw new JsonSerializationException($"Error serializing data to JSON: {ex.Message}", ex);
            }
        }
    }
}
