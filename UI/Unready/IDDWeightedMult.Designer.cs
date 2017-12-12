namespace UI
{
    partial class IDDWeightedMult
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
            this.PrintCalButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Coeff = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SpFissD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SpFissT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlphaND = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlphaNT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // PrintCalButton
            // 
            this.PrintCalButton.Location = new System.Drawing.Point(58, 216);
            this.PrintCalButton.Name = "PrintCalButton";
            this.PrintCalButton.Size = new System.Drawing.Size(112, 23);
            this.PrintCalButton.TabIndex = 1;
            this.PrintCalButton.Text = "Print calibration";
            this.PrintCalButton.UseVisualStyleBackColor = true;
            this.PrintCalButton.Click += new System.EventHandler(this.PrintCalButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(233, 216);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(371, 216);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(509, 216);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Coeff,
            this.SpFissD,
            this.SpFissT,
            this.AlphaND,
            this.AlphaNT});
            this.listView1.Location = new System.Drawing.Point(12, 14);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(644, 188);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Coeff
            // 
            this.Coeff.Text = "Coeff.";
            this.Coeff.Width = 40;
            // 
            // SpFissD
            // 
            this.SpFissD.Text = "Sp.Fiss. Doubles";
            this.SpFissD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpFissD.Width = 150;
            // 
            // SpFissT
            // 
            this.SpFissT.Text = "Sp.Fiss. Triples";
            this.SpFissT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpFissT.Width = 150;
            // 
            // AlphaND
            // 
            this.AlphaND.Text = "Alpha,N Doubles";
            this.AlphaND.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlphaND.Width = 150;
            // 
            // AlphaNT
            // 
            this.AlphaNT.Text = "Alpha,N Triples";
            this.AlphaNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlphaNT.Width = 150;
            // 
            // IDDWeightedMult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 254);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.PrintCalButton);
            this.Name = "IDDWeightedMult";
            this.Text = "Weighted Multiplicity";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PrintCalButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Coeff;
        private System.Windows.Forms.ColumnHeader SpFissD;
        private System.Windows.Forms.ColumnHeader SpFissT;
        private System.Windows.Forms.ColumnHeader AlphaND;
        private System.Windows.Forms.ColumnHeader AlphaNT;
    }
}