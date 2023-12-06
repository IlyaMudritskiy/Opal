using ProcessDashboard.src.Model.AppConfiguration;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Data.Acoustic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProcessDashboard.src.Controller.Acoustic
{
    public static class AcousticDataProcessor
    {
        public static List<AcousticFile> OpenFiles(ref List<JsonFile> files, string typeID)
        {
            if (files == null) return null;

            Config config = Config.Instance;

            if (config.Acoustic.IsFilesCustomLocation)
                return fromCustomLocation(ref files, config.Acoustic.CustomFilesPath);
            if (!config.Acoustic.IsFilesCustomLocation)
                return fromDefaultLocation(ref files, typeID);
            else
                return null;
        }

        public static Dictionary<string, Limit> OpenLimitFiles()
        {
            Config config = Config.Instance;

            string[] limitFiles = Directory.GetFiles(config.LimitsFolder);

            return checkLimitName(limitFiles);
        }

        private static List<AcousticFile> fromDefaultLocation(ref List<JsonFile> processFiles, string typeID)
        {
            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();

            DateTime dt = DateTime.Parse(processFiles[0].Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements[0].DateTime);

            for (int i = -1; i < 3; i++)
            {
                string date = dt.AddDays(i).ToString("yyyyMMdd");
                string path = $"Z:\\autolines\\ttl\\acoustic\\{typeID}\\{date}";

                if (!Directory.Exists(path)) continue;

                string[] hourDirs = Directory.GetDirectories(path);

                foreach (var dir in hourDirs)
                {
                    string[] hourFiles = Directory.GetFiles(dir);
                    matchingFiles.AddRange(findMatchingFileNames(ref processFiles, hourFiles));
                }
            }

            foreach (var f in JsonReader.ReadFromZip<AcousticFile>(matchingFiles))
                if (f.DUT.Pass)
                    result.Add(f);

            return result;
        }

        private static List<AcousticFile> fromCustomLocation(ref List<JsonFile> processFiles, string path)
        {
            if (!Directory.Exists(path)) return null;

            List<AcousticFile> result = new List<AcousticFile>();

            string[] files = Directory.GetFiles(path);

            List<string> matchingFiles = findMatchingFileNames(ref processFiles, files);

            foreach (var f in JsonReader.ReadFromZip<AcousticFile>(matchingFiles))
                if (f.DUT.Pass)
                    result.Add(f);

            return result;
        }

        private static Dictionary<string, Limit> checkLimitName(string[] filepaths)
        {
            string[] limitNames = {
                "FRUpper", "FRLower", "FRReference",
                "THDUpper", "THDLower", "THDReference",
                "RNBUpper", "RNBLower", "RNBReference",
                "IMPUpper", "IMPLower", "IMPReference"
            };

            Dictionary<string, Limit> result = new Dictionary<string, Limit>();

            foreach (string name in limitNames) result[name] = null;

            foreach (string file in filepaths)
            {
                if (file.Contains("FR"))
                {
                    (string type, Limit limit) = checkLimitType(file, "FR");
                    result[type] = limit;
                }
                if (file.Contains("THD"))
                {
                    (string type, Limit limit) = checkLimitType(file, "THD");
                    result[type] = limit;
                }
                if (file.Contains("RNB"))
                {
                    (string type, Limit limit) = checkLimitType(file, "RNB");
                    result[type] = limit;
                }
                if (file.Contains("IMP"))
                {
                    (string type, Limit limit) = checkLimitType(file, "IMP");
                    result[type] = limit;
                }
            }
            return result;
        }

        private static (string, Limit) checkLimitType(string filepath, string limitName)
        {
            if (!File.Exists(filepath)) return (null, null);

            string filename = Path.GetFileName(filepath);

            if (filename.Contains("Upper"))
                return ($"{limitName}Upper", new Limit(filepath));
            if (filename.Contains("Lower"))
                return ($"{limitName}Lower", new Limit(filepath));
            if (filename.Contains("Reference"))
                return ($"{limitName}Reference", new Limit(filepath));

            return (null, null);
        }

        private static List<string> findMatchingFileNames(ref List<JsonFile> files, string[] fileNames)
        {
            List<string> matchingFileNames = new List<string>();

            foreach (string fileName in fileNames)
                if (files.Any(obj => fileName.Contains(obj.DUT.SerialNumber)))
                    matchingFileNames.Add(fileName);

            return matchingFileNames;
        }
    }
}
