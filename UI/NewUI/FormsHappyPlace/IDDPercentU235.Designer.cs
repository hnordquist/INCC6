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
            this.U235PercentLabel.Location = new System.Drawing.Point(8, 46);
            this.U235PercentLabel.Name = "U235PercentLabel";
            this.U235PercentLabel.Size = new System.Drawing.Size(44, 13);
            this.U235PercentLabel.TabIndex = 0;
            this.U235PercentLabel.Text = "% U235";
            // 
            // U235PercentTextBox
            // 
            this.U235PercentTextBox.Location = new System.Drawing.Point(58, 43);
            this.U235PercentTextBox.Name = "U235PercentTextBox";
            this.U235PercentTextBox.Size = new System.Drawing.Size(100, 20);
            this.U235PercentTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.U235PercentTextBox, "For passive measurements U238 is determined from the\r\ndoubles rate using the norm" +
        "al calibration curve, and then U235\r\n= U238 * e/(1 - e) , where e = fractional e" +
        "nrichment of the material. ");
            this.U235PercentTextBox.Leave += new System.EventHandler(this.U235PercentTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(194, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(194, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(194, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDPercentU235
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 106);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.U235PercentTextBox);
            this.Controls.Add(this.U235PercentLabel);
            this.Name = "IDDPercentU235";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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