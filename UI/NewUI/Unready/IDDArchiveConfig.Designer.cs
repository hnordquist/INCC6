namespace NewUI
{
    partial class IDDArchiveConfig
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
            this.DaysBeforeFileArchiveTextBox = new System.Windows.Forms.TextBox();
            this.DaysBeforeFileDeleteTextBox = new System.Windows.Forms.TextBox();
            this.DaysBeforeDBDeleteTextBox = new System.Windows.Forms.TextBox();
            this.DaysBeforeFileArchiveLabel = new System.Windows.Forms.Label();
            this.DaysBeforeFileDeleteLabel = new System.Windows.Forms.Label();
            this.DaysBeforeDBDeleteLabel = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DaysBeforeFileArchiveTextBox
            // 
            this.DaysBeforeFileArchiveTextBox.Location = new System.Drawing.Point(258, 14);
            this.DaysBeforeFileArchiveTextBox.Name = "DaysBeforeFileArchiveTextBox";
            this.DaysBeforeFileArchiveTextBox.Size = new System.Drawing.Size(100, 20);
            this.DaysBeforeFileArchiveTextBox.TabIndex = 0;
            this.DaysBeforeFileArchiveTextBox.Leave += new System.EventHandler(this.DaysBeforeFileArchiveTextBox_Leave);
            // 
            // DaysBeforeFileDeleteTextBox
            // 
            this.DaysBeforeFileDeleteTextBox.Location = new System.Drawing.Point(258, 40);
            this.DaysBeforeFileDeleteTextBox.Name = "DaysBeforeFileDeleteTextBox";
            this.DaysBeforeFileDeleteTextBox.Size = new System.Drawing.Size(100, 20);
            this.DaysBeforeFileDeleteTextBox.TabIndex = 1;
            this.DaysBeforeFileDeleteTextBox.Leave += new System.EventHandler(this.DaysBeforeFileDeleteTextBox_Leave);
            // 
            // DaysBeforeDBDeleteTextBox
            // 
            this.DaysBeforeDBDeleteTextBox.Location = new System.Drawing.Point(258, 66);
            this.DaysBeforeDBDeleteTextBox.Name = "DaysBeforeDBDeleteTextBox";
            this.DaysBeforeDBDeleteTextBox.Size = new System.Drawing.Size(100, 20);
            this.DaysBeforeDBDeleteTextBox.TabIndex = 2;
            this.DaysBeforeDBDeleteTextBox.Leave += new System.EventHandler(this.DaysBeforeDBDeleteTextBox_Leave);
            // 
            // DaysBeforeFileArchiveLabel
            // 
            this.DaysBeforeFileArchiveLabel.AutoSize = true;
            this.DaysBeforeFileArchiveLabel.Location = new System.Drawing.Point(36, 17);
            this.DaysBeforeFileArchiveLabel.Name = "DaysBeforeFileArchiveLabel";
            this.DaysBeforeFileArchiveLabel.Size = new System.Drawing.Size(216, 13);
            this.DaysBeforeFileArchiveLabel.TabIndex = 3;
            this.DaysBeforeFileArchiveLabel.Text = "Number of days before data file auto archive";
            // 
            // DaysBeforeFileDeleteLabel
            // 
            this.DaysBeforeFileDeleteLabel.AutoSize = true;
            this.DaysBeforeFileDeleteLabel.Location = new System.Drawing.Point(42, 43);
            this.DaysBeforeFileDeleteLabel.Name = "DaysBeforeFileDeleteLabel";
            this.DaysBeforeFileDeleteLabel.Size = new System.Drawing.Size(210, 13);
            this.DaysBeforeFileDeleteLabel.TabIndex = 4;
            this.DaysBeforeFileDeleteLabel.Text = "Number of days before data file auto delete";
            // 
            // DaysBeforeDBDeleteLabel
            // 
            this.DaysBeforeDBDeleteLabel.AutoSize = true;
            this.DaysBeforeDBDeleteLabel.Location = new System.Drawing.Point(11, 69);
            this.DaysBeforeDBDeleteLabel.Name = "DaysBeforeDBDeleteLabel";
            this.DaysBeforeDBDeleteLabel.Size = new System.Drawing.Size(241, 13);
            this.DaysBeforeDBDeleteLabel.TabIndex = 5;
            this.DaysBeforeDBDeleteLabel.Text = "Number of days before database data auto delete";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(394, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(394, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(394, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDArchiveConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 105);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DaysBeforeDBDeleteLabel);
            this.Controls.Add(this.DaysBeforeFileDeleteLabel);
            this.Controls.Add(this.DaysBeforeFileArchiveLabel);
            this.Controls.Add(this.DaysBeforeDBDeleteTextBox);
            this.Controls.Add(this.DaysBeforeFileDeleteTextBox);
            this.Controls.Add(this.DaysBeforeFileArchiveTextBox);
            this.Name = "IDDArchiveConfig";
            this.Text = "Archive Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DaysBeforeFileArchiveTextBox;
        private System.Windows.Forms.TextBox DaysBeforeFileDeleteTextBox;
        private System.Windows.Forms.TextBox DaysBeforeDBDeleteTextBox;
        private System.Windows.Forms.Label DaysBeforeFileArchiveLabel;
        private System.Windows.Forms.Label DaysBeforeFileDeleteLabel;
        private System.Windows.Forms.Label DaysBeforeDBDeleteLabel;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}