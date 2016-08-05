﻿namespace NewUI
{
    partial class IDDStratumId
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
            this.AddStratumIdBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.DeleteStratumIdBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddStratumIdBtn
            // 
            this.AddStratumIdBtn.Location = new System.Drawing.Point(12, 12);
            this.AddStratumIdBtn.Name = "AddStratumIdBtn";
            this.AddStratumIdBtn.Size = new System.Drawing.Size(131, 23);
            this.AddStratumIdBtn.TabIndex = 0;
            this.AddStratumIdBtn.Text = "Add stratum id...";
            this.AddStratumIdBtn.UseVisualStyleBackColor = true;
            this.AddStratumIdBtn.Click += new System.EventHandler(this.AddStratumIdBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(209, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(209, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(209, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // DeleteStratumIdBtn
            // 
            this.DeleteStratumIdBtn.Location = new System.Drawing.Point(12, 70);
            this.DeleteStratumIdBtn.Name = "DeleteStratumIdBtn";
            this.DeleteStratumIdBtn.Size = new System.Drawing.Size(131, 23);
            this.DeleteStratumIdBtn.TabIndex = 5;
            this.DeleteStratumIdBtn.Text = "Delete stratum id...";
            this.DeleteStratumIdBtn.UseVisualStyleBackColor = true;
            this.DeleteStratumIdBtn.Click += new System.EventHandler(this.DeleteStratumIdBtn_Click);
            // 
            // IDDStratumId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 105);
            this.Controls.Add(this.DeleteStratumIdBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.AddStratumIdBtn);
            this.Name = "IDDStratumId";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stratum id Add and Delete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddStratumIdBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button DeleteStratumIdBtn;
    }
}