using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.AppConfiguration;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private Config config = Config.Instance;

        public App() 
        {
            setCultureSettings();
        }

        // Props
        private IScreen screen;

        public void Run(ref OpenFileDialog dialog, ref Panel panel)
        {
            // Get the user selected files
            List<string> files = TTLineDataProcessor.GetFiles(dialog);
            List<JsonFile> jsonFiles = TTLineDataProcessor.OpenFiles(files);

            if (files == null || files.Count == 0)
            {
                Log.Trace("No files are selected (dialog was canceled or closed)");
                return;
            }
                            
            if (screen == null)
            {
                screen = ScreenCreator.GetIScreen(ref jsonFiles);
                screen.Create(ref panel, dialog);
                screen.LoadData(ref jsonFiles);
            }
            else
            {
                screen.Update(ref jsonFiles);
            }
        }

        private void setCultureSettings()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
