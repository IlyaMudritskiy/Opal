using Newtonsoft.Json.Linq;
using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Containers;
using Opal.src.CommonClasses.Processing;
using Opal.src.CommonClasses.SreenProvider;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Opal.src.CommonClasses.DataProvider
{
    public class FileDataProvider : IDataProvider
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Config _config = Config.Instance;
        private static MainForm _form;
        private IScreen _screen;

        public FileDataProvider(MainForm form)
        {
            _form = form;
        }

        public void Start()
        {
            var files = ReadJsonFiles();
            _config.LineID = CommonFileContentManager.GetLineCode(files);
            _config.ProductID = CommonFileContentManager.GetProductCode(files);

            if (_config.LineID == null) return;

            files = CommonFileContentManager.FilterByLine(files, _config.LineID);
            files = CommonFileContentManager.FilterByProduct(files, _config.ProductID);

            if (_screen == null) {
                _screen = ScreenFactory.Create(_config.LineID);
                _screen.Show(_form.MainFormPanel);
            }

            _screen.Update(files, _form);
        }

        public Func<Dictionary<string, TableDataContainer>> GetDVCallback()
        {
            return _screen.GetDVCallback();
        }

        private List<string> GetFilepathsFromDialog()
        {
            List<string> result = new List<string>();

            Thread t = new Thread(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    result = dialog.FileNames.ToList();
                }
                else
                {
                    Log.Info("No files were selected in OpenFileDialog");
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return result;
        }

        private List<JObject> ReadJsonFiles()
        {
            var paths = GetFilepathsFromDialog();
            return CommonFileManager.ParseJsonFiles(paths);
        }
    }
}
