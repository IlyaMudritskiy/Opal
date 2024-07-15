using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.TTL.Containers.FileContent;
using ProcessDashboard.src.TTL.Containers.ScreenData;

namespace ProcessDashboard.src.TTL.Processing
{
    public static class TTLDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public async static Task<List<TTLUnit>> LoadFiles(List<JObject> files)
        {
            if (files == null || files.Count == 0) return null;

            Task<List<ProcessFile>> processFilesTask = Task.Run(() => ProcessDataProcessor.GetProcessFiles(ref files));
            Task<List<AcousticFile>> acousticFilesTask = Task.Run(() => AcousticDataProcessor.GetAcousticFiles(ref files));

            await Task.WhenAll(processFilesTask, acousticFilesTask);

            List<ProcessFile> processFiles = processFilesTask.Result;
            List<AcousticFile> acousticFiles = acousticFilesTask.Result;

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

            return result.ToList();
        }
    }
}
