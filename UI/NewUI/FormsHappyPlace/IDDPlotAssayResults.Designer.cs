namespace NewUI
{
    partial class IDDPlotAssayResults
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
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.AnalysisMethodLabel = new System.Windows.Forms.Label();
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.AnalysisMethodComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(26, 18);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 0;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // AnalysisMethodLabel
            // 
            this.AnalysisMethodLabel.AutoSize = true;
            this.AnalysisMethodLabel.Location = new System.Drawing.Point(12, 67);
            this.AnalysisMethodLabel.Name = "AnalysisMethodLabel";
            this.AnalysisMethodLabel.Size = new System.Drawing.Size(81, 13);
            this.AnalysisMethodLabel.TabIndex = 1;
            this.AnalysisMethodLabel.Text = "AnalysisMethod";
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(99, 15);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(163, 21);
            this.MaterialTypeComboBox.TabIndex = 2;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // AnalysisMethodComboBox
            // 
            this.AnalysisMethodComboBox.FormattingEnabled = true;
            this.AnalysisMethodComboBox.Location = new System.Drawing.Point(99, 64);
            this.AnalysisMethodComboBox.Name = "AnalysisMethodComboBox";
            this.AnalysisMethodComboBox.Size = new System.Drawing.Size(163, 21);
            this.AnalysisMethodComboBox.TabIndex = 3;
            this.AnalysisMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.AnalysisMethodComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(305, 13);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(305, 42);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(305, 71);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 6;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDPlotAssayResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 112);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.AnalysisMethodComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.AnalysisMethodLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Name = "IDDPlotAssayResults";
            this.Text = "Plot Calibration and Verification Results";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label AnalysisMethodLabel;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox AnalysisMethodComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}