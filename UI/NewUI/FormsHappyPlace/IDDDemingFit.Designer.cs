namespace NewUI
{
    partial class IDDDemingFit
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
			this.SelectTypeGroupBox = new System.Windows.Forms.GroupBox();
			this.HoldupDataRadioButton = new System.Windows.Forms.RadioButton();
			this.VerificationCalDataRadioButton = new System.Windows.Forms.RadioButton();
			this.MaterialTypeLabel = new System.Windows.Forms.Label();
			this.AnalysisMethodLabel = new System.Windows.Forms.Label();
			this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
			this.AnalysisMethodComboBox = new System.Windows.Forms.ComboBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SelectTypeGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// SelectTypeGroupBox
			// 
			this.SelectTypeGroupBox.Controls.Add(this.HoldupDataRadioButton);
			this.SelectTypeGroupBox.Controls.Add(this.VerificationCalDataRadioButton);
			this.SelectTypeGroupBox.Location = new System.Drawing.Point(16, 15);
			this.SelectTypeGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SelectTypeGroupBox.Name = "SelectTypeGroupBox";
			this.SelectTypeGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SelectTypeGroupBox.Size = new System.Drawing.Size(405, 117);
			this.SelectTypeGroupBox.TabIndex = 0;
			this.SelectTypeGroupBox.TabStop = false;
			this.SelectTypeGroupBox.Text = "Select type of data to fit";
			// 
			// HoldupDataRadioButton
			// 
			this.HoldupDataRadioButton.AutoSize = true;
			this.HoldupDataRadioButton.Location = new System.Drawing.Point(28, 69);
			this.HoldupDataRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HoldupDataRadioButton.Name = "HoldupDataRadioButton";
			this.HoldupDataRadioButton.Size = new System.Drawing.Size(123, 21);
			this.HoldupDataRadioButton.TabIndex = 1;
			this.HoldupDataRadioButton.TabStop = true;
			this.HoldupDataRadioButton.Text = "Fit holdup data";
			this.toolTip1.SetToolTip(this.HoldupDataRadioButton, "Click here if you want to fit holdup measurement\r\ndata to get passive calibration" +
        " curve or known alpha\r\ncalibration parameters");
			this.HoldupDataRadioButton.UseVisualStyleBackColor = true;
			// 
			// VerificationCalDataRadioButton
			// 
			this.VerificationCalDataRadioButton.AutoSize = true;
			this.VerificationCalDataRadioButton.Location = new System.Drawing.Point(28, 36);
			this.VerificationCalDataRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.VerificationCalDataRadioButton.Name = "VerificationCalDataRadioButton";
			this.VerificationCalDataRadioButton.Size = new System.Drawing.Size(217, 21);
			this.VerificationCalDataRadioButton.TabIndex = 0;
			this.VerificationCalDataRadioButton.TabStop = true;
			this.VerificationCalDataRadioButton.Text = "Fit verification calibration data";
			this.toolTip1.SetToolTip(this.VerificationCalDataRadioButton, "Click here if you want to fit calibration measurement\r\ndata to get calibration pa" +
        "rameters");
			this.VerificationCalDataRadioButton.UseVisualStyleBackColor = true;
			this.VerificationCalDataRadioButton.CheckedChanged += new System.EventHandler(this.VerificationCalDataRadioButton_CheckedChanged);
			// 
			// MaterialTypeLabel
			// 
			this.MaterialTypeLabel.AutoSize = true;
			this.MaterialTypeLabel.Location = new System.Drawing.Point(40, 159);
			this.MaterialTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MaterialTypeLabel.Name = "MaterialTypeLabel";
			this.MaterialTypeLabel.Size = new System.Drawing.Size(89, 17);
			this.MaterialTypeLabel.TabIndex = 1;
			this.MaterialTypeLabel.Text = "Material type";
			// 
			// AnalysisMethodLabel
			// 
			this.AnalysisMethodLabel.AutoSize = true;
			this.AnalysisMethodLabel.Location = new System.Drawing.Point(19, 207);
			this.AnalysisMethodLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.AnalysisMethodLabel.Name = "AnalysisMethodLabel";
			this.AnalysisMethodLabel.Size = new System.Drawing.Size(111, 17);
			this.AnalysisMethodLabel.TabIndex = 2;
			this.AnalysisMethodLabel.Text = "Analysis method";
			// 
			// MaterialTypeComboBox
			// 
			this.MaterialTypeComboBox.FormattingEnabled = true;
			this.MaterialTypeComboBox.Location = new System.Drawing.Point(137, 155);
			this.MaterialTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
			this.MaterialTypeComboBox.Size = new System.Drawing.Size(283, 24);
			this.MaterialTypeComboBox.TabIndex = 3;
			this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
			// 
			// AnalysisMethodComboBox
			// 
			this.AnalysisMethodComboBox.FormattingEnabled = true;
			this.AnalysisMethodComboBox.Location = new System.Drawing.Point(137, 203);
			this.AnalysisMethodComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.AnalysisMethodComboBox.Name = "AnalysisMethodComboBox";
			this.AnalysisMethodComboBox.Size = new System.Drawing.Size(283, 24);
			this.AnalysisMethodComboBox.TabIndex = 4;
			this.AnalysisMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.AnalysisMethodComboBox_SelectedIndexChanged);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(445, 21);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 5;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(445, 57);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 6;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(445, 92);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 7;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// IDDDemingFit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 247);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.AnalysisMethodComboBox);
			this.Controls.Add(this.MaterialTypeComboBox);
			this.Controls.Add(this.AnalysisMethodLabel);
			this.Controls.Add(this.MaterialTypeLabel);
			this.Controls.Add(this.SelectTypeGroupBox);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IDDDemingFit";
			this.Text = "Deming Curve Fitting";
			this.SelectTypeGroupBox.ResumeLayout(false);
			this.SelectTypeGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox SelectTypeGroupBox;
        private System.Windows.Forms.RadioButton HoldupDataRadioButton;
        private System.Windows.Forms.RadioButton VerificationCalDataRadioButton;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label AnalysisMethodLabel;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox AnalysisMethodComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}