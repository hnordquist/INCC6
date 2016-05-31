namespace NewUI
{
    partial class IDDPercentU235
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
			this.U235PercentLabel = new System.Windows.Forms.Label();
			this.U235PercentTextBox = new System.Windows.Forms.TextBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// U235PercentLabel
			// 
			this.U235PercentLabel.AutoSize = true;
			this.U235PercentLabel.Location = new System.Drawing.Point(11, 57);
			this.U235PercentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.U235PercentLabel.Name = "U235PercentLabel";
			this.U235PercentLabel.Size = new System.Drawing.Size(58, 17);
			this.U235PercentLabel.TabIndex = 0;
			this.U235PercentLabel.Text = "% U235";
			// 
			// U235PercentTextBox
			// 
			this.U235PercentTextBox.Location = new System.Drawing.Point(77, 53);
			this.U235PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.U235PercentTextBox.Name = "U235PercentTextBox";
			this.U235PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.U235PercentTextBox.TabIndex = 1;
			this.toolTip1.SetToolTip(this.U235PercentTextBox, "For passive measurements U238 is determined from the\r\ndoubles rate using the norm" +
        "al calibration curve, and then U235\r\n= U238 * e/(1 - e) , where e = fractional e" +
        "nrichment of the material. ");
			this.U235PercentTextBox.Leave += new System.EventHandler(this.U235PercentTextBox_Leave);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(259, 15);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 2;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(259, 50);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 3;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(259, 86);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 4;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// IDDPercentU235
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(377, 130);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.U235PercentTextBox);
			this.Controls.Add(this.U235PercentLabel);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IDDPercentU235";
			this.Text = "Enter U235 enrichment";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label U235PercentLabel;
        private System.Windows.Forms.TextBox U235PercentTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}