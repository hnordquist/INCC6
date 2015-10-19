namespace NewUI
{
    partial class FileSel
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.CancelBut = new System.Windows.Forms.Button();
            this.OkBut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 1);
            this.listBox1.MultiColumn = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(285, 212);
            this.listBox1.TabIndex = 0;
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point(13, 227);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(75, 23);
            this.CancelBut.TabIndex = 2;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // OkBut
            // 
            this.OkBut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBut.Location = new System.Drawing.Point(197, 227);
            this.OkBut.Name = "OkBut";
            this.OkBut.Size = new System.Drawing.Size(75, 23);
            this.OkBut.TabIndex = 1;
            this.OkBut.Text = "OK";
            this.OkBut.UseVisualStyleBackColor = true;
            // 
            // FileSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.OkBut);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.listBox1);
            this.Name = "FileSel";
            this.Text = "Files Selected for Analysis";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.Button OkBut;
    }
}