using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Opal.src.CommonClasses.Processing
{
    public static class CommonFileManager
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            DateParseHandling = DateParseHandling.None
        };

        #region JObject to <T>

        /// <summary>
        /// Converts a JSON object (JObject) to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the JSON object should be converted.</typeparam>
        /// <param name="file">The JSON object (JObject) to be converted.</param>
        /// <returns>
        /// An instance of type <typeparamref name="T"/> representing the converted JSON object.
        /// If the conversion fails, logs a warning and returns the default value for type <typeparamref name="T"/>.
        /// </returns>
        public static T OpenTo<T>(JObject file)
        {
            try
            {
                return file.ToObject<T>();
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not transform file from JObject. Message: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// Opens and deserializes a list of JSON objects into a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to deserialize the JSON into.</typeparam>
        /// <param name="files">The list of JSON objects to open and deserialize.</param>
        /// <returns>
        /// A list of objects of type <typeparamref name="T"/> representing the deserialized content of the JSON objects.
        /// If the input list is null or empty, logs a trace message and returns null.
        /// </returns>
        public static List<T> OpenTo<T>(List<JObject> files)
        {
            if (files == null || files.Count() == 0)
            {
                Log.Trace("Input argument is empty");
                return null;
            }

            ConcurrentBag<T> result = new ConcurrentBag<T>();

            Parallel.ForEach(files, f => { result.Add(OpenTo<T>(f)); });

            return result.ToList();
        }

        #endregion

        #region File path to <T>
        /// <summary>
        /// Opens and deserializes a JSON file at the specified file path into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the JSON into.</typeparam>
        /// <param name="filePath">The path of the JSON file to open and deserialize.</param>
        /// <returns>
        /// An instance of type <typeparamref name="T"/> representing the deserialized content of the JSON file.
        /// If the file does not exist, logs an error and returns the default value for type <typeparamref name="T"/>.
        /// If there is an error during deserialization, logs an error and returns the default value for type <typeparamref name="T"/>.
        /// </returns>
        public static T OpenTo<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Log.Warn($"The file '{filePath}' does not exist");
                return default;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                T result = JsonConvert.DeserializeObject<T>(json, JsonSettings);
                return result;
            }
            catch (JsonSerializationException ex)
            {
                Log.Warn($"Error deserializing JSON: {ex.Message}");
            }
            return default;
        }

        public static async Task<T> OpenToAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Log.Warn($"The file '{filePath}' does not exist");
                return default;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                T result = JsonConvert.DeserializeObject<T>(json, JsonSettings);
                return result;
            }
            catch (JsonSerializationException ex)
            {
                Log.Warn($"Error deserializing JSON: {ex.Message}");
            }
            return default;
        }

        /// <summary>
        /// Opens and deserializes a collection of JSON files at the specified file paths into a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to deserialize the JSON files into.</typeparam>
        /// <param name="filePaths">The collection of file paths for the JSON files to open and deserialize.</param>
        /// <returns>
        /// A list of instances of type <typeparamref name="T"/> representing the deserialized content of the JSON files.
        /// </returns>
        public static List<T> OpenTo<T>(IEnumerable<string> filePaths)
        {
            ConcurrentBag<T> result = new ConcurrentBag<T>();

            Parallel.ForEach(filePaths, f =>
            {
                result.Add(OpenTo<T>(f));
            });

            return result.ToList();
        }

        public async static Task<List<T>> OpenToAsync<T>(IEnumerable<string> filePaths)
        {
            ConcurrentBag<T> result = new ConcurrentBag<T>();

            foreach (var f in  filePaths)
            {
                var cont = await OpenToAsync<T>(f);
                result.Add(cont);
            }

            return result.ToList();
        }

        #endregion

        #region ZIP content to <T>

        /// <summary>
        /// Opens and deserializes the content of a JSON file within a ZIP archive into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the JSON content into.</typeparam>
        /// <param name="zipFilePath">The path of the ZIP file containing a single JSON file entry.</param>
        /// <returns>
        /// An instance of type <typeparamref name="T"/> representing the deserialized content of the JSON file.
        /// If the ZIP file does not contain exactly one entry (JSON file), logs an error and returns the default value for type <typeparamref name="T"/>.
        /// If there is an error during deserialization or accessing the ZIP file, logs an error and returns the default value for type <typeparamref name="T"/>.
        /// </returns>
        public static T OpenZipContentTo<T>(string zipFilePath)
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
                    T result = JsonConvert.DeserializeObject<T>(json, JsonSettings);
                    return result;
                }
            }
        }

        /// <summary>
        /// Opens and deserializes the content of JSON files within ZIP archives at the specified file paths into a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to deserialize the JSON content into.</typeparam>
        /// <param name="zipFilePaths">The collection of file paths for the ZIP files containing JSON file entries.</param>
        /// <returns>
        /// A list of instances of type <typeparamref name="T"/> representing the deserialized content of the JSON files.
        /// </returns>
        public static List<T> OpenZipContentTo<T>(IEnumerable<string> zipFilePaths)
        {
            ConcurrentBag<T> result = new ConcurrentBag<T>();

            Parallel.ForEach(zipFilePaths, f =>
            {
                result.Add(OpenZipContentTo<T>(f));
            });

            return result.ToList();
        }

        #endregion

        #region Parse .json to JObject

        /// <summary>
        /// Parses the content of a JSON file at the specified file path into a <see cref="JObject"/>.
        /// </summary>
        /// <param name="filepath">The path of the JSON file to parse.</param>
        /// <returns>
        /// A <see cref="JObject"/> representing the parsed content of the JSON file.
        /// If the file does not exist, logs an informational message and returns null.
        /// If there is an error during parsing, returns null.
        /// </returns>
        public static JObject ParseJsonFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Log.Info($"File [{filepath}] does not exist.");
                return null;
            }

            string content;
            using (StreamReader r = new StreamReader(filepath))
            {
                content = r.ReadToEnd();
            }
            var res = JObject.Parse(content);
            return res;
        }

        public async static Task<JObject> ParseJsonFileAsync(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Log.Info($"File [{filepath}] does not exist.");
                return null;
            }

            string content;
            using (StreamReader r = new StreamReader(filepath))
            {
                content = r.ReadToEnd();
            }
            var res = JObject.Parse(content);
            return res;
        }

        /// <summary>
        /// Checks for existance and reads all files in filepaths to list of generic JOobject objects.
        /// </summary>
        /// <param name="filepaths">List of full paths to files.</param>
        /// <returns>List of Json Objects of selected files.</returns>
        public static List<JObject> ParseJsonFiles(List<string> filepaths)
        {
            List<JObject> result = new List<JObject>();

            foreach (var file in filepaths)
                result.Add(ParseJsonFile(file));

            return result;
        }

        /// <summary>
        /// Checks for existance and asynchronosly reads all files in filepaths to list of generic JOobject objects.
        /// </summary>
        /// <param name="filepaths">List of full paths to files.</param>
        /// <returns>List of Json Objects of selected files.</returns>
        public async static Task<List<JObject>> ParseJsonFilesAsync(List<string> filepaths)
        {
            List<JObject> result = new List<JObject>();

            foreach (var file in filepaths)
                result.Add(await ParseJsonFileAsync(file));

            return result;
        }

        #endregion

        #region File Dialog and Write

        public static List<string> GetFilesFromDialog()
        {
            List<string> result = new List<string>();

            Thread t = new Thread((ThreadStart)(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true };
                if (dialog.ShowDialog() == DialogResult.OK)
                    result = dialog.FileNames.ToList();
                else
                {
                    Log.Info("No files were selected in OpenFileDialog");
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return result;
        }

        public static string GetFolderFromDialog()
        {
            string result = "";

            Thread t = new Thread((ThreadStart)(() =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    result = dialog.SelectedPath;
                else
                {
                    Log.Info("No folder was selected on FolderBrowserDialog");
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return result;
        }

        /// <summary>
        /// Writes the serialized JSON representation of an object of type <typeparamref name="T"/> to a file.
        /// </summary>
        /// <typeparam name="T">The type of the data to be serialized and written.</typeparam>
        /// <param name="filePath">The path of the file to write the JSON data to.</param>
        /// <param name="data">The data of type <typeparamref name="T"/> to be serialized and written.</param>
        public static void Write<T>(string filePath, T data)
        {
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

        #endregion
    }
}
