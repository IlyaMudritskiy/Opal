﻿using ProcessDashboard.src.Controller;
using ProcessDashboard.src.Controller.TTLine;
using ProcessDashboard.src.Model.Data;
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
        }

        private void setCultureSettings()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}