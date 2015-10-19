namespace NewUI
{
    partial class IDDTruncatedMultCalibration
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
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.ALabel = new System.Windows.Forms.Label();
            this.ATextBox = new System.Windows.Forms.TextBox();
            this.BLabel = new System.Windows.Forms.Label();
            this.BTextBox = new System.Windows.Forms.TextBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.BothEfficiencyMethodsRadioButton = new System.Windows.Forms.RadioButton();
            this.SolveForEfficiencyRadioButton = new System.Windows.Forms.RadioButton();
            this.KnownEfficiencyRadioButton = new System.Windows.Forms.RadioButton();
            this.PrintCalParametersButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(12, 21);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 0;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(85, 18);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(153, 21);
            this.MaterialTypeComboBox.TabIndex = 1;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // ALabel
            // 
            this.ALabel.AutoSize = true;
            this.ALabel.Location = new System.Drawing.Point(66, 165);
            this.ALabel.Name = "ALabel";
            this.ALabel.Size = new System.Drawing.Size(13, 13);
            this.ALabel.TabIndex = 2;
            this.ALabel.Text = "a";
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(85, 160);
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.TabIndex = 3;
            this.ATextBox.TextChanged += new System.EventHandler(this.ATextBox_TextChanged);
            // 
            // BLabel
            // 
            this.BLabel.AutoSize = true;
            this.BLabel.Location = new System.Drawing.Point(66, 195);
            this.BLabel.Name = "BLabel";
            this.BLabel.Size = new System.Drawing.Size(13, 13);
            this.BLabel.TabIndex = 4;
            this.BLabel.Text = "b";
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(85, 192);
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.TabIndex = 5;
            this.BTextBox.TextChanged += new System.EventHandler(this.BTextBox_TextChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.BothEfficiencyMethodsRadioButton);
            this.GroupBox1.Controls.Add(this.SolveForEfficiencyRadioButton);
            this.GroupBox1.Controls.Add(this.KnownEfficiencyRadioButton);
            this.GroupBox1.Location = new System.Drawing.Point(203, 144);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(199, 102);
            this.GroupBox1.TabIndex = 6;
            this.GroupBox1.TabStop = false;
            // 
            // BothEfficiencyMethodsRadioButton
            // 
            this.BothEfficiencyMethodsRadioButton.AutoSize = true;
            this.BothEfficiencyMethodsRadioButton.Location = new System.Drawing.Point(20, 65);
            this.BothEfficiencyMethodsRadioButton.Name = "BothEfficiencyMethodsRadioButton";
            this.BothEfficiencyMethodsRadioButton.Size = new System.Drawing.Size(159, 17);
            this.BothEfficiencyMethodsRadioButton.TabIndex = 2;
            this.BothEfficiencyMethodsRadioButton.TabStop = true;
            this.BothEfficiencyMethodsRadioButton.Text = "Use both efficiency methods";
            this.BothEfficiencyMethodsRadioButton.UseVisualStyleBackColor = true;
            this.BothEfficiencyMethodsRadioButton.CheckedChanged += new System.EventHandler(this.BothEfficiencyMethodsRadioButton_CheckedChanged);
            // 
            // SolveForEfficiencyRadioButton
            // 
            this.SolveForEfficiencyRadioButton.AutoSize = true;
            this.SolveForEfficiencyRadioButton.Location = new System.Drawing.Point(20, 42);
            this.SolveForEfficiencyRadioButton.Name = "SolveForEfficiencyRadioButton";
            this.SolveForEfficiencyRadioButton.Size = new System.Drawing.Size(115, 17);
            this.SolveForEfficiencyRadioButton.TabIndex = 1;
            this.SolveForEfficiencyRadioButton.TabStop = true;
            this.SolveForEfficiencyRadioButton.Text = "Solve for efficiency";
            this.SolveForEfficiencyRadioButton.UseVisualStyleBackColor = true;
            this.SolveForEfficiencyRadioButton.CheckedChanged += new System.EventHandler(this.SolveForEfficiencyRadioButton_CheckedChanged);
            // 
            // KnownEfficiencyRadioButton
            // 
            this.KnownEfficiencyRadioButton.AutoSize = true;
            this.KnownEfficiencyRadioButton.Location = new System.Drawing.Point(20, 19);
            this.KnownEfficiencyRadioButton.Name = "KnownEfficiencyRadioButton";
            this.KnownEfficiencyRadioButton.Size = new System.Drawing.Size(127, 17);
            this.KnownEfficiencyRadioButton.TabIndex = 0;
            this.KnownEfficiencyRadioButton.TabStop = true;
            this.KnownEfficiencyRadioButton.Text = "Use known efficiency";
            this.KnownEfficiencyRadioButton.UseVisualStyleBackColor = true;
            this.KnownEfficiencyRadioButton.CheckedChanged += new System.EventHandler(this.KnownEfficiencyRadioButton_CheckedChanged);
            // 
            // PrintCalParametersButton
            // 
            this.PrintCalParametersButton.Location = new System.Drawing.Point(257, 16);
            this.PrintCalParametersButton.Name = "PrintCalParametersButton";
            this.PrintCalParametersButton.Size = new System.Drawing.Size(145, 23);
            this.PrintCalParametersButton.TabIndex = 7;
            this.PrintCalParametersButton.Text = "Print calibration parameters";
            this.PrintCalParametersButton.UseVisualStyleBackColor = true;
            this.PrintCalParametersButton.Click += new System.EventHandler(this.PrintCalParametersButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(327, 45);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 8;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(327, 74);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(327, 103);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 10;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDTruncatedMultCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 271);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.PrintCalParametersButton);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.BTextBox);
            this.Controls.Add(this.BLabel);
            this.Controls.Add(this.ATextBox);
            this.Controls.Add(this.ALabel);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Name = "IDDTruncatedMultCalibration";
            this.Text = "Truncated Multiplicity Calibration";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.Label ALabel;
        private System.Windows.Forms.TextBox ATextBox;
        private System.Windows.Forms.Label BLabel;
        private System.Windows.Forms.TextBox BTextBox;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton BothEfficiencyMethodsRadioButton;
        private System.Windows.Forms.RadioButton SolveForEfficiencyRadioButton;
        private System.Windows.Forms.RadioButton KnownEfficiencyRadioButton;
        private System.Windows.Forms.Button PrintCalParametersButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}