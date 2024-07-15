using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ProcessDashboard.src.CommonClasses.Processing
{
    public static class CommonFileContentManager
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        #region Get Field Value (from DUT)
        /// <summary>
        /// Retrieves the value of a specified field within a JSON file.
        /// </summary>
        /// <param name="filepath">The path of the JSON file to read.</param>
        /// <param name="fieldName">The name of the field whose value is to be retrieved.</param>
        /// <returns>
        /// The value of the specified field within the JSON file.
        /// If the file does not exist, returns null.
        /// If the field does not exist or there is an error during parsing, returns null.
        /// </returns>
        private static string getFieldValue(string filepath, string fieldName)
        {
            if (!File.Exists(filepath)) return null;

            return getFieldValue(CommonFileManager.ParseJsonFile(filepath), fieldName);
        }

        /// <summary>
        /// Retrieves the value of a specified field within a JSON object.
        /// </summary>
        /// <param name="file">The JSON object from which to retrieve the field value.</param>
        /// <param name="fieldName">The name of the field whose value is to be retrieved.</param>
        /// <returns>
        /// The value of the specified field within the JSON object.
        /// If the JSON object is null, returns null.
        /// If the field does not exist or there is an error during retrieval, returns null.
        /// </returns>
        private static string getFieldValue(JObject file, string fieldName)
        {
            if (file == null) return null;

            JToken fieldValue = file["DUT"]?[fieldName];

            if (fieldValue == null)
            {
                Log.Info($"File does not contain field {fieldName}.");
                return null;
            }

            return fieldValue.ToString();
        }

        public static string GetFieldValue(JObject file, string section, string fieldName)
        {
            if (file == null) return null;

            try
            {
                JToken fieldValue = file[section]?[fieldName];
                return fieldValue.ToString();
            }
            catch (Exception ex)
            {
                Log.Error($"[Exception] {ex}\n[Message] {ex.Message}");
                Log.Info($"File does not contain field {section} -> {fieldName}");
                return null;
            }
        }

        #endregion

        #region Get Line Code

        /// <summary>
        /// Retrieves the line code from a JSON object by extracting the value of the "machine_id" field.
        /// </summary>
        /// <param name="file">The JSON object from which to retrieve the line code.</param>
        /// <returns>
        /// The line code extracted from the "machine_id" field of the JSON object.
        /// If the JSON object is null or the "machine_id" field does not exist, returns null.
        /// </returns>
        public static string GetLineCode(JObject file)
        {
            if (file == null) return null;

            return getFieldValue(file, "machine_id");
        }

        /// <summary>
        /// Retrieves the line code from the first JSON object in a list by extracting the value of the "machine_id" field.
        /// </summary>
        /// <param name="files">The list of JSON objects from which to retrieve the line code.</param>
        /// <returns>
        /// The line code extracted from the "machine_id" field of the first JSON object in the list.
        /// If the list is null or empty, or the "machine_id" field does not exist, returns null.
        /// </returns>
        public static string GetLineCode(List<JObject> files)
        {
            if (files == null || files.Count() == 0) return null;

            return getFieldValue(files.First(), "machine_id");
        }

        /// <summary>
        /// Retrieves the line code from the first JSON file in a list of file paths by extracting the value of the "machine_id" field.
        /// </summary>
        /// <param name="filepaths">The list of file paths representing JSON files from which to retrieve the line code.</param>
        /// <returns>
        /// The line code extracted from the "machine_id" field of the first JSON file in the list.
        /// If the list is null or empty, or the "machine_id" field does not exist, returns null.
        /// </returns>
        public static string GetLineCode(List<string> filepaths)
        {
            if (filepaths == null || filepaths.Count() == 0) return null;

            return getFieldValue(filepaths.First(), "machine_id");
        }

        #endregion

        #region Get Product Code

        /// <summary>
        /// Retrieves the product code from a JSON object by extracting the value of the "type_id" field.
        /// </summary>
        /// <param name="file">The JSON object from which to retrieve the product code.</param>
        /// <returns>
        /// The product code extracted from the "type_id" field of the JSON object.
        /// If the JSON object is null or the "type_id" field does not exist, returns null.
        /// </returns>
        public static string GetProductCode(JObject file)
        {
            if (file == null) return null;

            return getFieldValue(file, "type_id");
        }

        /// <summary>
        /// Retrieves the product code from the first JSON object in a list by extracting the value of the "type_id" field.
        /// </summary>
        /// <param name="files">The list of JSON objects from which to retrieve the product code.</param>
        /// <returns>
        /// The product code extracted from the "type_id" field of the first JSON object in the list.
        /// If the list is null or empty, or the "type_id" field does not exist, returns null.
        /// </returns>
        public static string GetProductCode(List<JObject> files)
        {
            if (files == null || files.Count() == 0) return null;

            return getFieldValue(files.First(), "type_id");
        }

        /// <summary>
        /// Retrieves the product code from the first JSON file in a list of file paths by extracting the value of the "type_id" field.
        /// </summary>
        /// <param name="filepaths">The list of file paths representing JSON files from which to retrieve the product code.</param>
        /// <returns>
        /// The product code extracted from the "type_id" field of the first JSON file in the list.
        /// If the list is null or empty, or the "type_id" field does not exist, returns null.
        /// </returns>
        public static string GetProductCode(List<string> filepaths)
        {
            if (filepaths == null || filepaths.Count() == 0) return null;

            return getFieldValue(filepaths.First(), "type_id");
        }

        #endregion

        #region Filter opened files

        /// <summary>
        /// Filters a list of JSON objects based on a specified product code.
        /// </summary>
        /// <param name="files">The list of JSON objects to filter.</param>
        /// <param name="lineCode">The product code to use for filtering.</param>
        /// <returns>
        /// A new list containing only the JSON objects from the input list
        /// whose product code matches the specified product code.
        /// If the input list is null or empty, returns null.
        /// </returns>
        public static List<JObject> FilterByProduct(List<JObject> files, string productCode)
        {
            if (files == null || files.Count == 0) return null;

            List<JObject> result = new List<JObject>();

            foreach (var file in files)
            {
                if (GetProductCode(file) == productCode)
                    result.Add(file);
            }
            return result;
        }

        /// <summary>
        /// Filters a list of JSON objects based on a specified line code.
        /// </summary>
        /// <param name="files">The list of JSON objects to filter.</param>
        /// <param name="lineCode">The line code to use for filtering.</param>
        /// <returns>
        /// A new list containing only the JSON objects from the input list
        /// whose line code matches the specified line code.
        /// If the input list is null or empty, returns null.
        /// </returns>
        public static List<JObject> FilterByLine(List<JObject> files, string lineCode)
        {
            if (files == null || files.Count == 0) return null;

            List<JObject> result = new List<JObject>();

            foreach (var file in files)
            {
                if (GetLineCode(file) == lineCode)
                    result.Add(file);
            }
            return result;
        }

        #endregion
    }
}
