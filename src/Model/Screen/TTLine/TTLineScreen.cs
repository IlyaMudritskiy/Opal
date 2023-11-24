using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Data.TTLine;
using ProcessDashboard.src.Utils.Design;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen.TTLine
{
    public class TTLineScreen : IScreen
    {
        public TabControl Tabs { get; set; }
        public TTLineTab Temperature { get; set; }
        public TTLineTab Pressure { get; set; }

        private string lineID;
        private string typeID;

        public TTLineScreen()
        {
            //Create(ref panel);
        }

        public void Create(ref Panel panel)
        {
            Tabs = new TabControl() { Dock = DockStyle.Fill };
            Temperature = new TTLineTab("Temperature Details");
            Pressure = new TTLineTab("Pressure Details");
            Tabs.TabPages.Add(Temperature.Tab);
            Tabs.TabPages.Add(Pressure.Tab);
            panel.SuspendLayout();
            panel.Controls.Add(Tabs);
            panel.ResumeLayout();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void LoadData(ref List<JsonFile> files)
        {
            List<TTLUnitData> data = TTLineDataProcessor.LoadFiles(files);
            var screenData = new ScreenData(data);

            set(screenData.DS11, Temperature.DS11, Colors.DS11C, "DS 1-1");
            set(screenData.DS12, Temperature.DS12, Colors.DS12C, "DS 1-2");
            set(screenData.DS21, Temperature.DS21, Colors.DS21C, "DS 2-1");
            set(screenData.DS22, Temperature.DS22, Colors.DS22C, "DS 2-2");

            Temperature.Header.Text += $" |  {lineID} - {typeID}";
        }

        private void set(DSXXData ds, TableView tv, Color color, string label)
        {
            if (ds != null && ds.Temperature.Count != 0)
            {
                tv.AddData(ds.TempFeaturesMean, color);

                foreach (var item in ds.Temperature)
                    Temperature.AddScatter(item.TimeOffset, item.Values, color, label);

                foreach (var item in ds.Pressure)
                    Pressure.AddScatter(item.TimeOffset, item.Values, color, label);

                lineID = ds.LineID;
                typeID = ds.TypeID.ToString();
            }
        }
    }
}
