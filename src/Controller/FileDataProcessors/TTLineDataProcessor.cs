using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Forms;
using ProcessDashboard.src.Model.Data.TTLine;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ProcessDashboard.src.Controller.TTLine
{
    public static class TTLineDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public static List<JsonFile> OpenFiles(IEnumerable<string> files)
        {
            if (files == null || files.Count() == 0)
            {
                Log.Trace("No files were passed to method");
                return null;
            }

            ConcurrentBag<JsonFile> result = new ConcurrentBag<JsonFile>();

            Parallel.ForEach(files, f => { result.Add(JsonReader.Read<JsonFile>(f)); });

            return result.ToList();
        }

        public static List<TTLUnitData> LoadFiles(IEnumerable<JsonFile> files)
        {
            ConcurrentBag<TTLUnitData> result = new ConcurrentBag<TTLUnitData>();
            
            Parallel.ForEach(files, f =>
            {
                try
                {
                    if (checkFile(f))
                        result.Add(new TTLUnitData(f));
                }
                catch (Exception ex) {
                    Log.Warn($"Json file Serial:{f.DUT.SerialNumber} failed to be transformed. Exception: {ex.Message}");
                }
            });
            return result.ToList();
        }

        public static List<string> GetFiles(OpenFileDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileNames.ToList();
            else
                return null;
        }

        private static bool checkFile(JsonFile file)
        {
            var temp = new Measurements(file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements);
            var press = new Measurements(file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements);
            var heater = file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault();

            if (heater == null || heater.Measurements.Count != 2)
                return false;

            if ((temp.MaxTime() + press.MaxTime()) / 2 > 25)
                return false;

            return true;
        }
    }
}
