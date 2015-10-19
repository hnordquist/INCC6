namespace NewUI
{
    partial class IDDGloveboxDelete
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.GloveboxIdLabel = new System.Windows.Forms.Label();
            this.GloveboxIdComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(327, 23);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(327, 52);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(327, 81);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // GloveboxIdLabel
            // 
            this.GloveboxIdLabel.AutoSize = true;
            this.GloveboxIdLabel.Location = new System.Drawing.Point(22, 28);
            this.GloveboxIdLabel.Name = "GloveboxIdLabel";
            this.GloveboxIdLabel.Size = new System.Drawing.Size(63, 13);
            this.GloveboxIdLabel.TabIndex = 3;
            this.GloveboxIdLabel.Text = "Glovebox id";
            // 
            // GloveboxIdComboBox
            // 
            this.GloveboxIdComboBox.FormattingEnabled = true;
            this.GloveboxIdComboBox.Location = new System.Drawing.Point(91, 25);
            this.GloveboxIdComboBox.Name = "GloveboxIdComboBox";
            this.GloveboxIdComboBox.Size = new System.Drawing.Size(205, 21);
            this.GloveboxIdComboBox.TabIndex = 4;
            this.GloveboxIdComboBox.SelectedIndexChanged += new System.EventHandler(this.GloveboxIdComboBox_SelectedIndexChanged);
            // 
            // IDDGloveboxDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 118);
            this.Controls.Add(this.GloveboxIdComboBox);
            this.Controls.Add(this.GloveboxIdLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDGloveboxDelete";
            this.Text = "Select a Glovebox to Delete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label GloveboxIdLabel;
        private System.Windows.Forms.ComboBox GloveboxIdComboBox;
    }
}