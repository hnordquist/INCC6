namespace UI
{
    partial class IDDAboutNCC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDAboutNCC));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NoticeBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CretidsBtn = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 266);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.NoticeBtn);
            this.groupBox2.Location = new System.Drawing.Point(268, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 266);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.MaximumSize = new System.Drawing.Size(230, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 26);
            this.label3.TabIndex = 7;
            this.label3.Text = "Please select the \'Notice\' button to read additional legal notices.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.MaximumSize = new System.Drawing.Size(230, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 52);
            this.label2.TabIndex = 6;
            this.label2.Text = "LANS has certain rights in the program pursuant to the contract, and the program " +
    "should not be copied or distributed outside your organization.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.MaximumSize = new System.Drawing.Size(230, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Copyright © Los Alamos National Security, LLC, (LANS), 2014.";
            // 
            // NoticeBtn
            // 
            this.NoticeBtn.Location = new System.Drawing.Point(92, 221);
            this.NoticeBtn.Name = "NoticeBtn";
            this.NoticeBtn.Size = new System.Drawing.Size(75, 23);
            this.NoticeBtn.TabIndex = 4;
            this.NoticeBtn.Text = "Notice";
            this.NoticeBtn.UseVisualStyleBackColor = true;
            this.NoticeBtn.Click += new System.EventHandler(this.NoticeBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(532, 17);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CretidsBtn
            // 
            this.CretidsBtn.Location = new System.Drawing.Point(532, 46);
            this.CretidsBtn.Name = "CretidsBtn";
            this.CretidsBtn.Size = new System.Drawing.Size(75, 23);
            this.CretidsBtn.TabIndex = 3;
            this.CretidsBtn.Text = "Credits";
            this.CretidsBtn.UseVisualStyleBackColor = true;
            this.CretidsBtn.Click += new System.EventHandler(this.CretidsBtn_Click);
            // 
            // IDDAboutNCC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 292);
            this.Controls.Add(this.CretidsBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "IDDAboutNCC";
            this.Text = "About Neutron Coincidence Counting";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button NoticeBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CretidsBtn;
    }
}