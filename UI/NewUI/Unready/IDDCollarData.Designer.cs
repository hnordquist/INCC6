namespace NewUI
{
    partial class IDDCollarData
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
            this.PrintBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ItemId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Len = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LenError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalU235 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalU235Error = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalU238 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalU238Error = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalRods = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalPoisonRods = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PoisonPct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PoisonPctError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PoisonRodType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(982, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(95, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(982, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(95, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(982, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(95, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(982, 99);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(95, 23);
            this.PrintBtn.TabIndex = 5;
            this.PrintBtn.Text = "Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(982, 128);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(95, 23);
            this.DeleteBtn.TabIndex = 6;
            this.DeleteBtn.Text = "Delete Items...";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemId,
            this.Len,
            this.LenError,
            this.TotalU235,
            this.TotalU235Error,
            this.TotalU238,
            this.TotalU238Error,
            this.TotalRods,
            this.TotalPoisonRods,
            this.PoisonPct,
            this.PoisonPctError,
            this.PoisonRodType});
            this.listView1.Location = new System.Drawing.Point(7, 9);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(959, 446);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // ItemId
            // 
            this.ItemId.Text = "Item id";
            // 
            // Len
            // 
            this.Len.Text = "Length (cm)";
            this.Len.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Len.Width = 75;
            // 
            // LenError
            // 
            this.LenError.Text = "Length Error";
            this.LenError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LenError.Width = 75;
            // 
            // TotalU235
            // 
            this.TotalU235.Text = "Total U235 (g)";
            this.TotalU235.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalU235.Width = 80;
            // 
            // TotalU235Error
            // 
            this.TotalU235Error.Text = "Total U235 Error";
            this.TotalU235Error.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalU235Error.Width = 90;
            // 
            // TotalU238
            // 
            this.TotalU238.Text = "Total U238 (g)";
            this.TotalU238.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalU238.Width = 80;
            // 
            // TotalU238Error
            // 
            this.TotalU238Error.Text = "Total U238 Error";
            this.TotalU238Error.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalU238Error.Width = 90;
            // 
            // TotalRods
            // 
            this.TotalRods.Text = "Total Rods";
            this.TotalRods.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalRods.Width = 65;
            // 
            // TotalPoisonRods
            // 
            this.TotalPoisonRods.Text = "Total Poison Rods";
            this.TotalPoisonRods.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalPoisonRods.Width = 100;
            // 
            // PoisonPct
            // 
            this.PoisonPct.Text = "Poison %";
            this.PoisonPct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PoisonPctError
            // 
            this.PoisonPctError.Text = "Poison % Error";
            this.PoisonPctError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PoisonPctError.Width = 80;
            // 
            // PoisonRodType
            // 
            this.PoisonRodType.Text = "Poison Rod Type";
            this.PoisonRodType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PoisonRodType.Width = 100;
            // 
            // IDDCollarData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 464);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDCollarData";
            this.Text = "Enter Collar Item Data";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ItemId;
        private System.Windows.Forms.ColumnHeader Len;
        private System.Windows.Forms.ColumnHeader LenError;
        private System.Windows.Forms.ColumnHeader TotalU235;
        private System.Windows.Forms.ColumnHeader TotalU235Error;
        private System.Windows.Forms.ColumnHeader TotalU238;
        private System.Windows.Forms.ColumnHeader TotalU238Error;
        private System.Windows.Forms.ColumnHeader TotalRods;
        private System.Windows.Forms.ColumnHeader TotalPoisonRods;
        private System.Windows.Forms.ColumnHeader PoisonPct;
        private System.Windows.Forms.ColumnHeader PoisonPctError;
        private System.Windows.Forms.ColumnHeader PoisonRodType;
    }
}