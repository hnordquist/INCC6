namespace NewUI
{
    partial class IDDAcquireDBMeas
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
			this.MeasurementDateTimeLabel = new System.Windows.Forms.Label();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.MeasurementDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.listView1 = new System.Windows.Forms.ListView();
			this.MeasOption = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Filename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Comment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// MeasurementDateTimeLabel
			// 
			this.MeasurementDateTimeLabel.AutoSize = true;
			this.MeasurementDateTimeLabel.Location = new System.Drawing.Point(13, 20);
			this.MeasurementDateTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MeasurementDateTimeLabel.Name = "MeasurementDateTimeLabel";
			this.MeasurementDateTimeLabel.Size = new System.Drawing.Size(156, 17);
			this.MeasurementDateTimeLabel.TabIndex = 3;
			this.MeasurementDateTimeLabel.Text = "Measurement date/time";
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(844, 59);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 9;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(844, 95);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 10;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(844, 130);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 11;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// MeasurementDateTimePicker
			// 
			this.MeasurementDateTimePicker.CustomFormat = "yy.MM.dd HH:mm:ss";
			this.MeasurementDateTimePicker.Location = new System.Drawing.Point(179, 15);
			this.MeasurementDateTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
			this.MeasurementDateTimePicker.Size = new System.Drawing.Size(265, 22);
			this.MeasurementDateTimePicker.TabIndex = 12;
			this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MeasOption,
            this.StratumId,
            this.ItemId,
            this.DateTime,
            this.Filename,
            this.Comment});
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point(16, 59);
			this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.ShowItemToolTips = true;
			this.listView1.Size = new System.Drawing.Size(804, 514);
			this.listView1.TabIndex = 13;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// MeasOption
			// 
			this.MeasOption.Text = "Meas Option";
			this.MeasOption.Width = 87;
			// 
			// StratumId
			// 
			this.StratumId.Text = "Stratum id";
			this.StratumId.Width = 68;
			// 
			// ItemId
			// 
			this.ItemId.Text = "Item id";
			this.ItemId.Width = 69;
			// 
			// DateTime
			// 
			this.DateTime.Text = "Date and Time";
			this.DateTime.Width = 104;
			// 
			// Filename
			// 
			this.Filename.Text = "Filename";
			this.Filename.Width = 110;
			// 
			// Comment
			// 
			this.Comment.Text = "Comment";
			this.Comment.Width = 170;
			// 
			// IDDAcquireDBMeas
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(960, 591);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.MeasurementDateTimePicker);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.MeasurementDateTimeLabel);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IDDAcquireDBMeas";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Measurement for Analysis";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MeasurementDateTimeLabel;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.DateTimePicker MeasurementDateTimePicker;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader StratumId;
        private System.Windows.Forms.ColumnHeader DateTime;
        private System.Windows.Forms.ColumnHeader Filename;
        private System.Windows.Forms.ColumnHeader MeasOption;
        private System.Windows.Forms.ColumnHeader Comment;
    }
}