using ProcessDashboard.src.Models;
using ScottPlot;
using System;
using System.Windows.Forms;

namespace ProcessDashboard.src.GUI.ProcessScreens
{
    public class EmbossingScreenCreator
    {
        private EmbossingScreenModel _embossingScreen;

        // Lazy Singleton
        private static readonly Lazy<EmbossingScreenCreator> _instance = new Lazy<EmbossingScreenCreator>(() => new EmbossingScreenCreator());
        public static EmbossingScreenCreator Instance { get { return _instance.Value; } }

        public EmbossingScreenModel Get()
        {
            // If screen is not created, create and put controls inside.
            // If screen exists, no need to put controls insode
            if (_embossingScreen == null)
            {
                _embossingScreen = new EmbossingScreenModel();
                createScreen();
            }
            return _embossingScreen;
        }

        private void createScreen()
        {
            _embossingScreen.Overview = createOverview();
            //_embossingScreen.Details = createDetails();

            _embossingScreen.Tabs = new TabControl()
            {
                Dock = DockStyle.Fill,
            };
            _embossingScreen.Tabs.TabPages.Add(_embossingScreen.Overview.TabPage);
        }

        private Overview createOverview()
        {
            // Create main controls that will display information
            TabPage TabPage = new TabPage("Overview");

            Label Header = new Label()
            {
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "Testing testing testing"
            };

            FormsPlot TemperaturePlot = new FormsPlot()
            {
                Dock = DockStyle.Fill
            };

            FormsPlot PressurePlot = new FormsPlot()
            {
                Dock = DockStyle.Fill
            };

            FormsPlot FeaturesPlot = new FormsPlot()
            {
                Dock = DockStyle.Fill
            };

            TableLayoutPanel TemperatureFeatures = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1
            };

            TableLayoutPanel PressureFeatures = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1
            };

            // Create table layout to place controls on the screen correctly
            var Base = new TableLayoutPanel()
            {
                // Rows and Columns
                ColumnCount = 2,
                RowCount = 1,

                // Screen behaviour
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,

                // Styles
                RowStyles = { new RowStyle(SizeType.Percent, 100F) },
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100F),
                    new ColumnStyle(SizeType.Absolute, 400)
                }
            };

            var PlotArea = new TableLayoutPanel()
            {
                // Rows and Columns
                ColumnCount = 1,
                RowCount = 4,

                // Screen behaviour
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,

                // Styles
                RowStyles = {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 33.3F),
                    new RowStyle(SizeType.Percent, 33.3F),
                    new RowStyle(SizeType.Percent, 33.3F)
                },
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) }
            };

            PlotArea.SuspendLayout();
            PlotArea.Controls.Add(Header, 0, 0);
            PlotArea.Controls.Add(TemperaturePlot, 0, 1);
            PlotArea.Controls.Add(PressurePlot, 0, 2);
            PlotArea.Controls.Add(FeaturesPlot, 0, 3);
            PlotArea.ResumeLayout();

            var TableArea = new TableLayoutPanel()
            {
                ColumnCount = 4,
                RowCount = 2,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 25F),
                    new ColumnStyle(SizeType.Percent, 25F),
                    new ColumnStyle(SizeType.Percent, 25F),
                    new ColumnStyle(SizeType.Percent, 25F)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 100 / 2),
                    new RowStyle(SizeType.Percent, 100 / 2),
                }
            };

            Base.SuspendLayout();
            Base.Controls.Add(PlotArea, 0, 0);
            Base.Controls.Add(TableArea, 1, 0);
            Base.ResumeLayout();

            TabPage.Controls.Add(Base);

            return new Overview()
            {
                TabPage = TabPage,
                Header = Header,
                TemperatureFeatures = TemperatureFeatures,
                PressureFeatures = PressureFeatures,
                TemperaturePlot = TemperaturePlot,
                PressurePlot = PressurePlot,
                FeaturesPlot = FeaturesPlot
            };
        }

        private Details createDetails()
        {
            Details details = new Details();

            return details;
        }
    }
}
