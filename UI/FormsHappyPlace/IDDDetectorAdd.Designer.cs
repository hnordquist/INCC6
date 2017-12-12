namespace UI
{
    partial class IDDDetectorAdd
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
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CurrentDetectorsComboBox = new System.Windows.Forms.ComboBox();
            this.DetectorIdTextBox = new System.Windows.Forms.TextBox();
            this.DetectorTypeTextBox = new System.Windows.Forms.TextBox();
            this.ElectronicsIdTextBox = new System.Windows.Forms.TextBox();
            this.InitializationHelpLabel = new System.Windows.Forms.Label();
            this.CurrentDetectorsLabel = new System.Windows.Forms.Label();
            this.DetectorIdLabel = new System.Windows.Forms.Label();
            this.DetectorTypeLabel = new System.Windows.Forms.Label();
            this.ElectronicsIdLabel = new System.Windows.Forms.Label();
            this.LM = new System.Windows.Forms.RadioButton();
            this.SR = new System.Windows.Forms.RadioButton();
            this.LMTypes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(258, 12);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(258, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(258, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // CurrentDetectorsComboBox
            // 
            this.CurrentDetectorsComboBox.FormattingEnabled = true;
            this.CurrentDetectorsComboBox.Location = new System.Drawing.Point(96, 47);
            this.CurrentDetectorsComboBox.Name = "CurrentDetectorsComboBox";
            this.CurrentDetectorsComboBox.Size = new System.Drawing.Size(129, 21);
            this.CurrentDetectorsComboBox.TabIndex = 3;
            this.CurrentDetectorsComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentDetectorsComboBox_SelectedIndexChanged);
            // 
            // DetectorIdTextBox
            // 
            this.DetectorIdTextBox.Location = new System.Drawing.Point(96, 92);
            this.DetectorIdTextBox.Name = "DetectorIdTextBox";
            this.DetectorIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.DetectorIdTextBox.TabIndex = 4;
            this.DetectorIdTextBox.Leave += new System.EventHandler(this.DetectorIdTextBox_Leave);
            // 
            // DetectorTypeTextBox
            // 
            this.DetectorTypeTextBox.Location = new System.Drawing.Point(96, 117);
            this.DetectorTypeTextBox.Name = "DetectorTypeTextBox";
            this.DetectorTypeTextBox.Size = new System.Drawing.Size(100, 20);
            this.DetectorTypeTextBox.TabIndex = 5;
            this.DetectorTypeTextBox.Leave += new System.EventHandler(this.DetectorTypeTextBox_Leave);
            // 
            // ElectronicsIdTextBox
            // 
            this.ElectronicsIdTextBox.Location = new System.Drawing.Point(96, 141);
            this.ElectronicsIdTextBox.Name = "ElectronicsIdTextBox";
            this.ElectronicsIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.ElectronicsIdTextBox.TabIndex = 6;
            this.ElectronicsIdTextBox.Leave += new System.EventHandler(this.ElectronicsIdTextBox_Leave);
            // 
            // InitializationHelpLabel
            // 
            this.InitializationHelpLabel.AutoSize = true;
            this.InitializationHelpLabel.Location = new System.Drawing.Point(12, 3);
            this.InitializationHelpLabel.MaximumSize = new System.Drawing.Size(215, 0);
            this.InitializationHelpLabel.Name = "InitializationHelpLabel";
            this.InitializationHelpLabel.Size = new System.Drawing.Size(213, 26);
            this.InitializationHelpLabel.TabIndex = 7;
            this.InitializationHelpLabel.Text = "The detector you select here will be used to initialize parameters for the new de" +
    "tector.";
            // 
            // CurrentDetectorsLabel
            // 
            this.CurrentDetectorsLabel.AutoSize = true;
            this.CurrentDetectorsLabel.Location = new System.Drawing.Point(2, 50);
            this.CurrentDetectorsLabel.Name = "CurrentDetectorsLabel";
            this.CurrentDetectorsLabel.Size = new System.Drawing.Size(88, 13);
            this.CurrentDetectorsLabel.TabIndex = 8;
            this.CurrentDetectorsLabel.Text = "Current detectors";
            // 
            // DetectorIdLabel
            // 
            this.DetectorIdLabel.AutoSize = true;
            this.DetectorIdLabel.Location = new System.Drawing.Point(29, 95);
            this.DetectorIdLabel.Name = "DetectorIdLabel";
            this.DetectorIdLabel.Size = new System.Drawing.Size(59, 13);
            this.DetectorIdLabel.TabIndex = 9;
            this.DetectorIdLabel.Text = "Detector id";
            // 
            // DetectorTypeLabel
            // 
            this.DetectorTypeLabel.AutoSize = true;
            this.DetectorTypeLabel.Location = new System.Drawing.Point(17, 120);
            this.DetectorTypeLabel.Name = "DetectorTypeLabel";
            this.DetectorTypeLabel.Size = new System.Drawing.Size(71, 13);
            this.DetectorTypeLabel.TabIndex = 10;
            this.DetectorTypeLabel.Text = "Detector type";
            // 
            // ElectronicsIdLabel
            // 
            this.ElectronicsIdLabel.AutoSize = true;
            this.ElectronicsIdLabel.Location = new System.Drawing.Point(18, 144);
            this.ElectronicsIdLabel.Name = "ElectronicsIdLabel";
            this.ElectronicsIdLabel.Size = new System.Drawing.Size(70, 13);
            this.ElectronicsIdLabel.TabIndex = 11;
            this.ElectronicsIdLabel.Text = "Electronics id";
            // 
            // LM
            // 
            this.LM.AutoSize = true;
            this.LM.Location = new System.Drawing.Point(296, 119);
            this.LM.Name = "LM";
            this.LM.Size = new System.Drawing.Size(71, 17);
            this.LM.TabIndex = 12;
            this.LM.TabStop = true;
            this.LM.Text = "List Mode";
            this.LM.UseVisualStyleBackColor = true;
            this.LM.CheckedChanged += new System.EventHandler(this.LMSR_CheckedChanged);
            // 
            // SR
            // 
            this.SR.AutoSize = true;
            this.SR.Location = new System.Drawing.Point(202, 119);
            this.SR.Name = "SR";
            this.SR.Size = new System.Drawing.Size(88, 17);
            this.SR.TabIndex = 13;
            this.SR.TabStop = true;
            this.SR.Text = "Shift Register";
            this.SR.UseVisualStyleBackColor = true;
            this.SR.CheckedChanged += new System.EventHandler(this.LMSR_CheckedChanged);
            // 
            // LMTypes
            // 
            this.LMTypes.FormattingEnabled = true;
            this.LMTypes.Location = new System.Drawing.Point(296, 139);
            this.LMTypes.Name = "LMTypes";
            this.LMTypes.Size = new System.Drawing.Size(71, 21);
            this.LMTypes.TabIndex = 14;
            this.LMTypes.Visible = false;
            // 
            // IDDDetectorAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 183);
            this.Controls.Add(this.LMTypes);
            this.Controls.Add(this.SR);
            this.Controls.Add(this.LM);
            this.Controls.Add(this.ElectronicsIdLabel);
            this.Controls.Add(this.DetectorTypeLabel);
            this.Controls.Add(this.DetectorIdLabel);
            this.Controls.Add(this.CurrentDetectorsLabel);
            this.Controls.Add(this.InitializationHelpLabel);
            this.Controls.Add(this.ElectronicsIdTextBox);
            this.Controls.Add(this.DetectorTypeTextBox);
            this.Controls.Add(this.DetectorIdTextBox);
            this.Controls.Add(this.CurrentDetectorsComboBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Name = "IDDDetectorAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter New Detector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ComboBox CurrentDetectorsComboBox;
        private System.Windows.Forms.TextBox DetectorIdTextBox;
        private System.Windows.Forms.TextBox DetectorTypeTextBox;
        private System.Windows.Forms.TextBox ElectronicsIdTextBox;
        private System.Windows.Forms.Label InitializationHelpLabel;
        private System.Windows.Forms.Label CurrentDetectorsLabel;
        private System.Windows.Forms.Label DetectorIdLabel;
        private System.Windows.Forms.Label DetectorTypeLabel;
        private System.Windows.Forms.Label ElectronicsIdLabel;
        private System.Windows.Forms.RadioButton LM;
        private System.Windows.Forms.RadioButton SR;
        private System.Windows.Forms.ComboBox LMTypes;
    }
}