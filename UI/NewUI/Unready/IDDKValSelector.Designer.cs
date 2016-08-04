namespace NewUI
{
    partial class IDDKValSelector
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
            this.PANDARadioButton = new System.Windows.Forms.RadioButton();
            this.ESARDA64RadioButton = new System.Windows.Forms.RadioButton();
            this.ESARDA128RadioButton = new System.Windows.Forms.RadioButton();
            this.N9588RadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.N9588RadioButton);
            this.groupBox1.Controls.Add(this.ESARDA128RadioButton);
            this.groupBox1.Controls.Add(this.ESARDA64RadioButton);
            this.groupBox1.Controls.Add(this.PANDARadioButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 119);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // PANDARadioButton
            // 
            this.PANDARadioButton.AutoSize = true;
            this.PANDARadioButton.Location = new System.Drawing.Point(20, 19);
            this.PANDARadioButton.Name = "PANDARadioButton";
            this.PANDARadioButton.Size = new System.Drawing.Size(120, 17);
            this.PANDARadioButton.TabIndex = 0;
            this.PANDARadioButton.TabStop = true;
            this.PANDARadioButton.Text = "PANDA/App. Guide";
            this.PANDARadioButton.UseVisualStyleBackColor = true;
            // 
            // ESARDA64RadioButton
            // 
            this.ESARDA64RadioButton.AutoSize = true;
            this.ESARDA64RadioButton.Location = new System.Drawing.Point(20, 42);
            this.ESARDA64RadioButton.Name = "ESARDA64RadioButton";
            this.ESARDA64RadioButton.Size = new System.Drawing.Size(122, 17);
            this.ESARDA64RadioButton.TabIndex = 1;
            this.ESARDA64RadioButton.TabStop = true;
            this.ESARDA64RadioButton.Text = "ESARDA 1999 64µs";
            this.ESARDA64RadioButton.UseVisualStyleBackColor = true;
            // 
            // ESARDA128RadioButton
            // 
            this.ESARDA128RadioButton.AutoSize = true;
            this.ESARDA128RadioButton.Location = new System.Drawing.Point(20, 68);
            this.ESARDA128RadioButton.Name = "ESARDA128RadioButton";
            this.ESARDA128RadioButton.Size = new System.Drawing.Size(128, 17);
            this.ESARDA128RadioButton.TabIndex = 2;
            this.ESARDA128RadioButton.TabStop = true;
            this.ESARDA128RadioButton.Text = "ESARDA 1999 128µs";
            this.ESARDA128RadioButton.UseVisualStyleBackColor = true;
            // 
            // N9588RadioButton
            // 
            this.N9588RadioButton.AutoSize = true;
            this.N9588RadioButton.Location = new System.Drawing.Point(20, 91);
            this.N9588RadioButton.Name = "N9588RadioButton";
            this.N9588RadioButton.Size = new System.Drawing.Size(71, 17);
            this.N9588RadioButton.TabIndex = 3;
            this.N9588RadioButton.TabStop = true;
            this.N9588RadioButton.Text = "N95 88µs";
            this.N9588RadioButton.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(211, 23);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(211, 52);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDKValSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 157);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "IDDKValSelector";
            this.Text = "Pu240 Effective Coefficients";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton N9588RadioButton;
        private System.Windows.Forms.RadioButton ESARDA128RadioButton;
        private System.Windows.Forms.RadioButton ESARDA64RadioButton;
        private System.Windows.Forms.RadioButton PANDARadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}