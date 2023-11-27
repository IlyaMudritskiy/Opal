namespace ProcessDashboard
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProcessedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pDFReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataAndReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectDataFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProcessStepsCmb = new System.Windows.Forms.ToolStripComboBox();
            this.stepsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pS01EmbossingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pS99EnjoyingMusicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.MainFormPanel = new System.Windows.Forms.Panel();
            this.JsonFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.JsonFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.stepsToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(1584, 24);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "MainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProcessedDataToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveProcessedDataToolStripMenuItem
            // 
            this.saveProcessedDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processedDataToolStripMenuItem,
            this.pDFReportToolStripMenuItem,
            this.dataAndReportToolStripMenuItem});
            this.saveProcessedDataToolStripMenuItem.Name = "saveProcessedDataToolStripMenuItem";
            this.saveProcessedDataToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.saveProcessedDataToolStripMenuItem.Text = "Save";
            // 
            // processedDataToolStripMenuItem
            // 
            this.processedDataToolStripMenuItem.Name = "processedDataToolStripMenuItem";
            this.processedDataToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.processedDataToolStripMenuItem.Text = "Processed Data";
            // 
            // pDFReportToolStripMenuItem
            // 
            this.pDFReportToolStripMenuItem.Name = "pDFReportToolStripMenuItem";
            this.pDFReportToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.pDFReportToolStripMenuItem.Text = "PDF Report";
            // 
            // dataAndReportToolStripMenuItem
            // 
            this.dataAndReportToolStripMenuItem.Name = "dataAndReportToolStripMenuItem";
            this.dataAndReportToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.dataAndReportToolStripMenuItem.Text = "Data and Report";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDataFilesToolStripMenuItem,
            this.selectProcessToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // selectDataFilesToolStripMenuItem
            // 
            this.selectDataFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jSONToolStripMenuItem,
            this.oPCToolStripMenuItem,
            this.dBToolStripMenuItem});
            this.selectDataFilesToolStripMenuItem.Name = "selectDataFilesToolStripMenuItem";
            this.selectDataFilesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectDataFilesToolStripMenuItem.Text = "Select Data Source";
            // 
            // jSONToolStripMenuItem
            // 
            this.jSONToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFilesToolStripMenuItem,
            this.selectFolderToolStripMenuItem});
            this.jSONToolStripMenuItem.Name = "jSONToolStripMenuItem";
            this.jSONToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.jSONToolStripMenuItem.Text = "JSON";
            // 
            // selectFilesToolStripMenuItem
            // 
            this.selectFilesToolStripMenuItem.Name = "selectFilesToolStripMenuItem";
            this.selectFilesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectFilesToolStripMenuItem.Text = "Select File(s)";
            this.selectFilesToolStripMenuItem.Click += new System.EventHandler(this.SelectJsonFilesToolStripMenuItem_Click);
            // 
            // selectFolderToolStripMenuItem
            // 
            this.selectFolderToolStripMenuItem.Name = "selectFolderToolStripMenuItem";
            this.selectFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectFolderToolStripMenuItem.Text = "Select Folder";
            // 
            // oPCToolStripMenuItem
            // 
            this.oPCToolStripMenuItem.Name = "oPCToolStripMenuItem";
            this.oPCToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.oPCToolStripMenuItem.Text = "OPC";
            // 
            // dBToolStripMenuItem
            // 
            this.dBToolStripMenuItem.Name = "dBToolStripMenuItem";
            this.dBToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dBToolStripMenuItem.Text = "DB";
            // 
            // selectProcessToolStripMenuItem
            // 
            this.selectProcessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProcessStepsCmb});
            this.selectProcessToolStripMenuItem.Name = "selectProcessToolStripMenuItem";
            this.selectProcessToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectProcessToolStripMenuItem.Text = "Select Process";
            // 
            // ProcessStepsCmb
            // 
            this.ProcessStepsCmb.Items.AddRange(new object[] {
            "Embossing (PS01)",
            "Coil (PS02)",
            "Glueing(PS03)"});
            this.ProcessStepsCmb.Name = "ProcessStepsCmb";
            this.ProcessStepsCmb.Size = new System.Drawing.Size(121, 23);
            // 
            // stepsToolStripMenuItem
            // 
            this.stepsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pS01EmbossingToolStripMenuItem,
            this.pS99EnjoyingMusicToolStripMenuItem,
            this.toolStripComboBox1});
            this.stepsToolStripMenuItem.Name = "stepsToolStripMenuItem";
            this.stepsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.stepsToolStripMenuItem.Text = "Steps";
            // 
            // pS01EmbossingToolStripMenuItem
            // 
            this.pS01EmbossingToolStripMenuItem.Name = "pS01EmbossingToolStripMenuItem";
            this.pS01EmbossingToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.pS01EmbossingToolStripMenuItem.Text = "PS01 Embossing";
            // 
            // pS99EnjoyingMusicToolStripMenuItem
            // 
            this.pS99EnjoyingMusicToolStripMenuItem.Name = "pS99EnjoyingMusicToolStripMenuItem";
            this.pS99EnjoyingMusicToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.pS99EnjoyingMusicToolStripMenuItem.Text = "PS99 Enjoying music";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // MainFormPanel
            // 
            this.MainFormPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainFormPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainFormPanel.Location = new System.Drawing.Point(0, 27);
            this.MainFormPanel.Name = "MainFormPanel";
            this.MainFormPanel.Size = new System.Drawing.Size(1584, 835);
            this.MainFormPanel.TabIndex = 1;
            // 
            // JsonFileDialog
            // 
            this.JsonFileDialog.FileName = "JsonFileDialog";
            this.JsonFileDialog.Multiselect = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.MainFormPanel);
            this.Controls.Add(this.MainMenuStrip);
            this.MinimumSize = new System.Drawing.Size(1600, 900);
            this.Name = "MainForm";
            this.Text = "Process Dashboard";
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProcessedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pDFReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataAndReportToolStripMenuItem;
        private System.Windows.Forms.Panel MainFormPanel;
        private System.Windows.Forms.ToolStripComboBox ProcessStepsCmb;
        private System.Windows.Forms.ToolStripMenuItem selectDataFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dBToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog JsonFileDialog;
        private System.Windows.Forms.ToolStripMenuItem stepsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pS01EmbossingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pS99EnjoyingMusicToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.FolderBrowserDialog JsonFolderDialog;
    }
}

