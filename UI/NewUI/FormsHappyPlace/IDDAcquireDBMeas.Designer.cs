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
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // MeasurementDateTimeLabel
            // 
            this.MeasurementDateTimeLabel.AutoSize = true;
            this.MeasurementDateTimeLabel.Location = new System.Drawing.Point(10, 16);
            this.MeasurementDateTimeLabel.Name = "MeasurementDateTimeLabel";
            this.MeasurementDateTimeLabel.Size = new System.Drawing.Size(119, 13);
            this.MeasurementDateTimeLabel.TabIndex = 3;
            this.MeasurementDateTimeLabel.Text = "Measurement date/time";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(633, 48);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 9;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(633, 77);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 10;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(633, 106);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 11;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // MeasurementDateTimePicker
            // 
            this.MeasurementDateTimePicker.CustomFormat = "yy.MM.dd HH:mm:ss";
            this.MeasurementDateTimePicker.Location = new System.Drawing.Point(134, 12);
            this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
            this.MeasurementDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.MeasurementDateTimePicker.TabIndex = 12;
            this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemId,
            this.StratumId,
            this.Date,
            this.Time});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(12, 48);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(604, 418);
            this.listView1.TabIndex = 13;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // ItemId
            // 
            this.ItemId.Text = "Item id";
            this.ItemId.Width = 150;
            // 
            // StratumId
            // 
            this.StratumId.Text = "Stratum id";
            this.StratumId.Width = 150;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.Width = 150;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 150;
            // 
            // IDDAcquireDBMeas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 480);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.MeasurementDateTimePicker);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MeasurementDateTimeLabel);
            this.Name = "IDDAcquireDBMeas";
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
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Time;
    }
}