using Newtonsoft.Json.Linq;
using ProcessDashboard.src.Controller.FileProcessors;
using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.AppConfiguration;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ProcessDashboard.src.Controller.App
{
    public class App
    {
        // Singleton
        private static readonly Lazy<App> lazy = new Lazy<App>(() => new App());
        public static App Instance => lazy.Value;

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private IScreen screen;

        public App() 
        {
            setCultureSettings();
        }

        /// <summary>
        /// Generalised method to start the application.
        /// </summary>
        /// <param name="dialog">File dialog that will open files.</param>
        /// <param name="panel">Container in MainForm that holds the TabControl with all tabs related to the line.</param>
        public void Run(ref OpenFileDialog dialog, ref Panel panel)
        {
            // Get paths of selected files
            List<string> filepaths = CommonFileManager.GetFilePaths(ref dialog);
            // Read selected files into JObject (JObject is not tied to a specific screen)
            List<JObject> files = CommonFileManager.GetFiles(filepaths);

            Log.Trace($"Selected [{filepaths.Count}] files.");
            Log.Trace($"Opened [{files.Count}] files.");

            string lineCode = CommonFileManager.GetLineCode(files);
            string productCode = CommonFileManager.GetProductCode(files);

            Log.Trace($"Line code [{lineCode}].");
            Log.Trace($"Product code [{productCode}].");

            files = CommonFileManager.FilterByLine(files, lineCode);
            files = CommonFileManager.FilterByProduct(files, productCode);

            Log.Trace($"After filtering: Left [{files.Count}], Dropped [{filepaths.Count - files.Count}].");
                            
            if (screen == null)
            {
                screen = ScreenCreator.GetIScreen(lineCode);
                screen.Create(ref panel, ref files);
                screen.LoadData(ref files);
            }
            else
            {
                screen.Update(ref files);
            }
        }

        private void setCultureSettings()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
