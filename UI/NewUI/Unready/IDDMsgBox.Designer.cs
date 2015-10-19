namespace NewUI
{
    partial class IDDMsgBox
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
            this.NotificationTextLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.DontAskAgainCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // NotificationTextLabel
            // 
            this.NotificationTextLabel.AutoSize = true;
            this.NotificationTextLabel.Location = new System.Drawing.Point(12, 9);
            this.NotificationTextLabel.MaximumSize = new System.Drawing.Size(310, 110);
            this.NotificationTextLabel.Name = "NotificationTextLabel";
            this.NotificationTextLabel.Size = new System.Drawing.Size(130, 13);
            this.NotificationTextLabel.TabIndex = 0;
            this.NotificationTextLabel.Text = "Notification text goes here";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(188, 137);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(59, 137);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // DontAskAgainCheckBox
            // 
            this.DontAskAgainCheckBox.AutoSize = true;
            this.DontAskAgainCheckBox.Location = new System.Drawing.Point(102, 166);
            this.DontAskAgainCheckBox.Name = "DontAskAgainCheckBox";
            this.DontAskAgainCheckBox.Size = new System.Drawing.Size(136, 17);
            this.DontAskAgainCheckBox.TabIndex = 4;
            this.DontAskAgainCheckBox.Text = "Don\'t ask me this again";
            this.DontAskAgainCheckBox.UseVisualStyleBackColor = true;
            this.DontAskAgainCheckBox.CheckedChanged += new System.EventHandler(this.DontAskAgainCheckBox_CheckedChanged);
            // 
            // IDDMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 196);
            this.Controls.Add(this.DontAskAgainCheckBox);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NotificationTextLabel);
            this.Name = "IDDMsgBox";
            this.Text = "Replace Me Dynamically";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NotificationTextLabel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.CheckBox DontAskAgainCheckBox;
    }
}