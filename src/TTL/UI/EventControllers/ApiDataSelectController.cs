using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Containers;
using Opal.src.Forms;
using System.IO;
using System.Reflection;

namespace Opal.src.TTL.UI.EventControllers
{
    public class ApiDataSelectController
    {
        private APIDataSelectorForm _dsf;
        private static Config _config = Config.Instance;
        private string queriesPath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Queries";

        private void InitializeForm()
        {
            _dsf = new APIDataSelectorForm();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _dsf.Confirm_btn.Click += (sender, e) =>
            {
                _dsf.SaveState();
                _dsf.Close();
            };
        }

        public void OpenApiDataFilters()
        {
            InitializeForm();
            _config.Filter = new SearchFilter();
            _dsf.ShowDialog();
        }

        private string GetCustomQueryFilePath()
        {
            var path = $"{queriesPath}\\{_dsf.CustomQuery_cmb.Text}";

            if (File.Exists(path))
                return path;

            return null;
        }
    }
}
