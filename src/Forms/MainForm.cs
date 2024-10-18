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
        private ScreenshotHandler _screenshotHandler;

        public MainForm()
        {
            InitializeComponent();
            UIController = new MainFormController(this);
            App = App.Instance;
            _screenshotHandler = new ScreenshotHandler();
        }

        public void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            App.Run(MainFormPanel, this);
        }

        private void SettingsMenuButton_Click(object sender, EventArgs e)
        {
            UIController.Settings.OpenSettings();
        }

        private void DataViewer_MenuStripBtn_Click(object sender, EventArgs e)
        {
            UIController.DataViewer.Show();
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
            Status_lbl.Text = message;
            Status_lbl.ForeColor = color;
        }

        public void ClearMessage()
        {
            Status_lbl.Text = "";
            Status_lbl.ForeColor = Colors.Black;
        }

        public void ClearScreen()
        {
            MainFormPanel.Controls.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _screenshotHandler?.Dispose();
            base.OnFormClosing(e);
        }

        private void makeScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _screenshotHandler.Make();
        }
    }
}
