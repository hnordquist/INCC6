namespace NewUI
{
    partial class IDDCollarNormalization
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
            this.AmLiSourceIdTextBox = new System.Windows.Forms.TextBox();
            this.NeutronYieldTextBox = new NumericTextBox();
            this.NormalizationConstantTextBox = new NumericTextBox();
            this.NormalizationConstantErrorTextBox = new NumericTextBox();
            this.ReferenceSinglesRateTextBox = new NumericTextBox();
            this.AcceptanceLimitTextBox = new NumericTextBox();
            this.AmLiSourceIdLabel = new System.Windows.Forms.Label();
            this.NeutronYieldLabel = new System.Windows.Forms.Label();
            this.NormalizationConstantLabel = new System.Windows.Forms.Label();
            this.NormalizationConstantErrorLabel = new System.Windows.Forms.Label();
            this.ReferenceSinglesRateLabel = new System.Windows.Forms.Label();
            this.ReferenceSinglesDateLabel = new System.Windows.Forms.Label();
            this.AcceptanceLimitLabel = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.ReferenceSinglesDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // AmLiSourceIdTextBox
            // 
            this.AmLiSourceIdTextBox.Location = new System.Drawing.Point(187, 12);
            this.AmLiSourceIdTextBox.Name = "AmLiSourceIdTextBox";
            this.AmLiSourceIdTextBox.Size = new System.Drawing.Size(199, 20);
            this.AmLiSourceIdTextBox.TabIndex = 0;
            this.AmLiSourceIdTextBox.Leave += new System.EventHandler(this.AmLiSourceIdTextBox_Leave);
            // 
            // NeutronYieldTextBox
            // 
            this.NeutronYieldTextBox.Location = new System.Drawing.Point(187, 38);
            this.NeutronYieldTextBox.Name = "NeutronYieldTextBox";
            this.NeutronYieldTextBox.Size = new System.Drawing.Size(199, 20);
            this.NeutronYieldTextBox.TabIndex = 1;
            // 
            // NormalizationConstantTextBox
            // 
            this.NormalizationConstantTextBox.Location = new System.Drawing.Point(187, 64);
            this.NormalizationConstantTextBox.Name = "NormalizationConstantTextBox";
            this.NormalizationConstantTextBox.Size = new System.Drawing.Size(199, 20);
            this.NormalizationConstantTextBox.TabIndex = 2;
            // 
            // NormalizationConstantErrorTextBox
            // 
            this.NormalizationConstantErrorTextBox.Location = new System.Drawing.Point(187, 90);
            this.NormalizationConstantErrorTextBox.Name = "NormalizationConstantErrorTextBox";
            this.NormalizationConstantErrorTextBox.Size = new System.Drawing.Size(199, 20);
            this.NormalizationConstantErrorTextBox.TabIndex = 3;
            // 
            // ReferenceSinglesRateTextBox
            // 
            this.ReferenceSinglesRateTextBox.Location = new System.Drawing.Point(187, 116);
            this.ReferenceSinglesRateTextBox.Name = "ReferenceSinglesRateTextBox";
            this.ReferenceSinglesRateTextBox.Size = new System.Drawing.Size(199, 20);
            this.ReferenceSinglesRateTextBox.TabIndex = 4;
            // 
            // AcceptanceLimitTextBox
            // 
            this.AcceptanceLimitTextBox.Location = new System.Drawing.Point(187, 168);
            this.AcceptanceLimitTextBox.Name = "AcceptanceLimitTextBox";
            this.AcceptanceLimitTextBox.Size = new System.Drawing.Size(199, 20);
            this.AcceptanceLimitTextBox.TabIndex = 6;
            // 
            // AmLiSourceIdLabel
            // 
            this.AmLiSourceIdLabel.AutoSize = true;
            this.AmLiSourceIdLabel.Location = new System.Drawing.Point(95, 15);
            this.AmLiSourceIdLabel.Name = "AmLiSourceIdLabel";
            this.AmLiSourceIdLabel.Size = new System.Drawing.Size(76, 13);
            this.AmLiSourceIdLabel.TabIndex = 7;
            this.AmLiSourceIdLabel.Text = "AmLi source id";
            // 
            // NeutronYieldLabel
            // 
            this.NeutronYieldLabel.AutoSize = true;
            this.NeutronYieldLabel.Location = new System.Drawing.Point(11, 41);
            this.NeutronYieldLabel.Name = "NeutronYieldLabel";
            this.NeutronYieldLabel.Size = new System.Drawing.Size(160, 13);
            this.NeutronYieldLabel.TabIndex = 8;
            this.NeutronYieldLabel.Text = "Neutron yield relative to MRC-95";
            // 
            // NormalizationConstantLabel
            // 
            this.NormalizationConstantLabel.AutoSize = true;
            this.NormalizationConstantLabel.Location = new System.Drawing.Point(57, 67);
            this.NormalizationConstantLabel.Name = "NormalizationConstantLabel";
            this.NormalizationConstantLabel.Size = new System.Drawing.Size(114, 13);
            this.NormalizationConstantLabel.TabIndex = 9;
            this.NormalizationConstantLabel.Text = "Normalization constant";
            // 
            // NormalizationConstantErrorLabel
            // 
            this.NormalizationConstantErrorLabel.AutoSize = true;
            this.NormalizationConstantErrorLabel.Location = new System.Drawing.Point(33, 93);
            this.NormalizationConstantErrorLabel.Name = "NormalizationConstantErrorLabel";
            this.NormalizationConstantErrorLabel.Size = new System.Drawing.Size(138, 13);
            this.NormalizationConstantErrorLabel.TabIndex = 10;
            this.NormalizationConstantErrorLabel.Text = "Normalization constant error";
            // 
            // ReferenceSinglesRateLabel
            // 
            this.ReferenceSinglesRateLabel.AutoSize = true;
            this.ReferenceSinglesRateLabel.Location = new System.Drawing.Point(21, 119);
            this.ReferenceSinglesRateLabel.Name = "ReferenceSinglesRateLabel";
            this.ReferenceSinglesRateLabel.Size = new System.Drawing.Size(150, 13);
            this.ReferenceSinglesRateLabel.TabIndex = 11;
            this.ReferenceSinglesRateLabel.Text = "MRC-95 reference singles rate";
            // 
            // ReferenceSinglesDateLabel
            // 
            this.ReferenceSinglesDateLabel.AutoSize = true;
            this.ReferenceSinglesDateLabel.Location = new System.Drawing.Point(18, 148);
            this.ReferenceSinglesDateLabel.Name = "ReferenceSinglesDateLabel";
            this.ReferenceSinglesDateLabel.Size = new System.Drawing.Size(153, 13);
            this.ReferenceSinglesDateLabel.TabIndex = 12;
            this.ReferenceSinglesDateLabel.Text = "MRC-95 reference singles date";
            // 
            // AcceptanceLimitLabel
            // 
            this.AcceptanceLimitLabel.AutoSize = true;
            this.AcceptanceLimitLabel.Location = new System.Drawing.Point(69, 171);
            this.AcceptanceLimitLabel.Name = "AcceptanceLimitLabel";
            this.AcceptanceLimitLabel.Size = new System.Drawing.Size(102, 13);
            this.AcceptanceLimitLabel.TabIndex = 13;
            this.AcceptanceLimitLabel.Text = "Acceptance limit (%)";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(401, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 14;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(401, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 15;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(401, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 16;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // ReferenceSinglesDateTimePicker
            // 
            this.ReferenceSinglesDateTimePicker.Location = new System.Drawing.Point(187, 142);
            this.ReferenceSinglesDateTimePicker.Name = "ReferenceSinglesDateTimePicker";
            this.ReferenceSinglesDateTimePicker.Size = new System.Drawing.Size(199, 20);
            this.ReferenceSinglesDateTimePicker.TabIndex = 17;
            this.ReferenceSinglesDateTimePicker.ValueChanged += new System.EventHandler(this.ReferenceSinglesDateTimePicker_ValueChanged);
            // 
            // IDDCollarNormalization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 199);
            this.Controls.Add(this.ReferenceSinglesDateTimePicker);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.AcceptanceLimitLabel);
            this.Controls.Add(this.ReferenceSinglesDateLabel);
            this.Controls.Add(this.ReferenceSinglesRateLabel);
            this.Controls.Add(this.NormalizationConstantErrorLabel);
            this.Controls.Add(this.NormalizationConstantLabel);
            this.Controls.Add(this.NeutronYieldLabel);
            this.Controls.Add(this.AmLiSourceIdLabel);
            this.Controls.Add(this.AcceptanceLimitTextBox);
            this.Controls.Add(this.ReferenceSinglesRateTextBox);
            this.Controls.Add(this.NormalizationConstantErrorTextBox);
            this.Controls.Add(this.NormalizationConstantTextBox);
            this.Controls.Add(this.NeutronYieldTextBox);
            this.Controls.Add(this.AmLiSourceIdTextBox);
            this.Name = "IDDCollarNormalization";
            this.Text = "Collar Normalization Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AmLiSourceIdTextBox;
        private NumericTextBox NeutronYieldTextBox;
        private NumericTextBox NormalizationConstantTextBox;
        private NumericTextBox NormalizationConstantErrorTextBox;
        private NumericTextBox ReferenceSinglesRateTextBox;
        private NumericTextBox AcceptanceLimitTextBox;
        private System.Windows.Forms.Label AmLiSourceIdLabel;
        private System.Windows.Forms.Label NeutronYieldLabel;
        private System.Windows.Forms.Label NormalizationConstantLabel;
        private System.Windows.Forms.Label NormalizationConstantErrorLabel;
        private System.Windows.Forms.Label ReferenceSinglesRateLabel;
        private System.Windows.Forms.Label ReferenceSinglesDateLabel;
        private System.Windows.Forms.Label AcceptanceLimitLabel;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.DateTimePicker ReferenceSinglesDateTimePicker;
    }
}