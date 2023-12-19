using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Forms;
using ProcessDashboard.src.Model.Data.TTLine;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using ProcessDashboard.src.Controller.FileDataProcessors;
using ProcessDashboard.src.Controller.Acoustic;
using ProcessDashboard.src.Controller.FileDataProcessors.TTL;
using ProcessDashboard.src.Model.Data.Acoustic;
using ProcessDashboard.src.Model.Data.TTLine.Process;

namespace ProcessDashboard.src.Controller.TTLine
{
    public static class TTLDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        // Get the JsonFile files
        // Join them into one unit object
        // Return

        public static List<TTLUnit> LoadFiles(ref List<JObject> files)
        {
            if (files  == null || files.Count == 0) return null;

            List<ProcessFile> processFiles = ProcessDataProcessor.GetProcessFiles(ref files);
            List<AcousticFile> acousticFiles = AcousticDataProcessor.OpenFiles(ref processFiles);
            
            return JoinFiles(processFiles, acousticFiles);
        }

        private static List<TTLUnit> JoinFiles(IEnumerable<ProcessFile> processFiles, IEnumerable<AcousticFile> acousticFiles)
        {
            ConcurrentBag<TTLUnit> result = new ConcurrentBag<TTLUnit>();
            ConcurrentBag<ProcessFile> pf = new ConcurrentBag<ProcessFile>(processFiles);
            ConcurrentBag<AcousticFile> af = new ConcurrentBag<AcousticFile>(acousticFiles);

            Parallel.ForEach(processFiles, file =>
            {
                string serial = file.DUT.SerialNumber;
                AcousticFile acousticFile = acousticFiles.Where(s => s.DUT.Serial == serial).FirstOrDefault();

                result.Add(new TTLUnit(file, acousticFile));
            });
            /*
            foreach (var processFile in processFiles)
            {
                string serial = processFile.DUT.SerialNumber;
                AcousticFile acousticFile = acousticFiles.Where(s => s.DUT.Serial == serial).FirstOrDefault();

                result.Add(new TTLUnit(processFile, acousticFile));
            }*/
            return result.ToList();
        }









        public static List<TTLUnitDataOld> LoadFiles(IEnumerable<ProcessFile> files)
        {
            ConcurrentBag<TTLUnitDataOld> result = new ConcurrentBag<TTLUnitDataOld>();
            
            Parallel.ForEach(files, f =>
            {
                try
                {
                    if (checkFile(f))
                        result.Add(new TTLUnitDataOld(f));
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

        private static bool checkFile(ProcessFile file)
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
