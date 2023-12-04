using ProcessDashboard.src.Controller.TTLine;
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
        private static readonly Lazy<App> lazy =
        new Lazy<App>(() => new App());
        public static App Instance => lazy.Value;
        public App() 
        {
            setCultureSettings();
        }

        // Props
        private IScreen screen;

        public void Run(ref OpenFileDialog dialog, ref Panel panel)
        {
            // Get the user selected files
            List<JsonFile> files = TTLineDataProcessor.OpenFiles(dialog);

            if (files == null || files.Count == 0) return;

            if (screen == null)
            {
                screen = ScreenCreator.GetIScreen(ref files);
                screen.Create(ref panel);
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
