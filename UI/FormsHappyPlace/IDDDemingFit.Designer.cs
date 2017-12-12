namespace UI
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
            this.SelectTypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.SelectTypeGroupBox.Name = "SelectTypeGroupBox";
            this.SelectTypeGroupBox.Size = new System.Drawing.Size(304, 95);
            this.SelectTypeGroupBox.TabIndex = 0;
            this.SelectTypeGroupBox.TabStop = false;
            this.SelectTypeGroupBox.Text = "Select type of data to fit";
            // 
            // HoldupDataRadioButton
            // 
            this.HoldupDataRadioButton.AutoSize = true;
            this.HoldupDataRadioButton.Location = new System.Drawing.Point(21, 56);
            this.HoldupDataRadioButton.Name = "HoldupDataRadioButton";
            this.HoldupDataRadioButton.Size = new System.Drawing.Size(95, 17);
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
            this.VerificationCalDataRadioButton.Location = new System.Drawing.Point(21, 29);
            this.VerificationCalDataRadioButton.Name = "VerificationCalDataRadioButton";
            this.VerificationCalDataRadioButton.Size = new System.Drawing.Size(165, 17);
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
            this.MaterialTypeLabel.Location = new System.Drawing.Point(30, 129);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 1;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // AnalysisMethodLabel
            // 
            this.AnalysisMethodLabel.AutoSize = true;
            this.AnalysisMethodLabel.Location = new System.Drawing.Point(14, 168);
            this.AnalysisMethodLabel.Name = "AnalysisMethodLabel";
            this.AnalysisMethodLabel.Size = new System.Drawing.Size(83, 13);
            this.AnalysisMethodLabel.TabIndex = 2;
            this.AnalysisMethodLabel.Text = "Analysis method";
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(103, 126);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(213, 21);
            this.MaterialTypeComboBox.TabIndex = 3;
            this.toolTip1.SetToolTip(this.MaterialTypeComboBox, "Calibration measurement data sets for the selected material type will\r\nbe exporte" +
        "d for use in an external curve-fitting application");
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // AnalysisMethodComboBox
            // 
            this.AnalysisMethodComboBox.FormattingEnabled = true;
            this.AnalysisMethodComboBox.Location = new System.Drawing.Point(103, 165);
            this.AnalysisMethodComboBox.Name = "AnalysisMethodComboBox";
            this.AnalysisMethodComboBox.Size = new System.Drawing.Size(213, 21);
            this.AnalysisMethodComboBox.TabIndex = 4;
            this.toolTip1.SetToolTip(this.AnalysisMethodComboBox, "Select the analysis method for which calibration coefficients, variances\r\nand cov" +
        "ariances are to be calculated using an external curve-fitting\r\napplication.");
            this.AnalysisMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.AnalysisMethodComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(334, 17);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(334, 46);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(334, 75);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 7;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDDemingFit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 201);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.AnalysisMethodComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.AnalysisMethodLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.SelectTypeGroupBox);
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