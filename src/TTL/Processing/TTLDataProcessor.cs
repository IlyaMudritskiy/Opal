using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Opal.Model.Data.Acoustic;
using Opal.src.TTL.Containers.FileContent;
using Opal.src.TTL.Containers.ScreenData;

namespace Opal.src.TTL.Processing
{
    public static class TTLDataProcessor
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public async static Task<List<TTLUnit>> LoadFiles(List<JObject> files)
        {
            if (files == null || files.Count == 0) return null;

            List<ProcessFile> processFilesTask = Task.Run(() => ProcessDataProcessor.GetProcessFiles(ref files)).Result;
            List<AcousticFile> acousticFilesTask = Task.Run(() => AcousticDataProcessor.GetAcousticFiles(ref files)).Result;

            //await Task.WhenAll(processFilesTask, acousticFilesTask);

            //List<ProcessFile> processFiles = processFilesTask;
            //List<AcousticFile> acousticFiles = acousticFilesTask;

            return JoinFiles(processFilesTask, acousticFilesTask);
        }

        private static List<TTLUnit> JoinFiles(IEnumerable<ProcessFile> processFiles, IEnumerable<AcousticFile> acousticFiles)
        {
            ConcurrentBag<TTLUnit> result = new ConcurrentBag<TTLUnit>();

            Parallel.ForEach(processFiles, file =>
            {
                string serial = file.DUT.SerialNumber;
                if (acousticFiles != null)
                {
                    AcousticFile acousticFile = acousticFiles.Where(s => s.DUT.Serial == serial).FirstOrDefault();
                    result.Add(new TTLUnit(file, acousticFile));
                }
                result.Add(new TTLUnit(file, null));
            });

            return result.ToList();
        }
    }
}
