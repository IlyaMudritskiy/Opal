using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.DataProvider;
using Opal.src.CommonClasses.SreenProvider;
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

        private Config _config = Config.Instance;

        private string _providerInfo;

        public App()
        {
            AppSettings();
        }

        /// <summary>
        /// Generalised method to start the application.
        /// </summary>
        /// <param name="dialog">File dialog that will open files.</param>
        /// <param name="panel">Container in MainForm that holds the TabControl with all tabs related to the line.</param>
        public void Run(Panel panel, MainForm mainForm)
        {
            mainForm.ClearMessage();
            SetDataprovider(mainForm);

            _dataProvider.Start();
        }

        /// <summary>
        /// Sets the data provider. If data provider was not changed, it will keep the same object instead of
        /// creating new DataProvider. If type of DataProvider was changed, it will create new provider.
        /// </summary>
        /// <param name="mainForm"></param>
        private void SetDataprovider(MainForm mainForm)
        {
            if (_dataProvider == null)
            {
                _dataProvider = DataProviderFactory.Get(_config.DataProvider.Type, mainForm);
                _providerInfo = _config.DataProvider.Type;
            }

            if (_dataProvider != null && !string.IsNullOrEmpty(_providerInfo))
            {
                if (_providerInfo != _config.DataProvider.Type)
                {
                    mainForm.ClearScreen();
                    _dataProvider = DataProviderFactory.Get(_config.DataProvider.Type, mainForm);
                    _providerInfo = _config.DataProvider.Type;
                }
            }
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
            DateTimeFormatInfo dtfi = cultureInfo.DateTimeFormat;

            typeof(DateTimeFormatInfo).GetField("generalLongTimePattern", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                                     .SetValue(dtfi, "yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}
