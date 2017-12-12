namespace UI
{
    partial class IDDEnterIsotopics
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
            this.IsotopicsBtn = new System.Windows.Forms.Button();
            this.CompositeIsotopicsBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IsotopicsBtn
            // 
            this.IsotopicsBtn.Location = new System.Drawing.Point(12, 12);
            this.IsotopicsBtn.Name = "IsotopicsBtn";
            this.IsotopicsBtn.Size = new System.Drawing.Size(144, 23);
            this.IsotopicsBtn.TabIndex = 0;
            this.IsotopicsBtn.Text = "Isotopics...";
            this.IsotopicsBtn.UseVisualStyleBackColor = true;
            this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
            // 
            // CompositeIsotopicsBtn
            // 
            this.CompositeIsotopicsBtn.Location = new System.Drawing.Point(12, 70);
            this.CompositeIsotopicsBtn.Name = "CompositeIsotopicsBtn";
            this.CompositeIsotopicsBtn.Size = new System.Drawing.Size(144, 23);
            this.CompositeIsotopicsBtn.TabIndex = 1;
            this.CompositeIsotopicsBtn.Text = "Composite isotopics...";
            this.CompositeIsotopicsBtn.UseVisualStyleBackColor = true;
            this.CompositeIsotopicsBtn.Click += new System.EventHandler(this.CompositeIsotopicsBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(198, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(198, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(198, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDEnterIsotopics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 106);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CompositeIsotopicsBtn);
            this.Controls.Add(this.IsotopicsBtn);
            this.Name = "IDDEnterIsotopics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Isotopics For New Item";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button CompositeIsotopicsBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}