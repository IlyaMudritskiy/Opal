using System;
using System.Windows.Forms;
using ProcessDashboard.src.App;
using ProcessDashboard.src.TTL.UI.EventControllers;

namespace ProcessDashboard.Forms
{
    public partial class MainForm : Form
    {
        private readonly UIController UIController;
        private readonly App App;

        public MainForm()
        {
            InitializeComponent();
            UIController = new UIController(this);
            App = App.Instance;
            
        }

        public void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            App.Run(ref JsonFileDialog, ref MainFormPanel);
        }
    }
}
