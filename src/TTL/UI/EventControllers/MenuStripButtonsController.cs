using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.src.TTL.Containers.ScreenData;
using System;

namespace Opal.src.TTL.UI.EventControllers
{
    public class MenuStripButtonsController
    {
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
            var DV = new DataViewerController(); // We create new DV every time but when close we hide it
            /*
            var data = TTLData.Instance;
            if (data != null)
            {
                DV.AddData(data.GetDataViewerFormat());
                DV.Show();
            }
            */

            DV.AddData();
            DV.Show();
        }
    }
}