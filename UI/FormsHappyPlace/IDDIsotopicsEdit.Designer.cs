﻿namespace UI
{
    partial class IDDIsotopicsEdit
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
            this.IsotopicsIdLabel = new System.Windows.Forms.Label();
            this.IsotopicsIdTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IsotopicsIdLabel
            // 
            this.IsotopicsIdLabel.AutoSize = true;
            this.IsotopicsIdLabel.Location = new System.Drawing.Point(31, 20);
            this.IsotopicsIdLabel.Name = "IsotopicsIdLabel";
            this.IsotopicsIdLabel.Size = new System.Drawing.Size(60, 13);
            this.IsotopicsIdLabel.TabIndex = 0;
            this.IsotopicsIdLabel.Text = "Isotopics id";
            // 
            // IsotopicsIdTextBox
            // 
            this.IsotopicsIdTextBox.Location = new System.Drawing.Point(97, 17);
            this.IsotopicsIdTextBox.Name = "IsotopicsIdTextBox";
            this.IsotopicsIdTextBox.Size = new System.Drawing.Size(185, 20);
            this.IsotopicsIdTextBox.TabIndex = 1;
            this.IsotopicsIdTextBox.Leave += new System.EventHandler(this.IsotopicsIdTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(27, 55);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(121, 55);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(216, 55);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDIsotopicsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 91);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.IsotopicsIdTextBox);
            this.Controls.Add(this.IsotopicsIdLabel);
            this.Name = "IDDIsotopicsEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter new id for current isotopics data set";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IsotopicsIdLabel;
        private System.Windows.Forms.TextBox IsotopicsIdTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}