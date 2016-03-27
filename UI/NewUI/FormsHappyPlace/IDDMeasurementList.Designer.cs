namespace NewUI
{
    partial class IDDMeasurementList
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

        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;

        private void InitializeComponent()
        {
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Option = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Det = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StratumId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(575, 12);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(75, 23);
            this.PrintBtn.TabIndex = 5;
            this.PrintBtn.Text = "Print this list";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(575, 56);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(575, 85);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(575, 114);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Option,
            this.Det,
            this.ItemId,
            this.StratumId,
            this.DT,
            this.FileName});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(25, 12);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(544, 365);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // Option
            // 
            this.Option.Text = "Meas Option";
            this.Option.Width = 72;
            // 
            // Det
            // 
            this.Det.Text = "Detector";
            this.Det.Width = 80;
            // 
            // ItemId
            // 
            this.ItemId.DisplayIndex = 3;
            this.ItemId.Text = "Item id";
            this.ItemId.Width = 94;
            // 
            // StratumId
            // 
            this.StratumId.DisplayIndex = 2;
            this.StratumId.Text = "Stratum id";
            this.StratumId.Width = 74;
            // 
            // DT
            // 
            this.DT.Text = "Date and Time";
            this.DT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DT.Width = 120;
            // 
            // FileName
            // 
            this.FileName.Text = "File Name";
            this.FileName.Width = 120;
            // 
            // IDDMeasurementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 389);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Name = "IDDMeasurementList";
            this.Text = "Verification Measurement Selection";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader StratumId;
        private System.Windows.Forms.ColumnHeader DT;
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		#endregion

		private System.Windows.Forms.ColumnHeader Option;
        private System.Windows.Forms.ColumnHeader Det;
        private System.Windows.Forms.ColumnHeader FileName;
    }
}