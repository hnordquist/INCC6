namespace NewUI
{
    partial class IDDTakeDataWaitMsg
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.quit = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.FieldLeft1 = new System.Windows.Forms.TextBox();
            this.FieldRight1 = new System.Windows.Forms.TextBox();
            this.FieldLeft2 = new System.Windows.Forms.TextBox();
            this.FieldRight2 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 26);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // quit
            // 
            this.quit.Location = new System.Drawing.Point(3, 3);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(175, 23);
            this.quit.TabIndex = 0;
            this.quit.Text = "Quit measurement with results";
            this.quit.UseVisualStyleBackColor = true;
            // 
            // abort
            // 
            this.abort.Location = new System.Drawing.Point(235, 3);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(147, 23);
            this.abort.TabIndex = 1;
            this.abort.Text = "Abort measurement";
            this.abort.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 2);
            this.progressBar1.Location = new System.Drawing.Point(3, 32);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(379, 45);
            this.progressBar1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.FieldLeft2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.FieldRight2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.quit, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.abort, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.FieldLeft1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.FieldRight1, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(385, 145);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // FieldLeft1
            // 
            this.FieldLeft1.Location = new System.Drawing.Point(3, 83);
            this.FieldLeft1.Name = "FieldLeft1";
            this.FieldLeft1.Size = new System.Drawing.Size(226, 20);
            this.FieldLeft1.TabIndex = 3;
            // 
            // FieldRight1
            // 
            this.FieldRight1.Location = new System.Drawing.Point(235, 83);
            this.FieldRight1.Name = "FieldRight1";
            this.FieldRight1.Size = new System.Drawing.Size(147, 20);
            this.FieldRight1.TabIndex = 4;
            // 
            // FieldLeft2
            // 
            this.FieldLeft2.Location = new System.Drawing.Point(3, 109);
            this.FieldLeft2.Name = "FieldLeft2";
            this.FieldLeft2.Size = new System.Drawing.Size(226, 20);
            this.FieldLeft2.TabIndex = 5;
            // 
            // FieldRight2
            // 
            this.FieldRight2.Location = new System.Drawing.Point(235, 109);
            this.FieldRight2.Name = "FieldRight2";
            this.FieldRight2.Size = new System.Drawing.Size(147, 20);
            this.FieldRight2.TabIndex = 6;
            // 
            // IDDTakeDataWaitMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 204);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "IDDTakeDataWaitMsg";
            this.Text = "Acquiring Data From Shift Register";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button quit;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox FieldLeft2;
        private System.Windows.Forms.TextBox FieldRight2;
        private System.Windows.Forms.TextBox FieldLeft1;
        private System.Windows.Forms.TextBox FieldRight1;
    }
}