namespace NewUI
{
    partial class IDDGlovebox
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
            this.AddGloveboxBtn = new System.Windows.Forms.Button();
            this.EditGloveboxBtn = new System.Windows.Forms.Button();
            this.DeleteGloveboxBtn = new System.Windows.Forms.Button();
            this.DisplayConfBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PrintCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // AddGloveboxBtn
            // 
            this.AddGloveboxBtn.Location = new System.Drawing.Point(22, 24);
            this.AddGloveboxBtn.Name = "AddGloveboxBtn";
            this.AddGloveboxBtn.Size = new System.Drawing.Size(170, 23);
            this.AddGloveboxBtn.TabIndex = 0;
            this.AddGloveboxBtn.Text = "Add glovebox...";
            this.AddGloveboxBtn.UseVisualStyleBackColor = true;
            this.AddGloveboxBtn.Click += new System.EventHandler(this.AddGloveboxBtn_Click);
            // 
            // EditGloveboxBtn
            // 
            this.EditGloveboxBtn.Location = new System.Drawing.Point(22, 53);
            this.EditGloveboxBtn.Name = "EditGloveboxBtn";
            this.EditGloveboxBtn.Size = new System.Drawing.Size(170, 23);
            this.EditGloveboxBtn.TabIndex = 1;
            this.EditGloveboxBtn.Text = "Edit an existing glovebox...";
            this.EditGloveboxBtn.UseVisualStyleBackColor = true;
            this.EditGloveboxBtn.Click += new System.EventHandler(this.EditGloveboxBtn_Click);
            // 
            // DeleteGloveboxBtn
            // 
            this.DeleteGloveboxBtn.Location = new System.Drawing.Point(22, 82);
            this.DeleteGloveboxBtn.Name = "DeleteGloveboxBtn";
            this.DeleteGloveboxBtn.Size = new System.Drawing.Size(170, 23);
            this.DeleteGloveboxBtn.TabIndex = 2;
            this.DeleteGloveboxBtn.Text = "Delete glovebox...";
            this.DeleteGloveboxBtn.UseVisualStyleBackColor = true;
            this.DeleteGloveboxBtn.Click += new System.EventHandler(this.DeleteGloveboxBtn_Click);
            // 
            // DisplayConfBtn
            // 
            this.DisplayConfBtn.Location = new System.Drawing.Point(22, 111);
            this.DisplayConfBtn.Name = "DisplayConfBtn";
            this.DisplayConfBtn.Size = new System.Drawing.Size(170, 23);
            this.DisplayConfBtn.TabIndex = 3;
            this.DisplayConfBtn.Text = "Display glovebox configuration";
            this.DisplayConfBtn.UseVisualStyleBackColor = true;
            this.DisplayConfBtn.Click += new System.EventHandler(this.DisplayConfBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(237, 24);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(237, 53);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(237, 82);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 6;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PrintCheckBox
            // 
            this.PrintCheckBox.AutoSize = true;
            this.PrintCheckBox.Location = new System.Drawing.Point(22, 151);
            this.PrintCheckBox.Name = "PrintCheckBox";
            this.PrintCheckBox.Size = new System.Drawing.Size(233, 17);
            this.PrintCheckBox.TabIndex = 7;
            this.PrintCheckBox.Text = "Print glovebox configuration when displayed";
            this.PrintCheckBox.UseVisualStyleBackColor = true;
            this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
            // 
            // IDDGlovebox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 180);
            this.Controls.Add(this.PrintCheckBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DisplayConfBtn);
            this.Controls.Add(this.DeleteGloveboxBtn);
            this.Controls.Add(this.EditGloveboxBtn);
            this.Controls.Add(this.AddGloveboxBtn);
            this.Name = "IDDGlovebox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Glovebox Add, Edit, and Delete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddGloveboxBtn;
        private System.Windows.Forms.Button EditGloveboxBtn;
        private System.Windows.Forms.Button DeleteGloveboxBtn;
        private System.Windows.Forms.Button DisplayConfBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.CheckBox PrintCheckBox;
    }
}