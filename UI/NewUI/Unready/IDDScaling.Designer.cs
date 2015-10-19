namespace NewUI
{
    partial class IDDScaling
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
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.AutomaticScalingRadioButton = new System.Windows.Forms.RadioButton();
            this.ManualScalingRadioButton = new System.Windows.Forms.RadioButton();
            this.MinSinglesRateLabel = new System.Windows.Forms.Label();
            this.MaxSinglesRateLabel = new System.Windows.Forms.Label();
            this.MinDoublesRateLabel = new System.Windows.Forms.Label();
            this.MaxDoublesRateLabel = new System.Windows.Forms.Label();
            this.MinTriplesRateLabel = new System.Windows.Forms.Label();
            this.MaxTriplesRateLabel = new System.Windows.Forms.Label();
            this.MinSinglesRateTextBox = new System.Windows.Forms.TextBox();
            this.MaxSinglesRateTextBot = new System.Windows.Forms.TextBox();
            this.MinDoublesRateTextBox = new System.Windows.Forms.TextBox();
            this.MaxDoublesRateTextBox = new System.Windows.Forms.TextBox();
            this.MinTriplesRateTextBox = new System.Windows.Forms.TextBox();
            this.MaxTriplesRateTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.ManualScalingRadioButton);
            this.GroupBox1.Controls.Add(this.AutomaticScalingRadioButton);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(119, 75);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            // 
            // AutomaticScalingRadioButton
            // 
            this.AutomaticScalingRadioButton.AutoSize = true;
            this.AutomaticScalingRadioButton.Location = new System.Drawing.Point(6, 19);
            this.AutomaticScalingRadioButton.Name = "AutomaticScalingRadioButton";
            this.AutomaticScalingRadioButton.Size = new System.Drawing.Size(108, 17);
            this.AutomaticScalingRadioButton.TabIndex = 0;
            this.AutomaticScalingRadioButton.TabStop = true;
            this.AutomaticScalingRadioButton.Text = "Automatic scaling";
            this.AutomaticScalingRadioButton.UseVisualStyleBackColor = true;
            this.AutomaticScalingRadioButton.CheckedChanged += new System.EventHandler(this.AutomaticScalingRadioButton_CheckedChanged);
            // 
            // ManualScalingRadioButton
            // 
            this.ManualScalingRadioButton.AutoSize = true;
            this.ManualScalingRadioButton.Location = new System.Drawing.Point(6, 42);
            this.ManualScalingRadioButton.Name = "ManualScalingRadioButton";
            this.ManualScalingRadioButton.Size = new System.Drawing.Size(96, 17);
            this.ManualScalingRadioButton.TabIndex = 1;
            this.ManualScalingRadioButton.TabStop = true;
            this.ManualScalingRadioButton.Text = "Manual scaling";
            this.ManualScalingRadioButton.UseVisualStyleBackColor = true;
            this.ManualScalingRadioButton.CheckedChanged += new System.EventHandler(this.ManualScalingRadioButton_CheckedChanged);
            // 
            // MinSinglesRateLabel
            // 
            this.MinSinglesRateLabel.AutoSize = true;
            this.MinSinglesRateLabel.Location = new System.Drawing.Point(30, 119);
            this.MinSinglesRateLabel.Name = "MinSinglesRateLabel";
            this.MinSinglesRateLabel.Size = new System.Drawing.Size(104, 13);
            this.MinSinglesRateLabel.TabIndex = 1;
            this.MinSinglesRateLabel.Text = "Minimum singles rate";
            // 
            // MaxSinglesRateLabel
            // 
            this.MaxSinglesRateLabel.AutoSize = true;
            this.MaxSinglesRateLabel.Location = new System.Drawing.Point(27, 145);
            this.MaxSinglesRateLabel.Name = "MaxSinglesRateLabel";
            this.MaxSinglesRateLabel.Size = new System.Drawing.Size(107, 13);
            this.MaxSinglesRateLabel.TabIndex = 2;
            this.MaxSinglesRateLabel.Text = "Maximum singles rate";
            // 
            // MinDoublesRateLabel
            // 
            this.MinDoublesRateLabel.AutoSize = true;
            this.MinDoublesRateLabel.Location = new System.Drawing.Point(25, 171);
            this.MinDoublesRateLabel.Name = "MinDoublesRateLabel";
            this.MinDoublesRateLabel.Size = new System.Drawing.Size(109, 13);
            this.MinDoublesRateLabel.TabIndex = 3;
            this.MinDoublesRateLabel.Text = "Minimum doubles rate";
            // 
            // MaxDoublesRateLabel
            // 
            this.MaxDoublesRateLabel.AutoSize = true;
            this.MaxDoublesRateLabel.Location = new System.Drawing.Point(22, 197);
            this.MaxDoublesRateLabel.Name = "MaxDoublesRateLabel";
            this.MaxDoublesRateLabel.Size = new System.Drawing.Size(112, 13);
            this.MaxDoublesRateLabel.TabIndex = 4;
            this.MaxDoublesRateLabel.Text = "Maximum doubles rate";
            // 
            // MinTriplesRateLabel
            // 
            this.MinTriplesRateLabel.AutoSize = true;
            this.MinTriplesRateLabel.Location = new System.Drawing.Point(35, 223);
            this.MinTriplesRateLabel.Name = "MinTriplesRateLabel";
            this.MinTriplesRateLabel.Size = new System.Drawing.Size(99, 13);
            this.MinTriplesRateLabel.TabIndex = 5;
            this.MinTriplesRateLabel.Text = "Minimum triples rate";
            // 
            // MaxTriplesRateLabel
            // 
            this.MaxTriplesRateLabel.AutoSize = true;
            this.MaxTriplesRateLabel.Location = new System.Drawing.Point(32, 249);
            this.MaxTriplesRateLabel.Name = "MaxTriplesRateLabel";
            this.MaxTriplesRateLabel.Size = new System.Drawing.Size(102, 13);
            this.MaxTriplesRateLabel.TabIndex = 6;
            this.MaxTriplesRateLabel.Text = "Maximum triples rate";
            // 
            // MinSinglesRateTextBox
            // 
            this.MinSinglesRateTextBox.Location = new System.Drawing.Point(140, 116);
            this.MinSinglesRateTextBox.Name = "MinSinglesRateTextBox";
            this.MinSinglesRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinSinglesRateTextBox.TabIndex = 7;
            this.MinSinglesRateTextBox.TextChanged += new System.EventHandler(this.MinSinglesRateTextBox_TextChanged);
            // 
            // MaxSinglesRateTextBot
            // 
            this.MaxSinglesRateTextBot.Location = new System.Drawing.Point(140, 142);
            this.MaxSinglesRateTextBot.Name = "MaxSinglesRateTextBot";
            this.MaxSinglesRateTextBot.Size = new System.Drawing.Size(100, 20);
            this.MaxSinglesRateTextBot.TabIndex = 8;
            this.MaxSinglesRateTextBot.TextChanged += new System.EventHandler(this.MaxSinglesRateTextBot_TextChanged);
            // 
            // MinDoublesRateTextBox
            // 
            this.MinDoublesRateTextBox.Location = new System.Drawing.Point(140, 168);
            this.MinDoublesRateTextBox.Name = "MinDoublesRateTextBox";
            this.MinDoublesRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinDoublesRateTextBox.TabIndex = 9;
            this.MinDoublesRateTextBox.TextChanged += new System.EventHandler(this.MinDoublesRateTextBox_TextChanged);
            // 
            // MaxDoublesRateTextBox
            // 
            this.MaxDoublesRateTextBox.Location = new System.Drawing.Point(140, 194);
            this.MaxDoublesRateTextBox.Name = "MaxDoublesRateTextBox";
            this.MaxDoublesRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxDoublesRateTextBox.TabIndex = 10;
            this.MaxDoublesRateTextBox.TextChanged += new System.EventHandler(this.MaxDoublesRateTextBox_TextChanged);
            // 
            // MinTriplesRateTextBox
            // 
            this.MinTriplesRateTextBox.Location = new System.Drawing.Point(140, 220);
            this.MinTriplesRateTextBox.Name = "MinTriplesRateTextBox";
            this.MinTriplesRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinTriplesRateTextBox.TabIndex = 11;
            this.MinTriplesRateTextBox.TextChanged += new System.EventHandler(this.MinTriplesRateTextBox_TextChanged);
            // 
            // MaxTriplesRateTextBox
            // 
            this.MaxTriplesRateTextBox.Location = new System.Drawing.Point(140, 246);
            this.MaxTriplesRateTextBox.Name = "MaxTriplesRateTextBox";
            this.MaxTriplesRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxTriplesRateTextBox.TabIndex = 12;
            this.MaxTriplesRateTextBox.TextChanged += new System.EventHandler(this.MaxTriplesRateTextBox_TextChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(183, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 13;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(183, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 14;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(183, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 15;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDScaling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 286);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MaxTriplesRateTextBox);
            this.Controls.Add(this.MinTriplesRateTextBox);
            this.Controls.Add(this.MaxDoublesRateTextBox);
            this.Controls.Add(this.MinDoublesRateTextBox);
            this.Controls.Add(this.MaxSinglesRateTextBot);
            this.Controls.Add(this.MinSinglesRateTextBox);
            this.Controls.Add(this.MaxTriplesRateLabel);
            this.Controls.Add(this.MinTriplesRateLabel);
            this.Controls.Add(this.MaxDoublesRateLabel);
            this.Controls.Add(this.MinDoublesRateLabel);
            this.Controls.Add(this.MaxSinglesRateLabel);
            this.Controls.Add(this.MinSinglesRateLabel);
            this.Controls.Add(this.GroupBox1);
            this.Name = "IDDScaling";
            this.Text = "Singles, Doubles, and Triples Plot Scaling";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton ManualScalingRadioButton;
        private System.Windows.Forms.RadioButton AutomaticScalingRadioButton;
        private System.Windows.Forms.Label MinSinglesRateLabel;
        private System.Windows.Forms.Label MaxSinglesRateLabel;
        private System.Windows.Forms.Label MinDoublesRateLabel;
        private System.Windows.Forms.Label MaxDoublesRateLabel;
        private System.Windows.Forms.Label MinTriplesRateLabel;
        private System.Windows.Forms.Label MaxTriplesRateLabel;
        private System.Windows.Forms.TextBox MinSinglesRateTextBox;
        private System.Windows.Forms.TextBox MaxSinglesRateTextBot;
        private System.Windows.Forms.TextBox MinDoublesRateTextBox;
        private System.Windows.Forms.TextBox MaxDoublesRateTextBox;
        private System.Windows.Forms.TextBox MinTriplesRateTextBox;
        private System.Windows.Forms.TextBox MaxTriplesRateTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}