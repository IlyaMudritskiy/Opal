using System;
using System.Drawing;
using System.Windows.Forms;
using Opal.src.App;
using Opal.src.TTL.UI.EventControllers;
using Opal.src.Utils;

namespace Opal.Forms
{
    public partial class MainForm : Form
    {
        private readonly MainFormController UIController;
        private readonly App App;

        public MainForm()
        {
            InitializeComponent();
            UIController = new MainFormController(this);
            App = App.Instance;
        }

        public void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            App.Run(MainFormPanel, this);
        }

        private void SettingsMenuButton_Click(object sender, EventArgs e)
        {
            UIController.Settings.OpenSettings();
        }

        public void SettingsEnabled(bool enabled)
        {
            SettingsMenuButton.Enabled = enabled;
        }

        public void RunEnabled(bool enabled)
        {
            StartButton.Enabled = enabled;
        }

        public void SetMessage(string message, Color color)
        {
            Status_tstxb.Text = message;
            Status_tstxb.ForeColor = color;
        }

        public void ClearMessage()
        {
            Status_tstxb.Text = "";
            Status_tstxb.ForeColor = Colors.Black;
        }

        public void ClearScreen()
        {
            MainFormPanel.Controls.Clear();
        }
    }
}
