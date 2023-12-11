using ProcessDashboard.src.Controller.App;
using ProcessDashboard.src.Model.AppConfiguration;
using System;
using System.Windows.Forms;

namespace ProcessDashboard
{
    public partial class MainForm : Form
    {
        private Config config = Config.Instance;
        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            App app = App.Instance;
            app.Run(ref JsonFileDialog, ref MainFormPanel);
        }

        private void OnOffAcousticPlotsMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.Enabled = !config.Acoustic.Enabled;
            OnOffAcousticPlotsMenuButton.Checked = config.Acoustic.Enabled;
            config.Save();
        }

        private void FileSelectionTypeMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.ManualSelection = !config.Acoustic.ManualSelection;
            FileSelectionTypeMenuButton.Checked = config.Acoustic.ManualSelection;
            config.Save();
        }
    }
}
