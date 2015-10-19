namespace NewUI
{
    partial class IDDDualEnergyMult
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
            this.InnerTextBox = new System.Windows.Forms.TextBox();
            this.OuterTextBox = new System.Windows.Forms.TextBox();
            this.InnerLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // InnerTextBox
            // 
            this.InnerTextBox.Location = new System.Drawing.Point(117, 17);
            this.InnerTextBox.Name = "InnerTextBox";
            this.InnerTextBox.Size = new System.Drawing.Size(100, 20);
            this.InnerTextBox.TabIndex = 0;
            this.InnerTextBox.TextChanged += new System.EventHandler(this.InnerTextBox_TextChanged);
            // 
            // OuterTextBox
            // 
            this.OuterTextBox.Location = new System.Drawing.Point(374, 17);
            this.OuterTextBox.Name = "OuterTextBox";
            this.OuterTextBox.Size = new System.Drawing.Size(100, 20);
            this.OuterTextBox.TabIndex = 1;
            this.OuterTextBox.TextChanged += new System.EventHandler(this.OuterTextBox_TextChanged);
            // 
            // InnerLabel
            // 
            this.InnerLabel.AutoSize = true;
            this.InnerLabel.Location = new System.Drawing.Point(12, 20);
            this.InnerLabel.Name = "InnerLabel";
            this.InnerLabel.Size = new System.Drawing.Size(99, 13);
            this.InnerLabel.TabIndex = 2;
            this.InnerLabel.Text = "Inner ring efficiency";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Outer ring efficiency";
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(16, 462);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(107, 23);
            this.PrintBtn.TabIndex = 4;
            this.PrintBtn.Text = "Print calibration";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(238, 462);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(319, 462);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(400, 462);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 53);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(459, 394);
            this.listBox1.TabIndex = 9;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // IDDDualEnergyMult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 498);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InnerLabel);
            this.Controls.Add(this.OuterTextBox);
            this.Controls.Add(this.InnerTextBox);
            this.Name = "IDDDualEnergyMult";
            this.Text = "Dual Energy Multiplicity";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InnerTextBox;
        private System.Windows.Forms.TextBox OuterTextBox;
        private System.Windows.Forms.Label InnerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListBox listBox1;
    }
}