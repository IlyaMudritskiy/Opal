using ProcessDashboard.src.GUI.ProcessScreens;
using ScottPlot;
using System;
using System.Windows.Forms;

namespace ProcessDashboard.src.Models
{
    public class EmbossingScreenModel
    {
        public TabControl Tabs { get; set; }
        public Overview Overview { get; set; }
        public Details Details { get; set; }

        private static readonly Lazy<EmbossingScreenModel> _instance = new Lazy<EmbossingScreenModel>(() => new EmbossingScreenModel());
        public static EmbossingScreenModel Instance { get { return _instance.Value; } }
    }

    public class Overview : EmbossingMixIn
    {
        public FormsPlot TemperaturePlot { get; set; }
        public FormsPlot PressurePlot { get; set; }
        public FormsPlot FeaturesPlot { get; set; }
    }

    public class Details : EmbossingMixIn
    {
        public FormsPlot DS11 { get; set; }
        public FormsPlot DS12 { get; set; }
        public FormsPlot DS21 { get; set; }
        public FormsPlot DS22 { get; set; }
        public FormsPlot DSMean { get; set; }
    }

    public class EmbossingMixIn
    {
        public TabPage TabPage { get; set; }
        public Label Header { get; set; }
        public TableLayoutPanel TemperatureFeatures { get; set; }
        public TableLayoutPanel PressureFeatures { get; set; }
    }
}
