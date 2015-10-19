namespace NewUI
{
    partial class PlateauCurve
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExcelCheckBox = new System.Windows.Forms.CheckBox();
            this.DelayUnitsLabel = new System.Windows.Forms.Label();
            this.DelayTextBox = new System.Windows.Forms.TextBox();
            this.DelayLabel = new System.Windows.Forms.Label();
            this.HVStepDurationUnitsLabel = new System.Windows.Forms.Label();
            this.HVStepDurationTextBox = new System.Windows.Forms.TextBox();
            this.HVStepDurationLabel = new System.Windows.Forms.Label();
            this.HVIncrementUnitsLabel = new System.Windows.Forms.Label();
            this.HVIncrementTextBox = new System.Windows.Forms.TextBox();
            this.HVIncrementLabel = new System.Windows.Forms.Label();
            this.VoltsLabel = new System.Windows.Forms.Label();
            this.MaxHVTextBox = new System.Windows.Forms.TextBox();
            this.AndLabel = new System.Windows.Forms.Label();
            this.MinHVTextBox = new System.Windows.Forms.TextBox();
            this.HVRangeLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ExcelCheckBox);
            this.groupBox1.Controls.Add(this.DelayUnitsLabel);
            this.groupBox1.Controls.Add(this.DelayTextBox);
            this.groupBox1.Controls.Add(this.DelayLabel);
            this.groupBox1.Controls.Add(this.HVStepDurationUnitsLabel);
            this.groupBox1.Controls.Add(this.HVStepDurationTextBox);
            this.groupBox1.Controls.Add(this.HVStepDurationLabel);
            this.groupBox1.Controls.Add(this.HVIncrementUnitsLabel);
            this.groupBox1.Controls.Add(this.HVIncrementTextBox);
            this.groupBox1.Controls.Add(this.HVIncrementLabel);
            this.groupBox1.Controls.Add(this.VoltsLabel);
            this.groupBox1.Controls.Add(this.MaxHVTextBox);
            this.groupBox1.Controls.Add(this.AndLabel);
            this.groupBox1.Controls.Add(this.MinHVTextBox);
            this.groupBox1.Controls.Add(this.HVRangeLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Plateau Curve Parameters";
            // 
            // ExcelCheckBox
            // 
            this.ExcelCheckBox.AutoSize = true;
            this.ExcelCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ExcelCheckBox.Location = new System.Drawing.Point(16, 134);
            this.ExcelCheckBox.Name = "ExcelCheckBox";
            this.ExcelCheckBox.Size = new System.Drawing.Size(173, 17);
            this.ExcelCheckBox.TabIndex = 14;
            this.ExcelCheckBox.Text = "View ongoing progress in Excel";
            this.ExcelCheckBox.UseVisualStyleBackColor = true;
            // 
            // DelayUnitsLabel
            // 
            this.DelayUnitsLabel.AutoSize = true;
            this.DelayUnitsLabel.Location = new System.Drawing.Point(205, 111);
            this.DelayUnitsLabel.Name = "DelayUnitsLabel";
            this.DelayUnitsLabel.Size = new System.Drawing.Size(47, 13);
            this.DelayUnitsLabel.TabIndex = 13;
            this.DelayUnitsLabel.Text = "seconds";
            // 
            // DelayTextBox
            // 
            this.DelayTextBox.Location = new System.Drawing.Point(165, 108);
            this.DelayTextBox.Name = "DelayTextBox";
            this.DelayTextBox.Size = new System.Drawing.Size(34, 20);
            this.DelayTextBox.TabIndex = 12;
            this.DelayTextBox.Text = "2";
            // 
            // DelayLabel
            // 
            this.DelayLabel.AutoSize = true;
            this.DelayLabel.Location = new System.Drawing.Point(18, 111);
            this.DelayLabel.Name = "DelayLabel";
            this.DelayLabel.Size = new System.Drawing.Size(141, 13);
            this.DelayLabel.TabIndex = 11;
            this.DelayLabel.Text = "High voltage intra-step delay";
            // 
            // HVStepDurationUnitsLabel
            // 
            this.HVStepDurationUnitsLabel.AutoSize = true;
            this.HVStepDurationUnitsLabel.Location = new System.Drawing.Point(205, 85);
            this.HVStepDurationUnitsLabel.Name = "HVStepDurationUnitsLabel";
            this.HVStepDurationUnitsLabel.Size = new System.Drawing.Size(47, 13);
            this.HVStepDurationUnitsLabel.TabIndex = 10;
            this.HVStepDurationUnitsLabel.Text = "seconds";
            // 
            // HVStepDurationTextBox
            // 
            this.HVStepDurationTextBox.Location = new System.Drawing.Point(165, 82);
            this.HVStepDurationTextBox.Name = "HVStepDurationTextBox";
            this.HVStepDurationTextBox.Size = new System.Drawing.Size(34, 20);
            this.HVStepDurationTextBox.TabIndex = 9;
            this.HVStepDurationTextBox.Text = "1";
            // 
            // HVStepDurationLabel
            // 
            this.HVStepDurationLabel.AutoSize = true;
            this.HVStepDurationLabel.Location = new System.Drawing.Point(18, 85);
            this.HVStepDurationLabel.Name = "HVStepDurationLabel";
            this.HVStepDurationLabel.Size = new System.Drawing.Size(131, 13);
            this.HVStepDurationLabel.TabIndex = 8;
            this.HVStepDurationLabel.Text = "High voltage step duration";
            // 
            // HVIncrementUnitsLabel
            // 
            this.HVIncrementUnitsLabel.AutoSize = true;
            this.HVIncrementUnitsLabel.Location = new System.Drawing.Point(205, 57);
            this.HVIncrementUnitsLabel.Name = "HVIncrementUnitsLabel";
            this.HVIncrementUnitsLabel.Size = new System.Drawing.Size(30, 13);
            this.HVIncrementUnitsLabel.TabIndex = 7;
            this.HVIncrementUnitsLabel.Text = "Volts";
            // 
            // HVIncrementTextBox
            // 
            this.HVIncrementTextBox.Location = new System.Drawing.Point(165, 54);
            this.HVIncrementTextBox.Name = "HVIncrementTextBox";
            this.HVIncrementTextBox.Size = new System.Drawing.Size(34, 20);
            this.HVIncrementTextBox.TabIndex = 6;
            this.HVIncrementTextBox.Text = "10";
            // 
            // HVIncrementLabel
            // 
            this.HVIncrementLabel.AutoSize = true;
            this.HVIncrementLabel.Location = new System.Drawing.Point(18, 57);
            this.HVIncrementLabel.Name = "HVIncrementLabel";
            this.HVIncrementLabel.Size = new System.Drawing.Size(116, 13);
            this.HVIncrementLabel.TabIndex = 5;
            this.HVIncrementLabel.Text = "High voltage increment";
            // 
            // VoltsLabel
            // 
            this.VoltsLabel.AutoSize = true;
            this.VoltsLabel.Location = new System.Drawing.Point(276, 31);
            this.VoltsLabel.Name = "VoltsLabel";
            this.VoltsLabel.Size = new System.Drawing.Size(33, 13);
            this.VoltsLabel.TabIndex = 4;
            this.VoltsLabel.Text = "Volts.";
            // 
            // MaxHVTextBox
            // 
            this.MaxHVTextBox.Location = new System.Drawing.Point(236, 28);
            this.MaxHVTextBox.Name = "MaxHVTextBox";
            this.MaxHVTextBox.Size = new System.Drawing.Size(34, 20);
            this.MaxHVTextBox.TabIndex = 3;
            this.MaxHVTextBox.Text = "2000";
            // 
            // AndLabel
            // 
            this.AndLabel.AutoSize = true;
            this.AndLabel.Location = new System.Drawing.Point(205, 31);
            this.AndLabel.Name = "AndLabel";
            this.AndLabel.Size = new System.Drawing.Size(25, 13);
            this.AndLabel.TabIndex = 2;
            this.AndLabel.Text = "and";
            // 
            // MinHVTextBox
            // 
            this.MinHVTextBox.Location = new System.Drawing.Point(165, 28);
            this.MinHVTextBox.Name = "MinHVTextBox";
            this.MinHVTextBox.Size = new System.Drawing.Size(34, 20);
            this.MinHVTextBox.TabIndex = 1;
            this.MinHVTextBox.Text = "0";
            // 
            // HVRangeLabel
            // 
            this.HVRangeLabel.AutoSize = true;
            this.HVRangeLabel.Location = new System.Drawing.Point(18, 31);
            this.HVRangeLabel.Name = "HVRangeLabel";
            this.HVRangeLabel.Size = new System.Drawing.Size(141, 13);
            this.HVRangeLabel.TabIndex = 0;
            this.HVRangeLabel.Text = "High voltage range between";
            // 
            // PlateauCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 229);
            this.Controls.Add(this.groupBox1);
            this.Name = "PlateauCurve";
            this.Text = "Plateau Curve Parameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ExcelCheckBox;
        private System.Windows.Forms.Label DelayUnitsLabel;
        private System.Windows.Forms.TextBox DelayTextBox;
        private System.Windows.Forms.Label DelayLabel;
        private System.Windows.Forms.Label HVStepDurationUnitsLabel;
        private System.Windows.Forms.TextBox HVStepDurationTextBox;
        private System.Windows.Forms.Label HVStepDurationLabel;
        private System.Windows.Forms.Label HVIncrementUnitsLabel;
        private System.Windows.Forms.TextBox HVIncrementTextBox;
        private System.Windows.Forms.Label HVIncrementLabel;
        private System.Windows.Forms.Label VoltsLabel;
        private System.Windows.Forms.TextBox MaxHVTextBox;
        private System.Windows.Forms.Label AndLabel;
        private System.Windows.Forms.TextBox MinHVTextBox;
        private System.Windows.Forms.Label HVRangeLabel;
    }
}