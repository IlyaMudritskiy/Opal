namespace Opal.src.Forms
{
    partial class APIDataSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APIDataSelectorForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Confirm_btn = new System.Windows.Forms.Button();
            this.DateFrom_dtp = new System.Windows.Forms.DateTimePicker();
            this.ThisHour_rbn = new System.Windows.Forms.RadioButton();
            this.Range_rbn = new System.Windows.Forms.RadioButton();
            this.LastHour_rbn = new System.Windows.Forms.RadioButton();
            this.DateTimeRange_grp = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LastMinutes_txb = new System.Windows.Forms.TextBox();
            this.LastMinutes_rbn = new System.Windows.Forms.RadioButton();
            this.Range_grp = new System.Windows.Forms.GroupBox();
            this.TimeTo_dtp = new System.Windows.Forms.DateTimePicker();
            this.DateTo_dtp = new System.Windows.Forms.DateTimePicker();
            this.TimeFrom_dtp = new System.Windows.Forms.DateTimePicker();
            this.Serial_grp = new System.Windows.Forms.GroupBox();
            this.Serial_txb = new System.Windows.Forms.TextBox();
            this.CustomQuery_grp = new System.Windows.Forms.GroupBox();
            this.Clear_btn = new System.Windows.Forms.Button();
            this.CustomQuery_cmb = new System.Windows.Forms.ComboBox();
            this.RefreshQueries_btn = new System.Windows.Forms.Button();
            this.DateTimeRange_grp.SuspendLayout();
            this.Range_grp.SuspendLayout();
            this.Serial_grp.SuspendLayout();
            this.CustomQuery_grp.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "From:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "To:";
            // 
            // Confirm_btn
            // 
            this.Confirm_btn.Location = new System.Drawing.Point(208, 345);
            this.Confirm_btn.Name = "Confirm_btn";
            this.Confirm_btn.Size = new System.Drawing.Size(131, 23);
            this.Confirm_btn.TabIndex = 7;
            this.Confirm_btn.Text = "Confirm";
            this.Confirm_btn.UseVisualStyleBackColor = true;
            // 
            // DateFrom_dtp
            // 
            this.DateFrom_dtp.CustomFormat = "dddd dd.MM.yyyy";
            this.DateFrom_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateFrom_dtp.ImeMode = System.Windows.Forms.ImeMode.On;
            this.DateFrom_dtp.Location = new System.Drawing.Point(45, 19);
            this.DateFrom_dtp.Name = "DateFrom_dtp";
            this.DateFrom_dtp.Size = new System.Drawing.Size(165, 20);
            this.DateFrom_dtp.TabIndex = 8;
            // 
            // ThisHour_rbn
            // 
            this.ThisHour_rbn.AutoSize = true;
            this.ThisHour_rbn.Checked = true;
            this.ThisHour_rbn.Location = new System.Drawing.Point(6, 19);
            this.ThisHour_rbn.Name = "ThisHour_rbn";
            this.ThisHour_rbn.Size = new System.Drawing.Size(222, 17);
            this.ThisHour_rbn.TabIndex = 10;
            this.ThisHour_rbn.TabStop = true;
            this.ThisHour_rbn.Text = "This hour (start of the hour to current time)";
            this.ThisHour_rbn.UseVisualStyleBackColor = true;
            // 
            // Range_rbn
            // 
            this.Range_rbn.AutoSize = true;
            this.Range_rbn.Location = new System.Drawing.Point(6, 96);
            this.Range_rbn.Name = "Range_rbn";
            this.Range_rbn.Size = new System.Drawing.Size(14, 13);
            this.Range_rbn.TabIndex = 11;
            this.Range_rbn.UseVisualStyleBackColor = true;
            // 
            // LastHour_rbn
            // 
            this.LastHour_rbn.AutoSize = true;
            this.LastHour_rbn.Location = new System.Drawing.Point(6, 42);
            this.LastHour_rbn.Name = "LastHour_rbn";
            this.LastHour_rbn.Size = new System.Drawing.Size(90, 17);
            this.LastHour_rbn.TabIndex = 12;
            this.LastHour_rbn.Text = "Previous hour";
            this.LastHour_rbn.UseVisualStyleBackColor = true;
            // 
            // DateTimeRange_grp
            // 
            this.DateTimeRange_grp.Controls.Add(this.label1);
            this.DateTimeRange_grp.Controls.Add(this.LastMinutes_txb);
            this.DateTimeRange_grp.Controls.Add(this.LastMinutes_rbn);
            this.DateTimeRange_grp.Controls.Add(this.Range_grp);
            this.DateTimeRange_grp.Controls.Add(this.ThisHour_rbn);
            this.DateTimeRange_grp.Controls.Add(this.Range_rbn);
            this.DateTimeRange_grp.Controls.Add(this.LastHour_rbn);
            this.DateTimeRange_grp.Location = new System.Drawing.Point(12, 12);
            this.DateTimeRange_grp.Name = "DateTimeRange_grp";
            this.DateTimeRange_grp.Size = new System.Drawing.Size(327, 187);
            this.DateTimeRange_grp.TabIndex = 13;
            this.DateTimeRange_grp.TabStop = false;
            this.DateTimeRange_grp.Text = "Date and Time range";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "minutes";
            // 
            // LastMinutes_txb
            // 
            this.LastMinutes_txb.Location = new System.Drawing.Point(55, 64);
            this.LastMinutes_txb.Name = "LastMinutes_txb";
            this.LastMinutes_txb.Size = new System.Drawing.Size(37, 20);
            this.LastMinutes_txb.TabIndex = 16;
            // 
            // LastMinutes_rbn
            // 
            this.LastMinutes_rbn.AutoSize = true;
            this.LastMinutes_rbn.Location = new System.Drawing.Point(6, 65);
            this.LastMinutes_rbn.Name = "LastMinutes_rbn";
            this.LastMinutes_rbn.Size = new System.Drawing.Size(45, 17);
            this.LastMinutes_rbn.TabIndex = 15;
            this.LastMinutes_rbn.Text = "Last";
            this.LastMinutes_rbn.UseVisualStyleBackColor = true;
            // 
            // Range_grp
            // 
            this.Range_grp.Controls.Add(this.TimeTo_dtp);
            this.Range_grp.Controls.Add(this.DateTo_dtp);
            this.Range_grp.Controls.Add(this.TimeFrom_dtp);
            this.Range_grp.Controls.Add(this.DateFrom_dtp);
            this.Range_grp.Controls.Add(this.label3);
            this.Range_grp.Controls.Add(this.label2);
            this.Range_grp.Location = new System.Drawing.Point(26, 90);
            this.Range_grp.Name = "Range_grp";
            this.Range_grp.Size = new System.Drawing.Size(293, 89);
            this.Range_grp.TabIndex = 14;
            this.Range_grp.TabStop = false;
            this.Range_grp.Text = "Range";
            // 
            // TimeTo_dtp
            // 
            this.TimeTo_dtp.CustomFormat = "HH:mm";
            this.TimeTo_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.TimeTo_dtp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TimeTo_dtp.Location = new System.Drawing.Point(216, 55);
            this.TimeTo_dtp.Name = "TimeTo_dtp";
            this.TimeTo_dtp.ShowUpDown = true;
            this.TimeTo_dtp.Size = new System.Drawing.Size(66, 20);
            this.TimeTo_dtp.TabIndex = 12;
            // 
            // DateTo_dtp
            // 
            this.DateTo_dtp.CustomFormat = "dddd dd.MM.yyyy";
            this.DateTo_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTo_dtp.ImeMode = System.Windows.Forms.ImeMode.On;
            this.DateTo_dtp.Location = new System.Drawing.Point(45, 55);
            this.DateTo_dtp.Name = "DateTo_dtp";
            this.DateTo_dtp.Size = new System.Drawing.Size(165, 20);
            this.DateTo_dtp.TabIndex = 11;
            // 
            // TimeFrom_dtp
            // 
            this.TimeFrom_dtp.CustomFormat = "HH:mm";
            this.TimeFrom_dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.TimeFrom_dtp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TimeFrom_dtp.Location = new System.Drawing.Point(216, 19);
            this.TimeFrom_dtp.Name = "TimeFrom_dtp";
            this.TimeFrom_dtp.ShowUpDown = true;
            this.TimeFrom_dtp.Size = new System.Drawing.Size(66, 20);
            this.TimeFrom_dtp.TabIndex = 10;
            // 
            // Serial_grp
            // 
            this.Serial_grp.Controls.Add(this.Serial_txb);
            this.Serial_grp.Location = new System.Drawing.Point(12, 205);
            this.Serial_grp.Name = "Serial_grp";
            this.Serial_grp.Size = new System.Drawing.Size(327, 49);
            this.Serial_grp.TabIndex = 15;
            this.Serial_grp.TabStop = false;
            this.Serial_grp.Text = "Serial Number";
            // 
            // Serial_txb
            // 
            this.Serial_txb.Location = new System.Drawing.Point(6, 19);
            this.Serial_txb.Name = "Serial_txb";
            this.Serial_txb.Size = new System.Drawing.Size(313, 20);
            this.Serial_txb.TabIndex = 0;
            // 
            // CustomQuery_grp
            // 
            this.CustomQuery_grp.Controls.Add(this.Clear_btn);
            this.CustomQuery_grp.Controls.Add(this.CustomQuery_cmb);
            this.CustomQuery_grp.Controls.Add(this.RefreshQueries_btn);
            this.CustomQuery_grp.Enabled = false;
            this.CustomQuery_grp.Location = new System.Drawing.Point(12, 260);
            this.CustomQuery_grp.Name = "CustomQuery_grp";
            this.CustomQuery_grp.Size = new System.Drawing.Size(327, 79);
            this.CustomQuery_grp.TabIndex = 17;
            this.CustomQuery_grp.TabStop = false;
            this.CustomQuery_grp.Text = "Custom Query";
            // 
            // Clear_btn
            // 
            this.Clear_btn.Enabled = false;
            this.Clear_btn.Location = new System.Drawing.Point(244, 46);
            this.Clear_btn.Name = "Clear_btn";
            this.Clear_btn.Size = new System.Drawing.Size(75, 23);
            this.Clear_btn.TabIndex = 2;
            this.Clear_btn.Text = "Clear";
            this.Clear_btn.UseVisualStyleBackColor = true;
            // 
            // CustomQuery_cmb
            // 
            this.CustomQuery_cmb.Enabled = false;
            this.CustomQuery_cmb.FormattingEnabled = true;
            this.CustomQuery_cmb.Location = new System.Drawing.Point(6, 19);
            this.CustomQuery_cmb.Name = "CustomQuery_cmb";
            this.CustomQuery_cmb.Size = new System.Drawing.Size(313, 21);
            this.CustomQuery_cmb.TabIndex = 1;
            // 
            // RefreshQueries_btn
            // 
            this.RefreshQueries_btn.Enabled = false;
            this.RefreshQueries_btn.Location = new System.Drawing.Point(161, 46);
            this.RefreshQueries_btn.Name = "RefreshQueries_btn";
            this.RefreshQueries_btn.Size = new System.Drawing.Size(75, 23);
            this.RefreshQueries_btn.TabIndex = 0;
            this.RefreshQueries_btn.Text = "Refresh";
            this.RefreshQueries_btn.UseVisualStyleBackColor = true;
            // 
            // APIDataSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 377);
            this.Controls.Add(this.CustomQuery_grp);
            this.Controls.Add(this.Serial_grp);
            this.Controls.Add(this.DateTimeRange_grp);
            this.Controls.Add(this.Confirm_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "APIDataSelectorForm";
            this.Text = "Select data from API";
            this.DateTimeRange_grp.ResumeLayout(false);
            this.DateTimeRange_grp.PerformLayout();
            this.Range_grp.ResumeLayout(false);
            this.Range_grp.PerformLayout();
            this.Serial_grp.ResumeLayout(false);
            this.Serial_grp.PerformLayout();
            this.CustomQuery_grp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button Confirm_btn;
        internal System.Windows.Forms.DateTimePicker DateFrom_dtp;
        internal System.Windows.Forms.RadioButton ThisHour_rbn;
        internal System.Windows.Forms.RadioButton Range_rbn;
        internal System.Windows.Forms.RadioButton LastHour_rbn;
        internal System.Windows.Forms.GroupBox DateTimeRange_grp;
        internal System.Windows.Forms.GroupBox Range_grp;
        internal System.Windows.Forms.DateTimePicker TimeFrom_dtp;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox LastMinutes_txb;
        internal System.Windows.Forms.RadioButton LastMinutes_rbn;
        internal System.Windows.Forms.DateTimePicker TimeTo_dtp;
        internal System.Windows.Forms.DateTimePicker DateTo_dtp;
        internal System.Windows.Forms.GroupBox Serial_grp;
        internal System.Windows.Forms.TextBox Serial_txb;
        private System.Windows.Forms.GroupBox CustomQuery_grp;
        internal System.Windows.Forms.ComboBox CustomQuery_cmb;
        internal System.Windows.Forms.Button RefreshQueries_btn;
        internal System.Windows.Forms.Button Clear_btn;
    }
}