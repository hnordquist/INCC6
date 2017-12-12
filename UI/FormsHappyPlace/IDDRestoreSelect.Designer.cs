namespace UI
{
    partial class IDDRestoreSelect
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PromptForOverwriteRadioButton = new System.Windows.Forms.RadioButton();
            this.AlwaysOverwriteRadioButton = new System.Windows.Forms.RadioButton();
            this.NeverOverwriteRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.DetectorId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Filenname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PromptForOverwriteRadioButton);
            this.groupBox1.Controls.Add(this.AlwaysOverwriteRadioButton);
            this.groupBox1.Controls.Add(this.NeverOverwriteRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // PromptForOverwriteRadioButton
            // 
            this.PromptForOverwriteRadioButton.AutoSize = true;
            this.PromptForOverwriteRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PromptForOverwriteRadioButton.Location = new System.Drawing.Point(16, 65);
            this.PromptForOverwriteRadioButton.Name = "PromptForOverwriteRadioButton";
            this.PromptForOverwriteRadioButton.Size = new System.Drawing.Size(398, 21);
            this.PromptForOverwriteRadioButton.TabIndex = 2;
            this.PromptForOverwriteRadioButton.TabStop = true;
            this.PromptForOverwriteRadioButton.Text = "Prompt to overwrite existing data and database parameters";
            this.PromptForOverwriteRadioButton.UseVisualStyleBackColor = true;
            this.PromptForOverwriteRadioButton.CheckedChanged += new System.EventHandler(this.PromptForOverwriteRadioButton_CheckedChanged);
            // 
            // AlwaysOverwriteRadioButton
            // 
            this.AlwaysOverwriteRadioButton.AutoSize = true;
            this.AlwaysOverwriteRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlwaysOverwriteRadioButton.Location = new System.Drawing.Point(16, 42);
            this.AlwaysOverwriteRadioButton.Name = "AlwaysOverwriteRadioButton";
            this.AlwaysOverwriteRadioButton.Size = new System.Drawing.Size(380, 21);
            this.AlwaysOverwriteRadioButton.TabIndex = 1;
            this.AlwaysOverwriteRadioButton.TabStop = true;
            this.AlwaysOverwriteRadioButton.Text = "Always overwrite existing data and database parameters";
            this.AlwaysOverwriteRadioButton.UseVisualStyleBackColor = true;
            this.AlwaysOverwriteRadioButton.CheckedChanged += new System.EventHandler(this.AlwaysOverwriteRadioButton_CheckedChanged);
            // 
            // NeverOverwriteRadioButton
            // 
            this.NeverOverwriteRadioButton.AutoSize = true;
            this.NeverOverwriteRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NeverOverwriteRadioButton.Location = new System.Drawing.Point(16, 19);
            this.NeverOverwriteRadioButton.Name = "NeverOverwriteRadioButton";
            this.NeverOverwriteRadioButton.Size = new System.Drawing.Size(375, 21);
            this.NeverOverwriteRadioButton.TabIndex = 0;
            this.NeverOverwriteRadioButton.TabStop = true;
            this.NeverOverwriteRadioButton.Text = "Never overwrite existing data and database parameters";
            this.NeverOverwriteRadioButton.UseVisualStyleBackColor = true;
            this.NeverOverwriteRadioButton.CheckedChanged += new System.EventHandler(this.NeverOverwriteRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(680, 19);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(680, 48);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(680, 77);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 10;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DetectorId,
            this.ItemId,
            this.StratumId,
            this.Date,
            this.Time,
            this.Filenname});
            this.listView1.Location = new System.Drawing.Point(12, 131);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(743, 495);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // DetectorId
            // 
            this.DetectorId.Text = "Detector id";
            this.DetectorId.Width = 80;
            // 
            // ItemId
            // 
            this.ItemId.Text = "Item id";
            this.ItemId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ItemId.Width = 80;
            // 
            // StratumId
            // 
            this.StratumId.Text = "Stratum id";
            this.StratumId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StratumId.Width = 80;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Date.Width = 150;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Time.Width = 150;
            // 
            // Filenname
            // 
            this.Filenname.Text = "Filename";
            this.Filenname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Filenname.Width = 200;
            // 
            // IDDRestoreSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 639);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "IDDRestoreSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Measurements to Restore";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton PromptForOverwriteRadioButton;
        private System.Windows.Forms.RadioButton AlwaysOverwriteRadioButton;
        private System.Windows.Forms.RadioButton NeverOverwriteRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader DetectorId;
        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader StratumId;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader Filenname;
    }
}