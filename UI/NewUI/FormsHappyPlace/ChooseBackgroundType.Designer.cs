namespace NewUI
{
    partial class ChooseBackgroundType
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
            this.DetectorConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.ActiveBackgroundButton = new System.Windows.Forms.RadioButton();
            this.PassiveBackgroundButton = new System.Windows.Forms.RadioButton();
            this.OKButton = new System.Windows.Forms.Button();
            this.DetectorConfigurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // DetectorConfigurationGroupBox
            // 
            this.DetectorConfigurationGroupBox.Controls.Add(this.ActiveBackgroundButton);
            this.DetectorConfigurationGroupBox.Controls.Add(this.PassiveBackgroundButton);
            this.DetectorConfigurationGroupBox.Location = new System.Drawing.Point(12, 25);
            this.DetectorConfigurationGroupBox.Name = "DetectorConfigurationGroupBox";
            this.DetectorConfigurationGroupBox.Size = new System.Drawing.Size(189, 107);
            this.DetectorConfigurationGroupBox.TabIndex = 0;
            this.DetectorConfigurationGroupBox.TabStop = false;
            this.DetectorConfigurationGroupBox.Text = "Detector Configuration";
            // 
            // ActiveBackgroundButton
            // 
            this.ActiveBackgroundButton.AutoSize = true;
            this.ActiveBackgroundButton.Location = new System.Drawing.Point(19, 58);
            this.ActiveBackgroundButton.Name = "ActiveBackgroundButton";
            this.ActiveBackgroundButton.Size = new System.Drawing.Size(116, 17);
            this.ActiveBackgroundButton.TabIndex = 1;
            this.ActiveBackgroundButton.TabStop = true;
            this.ActiveBackgroundButton.Text = "Active Background";
            this.ActiveBackgroundButton.UseVisualStyleBackColor = true;
            this.ActiveBackgroundButton.CheckedChanged += new System.EventHandler(this.ActiveBackgroundButton_CheckedChanged);
            // 
            // PassiveBackgroundButton
            // 
            this.PassiveBackgroundButton.AutoSize = true;
            this.PassiveBackgroundButton.Location = new System.Drawing.Point(19, 30);
            this.PassiveBackgroundButton.Name = "PassiveBackgroundButton";
            this.PassiveBackgroundButton.Size = new System.Drawing.Size(123, 17);
            this.PassiveBackgroundButton.TabIndex = 0;
            this.PassiveBackgroundButton.TabStop = true;
            this.PassiveBackgroundButton.Text = "Passive Background";
            this.PassiveBackgroundButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(272, 25);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // ChooseBackgroundType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 144);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DetectorConfigurationGroupBox);
            this.Name = "ChooseBackgroundType";
            this.Text = "ChooseBackgroundType";
            this.DetectorConfigurationGroupBox.ResumeLayout(false);
            this.DetectorConfigurationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox DetectorConfigurationGroupBox;
        private System.Windows.Forms.RadioButton ActiveBackgroundButton;
        private System.Windows.Forms.RadioButton PassiveBackgroundButton;
        private System.Windows.Forms.Button OKButton;
    }
}