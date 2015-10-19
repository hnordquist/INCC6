namespace NewUI
{
    partial class IDDStratumIdAdd
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
            this.CurrentStratumIdsLabel = new System.Windows.Forms.Label();
            this.CurrentStratumIdsComboBox = new System.Windows.Forms.ComboBox();
            this.StratumIdLabel = new System.Windows.Forms.Label();
            this.StratumIdTextBox = new System.Windows.Forms.TextBox();
            this.StratumIdDescLabel = new System.Windows.Forms.Label();
            this.StratumIdDescTextBox = new System.Windows.Forms.TextBox();
            this.HistoricalBiasLabel = new System.Windows.Forms.Label();
            this.HistoricalBiasTextBox = new System.Windows.Forms.TextBox();
            this.RandomUncertaintyLabel = new System.Windows.Forms.Label();
            this.RandomUncertaintyTextBox = new System.Windows.Forms.TextBox();
            this.SystematicUncertaintyLabel = new System.Windows.Forms.Label();
            this.SystematicUncertaintyTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentStratumIdsLabel
            // 
            this.CurrentStratumIdsLabel.AutoSize = true;
            this.CurrentStratumIdsLabel.Location = new System.Drawing.Point(111, 16);
            this.CurrentStratumIdsLabel.Name = "CurrentStratumIdsLabel";
            this.CurrentStratumIdsLabel.Size = new System.Drawing.Size(94, 13);
            this.CurrentStratumIdsLabel.TabIndex = 0;
            this.CurrentStratumIdsLabel.Text = "Current stratum ids";
            // 
            // CurrentStratumIdsComboBox
            // 
            this.CurrentStratumIdsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CurrentStratumIdsComboBox.FormattingEnabled = true;
            this.CurrentStratumIdsComboBox.Location = new System.Drawing.Point(232, 12);
            this.CurrentStratumIdsComboBox.Name = "CurrentStratumIdsComboBox";
            this.CurrentStratumIdsComboBox.Size = new System.Drawing.Size(273, 21);
            this.CurrentStratumIdsComboBox.TabIndex = 1;
            this.CurrentStratumIdsComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentStratumIdsComboBox_SelectedIndexChanged);
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Location = new System.Drawing.Point(161, 102);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(54, 13);
            this.StratumIdLabel.TabIndex = 2;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // StratumIdTextBox
            // 
            this.StratumIdTextBox.Location = new System.Drawing.Point(232, 99);
            this.StratumIdTextBox.Name = "StratumIdTextBox";
            this.StratumIdTextBox.Size = new System.Drawing.Size(273, 20);
            this.StratumIdTextBox.TabIndex = 3;
            this.StratumIdTextBox.Leave += new System.EventHandler(this.StratumIdTextBox_Leave);
            // 
            // StratumIdDescLabel
            // 
            this.StratumIdDescLabel.AutoSize = true;
            this.StratumIdDescLabel.Location = new System.Drawing.Point(107, 128);
            this.StratumIdDescLabel.Name = "StratumIdDescLabel";
            this.StratumIdDescLabel.Size = new System.Drawing.Size(108, 13);
            this.StratumIdDescLabel.TabIndex = 4;
            this.StratumIdDescLabel.Text = "Stratum id description";
            // 
            // StratumIdDescTextBox
            // 
            this.StratumIdDescTextBox.Location = new System.Drawing.Point(232, 125);
            this.StratumIdDescTextBox.Name = "StratumIdDescTextBox";
            this.StratumIdDescTextBox.Size = new System.Drawing.Size(273, 20);
            this.StratumIdDescTextBox.TabIndex = 5;
            this.StratumIdDescTextBox.Leave += new System.EventHandler(this.StratumIdDescTextBox_Leave);
            // 
            // HistoricalBiasLabel
            // 
            this.HistoricalBiasLabel.AutoSize = true;
            this.HistoricalBiasLabel.Location = new System.Drawing.Point(99, 155);
            this.HistoricalBiasLabel.Name = "HistoricalBiasLabel";
            this.HistoricalBiasLabel.Size = new System.Drawing.Size(116, 13);
            this.HistoricalBiasLabel.TabIndex = 6;
            this.HistoricalBiasLabel.Text = "Historical bias (fraction)";
            // 
            // HistoricalBiasTextBox
            // 
            this.HistoricalBiasTextBox.Location = new System.Drawing.Point(232, 152);
            this.HistoricalBiasTextBox.Name = "HistoricalBiasTextBox";
            this.HistoricalBiasTextBox.Size = new System.Drawing.Size(76, 20);
            this.HistoricalBiasTextBox.TabIndex = 7;
            this.HistoricalBiasTextBox.Leave += new System.EventHandler(this.HistoricalBiasTextBox_Leave);
            // 
            // RandomUncertaintyLabel
            // 
            this.RandomUncertaintyLabel.AutoSize = true;
            this.RandomUncertaintyLabel.Location = new System.Drawing.Point(28, 181);
            this.RandomUncertaintyLabel.Name = "RandomUncertaintyLabel";
            this.RandomUncertaintyLabel.Size = new System.Drawing.Size(187, 13);
            this.RandomUncertaintyLabel.TabIndex = 8;
            this.RandomUncertaintyLabel.Text = "Historical random uncertainty (fraction)";
            // 
            // RandomUncertaintyTextBox
            // 
            this.RandomUncertaintyTextBox.Location = new System.Drawing.Point(232, 178);
            this.RandomUncertaintyTextBox.Name = "RandomUncertaintyTextBox";
            this.RandomUncertaintyTextBox.Size = new System.Drawing.Size(76, 20);
            this.RandomUncertaintyTextBox.TabIndex = 9;
            this.RandomUncertaintyTextBox.Leave += new System.EventHandler(this.RandomUncertaintyTextBox_Leave);
            // 
            // SystematicUncertaintyLabel
            // 
            this.SystematicUncertaintyLabel.AutoSize = true;
            this.SystematicUncertaintyLabel.Location = new System.Drawing.Point(14, 207);
            this.SystematicUncertaintyLabel.Name = "SystematicUncertaintyLabel";
            this.SystematicUncertaintyLabel.Size = new System.Drawing.Size(201, 13);
            this.SystematicUncertaintyLabel.TabIndex = 10;
            this.SystematicUncertaintyLabel.Text = "Historical systematic uncertainty (fraction)";
            // 
            // SystematicUncertaintyTextBox
            // 
            this.SystematicUncertaintyTextBox.Location = new System.Drawing.Point(232, 204);
            this.SystematicUncertaintyTextBox.Name = "SystematicUncertaintyTextBox";
            this.SystematicUncertaintyTextBox.Size = new System.Drawing.Size(76, 20);
            this.SystematicUncertaintyTextBox.TabIndex = 11;
            this.SystematicUncertaintyTextBox.Leave += new System.EventHandler(this.SystematicUncertaintyTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(536, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 12;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(536, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 13;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(536, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 14;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDStratumIdAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 237);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SystematicUncertaintyTextBox);
            this.Controls.Add(this.SystematicUncertaintyLabel);
            this.Controls.Add(this.RandomUncertaintyTextBox);
            this.Controls.Add(this.RandomUncertaintyLabel);
            this.Controls.Add(this.HistoricalBiasTextBox);
            this.Controls.Add(this.HistoricalBiasLabel);
            this.Controls.Add(this.StratumIdDescTextBox);
            this.Controls.Add(this.StratumIdDescLabel);
            this.Controls.Add(this.StratumIdTextBox);
            this.Controls.Add(this.StratumIdLabel);
            this.Controls.Add(this.CurrentStratumIdsComboBox);
            this.Controls.Add(this.CurrentStratumIdsLabel);
            this.Name = "IDDStratumIdAdd";
            this.Text = "Add a New Stratum id";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentStratumIdsLabel;
        private System.Windows.Forms.ComboBox CurrentStratumIdsComboBox;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.TextBox StratumIdTextBox;
        private System.Windows.Forms.Label StratumIdDescLabel;
        private System.Windows.Forms.TextBox StratumIdDescTextBox;
        private System.Windows.Forms.Label HistoricalBiasLabel;
        private System.Windows.Forms.TextBox HistoricalBiasTextBox;
        private System.Windows.Forms.Label RandomUncertaintyLabel;
        private System.Windows.Forms.TextBox RandomUncertaintyTextBox;
        private System.Windows.Forms.Label SystematicUncertaintyLabel;
        private System.Windows.Forms.TextBox SystematicUncertaintyTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}