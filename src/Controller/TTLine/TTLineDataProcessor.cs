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
        public static List<JsonFile> OpenFiles(OpenFileDialog dialog)
        {
            List<string> files = new List<string>();
            ConcurrentBag<JsonFile> result = new ConcurrentBag<JsonFile>();

            if (dialog.ShowDialog() == DialogResult.OK)
                files = dialog.FileNames.ToList();
            else
                return null;

            Parallel.ForEach(files, f =>
            {
                    result.Add(JsonReader.Read<JsonFile>(f));
            });

            return result.ToList();
        }

        public static List<TTLUnitData> LoadFiles(List<JsonFile> files)
        {
            ConcurrentBag<TTLUnitData> result = new ConcurrentBag<TTLUnitData>();
            
            Parallel.ForEach(files, f =>
            {
                try
                {
                    result.Add(new TTLUnitData(f));
                }
                catch (Exception ex) { }
            });
            return result.ToList();
        }
    }
}
