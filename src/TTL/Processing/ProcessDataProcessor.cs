using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Opal.src.CommonClasses.Containers;
using Opal.src.CommonClasses.Processing;
using Opal.src.TTL.Containers.FileContent;

namespace Opal.src.TTL.Processing
{
    public static class ProcessDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static async Task<List<ProcessFile>> GetProcessFiles(List<JObject> files)
        {
            List<ProcessFile> processFiles = CommonFileManager.OpenTo<ProcessFile>(files);
            return checkFilesContent(processFiles);
        }

        public static async Task<ProcessFile> GetProcessFile(JObject file)
        {
            ProcessFile processFile = CommonFileManager.OpenTo<ProcessFile>(file);
            return checkFileContent(processFile);
        }

        private static List<ProcessFile> checkFilesContent(List<ProcessFile> files)
        {
            if (files == null || files.Count == 0) return null;

            // Maybe use concurrency here
            List<ProcessFile> result = new List<ProcessFile>();

            foreach (var file in files)
            {
                var checkedFile = checkFileContent(file);
                if (checkedFile != null)
                {
                    result.Add(checkedFile);
                }
                else
                {
                    continue;
                }
            }
            return result;
        }

        private static ProcessFile checkFileContent(ProcessFile file)
        {
            var temp = new Measurements2D(file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements);
            var press = new Measurements2D(file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements);
            var heater = file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault();

            if (heater == null || heater.Measurements.Count != 2)
            {
                Log.Warn($"File [{file.DUT.SerialNumber}] ps01_heater_on step is wrong.");
                return null;
            }
                

            if ((temp.MaxX() + press.MaxX()) / 2 > 25)
            {
                Log.Warn($"File [{file.DUT.SerialNumber}] temperature max time ({temp.MaxX()}) and pressure max time ({press.MaxX()}) are off.");
                return null;
            }

            return file;
        }
    }
}
