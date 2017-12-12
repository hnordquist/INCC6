namespace UI
{
    partial class IDDCmSetup
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
            this.SerialPortComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.RotateDrumCheckBox = new System.Windows.Forms.CheckBox();
            this.HomeToForwardSwitchTextBox = new System.Windows.Forms.TextBox();
            this.HomeToReverseSwitchTextBox = new System.Windows.Forms.TextBox();
            this.NumMeasPosTextBox = new System.Windows.Forms.TextBox();
            this.DistHomeTo1stPosTextBox = new System.Windows.Forms.TextBox();
            this.Dist1stTo2ndPosTextBox = new System.Windows.Forms.TextBox();
            this.Dist2ndTo3rdPosTextBox = new System.Windows.Forms.TextBox();
            this.Dist4thTo5thPosTextBox = new System.Windows.Forms.TextBox();
            this.StepsPerInchTextBox = new System.Windows.Forms.TextBox();
            this.ForwardBitMaskTextBox = new System.Windows.Forms.TextBox();
            this.ReverseBitMaskTextBox = new System.Windows.Forms.TextBox();
            this.AxisNumberTextBox = new System.Windows.Forms.TextBox();
            this.OverTravelPolarityTextBox = new System.Windows.Forms.TextBox();
            this.EncoderRatioTextBox = new System.Windows.Forms.TextBox();
            this.HomeSearchDistTextBox = new System.Windows.Forms.TextBox();
            this.Dist3rdTo4thPosTextBox = new System.Windows.Forms.TextBox();
            this.SerialPortLabel = new System.Windows.Forms.Label();
            this.HomeToForwardSwitchLabel = new System.Windows.Forms.Label();
            this.HomeToReverseSwitchLabel = new System.Windows.Forms.Label();
            this.NumMeasPosLabel = new System.Windows.Forms.Label();
            this.DistHomeTo1stPosLabel = new System.Windows.Forms.Label();
            this.Dist1stTo2ndPosLabel = new System.Windows.Forms.Label();
            this.Dist2ndTo3rdPosLabel = new System.Windows.Forms.Label();
            this.Dist3rdTo4thPosLabel = new System.Windows.Forms.Label();
            this.Dist4thTo5thPosLabel = new System.Windows.Forms.Label();
            this.StepsPerInchLabel = new System.Windows.Forms.Label();
            this.ForwardBitMaskLabel = new System.Windows.Forms.Label();
            this.ReverseBitMaskLabel = new System.Windows.Forms.Label();
            this.AxisNumberLabel = new System.Windows.Forms.Label();
            this.OverTravelPolarityLabel = new System.Windows.Forms.Label();
            this.EncoderRatioLabel = new System.Windows.Forms.Label();
            this.HomeSearchDistLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SerialPortComboBox
            // 
            this.SerialPortComboBox.FormattingEnabled = true;
            this.SerialPortComboBox.Location = new System.Drawing.Point(387, 12);
            this.SerialPortComboBox.Name = "SerialPortComboBox";
            this.SerialPortComboBox.Size = new System.Drawing.Size(121, 21);
            this.SerialPortComboBox.TabIndex = 0;
            this.SerialPortComboBox.SelectedIndexChanged += new System.EventHandler(this.SerialPortComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(539, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(539, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(539, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(539, 99);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(75, 23);
            this.PrintBtn.TabIndex = 4;
            this.PrintBtn.Text = "Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // RotateDrumCheckBox
            // 
            this.RotateDrumCheckBox.AutoSize = true;
            this.RotateDrumCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RotateDrumCheckBox.Location = new System.Drawing.Point(316, 448);
            this.RotateDrumCheckBox.Name = "RotateDrumCheckBox";
            this.RotateDrumCheckBox.Size = new System.Drawing.Size(84, 17);
            this.RotateDrumCheckBox.TabIndex = 5;
            this.RotateDrumCheckBox.Text = "Rotate drum";
            this.RotateDrumCheckBox.UseVisualStyleBackColor = true;
            this.RotateDrumCheckBox.CheckedChanged += new System.EventHandler(this.RotateDrumCheckBox_CheckedChanged);
            // 
            // HomeToForwardSwitchTextBox
            // 
            this.HomeToForwardSwitchTextBox.Location = new System.Drawing.Point(387, 58);
            this.HomeToForwardSwitchTextBox.Name = "HomeToForwardSwitchTextBox";
            this.HomeToForwardSwitchTextBox.Size = new System.Drawing.Size(121, 20);
            this.HomeToForwardSwitchTextBox.TabIndex = 6;
            this.HomeToForwardSwitchTextBox.TextChanged += new System.EventHandler(this.HomeToForwardSwitchTextBox_TextChanged);
            // 
            // HomeToReverseSwitchTextBox
            // 
            this.HomeToReverseSwitchTextBox.Location = new System.Drawing.Point(387, 84);
            this.HomeToReverseSwitchTextBox.Name = "HomeToReverseSwitchTextBox";
            this.HomeToReverseSwitchTextBox.Size = new System.Drawing.Size(121, 20);
            this.HomeToReverseSwitchTextBox.TabIndex = 7;
            this.HomeToReverseSwitchTextBox.TextChanged += new System.EventHandler(this.HomeToReverseSwitchTextBox_TextChanged);
            // 
            // NumMeasPosTextBox
            // 
            this.NumMeasPosTextBox.Location = new System.Drawing.Point(387, 110);
            this.NumMeasPosTextBox.Name = "NumMeasPosTextBox";
            this.NumMeasPosTextBox.Size = new System.Drawing.Size(121, 20);
            this.NumMeasPosTextBox.TabIndex = 8;
            this.NumMeasPosTextBox.TextChanged += new System.EventHandler(this.NumMeasPosTextBox_TextChanged);
            // 
            // DistHomeTo1stPosTextBox
            // 
            this.DistHomeTo1stPosTextBox.Location = new System.Drawing.Point(387, 136);
            this.DistHomeTo1stPosTextBox.Name = "DistHomeTo1stPosTextBox";
            this.DistHomeTo1stPosTextBox.Size = new System.Drawing.Size(121, 20);
            this.DistHomeTo1stPosTextBox.TabIndex = 9;
            this.DistHomeTo1stPosTextBox.TextChanged += new System.EventHandler(this.DistHomeTo1stPosTextBox_TextChanged);
            // 
            // Dist1stTo2ndPosTextBox
            // 
            this.Dist1stTo2ndPosTextBox.Location = new System.Drawing.Point(387, 162);
            this.Dist1stTo2ndPosTextBox.Name = "Dist1stTo2ndPosTextBox";
            this.Dist1stTo2ndPosTextBox.Size = new System.Drawing.Size(121, 20);
            this.Dist1stTo2ndPosTextBox.TabIndex = 10;
            this.Dist1stTo2ndPosTextBox.TextChanged += new System.EventHandler(this.Dist1stTo2ndPosTextBox_TextChanged);
            // 
            // Dist2ndTo3rdPosTextBox
            // 
            this.Dist2ndTo3rdPosTextBox.Location = new System.Drawing.Point(387, 188);
            this.Dist2ndTo3rdPosTextBox.Name = "Dist2ndTo3rdPosTextBox";
            this.Dist2ndTo3rdPosTextBox.Size = new System.Drawing.Size(121, 20);
            this.Dist2ndTo3rdPosTextBox.TabIndex = 11;
            this.Dist2ndTo3rdPosTextBox.TextChanged += new System.EventHandler(this.Dist2ndTo3rdPosTextBox_TextChanged);
            // 
            // Dist4thTo5thPosTextBox
            // 
            this.Dist4thTo5thPosTextBox.Location = new System.Drawing.Point(387, 240);
            this.Dist4thTo5thPosTextBox.Name = "Dist4thTo5thPosTextBox";
            this.Dist4thTo5thPosTextBox.Size = new System.Drawing.Size(121, 20);
            this.Dist4thTo5thPosTextBox.TabIndex = 13;
            this.Dist4thTo5thPosTextBox.TextChanged += new System.EventHandler(this.Dist4thTo5thPosTextBox_TextChanged);
            // 
            // StepsPerInchTextBox
            // 
            this.StepsPerInchTextBox.Location = new System.Drawing.Point(387, 266);
            this.StepsPerInchTextBox.Name = "StepsPerInchTextBox";
            this.StepsPerInchTextBox.Size = new System.Drawing.Size(121, 20);
            this.StepsPerInchTextBox.TabIndex = 14;
            this.StepsPerInchTextBox.TextChanged += new System.EventHandler(this.StepsPerInchTextBox_TextChanged);
            // 
            // ForwardBitMaskTextBox
            // 
            this.ForwardBitMaskTextBox.Location = new System.Drawing.Point(387, 292);
            this.ForwardBitMaskTextBox.Name = "ForwardBitMaskTextBox";
            this.ForwardBitMaskTextBox.Size = new System.Drawing.Size(121, 20);
            this.ForwardBitMaskTextBox.TabIndex = 15;
            this.ForwardBitMaskTextBox.TextChanged += new System.EventHandler(this.ForwardBitMaskTextBox_TextChanged);
            // 
            // ReverseBitMaskTextBox
            // 
            this.ReverseBitMaskTextBox.Location = new System.Drawing.Point(387, 318);
            this.ReverseBitMaskTextBox.Name = "ReverseBitMaskTextBox";
            this.ReverseBitMaskTextBox.Size = new System.Drawing.Size(121, 20);
            this.ReverseBitMaskTextBox.TabIndex = 16;
            this.ReverseBitMaskTextBox.TextChanged += new System.EventHandler(this.ReverseBitMaskTextBox_TextChanged);
            // 
            // AxisNumberTextBox
            // 
            this.AxisNumberTextBox.Location = new System.Drawing.Point(387, 344);
            this.AxisNumberTextBox.Name = "AxisNumberTextBox";
            this.AxisNumberTextBox.Size = new System.Drawing.Size(121, 20);
            this.AxisNumberTextBox.TabIndex = 17;
            this.AxisNumberTextBox.TextChanged += new System.EventHandler(this.AxisNumberTextBox_TextChanged);
            // 
            // OverTravelPolarityTextBox
            // 
            this.OverTravelPolarityTextBox.Location = new System.Drawing.Point(387, 370);
            this.OverTravelPolarityTextBox.Name = "OverTravelPolarityTextBox";
            this.OverTravelPolarityTextBox.Size = new System.Drawing.Size(121, 20);
            this.OverTravelPolarityTextBox.TabIndex = 18;
            this.OverTravelPolarityTextBox.TextChanged += new System.EventHandler(this.OverTravelPolarityTextBox_TextChanged);
            // 
            // EncoderRatioTextBox
            // 
            this.EncoderRatioTextBox.Location = new System.Drawing.Point(387, 396);
            this.EncoderRatioTextBox.Name = "EncoderRatioTextBox";
            this.EncoderRatioTextBox.Size = new System.Drawing.Size(121, 20);
            this.EncoderRatioTextBox.TabIndex = 19;
            this.EncoderRatioTextBox.TextChanged += new System.EventHandler(this.EncoderRatioTextBox_TextChanged);
            // 
            // HomeSearchDistTextBox
            // 
            this.HomeSearchDistTextBox.Location = new System.Drawing.Point(387, 422);
            this.HomeSearchDistTextBox.Name = "HomeSearchDistTextBox";
            this.HomeSearchDistTextBox.Size = new System.Drawing.Size(121, 20);
            this.HomeSearchDistTextBox.TabIndex = 20;
            this.HomeSearchDistTextBox.TextChanged += new System.EventHandler(this.HomeSearchDistTextBox_TextChanged);
            // 
            // Dist3rdTo4thPosTextBox
            // 
            this.Dist3rdTo4thPosTextBox.Location = new System.Drawing.Point(387, 214);
            this.Dist3rdTo4thPosTextBox.Name = "Dist3rdTo4thPosTextBox";
            this.Dist3rdTo4thPosTextBox.Size = new System.Drawing.Size(121, 20);
            this.Dist3rdTo4thPosTextBox.TabIndex = 21;
            this.Dist3rdTo4thPosTextBox.TextChanged += new System.EventHandler(this.Dist3rdTo4thPosTextBox_TextChanged);
            // 
            // SerialPortLabel
            // 
            this.SerialPortLabel.AutoSize = true;
            this.SerialPortLabel.Location = new System.Drawing.Point(266, 15);
            this.SerialPortLabel.Name = "SerialPortLabel";
            this.SerialPortLabel.Size = new System.Drawing.Size(115, 13);
            this.SerialPortLabel.TabIndex = 22;
            this.SerialPortLabel.Text = "CompuMotor serial port";
            // 
            // HomeToForwardSwitchLabel
            // 
            this.HomeToForwardSwitchLabel.AutoSize = true;
            this.HomeToForwardSwitchLabel.Location = new System.Drawing.Point(104, 61);
            this.HomeToForwardSwitchLabel.Name = "HomeToForwardSwitchLabel";
            this.HomeToForwardSwitchLabel.Size = new System.Drawing.Size(277, 13);
            this.HomeToForwardSwitchLabel.TabIndex = 23;
            this.HomeToForwardSwitchLabel.Text = "Distance from home to forward over travel switch (inches)";
            // 
            // HomeToReverseSwitchLabel
            // 
            this.HomeToReverseSwitchLabel.AutoSize = true;
            this.HomeToReverseSwitchLabel.Location = new System.Drawing.Point(104, 87);
            this.HomeToReverseSwitchLabel.Name = "HomeToReverseSwitchLabel";
            this.HomeToReverseSwitchLabel.Size = new System.Drawing.Size(277, 13);
            this.HomeToReverseSwitchLabel.TabIndex = 24;
            this.HomeToReverseSwitchLabel.Text = "Distance from home to reverse over travel switch (inches)";
            // 
            // NumMeasPosLabel
            // 
            this.NumMeasPosLabel.AutoSize = true;
            this.NumMeasPosLabel.Location = new System.Drawing.Point(149, 113);
            this.NumMeasPosLabel.Name = "NumMeasPosLabel";
            this.NumMeasPosLabel.Size = new System.Drawing.Size(232, 13);
            this.NumMeasPosLabel.TabIndex = 25;
            this.NumMeasPosLabel.Text = "Number of Cf252 source measurement positions";
            // 
            // DistHomeTo1stPosLabel
            // 
            this.DistHomeTo1stPosLabel.AutoSize = true;
            this.DistHomeTo1stPosLabel.Location = new System.Drawing.Point(65, 139);
            this.DistHomeTo1stPosLabel.Name = "DistHomeTo1stPosLabel";
            this.DistHomeTo1stPosLabel.Size = new System.Drawing.Size(316, 13);
            this.DistHomeTo1stPosLabel.TabIndex = 26;
            this.DistHomeTo1stPosLabel.Text = "Distance to move Cf252 source from home to 1st position (inches)";
            // 
            // Dist1stTo2ndPosLabel
            // 
            this.Dist1stTo2ndPosLabel.AutoSize = true;
            this.Dist1stTo2ndPosLabel.Location = new System.Drawing.Point(34, 165);
            this.Dist1stTo2ndPosLabel.Name = "Dist1stTo2ndPosLabel";
            this.Dist1stTo2ndPosLabel.Size = new System.Drawing.Size(347, 13);
            this.Dist1stTo2ndPosLabel.TabIndex = 27;
            this.Dist1stTo2ndPosLabel.Text = "Distance to move Cf252 source from 1st position to 2nd position (inches)";
            // 
            // Dist2ndTo3rdPosLabel
            // 
            this.Dist2ndTo3rdPosLabel.AutoSize = true;
            this.Dist2ndTo3rdPosLabel.Location = new System.Drawing.Point(33, 191);
            this.Dist2ndTo3rdPosLabel.Name = "Dist2ndTo3rdPosLabel";
            this.Dist2ndTo3rdPosLabel.Size = new System.Drawing.Size(348, 13);
            this.Dist2ndTo3rdPosLabel.TabIndex = 28;
            this.Dist2ndTo3rdPosLabel.Text = "Distance to move Cf252 source from 2nd position to 3rd position (inches)";
            // 
            // Dist3rdTo4thPosLabel
            // 
            this.Dist3rdTo4thPosLabel.AutoSize = true;
            this.Dist3rdTo4thPosLabel.Location = new System.Drawing.Point(36, 217);
            this.Dist3rdTo4thPosLabel.Name = "Dist3rdTo4thPosLabel";
            this.Dist3rdTo4thPosLabel.Size = new System.Drawing.Size(345, 13);
            this.Dist3rdTo4thPosLabel.TabIndex = 29;
            this.Dist3rdTo4thPosLabel.Text = "Distance to move Cf252 source from 3rd position to 4th position (inches)";
            // 
            // Dist4thTo5thPosLabel
            // 
            this.Dist4thTo5thPosLabel.AutoSize = true;
            this.Dist4thTo5thPosLabel.Location = new System.Drawing.Point(33, 243);
            this.Dist4thTo5thPosLabel.Name = "Dist4thTo5thPosLabel";
            this.Dist4thTo5thPosLabel.Size = new System.Drawing.Size(348, 13);
            this.Dist4thTo5thPosLabel.TabIndex = 30;
            this.Dist4thTo5thPosLabel.Text = "Distance to move CF252 source from 4th position to 5th position (inches)";
            // 
            // StepsPerInchLabel
            // 
            this.StepsPerInchLabel.AutoSize = true;
            this.StepsPerInchLabel.Location = new System.Drawing.Point(245, 269);
            this.StepsPerInchLabel.Name = "StepsPerInchLabel";
            this.StepsPerInchLabel.Size = new System.Drawing.Size(136, 13);
            this.StepsPerInchLabel.TabIndex = 31;
            this.StepsPerInchLabel.Text = "CompuMotor steps per inch";
            // 
            // ForwardBitMaskLabel
            // 
            this.ForwardBitMaskLabel.AutoSize = true;
            this.ForwardBitMaskLabel.Location = new System.Drawing.Point(181, 295);
            this.ForwardBitMaskLabel.Name = "ForwardBitMaskLabel";
            this.ForwardBitMaskLabel.Size = new System.Drawing.Size(200, 13);
            this.ForwardBitMaskLabel.TabIndex = 32;
            this.ForwardBitMaskLabel.Text = "CompuMotor forward over travel bit mask";
            // 
            // ReverseBitMaskLabel
            // 
            this.ReverseBitMaskLabel.AutoSize = true;
            this.ReverseBitMaskLabel.Location = new System.Drawing.Point(181, 321);
            this.ReverseBitMaskLabel.Name = "ReverseBitMaskLabel";
            this.ReverseBitMaskLabel.Size = new System.Drawing.Size(200, 13);
            this.ReverseBitMaskLabel.TabIndex = 33;
            this.ReverseBitMaskLabel.Text = "CompuMotor reverse over travel bit mask";
            // 
            // AxisNumberLabel
            // 
            this.AxisNumberLabel.AutoSize = true;
            this.AxisNumberLabel.Location = new System.Drawing.Point(255, 347);
            this.AxisNumberLabel.Name = "AxisNumberLabel";
            this.AxisNumberLabel.Size = new System.Drawing.Size(126, 13);
            this.AxisNumberLabel.TabIndex = 34;
            this.AxisNumberLabel.Text = "CompuMotor axis number";
            // 
            // OverTravelPolarityLabel
            // 
            this.OverTravelPolarityLabel.AutoSize = true;
            this.OverTravelPolarityLabel.Location = new System.Drawing.Point(105, 373);
            this.OverTravelPolarityLabel.Name = "OverTravelPolarityLabel";
            this.OverTravelPolarityLabel.Size = new System.Drawing.Size(276, 13);
            this.OverTravelPolarityLabel.TabIndex = 35;
            this.OverTravelPolarityLabel.Text = "Bit state when CompuMotor at an over travel switch (0/1)";
            // 
            // EncoderRatioLabel
            // 
            this.EncoderRatioLabel.AutoSize = true;
            this.EncoderRatioLabel.Location = new System.Drawing.Point(105, 399);
            this.EncoderRatioLabel.Name = "EncoderRatioLabel";
            this.EncoderRatioLabel.Size = new System.Drawing.Size(276, 13);
            this.EncoderRatioLabel.TabIndex = 36;
            this.EncoderRatioLabel.Text = "CompuMotor steps commanded / encoder steps returned";
            // 
            // HomeSearchDistLabel
            // 
            this.HomeSearchDistLabel.AutoSize = true;
            this.HomeSearchDistLabel.Location = new System.Drawing.Point(10, 425);
            this.HomeSearchDistLabel.Name = "HomeSearchDistLabel";
            this.HomeSearchDistLabel.Size = new System.Drawing.Size(371, 13);
            this.HomeSearchDistLabel.TabIndex = 37;
            this.HomeSearchDistLabel.Text = "Distance to move Cf252 source at one time while searching for home (inches)";
            // 
            // IDDCmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 476);
            this.Controls.Add(this.HomeSearchDistLabel);
            this.Controls.Add(this.EncoderRatioLabel);
            this.Controls.Add(this.OverTravelPolarityLabel);
            this.Controls.Add(this.AxisNumberLabel);
            this.Controls.Add(this.ReverseBitMaskLabel);
            this.Controls.Add(this.ForwardBitMaskLabel);
            this.Controls.Add(this.StepsPerInchLabel);
            this.Controls.Add(this.Dist4thTo5thPosLabel);
            this.Controls.Add(this.Dist3rdTo4thPosLabel);
            this.Controls.Add(this.Dist2ndTo3rdPosLabel);
            this.Controls.Add(this.Dist1stTo2ndPosLabel);
            this.Controls.Add(this.DistHomeTo1stPosLabel);
            this.Controls.Add(this.NumMeasPosLabel);
            this.Controls.Add(this.HomeToReverseSwitchLabel);
            this.Controls.Add(this.HomeToForwardSwitchLabel);
            this.Controls.Add(this.SerialPortLabel);
            this.Controls.Add(this.Dist3rdTo4thPosTextBox);
            this.Controls.Add(this.HomeSearchDistTextBox);
            this.Controls.Add(this.EncoderRatioTextBox);
            this.Controls.Add(this.OverTravelPolarityTextBox);
            this.Controls.Add(this.AxisNumberTextBox);
            this.Controls.Add(this.ReverseBitMaskTextBox);
            this.Controls.Add(this.ForwardBitMaskTextBox);
            this.Controls.Add(this.StepsPerInchTextBox);
            this.Controls.Add(this.Dist4thTo5thPosTextBox);
            this.Controls.Add(this.Dist2ndTo3rdPosTextBox);
            this.Controls.Add(this.Dist1stTo2ndPosTextBox);
            this.Controls.Add(this.DistHomeTo1stPosTextBox);
            this.Controls.Add(this.NumMeasPosTextBox);
            this.Controls.Add(this.HomeToReverseSwitchTextBox);
            this.Controls.Add(this.HomeToForwardSwitchTextBox);
            this.Controls.Add(this.RotateDrumCheckBox);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SerialPortComboBox);
            this.Name = "IDDCmSetup";
            this.Text = "CompuMotor Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SerialPortComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.CheckBox RotateDrumCheckBox;
        private System.Windows.Forms.TextBox HomeToForwardSwitchTextBox;
        private System.Windows.Forms.TextBox HomeToReverseSwitchTextBox;
        private System.Windows.Forms.TextBox NumMeasPosTextBox;
        private System.Windows.Forms.TextBox DistHomeTo1stPosTextBox;
        private System.Windows.Forms.TextBox Dist1stTo2ndPosTextBox;
        private System.Windows.Forms.TextBox Dist2ndTo3rdPosTextBox;
        private System.Windows.Forms.TextBox Dist4thTo5thPosTextBox;
        private System.Windows.Forms.TextBox StepsPerInchTextBox;
        private System.Windows.Forms.TextBox ForwardBitMaskTextBox;
        private System.Windows.Forms.TextBox ReverseBitMaskTextBox;
        private System.Windows.Forms.TextBox AxisNumberTextBox;
        private System.Windows.Forms.TextBox OverTravelPolarityTextBox;
        private System.Windows.Forms.TextBox EncoderRatioTextBox;
        private System.Windows.Forms.TextBox HomeSearchDistTextBox;
        private System.Windows.Forms.TextBox Dist3rdTo4thPosTextBox;
        private System.Windows.Forms.Label SerialPortLabel;
        private System.Windows.Forms.Label HomeToForwardSwitchLabel;
        private System.Windows.Forms.Label HomeToReverseSwitchLabel;
        private System.Windows.Forms.Label NumMeasPosLabel;
        private System.Windows.Forms.Label DistHomeTo1stPosLabel;
        private System.Windows.Forms.Label Dist1stTo2ndPosLabel;
        private System.Windows.Forms.Label Dist2ndTo3rdPosLabel;
        private System.Windows.Forms.Label Dist3rdTo4thPosLabel;
        private System.Windows.Forms.Label Dist4thTo5thPosLabel;
        private System.Windows.Forms.Label StepsPerInchLabel;
        private System.Windows.Forms.Label ForwardBitMaskLabel;
        private System.Windows.Forms.Label ReverseBitMaskLabel;
        private System.Windows.Forms.Label AxisNumberLabel;
        private System.Windows.Forms.Label OverTravelPolarityLabel;
        private System.Windows.Forms.Label EncoderRatioLabel;
        private System.Windows.Forms.Label HomeSearchDistLabel;
    }
}