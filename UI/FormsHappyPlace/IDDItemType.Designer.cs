namespace UI
{
    partial class IDDItemType
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
            this.AddMaterialTypeBtn = new System.Windows.Forms.Button();
            this.DeleteMaterialTypeBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddMaterialTypeBtn
            // 
            this.AddMaterialTypeBtn.Location = new System.Drawing.Point(16, 12);
            this.AddMaterialTypeBtn.Name = "AddMaterialTypeBtn";
            this.AddMaterialTypeBtn.Size = new System.Drawing.Size(121, 23);
            this.AddMaterialTypeBtn.TabIndex = 0;
            this.AddMaterialTypeBtn.Text = "Add material type...";
            this.AddMaterialTypeBtn.UseVisualStyleBackColor = true;
            this.AddMaterialTypeBtn.Click += new System.EventHandler(this.AddMaterialTypeBtn_Click);
            // 
            // DeleteMaterialTypeBtn
            // 
            this.DeleteMaterialTypeBtn.Location = new System.Drawing.Point(16, 68);
            this.DeleteMaterialTypeBtn.Name = "DeleteMaterialTypeBtn";
            this.DeleteMaterialTypeBtn.Size = new System.Drawing.Size(121, 23);
            this.DeleteMaterialTypeBtn.TabIndex = 1;
            this.DeleteMaterialTypeBtn.Text = "Delete material type...";
            this.DeleteMaterialTypeBtn.UseVisualStyleBackColor = true;
            this.DeleteMaterialTypeBtn.Click += new System.EventHandler(this.DeleteMaterialTypeBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(182, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(182, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(182, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDItemType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 109);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DeleteMaterialTypeBtn);
            this.Controls.Add(this.AddMaterialTypeBtn);
            this.Name = "IDDItemType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Material Type Add and Delete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddMaterialTypeBtn;
        private System.Windows.Forms.Button DeleteMaterialTypeBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}