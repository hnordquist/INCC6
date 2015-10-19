namespace NewUI
{
    partial class IDDDeleteMeas
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
            this.MeasurementView = new System.Windows.Forms.ListView();
            this.DetID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MeasType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(752, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(752, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(752, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // MeasurementView
            // 
            this.MeasurementView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DetID,
            this.MeasType,
            this.ItemId,
            this.StratumId,
            this.Date,
            this.Time});
            this.MeasurementView.FullRowSelect = true;
            this.MeasurementView.Location = new System.Drawing.Point(12, 12);
            this.MeasurementView.Name = "MeasurementView";
            this.MeasurementView.Size = new System.Drawing.Size(722, 461);
            this.MeasurementView.TabIndex = 9;
            this.MeasurementView.UseCompatibleStateImageBehavior = false;
            this.MeasurementView.View = System.Windows.Forms.View.Details;
            // 
            // DetID
            // 
            this.DetID.Text = "Detector ID";
            this.DetID.Width = 102;
            // 
            // MeasType
            // 
            this.MeasType.Text = "Measurement Type";
            this.MeasType.Width = 135;
            // 
            // ItemId
            // 
            this.ItemId.Text = "Item id";
            this.ItemId.Width = 120;
            // 
            // StratumId
            // 
            this.StratumId.Text = "Stratum id";
            this.StratumId.Width = 115;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.Width = 126;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 118;
            // 
            // IDDDeleteMeas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 488);
            this.Controls.Add(this.MeasurementView);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDDeleteMeas";
            this.Text = "Delete Measurements From Database";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView MeasurementView;
        private System.Windows.Forms.ColumnHeader MeasType;
        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader StratumId;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader DetID;
    }
}