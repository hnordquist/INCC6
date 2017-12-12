namespace UI
{
    partial class FoldorFile
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
            this.ChooseFolder = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ChooseFiles = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChooseFolder
            // 
            this.ChooseFolder.Location = new System.Drawing.Point(12, 12);
            this.ChooseFolder.Name = "ChooseFolder";
            this.ChooseFolder.Size = new System.Drawing.Size(106, 23);
            this.ChooseFolder.TabIndex = 1;
            this.ChooseFolder.Text = "Use Folder";
            this.toolTip1.SetToolTip(this.ChooseFolder, "Use all data files in a folder as input");
            this.ChooseFolder.UseVisualStyleBackColor = true;
            this.ChooseFolder.Click += new System.EventHandler(this.ChooseFolder_Click);
            // 
            // ChooseFiles
            // 
            this.ChooseFiles.Location = new System.Drawing.Point(141, 12);
            this.ChooseFiles.Name = "ChooseFiles";
            this.ChooseFiles.Size = new System.Drawing.Size(106, 23);
            this.ChooseFiles.TabIndex = 0;
            this.ChooseFiles.Text = "Use Selected Files";
            this.toolTip1.SetToolTip(this.ChooseFiles, "Select one or more files as input");
            this.ChooseFiles.UseVisualStyleBackColor = true;
            this.ChooseFiles.Click += new System.EventHandler(this.ChooseFiles_Click);
            // 
            // FoldorFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 70);
            this.Controls.Add(this.ChooseFiles);
            this.Controls.Add(this.ChooseFolder);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FoldorFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose data file source";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ChooseFolder;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button ChooseFiles;
    }
}