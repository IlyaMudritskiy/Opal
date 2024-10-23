using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opal.Model.AppConfiguration;
using Opal.Model.Data.Acoustic;
using Opal.src.CommonClasses.Processing;

namespace Opal.src.TTL.Processing
{
    public static class AcousticDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static Config config = Config.Instance;

        private static string sep_str = Path.DirectorySeparatorChar.ToString();

        private delegate List<AcousticFile> FileOpener(List<JObject> files, string path);

        public static async Task<List<AcousticFile>> GetAcousticFiles(List<JObject> files)
        {
            if (files == null || files.Count == 0) return null;

            if (!config.Acoustic.Enabled) return null;

            if (config.Acoustic.CustomLocationEnabled)
            {
                if (config.ASxReports)
                    return openFromLocalFiles(files, ASxReportsGet);
                else
                    return openFromLocalFiles(files, KlippelReportsGet);
            }
            else // Default local network location
            {
                if (config.ASxReports)
                    return openFromShareDrive(files, ASxReportsGet);
                else
                    return openFromShareDrive(files, KlippelReportsGet);
            }
        }

        #region Limit files

        public static Dictionary<string, Limit> OpenLimitFiles()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\Limits\\{config.ProductID}";

            if (!Directory.Exists(path))
            {
                Log.Debug($"Directory for limits does not exist ({path})");
                return null;
            }

            string[] limitFiles = Directory.GetFiles(path);

            return checkLimitName(limitFiles);
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

        #endregion

        #region Nest number
        private static int getNestNumber(string zipPath)
        {
            if (zipPath.Contains("TestPC1")) return 1;
            if (zipPath.Contains("TestPC2")) return 2;
            if (zipPath.Contains("TestPC3")) return 3;
            if (zipPath.Contains("TestPC4")) return 4;
            return 1;
        }

        #endregion

        #region Open files from local or share drive

        private static List<AcousticFile> openFromShareDrive(List<JObject> files, FileOpener fileOpener)
        {
            var path = config.Acoustic.CustomFilesLocation;

            if (!Directory.Exists(path))
                path = $"{config.DataDriveLetter}:{sep_str}autolines{sep_str}ttl{sep_str}acoustic{sep_str}Reports";

            return fileOpener(files, path);
        }

        private static List<AcousticFile> openFromLocalFiles(List<JObject> files, FileOpener fileOpener)
        {
            string acousticPath = CommonFileManager.GetFolderFromDialog();
            return fileOpener(files, acousticPath);
        }

        #endregion

        #region Open file from ASx or Klippel

        #region ASx

        private static List<AcousticFile> ASxReportsGet(List<JObject> files, string path)
        {
            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<string> correspondingZipFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();

            List<string> serialNumbers = files.Select(x => CommonFileContentManager.GetFieldValue(x, "DUT", "serial_nr").ToString()).ToList();
            string typeid = CommonFileContentManager.GetFieldValue(files[0], "DUT", "type_id").ToString();
            List<DateTime> dts = files.Select(x => DateTime.Parse(files[0]?["Steps"]?[0]?["Measurements"]?[0]?["Date"].ToString())).ToList();

            var zipFiles = Directory.GetFiles(path, $"Y*D*H*.zip", SearchOption.AllDirectories);

            foreach (var processPath in config.ProcessFilePaths)
            {
                var correspondingZipName = "";
                if (Path.GetExtension(processPath).ToLower() == ".json")
                {
                    correspondingZipName = getCorrespondingZipNameFromJson(processPath);
                }
                if (Path.GetExtension(processPath).ToLower() == ".zip")
                {
                    correspondingZipName = getCorrespondingZipNameFromZip(processPath);
                }

                if (string.IsNullOrEmpty(correspondingZipName))
                    continue;

                foreach (var zipfile in zipFiles)
                    if (Path.GetFileName(zipfile).Equals(correspondingZipName) && !correspondingZipFiles.Contains(zipfile))
                        correspondingZipFiles.Add(zipfile);
            }
            //correspondingZipFiles = correspondingZipFiles.Distinct().ToList();
            return ASxReportsOpen(correspondingZipFiles, serialNumbers);
        }

        private static List<AcousticFile> ASxReportsOpen(List<string> correspondingZipFiles, List<string> serialNumbers)
        {
            List<AcousticFile> result = new List<AcousticFile>();

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
                                using (StreamReader reader = new StreamReader(entry.Open()))
                                {
                                    string jsonContent = reader.ReadToEnd();
                                    var acousticFile = JsonConvert.DeserializeObject<AcousticFile>(jsonContent);
                                    acousticFile.DUT.Nest = getNestNumber(zipPath);
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

        #endregion

        #region Klippel

        private static List<AcousticFile> KlippelReportsGet(List<JObject> files, string path)
        {
            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();

            List<string> serialNumbers = files.Select(x => CommonFileContentManager.GetFieldValue(x, "DUT", "serial_nr").ToString()).ToList();

            DateTime dt = DateTime.Parse(files[0]?["Steps"]?[0]?["Measurements"]?[0]?["Date"].ToString());

            for (int i = -1; i < 3; i++)
            {
                string date = dt.AddDays(i).ToString("yyyyMMdd");
                //string path = $"{config.DataDriveLetter}:\\autolines\\ttl\\acoustic\\{config.ProductID}\\{date}";

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

        #endregion

        #endregion

        #region Service methods

        private static string getCorrespondingZipNameFromZip(string processPath)
        {
                string[] segments = processPath.Split(Path.DirectorySeparatorChar);
                DateTime dt = DateTime.ParseExact(segments[segments.Length - 3], "yyyyMMdd", CultureInfo.InvariantCulture);
                dt = dt.AddHours(double.Parse(segments[segments.Length - 2]));

                var year = dt.Year.ToString("D4");
                var day = dt.DayOfYear.ToString("D3");
                var hour = dt.Hour.ToString("D2");

                return $"Y{year}D{day}H{hour}.zip";
        }

        private static string getCorrespondingZipNameFromJson(string processPath)
        {
            var filename = Path.GetFileName(processPath);

            string extractedYear = filename.Substring(0, 2); // First 2 chars
            string extractedDate = filename.Substring(2, 4); // Next 4 chars
            string extractedHour = filename.Substring(6, 2); // Next 2 chars after date

            // Transform year to 2024
            int transformedYear = int.Parse("20" + extractedYear);

            // Convert extracted date to DateTime to calculate day number in the year
            int month = int.Parse(extractedDate.Substring(0, 2));
            int day = int.Parse(extractedDate.Substring(2, 2));
            DateTime date = new DateTime(transformedYear, month, day);

            // Calculate day number in the year
            int dayNumberInYear = date.DayOfYear;

            var res = $"Y{transformedYear}D{dayNumberInYear}H{extractedHour}.zip";

            return res;
        }

        private static List<string> findMatchingFileNames(List<string> serials, IEnumerable<string> fileNames)
        {
            return fileNames
                .Where(filename => serials.Any(serial => filename.Contains(serial))).ToList();
        }

        #endregion

    }
}
