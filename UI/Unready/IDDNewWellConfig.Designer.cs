namespace UI
{
    partial class IDDNewWellConfig
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
            this.PassiveBackgroundRadioButton = new System.Windows.Forms.RadioButton();
            this.ActiveBackgroundRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.ActiveBackgroundRadioButton);
            this.GroupBox1.Controls.Add(this.PassiveBackgroundRadioButton);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(157, 75);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            // 
            // PassiveBackgroundRadioButton
            // 
            this.PassiveBackgroundRadioButton.AutoSize = true;
            this.PassiveBackgroundRadioButton.Location = new System.Drawing.Point(15, 19);
            this.PassiveBackgroundRadioButton.Name = "PassiveBackgroundRadioButton";
            this.PassiveBackgroundRadioButton.Size = new System.Drawing.Size(122, 17);
            this.PassiveBackgroundRadioButton.TabIndex = 0;
            this.PassiveBackgroundRadioButton.TabStop = true;
            this.PassiveBackgroundRadioButton.Text = "Passive background";
            this.PassiveBackgroundRadioButton.UseVisualStyleBackColor = true;
            this.PassiveBackgroundRadioButton.CheckedChanged += new System.EventHandler(this.PassiveBackgroundRadioButton_CheckedChanged);
            // 
            // ActiveBackgroundRadioButton
            // 
            this.ActiveBackgroundRadioButton.AutoSize = true;
            this.ActiveBackgroundRadioButton.Location = new System.Drawing.Point(15, 42);
            this.ActiveBackgroundRadioButton.Name = "ActiveBackgroundRadioButton";
            this.ActiveBackgroundRadioButton.Size = new System.Drawing.Size(115, 17);
            this.ActiveBackgroundRadioButton.TabIndex = 1;
            this.ActiveBackgroundRadioButton.TabStop = true;
            this.ActiveBackgroundRadioButton.Text = "Active background";
            this.ActiveBackgroundRadioButton.UseVisualStyleBackColor = true;
            this.ActiveBackgroundRadioButton.CheckedChanged += new System.EventHandler(this.ActiveBackgroundRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(187, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(187, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(187, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDNewWellConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 107);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.GroupBox1);
            this.Name = "IDDNewWellConfig";
            this.Text = "Detector Configuration";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton ActiveBackgroundRadioButton;
        private System.Windows.Forms.RadioButton PassiveBackgroundRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}