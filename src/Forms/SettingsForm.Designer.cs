namespace Opal.src.Forms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.DataProvider_grp = new System.Windows.Forms.GroupBox();
            this.HubProvider_rbn = new System.Windows.Forms.RadioButton();
            this.ApiProvider_rbn = new System.Windows.Forms.RadioButton();
            this.FilesProvider_rbn = new System.Windows.Forms.RadioButton();
            this.HubProvider_grp = new System.Windows.Forms.GroupBox();
            this.HubProviderDescription_lbl = new System.Windows.Forms.Label();
            this.HubUrl_lbl = new System.Windows.Forms.Label();
            this.HubUrl_txb = new System.Windows.Forms.TextBox();
            this.ApiProvider_grp = new System.Windows.Forms.GroupBox();
            this.ApiProviderDescription_lbl = new System.Windows.Forms.Label();
            this.ApiUrl_txb = new System.Windows.Forms.TextBox();
            this.ApiUrl_lbl = new System.Windows.Forms.Label();
            this.FilesProvider_grp = new System.Windows.Forms.GroupBox();
            this.FilesProviderDescription_lbl = new System.Windows.Forms.Label();
            this.FilesProviderAvoustic_chb = new System.Windows.Forms.CheckBox();
            this.FilesLocation_lbl = new System.Windows.Forms.Label();
            this.FilesLocation_txb = new System.Windows.Forms.TextBox();
            this.LineProduct_grp = new System.Windows.Forms.GroupBox();
            this.ProductId_cmb = new System.Windows.Forms.ComboBox();
            this.LineName_cmb = new System.Windows.Forms.ComboBox();
            this.ProductId_lbl = new System.Windows.Forms.Label();
            this.LineName_lbl = new System.Windows.Forms.Label();
            this.Save_btn = new System.Windows.Forms.Button();
            this.Close_btn = new System.Windows.Forms.Button();
            this.Status_lbl = new System.Windows.Forms.Label();
            this.StatusText_lbl = new System.Windows.Forms.Label();
            this.OtherSettings_grp = new System.Windows.Forms.GroupBox();
            this.DataDriveLetter_cmb = new System.Windows.Forms.ComboBox();
            this.ASxCompliant_chk = new System.Windows.Forms.CheckBox();
            this.DriveLetter_lbl = new System.Windows.Forms.Label();
            this.DataProvider_grp.SuspendLayout();
            this.HubProvider_grp.SuspendLayout();
            this.ApiProvider_grp.SuspendLayout();
            this.FilesProvider_grp.SuspendLayout();
            this.LineProduct_grp.SuspendLayout();
            this.OtherSettings_grp.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataProvider_grp
            // 
            this.DataProvider_grp.Controls.Add(this.HubProvider_rbn);
            this.DataProvider_grp.Controls.Add(this.ApiProvider_rbn);
            this.DataProvider_grp.Controls.Add(this.FilesProvider_rbn);
            this.DataProvider_grp.Controls.Add(this.HubProvider_grp);
            this.DataProvider_grp.Controls.Add(this.ApiProvider_grp);
            this.DataProvider_grp.Controls.Add(this.FilesProvider_grp);
            this.DataProvider_grp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataProvider_grp.Location = new System.Drawing.Point(12, 12);
            this.DataProvider_grp.Name = "DataProvider_grp";
            this.DataProvider_grp.Size = new System.Drawing.Size(429, 286);
            this.DataProvider_grp.TabIndex = 0;
            this.DataProvider_grp.TabStop = false;
            this.DataProvider_grp.Text = "Data Provider";
            // 
            // HubProvider_rbn
            // 
            this.HubProvider_rbn.AutoSize = true;
            this.HubProvider_rbn.Location = new System.Drawing.Point(6, 216);
            this.HubProvider_rbn.Name = "HubProvider_rbn";
            this.HubProvider_rbn.Size = new System.Drawing.Size(14, 13);
            this.HubProvider_rbn.TabIndex = 3;
            this.HubProvider_rbn.TabStop = true;
            this.HubProvider_rbn.UseVisualStyleBackColor = true;
            // 
            // ApiProvider_rbn
            // 
            this.ApiProvider_rbn.AutoSize = true;
            this.ApiProvider_rbn.Location = new System.Drawing.Point(6, 143);
            this.ApiProvider_rbn.Name = "ApiProvider_rbn";
            this.ApiProvider_rbn.Size = new System.Drawing.Size(14, 13);
            this.ApiProvider_rbn.TabIndex = 1;
            this.ApiProvider_rbn.TabStop = true;
            this.ApiProvider_rbn.UseVisualStyleBackColor = true;
            // 
            // FilesProvider_rbn
            // 
            this.FilesProvider_rbn.AutoSize = true;
            this.FilesProvider_rbn.Location = new System.Drawing.Point(6, 26);
            this.FilesProvider_rbn.Name = "FilesProvider_rbn";
            this.FilesProvider_rbn.Size = new System.Drawing.Size(14, 13);
            this.FilesProvider_rbn.TabIndex = 0;
            this.FilesProvider_rbn.TabStop = true;
            this.FilesProvider_rbn.UseVisualStyleBackColor = true;
            // 
            // HubProvider_grp
            // 
            this.HubProvider_grp.Controls.Add(this.HubProviderDescription_lbl);
            this.HubProvider_grp.Controls.Add(this.HubUrl_lbl);
            this.HubProvider_grp.Controls.Add(this.HubUrl_txb);
            this.HubProvider_grp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HubProvider_grp.Location = new System.Drawing.Point(26, 209);
            this.HubProvider_grp.Name = "HubProvider_grp";
            this.HubProvider_grp.Size = new System.Drawing.Size(392, 65);
            this.HubProvider_grp.TabIndex = 2;
            this.HubProvider_grp.TabStop = false;
            this.HubProvider_grp.Text = "Onyx Notifications Hub";
            // 
            // HubProviderDescription_lbl
            // 
            this.HubProviderDescription_lbl.AutoSize = true;
            this.HubProviderDescription_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HubProviderDescription_lbl.Location = new System.Drawing.Point(7, 16);
            this.HubProviderDescription_lbl.Name = "HubProviderDescription_lbl";
            this.HubProviderDescription_lbl.Size = new System.Drawing.Size(308, 13);
            this.HubProviderDescription_lbl.TabIndex = 6;
            this.HubProviderDescription_lbl.Text = "App will subscribe to updates from API if the service is available.";
            // 
            // HubUrl_lbl
            // 
            this.HubUrl_lbl.AutoSize = true;
            this.HubUrl_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HubUrl_lbl.Location = new System.Drawing.Point(7, 40);
            this.HubUrl_lbl.Name = "HubUrl_lbl";
            this.HubUrl_lbl.Size = new System.Drawing.Size(52, 13);
            this.HubUrl_lbl.TabIndex = 1;
            this.HubUrl_lbl.Text = "Hub URL";
            // 
            // HubUrl_txb
            // 
            this.HubUrl_txb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HubUrl_txb.Location = new System.Drawing.Point(77, 37);
            this.HubUrl_txb.Name = "HubUrl_txb";
            this.HubUrl_txb.Size = new System.Drawing.Size(309, 20);
            this.HubUrl_txb.TabIndex = 0;
            // 
            // ApiProvider_grp
            // 
            this.ApiProvider_grp.Controls.Add(this.ApiProviderDescription_lbl);
            this.ApiProvider_grp.Controls.Add(this.ApiUrl_txb);
            this.ApiProvider_grp.Controls.Add(this.ApiUrl_lbl);
            this.ApiProvider_grp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApiProvider_grp.Location = new System.Drawing.Point(26, 136);
            this.ApiProvider_grp.Name = "ApiProvider_grp";
            this.ApiProvider_grp.Size = new System.Drawing.Size(392, 67);
            this.ApiProvider_grp.TabIndex = 1;
            this.ApiProvider_grp.TabStop = false;
            this.ApiProvider_grp.Text = "Onyx API";
            // 
            // ApiProviderDescription_lbl
            // 
            this.ApiProviderDescription_lbl.AutoSize = true;
            this.ApiProviderDescription_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApiProviderDescription_lbl.Location = new System.Drawing.Point(7, 16);
            this.ApiProviderDescription_lbl.Name = "ApiProviderDescription_lbl";
            this.ApiProviderDescription_lbl.Size = new System.Drawing.Size(230, 13);
            this.ApiProviderDescription_lbl.TabIndex = 5;
            this.ApiProviderDescription_lbl.Text = "App will retrieve data from API if it is accessible.";
            // 
            // ApiUrl_txb
            // 
            this.ApiUrl_txb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApiUrl_txb.Location = new System.Drawing.Point(77, 37);
            this.ApiUrl_txb.Name = "ApiUrl_txb";
            this.ApiUrl_txb.Size = new System.Drawing.Size(309, 20);
            this.ApiUrl_txb.TabIndex = 1;
            // 
            // ApiUrl_lbl
            // 
            this.ApiUrl_lbl.AutoSize = true;
            this.ApiUrl_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApiUrl_lbl.Location = new System.Drawing.Point(7, 40);
            this.ApiUrl_lbl.Name = "ApiUrl_lbl";
            this.ApiUrl_lbl.Size = new System.Drawing.Size(49, 13);
            this.ApiUrl_lbl.TabIndex = 0;
            this.ApiUrl_lbl.Text = "API URL";
            // 
            // FilesProvider_grp
            // 
            this.FilesProvider_grp.Controls.Add(this.FilesProviderDescription_lbl);
            this.FilesProvider_grp.Controls.Add(this.FilesProviderAvoustic_chb);
            this.FilesProvider_grp.Controls.Add(this.FilesLocation_lbl);
            this.FilesProvider_grp.Controls.Add(this.FilesLocation_txb);
            this.FilesProvider_grp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilesProvider_grp.Location = new System.Drawing.Point(26, 19);
            this.FilesProvider_grp.Name = "FilesProvider_grp";
            this.FilesProvider_grp.Size = new System.Drawing.Size(392, 111);
            this.FilesProvider_grp.TabIndex = 0;
            this.FilesProvider_grp.TabStop = false;
            this.FilesProvider_grp.Text = "File(s)";
            // 
            // FilesProviderDescription_lbl
            // 
            this.FilesProviderDescription_lbl.AutoSize = true;
            this.FilesProviderDescription_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilesProviderDescription_lbl.Location = new System.Drawing.Point(7, 16);
            this.FilesProviderDescription_lbl.Name = "FilesProviderDescription_lbl";
            this.FilesProviderDescription_lbl.Size = new System.Drawing.Size(354, 39);
            this.FilesProviderDescription_lbl.TabIndex = 4;
            this.FilesProviderDescription_lbl.Text = resources.GetString("FilesProviderDescription_lbl.Text");
            // 
            // FilesProviderAvoustic_chb
            // 
            this.FilesProviderAvoustic_chb.AutoSize = true;
            this.FilesProviderAvoustic_chb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilesProviderAvoustic_chb.Location = new System.Drawing.Point(6, 65);
            this.FilesProviderAvoustic_chb.Name = "FilesProviderAvoustic_chb";
            this.FilesProviderAvoustic_chb.Size = new System.Drawing.Size(139, 17);
            this.FilesProviderAvoustic_chb.TabIndex = 3;
            this.FilesProviderAvoustic_chb.Text = "Acoustic Tabs and Files";
            this.FilesProviderAvoustic_chb.UseVisualStyleBackColor = true;
            // 
            // FilesLocation_lbl
            // 
            this.FilesLocation_lbl.AutoSize = true;
            this.FilesLocation_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilesLocation_lbl.Location = new System.Drawing.Point(2, 84);
            this.FilesLocation_lbl.Name = "FilesLocation_lbl";
            this.FilesLocation_lbl.Size = new System.Drawing.Size(68, 13);
            this.FilesLocation_lbl.TabIndex = 2;
            this.FilesLocation_lbl.Text = "Files location";
            // 
            // FilesLocation_txb
            // 
            this.FilesLocation_txb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilesLocation_txb.Location = new System.Drawing.Point(76, 81);
            this.FilesLocation_txb.Name = "FilesLocation_txb";
            this.FilesLocation_txb.Size = new System.Drawing.Size(309, 20);
            this.FilesLocation_txb.TabIndex = 0;
            // 
            // LineProduct_grp
            // 
            this.LineProduct_grp.Controls.Add(this.ProductId_cmb);
            this.LineProduct_grp.Controls.Add(this.LineName_cmb);
            this.LineProduct_grp.Controls.Add(this.ProductId_lbl);
            this.LineProduct_grp.Controls.Add(this.LineName_lbl);
            this.LineProduct_grp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LineProduct_grp.Location = new System.Drawing.Point(12, 304);
            this.LineProduct_grp.Name = "LineProduct_grp";
            this.LineProduct_grp.Size = new System.Drawing.Size(216, 82);
            this.LineProduct_grp.TabIndex = 1;
            this.LineProduct_grp.TabStop = false;
            this.LineProduct_grp.Text = "Line and Product";
            // 
            // ProductId_cmb
            // 
            this.ProductId_cmb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductId_cmb.FormattingEnabled = true;
            this.ProductId_cmb.Location = new System.Drawing.Point(102, 50);
            this.ProductId_cmb.Name = "ProductId_cmb";
            this.ProductId_cmb.Size = new System.Drawing.Size(105, 21);
            this.ProductId_cmb.TabIndex = 5;
            // 
            // LineName_cmb
            // 
            this.LineName_cmb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LineName_cmb.FormattingEnabled = true;
            this.LineName_cmb.Location = new System.Drawing.Point(102, 23);
            this.LineName_cmb.Name = "LineName_cmb";
            this.LineName_cmb.Size = new System.Drawing.Size(105, 21);
            this.LineName_cmb.TabIndex = 4;
            // 
            // ProductId_lbl
            // 
            this.ProductId_lbl.AutoSize = true;
            this.ProductId_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductId_lbl.Location = new System.Drawing.Point(6, 53);
            this.ProductId_lbl.Name = "ProductId_lbl";
            this.ProductId_lbl.Size = new System.Drawing.Size(58, 13);
            this.ProductId_lbl.TabIndex = 3;
            this.ProductId_lbl.Text = "Product ID";
            // 
            // LineName_lbl
            // 
            this.LineName_lbl.AutoSize = true;
            this.LineName_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LineName_lbl.Location = new System.Drawing.Point(6, 26);
            this.LineName_lbl.Name = "LineName_lbl";
            this.LineName_lbl.Size = new System.Drawing.Size(58, 13);
            this.LineName_lbl.TabIndex = 2;
            this.LineName_lbl.Text = "Line Name";
            // 
            // Save_btn
            // 
            this.Save_btn.Location = new System.Drawing.Point(366, 392);
            this.Save_btn.Name = "Save_btn";
            this.Save_btn.Size = new System.Drawing.Size(75, 23);
            this.Save_btn.TabIndex = 2;
            this.Save_btn.Text = "Save";
            this.Save_btn.UseVisualStyleBackColor = true;
            // 
            // Close_btn
            // 
            this.Close_btn.Location = new System.Drawing.Point(285, 392);
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.Size = new System.Drawing.Size(75, 23);
            this.Close_btn.TabIndex = 3;
            this.Close_btn.Text = "Close";
            this.Close_btn.UseVisualStyleBackColor = true;
            // 
            // Status_lbl
            // 
            this.Status_lbl.AutoSize = true;
            this.Status_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_lbl.Location = new System.Drawing.Point(9, 397);
            this.Status_lbl.Name = "Status_lbl";
            this.Status_lbl.Size = new System.Drawing.Size(51, 15);
            this.Status_lbl.TabIndex = 4;
            this.Status_lbl.Text = "Status:";
            // 
            // StatusText_lbl
            // 
            this.StatusText_lbl.AutoSize = true;
            this.StatusText_lbl.Location = new System.Drawing.Point(66, 399);
            this.StatusText_lbl.Name = "StatusText_lbl";
            this.StatusText_lbl.Size = new System.Drawing.Size(52, 13);
            this.StatusText_lbl.TabIndex = 5;
            this.StatusText_lbl.Text = "No status";
            // 
            // OtherSettings_grp
            // 
            this.OtherSettings_grp.Controls.Add(this.DataDriveLetter_cmb);
            this.OtherSettings_grp.Controls.Add(this.ASxCompliant_chk);
            this.OtherSettings_grp.Controls.Add(this.DriveLetter_lbl);
            this.OtherSettings_grp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OtherSettings_grp.Location = new System.Drawing.Point(235, 305);
            this.OtherSettings_grp.Name = "OtherSettings_grp";
            this.OtherSettings_grp.Size = new System.Drawing.Size(206, 81);
            this.OtherSettings_grp.TabIndex = 6;
            this.OtherSettings_grp.TabStop = false;
            this.OtherSettings_grp.Text = "Other Settings";
            // 
            // DataDriveLetter_cmb
            // 
            this.DataDriveLetter_cmb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataDriveLetter_cmb.FormattingEnabled = true;
            this.DataDriveLetter_cmb.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "G",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.DataDriveLetter_cmb.Location = new System.Drawing.Point(101, 22);
            this.DataDriveLetter_cmb.Name = "DataDriveLetter_cmb";
            this.DataDriveLetter_cmb.Size = new System.Drawing.Size(49, 21);
            this.DataDriveLetter_cmb.TabIndex = 2;
            // 
            // ASxCompliant_chk
            // 
            this.ASxCompliant_chk.AutoSize = true;
            this.ASxCompliant_chk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ASxCompliant_chk.Location = new System.Drawing.Point(10, 51);
            this.ASxCompliant_chk.Name = "ASxCompliant_chk";
            this.ASxCompliant_chk.Size = new System.Drawing.Size(163, 17);
            this.ASxCompliant_chk.TabIndex = 1;
            this.ASxCompliant_chk.Text = "ASx Reports Compliant mode";
            this.ASxCompliant_chk.UseVisualStyleBackColor = true;
            // 
            // DriveLetter_lbl
            // 
            this.DriveLetter_lbl.AutoSize = true;
            this.DriveLetter_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriveLetter_lbl.Location = new System.Drawing.Point(7, 25);
            this.DriveLetter_lbl.Name = "DriveLetter_lbl";
            this.DriveLetter_lbl.Size = new System.Drawing.Size(88, 13);
            this.DriveLetter_lbl.TabIndex = 0;
            this.DriveLetter_lbl.Text = "Data Drive Letter";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(452, 424);
            this.Controls.Add(this.OtherSettings_grp);
            this.Controls.Add(this.StatusText_lbl);
            this.Controls.Add(this.Status_lbl);
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.Save_btn);
            this.Controls.Add(this.LineProduct_grp);
            this.Controls.Add(this.DataProvider_grp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Opal Settings";
            this.DataProvider_grp.ResumeLayout(false);
            this.DataProvider_grp.PerformLayout();
            this.HubProvider_grp.ResumeLayout(false);
            this.HubProvider_grp.PerformLayout();
            this.ApiProvider_grp.ResumeLayout(false);
            this.ApiProvider_grp.PerformLayout();
            this.FilesProvider_grp.ResumeLayout(false);
            this.FilesProvider_grp.PerformLayout();
            this.LineProduct_grp.ResumeLayout(false);
            this.LineProduct_grp.PerformLayout();
            this.OtherSettings_grp.ResumeLayout(false);
            this.OtherSettings_grp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox DataProvider_grp;
        internal System.Windows.Forms.GroupBox HubProvider_grp;
        internal System.Windows.Forms.GroupBox ApiProvider_grp;
        internal System.Windows.Forms.GroupBox FilesProvider_grp;
        internal System.Windows.Forms.RadioButton HubProvider_rbn;
        internal System.Windows.Forms.RadioButton ApiProvider_rbn;
        internal System.Windows.Forms.RadioButton FilesProvider_rbn;
        internal System.Windows.Forms.TextBox FilesLocation_txb;
        private System.Windows.Forms.Label HubUrl_lbl;
        internal System.Windows.Forms.TextBox HubUrl_txb;
        internal System.Windows.Forms.TextBox ApiUrl_txb;
        private System.Windows.Forms.Label ApiUrl_lbl;
        private System.Windows.Forms.Label FilesProviderDescription_lbl;
        private System.Windows.Forms.Label FilesLocation_lbl;
        private System.Windows.Forms.Label HubProviderDescription_lbl;
        private System.Windows.Forms.Label ApiProviderDescription_lbl;
        internal System.Windows.Forms.CheckBox FilesProviderAvoustic_chb;
        internal System.Windows.Forms.GroupBox LineProduct_grp;
        private System.Windows.Forms.Label ProductId_lbl;
        private System.Windows.Forms.Label LineName_lbl;
        internal System.Windows.Forms.Button Save_btn;
        internal System.Windows.Forms.Button Close_btn;
        internal System.Windows.Forms.ComboBox ProductId_cmb;
        internal System.Windows.Forms.ComboBox LineName_cmb;
        internal System.Windows.Forms.Label StatusText_lbl;
        internal System.Windows.Forms.ComboBox DataDriveLetter_cmb;
        internal System.Windows.Forms.CheckBox ASxCompliant_chk;
        private System.Windows.Forms.Label DriveLetter_lbl;
        internal System.Windows.Forms.GroupBox OtherSettings_grp;
        internal System.Windows.Forms.Label Status_lbl;
    }
}