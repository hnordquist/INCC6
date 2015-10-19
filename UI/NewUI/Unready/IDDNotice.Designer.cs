namespace NewUI
{
    partial class IDDNotice
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
            this.CloseBtn = new System.Windows.Forms.Button();
            this.ImportantNoticeLabel = new System.Windows.Forms.Label();
            this.LANLLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(397, 12);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 0;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // ImportantNoticeLabel
            // 
            this.ImportantNoticeLabel.AutoSize = true;
            this.ImportantNoticeLabel.Location = new System.Drawing.Point(192, 9);
            this.ImportantNoticeLabel.Name = "ImportantNoticeLabel";
            this.ImportantNoticeLabel.Size = new System.Drawing.Size(94, 13);
            this.ImportantNoticeLabel.TabIndex = 1;
            this.ImportantNoticeLabel.Text = "Important Notice...";
            // 
            // LANLLabel
            // 
            this.LANLLabel.AutoSize = true;
            this.LANLLabel.Location = new System.Drawing.Point(49, 248);
            this.LANLLabel.Name = "LANLLabel";
            this.LANLLabel.Size = new System.Drawing.Size(379, 13);
            this.LANLLabel.TabIndex = 2;
            this.LANLLabel.Text = "Los Alamos National Laboratory; NEN-1:  Safeguards Science and Technology";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(155, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 157);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // IDDNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 632);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LANLLabel);
            this.Controls.Add(this.ImportantNoticeLabel);
            this.Controls.Add(this.CloseBtn);
            this.Name = "IDDNotice";
            this.Text = "INCC Legal Notices";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Label ImportantNoticeLabel;
        private System.Windows.Forms.Label LANLLabel;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}