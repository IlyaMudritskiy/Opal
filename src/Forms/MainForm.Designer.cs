namespace Opal.Forms
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
            this.FileMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.processedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PDFReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataAndReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DataViewer_MenuStripBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.JsonFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Status_lbl = new System.Windows.Forms.Label();
            this.MainFormPanel = new System.Windows.Forms.Panel();
            this.takeAScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuButton,
            this.StartButton,
            this.SettingsMenuButton,
            this.DataViewer_MenuStripBtn,
            this.takeAScreenshotToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.ShowItemToolTips = true;
            this.MainMenuStrip.Size = new System.Drawing.Size(1584, 28);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "MainMenuStrip";
            // 
            // FileMenuButton
            // 
            this.FileMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileSaveMenuButton});
            this.FileMenuButton.Enabled = false;
            this.FileMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("FileMenuButton.Image")));
            this.FileMenuButton.Name = "FileMenuButton";
            this.FileMenuButton.Size = new System.Drawing.Size(60, 24);
            this.FileMenuButton.Text = "File";
            // 
            // FileSaveMenuButton
            // 
            this.FileSaveMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processedDataToolStripMenuItem,
            this.PDFReportToolStripMenuItem,
            this.dataAndReportToolStripMenuItem});
            this.FileSaveMenuButton.Name = "FileSaveMenuButton";
            this.FileSaveMenuButton.Size = new System.Drawing.Size(109, 24);
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
            // StartButton
            // 
            this.StartButton.Image = global::Opal.Properties.Resources.start_green;
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(68, 24);
            this.StartButton.Text = "Start";
            this.StartButton.Click += new System.EventHandler(this.SelectFilesMenuButton_Click);
            // 
            // SettingsMenuButton
            // 
            this.SettingsMenuButton.Image = global::Opal.Properties.Resources.settings_blue;
            this.SettingsMenuButton.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.SettingsMenuButton.Name = "SettingsMenuButton";
            this.SettingsMenuButton.Size = new System.Drawing.Size(90, 24);
            this.SettingsMenuButton.Text = "Settings";
            this.SettingsMenuButton.Click += new System.EventHandler(this.SettingsMenuButton_Click);
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
            // JsonFileDialog
            // 
            this.JsonFileDialog.FileName = "JsonFileDialog";
            this.JsonFileDialog.Multiselect = true;
            // 
            // Status_lbl
            // 
            this.Status_lbl.AutoSize = true;
            this.Status_lbl.Location = new System.Drawing.Point(300, 9);
            this.Status_lbl.Name = "Status_lbl";
            this.Status_lbl.Size = new System.Drawing.Size(0, 13);
            this.Status_lbl.TabIndex = 2;
            // 
            // MainFormPanel
            // 
            this.MainFormPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainFormPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MainFormPanel.BackgroundImage = global::Opal.Properties.Resources.opal_background1;
            this.MainFormPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MainFormPanel.Location = new System.Drawing.Point(0, 27);
            this.MainFormPanel.Name = "MainFormPanel";
            this.MainFormPanel.Size = new System.Drawing.Size(1584, 835);
            this.MainFormPanel.TabIndex = 1;
            // 
            // takeAScreenshotToolStripMenuItem
            // 
            this.takeAScreenshotToolStripMenuItem.Name = "takeAScreenshotToolStripMenuItem";
            this.takeAScreenshotToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.takeAScreenshotToolStripMenuItem.Text = "Take a Screenshot";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.Status_lbl);
            this.Controls.Add(this.MainFormPanel);
            this.Controls.Add(this.MainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1600, 900);
            this.Name = "MainForm";
            this.Text = "Opal";
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal new System.Windows.Forms.MenuStrip MainMenuStrip;
        internal System.Windows.Forms.Panel MainFormPanel;
        internal System.Windows.Forms.OpenFileDialog JsonFileDialog;
        internal System.Windows.Forms.ToolStripMenuItem StartButton;
        internal System.Windows.Forms.ToolStripMenuItem DataViewer_MenuStripBtn;
        internal System.Windows.Forms.ToolStripMenuItem FileMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem FileSaveMenuButton;
        internal System.Windows.Forms.ToolStripMenuItem processedDataToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem PDFReportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem dataAndReportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SettingsMenuButton;
        private System.Windows.Forms.Label Status_lbl;
        private System.Windows.Forms.ToolStripMenuItem takeAScreenshotToolStripMenuItem;
    }
}

