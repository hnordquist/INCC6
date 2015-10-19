namespace NewUI
{
    partial class IDDAnalysisMethodsConfig
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
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PassiveCalCurveCheckBox = new System.Windows.Forms.CheckBox();
            this.KnownAlphaCheckBox = new System.Windows.Forms.CheckBox();
            this.KnownMCheckBox = new System.Windows.Forms.CheckBox();
            this.PassiveMultiplicityCheckBox = new System.Windows.Forms.CheckBox();
            this.AddASourceCheckBox = new System.Windows.Forms.CheckBox();
            this.CuRatioCheckBox = new System.Windows.Forms.CheckBox();
            this.TruncatedMultCheckBox = new System.Windows.Forms.CheckBox();
            this.ActiveCalCurveCheckBox = new System.Windows.Forms.CheckBox();
            this.CollarCheckBox = new System.Windows.Forms.CheckBox();
            this.ActiveMultCheckBox = new System.Windows.Forms.CheckBox();
            this.ActivePassiveCheckBox = new System.Windows.Forms.CheckBox();
            this.PassiveLabel = new System.Windows.Forms.Label();
            this.ActiveLabel = new System.Windows.Forms.Label();
            this.SelectMethodsLabel = new System.Windows.Forms.Label();
            this.ProvisoLabel = new System.Windows.Forms.Label();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(121, 12);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.MaterialTypeComboBox.TabIndex = 0;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(372, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(372, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(372, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PassiveCalCurveCheckBox
            // 
            this.PassiveCalCurveCheckBox.AutoSize = true;
            this.PassiveCalCurveCheckBox.Location = new System.Drawing.Point(28, 152);
            this.PassiveCalCurveCheckBox.Name = "PassiveCalCurveCheckBox";
            this.PassiveCalCurveCheckBox.Size = new System.Drawing.Size(105, 17);
            this.PassiveCalCurveCheckBox.TabIndex = 4;
            this.PassiveCalCurveCheckBox.Text = "Calibration curve";
            this.PassiveCalCurveCheckBox.UseVisualStyleBackColor = true;
            this.PassiveCalCurveCheckBox.CheckedChanged += new System.EventHandler(this.PassiveCalCurveCheckBox_CheckedChanged);
            // 
            // KnownAlphaCheckBox
            // 
            this.KnownAlphaCheckBox.AutoSize = true;
            this.KnownAlphaCheckBox.Location = new System.Drawing.Point(28, 175);
            this.KnownAlphaCheckBox.Name = "KnownAlphaCheckBox";
            this.KnownAlphaCheckBox.Size = new System.Drawing.Size(88, 17);
            this.KnownAlphaCheckBox.TabIndex = 5;
            this.KnownAlphaCheckBox.Text = "Known alpha";
            this.KnownAlphaCheckBox.UseVisualStyleBackColor = true;
            this.KnownAlphaCheckBox.CheckedChanged += new System.EventHandler(this.KnownAlphaCheckBox_CheckedChanged);
            // 
            // KnownMCheckBox
            // 
            this.KnownMCheckBox.AutoSize = true;
            this.KnownMCheckBox.Location = new System.Drawing.Point(28, 198);
            this.KnownMCheckBox.Name = "KnownMCheckBox";
            this.KnownMCheckBox.Size = new System.Drawing.Size(71, 17);
            this.KnownMCheckBox.TabIndex = 6;
            this.KnownMCheckBox.Text = "Known M";
            this.KnownMCheckBox.UseVisualStyleBackColor = true;
            this.KnownMCheckBox.CheckedChanged += new System.EventHandler(this.KnownMCheckBox_CheckedChanged);
            // 
            // PassiveMultiplicityCheckBox
            // 
            this.PassiveMultiplicityCheckBox.AutoSize = true;
            this.PassiveMultiplicityCheckBox.Location = new System.Drawing.Point(28, 221);
            this.PassiveMultiplicityCheckBox.Name = "PassiveMultiplicityCheckBox";
            this.PassiveMultiplicityCheckBox.Size = new System.Drawing.Size(74, 17);
            this.PassiveMultiplicityCheckBox.TabIndex = 7;
            this.PassiveMultiplicityCheckBox.Text = "Multiplicity";
            this.PassiveMultiplicityCheckBox.UseVisualStyleBackColor = true;
            this.PassiveMultiplicityCheckBox.CheckedChanged += new System.EventHandler(this.PassiveMultiplicityCheckBox_CheckedChanged);
            // 
            // AddASourceCheckBox
            // 
            this.AddASourceCheckBox.AutoSize = true;
            this.AddASourceCheckBox.Location = new System.Drawing.Point(27, 244);
            this.AddASourceCheckBox.Name = "AddASourceCheckBox";
            this.AddASourceCheckBox.Size = new System.Drawing.Size(89, 17);
            this.AddASourceCheckBox.TabIndex = 8;
            this.AddASourceCheckBox.Text = "Add-a-source";
            this.AddASourceCheckBox.UseVisualStyleBackColor = true;
            this.AddASourceCheckBox.CheckedChanged += new System.EventHandler(this.AddASourceCheckBox_CheckedChanged);
            // 
            // CuRatioCheckBox
            // 
            this.CuRatioCheckBox.AutoSize = true;
            this.CuRatioCheckBox.Location = new System.Drawing.Point(28, 267);
            this.CuRatioCheckBox.Name = "CuRatioCheckBox";
            this.CuRatioCheckBox.Size = new System.Drawing.Size(81, 17);
            this.CuRatioCheckBox.TabIndex = 9;
            this.CuRatioCheckBox.Text = "Curium ratio";
            this.CuRatioCheckBox.UseVisualStyleBackColor = true;
            this.CuRatioCheckBox.CheckedChanged += new System.EventHandler(this.CuRatioCheckBox_CheckedChanged);
            // 
            // TruncatedMultCheckBox
            // 
            this.TruncatedMultCheckBox.AutoSize = true;
            this.TruncatedMultCheckBox.Location = new System.Drawing.Point(28, 290);
            this.TruncatedMultCheckBox.Name = "TruncatedMultCheckBox";
            this.TruncatedMultCheckBox.Size = new System.Drawing.Size(125, 17);
            this.TruncatedMultCheckBox.TabIndex = 10;
            this.TruncatedMultCheckBox.Text = "Truncated multiplicity";
            this.TruncatedMultCheckBox.UseVisualStyleBackColor = true;
            this.TruncatedMultCheckBox.CheckedChanged += new System.EventHandler(this.TruncatedMultCheckBox_CheckedChanged);
            // 
            // ActiveCalCurveCheckBox
            // 
            this.ActiveCalCurveCheckBox.AutoSize = true;
            this.ActiveCalCurveCheckBox.Location = new System.Drawing.Point(221, 152);
            this.ActiveCalCurveCheckBox.Name = "ActiveCalCurveCheckBox";
            this.ActiveCalCurveCheckBox.Size = new System.Drawing.Size(105, 17);
            this.ActiveCalCurveCheckBox.TabIndex = 11;
            this.ActiveCalCurveCheckBox.Text = "Calibration curve";
            this.ActiveCalCurveCheckBox.UseVisualStyleBackColor = true;
            this.ActiveCalCurveCheckBox.CheckedChanged += new System.EventHandler(this.ActiveCalCurveCheckBox_CheckedChanged);
            // 
            // CollarCheckBox
            // 
            this.CollarCheckBox.AutoSize = true;
            this.CollarCheckBox.Enabled = false;
            this.CollarCheckBox.Location = new System.Drawing.Point(221, 175);
            this.CollarCheckBox.Name = "CollarCheckBox";
            this.CollarCheckBox.Size = new System.Drawing.Size(52, 17);
            this.CollarCheckBox.TabIndex = 12;
            this.CollarCheckBox.Text = "Collar";
            this.CollarCheckBox.UseVisualStyleBackColor = true;
            this.CollarCheckBox.CheckedChanged += new System.EventHandler(this.CollarCheckBox_CheckedChanged);
            // 
            // ActiveMultCheckBox
            // 
            this.ActiveMultCheckBox.AutoSize = true;
            this.ActiveMultCheckBox.Location = new System.Drawing.Point(221, 198);
            this.ActiveMultCheckBox.Name = "ActiveMultCheckBox";
            this.ActiveMultCheckBox.Size = new System.Drawing.Size(74, 17);
            this.ActiveMultCheckBox.TabIndex = 13;
            this.ActiveMultCheckBox.Text = "Multiplicity";
            this.ActiveMultCheckBox.UseVisualStyleBackColor = true;
            this.ActiveMultCheckBox.CheckedChanged += new System.EventHandler(this.ActiveMultCheckBox_CheckedChanged);
            // 
            // ActivePassiveCheckBox
            // 
            this.ActivePassiveCheckBox.AutoSize = true;
            this.ActivePassiveCheckBox.Location = new System.Drawing.Point(221, 221);
            this.ActivePassiveCheckBox.Name = "ActivePassiveCheckBox";
            this.ActivePassiveCheckBox.Size = new System.Drawing.Size(97, 17);
            this.ActivePassiveCheckBox.TabIndex = 14;
            this.ActivePassiveCheckBox.Text = "Active/passive";
            this.ActivePassiveCheckBox.UseVisualStyleBackColor = true;
            this.ActivePassiveCheckBox.CheckedChanged += new System.EventHandler(this.ActivePassiveCheckBox_CheckedChanged);
            // 
            // PassiveLabel
            // 
            this.PassiveLabel.AutoSize = true;
            this.PassiveLabel.Location = new System.Drawing.Point(46, 127);
            this.PassiveLabel.Name = "PassiveLabel";
            this.PassiveLabel.Size = new System.Drawing.Size(44, 13);
            this.PassiveLabel.TabIndex = 15;
            this.PassiveLabel.Text = "Passive";
            // 
            // ActiveLabel
            // 
            this.ActiveLabel.AutoSize = true;
            this.ActiveLabel.Location = new System.Drawing.Point(239, 127);
            this.ActiveLabel.Name = "ActiveLabel";
            this.ActiveLabel.Size = new System.Drawing.Size(37, 13);
            this.ActiveLabel.TabIndex = 16;
            this.ActiveLabel.Text = "Active";
            // 
            // SelectMethodsLabel
            // 
            this.SelectMethodsLabel.AutoSize = true;
            this.SelectMethodsLabel.Location = new System.Drawing.Point(26, 57);
            this.SelectMethodsLabel.Name = "SelectMethodsLabel";
            this.SelectMethodsLabel.Size = new System.Drawing.Size(281, 13);
            this.SelectMethodsLabel.TabIndex = 17;
            this.SelectMethodsLabel.Text = "Select analysis methods valid for the chosen material type.";
            // 
            // ProvisoLabel
            // 
            this.ProvisoLabel.AutoSize = true;
            this.ProvisoLabel.Location = new System.Drawing.Point(26, 80);
            this.ProvisoLabel.Name = "ProvisoLabel";
            this.ProvisoLabel.Size = new System.Drawing.Size(310, 13);
            this.ProvisoLabel.TabIndex = 18;
            this.ProvisoLabel.Text = "Configuration will only be done for the last material type selected.";
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(48, 15);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 19;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // IDDAnalysisMethodsConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 325);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.ProvisoLabel);
            this.Controls.Add(this.SelectMethodsLabel);
            this.Controls.Add(this.ActiveLabel);
            this.Controls.Add(this.PassiveLabel);
            this.Controls.Add(this.ActivePassiveCheckBox);
            this.Controls.Add(this.ActiveMultCheckBox);
            this.Controls.Add(this.CollarCheckBox);
            this.Controls.Add(this.ActiveCalCurveCheckBox);
            this.Controls.Add(this.TruncatedMultCheckBox);
            this.Controls.Add(this.CuRatioCheckBox);
            this.Controls.Add(this.AddASourceCheckBox);
            this.Controls.Add(this.PassiveMultiplicityCheckBox);
            this.Controls.Add(this.KnownMCheckBox);
            this.Controls.Add(this.KnownAlphaCheckBox);
            this.Controls.Add(this.PassiveCalCurveCheckBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Name = "IDDAnalysisMethodsConfig";
            this.Text = "Analysis Methods Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.CheckBox PassiveCalCurveCheckBox;
        private System.Windows.Forms.CheckBox KnownAlphaCheckBox;
        private System.Windows.Forms.CheckBox KnownMCheckBox;
        private System.Windows.Forms.CheckBox PassiveMultiplicityCheckBox;
        private System.Windows.Forms.CheckBox AddASourceCheckBox;
        private System.Windows.Forms.CheckBox CuRatioCheckBox;
        private System.Windows.Forms.CheckBox TruncatedMultCheckBox;
        private System.Windows.Forms.CheckBox ActiveCalCurveCheckBox;
        private System.Windows.Forms.CheckBox CollarCheckBox;
        private System.Windows.Forms.CheckBox ActiveMultCheckBox;
        private System.Windows.Forms.CheckBox ActivePassiveCheckBox;
        private System.Windows.Forms.Label PassiveLabel;
        private System.Windows.Forms.Label ActiveLabel;
        private System.Windows.Forms.Label SelectMethodsLabel;
        private System.Windows.Forms.Label ProvisoLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
    }
}