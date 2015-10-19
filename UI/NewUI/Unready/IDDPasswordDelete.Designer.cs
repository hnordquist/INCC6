namespace NewUI
{
    partial class IDDPasswordDelete
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CurrentPasswordLabel = new System.Windows.Forms.Label();
            this.CurrentPasswordTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(235, 16);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(235, 45);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(235, 74);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CurrentPasswordLabel
            // 
            this.CurrentPasswordLabel.AutoSize = true;
            this.CurrentPasswordLabel.Location = new System.Drawing.Point(12, 21);
            this.CurrentPasswordLabel.Name = "CurrentPasswordLabel";
            this.CurrentPasswordLabel.Size = new System.Drawing.Size(89, 13);
            this.CurrentPasswordLabel.TabIndex = 3;
            this.CurrentPasswordLabel.Text = "Current password";
            // 
            // CurrentPasswordTextBox
            // 
            this.CurrentPasswordTextBox.Location = new System.Drawing.Point(107, 18);
            this.CurrentPasswordTextBox.Name = "CurrentPasswordTextBox";
            this.CurrentPasswordTextBox.PasswordChar = '•';
            this.CurrentPasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.CurrentPasswordTextBox.TabIndex = 4;
            this.CurrentPasswordTextBox.UseSystemPasswordChar = true;
            this.CurrentPasswordTextBox.TextChanged += new System.EventHandler(this.CurrentPasswordTextBox_TextChanged);
            // 
            // IDDPasswordDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 109);
            this.Controls.Add(this.CurrentPasswordTextBox);
            this.Controls.Add(this.CurrentPasswordLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDPasswordDelete";
            this.Text = "Delete current password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label CurrentPasswordLabel;
        private System.Windows.Forms.TextBox CurrentPasswordTextBox;
    }
}