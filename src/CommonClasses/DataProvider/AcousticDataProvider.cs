using Newtonsoft.Json.Linq;
using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Containers;
using Opal.src.CommonClasses.Processing;
using Opal.src.CommonClasses.SreenProvider;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System;
using System.Collections.Generic;

namespace Opal.src.CommonClasses.DataProvider
{
    internal class AcousticDataProvider : IDataProvider
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Config _config = Config.Instance;
        private static MainForm _form;
        private IScreen _screen;

        public AcousticDataProvider(MainForm form)
        {
            _form = form;
        }

        public void Start()
        {
            var files = ReadJsonFiles();

            if (files.Count == 0)
                return;

            _config.LineID = CommonFileContentManager.GetTypeName(files);
            _config.ProductID = CommonFileContentManager.GetAcousticProductCode(files);

            if (_screen == null)
            {
                _screen = ScreenFactory.Create("acoustic");
                _screen.Show(_form.MainFormPanel);
            }

            _screen.Update(files, _form);
        }

        private List<JObject> ReadJsonFiles()
        {
            var paths = CommonFileManager.GetFilesFromDialog();
            return CommonFileManager.ParseJsonFiles(paths);
        }

        public Func<Dictionary<string, TableDataContainer>> GetDVCallback()
        {
            return _screen.GetDVCallback();
        }
    }
}
