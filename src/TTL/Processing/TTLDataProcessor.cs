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
            if (files == null || !files.Any())
            {
                return new List<TTLUnit>();
            }

            var processFilesTask = ProcessDataProcessor.GetProcessFiles(files);
            var acousticFilesTask = AcousticDataProcessor.GetAcousticFiles(files);

            await Task.WhenAll(processFilesTask, acousticFilesTask);

            var processFiles = processFilesTask.Result;
            var acousticFiles = acousticFilesTask.Result;

            return JoinFiles(processFiles, acousticFiles);
        }

        public async static Task<List<AcousticFile>> LoadAcousticFiles(List<JObject> files)
        {
            if (files == null || !files.Any())
            {
                return new List<AcousticFile>();
            }

            return await AcousticDataProcessor.GetAcousticFiles(files);
        }

        public async static Task<TTLUnit> LoadFile(JObject file)
        {
            if (file == null)
            {
                return null;
            }

            var processFile = await ProcessDataProcessor.GetProcessFile(file);

            if (processFile == null) return null;

            return new TTLUnit(processFile);
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
