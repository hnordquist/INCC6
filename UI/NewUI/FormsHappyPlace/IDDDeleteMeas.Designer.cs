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
            this.MeasType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Comment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MCountSel = new System.Windows.Forms.TextBox();
            this.MCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(421, 11);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(421, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(421, 68);
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
            this.MeasType,
            this.ItemId,
            this.StratumId,
            this.DT,
            this.Comment});
            this.MeasurementView.FullRowSelect = true;
            this.MeasurementView.Location = new System.Drawing.Point(12, 12);
            this.MeasurementView.Name = "MeasurementView";
            this.MeasurementView.Size = new System.Drawing.Size(404, 461);
            this.MeasurementView.TabIndex = 9;
            this.MeasurementView.UseCompatibleStateImageBehavior = false;
            this.MeasurementView.View = System.Windows.Forms.View.Details;
            this.MeasurementView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.MeasurementView_ColumnClick);
            this.MeasurementView.SelectedIndexChanged += new System.EventHandler(this.MeasurementView_SelectedIndexChanged);
            // 
            // MeasType
            // 
            this.MeasType.Text = "Meas. Type";
            this.MeasType.Width = 112;
            // 
            // ItemId
            // 
            this.ItemId.Text = "Item id";
            this.ItemId.Width = 74;
            // 
            // StratumId
            // 
            this.StratumId.Text = "Stratum id";
            this.StratumId.Width = 74;
            // 
            // DT
            // 
            this.DT.Text = "Date and Time";
            this.DT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DT.Width = 120;
            // 
            // Comment
            // 
            this.Comment.Text = "Comment";
            this.Comment.Width = 72;
            // 
            // MCountSel
            // 
            this.MCountSel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MCountSel.CausesValidation = false;
            this.MCountSel.Location = new System.Drawing.Point(421, 245);
            this.MCountSel.Name = "MCountSel";
            this.MCountSel.ReadOnly = true;
            this.MCountSel.Size = new System.Drawing.Size(87, 13);
            this.MCountSel.TabIndex = 12;
            // 
            // MCount
            // 
            this.MCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MCount.CausesValidation = false;
            this.MCount.Location = new System.Drawing.Point(422, 226);
            this.MCount.Name = "MCount";
            this.MCount.ReadOnly = true;
            this.MCount.Size = new System.Drawing.Size(87, 13);
            this.MCount.TabIndex = 13;
            // 
            // IDDDeleteMeas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 488);
            this.Controls.Add(this.MCount);
            this.Controls.Add(this.MCountSel);
            this.Controls.Add(this.MeasurementView);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDDeleteMeas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Measurements From Database";
            this.Shown += new System.EventHandler(this.IDDDeleteMeas_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        public System.Windows.Forms.ListView MeasurementView;
        private System.Windows.Forms.ColumnHeader MeasType;
        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader StratumId;
        private System.Windows.Forms.ColumnHeader DT;
		private System.Windows.Forms.ColumnHeader Comment;
		private System.Windows.Forms.TextBox MCountSel;
        private System.Windows.Forms.TextBox MCount;
    }
}