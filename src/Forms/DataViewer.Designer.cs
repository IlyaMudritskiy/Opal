namespace ProcessDashboard.src.Forms
{
    partial class DataViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataViewer));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DataViewerMenuStrip = new System.Windows.Forms.MenuStrip();
            this.SelectObjectDropDown = new System.Windows.Forms.ToolStripComboBox();
            this.DataViewerInputField = new System.Windows.Forms.ToolStripTextBox();
            this.DataViewerSearchButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DataViewerPanel = new System.Windows.Forms.Panel();
            this.DataViewerMainTable = new System.Windows.Forms.DataGridView();
            this.DataViewerMenuStrip.SuspendLayout();
            this.DataViewerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataViewerMainTable)).BeginInit();
            this.SuspendLayout();
            // 
            // DataViewerMenuStrip
            // 
            this.DataViewerMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataViewerMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectObjectDropDown,
            this.DataViewerInputField,
            this.DataViewerSearchButton});
            this.DataViewerMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.DataViewerMenuStrip.Name = "DataViewerMenuStrip";
            this.DataViewerMenuStrip.Size = new System.Drawing.Size(1400, 27);
            this.DataViewerMenuStrip.TabIndex = 0;
            this.DataViewerMenuStrip.Text = "menuStrip1";
            // 
            // SelectObjectDropDown
            // 
            this.SelectObjectDropDown.Name = "SelectObjectDropDown";
            this.SelectObjectDropDown.Size = new System.Drawing.Size(123, 23);
            this.SelectObjectDropDown.Text = "Select object(s)";
            // 
            // DataViewerInputField
            // 
            this.DataViewerInputField.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DataViewerInputField.Name = "DataViewerInputField";
            this.DataViewerInputField.Size = new System.Drawing.Size(100, 23);
            // 
            // DataViewerSearchButton
            // 
            this.DataViewerSearchButton.AutoToolTip = true;
            this.DataViewerSearchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DataViewerSearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DataViewerSearchButton.Image = ((System.Drawing.Image)(resources.GetObject("DataViewerSearchButton.Image")));
            this.DataViewerSearchButton.Name = "DataViewerSearchButton";
            this.DataViewerSearchButton.Size = new System.Drawing.Size(28, 23);
            this.DataViewerSearchButton.ToolTipText = "Search";
            // 
            // DataViewerPanel
            // 
            this.DataViewerPanel.Controls.Add(this.DataViewerMainTable);
            this.DataViewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataViewerPanel.Location = new System.Drawing.Point(0, 27);
            this.DataViewerPanel.Name = "DataViewerPanel";
            this.DataViewerPanel.Size = new System.Drawing.Size(1400, 723);
            this.DataViewerPanel.TabIndex = 1;
            // 
            // DataViewerMainTable
            // 
            this.DataViewerMainTable.AllowUserToAddRows = false;
            this.DataViewerMainTable.AllowUserToDeleteRows = false;
            this.DataViewerMainTable.AllowUserToOrderColumns = true;
            this.DataViewerMainTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DataViewerMainTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataViewerMainTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataViewerMainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataViewerMainTable.Location = new System.Drawing.Point(0, 0);
            this.DataViewerMainTable.Name = "DataViewerMainTable";
            this.DataViewerMainTable.ReadOnly = true;
            this.DataViewerMainTable.RowHeadersVisible = false;
            this.DataViewerMainTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.DataViewerMainTable.Size = new System.Drawing.Size(1400, 723);
            this.DataViewerMainTable.TabIndex = 0;
            // 
            // DataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 750);
            this.Controls.Add(this.DataViewerPanel);
            this.Controls.Add(this.DataViewerMenuStrip);
            this.Name = "DataViewer";
            this.Text = "DataViewer";
            this.DataViewerMenuStrip.ResumeLayout(false);
            this.DataViewerMenuStrip.PerformLayout();
            this.DataViewerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataViewerMainTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip DataViewerMenuStrip;
        public System.Windows.Forms.Panel DataViewerPanel;
        public System.Windows.Forms.ToolStripComboBox SelectObjectDropDown;
        public System.Windows.Forms.ToolStripMenuItem DataViewerSearchButton;
        public System.Windows.Forms.DataGridView DataViewerMainTable;
        public System.Windows.Forms.ToolStripTextBox DataViewerInputField;
    }
}