using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.CommonClasses;
using ProcessDashboard.src.CommonClasses.Processing;
using ProcessDashboard.src.TTL.Screen;

namespace ProcessDashboard.src.App
{
    public class App
    {
        // Singleton
        private static readonly Lazy<App> Lazy = new Lazy<App>(() => new App());
        public static App Instance => Lazy.Value;

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private IScreen screen;

        private Config config = Config.Instance;

        public App()
        {
            AppSettings();
        }

        /// <summary>
        /// Generalised method to start the application.
        /// </summary>
        /// <param name="dialog">File dialog that will open files.</param>
        /// <param name="panel">Container in MainForm that holds the TabControl with all tabs related to the line.</param>
        public void Run(ref OpenFileDialog dialog, ref Panel panel)
        {
            // Get paths of selected files
            List<string> filepaths = CommonFileManager.GetFilesFromDialog();
            // Read selected files into JObject (JObject is not tied to a specific screen)
            if (filepaths == null || filepaths.Count == 0) return;

            config.ProcessFilePaths = filepaths;

            List<JObject> files = CommonFileManager.ParseJsonFiles(filepaths);

            Log.Trace($"Selected [{filepaths.Count}] files.");
            Log.Trace($"Opened [{files.Count}] files.");

            string lineCode = CommonFileContentManager.GetLineCode(files);
            string productCode = CommonFileContentManager.GetProductCode(files);
            config.ProductID = productCode;

            Log.Trace($"Line code [{lineCode}].");
            Log.Trace($"Product code [{productCode}].");

            files = CommonFileContentManager.FilterByLine(files, lineCode);
            files = CommonFileContentManager.FilterByProduct(files, productCode);

            Log.Trace($"After filtering: Left [{files.Count}], Dropped [{filepaths.Count - files.Count}].");

            if (screen == null)
            {
                screen = ScreenCreator.GetIScreen(lineCode);
                screen.Create(ref panel);
            }
            screen.LoadData(files);
        }

        private void AppSettings()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            SetGlobalDateTimeFormat();
        }

        static void SetGlobalDateTimeFormat()
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            //Type dateTimeInfoType = typeof(DateTimeFormatInfo);
            DateTimeFormatInfo dtfi = cultureInfo.DateTimeFormat;

            // Use reflection to modify the read-only property
            typeof(DateTimeFormatInfo).GetField("generalLongTimePattern", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                                     .SetValue(dtfi, "yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}
