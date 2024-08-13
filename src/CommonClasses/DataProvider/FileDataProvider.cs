using Newtonsoft.Json.Linq;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Processing;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opal.src.CommonClasses.DataProvider
{
    public class FileDataProvider : IDataProvider
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Config _config = Config.Instance;
        private List<JObject> _files = new List<JObject>();

        public string GetLineCode()
        {
            return CommonFileContentManager.GetLineCode(_files);
        }

        public string GetProductCode()
        {
            return CommonFileContentManager.GetProductCode(_files);
        }

        public async void ProvideData(IScreen screen)
        {
            _files = await ReadJsonFilesAsync();
            string lineCode = GetLineCode();
            string productCode = GetProductCode();

            _config.ProductID = productCode;

            _files = CommonFileContentManager.FilterByLine(_files, lineCode);
            _files = CommonFileContentManager.FilterByProduct(_files, productCode);

            screen.Update(_files);
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

        private async Task<List<JObject>> ReadJsonFilesAsync()
        {
            var paths = GetFilepathsFromDialog();
            return await CommonFileManager.ParseJsonFilesAsync(paths);
        }
    }
}
