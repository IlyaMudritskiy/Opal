using ProcessDashboard.src.Controller.App;
using ProcessDashboard.src.Controller.UI;
using System;
using System.Windows.Forms;

namespace ProcessDashboard
{
    public partial class MainForm : Form
    {
        private UIController UIController;
        private App App;

        public MainForm()
        {
            UIController = new UIController(this);
            App = App.Instance;
            InitializeComponent();
        }

        public void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            App.Run(ref JsonFileDialog, ref MainFormPanel);
        }
    }
}
