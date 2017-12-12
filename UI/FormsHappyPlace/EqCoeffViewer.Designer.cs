namespace UI
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
			this.components = new System.ComponentModel.Container();
			this.CurveType = new System.Windows.Forms.ComboBox();
			this.MtlAlg = new System.Windows.Forms.Label();
			this.OK = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.PointView = new System.Windows.Forms.DataGridView();
			this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CoeffView = new System.Windows.Forms.DataGridView();
			this.VarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UpperMassLimitLabel = new System.Windows.Forms.Label();
			this.LowerMassLimitLabel = new System.Windows.Forms.Label();
			this.UpperMassLimitTextBox = new System.Windows.Forms.TextBox();
			this.LowerMassLimitTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PointView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CoeffView)).BeginInit();
			this.SuspendLayout();
			// 
			// CurveType
			// 
			this.CurveType.Enabled = false;
			this.CurveType.FormattingEnabled = true;
			this.CurveType.Location = new System.Drawing.Point(296, 9);
			this.CurveType.Name = "CurveType";
			this.CurveType.Size = new System.Drawing.Size(155, 24);
			this.CurveType.TabIndex = 1;
			this.toolTip1.SetToolTip(this.CurveType, "The current calibration curve equation");
			// 
			// MtlAlg
			// 
			this.MtlAlg.AutoSize = true;
			this.MtlAlg.Location = new System.Drawing.Point(14, 13);
			this.MtlAlg.Name = "MtlAlg";
			this.MtlAlg.Size = new System.Drawing.Size(133, 17);
			this.MtlAlg.TabIndex = 2;
			this.MtlAlg.Text = "material && algorithm";
			this.toolTip1.SetToolTip(this.MtlAlg, "Current material type and Pu method");
			// 
			// OK
			// 
			this.OK.Location = new System.Drawing.Point(361, 497);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(75, 32);
			this.OK.TabIndex = 3;
			this.OK.Text = "OK";
			this.toolTip1.SetToolTip(this.OK, "Apply these coefficients, curve type, mass limits, and data points\r\nto the curren" +
        "t material and algorithm; save to the database.");
			this.OK.UseVisualStyleBackColor = true;
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// Cancel
			// 
			this.Cancel.Location = new System.Drawing.Point(449, 497);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 32);
			this.Cancel.TabIndex = 4;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.PointView);
			this.panel1.Controls.Add(this.CoeffView);
			this.panel1.Location = new System.Drawing.Point(5, 69);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(546, 361);
			this.panel1.TabIndex = 5;
			// 
			// PointView
			// 
			this.PointView.AllowUserToAddRows = false;
			this.PointView.AllowUserToDeleteRows = false;
			this.PointView.AllowUserToResizeRows = false;
			this.PointView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.PointView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
			this.PointView.Location = new System.Drawing.Point(251, 1);
			this.PointView.Name = "PointView";
			this.PointView.ReadOnly = true;
			this.PointView.RowHeadersVisible = false;
			this.PointView.RowTemplate.Height = 24;
			this.PointView.Size = new System.Drawing.Size(295, 361);
			this.PointView.TabIndex = 1;
			// 
			// Num
			// 
			this.Num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Num.HeaderText = "#";
			this.Num.Name = "Num";
			this.Num.ReadOnly = true;
			this.Num.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Num.Width = 22;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.HeaderText = "Decl Mass";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn4.Width = 120;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.HeaderText = "Doubles";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn5.Width = 120;
			// 
			// CoeffView
			// 
			this.CoeffView.AllowUserToAddRows = false;
			this.CoeffView.AllowUserToDeleteRows = false;
			this.CoeffView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.CoeffView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VarName,
            this.Value});
			this.CoeffView.Location = new System.Drawing.Point(0, 0);
			this.CoeffView.Name = "CoeffView";
			this.CoeffView.RowHeadersVisible = false;
			this.CoeffView.RowTemplate.Height = 24;
			this.CoeffView.Size = new System.Drawing.Size(251, 361);
			this.CoeffView.TabIndex = 0;
			this.CoeffView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CoeffView_CellContentClick);
			this.CoeffView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CoeffView_CellEndEdit);
			this.CoeffView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.CoeffView_CellValidating);
			// 
			// VarName
			// 
			this.VarName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.VarName.HeaderText = "Var";
			this.VarName.Name = "VarName";
			this.VarName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.VarName.Width = 36;
			// 
			// Value
			// 
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Value.Width = 130;
			// 
			// UpperMassLimitLabel
			// 
			this.UpperMassLimitLabel.AutoSize = true;
			this.UpperMassLimitLabel.Location = new System.Drawing.Point(266, 466);
			this.UpperMassLimitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.UpperMassLimitLabel.Name = "UpperMassLimitLabel";
			this.UpperMassLimitLabel.Size = new System.Drawing.Size(134, 17);
			this.UpperMassLimitLabel.TabIndex = 55;
			this.UpperMassLimitLabel.Text = "Upper mass limit (g)";
			// 
			// LowerMassLimitLabel
			// 
			this.LowerMassLimitLabel.AutoSize = true;
			this.LowerMassLimitLabel.Location = new System.Drawing.Point(266, 437);
			this.LowerMassLimitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.LowerMassLimitLabel.Name = "LowerMassLimitLabel";
			this.LowerMassLimitLabel.Size = new System.Drawing.Size(133, 17);
			this.LowerMassLimitLabel.TabIndex = 54;
			this.LowerMassLimitLabel.Text = "Lower mass limit (g)";
			// 
			// UpperMassLimitTextBox
			// 
			this.UpperMassLimitTextBox.Location = new System.Drawing.Point(407, 464);
			this.UpperMassLimitTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.UpperMassLimitTextBox.Name = "UpperMassLimitTextBox";
			this.UpperMassLimitTextBox.Size = new System.Drawing.Size(132, 22);
			this.UpperMassLimitTextBox.TabIndex = 53;
			this.toolTip1.SetToolTip(this.UpperMassLimitTextBox, "Calculated upper mass limit (maximum data point *= 1.1)");
			// 
			// LowerMassLimitTextBox
			// 
			this.LowerMassLimitTextBox.Location = new System.Drawing.Point(407, 435);
			this.LowerMassLimitTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.LowerMassLimitTextBox.Name = "LowerMassLimitTextBox";
			this.LowerMassLimitTextBox.Size = new System.Drawing.Size(132, 22);
			this.LowerMassLimitTextBox.TabIndex = 52;
			this.toolTip1.SetToolTip(this.LowerMassLimitTextBox, "Calculated lower mass limit (minimum data point *= 0.9)");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(52, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(147, 20);
			this.label1.TabIndex = 56;
			this.label1.Text = "Curve Coefficients";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(301, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(204, 20);
			this.label2.TabIndex = 57;
			this.label2.Text = "Measurement Data Points";
			// 
			// EqCoeffViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 534);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.UpperMassLimitLabel);
			this.Controls.Add(this.LowerMassLimitLabel);
			this.Controls.Add(this.UpperMassLimitTextBox);
			this.Controls.Add(this.LowerMassLimitTextBox);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.Controls.Add(this.MtlAlg);
			this.Controls.Add(this.CurveType);
			this.Controls.Add(this.panel1);
			this.Name = "EqCoeffViewer";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Curve Coefficients and Data Points";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PointView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CoeffView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox CurveType;
		private System.Windows.Forms.Label MtlAlg;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView CoeffView;
		private System.Windows.Forms.DataGridView PointView;
		private System.Windows.Forms.Label UpperMassLimitLabel;
		private System.Windows.Forms.Label LowerMassLimitLabel;
		private System.Windows.Forms.TextBox UpperMassLimitTextBox;
		private System.Windows.Forms.TextBox LowerMassLimitTextBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn Num;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn VarName;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}