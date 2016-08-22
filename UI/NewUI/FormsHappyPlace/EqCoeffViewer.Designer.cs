namespace NewUI
{
	partial class EqCoeffViewer
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
			this.CoeffView = new System.Windows.Forms.DataGridView();
			this.CurveType = new System.Windows.Forms.ComboBox();
			this.MtlAlg = new System.Windows.Forms.Label();
			this.OK = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.VarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.CoeffView)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// CoeffView
			// 
			this.CoeffView.AllowUserToAddRows = false;
			this.CoeffView.AllowUserToDeleteRows = false;
			this.CoeffView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.CoeffView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VarName,
            this.Value});
			this.CoeffView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CoeffView.Location = new System.Drawing.Point(0, 0);
			this.CoeffView.Name = "CoeffView";
			this.CoeffView.RowTemplate.Height = 24;
			this.CoeffView.Size = new System.Drawing.Size(280, 361);
			this.CoeffView.TabIndex = 0;
			this.CoeffView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CoeffView_CellEndEdit);
            this.CoeffView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.CoeffView_CellValidating);
			// 
			// CurveType
			// 
			this.CurveType.Enabled = false;
			this.CurveType.FormattingEnabled = true;
			this.CurveType.Location = new System.Drawing.Point(149, 4);
			this.CurveType.Name = "CurveType";
			this.CurveType.Size = new System.Drawing.Size(130, 24);
			this.CurveType.TabIndex = 1;
			// 
			// MtlAlg
			// 
			this.MtlAlg.AutoSize = true;
			this.MtlAlg.Location = new System.Drawing.Point(9, 7);
			this.MtlAlg.Name = "MtlAlg";
			this.MtlAlg.Size = new System.Drawing.Size(133, 17);
			this.MtlAlg.TabIndex = 2;
			this.MtlAlg.Text = "material && algorithm";
			// 
			// OK
			// 
			this.OK.Location = new System.Drawing.Point(123, 398);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(75, 32);
			this.OK.TabIndex = 3;
			this.OK.Text = "OK";
			this.OK.UseVisualStyleBackColor = true;
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// Cancel
			// 
			this.Cancel.Location = new System.Drawing.Point(211, 398);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 32);
			this.Cancel.TabIndex = 4;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// VarName
			// 
			this.VarName.HeaderText = "Var";
			this.VarName.Name = "VarName";
			// 
			// Value
			// 
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			this.Value.Width = 126;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.CoeffView);
			this.panel1.Location = new System.Drawing.Point(6, 31);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(280, 361);
			this.panel1.TabIndex = 5;
			// 
			// EqCoeffViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 434);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.Controls.Add(this.MtlAlg);
			this.Controls.Add(this.CurveType);
			this.Controls.Add(this.panel1);
			this.Name = "EqCoeffViewer";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Curve";
			((System.ComponentModel.ISupportInitialize)(this.CoeffView)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView CoeffView;
		private System.Windows.Forms.ComboBox CurveType;
		private System.Windows.Forms.Label MtlAlg;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.DataGridViewTextBoxColumn VarName;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		private System.Windows.Forms.Panel panel1;
	}
}