using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
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

        public static List<TTLUnit> LoadFiles(List<JObject> files)
        {
            if (files  == null || files.Count == 0) return null;

            List<ProcessFile> processFiles = ProcessDataProcessor.GetProcessFiles(ref files);
            List<AcousticFile> acousticFiles = AcousticDataProcessor.GetAcousticFiles(ref processFiles);
            
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
    }
}
