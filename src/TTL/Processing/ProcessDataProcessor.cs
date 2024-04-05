using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.CommonClasses.Processing;
using ProcessDashboard.src.TTL.Containers.FileContent;

namespace ProcessDashboard.src.TTL.Processing
{
    public static class ProcessDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static List<ProcessFile> GetProcessFiles(ref List<JObject> files)
        {
            List<ProcessFile> processFiles = CommonFileManager.OpenTo<ProcessFile>(files);
            return checkFilesContent(ref processFiles);
        }

        private static List<ProcessFile> checkFilesContent(ref List<ProcessFile> files)
        {
            if (files == null || files.Count == 0) return null;

            // Maybe use concurrency here
            List<ProcessFile> result = new List<ProcessFile>();

            foreach (var file in files)
            {
                var temp = new Measurements2D(file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements);
                var press = new Measurements2D(file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements);
                var heater = file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault();

                if (heater == null || heater.Measurements.Count != 2)
                    continue;

                if ((temp.MaxX() + press.MaxX()) / 2 > 25)
                    continue;

                result.Add(file);
            }
            return result;
        }
    }
}
