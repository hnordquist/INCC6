namespace NewUI
{
    partial class IDDNormalizationSetup
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
            this.CollarRadioButton = new System.Windows.Forms.RadioButton();
            this.UseAmLiRadioButton = new System.Windows.Forms.RadioButton();
            this.UseCf252SinglesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseCf252DoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.CollarRadioButton);
            this.GroupBox1.Controls.Add(this.UseAmLiRadioButton);
            this.GroupBox1.Controls.Add(this.UseCf252SinglesRadioButton);
            this.GroupBox1.Controls.Add(this.UseCf252DoublesRadioButton);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(312, 121);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            // 
            // CollarRadioButton
            // 
            this.CollarRadioButton.AutoSize = true;
            this.CollarRadioButton.Location = new System.Drawing.Point(15, 88);
            this.CollarRadioButton.Name = "CollarRadioButton";
            this.CollarRadioButton.Size = new System.Drawing.Size(135, 17);
            this.CollarRadioButton.TabIndex = 3;
            this.CollarRadioButton.Text = "Collar normalization test";
            this.CollarRadioButton.UseVisualStyleBackColor = true;
            this.CollarRadioButton.CheckedChanged += new System.EventHandler(this.CollarRadioButton_CheckedChanged);
            // 
            // UseAmLiRadioButton
            // 
            this.UseAmLiRadioButton.AutoSize = true;
            this.UseAmLiRadioButton.Location = new System.Drawing.Point(15, 65);
            this.UseAmLiRadioButton.Name = "UseAmLiRadioButton";
            this.UseAmLiRadioButton.Size = new System.Drawing.Size(204, 17);
            this.UseAmLiRadioButton.TabIndex = 2;
            this.UseAmLiRadioButton.TabStop = true;
            this.UseAmLiRadioButton.Text = "Use AmLi source for normalization test";
            this.UseAmLiRadioButton.UseVisualStyleBackColor = true;
            this.UseAmLiRadioButton.CheckedChanged += new System.EventHandler(this.UseAmLiRadioButton_CheckedChanged);
            // 
            // UseCf252SinglesRadioButton
            // 
            this.UseCf252SinglesRadioButton.AutoSize = true;
            this.UseCf252SinglesRadioButton.Location = new System.Drawing.Point(15, 42);
            this.UseCf252SinglesRadioButton.Name = "UseCf252SinglesRadioButton";
            this.UseCf252SinglesRadioButton.Size = new System.Drawing.Size(265, 17);
            this.UseCf252SinglesRadioButton.TabIndex = 1;
            this.UseCf252SinglesRadioButton.TabStop = true;
            this.UseCf252SinglesRadioButton.Text = "Use Cf252 source singles rate for normalization test";
            this.UseCf252SinglesRadioButton.UseVisualStyleBackColor = true;
            this.UseCf252SinglesRadioButton.CheckedChanged += new System.EventHandler(this.UseCf252SinglesRadioButton_CheckedChanged);
            // 
            // UseCf252DoublesRadioButton
            // 
            this.UseCf252DoublesRadioButton.AutoSize = true;
            this.UseCf252DoublesRadioButton.Location = new System.Drawing.Point(15, 19);
            this.UseCf252DoublesRadioButton.Name = "UseCf252DoublesRadioButton";
            this.UseCf252DoublesRadioButton.Size = new System.Drawing.Size(270, 17);
            this.UseCf252DoublesRadioButton.TabIndex = 0;
            this.UseCf252DoublesRadioButton.TabStop = true;
            this.UseCf252DoublesRadioButton.Text = "Use Cf252 source doubles rate for normalization test";
            this.UseCf252DoublesRadioButton.UseVisualStyleBackColor = true;
            this.UseCf252DoublesRadioButton.CheckedChanged += new System.EventHandler(this.UseCf252DoublesRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(330, 25);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(330, 54);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(330, 83);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDNormalizationSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 147);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.GroupBox1);
            this.Name = "IDDNormalizationSetup";
            this.Text = "Normalization Setup";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton CollarRadioButton;
        private System.Windows.Forms.RadioButton UseAmLiRadioButton;
        private System.Windows.Forms.RadioButton UseCf252SinglesRadioButton;
        private System.Windows.Forms.RadioButton UseCf252DoublesRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}