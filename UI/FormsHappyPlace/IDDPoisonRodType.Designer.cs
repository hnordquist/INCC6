namespace UI
{
    partial class IDDPoisonRodType
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
            this.AddPoisonRodTypeBtn = new System.Windows.Forms.Button();
            this.DeletePoisonRodTypeBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddPoisonRodTypeBtn
            // 
            this.AddPoisonRodTypeBtn.Location = new System.Drawing.Point(12, 12);
            this.AddPoisonRodTypeBtn.Name = "AddPoisonRodTypeBtn";
            this.AddPoisonRodTypeBtn.Size = new System.Drawing.Size(136, 23);
            this.AddPoisonRodTypeBtn.TabIndex = 0;
            this.AddPoisonRodTypeBtn.Text = "Add poison rod type...";
            this.AddPoisonRodTypeBtn.UseVisualStyleBackColor = true;
            this.AddPoisonRodTypeBtn.Click += new System.EventHandler(this.AddPoisonRodTypeBtn_Click);
            // 
            // DeletePoisonRodTypeBtn
            // 
            this.DeletePoisonRodTypeBtn.Location = new System.Drawing.Point(12, 52);
            this.DeletePoisonRodTypeBtn.Name = "DeletePoisonRodTypeBtn";
            this.DeletePoisonRodTypeBtn.Size = new System.Drawing.Size(136, 23);
            this.DeletePoisonRodTypeBtn.TabIndex = 1;
            this.DeletePoisonRodTypeBtn.Text = "Delete poison rod type...";
            this.DeletePoisonRodTypeBtn.UseVisualStyleBackColor = true;
            this.DeletePoisonRodTypeBtn.Click += new System.EventHandler(this.DeletePoisonRodTypeBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(185, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(185, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(185, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDPoisonRodType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 105);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DeletePoisonRodTypeBtn);
            this.Controls.Add(this.AddPoisonRodTypeBtn);
            this.Name = "IDDPoisonRodType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Poison Rod Type Add and Delete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddPoisonRodTypeBtn;
        private System.Windows.Forms.Button DeletePoisonRodTypeBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}