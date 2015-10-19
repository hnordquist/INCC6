namespace NewUI
{
    partial class IDDSaveSelect
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
            this.ListBox1 = new System.Windows.Forms.ListBox();
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.StratumIdLabel = new System.Windows.Forms.Label();
            this.DateLabel = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(697, 34);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(697, 63);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(697, 92);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // ListBox1
            // 
            this.ListBox1.FormattingEnabled = true;
            this.ListBox1.Location = new System.Drawing.Point(12, 34);
            this.ListBox1.Name = "ListBox1";
            this.ListBox1.Size = new System.Drawing.Size(668, 381);
            this.ListBox1.TabIndex = 3;
            this.ListBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemIdLabel.Location = new System.Drawing.Point(12, 13);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(78, 18);
            this.ItemIdLabel.TabIndex = 4;
            this.ItemIdLabel.Text = "Item id";
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StratumIdLabel.Location = new System.Drawing.Point(123, 13);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(108, 18);
            this.StratumIdLabel.TabIndex = 5;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateLabel.Location = new System.Drawing.Point(275, 13);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(48, 18);
            this.DateLabel.TabIndex = 6;
            this.DateLabel.Text = "Date";
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLabel.Location = new System.Drawing.Point(371, 13);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(48, 18);
            this.TimeLabel.TabIndex = 7;
            this.TimeLabel.Text = "Time";
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilenameLabel.Location = new System.Drawing.Point(478, 13);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(88, 18);
            this.FilenameLabel.TabIndex = 8;
            this.FilenameLabel.Text = "Filename";
            // 
            // IDDSaveSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 430);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.StratumIdLabel);
            this.Controls.Add(this.ItemIdLabel);
            this.Controls.Add(this.ListBox1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDSaveSelect";
            this.Text = "Select Measurements to Save";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListBox ListBox1;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label FilenameLabel;
    }
}