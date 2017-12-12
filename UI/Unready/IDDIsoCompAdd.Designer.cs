namespace UI
{
    partial class IDDIsoCompAdd
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
            this.CompositeIsotopicsIdLabel = new System.Windows.Forms.Label();
            this.CompositeIsotopicsIdTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CompositeIsotopicsIdLabel
            // 
            this.CompositeIsotopicsIdLabel.AutoSize = true;
            this.CompositeIsotopicsIdLabel.Location = new System.Drawing.Point(21, 18);
            this.CompositeIsotopicsIdLabel.Name = "CompositeIsotopicsIdLabel";
            this.CompositeIsotopicsIdLabel.Size = new System.Drawing.Size(111, 13);
            this.CompositeIsotopicsIdLabel.TabIndex = 0;
            this.CompositeIsotopicsIdLabel.Text = "Composite isotopics id";
            // 
            // CompositeIsotopicsIdTextBox
            // 
            this.CompositeIsotopicsIdTextBox.Location = new System.Drawing.Point(138, 15);
            this.CompositeIsotopicsIdTextBox.Name = "CompositeIsotopicsIdTextBox";
            this.CompositeIsotopicsIdTextBox.Size = new System.Drawing.Size(135, 20);
            this.CompositeIsotopicsIdTextBox.TabIndex = 1;
            this.CompositeIsotopicsIdTextBox.TextChanged += new System.EventHandler(this.CompositeIsotopicsIdTextBox_TextChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(24, 52);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(111, 52);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(198, 52);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDIsoCompAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 96);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CompositeIsotopicsIdTextBox);
            this.Controls.Add(this.CompositeIsotopicsIdLabel);
            this.Name = "IDDIsoCompAdd";
            this.Text = "Enter id for new composite isotopics data set";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CompositeIsotopicsIdLabel;
        private System.Windows.Forms.TextBox CompositeIsotopicsIdTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}