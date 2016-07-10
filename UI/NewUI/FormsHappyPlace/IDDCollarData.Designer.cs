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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.ItemIdDataGrid = new System.Windows.Forms.DataGridView();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LengthErr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotU235 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotU235Err = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotU238 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotU238Err = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotRods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotPoisonRods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoiPct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoiPctErr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RodType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ItemIdDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(878, 2);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(95, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(878, 31);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(95, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(878, 60);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(95, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(878, 89);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(95, 23);
            this.PrintBtn.TabIndex = 5;
            this.PrintBtn.Text = "Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(878, 118);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(95, 23);
            this.DeleteBtn.TabIndex = 6;
            this.DeleteBtn.Text = "Delete Items...";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // ItemIdDataGrid
            // 
            this.ItemIdDataGrid.AllowUserToDeleteRows = false;
            this.ItemIdDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ItemIdDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.Length,
            this.LengthErr,
            this.TotU235,
            this.TotU235Err,
            this.TotU238,
            this.TotU238Err,
            this.TotRods,
            this.TotPoisonRods,
            this.PoiPct,
            this.PoiPctErr,
            this.RodType});
            this.ItemIdDataGrid.Location = new System.Drawing.Point(1, 2);
            this.ItemIdDataGrid.Name = "ItemIdDataGrid";
            this.ItemIdDataGrid.RowHeadersVisible = false;
            this.ItemIdDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ItemIdDataGrid.Size = new System.Drawing.Size(871, 322);
            this.ItemIdDataGrid.TabIndex = 8;
            this.ItemIdDataGrid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.ItemIdDataGrid_CellValueNeeded);
            this.ItemIdDataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ItemIdDataGrid_DataError);
            this.ItemIdDataGrid.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.ItemIdDataGrid_SortCompare);
            this.ItemIdDataGrid.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.ItemIdDataGrid_UserAddedRow);
            // 
            // Item
            // 
            this.Item.DataPropertyName = "Item";
            this.Item.HeaderText = "Item id";
            this.Item.MaxInputLength = 128;
            this.Item.Name = "Item";
            this.Item.Width = 85;
            // 
            // Length
            // 
            dataGridViewCellStyle1.Format = "N3";
            dataGridViewCellStyle1.NullValue = "0";
            this.Length.DefaultCellStyle = dataGridViewCellStyle1;
            this.Length.HeaderText = "Length (cm)";
            this.Length.Name = "Length";
            this.Length.Width = 70;
            // 
            // LengthErr
            // 
            dataGridViewCellStyle2.Format = "F3";
            dataGridViewCellStyle2.NullValue = "0";
            this.LengthErr.DefaultCellStyle = dataGridViewCellStyle2;
            this.LengthErr.HeaderText = "Length Err";
            this.LengthErr.Name = "LengthErr";
            this.LengthErr.Width = 70;
            // 
            // TotU235
            // 
            dataGridViewCellStyle3.Format = "F3";
            dataGridViewCellStyle3.NullValue = "0";
            this.TotU235.DefaultCellStyle = dataGridViewCellStyle3;
            this.TotU235.HeaderText = "Total U235 (g)";
            this.TotU235.Name = "TotU235";
            this.TotU235.Width = 70;
            // 
            // TotU235Err
            // 
            dataGridViewCellStyle4.Format = "F3";
            dataGridViewCellStyle4.NullValue = "0";
            this.TotU235Err.DefaultCellStyle = dataGridViewCellStyle4;
            this.TotU235Err.HeaderText = "Total U235 Err";
            this.TotU235Err.Name = "TotU235Err";
            this.TotU235Err.Width = 70;
            // 
            // TotU238
            // 
            dataGridViewCellStyle5.NullValue = "0";
            this.TotU238.DefaultCellStyle = dataGridViewCellStyle5;
            this.TotU238.HeaderText = "Total U238 (g)";
            this.TotU238.Name = "TotU238";
            this.TotU238.Width = 70;
            // 
            // TotU238Err
            // 
            dataGridViewCellStyle6.NullValue = "0";
            this.TotU238Err.DefaultCellStyle = dataGridViewCellStyle6;
            this.TotU238Err.HeaderText = "Total U238 Err";
            this.TotU238Err.Name = "TotU238Err";
            this.TotU238Err.Width = 70;
            // 
            // TotRods
            // 
            dataGridViewCellStyle7.Format = "N3";
            dataGridViewCellStyle7.NullValue = "0";
            this.TotRods.DefaultCellStyle = dataGridViewCellStyle7;
            this.TotRods.HeaderText = "Total Rods";
            this.TotRods.Name = "TotRods";
            this.TotRods.Width = 70;
            // 
            // TotPoisonRods
            // 
            dataGridViewCellStyle8.Format = "N3";
            dataGridViewCellStyle8.NullValue = "0";
            this.TotPoisonRods.DefaultCellStyle = dataGridViewCellStyle8;
            this.TotPoisonRods.HeaderText = "Total Poison Rods";
            this.TotPoisonRods.Name = "TotPoisonRods";
            this.TotPoisonRods.Width = 70;
            // 
            // PoiPct
            // 
            dataGridViewCellStyle9.Format = "F3";
            dataGridViewCellStyle9.NullValue = "0";
            this.PoiPct.DefaultCellStyle = dataGridViewCellStyle9;
            this.PoiPct.HeaderText = "Poison %";
            this.PoiPct.Name = "PoiPct";
            this.PoiPct.Width = 70;
            // 
            // PoiPctErr
            // 
            dataGridViewCellStyle10.Format = "F3";
            dataGridViewCellStyle10.NullValue = "0";
            this.PoiPctErr.DefaultCellStyle = dataGridViewCellStyle10;
            this.PoiPctErr.HeaderText = "Poison % Err";
            this.PoiPctErr.Name = "PoiPctErr";
            this.PoiPctErr.Width = 70;
            // 
            // RodType
            // 
            this.RodType.HeaderText = "Rod Type";
            this.RodType.Name = "RodType";
            this.RodType.Width = 80;
            // 
            // IDDCollarData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 325);
            this.Controls.Add(this.ItemIdDataGrid);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDCollarData";
            this.Text = "Enter Collar Item Data";
            ((System.ComponentModel.ISupportInitialize)(this.ItemIdDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.DataGridView ItemIdDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn LengthErr;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotU235;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotU235Err;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotU238;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotU238Err;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotRods;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotPoisonRods;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoiPct;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoiPctErr;
        private System.Windows.Forms.DataGridViewComboBoxColumn RodType;
    }
}