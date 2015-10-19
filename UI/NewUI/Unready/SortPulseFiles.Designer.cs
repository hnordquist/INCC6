namespace NewUI
{
    partial class SortPulseFiles
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
            this.DirLabel = new System.Windows.Forms.Label();
            this.InputDirTextBox = new System.Windows.Forms.TextBox();
            this.InputDirBrowseBtn = new System.Windows.Forms.Button();
            this.OutputDirLabel = new System.Windows.Forms.Label();
            this.OutputDirTextBox = new System.Windows.Forms.TextBox();
            this.OutputDirBrowseBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DirLabel
            // 
            this.DirLabel.AutoSize = true;
            this.DirLabel.Location = new System.Drawing.Point(12, 18);
            this.DirLabel.Name = "DirLabel";
            this.DirLabel.Size = new System.Drawing.Size(194, 13);
            this.DirLabel.TabIndex = 0;
            this.DirLabel.Text = "Directory containing unsorted pulse files";
            // 
            // InputDirTextBox
            // 
            this.InputDirTextBox.Location = new System.Drawing.Point(212, 15);
            this.InputDirTextBox.Name = "InputDirTextBox";
            this.InputDirTextBox.Size = new System.Drawing.Size(346, 20);
            this.InputDirTextBox.TabIndex = 1;
            // 
            // InputDirBrowseBtn
            // 
            this.InputDirBrowseBtn.Location = new System.Drawing.Point(564, 13);
            this.InputDirBrowseBtn.Name = "InputDirBrowseBtn";
            this.InputDirBrowseBtn.Size = new System.Drawing.Size(29, 23);
            this.InputDirBrowseBtn.TabIndex = 2;
            this.InputDirBrowseBtn.Text = "...";
            this.InputDirBrowseBtn.UseVisualStyleBackColor = true;
            // 
            // OutputDirLabel
            // 
            this.OutputDirLabel.AutoSize = true;
            this.OutputDirLabel.Location = new System.Drawing.Point(31, 59);
            this.OutputDirLabel.Name = "OutputDirLabel";
            this.OutputDirLabel.Size = new System.Drawing.Size(175, 13);
            this.OutputDirLabel.TabIndex = 3;
            this.OutputDirLabel.Text = "Directory to place sorted output into";
            // 
            // OutputDirTextBox
            // 
            this.OutputDirTextBox.Location = new System.Drawing.Point(212, 56);
            this.OutputDirTextBox.Name = "OutputDirTextBox";
            this.OutputDirTextBox.Size = new System.Drawing.Size(346, 20);
            this.OutputDirTextBox.TabIndex = 4;
            // 
            // OutputDirBrowseBtn
            // 
            this.OutputDirBrowseBtn.Location = new System.Drawing.Point(564, 54);
            this.OutputDirBrowseBtn.Name = "OutputDirBrowseBtn";
            this.OutputDirBrowseBtn.Size = new System.Drawing.Size(29, 23);
            this.OutputDirBrowseBtn.TabIndex = 5;
            this.OutputDirBrowseBtn.Text = "...";
            this.OutputDirBrowseBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(644, 13);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(644, 54);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // SortPulseFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 94);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.OutputDirBrowseBtn);
            this.Controls.Add(this.OutputDirTextBox);
            this.Controls.Add(this.OutputDirLabel);
            this.Controls.Add(this.InputDirBrowseBtn);
            this.Controls.Add(this.InputDirTextBox);
            this.Controls.Add(this.DirLabel);
            this.Name = "SortPulseFiles";
            this.Text = "Select input and output locations for sorting pulse files";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DirLabel;
        private System.Windows.Forms.TextBox InputDirTextBox;
        private System.Windows.Forms.Button InputDirBrowseBtn;
        private System.Windows.Forms.Label OutputDirLabel;
        private System.Windows.Forms.TextBox OutputDirTextBox;
        private System.Windows.Forms.Button OutputDirBrowseBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}