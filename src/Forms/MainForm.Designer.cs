namespace ProcessDashboard.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.SelectFilesMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DataViewer_MenuStripBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormPanel = new System.Windows.Forms.Panel();
            this.JsonFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingsAcousticMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OnOffAcousticPlotsMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSelectionTypeMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.processedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PDFReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataAndReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuButton,
            this.SelectFilesMenuButton,
            this.SettingsMenuButton,
            this.DataViewer_MenuStripBtn});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.ShowItemToolTips = true;
            this.MainMenuStrip.Size = new System.Drawing.Size(1584, 28);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "MainMenuStrip";
            // 
            // SelectFilesMenuButton
            // 
            this.SelectFilesMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectFilesMenuButton.Image")));
            this.SelectFilesMenuButton.Name = "SelectFilesMenuButton";
            this.SelectFilesMenuButton.Size = new System.Drawing.Size(120, 24);
            this.SelectFilesMenuButton.Text = "Select File(s)";
            this.SelectFilesMenuButton.Click += new System.EventHandler(this.SelectFilesMenuButton_Click);
            // 
            // DataViewer_MenuStripBtn
            // 
            this.DataViewer_MenuStripBtn.AutoToolTip = true;
            this.DataViewer_MenuStripBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DataViewer_MenuStripBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DataViewer_MenuStripBtn.Image = ((System.Drawing.Image)(resources.GetObject("DataViewer_MenuStripBtn.Image")));
            this.DataViewer_MenuStripBtn.Name = "DataViewer_MenuStripBtn";
            this.DataViewer_MenuStripBtn.Size = new System.Drawing.Size(28, 24);
            this.DataViewer_MenuStripBtn.ToolTipText = "Process Data Viewer";
            // 
            // MainFormPanel
            // 
            this.MainFormPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            // SettingsAcousticMenuButton
            // 
            this.SettingsAcousticMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OnOffAcousticPlotsMenuButton,
            this.FileSelectionTypeMenuButton});
            this.SettingsAcousticMenuButton.Name = "SettingsAcousticMenuButton";
            this.SettingsAcousticMenuButton.Size = new System.Drawing.Size(180, 24);
            this.SettingsAcousticMenuButton.Text = "Acoustic";
            // 
            // OnOffAcousticPlotsMenuButton
            // 
            this.OnOffAcousticPlotsMenuButton.Name = "OnOffAcousticPlotsMenuButton";
            this.OnOffAcousticPlotsMenuButton.Size = new System.Drawing.Size(279, 24);
            this.OnOffAcousticPlotsMenuButton.Text = "Show Acoustic Tabs";
            // 
            // FileSelectionTypeMenuButton
            // 
            this.FileSelectionTypeMenuButton.Name = "FileSelectionTypeMenuButton";
            this.FileSelectionTypeMenuButton.Size = new System.Drawing.Size(279, 24);
            this.FileSelectionTypeMenuButton.Text = "Manual Acoustic File Selection";
            // 
            // SettingsMenuButton
            // 
            this.SettingsMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsAcousticMenuButton});
            this.SettingsMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingsMenuButton.Image")));
            this.SettingsMenuButton.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.SettingsMenuButton.Name = "SettingsMenuButton";
            this.SettingsMenuButton.Size = new System.Drawing.Size(90, 24);
            this.SettingsMenuButton.Text = "Settings";
            // 
            // FileSaveMenuButton
            // 
            this.FileSaveMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processedDataToolStripMenuItem,
            this.PDFReportToolStripMenuItem,
            this.dataAndReportToolStripMenuItem});
            this.FileSaveMenuButton.Name = "FileSaveMenuButton";
            this.FileSaveMenuButton.Size = new System.Drawing.Size(180, 24);
            this.FileSaveMenuButton.Text = "Save";
            // 
            // processedDataToolStripMenuItem
            // 
            this.processedDataToolStripMenuItem.Name = "processedDataToolStripMenuItem";
            this.processedDataToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
            this.processedDataToolStripMenuItem.Text = "Processed Data";
            // 
            // PDFReportToolStripMenuItem
            // 
            this.PDFReportToolStripMenuItem.Name = "PDFReportToolStripMenuItem";
            this.PDFReportToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
            this.PDFReportToolStripMenuItem.Text = "PDF Report";
            // 
            // dataAndReportToolStripMenuItem
            // 
            this.dataAndReportToolStripMenuItem.Name = "dataAndReportToolStripMenuItem";
            this.dataAndReportToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
            this.dataAndReportToolStripMenuItem.Text = "Data and Report";
            // 
            // FileMenuButton
            // 
            this.FileMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileSaveMenuButton});
            this.FileMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("FileMenuButton.Image")));
            this.FileMenuButton.Name = "FileMenuButton";
            this.FileMenuButton.Size = new System.Drawing.Size(60, 24);
            this.FileMenuButton.Text = "File";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.MainFormPanel);
            this.Controls.Add(this.MainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1600, 900);
            this.Name = "MainForm";
            this.Text = "Process Dashboard";
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal new System.Windows.Forms.MenuStrip MainMenuStrip;
        internal System.Windows.Forms.Panel MainFormPanel;
        internal System.Windows.Forms.OpenFileDialog JsonFileDialog;
        internal System.Windows.Forms.ToolStripMenuItem SelectFilesMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem DataViewer_MenuStripBtn;
        internal System.Windows.Forms.ToolStripMenuItem FileMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem FileSaveMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem processedDataToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem PDFReportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem dataAndReportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SettingsMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem SettingsAcousticMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem OnOffAcousticPlotsMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem FileSelectionTypeMenuButton;
    }
}

