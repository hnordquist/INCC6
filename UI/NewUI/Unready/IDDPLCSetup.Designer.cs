namespace NewUI
{
    partial class IDDPLCSetup
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
            this.OKbtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.PLCSerialPortLabel = new System.Windows.Forms.Label();
            this.PLCSerialPortComboBox = new System.Windows.Forms.ComboBox();
            this.NumCfSrcMeasPosLabel = new System.Windows.Forms.Label();
            this.DistanceHomeTo1stLabel = new System.Windows.Forms.Label();
            this.Distance1stTo2ndLabel = new System.Windows.Forms.Label();
            this.Distance2ndTo3rdLabel = new System.Windows.Forms.Label();
            this.Distance3rdTo4thLabel = new System.Windows.Forms.Label();
            this.Distance4thTo5thLabel = new System.Windows.Forms.Label();
            this.PLCStepsPerInchLabel = new System.Windows.Forms.Label();
            this.ScaleConversionFactorLabel = new System.Windows.Forms.Label();
            this.NumCfSrcMeasPosTextBox = new System.Windows.Forms.TextBox();
            this.DistanceHomeTo1stTextBox = new System.Windows.Forms.TextBox();
            this.Distance1stTo2ndTextBox = new System.Windows.Forms.TextBox();
            this.Distance2ndTo3rdTextBox = new System.Windows.Forms.TextBox();
            this.Distance3rdTo4thTextBox = new System.Windows.Forms.TextBox();
            this.Distance4thTo5thTextBox = new System.Windows.Forms.TextBox();
            this.PLCStepsPerInchTextBox = new System.Windows.Forms.TextBox();
            this.ScaleConversionFactorTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKbtn
            // 
            this.OKbtn.Location = new System.Drawing.Point(490, 12);
            this.OKbtn.Name = "OKbtn";
            this.OKbtn.Size = new System.Drawing.Size(75, 23);
            this.OKbtn.TabIndex = 0;
            this.OKbtn.Text = "OK";
            this.OKbtn.UseVisualStyleBackColor = true;
            this.OKbtn.Click += new System.EventHandler(this.OKbtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(490, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(490, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(490, 112);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(75, 23);
            this.PrintBtn.TabIndex = 3;
            this.PrintBtn.Text = "Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // PLCSerialPortLabel
            // 
            this.PLCSerialPortLabel.AutoSize = true;
            this.PLCSerialPortLabel.Location = new System.Drawing.Point(285, 17);
            this.PLCSerialPortLabel.Name = "PLCSerialPortLabel";
            this.PLCSerialPortLabel.Size = new System.Drawing.Size(75, 13);
            this.PLCSerialPortLabel.TabIndex = 4;
            this.PLCSerialPortLabel.Text = "PLC serial port";
            // 
            // PLCSerialPortComboBox
            // 
            this.PLCSerialPortComboBox.FormattingEnabled = true;
            this.PLCSerialPortComboBox.Location = new System.Drawing.Point(366, 14);
            this.PLCSerialPortComboBox.Name = "PLCSerialPortComboBox";
            this.PLCSerialPortComboBox.Size = new System.Drawing.Size(100, 21);
            this.PLCSerialPortComboBox.TabIndex = 5;
            this.PLCSerialPortComboBox.SelectedIndexChanged += new System.EventHandler(this.PLCSerialPortComboBox_SelectedIndexChanged);
            // 
            // NumCfSrcMeasPosLabel
            // 
            this.NumCfSrcMeasPosLabel.AutoSize = true;
            this.NumCfSrcMeasPosLabel.Location = new System.Drawing.Point(128, 75);
            this.NumCfSrcMeasPosLabel.Name = "NumCfSrcMeasPosLabel";
            this.NumCfSrcMeasPosLabel.Size = new System.Drawing.Size(232, 13);
            this.NumCfSrcMeasPosLabel.TabIndex = 6;
            this.NumCfSrcMeasPosLabel.Text = "Number of Cf252 source measurement positions";
            // 
            // DistanceHomeTo1stLabel
            // 
            this.DistanceHomeTo1stLabel.AutoSize = true;
            this.DistanceHomeTo1stLabel.Location = new System.Drawing.Point(44, 101);
            this.DistanceHomeTo1stLabel.Name = "DistanceHomeTo1stLabel";
            this.DistanceHomeTo1stLabel.Size = new System.Drawing.Size(316, 13);
            this.DistanceHomeTo1stLabel.TabIndex = 7;
            this.DistanceHomeTo1stLabel.Text = "Distance to move Cf252 source from home to 1st position (inches)";
            // 
            // Distance1stTo2ndLabel
            // 
            this.Distance1stTo2ndLabel.AutoSize = true;
            this.Distance1stTo2ndLabel.Location = new System.Drawing.Point(13, 127);
            this.Distance1stTo2ndLabel.Name = "Distance1stTo2ndLabel";
            this.Distance1stTo2ndLabel.Size = new System.Drawing.Size(347, 13);
            this.Distance1stTo2ndLabel.TabIndex = 8;
            this.Distance1stTo2ndLabel.Text = "Distance to move Cf252 source from 1st position to 2nd position (inches)";
            // 
            // Distance2ndTo3rdLabel
            // 
            this.Distance2ndTo3rdLabel.AutoSize = true;
            this.Distance2ndTo3rdLabel.Location = new System.Drawing.Point(12, 153);
            this.Distance2ndTo3rdLabel.Name = "Distance2ndTo3rdLabel";
            this.Distance2ndTo3rdLabel.Size = new System.Drawing.Size(348, 13);
            this.Distance2ndTo3rdLabel.TabIndex = 9;
            this.Distance2ndTo3rdLabel.Text = "Distance to move Cf252 source from 2nd position to 3rd position (inches)";
            // 
            // Distance3rdTo4thLabel
            // 
            this.Distance3rdTo4thLabel.AutoSize = true;
            this.Distance3rdTo4thLabel.Location = new System.Drawing.Point(15, 179);
            this.Distance3rdTo4thLabel.Name = "Distance3rdTo4thLabel";
            this.Distance3rdTo4thLabel.Size = new System.Drawing.Size(345, 13);
            this.Distance3rdTo4thLabel.TabIndex = 10;
            this.Distance3rdTo4thLabel.Text = "Distance to move Cf252 source from 3rd position to 4th position (inches)";
            // 
            // Distance4thTo5thLabel
            // 
            this.Distance4thTo5thLabel.AutoSize = true;
            this.Distance4thTo5thLabel.Location = new System.Drawing.Point(15, 205);
            this.Distance4thTo5thLabel.Name = "Distance4thTo5thLabel";
            this.Distance4thTo5thLabel.Size = new System.Drawing.Size(345, 13);
            this.Distance4thTo5thLabel.TabIndex = 11;
            this.Distance4thTo5thLabel.Text = "Distance to move Cf252 source from 4th position to 5th position (inches)";
            // 
            // PLCStepsPerInchLabel
            // 
            this.PLCStepsPerInchLabel.AutoSize = true;
            this.PLCStepsPerInchLabel.Location = new System.Drawing.Point(264, 231);
            this.PLCStepsPerInchLabel.Name = "PLCStepsPerInchLabel";
            this.PLCStepsPerInchLabel.Size = new System.Drawing.Size(96, 13);
            this.PLCStepsPerInchLabel.TabIndex = 12;
            this.PLCStepsPerInchLabel.Text = "PLC steps per inch";
            // 
            // ScaleConversionFactorLabel
            // 
            this.ScaleConversionFactorLabel.AutoSize = true;
            this.ScaleConversionFactorLabel.Location = new System.Drawing.Point(241, 257);
            this.ScaleConversionFactorLabel.Name = "ScaleConversionFactorLabel";
            this.ScaleConversionFactorLabel.Size = new System.Drawing.Size(119, 13);
            this.ScaleConversionFactorLabel.TabIndex = 13;
            this.ScaleConversionFactorLabel.Text = "Scale conversion factor";
            // 
            // NumCfSrcMeasPosTextBox
            // 
            this.NumCfSrcMeasPosTextBox.Location = new System.Drawing.Point(366, 72);
            this.NumCfSrcMeasPosTextBox.Name = "NumCfSrcMeasPosTextBox";
            this.NumCfSrcMeasPosTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumCfSrcMeasPosTextBox.TabIndex = 14;
            this.NumCfSrcMeasPosTextBox.TextChanged += new System.EventHandler(this.NumCfSrcMeasPosTextBox_TextChanged);
            // 
            // DistanceHomeTo1stTextBox
            // 
            this.DistanceHomeTo1stTextBox.Location = new System.Drawing.Point(366, 98);
            this.DistanceHomeTo1stTextBox.Name = "DistanceHomeTo1stTextBox";
            this.DistanceHomeTo1stTextBox.Size = new System.Drawing.Size(100, 20);
            this.DistanceHomeTo1stTextBox.TabIndex = 15;
            this.DistanceHomeTo1stTextBox.TextChanged += new System.EventHandler(this.DistanceHomeTo1stTextBox_TextChanged);
            // 
            // Distance1stTo2ndTextBox
            // 
            this.Distance1stTo2ndTextBox.Location = new System.Drawing.Point(366, 124);
            this.Distance1stTo2ndTextBox.Name = "Distance1stTo2ndTextBox";
            this.Distance1stTo2ndTextBox.Size = new System.Drawing.Size(100, 20);
            this.Distance1stTo2ndTextBox.TabIndex = 16;
            this.Distance1stTo2ndTextBox.TextChanged += new System.EventHandler(this.Distance1stTo2ndTextBox_TextChanged);
            // 
            // Distance2ndTo3rdTextBox
            // 
            this.Distance2ndTo3rdTextBox.Location = new System.Drawing.Point(366, 150);
            this.Distance2ndTo3rdTextBox.Name = "Distance2ndTo3rdTextBox";
            this.Distance2ndTo3rdTextBox.Size = new System.Drawing.Size(100, 20);
            this.Distance2ndTo3rdTextBox.TabIndex = 17;
            this.Distance2ndTo3rdTextBox.TextChanged += new System.EventHandler(this.Distance2ndTo3rdTextBox_TextChanged);
            // 
            // Distance3rdTo4thTextBox
            // 
            this.Distance3rdTo4thTextBox.Location = new System.Drawing.Point(366, 176);
            this.Distance3rdTo4thTextBox.Name = "Distance3rdTo4thTextBox";
            this.Distance3rdTo4thTextBox.Size = new System.Drawing.Size(100, 20);
            this.Distance3rdTo4thTextBox.TabIndex = 18;
            this.Distance3rdTo4thTextBox.TextChanged += new System.EventHandler(this.Distance3rdTo4thTextBox_TextChanged);
            // 
            // Distance4thTo5thTextBox
            // 
            this.Distance4thTo5thTextBox.Location = new System.Drawing.Point(366, 202);
            this.Distance4thTo5thTextBox.Name = "Distance4thTo5thTextBox";
            this.Distance4thTo5thTextBox.Size = new System.Drawing.Size(100, 20);
            this.Distance4thTo5thTextBox.TabIndex = 19;
            this.Distance4thTo5thTextBox.TextChanged += new System.EventHandler(this.Distance4thTo5thTextBox_TextChanged);
            // 
            // PLCStepsPerInchTextBox
            // 
            this.PLCStepsPerInchTextBox.Location = new System.Drawing.Point(366, 228);
            this.PLCStepsPerInchTextBox.Name = "PLCStepsPerInchTextBox";
            this.PLCStepsPerInchTextBox.Size = new System.Drawing.Size(100, 20);
            this.PLCStepsPerInchTextBox.TabIndex = 20;
            this.PLCStepsPerInchTextBox.TextChanged += new System.EventHandler(this.PLCStepsPerInchTextBox_TextChanged);
            // 
            // ScaleConversionFactorTextBox
            // 
            this.ScaleConversionFactorTextBox.Location = new System.Drawing.Point(366, 254);
            this.ScaleConversionFactorTextBox.Name = "ScaleConversionFactorTextBox";
            this.ScaleConversionFactorTextBox.Size = new System.Drawing.Size(100, 20);
            this.ScaleConversionFactorTextBox.TabIndex = 21;
            this.ScaleConversionFactorTextBox.TextChanged += new System.EventHandler(this.ScaleConversionFactorTextBox_TextChanged);
            // 
            // IDDPLCSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 290);
            this.Controls.Add(this.ScaleConversionFactorTextBox);
            this.Controls.Add(this.PLCStepsPerInchTextBox);
            this.Controls.Add(this.Distance4thTo5thTextBox);
            this.Controls.Add(this.Distance3rdTo4thTextBox);
            this.Controls.Add(this.Distance2ndTo3rdTextBox);
            this.Controls.Add(this.Distance1stTo2ndTextBox);
            this.Controls.Add(this.DistanceHomeTo1stTextBox);
            this.Controls.Add(this.NumCfSrcMeasPosTextBox);
            this.Controls.Add(this.ScaleConversionFactorLabel);
            this.Controls.Add(this.PLCStepsPerInchLabel);
            this.Controls.Add(this.Distance4thTo5thLabel);
            this.Controls.Add(this.Distance3rdTo4thLabel);
            this.Controls.Add(this.Distance2ndTo3rdLabel);
            this.Controls.Add(this.Distance1stTo2ndLabel);
            this.Controls.Add(this.DistanceHomeTo1stLabel);
            this.Controls.Add(this.NumCfSrcMeasPosLabel);
            this.Controls.Add(this.PLCSerialPortComboBox);
            this.Controls.Add(this.PLCSerialPortLabel);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKbtn);
            this.Name = "IDDPLCSetup";
            this.Text = "PLC Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKbtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Label PLCSerialPortLabel;
        private System.Windows.Forms.ComboBox PLCSerialPortComboBox;
        private System.Windows.Forms.Label NumCfSrcMeasPosLabel;
        private System.Windows.Forms.Label DistanceHomeTo1stLabel;
        private System.Windows.Forms.Label Distance1stTo2ndLabel;
        private System.Windows.Forms.Label Distance2ndTo3rdLabel;
        private System.Windows.Forms.Label Distance3rdTo4thLabel;
        private System.Windows.Forms.Label Distance4thTo5thLabel;
        private System.Windows.Forms.Label PLCStepsPerInchLabel;
        private System.Windows.Forms.Label ScaleConversionFactorLabel;
        private System.Windows.Forms.TextBox NumCfSrcMeasPosTextBox;
        private System.Windows.Forms.TextBox DistanceHomeTo1stTextBox;
        private System.Windows.Forms.TextBox Distance1stTo2ndTextBox;
        private System.Windows.Forms.TextBox Distance2ndTo3rdTextBox;
        private System.Windows.Forms.TextBox Distance3rdTo4thTextBox;
        private System.Windows.Forms.TextBox Distance4thTo5thTextBox;
        private System.Windows.Forms.TextBox PLCStepsPerInchTextBox;
        private System.Windows.Forms.TextBox ScaleConversionFactorTextBox;
    }
}