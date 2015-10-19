namespace NewUI
{
    partial class IDDAmLiNormalization
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
            this.SourceIdTextBox = new System.Windows.Forms.TextBox();
            this.NormConstTextBox = new NumericTextBox();
            this.NormConstErrorTextBox = new NumericTextBox();
            this.AccLimitTextBox = new NumericTextBox();
            this.RefSinglesRateTextBox = new NumericTextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SourceIdLabel = new System.Windows.Forms.Label();
            this.NormConstLabel = new System.Windows.Forms.Label();
            this.NormConstErrorLabel = new System.Windows.Forms.Label();
            this.RefSinglesRateLabel = new System.Windows.Forms.Label();
            this.RefSinglesDateLabel = new System.Windows.Forms.Label();
            this.AccLimitLabel = new System.Windows.Forms.Label();
            this.RefSinglesDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // SourceIdTextBox
            // 
            this.SourceIdTextBox.Location = new System.Drawing.Point(162, 12);
            this.SourceIdTextBox.Name = "SourceIdTextBox";
            this.SourceIdTextBox.Size = new System.Drawing.Size(200, 20);
            this.SourceIdTextBox.TabIndex = 0;
            this.SourceIdTextBox.Leave += new System.EventHandler(this.SourceIdTextBox_Leave);
            // 
            // NormConstTextBox
            // 
            this.NormConstTextBox.Location = new System.Drawing.Point(162, 38);
            this.NormConstTextBox.Name = "NormConstTextBox";
            this.NormConstTextBox.Size = new System.Drawing.Size(200, 20);
            this.NormConstTextBox.TabIndex = 1;

            // 
            // NormConstErrorTextBox
            // 
            this.NormConstErrorTextBox.Location = new System.Drawing.Point(162, 64);
            this.NormConstErrorTextBox.Name = "NormConstErrorTextBox";
            this.NormConstErrorTextBox.Size = new System.Drawing.Size(200, 20);
            this.NormConstErrorTextBox.TabIndex = 2;

            // 
            // AccLimitTextBox
            // 
            this.AccLimitTextBox.Location = new System.Drawing.Point(162, 142);
            this.AccLimitTextBox.Name = "AccLimitTextBox";
            this.AccLimitTextBox.Size = new System.Drawing.Size(200, 20);
            this.AccLimitTextBox.TabIndex = 5;

            // 
            // RefSinglesRateTextBox
            // 
            this.RefSinglesRateTextBox.Location = new System.Drawing.Point(162, 90);
            this.RefSinglesRateTextBox.Name = "RefSinglesRateTextBox";
            this.RefSinglesRateTextBox.Size = new System.Drawing.Size(200, 20);
            this.RefSinglesRateTextBox.TabIndex = 6;

            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(374, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 7;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(374, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(374, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 9;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // SourceIdLabel
            // 
            this.SourceIdLabel.AutoSize = true;
            this.SourceIdLabel.Location = new System.Drawing.Point(73, 15);
            this.SourceIdLabel.Name = "SourceIdLabel";
            this.SourceIdLabel.Size = new System.Drawing.Size(76, 13);
            this.SourceIdLabel.TabIndex = 10;
            this.SourceIdLabel.Text = "AmLi source id";
            // 
            // NormConstLabel
            // 
            this.NormConstLabel.AutoSize = true;
            this.NormConstLabel.Location = new System.Drawing.Point(35, 41);
            this.NormConstLabel.Name = "NormConstLabel";
            this.NormConstLabel.Size = new System.Drawing.Size(114, 13);
            this.NormConstLabel.TabIndex = 11;
            this.NormConstLabel.Text = "Normalization constant";
            // 
            // NormConstErrorLabel
            // 
            this.NormConstErrorLabel.AutoSize = true;
            this.NormConstErrorLabel.Location = new System.Drawing.Point(11, 67);
            this.NormConstErrorLabel.Name = "NormConstErrorLabel";
            this.NormConstErrorLabel.Size = new System.Drawing.Size(138, 13);
            this.NormConstErrorLabel.TabIndex = 12;
            this.NormConstErrorLabel.Text = "Normalization constant error";
            // 
            // RefSinglesRateLabel
            // 
            this.RefSinglesRateLabel.AutoSize = true;
            this.RefSinglesRateLabel.Location = new System.Drawing.Point(15, 93);
            this.RefSinglesRateLabel.Name = "RefSinglesRateLabel";
            this.RefSinglesRateLabel.Size = new System.Drawing.Size(134, 13);
            this.RefSinglesRateLabel.TabIndex = 13;
            this.RefSinglesRateLabel.Text = "AmLi reference singles rate";
            // 
            // RefSinglesDateLabel
            // 
            this.RefSinglesDateLabel.AutoSize = true;
            this.RefSinglesDateLabel.Location = new System.Drawing.Point(12, 119);
            this.RefSinglesDateLabel.Name = "RefSinglesDateLabel";
            this.RefSinglesDateLabel.Size = new System.Drawing.Size(137, 13);
            this.RefSinglesDateLabel.TabIndex = 14;
            this.RefSinglesDateLabel.Text = "AmLi reference singles date";
            // 
            // AccLimitLabel
            // 
            this.AccLimitLabel.AutoSize = true;
            this.AccLimitLabel.Location = new System.Drawing.Point(47, 145);
            this.AccLimitLabel.Name = "AccLimitLabel";
            this.AccLimitLabel.Size = new System.Drawing.Size(102, 13);
            this.AccLimitLabel.TabIndex = 15;
            this.AccLimitLabel.Text = "Acceptance limit (%)";
            // 
            // RefSinglesDateTimePicker
            // 
            this.RefSinglesDateTimePicker.Location = new System.Drawing.Point(162, 116);
            this.RefSinglesDateTimePicker.Name = "RefSinglesDateTimePicker";
            this.RefSinglesDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.RefSinglesDateTimePicker.TabIndex = 16;
            this.RefSinglesDateTimePicker.ValueChanged += new System.EventHandler(this.RefSinglesDateTimePicker_ValueChanged);
            // 
            // IDDAmLiNormalization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 176);
            this.Controls.Add(this.RefSinglesDateTimePicker);
            this.Controls.Add(this.AccLimitLabel);
            this.Controls.Add(this.RefSinglesDateLabel);
            this.Controls.Add(this.RefSinglesRateLabel);
            this.Controls.Add(this.NormConstErrorLabel);
            this.Controls.Add(this.NormConstLabel);
            this.Controls.Add(this.SourceIdLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.RefSinglesRateTextBox);
            this.Controls.Add(this.AccLimitTextBox);
            this.Controls.Add(this.NormConstErrorTextBox);
            this.Controls.Add(this.NormConstTextBox);
            this.Controls.Add(this.SourceIdTextBox);
            this.Name = "IDDAmLiNormalization";
            this.Text = "AmLi Normalization Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SourceIdTextBox;
        private NumericTextBox NormConstTextBox;
        private NumericTextBox NormConstErrorTextBox;
        private NumericTextBox AccLimitTextBox;
        private NumericTextBox RefSinglesRateTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label SourceIdLabel;
        private System.Windows.Forms.Label NormConstLabel;
        private System.Windows.Forms.Label NormConstErrorLabel;
        private System.Windows.Forms.Label RefSinglesRateLabel;
        private System.Windows.Forms.Label RefSinglesDateLabel;
        private System.Windows.Forms.Label AccLimitLabel;
        private System.Windows.Forms.DateTimePicker RefSinglesDateTimePicker;
    }
}