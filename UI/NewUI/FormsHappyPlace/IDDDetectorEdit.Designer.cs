namespace NewUI
{
    partial class IDDDetectorEdit
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
            this.AddDetectorButton = new System.Windows.Forms.Button();
            this.DeleteDetectorButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddDetectorButton
            // 
            this.AddDetectorButton.Location = new System.Drawing.Point(12, 16);
            this.AddDetectorButton.Name = "AddDetectorButton";
            this.AddDetectorButton.Size = new System.Drawing.Size(99, 23);
            this.AddDetectorButton.TabIndex = 0;
            this.AddDetectorButton.Text = "Add detector...";
            this.AddDetectorButton.UseVisualStyleBackColor = true;
            this.AddDetectorButton.Click += new System.EventHandler(this.AddDetectorButton_Click);
            // 
            // DeleteDetectorButton
            // 
            this.DeleteDetectorButton.Location = new System.Drawing.Point(12, 74);
            this.DeleteDetectorButton.Name = "DeleteDetectorButton";
            this.DeleteDetectorButton.Size = new System.Drawing.Size(99, 23);
            this.DeleteDetectorButton.TabIndex = 1;
            this.DeleteDetectorButton.Text = "Delete detector...";
            this.DeleteDetectorButton.UseVisualStyleBackColor = true;
            this.DeleteDetectorButton.Click += new System.EventHandler(this.DeleteDetectorButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(136, 16);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(136, 45);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(136, 74);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDDetectorEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 115);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DeleteDetectorButton);
            this.Controls.Add(this.AddDetectorButton);
            this.Name = "IDDDetectorEdit";
            this.Text = "Add or Delete a Detector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddDetectorButton;
        private System.Windows.Forms.Button DeleteDetectorButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}