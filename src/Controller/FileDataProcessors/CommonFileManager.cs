using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.Controller.FileProcessors
{
    public static class CommonFileManager
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static List<string> GetFilePaths(ref OpenFileDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileNames.ToList();
            else
            {
                Log.Info("No files were selected in OpenFileDialog");
                return null;
            }
        }

        public static List<JObject> GetFiles(List<string> filepaths)
        {
            List<JObject> result = new List<JObject>();

            foreach (var file in filepaths)
                result.Add(ParseJson(file));

            return result;
        }

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

        public static string GetLineCode(IEnumerable<string> filepaths)
        {
            if (filepaths == null && filepaths.Count() == 0) return null;

            return getFieldValue(filepaths.First(), "machine_id");
        }

        public static string GetLineCode(IEnumerable<JObject> filepaths)
        {
            if (filepaths == null && filepaths.Count() == 0) return null;

            return getFieldValue(filepaths.First(), "machine_id");
        }

        public static string GetProductCode(IEnumerable<JObject> filepaths)
        {
            if (filepaths == null && filepaths.Count() == 0) return null;

            return getFieldValue(filepaths.First(), "type_id");
        }

        public static string GetProductCode(IEnumerable<string> filepaths)
        {
            if (filepaths == null && filepaths.Count() == 0) return null;

            return getFieldValue(filepaths.First(), "type_id");
        }

        public static string GetLineCode(JObject file)
        {
            if (file == null) return null;

            return getFieldValue(file, "machine_id");
        }

        public static string GetProductCode(JObject file)
        {
            if (file == null) return null;

            return getFieldValue(file, "type_id");
        }

        public static void CheckProductCode()
        {

        }

        public static void CheckLineCode()
        {

        }

        public static JObject ParseJson(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Log.Info($"File [{filepath}] does not exist.");
                return null;
            }

            return JObject.Parse(filepath);
        }

        private static string getFieldValue(string filepath, string fieldName)
        {
            if (!File.Exists(filepath)) return null;
                
            return getFieldValue(ParseJson(filepath), fieldName);
        }

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
    }
}
