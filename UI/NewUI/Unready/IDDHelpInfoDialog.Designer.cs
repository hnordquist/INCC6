namespace NewUI
{
    partial class IDDHelpInfoDialog
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
            this.CopyBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DefValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Comment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(683, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(109, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CopyBtn
            // 
            this.CopyBtn.Location = new System.Drawing.Point(683, 41);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(109, 23);
            this.CopyBtn.TabIndex = 1;
            this.CopyBtn.Text = "Copy to Clipboard";
            this.CopyBtn.UseVisualStyleBackColor = true;
            this.CopyBtn.Click += new System.EventHandler(this.CopyBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(683, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(109, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCol,
            this.ValueCol,
            this.DefValue,
            this.Comment});
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(654, 379);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // NameCol
            // 
            this.NameCol.Text = "Name";
            this.NameCol.Width = 100;
            // 
            // ValueCol
            // 
            this.ValueCol.Text = "Value";
            this.ValueCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ValueCol.Width = 200;
            // 
            // DefValue
            // 
            this.DefValue.Text = "Default Value";
            this.DefValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DefValue.Width = 100;
            // 
            // Comment
            // 
            this.Comment.Text = "Comment";
            this.Comment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Comment.Width = 250;
            // 
            // IDDHelpInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 405);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CopyBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDHelpInfoDialog";
            this.Text = "INCC System Info";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CopyBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader ValueCol;
        private System.Windows.Forms.ColumnHeader DefValue;
        private System.Windows.Forms.ColumnHeader Comment;
    }
}