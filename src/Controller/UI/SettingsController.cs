﻿using ProcessDashboard.src.Model.AppConfiguration;
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
            this.mainForm.OnOffAcousticPlotsMenuButton.Checked = config.Acoustic.Enabled;
            this.mainForm.FileSelectionTypeMenuButton.Checked = config.Acoustic.ManualSelection;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            mainForm.FileSelectionTypeMenuButton.Click += new System.EventHandler(FileSelectionTypeMenuButton_Click);
            mainForm.OnOffAcousticPlotsMenuButton.Click += new System.EventHandler(OnOffAcousticPlotsMenuButton_Click);
        }

        public void OnOffAcousticPlotsMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.Enabled = !config.Acoustic.Enabled;
            config.Save();
            mainForm.OnOffAcousticPlotsMenuButton.Checked = config.Acoustic.Enabled;
        }

        public void FileSelectionTypeMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.ManualSelection = !config.Acoustic.ManualSelection;
            config.Save();
            mainForm.FileSelectionTypeMenuButton.Checked = config.Acoustic.ManualSelection;
        }
    }
}
