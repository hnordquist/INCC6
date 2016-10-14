namespace NewUI
{
    partial class IDDEndingComment
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
			this.EndingCommentLabel = new System.Windows.Forms.Label();
			this.EndingCommentTextBox = new System.Windows.Forms.TextBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.Lines = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(573, 13);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 0;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(573, 48);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(573, 84);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 2;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// EndingCommentLabel
			// 
			this.EndingCommentLabel.AutoSize = true;
			this.EndingCommentLabel.Location = new System.Drawing.Point(13, 234);
			this.EndingCommentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.EndingCommentLabel.Name = "EndingCommentLabel";
			this.EndingCommentLabel.Size = new System.Drawing.Size(113, 17);
			this.EndingCommentLabel.TabIndex = 21;
			this.EndingCommentLabel.Text = "Ending comment";
			// 
			// EndingCommentTextBox
			// 
			this.EndingCommentTextBox.Location = new System.Drawing.Point(136, 230);
			this.EndingCommentTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.EndingCommentTextBox.Name = "EndingCommentTextBox";
			this.EndingCommentTextBox.Size = new System.Drawing.Size(426, 22);
			this.EndingCommentTextBox.TabIndex = 22;
			this.EndingCommentTextBox.Leave += new System.EventHandler(this.EndingCommentTextBox_Leave);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Lines});
			this.listView1.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listView1.Location = new System.Drawing.Point(12, 13);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(549, 200);
			this.listView1.TabIndex = 23;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// Lines
			// 
			this.Lines.Text = "Lines";
			this.Lines.Width = 542;
			// 
			// IDDEndingComment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(686, 263);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.EndingCommentTextBox);
			this.Controls.Add(this.EndingCommentLabel);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IDDEndingComment";
			this.Text = "Enter Ending Comment For This Measurement";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label EndingCommentLabel;
        private System.Windows.Forms.TextBox EndingCommentTextBox;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader Lines;
	}
}