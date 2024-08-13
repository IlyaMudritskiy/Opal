using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.DataProvider;
using Opal.src.CommonClasses.Processing;
using Opal.src.CommonClasses.SreenProvider;
using Opal.src.TTL.Screen;
using ProcessDashboard.src.CommonClasses.SreenProvider;

namespace Opal.src.App
{
    public class App
    {
        // Singleton
        private static readonly Lazy<App> Lazy = new Lazy<App>(() => new App());
        public static App Instance => Lazy.Value;

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private IDataProvider _dataProvider;

        private IScreen _screen;

        private Config _config = Config.Instance;

        public App()
        {
            AppSettings();
            _screen = ScreenFactory.Create(_config.LineID);
            _dataProvider = DataProviderFactory.Get(_config.DataProvider);
        }

        /// <summary>
        /// Generalised method to start the application.
        /// </summary>
        /// <param name="dialog">File dialog that will open files.</param>
        /// <param name="panel">Container in MainForm that holds the TabControl with all tabs related to the line.</param>
        public void Run(ref Panel panel)
        {
            _screen.Show(ref panel);
            _dataProvider.ProvideData(_screen);
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
