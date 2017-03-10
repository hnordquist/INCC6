namespace NewUI
{
    partial class IDDDualEnergyMult
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
            this.InnerTextBox = new System.Windows.Forms.TextBox();
            this.OuterTextBox = new System.Windows.Forms.TextBox();
            this.InnerLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
			this.GridView = new System.Windows.Forms.DataGridView();
			this.NeutronEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DetectorEfficiency = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InnerOuterRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RelativeFissionProbability = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // InnerTextBox
            // 
			this.InnerTextBox.Location = new System.Drawing.Point(156, 21);
			this.InnerTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.InnerTextBox.Name = "InnerTextBox";
			this.InnerTextBox.Size = new System.Drawing.Size(132, 22);
            this.InnerTextBox.TabIndex = 0;
            // 
            // OuterTextBox
            // 
			this.OuterTextBox.Location = new System.Drawing.Point(499, 21);
			this.OuterTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.OuterTextBox.Name = "OuterTextBox";
			this.OuterTextBox.Size = new System.Drawing.Size(132, 22);
            this.OuterTextBox.TabIndex = 1;
            // 
            // InnerLabel
            // 
            this.InnerLabel.AutoSize = true;
			this.InnerLabel.Location = new System.Drawing.Point(16, 25);
			this.InnerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InnerLabel.Name = "InnerLabel";
			this.InnerLabel.Size = new System.Drawing.Size(131, 17);
            this.InnerLabel.TabIndex = 2;
            this.InnerLabel.Text = "Inner ring efficiency";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(356, 25);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(135, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Outer ring efficiency";
            // 
            // PrintBtn
            // 
			this.PrintBtn.Location = new System.Drawing.Point(19, 488);
			this.PrintBtn.Margin = new System.Windows.Forms.Padding(4);
            this.PrintBtn.Name = "PrintBtn";
			this.PrintBtn.Size = new System.Drawing.Size(143, 28);
            this.PrintBtn.TabIndex = 4;
            this.PrintBtn.Text = "Print calibration";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
			this.OKBtn.Location = new System.Drawing.Point(315, 488);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4);
            this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
			this.CancelBtn.Location = new System.Drawing.Point(423, 488);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
            this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
			this.HelpBtn.Location = new System.Drawing.Point(531, 488);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
            this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
			// GridView
            // 
			this.GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NeutronEnergy,
            this.DetectorEfficiency,
            this.InnerOuterRatio,
            this.RelativeFissionProbability});
			this.GridView.Location = new System.Drawing.Point(19, 81);
			this.GridView.Name = "GridView";
			this.GridView.RowTemplate.Height = 24;
			this.GridView.Size = new System.Drawing.Size(640, 400);
			this.GridView.TabIndex = 9;
			this.GridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.GridView_CellValidating);
			this.GridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellEndEdit);
			// 
			// NeutronEnergy
			// 
			this.NeutronEnergy.HeaderText = "Neutron Energy (Mev)";
			this.NeutronEnergy.Name = "NeutronEnergy";
			// 
			// DetectorEfficiency
			// 
			this.DetectorEfficiency.HeaderText = "Detector Efficiency (fraction)";
			this.DetectorEfficiency.Name = "DetectorEfficiency";
			// 
			// InnerOuterRatio
			// 
			this.InnerOuterRatio.HeaderText = "Inner/Outer Ring Ratio";
			this.InnerOuterRatio.Name = "InnerOuterRatio";
			// 
			// RelativeFissionProbability
			// 
			this.RelativeFissionProbability.HeaderText = "Relative Fission Probability";
			this.RelativeFissionProbability.Name = "RelativeFissionProbability";
            // 
            // IDDDualEnergyMult
            // 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(664, 526);
			this.Controls.Add(this.GridView);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InnerLabel);
            this.Controls.Add(this.OuterTextBox);
            this.Controls.Add(this.InnerTextBox);
			this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IDDDualEnergyMult";
            this.Text = "Dual Energy Multiplicity";
			((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InnerTextBox;
        private System.Windows.Forms.TextBox OuterTextBox;
        private System.Windows.Forms.Label InnerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
		private System.Windows.Forms.DataGridView GridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn NeutronEnergy;
		private System.Windows.Forms.DataGridViewTextBoxColumn DetectorEfficiency;
		private System.Windows.Forms.DataGridViewTextBoxColumn InnerOuterRatio;
		private System.Windows.Forms.DataGridViewTextBoxColumn RelativeFissionProbability;
    }
}