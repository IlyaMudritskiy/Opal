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
            this.SelectFilesMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormPanel = new System.Windows.Forms.Panel();
            this.JsonFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.SelectFilesMenuButton});
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
            // SelectFilesMenuButton
            // 
            this.SelectFilesMenuButton.Name = "SelectFilesMenuButton";
            this.SelectFilesMenuButton.Size = new System.Drawing.Size(84, 20);
            this.SelectFilesMenuButton.Text = "Select File(s)";
            this.SelectFilesMenuButton.Click += new System.EventHandler(this.SelectFilesMenuButton_Click);
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
        private System.Windows.Forms.ToolStripMenuItem processedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pDFReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataAndReportToolStripMenuItem;
        private System.Windows.Forms.Panel MainFormPanel;
        private System.Windows.Forms.OpenFileDialog JsonFileDialog;
        private System.Windows.Forms.ToolStripMenuItem SelectFilesMenuButton;
    }
}

