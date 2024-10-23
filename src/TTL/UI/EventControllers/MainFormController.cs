using Opal.Forms;

namespace Opal.src.TTL.UI.EventControllers
{
    public class MainFormController
    {
        public FileController File { get; private set; }
        public SettingsController Settings { get; private set; }
        public MenuStripButtonsController Menu { get; private set; }
        public DataViewerController DataViewer { get; private set; }

        private MainForm _mainForm;

        public MainFormController(MainForm mainForm)
        {
            this._mainForm = mainForm;
            Initialize();
        }

        private void Initialize()
        {
            File = new FileController(_mainForm);
            Settings = new SettingsController();
            Menu = new MenuStripButtonsController(_mainForm);
            DataViewer = new DataViewerController();
        }
    }
}
