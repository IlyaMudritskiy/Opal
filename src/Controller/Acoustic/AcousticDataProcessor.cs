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

            List<string> matchingFiles = new List<string>();
            List<string> acousticFiles = new List<string>();
            List<AcousticFile> result = new List<AcousticFile>();
            string path;

            for (int i = -1; i < 3; i++)
            {
                path = getPath(files[0], typeID, i);
                acousticFiles = Directory.GetFiles(path).ToList();
                matchingFiles.AddRange(findMatchingFileNames(ref files, ref acousticFiles));
            }

            foreach (var f in JsonReader.ReadFromZip<AcousticFile>(matchingFiles))
                if (f.DUT.Pass)
                    result.Add(f);

            return result;
        }

        private static List<string> findMatchingFileNames(ref List<JsonFile> files, ref List<string> fileNames)
        {
            List<string> matchingFileNames = new List<string>();
            foreach (string fileName in fileNames)
            {
                if (files.Any(obj => fileName.Contains(obj.DUT.SerialNumber)))
                    matchingFileNames.Add(fileName);
            }
            return matchingFileNames;
        }

        private static string getPath(JsonFile file, string typeID, int hourOffset = 0)
        {
            DateTime dt = DateTime.Parse(file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements[0].DateTime);
            string date = $"{dt.Year}{dt.Month}{dt.Day}";
            int hour = dt.Hour + hourOffset;
            return $"Z:\\autolines\\ttl\\acoustic\\{typeID}\\{date}\\{hour}";
        }
    }
}
