namespace UI
{
    partial class TransferList
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
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;

        private void InitializeComponent()
        {
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.Det = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Comment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MCount = new System.Windows.Forms.TextBox();
			this.MCountSel = new System.Windows.Forms.TextBox();
			this.Material = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(695, 15);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 6;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(695, 50);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 7;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(695, 86);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 8;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Det,
            this.ItemId,
            this.StratumId,
            this.DT,
            this.FileName,
            this.Comment,
            this.Material});
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point(4, 15);
			this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.listView1.Name = "listView1";
			this.listView1.ShowItemToolTips = true;
			this.listView1.Size = new System.Drawing.Size(683, 448);
			this.listView1.TabIndex = 9;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// Det
			// 
			this.Det.Text = "Detector";
			this.Det.Width = 90;
			// 
			// ItemId
			// 
			this.ItemId.DisplayIndex = 2;
			this.ItemId.Text = "Item id";
			this.ItemId.Width = 94;
			// 
			// StratumId
			// 
			this.StratumId.DisplayIndex = 1;
			this.StratumId.Text = "Stratum id";
			this.StratumId.Width = 74;
			// 
			// DT
			// 
			this.DT.Text = "Date and Time";
			this.DT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.DT.Width = 120;
			// 
			// FileName
			// 
			this.FileName.Text = "File Name";
			this.FileName.Width = 120;
			// 
			// Comment
			// 
			this.Comment.Text = "Comment";
			this.Comment.Width = 72;
			// 
			// MCount
			// 
			this.MCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MCount.CausesValidation = false;
			this.MCount.Location = new System.Drawing.Point(695, 290);
			this.MCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MCount.Name = "MCount";
			this.MCount.ReadOnly = true;
			this.MCount.Size = new System.Drawing.Size(116, 15);
			this.MCount.TabIndex = 10;
			// 
			// MCountSel
			// 
			this.MCountSel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MCountSel.CausesValidation = false;
			this.MCountSel.Location = new System.Drawing.Point(695, 322);
			this.MCountSel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MCountSel.Name = "MCountSel";
			this.MCountSel.ReadOnly = true;
			this.MCountSel.Size = new System.Drawing.Size(116, 15);
			this.MCountSel.TabIndex = 11;
			// 
			// Material
			// 
			this.Material.Text = "Material";
			// 
			// TransferList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(804, 468);
			this.Controls.Add(this.MCountSel);
			this.Controls.Add(this.MCount);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "TransferList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Measurement Selection";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader StratumId;
        private System.Windows.Forms.ColumnHeader DT;
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		#endregion
        private System.Windows.Forms.ColumnHeader Det;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.TextBox MCount;
        private System.Windows.Forms.TextBox MCountSel;
		private System.Windows.Forms.ColumnHeader Comment;
		private System.Windows.Forms.ColumnHeader Material;
	}
}