namespace NewUI
{
    partial class IDDErrorCalcMethod
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
            this.SampleRadioButton = new System.Windows.Forms.RadioButton();
            this.TheoreticalRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SampleRadioButton);
            this.groupBox1.Controls.Add(this.TheoreticalRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(21, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // SampleRadioButton
            // 
            this.SampleRadioButton.AutoSize = true;
            this.SampleRadioButton.Location = new System.Drawing.Point(15, 47);
            this.SampleRadioButton.Name = "SampleRadioButton";
            this.SampleRadioButton.Size = new System.Drawing.Size(170, 17);
            this.SampleRadioButton.TabIndex = 1;
            this.SampleRadioButton.TabStop = true;
            this.SampleRadioButton.Text = "Use sample standard deviation";
            this.SampleRadioButton.UseVisualStyleBackColor = true;
            this.SampleRadioButton.CheckedChanged += new System.EventHandler(this.SampleRadioButton_CheckedChanged);
            // 
            // TheoreticalRadioButton
            // 
            this.TheoreticalRadioButton.AutoSize = true;
            this.TheoreticalRadioButton.Location = new System.Drawing.Point(15, 24);
            this.TheoreticalRadioButton.Name = "TheoreticalRadioButton";
            this.TheoreticalRadioButton.Size = new System.Drawing.Size(186, 17);
            this.TheoreticalRadioButton.TabIndex = 0;
            this.TheoreticalRadioButton.TabStop = true;
            this.TheoreticalRadioButton.Text = "Use theoretical standard deviation";
            this.TheoreticalRadioButton.UseVisualStyleBackColor = true;
            this.TheoreticalRadioButton.CheckedChanged += new System.EventHandler(this.TheoreticalRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(347, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(347, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(347, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDErrorCalcMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 106);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "IDDErrorCalcMethod";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Method of Calculating Singles, Doubles, and Triples Errors";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SampleRadioButton;
        private System.Windows.Forms.RadioButton TheoreticalRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}