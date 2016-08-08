namespace NewUI
{
    partial class IDDFacilityEdit
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
            this.components = new System.ComponentModel.Container();
            this.AddFacilityButton = new System.Windows.Forms.Button();
            this.DeleteFacilityButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // AddFacilityButton
            // 
            this.AddFacilityButton.Location = new System.Drawing.Point(12, 12);
            this.AddFacilityButton.Name = "AddFacilityButton";
            this.AddFacilityButton.Size = new System.Drawing.Size(104, 23);
            this.AddFacilityButton.TabIndex = 0;
            this.AddFacilityButton.Text = "Add facility...";
            this.toolTip1.SetToolTip(this.AddFacilityButton, "Click here to create a new facility. You will be prompted to type in a new facili" +
        "ty name.");
            this.AddFacilityButton.UseVisualStyleBackColor = true;
            this.AddFacilityButton.Click += new System.EventHandler(this.AddFacilityButton_Click);
            // 
            // DeleteFacilityButton
            // 
            this.DeleteFacilityButton.Location = new System.Drawing.Point(12, 53);
            this.DeleteFacilityButton.Name = "DeleteFacilityButton";
            this.DeleteFacilityButton.Size = new System.Drawing.Size(104, 23);
            this.DeleteFacilityButton.TabIndex = 1;
            this.DeleteFacilityButton.Text = "Delete facility...";
            this.toolTip1.SetToolTip(this.DeleteFacilityButton, "Click here to delete an existing facility.");
            this.DeleteFacilityButton.UseVisualStyleBackColor = true;
            this.DeleteFacilityButton.Click += new System.EventHandler(this.DeleteFacilityButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(165, 12);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(165, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(165, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDFacilityEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 104);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DeleteFacilityButton);
            this.Controls.Add(this.AddFacilityButton);
            this.Name = "IDDFacilityEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Facility Add and Delete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddFacilityButton;
        private System.Windows.Forms.Button DeleteFacilityButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}