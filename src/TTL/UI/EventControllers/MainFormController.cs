using Opal.Forms;
using Opal.src.CommonClasses.DataProvider;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System.Threading.Tasks;

namespace Opal.src.TTL.UI.EventControllers
{
    public class MainFormController
    {
        public FileController File { get; private set; }
        public SettingsController Settings { get; private set; }
        public MenuStripButtonsController Menu { get; private set; }
        public DataViewerController DataViewer { get; private set; }

        private MainForm _mainForm;
        private IScreen _screen;
        private IDataProvider _provider;


        public MainFormController(MainForm mainForm)
        {
            this._mainForm = mainForm;
            Initialize();
        }

        private void Initialize()
        {
            File = new FileController(_mainForm);
            Settings = new SettingsController();
            //Menu = new MenuStripButtonsController(_mainForm);
            DataViewer = new DataViewerController(_mainForm);
        }
    }
}
