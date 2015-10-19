namespace NewUI
{
    partial class PrintForm
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
            this.PrintText = new System.Windows.Forms.RichTextBox();
            this.PrintParameters = new System.Windows.Forms.Button();
            this.SaveToFile = new System.Windows.Forms.Button();
            this.PrintDialog = new System.Windows.Forms.PrintDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // PrintText
            // 
            this.PrintText.Location = new System.Drawing.Point(4, 6);
            this.PrintText.Name = "PrintText";
            this.PrintText.Size = new System.Drawing.Size(803, 628);
            this.PrintText.TabIndex = 0;
            this.PrintText.Text = "";
            // 
            // PrintParameters
            // 
            this.PrintParameters.Location = new System.Drawing.Point(836, 69);
            this.PrintParameters.Name = "PrintParameters";
            this.PrintParameters.Size = new System.Drawing.Size(111, 23);
            this.PrintParameters.TabIndex = 1;
            this.PrintParameters.Text = "Print Parameters";
            this.PrintParameters.UseVisualStyleBackColor = true;
            this.PrintParameters.Click += new System.EventHandler(this.PrintParameters_Click);
            // 
            // SaveToFile
            // 
            this.SaveToFile.Location = new System.Drawing.Point(836, 110);
            this.SaveToFile.Name = "SaveToFile";
            this.SaveToFile.Size = new System.Drawing.Size(111, 23);
            this.SaveToFile.TabIndex = 2;
            this.SaveToFile.Text = "Save to File";
            this.SaveToFile.UseVisualStyleBackColor = true;
            this.SaveToFile.Click += new System.EventHandler(this.SaveToFile_Click);
            // 
            // PrintDialog
            // 
            this.PrintDialog.UseEXDialog = true;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 646);
            this.Controls.Add(this.SaveToFile);
            this.Controls.Add(this.PrintParameters);
            this.Controls.Add(this.PrintText);
            this.Name = "PrintForm";
            this.Text = "PrintForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox PrintText;
        private System.Windows.Forms.Button PrintParameters;
        private System.Windows.Forms.Button SaveToFile;
        private System.Windows.Forms.PrintDialog PrintDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}