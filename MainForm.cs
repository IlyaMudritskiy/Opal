using ProcessDashboard.src.Controller;
using ProcessDashboard.src.Controller.Acoustic;
using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Data.Acoustic;
using ProcessDashboard.src.Model.Screen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ProcessDashboard
{
    public partial class MainForm : Form
    {
        private IScreen screen;
        public MainForm()
        {
            InitializeComponent();
            setCultureSettings();
            //var acoustic = JsonReader.Read<AcousticFile>("C:\\Code\\Azure\\ProcessDashboard\\Assets\\Limits\\TestRef\\DUT_2_1513_231127105602112_20231127-110002.json");
            //Console.WriteLine();
        }

        private void SelectFilesMenuButton_Click(object sender, EventArgs e)
        {
            List<JsonFile> selectedFiles = TTLineDataProcessor.OpenFiles(JsonFileDialog);
            if (screen == null)
            {
                screen = ScreenCreator.GetIScreen(ref selectedFiles);
                screen.Create(ref MainFormPanel);
                screen.LoadData(ref selectedFiles);
            }
            else
            {
                screen.Update(ref selectedFiles);
            }

            //var acF = AcousticDataProcessor.GetAcousticFiles(ref selectedFiles, screen.GetTypeID());
        }

        private void setCultureSettings()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
