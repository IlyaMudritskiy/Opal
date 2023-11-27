using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ProcessDashboard.src.Model.Data.TTLine;

namespace ProcessDashboard.src.Controller.TTLine
{
    public static class TTLineDataProcessor
    {
        public static List<JsonFile> OpenFiles(CommonDialog dialog)
        {
            List<string> files = new List<string>();
            List<JsonFile> result = new List<JsonFile>();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog is OpenFileDialog)
                {
                    OpenFileDialog fileDialog = dialog as OpenFileDialog;
                    files = fileDialog.FileNames.ToList();
                }

                if (dialog is FolderBrowserDialog)
                {
                    FolderBrowserDialog folderDialog = dialog as FolderBrowserDialog;
                    files = Directory.GetFiles(folderDialog.SelectedPath).ToList();
                }

                foreach (string file in files)
                    result.Add(JsonReader.Read<JsonFile>(file));
            }
            return result;
        }

        public static List<TTLUnitData> LoadFiles(List<JsonFile> files)
        {
            List<TTLUnitData> result = new List<TTLUnitData>();

            foreach (JsonFile file in files)
            {
                try
                {
                    result.Add(new TTLUnitData(file));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
    }
}
