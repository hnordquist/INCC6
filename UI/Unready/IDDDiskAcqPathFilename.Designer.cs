namespace UI
{
    partial class IDDDiskAcqPathFilename
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
            this.PassiveBtn = new System.Windows.Forms.Button();
            this.ActiveBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PassiveTextBox = new System.Windows.Forms.TextBox();
            this.ActiveTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PassiveBtn
            // 
            this.PassiveBtn.Location = new System.Drawing.Point(12, 12);
            this.PassiveBtn.Name = "PassiveBtn";
            this.PassiveBtn.Size = new System.Drawing.Size(129, 23);
            this.PassiveBtn.TabIndex = 0;
            this.PassiveBtn.Text = "Passive data file...";
            this.PassiveBtn.UseVisualStyleBackColor = true;
            this.PassiveBtn.Click += new System.EventHandler(this.PassiveBtn_Click);
            // 
            // ActiveBtn
            // 
            this.ActiveBtn.Location = new System.Drawing.Point(12, 68);
            this.ActiveBtn.Name = "ActiveBtn";
            this.ActiveBtn.Size = new System.Drawing.Size(129, 23);
            this.ActiveBtn.TabIndex = 1;
            this.ActiveBtn.Text = "Active data file...";
            this.ActiveBtn.UseVisualStyleBackColor = true;
            this.ActiveBtn.Click += new System.EventHandler(this.ActiveBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(394, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(394, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(394, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PassiveTextBox
            // 
            this.PassiveTextBox.Location = new System.Drawing.Point(162, 12);
            this.PassiveTextBox.Name = "PassiveTextBox";
            this.PassiveTextBox.ReadOnly = true;
            this.PassiveTextBox.Size = new System.Drawing.Size(188, 20);
            this.PassiveTextBox.TabIndex = 6;
            this.PassiveTextBox.TextChanged += new System.EventHandler(this.PassiveTextBox_TextChanged);
            // 
            // ActiveTextBox
            // 
            this.ActiveTextBox.Location = new System.Drawing.Point(162, 70);
            this.ActiveTextBox.Name = "ActiveTextBox";
            this.ActiveTextBox.ReadOnly = true;
            this.ActiveTextBox.Size = new System.Drawing.Size(188, 20);
            this.ActiveTextBox.TabIndex = 7;
            this.ActiveTextBox.TextChanged += new System.EventHandler(this.ActiveTextBox_TextChanged);
            // 
            // IDDDiskAcqPathFilename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 103);
            this.Controls.Add(this.ActiveTextBox);
            this.Controls.Add(this.PassiveTextBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.ActiveBtn);
            this.Controls.Add(this.PassiveBtn);
            this.Name = "IDDDiskAcqPathFilename";
            this.Text = "Acquire Data From Disk File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PassiveBtn;
        private System.Windows.Forms.Button ActiveBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.TextBox PassiveTextBox;
        private System.Windows.Forms.TextBox ActiveTextBox;
    }
}