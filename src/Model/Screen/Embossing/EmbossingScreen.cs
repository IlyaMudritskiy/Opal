using ProcessDashboard.src.Model.Screen.Embossing;
using ScottPlot;
using System;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    public class EmbossingScreen
    {
        public TabControl Tabs { get; set; }
        public EmbossingTab Temperature { get; set; }
        public EmbossingTab Pressure { get; set; }

        public EmbossingScreen(ref Panel panel)
        {
            Create(ref panel);
        }

        private void Create(ref Panel panel)
        {
            Tabs = new TabControl() { Dock = DockStyle.Fill };
            Temperature = new EmbossingTab("Temperature Details");
            Pressure = new EmbossingTab("Pressure Details");
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

        public void LoadData<T>(T data)
        {
            throw new NotImplementedException();
        }

        public void Hide(ref Panel panel)
        {
            
        }
    }
}
