namespace NewUI
{
    partial class IDDDBPicker
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
            this.TextBoxDatabaseFile = new System.Windows.Forms.TextBox();
            this.SelectBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.LabelDBName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TextBoxDatabaseFile
            // 
            this.TextBoxDatabaseFile.Location = new System.Drawing.Point(12, 42);
            this.TextBoxDatabaseFile.Name = "TextBoxDatabaseFile";
            this.TextBoxDatabaseFile.ReadOnly = true;
            this.TextBoxDatabaseFile.Size = new System.Drawing.Size(385, 20);
            this.TextBoxDatabaseFile.TabIndex = 0;
            // 
            // SelectBtn
            // 
            this.SelectBtn.Location = new System.Drawing.Point(422, 36);
            this.SelectBtn.Name = "SelectBtn";
            this.SelectBtn.Size = new System.Drawing.Size(55, 23);
            this.SelectBtn.TabIndex = 2;
            this.SelectBtn.Text = "Browse...";
            this.SelectBtn.UseVisualStyleBackColor = true;
            this.SelectBtn.Click += new System.EventHandler(this.SelectBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(422, 75);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(55, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(422, 108);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(55, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(422, 137);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(55, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // LabelDBName
            // 
            this.LabelDBName.AutoSize = true;
            this.LabelDBName.Location = new System.Drawing.Point(12, 26);
            this.LabelDBName.Name = "LabelDBName";
            this.LabelDBName.Size = new System.Drawing.Size(109, 13);
            this.LabelDBName.TabIndex = 7;
            this.LabelDBName.Text = "Current Database File";
            // 
            // IDDDBPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 186);
            this.Controls.Add(this.LabelDBName);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SelectBtn);
            this.Controls.Add(this.TextBoxDatabaseFile);
            this.Name = "IDDDBPicker";
            this.Text = "Choose a Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxDatabaseFile;
        private System.Windows.Forms.Button SelectBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label LabelDBName;
    }
}