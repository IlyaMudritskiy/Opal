using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            if (!config.Acoustic.ManualSelection && !config.IsASxReports)
                return fromDefaultLocation(ref files);
            //if (config.IsASxReports)
                //return fromDefaultLocationNew(ref files);

            return null;
        }

        public static List<AcousticFile> GetAcousticFiles(ref List<JObject> files)
        {
            if (files == null || files.Count == 0) return null;

            if (config.Acoustic.ManualSelection)
            {
                List<string> filepaths = CommonFileManager.GetFilesFromDialog();
                if (filepaths == null || filepaths.Count == 0) return null;
            }

            if (!config.Acoustic.ManualSelection && !config.IsASxReports)
                return fromDefaultLocation(files);
            if (config.IsASxReports)
                return fromDefaultLocationNew(files);

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

        private static List<AcousticFile> fromDefaultLocation(List<JObject> files)
        {
            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();

            List<string> serialNumbers = files.Select(x => CommonFileContentManager.GetFieldValue(x, "DUT", "serial_nr").ToString()).ToList();

            DateTime dt = DateTime.Parse(files[0]?["Steps"]?[0]?["Measurements"]?[0]?["Date"].ToString());

            //DateTime dt = DateTime.Parse(files[0].Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements[0].DateTime);

            for (int i = -1; i < 3; i++)
            {
                string date = dt.AddDays(i).ToString("yyyyMMdd");
                string path = $"{config.DataDriveLetter}:\\autolines\\ttl\\acoustic\\{config.ProductID}\\{date}";

                if (!Directory.Exists(path)) continue;

                string[] hourDirs = Directory.GetDirectories(path);

                foreach (var dir in hourDirs)
                {
                    string[] hourFiles = Directory.GetFiles(dir);
                    matchingFiles.AddRange(findMatchingFileNames(serialNumbers, hourFiles));
                }
            }

            foreach (var f in CommonFileManager.OpenZipContentTo<AcousticFile>(matchingFiles))
                result.Add(f);

            return result;
        }

        private static List<AcousticFile> fromDefaultLocationNew(List<JObject> files)
        {
            string sep = Path.DirectorySeparatorChar.ToString();
            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();

            List<string> serialNumbers = files.Select(x => CommonFileContentManager.GetFieldValue(x, "DUT", "serial_nr").ToString()).ToList();
            string typeid = CommonFileContentManager.GetFieldValue(files[0], "DUT", "type_id").ToString();
            List<DateTime> dts = files.Select(x => DateTime.Parse(files[0]?["Steps"]?[0]?["Measurements"]?[0]?["Date"].ToString())).ToList();

            string defaultPath = $"{config.DataDriveLetter}:{sep}autolines{sep}ttl{sep}acoustic{sep}Reports";

            //string defaultPath = Path.Combine(config.DataDriveLetter, sep, "autolines", "ttl", "acoustic", "Reports");

            var zipFiles = Directory.GetFiles(defaultPath, $"Y*D*H*.zip", SearchOption.AllDirectories);
            List<string> correspondingZipFiles = new List<string>();

            var processFilesPaths = config.ProcessFilePaths;

            foreach (var processPath in processFilesPaths)
            {
                string filename = Path.GetFileName(processPath);
                string[] segments = processPath.Split(Path.DirectorySeparatorChar);

                DateTime dt = DateTime.ParseExact(segments[segments.Length - 3], "yyyyMMdd", CultureInfo.InvariantCulture);
                dt = dt.AddHours(double.Parse(segments[segments.Length - 2]));

                var year = dt.Year.ToString("D4");
                var day = dt.DayOfYear.ToString("D3");
                var hour = dt.Hour.ToString("D2");

                var correspondingZipName = $"Y{year}D{day}H{hour}.zip";

                foreach (var zipfile in zipFiles)
                {
                    if (Path.GetFileName(zipfile).Equals(correspondingZipName))
                    {
                        correspondingZipFiles.Add(zipfile);
                    }
                }
            }

            correspondingZipFiles = correspondingZipFiles.Distinct().ToList();

            foreach (var zipPath in correspondingZipFiles)
            {
                using (FileStream zipStream = new FileStream(zipPath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            // Check if the entry is a JSON file and contains a serial number in its name
                            if (entry.Name.EndsWith(".json") &&
                                serialNumbers.Any(serial => entry.Name.Contains(serial)))
                            {
                                // Read the content of the JSON file
                                using (StreamReader reader = new StreamReader(entry.Open()))
                                {
                                    string jsonContent = reader.ReadToEnd();
                                    // Deserialize the JSON content into an AcousticFile object
                                    var acousticFile = JsonConvert.DeserializeObject<AcousticFile>(jsonContent);
                                    acousticFile.DUT.Nest = getNestNumber(zipPath);
                                    // Add the deserialized object to the result list
                                    result.Add(acousticFile);

                                    // Remove the matched serial number from the list
                                    serialNumbers.RemoveAll(serial => entry.Name.Contains(serial));
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        
        private static int getNestNumber(string zipPath)
        {
            if (zipPath.Contains("TestPC1")) return 1;
            if (zipPath.Contains("TestPC2")) return 2;
            if (zipPath.Contains("TestPC3")) return 3;
            if (zipPath.Contains("TestPC4")) return 4;
            return 1;
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

        private static List<string> findMatchingFileNames(List<string> serials, IEnumerable<string> fileNames)
        {
            return fileNames
                .Where(filename => serials.Any(serial => filename.Contains(serial))).ToList();
        }
    }
}
