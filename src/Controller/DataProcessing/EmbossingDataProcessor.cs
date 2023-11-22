using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ProcessDashboard.src.Controller.FileProcessing;

namespace ProcessDashboard.src.Data
{
    public static class EmbossingDataProcessor
    {
        public static List<TransducerData> OpenFiles(CommonDialog dialog)
        {
            List<string> files = new List<string>();
            List <TransducerData> result = new List<TransducerData>();

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
                result = loadFiles(files);
            }
            return result;
        }

        private static List<TransducerData> loadFiles(List<string> files)
        {
            List<TransducerData> result = new List<TransducerData>();

            List<string> stepnames = new List<string> { 
                "ps01_temperature_actual", 
                "ps01_high_pressure_actual",
                "ps01_hold_pressure_actual",
                "ps01_heater_on"
            };

            foreach (string file in files)
            {
                try
                {
                    result.Add(new TransducerData(JsonReader.Read<JsonFile>(file)));
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
