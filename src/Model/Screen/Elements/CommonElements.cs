using ProcessDashboard.src.Utils.Design;
using ScottPlot;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    public static class CommonElements
    {
        public static Label Header(string Text = "Header")
        {
            return new Label()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = Text,
                BackColor = Colors.Black,
                ForeColor = Colors.White,
                Font = Fonts.Sennheiser.ML,
            };
        }

        public static FormsPlot Plot(string Title = "", string XLabel = "", string YLabel = "")
        {
            FormsPlot plot = new FormsPlot()
            {
                Dock = DockStyle.Fill
            };

            //Legend legend = plot.Plot.Legend();
            //legend.Location = Alignment.UpperRight;

            if (!string.IsNullOrEmpty(Title))
                plot.Plot.Title(Title);

            if (!string.IsNullOrEmpty(XLabel))
                plot.Plot.XLabel(XLabel);

            if (!string.IsNullOrEmpty(YLabel))
                plot.Plot.YLabel(YLabel);

            plot.Plot.AxisAuto();
            plot.Refresh();

            return plot;
        }

        public static void ToLog(FormsPlot plot)
        {
            //plot.Plot.XAxis.TickLabelFormat(logTickLabels);
            plot.Plot.XAxis.MinorLogScale(true);
            plot.Plot.XAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
            plot.Plot.XAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            plot.Plot.YAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
            //plot.Plot.SetAxisLimits(0, 150, 0, Math.Log10(10_000_000));
            plot.Plot.AxisAuto();
            plot.Refresh();
        }

        public static DataGridView DataGridView()
        {
            return new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                Font = Fonts.Sennheiser.M
            };
        }

        public static TableLayoutPanel Base()
        {
            return new TableLayoutPanel()
            {
                // Col 1 for graphs, Col 2 for tables
                ColumnCount = 1,
                RowCount = 3,

                // Screen behaviour
                Dock = DockStyle.Fill,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,

                // Styles
                RowStyles = {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 40F),
                    new RowStyle(SizeType.Percent, 60F)
                },
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) }
            };
        }

        public static GroupBox GroupBox(string title)
        {
            return new GroupBox()
            {
                Text = title,
                Dock = DockStyle.Fill,
                Font = Fonts.Sennheiser.M
            };
        }
    }
}
