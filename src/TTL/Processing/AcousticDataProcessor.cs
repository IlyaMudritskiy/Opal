using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.CommonClasses.Processing;
using ProcessDashboard.src.TTL.Containers.FileContent;

namespace ProcessDashboard.src.TTL.Processing
{
    public static class AcousticDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static Config config = Config.Instance;

        public static List<AcousticFile> GetAcousticFiles(ref List<ProcessFile> files)
        {
            if (files == null || files.Count == 0) return null;

            if (config.Acoustic.ManualSelection)
            {
                List<string> filepaths = CommonFileManager.GetFilesFromDialog();
                if (filepaths == null || filepaths.Count == 0) return null;
            }
            /*else
            {
                return manualSelected(ref files, CommonFileManager.GetFilesFromDialog());
            }*/

            if (!config.Acoustic.ManualSelection)
                return fromDefaultLocation(ref files);

            return null;
        }

        public static Dictionary<string, Limit> OpenLimitFiles()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\Limits\\{config.ProductID}";

            if (!Directory.Exists(path))
            {
                Log.Error($"Directory for limits does not exist ({path})");
                return null;
            }

            string[] limitFiles = Directory.GetFiles(path);

            return checkLimitName(limitFiles);
        }

        private static List<AcousticFile> fromDefaultLocation(ref List<ProcessFile> processFiles)
        {
            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();

            DateTime dt = DateTime.Parse(processFiles[0].Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements[0].DateTime);

            for (int i = -1; i < 3; i++)
            {
                string date = dt.AddDays(i).ToString("yyyyMMdd");
                string path = $"{config.DataDriveLetter}:\\autolines\\ttl\\acoustic\\{config.ProductID}\\{date}";

                if (!Directory.Exists(path)) continue;

                string[] hourDirs = Directory.GetDirectories(path);

                foreach (var dir in hourDirs)
                {
                    string[] hourFiles = Directory.GetFiles(dir);
                    matchingFiles.AddRange(findMatchingFileNames(ref processFiles, hourFiles));
                }
            }

            foreach (var f in CommonFileManager.OpenZipContentTo<AcousticFile>(matchingFiles))
                result.Add(f);

            return result;
        }

        private static List<AcousticFile> manualSelected(ref List<ProcessFile> processFiles, IEnumerable<string> acousticFiles)
        {
            if (acousticFiles == null || acousticFiles.Count() == 0) return null;

            List<AcousticFile> result = new List<AcousticFile>();

            List<string> matchingFiles = findMatchingFileNames(ref processFiles, acousticFiles);

            foreach (var f in CommonFileManager.OpenZipContentTo<AcousticFile>(matchingFiles))
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

        private static List<string> findMatchingFileNames(ref List<ProcessFile> files, IEnumerable<string> fileNames)
        {
            List<string> matchingFileNames = new List<string>();

            foreach (string fileName in fileNames)
                if (files.Any(obj => fileName.Contains(obj.DUT.SerialNumber)))
                    matchingFileNames.Add(fileName);

            return matchingFileNames;
        }
    }
}
