using System;
using System.Windows.Forms;
using Opal.src.App;
using Opal.src.TTL.UI.EventControllers;

namespace Opal.Forms
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
