namespace NewUI
{
    partial class IDDMBAEdit
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
            this.AddMBAButton = new System.Windows.Forms.Button();
            this.DeleteMBAButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // AddMBAButton
            // 
            this.AddMBAButton.Location = new System.Drawing.Point(12, 14);
            this.AddMBAButton.Name = "AddMBAButton";
            this.AddMBAButton.Size = new System.Drawing.Size(164, 23);
            this.AddMBAButton.TabIndex = 0;
            this.AddMBAButton.Text = "Add material balance area...";
            this.toolTip1.SetToolTip(this.AddMBAButton, "Click here to create a new material balance area. You will be prompted to type in" +
        " a new MBA name.");
            this.AddMBAButton.UseVisualStyleBackColor = true;
            this.AddMBAButton.Click += new System.EventHandler(this.AddMBAButton_Click);
            // 
            // DeleteMBAButton
            // 
            this.DeleteMBAButton.Location = new System.Drawing.Point(12, 57);
            this.DeleteMBAButton.Name = "DeleteMBAButton";
            this.DeleteMBAButton.Size = new System.Drawing.Size(164, 23);
            this.DeleteMBAButton.TabIndex = 1;
            this.DeleteMBAButton.Text = "Delete material balance area...";
            this.toolTip1.SetToolTip(this.DeleteMBAButton, "Click here to delete an existing material balance area.");
            this.DeleteMBAButton.UseVisualStyleBackColor = true;
            this.DeleteMBAButton.Click += new System.EventHandler(this.DeleteMBAButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(210, 14);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(210, 43);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(210, 72);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDMBAEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 112);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DeleteMBAButton);
            this.Controls.Add(this.AddMBAButton);
            this.Name = "IDDMBAEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Material Balance Area Add and Delete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddMBAButton;
        private System.Windows.Forms.Button DeleteMBAButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}