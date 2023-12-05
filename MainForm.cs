using ProcessDashboard.src.Controller.App;
using System;
using System.Windows.Forms;

namespace ProcessDashboard
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            App app = App.Instance;
            app.Run(ref JsonFileDialog, ref MainFormPanel);
        }
    }
}
