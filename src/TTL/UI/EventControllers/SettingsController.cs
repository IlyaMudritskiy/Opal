﻿using System;
using Opal.Forms;
using Opal.Model.AppConfiguration;

namespace Opal.src.TTL.UI.EventControllers
{
    public class SettingsController
    {
        private Config config = Config.Instance;

        private MainForm mainForm;

        public SettingsController(MainForm mainForm)
        {
            this.mainForm = mainForm;
            //this.mainForm.OnOffAcousticPlotsMenuButton.Checked = config.Acoustic.Enabled;
            //this.mainForm.FileSelectionTypeMenuButton.Checked = config.Acoustic.CustomLocationEnabled;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
        }

        public void OnOffAcousticPlotsMenuButton_Click(object sender, EventArgs e)
        {
            config.Acoustic.Enabled = !config.Acoustic.Enabled;
            config.Save();
        }

        public void FileSelectionTypeMenuButton_Click(object sender, EventArgs e)
        {
            //config.Acoustic.ManualSelection = !config.Acoustic.CustomLocationEnabled;
            //config.Save();
            //mainForm.FileSelectionTypeMenuButton.Checked = config.Acoustic.CustomLocationEnabled;
        }
    }
}
