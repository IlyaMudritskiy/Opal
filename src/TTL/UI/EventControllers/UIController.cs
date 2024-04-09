using ProcessDashboard.Forms;

namespace ProcessDashboard.src.TTL.UI.EventControllers
{
    public class UIController
    {
        public FileController File { get; private set; }
        public SettingsController Settings { get; private set; }
        public MenuStripButtonsController Menu { get; private set; }
        public DataViewerController DataViewer { get; private set; }

        private MainForm mainForm;

        public UIController(MainForm mainForm)
        {
            this.mainForm = mainForm;
            Initialize();
        }

        private void Initialize()
        {
            File = new FileController(mainForm);
            Settings = new SettingsController(mainForm);
            Menu = new MenuStripButtonsController(mainForm);
            DataViewer = new DataViewerController(mainForm);
        }
    }
}
