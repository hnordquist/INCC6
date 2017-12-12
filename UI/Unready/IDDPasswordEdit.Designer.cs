namespace UI
{
    partial class IDDPasswordEdit
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
            this.AddPasswordBtn = new System.Windows.Forms.Button();
            this.DeletePasswordBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddPasswordBtn
            // 
            this.AddPasswordBtn.Location = new System.Drawing.Point(12, 12);
            this.AddPasswordBtn.Name = "AddPasswordBtn";
            this.AddPasswordBtn.Size = new System.Drawing.Size(104, 23);
            this.AddPasswordBtn.TabIndex = 0;
            this.AddPasswordBtn.Text = "Add password...";
            this.AddPasswordBtn.UseVisualStyleBackColor = true;
            this.AddPasswordBtn.Click += new System.EventHandler(this.AddPasswordBtn_Click);
            // 
            // DeletePasswordBtn
            // 
            this.DeletePasswordBtn.Location = new System.Drawing.Point(12, 41);
            this.DeletePasswordBtn.Name = "DeletePasswordBtn";
            this.DeletePasswordBtn.Size = new System.Drawing.Size(104, 23);
            this.DeletePasswordBtn.TabIndex = 1;
            this.DeletePasswordBtn.Text = "Delete password...";
            this.DeletePasswordBtn.UseVisualStyleBackColor = true;
            this.DeletePasswordBtn.Click += new System.EventHandler(this.DeletePasswordBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(145, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(145, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(145, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDPasswordEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 104);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DeletePasswordBtn);
            this.Controls.Add(this.AddPasswordBtn);
            this.Name = "IDDPasswordEdit";
            this.Text = "Password Add and Delete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddPasswordBtn;
        private System.Windows.Forms.Button DeletePasswordBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}