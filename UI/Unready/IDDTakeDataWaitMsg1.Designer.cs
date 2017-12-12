namespace UI
{
    partial class IDDTakeDataWaitMsg1
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
            this.AbortBtn = new System.Windows.Forms.Button();
            this.MoveBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // AbortBtn
            // 
            this.AbortBtn.Location = new System.Drawing.Point(237, 12);
            this.AbortBtn.Name = "AbortBtn";
            this.AbortBtn.Size = new System.Drawing.Size(198, 23);
            this.AbortBtn.TabIndex = 1;
            this.AbortBtn.Text = "Abort measurement";
            this.AbortBtn.UseVisualStyleBackColor = true;
            this.AbortBtn.Click += new System.EventHandler(this.AbortBtn_Click);
            // 
            // MoveBtn
            // 
            this.MoveBtn.Location = new System.Drawing.Point(12, 12);
            this.MoveBtn.Name = "MoveBtn";
            this.MoveBtn.Size = new System.Drawing.Size(198, 23);
            this.MoveBtn.TabIndex = 2;
            this.MoveBtn.Text = "Move to next measurement position";
            this.MoveBtn.UseVisualStyleBackColor = true;
            this.MoveBtn.Click += new System.EventHandler(this.MoveBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 25);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // IDDTakeDataWaitMsg1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 100);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MoveBtn);
            this.Controls.Add(this.AbortBtn);
            this.Name = "IDDTakeDataWaitMsg1";
            this.Text = "Acquiring Data From Shift Register";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AbortBtn;
        private System.Windows.Forms.Button MoveBtn;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}