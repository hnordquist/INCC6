namespace UI
{
    partial class IDDStratumIdDelete
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
            this.StratumIdLabel = new System.Windows.Forms.Label();
            this.StratumIdComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Location = new System.Drawing.Point(12, 24);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(54, 13);
            this.StratumIdLabel.TabIndex = 0;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // StratumIdComboBox
            // 
            this.StratumIdComboBox.FormattingEnabled = true;
            this.StratumIdComboBox.Location = new System.Drawing.Point(72, 21);
            this.StratumIdComboBox.Name = "StratumIdComboBox";
            this.StratumIdComboBox.Size = new System.Drawing.Size(241, 21);
            this.StratumIdComboBox.TabIndex = 1;
            this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(319, 19);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(319, 77);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(319, 48);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // IDDStratumIdDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 121);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.StratumIdComboBox);
            this.Controls.Add(this.StratumIdLabel);
            this.Name = "IDDStratumIdDelete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a Stratum id to Delete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.ComboBox StratumIdComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}