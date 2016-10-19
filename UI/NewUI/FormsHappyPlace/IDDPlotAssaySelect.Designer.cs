namespace NewUI
{
    partial class IDDPlotAssaySelect
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Pu240e = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Dawbulls = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MCountSel = new System.Windows.Forms.TextBox();
            this.MCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(354, 0);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(71, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(354, 29);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(71, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(354, 58);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(71, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemId,
            this.DT,
            this.Pu240e,
            this.Dawbulls});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(348, 327);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ItemId
            // 
            this.ItemId.Text = "Item id";
            this.ItemId.Width = 94;
            // 
            // DT
            // 
            this.DT.Text = "Date and Time";
            this.DT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DT.Width = 120;
            // 
            // Pu240e
            // 
            this.Pu240e.Text = "Decl Pu240e Mass";
            this.Pu240e.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Pu240e.Width = 120;
            // 
            // Dawbulls
            // 
            this.Dawbulls.Text = "Dbls Rate";
            this.Dawbulls.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Dawbulls.Width = 96;
            // 
            // MCountSel
            // 
            this.MCountSel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MCountSel.CausesValidation = false;
            this.MCountSel.Location = new System.Drawing.Point(354, 178);
            this.MCountSel.Name = "MCountSel";
            this.MCountSel.ReadOnly = true;
            this.MCountSel.Size = new System.Drawing.Size(87, 13);
            this.MCountSel.TabIndex = 15;
            // 
            // MCount
            // 
            this.MCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MCount.CausesValidation = false;
            this.MCount.Location = new System.Drawing.Point(354, 152);
            this.MCount.Name = "MCount";
            this.MCount.ReadOnly = true;
            this.MCount.Size = new System.Drawing.Size(87, 13);
            this.MCount.TabIndex = 14;
            // 
            // IDDPlotAssaySelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 327);
            this.Controls.Add(this.MCountSel);
            this.Controls.Add(this.MCount);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDPlotAssaySelect";
            this.Text = "Select Verification Results to Plot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader DT;
        private System.Windows.Forms.ColumnHeader Pu240e;
        private System.Windows.Forms.ColumnHeader Dawbulls;
        private System.Windows.Forms.TextBox MCountSel;
        private System.Windows.Forms.TextBox MCount;
    }
}