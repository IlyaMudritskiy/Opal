namespace Opal.src.Forms
{
    partial class UserLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLoginForm));
            this.Username_lbl = new System.Windows.Forms.Label();
            this.Password_lbl = new System.Windows.Forms.Label();
            this.Username_txb = new System.Windows.Forms.TextBox();
            this.Password_txb = new System.Windows.Forms.TextBox();
            this.Login_btn = new System.Windows.Forms.Button();
            this.ResultMessage_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Username_lbl
            // 
            this.Username_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Username_lbl.AutoSize = true;
            this.Username_lbl.Location = new System.Drawing.Point(12, 9);
            this.Username_lbl.Name = "Username_lbl";
            this.Username_lbl.Size = new System.Drawing.Size(60, 13);
            this.Username_lbl.TabIndex = 0;
            this.Username_lbl.Text = "User Name";
            // 
            // Password_lbl
            // 
            this.Password_lbl.AutoSize = true;
            this.Password_lbl.Location = new System.Drawing.Point(9, 51);
            this.Password_lbl.Name = "Password_lbl";
            this.Password_lbl.Size = new System.Drawing.Size(53, 13);
            this.Password_lbl.TabIndex = 1;
            this.Password_lbl.Text = "Password";
            // 
            // Username_txb
            // 
            this.Username_txb.Location = new System.Drawing.Point(12, 25);
            this.Username_txb.Name = "Username_txb";
            this.Username_txb.Size = new System.Drawing.Size(194, 20);
            this.Username_txb.TabIndex = 2;
            // 
            // Password_txb
            // 
            this.Password_txb.Location = new System.Drawing.Point(12, 67);
            this.Password_txb.Name = "Password_txb";
            this.Password_txb.PasswordChar = '*';
            this.Password_txb.Size = new System.Drawing.Size(194, 20);
            this.Password_txb.TabIndex = 3;
            this.Password_txb.UseSystemPasswordChar = true;
            this.Password_txb.WordWrap = false;
            // 
            // Login_btn
            // 
            this.Login_btn.Location = new System.Drawing.Point(12, 93);
            this.Login_btn.Name = "Login_btn";
            this.Login_btn.Size = new System.Drawing.Size(194, 23);
            this.Login_btn.TabIndex = 4;
            this.Login_btn.Text = "Log In";
            this.Login_btn.UseVisualStyleBackColor = true;
            // 
            // ResultMessage_lbl
            // 
            this.ResultMessage_lbl.AutoSize = true;
            this.ResultMessage_lbl.Location = new System.Drawing.Point(12, 128);
            this.ResultMessage_lbl.Name = "ResultMessage_lbl";
            this.ResultMessage_lbl.Size = new System.Drawing.Size(0, 13);
            this.ResultMessage_lbl.TabIndex = 5;
            // 
            // UserLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 156);
            this.Controls.Add(this.Password_lbl);
            this.Controls.Add(this.Username_lbl);
            this.Controls.Add(this.Username_txb);
            this.Controls.Add(this.ResultMessage_lbl);
            this.Controls.Add(this.Login_btn);
            this.Controls.Add(this.Password_txb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserLoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Username_lbl;
        private System.Windows.Forms.Label Password_lbl;
        public System.Windows.Forms.TextBox Username_txb;
        public System.Windows.Forms.TextBox Password_txb;
        public System.Windows.Forms.Button Login_btn;
        public System.Windows.Forms.Label ResultMessage_lbl;
    }
}