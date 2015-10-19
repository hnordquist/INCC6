namespace NewUI
{
    partial class ImportINCC5
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
            this.RecomputeXferMeasCheckBox = new System.Windows.Forms.CheckBox();
            this.MassConstraintsCheckBox = new System.Windows.Forms.CheckBox();
            this.InputDirectoryLabel = new System.Windows.Forms.Label();
            this.RecurseCheckBox = new System.Windows.Forms.CheckBox();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.InputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RecomputeXferMeasCheckBox
            // 
            this.RecomputeXferMeasCheckBox.AutoSize = true;
            this.RecomputeXferMeasCheckBox.Location = new System.Drawing.Point(15, 118);
            this.RecomputeXferMeasCheckBox.Name = "RecomputeXferMeasCheckBox";
            this.RecomputeXferMeasCheckBox.Size = new System.Drawing.Size(256, 17);
            this.RecomputeXferMeasCheckBox.TabIndex = 12;
            this.RecomputeXferMeasCheckBox.Text = "Recompute imported INCC transfer measurement";
            this.RecomputeXferMeasCheckBox.UseVisualStyleBackColor = true;
            // 
            // MassConstraintsCheckBox
            // 
            this.MassConstraintsCheckBox.AutoSize = true;
            this.MassConstraintsCheckBox.Location = new System.Drawing.Point(15, 95);
            this.MassConstraintsCheckBox.Name = "MassConstraintsCheckBox";
            this.MassConstraintsCheckBox.Size = new System.Drawing.Size(166, 17);
            this.MassConstraintsCheckBox.TabIndex = 11;
            this.MassConstraintsCheckBox.Text = "Retain INCC mass constraints";
            this.MassConstraintsCheckBox.UseVisualStyleBackColor = true;
            // 
            // InputDirectoryLabel
            // 
            this.InputDirectoryLabel.AutoSize = true;
            this.InputDirectoryLabel.Location = new System.Drawing.Point(12, 9);
            this.InputDirectoryLabel.Name = "InputDirectoryLabel";
            this.InputDirectoryLabel.Size = new System.Drawing.Size(163, 13);
            this.InputDirectoryLabel.TabIndex = 47;
            this.InputDirectoryLabel.Text = "Directory containing transfer files:";
            // 
            // RecurseCheckBox
            // 
            this.RecurseCheckBox.AutoSize = true;
            this.RecurseCheckBox.Location = new System.Drawing.Point(15, 53);
            this.RecurseCheckBox.Name = "RecurseCheckBox";
            this.RecurseCheckBox.Size = new System.Drawing.Size(173, 17);
            this.RecurseCheckBox.TabIndex = 46;
            this.RecurseCheckBox.Text = "Recurse through subdirectories";
            this.RecurseCheckBox.UseVisualStyleBackColor = true;
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Location = new System.Drawing.Point(388, 25);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(29, 23);
            this.BrowseBtn.TabIndex = 45;
            this.BrowseBtn.Text = "...";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            // 
            // InputDirectoryTextBox
            // 
            this.InputDirectoryTextBox.Location = new System.Drawing.Point(15, 27);
            this.InputDirectoryTextBox.Name = "InputDirectoryTextBox";
            this.InputDirectoryTextBox.Size = new System.Drawing.Size(362, 20);
            this.InputDirectoryTextBox.TabIndex = 44;
            // 
            // ImportINCC5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 149);
            this.Controls.Add(this.InputDirectoryLabel);
            this.Controls.Add(this.RecurseCheckBox);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.InputDirectoryTextBox);
            this.Controls.Add(this.RecomputeXferMeasCheckBox);
            this.Controls.Add(this.MassConstraintsCheckBox);
            this.Name = "ImportINCC5";
            this.Text = "Import INCC 5 transfer files";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox RecomputeXferMeasCheckBox;
        private System.Windows.Forms.CheckBox MassConstraintsCheckBox;
        private System.Windows.Forms.Label InputDirectoryLabel;
        private System.Windows.Forms.CheckBox RecurseCheckBox;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.TextBox InputDirectoryTextBox;
    }
}