using ProcessDashboard.src.Model.AppConfiguration;
using System;

namespace ProcessDashboard.src.Controller.UI
{
    public class SettingsController
    {
        private Config config = Config.Instance;

        private MainForm mainForm;

        public SettingsController(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        internal void OnOffAcousticPlotsMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.Enabled = !config.Acoustic.Enabled;
            config.Save();
            mainForm.OnOffAcousticPlotsMenuButton.Checked = config.Acoustic.Enabled;
        }

        internal void FileSelectionTypeMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.ManualSelection = !config.Acoustic.ManualSelection;
            config.Save();
            mainForm.FileSelectionTypeMenuButton.Checked = config.Acoustic.ManualSelection;
        }
    }
}
