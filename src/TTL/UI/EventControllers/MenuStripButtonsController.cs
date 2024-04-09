﻿using ProcessDashboard.Forms;
using ProcessDashboard.Model.AppConfiguration;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using System;

namespace ProcessDashboard.src.TTL.UI.EventControllers
{
    public class MenuStripButtonsController
    {
        private Config config = Config.Instance;

        private MainForm mainForm;

        public MenuStripButtonsController(MainForm mainForm)
        {
            this.mainForm = mainForm;
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            mainForm.DataViewer_MenuStripBtn.Click += new EventHandler(DataViewer_MenuStripBtn_Click);
        }

        private void DataViewer_MenuStripBtn_Click(object sender, EventArgs e)
        {
            var DV = new DataViewerController(mainForm);
            var data = TTLData.GetInstance();
            if (data != null)
            {
                DV.AddData(data);
                DV.Show();
            }
        }
    }
}