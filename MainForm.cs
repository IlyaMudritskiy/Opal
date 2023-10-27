using ProcessDashboard.src.GUI.ProcessScreens;
using ProcessDashboard.src.Models;
using System.Windows.Forms;

namespace ProcessDashboard
{
    public partial class MainForm : Form
    {
        EmbossingScreenModel embossingScreenModel;
        public MainForm()
        {
            InitializeComponent();
            _createTableLayout();
        }

        private void _createTableLayout()
        {
            var EmbossingCreator = new EmbossingScreenCreator();
            embossingScreenModel = EmbossingCreator.Get();

            MainFormPanel.Controls.Add(embossingScreenModel.Tabs);
        }

        private void settingsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            embossingScreenModel.Overview.PressurePlot.BackColor = System.Drawing.Color.FromArgb(95, 169, 99);
        }
    }
}
