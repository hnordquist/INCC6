﻿namespace UI
{
    partial class IDDHighVoltagePlateau
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
            this.MinimumVoltageLabel = new System.Windows.Forms.Label();
            this.MaximumVoltageLabel = new System.Windows.Forms.Label();
            this.VoltageStepSizeLabel = new System.Windows.Forms.Label();
            this.CountTimeLabel = new System.Windows.Forms.Label();
            this.MinimumVoltageTextBox = new System.Windows.Forms.TextBox();
            this.MaximumVoltageTextBox = new System.Windows.Forms.TextBox();
            this.VoltageStepSizeTextBox = new System.Windows.Forms.TextBox();
            this.CountTimeTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.OpenInExcel = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.HVStepDelay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MinimumVoltageLabel
            // 
            this.MinimumVoltageLabel.AutoSize = true;
            this.MinimumVoltageLabel.Location = new System.Drawing.Point(66, 19);
            this.MinimumVoltageLabel.Name = "MinimumVoltageLabel";
            this.MinimumVoltageLabel.Size = new System.Drawing.Size(86, 13);
            this.MinimumVoltageLabel.TabIndex = 0;
            this.MinimumVoltageLabel.Text = "Minimum voltage";
            // 
            // MaximumVoltageLabel
            // 
            this.MaximumVoltageLabel.AutoSize = true;
            this.MaximumVoltageLabel.Location = new System.Drawing.Point(63, 45);
            this.MaximumVoltageLabel.Name = "MaximumVoltageLabel";
            this.MaximumVoltageLabel.Size = new System.Drawing.Size(89, 13);
            this.MaximumVoltageLabel.TabIndex = 1;
            this.MaximumVoltageLabel.Text = "Maximum voltage";
            // 
            // VoltageStepSizeLabel
            // 
            this.VoltageStepSizeLabel.AutoSize = true;
            this.VoltageStepSizeLabel.Location = new System.Drawing.Point(65, 71);
            this.VoltageStepSizeLabel.Name = "VoltageStepSizeLabel";
            this.VoltageStepSizeLabel.Size = new System.Drawing.Size(87, 13);
            this.VoltageStepSizeLabel.TabIndex = 2;
            this.VoltageStepSizeLabel.Text = "Voltage step size";
            // 
            // CountTimeLabel
            // 
            this.CountTimeLabel.AutoSize = true;
            this.CountTimeLabel.Location = new System.Drawing.Point(46, 97);
            this.CountTimeLabel.Name = "CountTimeLabel";
            this.CountTimeLabel.Size = new System.Drawing.Size(106, 13);
            this.CountTimeLabel.TabIndex = 3;
            this.CountTimeLabel.Text = "Count time (seconds)";
            // 
            // MinimumVoltageTextBox
            // 
            this.MinimumVoltageTextBox.Location = new System.Drawing.Point(162, 17);
            this.MinimumVoltageTextBox.Name = "MinimumVoltageTextBox";
            this.MinimumVoltageTextBox.Size = new System.Drawing.Size(74, 20);
            this.MinimumVoltageTextBox.TabIndex = 4;
            this.toolTip1.SetToolTip(this.MinimumVoltageTextBox, "Starting voltage for the plateua");
            this.MinimumVoltageTextBox.Leave += new System.EventHandler(this.MinimumVoltageTextBox_Leave);
            // 
            // MaximumVoltageTextBox
            // 
            this.MaximumVoltageTextBox.Location = new System.Drawing.Point(162, 42);
            this.MaximumVoltageTextBox.Name = "MaximumVoltageTextBox";
            this.MaximumVoltageTextBox.Size = new System.Drawing.Size(74, 20);
            this.MaximumVoltageTextBox.TabIndex = 5;
            this.toolTip1.SetToolTip(this.MaximumVoltageTextBox, "Max HV voltage");
            this.MaximumVoltageTextBox.Leave += new System.EventHandler(this.MaximumVoltageTextBox_Leave);
            // 
            // VoltageStepSizeTextBox
            // 
            this.VoltageStepSizeTextBox.Location = new System.Drawing.Point(162, 68);
            this.VoltageStepSizeTextBox.Name = "VoltageStepSizeTextBox";
            this.VoltageStepSizeTextBox.Size = new System.Drawing.Size(74, 20);
            this.VoltageStepSizeTextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.VoltageStepSizeTextBox, "Voltage step increment ");
            this.VoltageStepSizeTextBox.Leave += new System.EventHandler(this.VoltageStepSizeTextBox_Leave);
            // 
            // CountTimeTextBox
            // 
            this.CountTimeTextBox.Location = new System.Drawing.Point(162, 94);
            this.CountTimeTextBox.Name = "CountTimeTextBox";
            this.CountTimeTextBox.Size = new System.Drawing.Size(74, 20);
            this.CountTimeTextBox.TabIndex = 7;
            this.toolTip1.SetToolTip(this.CountTimeTextBox, "HV step duration, seconds");
            this.CountTimeTextBox.Leave += new System.EventHandler(this.CountTimeTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(258, 13);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(258, 42);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(258, 71);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 10;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // OpenInExcel
            // 
            this.OpenInExcel.AutoSize = true;
            this.OpenInExcel.Enabled = false;
            this.OpenInExcel.Location = new System.Drawing.Point(178, 156);
            this.OpenInExcel.Name = "OpenInExcel";
            this.OpenInExcel.Size = new System.Drawing.Size(155, 17);
            this.OpenInExcel.TabIndex = 11;
            this.OpenInExcel.Text = "Monitor progress with Excel";
            this.toolTip1.SetToolTip(this.OpenInExcel, "Send HV calibration steps to Excel. Enabled only if Excel available");
            this.OpenInExcel.UseVisualStyleBackColor = true;
            this.OpenInExcel.CheckedChanged += new System.EventHandler(this.OpenInExcel_CheckedChanged);
            // 
            // HVStepDelay
            // 
            this.HVStepDelay.Location = new System.Drawing.Point(162, 120);
            this.HVStepDelay.Name = "HVStepDelay";
            this.HVStepDelay.Size = new System.Drawing.Size(74, 20);
            this.HVStepDelay.TabIndex = 13;
            this.toolTip1.SetToolTip(this.HVStepDelay, "Delay after HV step is complete, in milliseconds");
            this.HVStepDelay.Leave += new System.EventHandler(this.HVStepDelay_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Intra step delay (milliseconds)";
            // 
            // IDDHighVoltagePlateau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 185);
            this.Controls.Add(this.HVStepDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpenInExcel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CountTimeTextBox);
            this.Controls.Add(this.VoltageStepSizeTextBox);
            this.Controls.Add(this.MaximumVoltageTextBox);
            this.Controls.Add(this.MinimumVoltageTextBox);
            this.Controls.Add(this.CountTimeLabel);
            this.Controls.Add(this.VoltageStepSizeLabel);
            this.Controls.Add(this.MaximumVoltageLabel);
            this.Controls.Add(this.MinimumVoltageLabel);
            this.Name = "IDDHighVoltagePlateau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Measure High Voltage Plateau";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MinimumVoltageLabel;
        private System.Windows.Forms.Label MaximumVoltageLabel;
        private System.Windows.Forms.Label VoltageStepSizeLabel;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.TextBox MinimumVoltageTextBox;
        private System.Windows.Forms.TextBox MaximumVoltageTextBox;
        private System.Windows.Forms.TextBox VoltageStepSizeTextBox;
        private System.Windows.Forms.TextBox CountTimeTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.CheckBox OpenInExcel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox HVStepDelay;
        private System.Windows.Forms.Label label1;
    }
}