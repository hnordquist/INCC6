namespace NewUI
{
	partial class IsotopicsList
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
            this.components = new System.ComponentModel.Container();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.IsoId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PuDT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AmDT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MCount = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.MCountSel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(225, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(225, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(225, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IsoId,
            this.PuDT,
            this.AmDT,
            this.SrcCode});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(3, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(217, 365);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // IsoId
            // 
            this.IsoId.Text = "Isotopics Id";
            this.IsoId.Width = 90;
            // 
            // PuDT
            // 
            this.PuDT.Text = "Pu Date";
            this.PuDT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PuDT.Width = 70;
            // 
            // AmDT
            // 
            this.AmDT.Text = "Am Date";
            this.AmDT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AmDT.Width = 70;
            // 
            // SrcCode
            // 
            this.SrcCode.Text = "Source Code";
            // 
            // MCount
            // 
            this.MCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MCount.CausesValidation = false;
            this.MCount.Location = new System.Drawing.Point(225, 256);
            this.MCount.Name = "MCount";
            this.MCount.ReadOnly = true;
            this.MCount.Size = new System.Drawing.Size(87, 13);
            this.MCount.TabIndex = 10;
            this.toolTip1.SetToolTip(this.MCount, "Measurements available for use");
            // 
            // MCountSel
            // 
            this.MCountSel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MCountSel.CausesValidation = false;
            this.MCountSel.Location = new System.Drawing.Point(225, 282);
            this.MCountSel.Name = "MCountSel";
            this.MCountSel.ReadOnly = true;
            this.MCountSel.Size = new System.Drawing.Size(87, 13);
            this.MCountSel.TabIndex = 11;
            this.toolTip1.SetToolTip(this.MCountSel, "Measurements available for use");
            // 
            // IsotopicsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 380);
            this.Controls.Add(this.MCountSel);
            this.Controls.Add(this.MCount);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IsotopicsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Isotopics Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ColumnHeader PuDT;
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		#endregion
		private System.Windows.Forms.ColumnHeader IsoId;
		private System.Windows.Forms.TextBox MCount;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TextBox MCountSel;
		private System.Windows.Forms.ColumnHeader AmDT;
		private System.Windows.Forms.ColumnHeader SrcCode;
	}
}