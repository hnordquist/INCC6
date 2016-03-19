namespace NewUI
{
    partial class IDDCompositeIsotopics
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
        {
            if (disposing && (components != null))
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDCompositeIsotopics));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.ReadBtn = new System.Windows.Forms.Button();
			this.WriteBtn = new System.Windows.Forms.Button();
			this.AddBtn = new System.Windows.Forms.Button();
			this.SaveBtn = new System.Windows.Forms.Button();
			this.EditBtn = new System.Windows.Forms.Button();
			this.DeleteBtn = new System.Windows.Forms.Button();
			this.InstructionsLabel = new System.Windows.Forms.Label();
			this.IsotopicsIdComboBox = new System.Windows.Forms.ComboBox();
			this.IsoSrcCodeComboBox = new System.Windows.Forms.ComboBox();
			this.IsotopicsIdLabel = new System.Windows.Forms.Label();
			this.ReferenceDateLabel = new System.Windows.Forms.Label();
			this.IsoSrcCodeLabel = new System.Windows.Forms.Label();
			this.CalculateBtn = new System.Windows.Forms.Button();
			this.ReferenceDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.IsoDataGrid = new System.Windows.Forms.DataGridView();
			this.PuMass = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Pu238 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Pu239 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Pu240 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Pu241 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Pu242 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PuDate = new NewUI.CalendarColumn();
			this.Am241 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AmDate = new NewUI.CalendarColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.IsoDataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(909, 15);
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
			this.CancelBtn.Location = new System.Drawing.Point(909, 50);
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
			this.HelpBtn.Location = new System.Drawing.Point(909, 86);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 2;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// ReadBtn
			// 
			this.ReadBtn.Location = new System.Drawing.Point(604, 15);
			this.ReadBtn.Margin = new System.Windows.Forms.Padding(4);
			this.ReadBtn.Name = "ReadBtn";
			this.ReadBtn.Size = new System.Drawing.Size(297, 28);
			this.ReadBtn.TabIndex = 3;
			this.ReadBtn.Text = "Read composite isotopics from file";
			this.toolTip1.SetToolTip(this.ReadBtn, "Clicking here brings up windows file selection dialog to select a composite isoto" +
        "pics file to import into INCC");
			this.ReadBtn.UseVisualStyleBackColor = true;
			this.ReadBtn.Click += new System.EventHandler(this.ReadBtn_Click);
			// 
			// WriteBtn
			// 
			this.WriteBtn.Enabled = false;
			this.WriteBtn.Location = new System.Drawing.Point(604, 51);
			this.WriteBtn.Margin = new System.Windows.Forms.Padding(4);
			this.WriteBtn.Name = "WriteBtn";
			this.WriteBtn.Size = new System.Drawing.Size(297, 28);
			this.WriteBtn.TabIndex = 4;
			this.WriteBtn.Text = "Write composite isotopics to file...";
			this.toolTip1.SetToolTip(this.WriteBtn, resources.GetString("WriteBtn.ToolTip"));
			this.WriteBtn.UseVisualStyleBackColor = true;
			// 
			// AddBtn
			// 
			this.AddBtn.Location = new System.Drawing.Point(604, 87);
			this.AddBtn.Margin = new System.Windows.Forms.Padding(4);
			this.AddBtn.Name = "AddBtn";
			this.AddBtn.Size = new System.Drawing.Size(297, 28);
			this.AddBtn.TabIndex = 5;
			this.AddBtn.Text = "Add new composite isotopics data set";
			this.toolTip1.SetToolTip(this.AddBtn, resources.GetString("AddBtn.ToolTip"));
			this.AddBtn.UseVisualStyleBackColor = true;
			this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
			// 
			// SaveBtn
			// 
			this.SaveBtn.Location = new System.Drawing.Point(604, 123);
			this.SaveBtn.Margin = new System.Windows.Forms.Padding(4);
			this.SaveBtn.Name = "SaveBtn";
			this.SaveBtn.Size = new System.Drawing.Size(297, 28);
			this.SaveBtn.TabIndex = 6;
			this.SaveBtn.Text = "Save composite isotopics data set";
			this.toolTip1.SetToolTip(this.SaveBtn, "Clicking here will save the currently displayed composite isotopics values for th" +
        "e currently displayed composite isotopics id");
			this.SaveBtn.UseVisualStyleBackColor = true;
			this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
			// 
			// EditBtn
			// 
			this.EditBtn.Location = new System.Drawing.Point(604, 159);
			this.EditBtn.Margin = new System.Windows.Forms.Padding(4);
			this.EditBtn.Name = "EditBtn";
			this.EditBtn.Size = new System.Drawing.Size(297, 28);
			this.EditBtn.TabIndex = 7;
			this.EditBtn.Text = "Edit composite isotopics id";
			this.toolTip1.SetToolTip(this.EditBtn, "Clicking here will bring up a dialog box prompting you to enter a new isotopics i" +
        "d for the currently displayed isotopics data set");
			this.EditBtn.UseVisualStyleBackColor = true;
			this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
			// 
			// DeleteBtn
			// 
			this.DeleteBtn.Location = new System.Drawing.Point(604, 193);
			this.DeleteBtn.Margin = new System.Windows.Forms.Padding(4);
			this.DeleteBtn.Name = "DeleteBtn";
			this.DeleteBtn.Size = new System.Drawing.Size(297, 28);
			this.DeleteBtn.TabIndex = 9;
			this.DeleteBtn.Text = "Delete composite isotopics data set";
			this.toolTip1.SetToolTip(this.DeleteBtn, "Clicking here will bring up a dialog box asking confirmation for deleting the cur" +
        "rently displayed composite isotopics data set ");
			this.DeleteBtn.UseVisualStyleBackColor = true;
			this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
			// 
			// InstructionsLabel
			// 
			this.InstructionsLabel.AutoSize = true;
			this.InstructionsLabel.Location = new System.Drawing.Point(16, 11);
			this.InstructionsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InstructionsLabel.Name = "InstructionsLabel";
			this.InstructionsLabel.Size = new System.Drawing.Size(575, 17);
			this.InstructionsLabel.TabIndex = 10;
			this.InstructionsLabel.Text = "When all isotopic data sets are entered, click on the \'Calculate and Store Isotop" +
    "ics\' button.";
			// 
			// IsotopicsIdComboBox
			// 
			this.IsotopicsIdComboBox.FormattingEnabled = true;
			this.IsotopicsIdComboBox.Location = new System.Drawing.Point(220, 90);
			this.IsotopicsIdComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.IsotopicsIdComboBox.Name = "IsotopicsIdComboBox";
			this.IsotopicsIdComboBox.Size = new System.Drawing.Size(203, 24);
			this.IsotopicsIdComboBox.TabIndex = 12;
			this.toolTip1.SetToolTip(this.IsotopicsIdComboBox, "Select the desired composite isotopics data set from the picklist");
			this.IsotopicsIdComboBox.SelectedIndexChanged += new System.EventHandler(this.IsotopicsIdComboBox_SelectedIndexChanged);
			// 
			// IsoSrcCodeComboBox
			// 
			this.IsoSrcCodeComboBox.FormattingEnabled = true;
			this.IsoSrcCodeComboBox.Location = new System.Drawing.Point(358, 163);
			this.IsoSrcCodeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.IsoSrcCodeComboBox.Name = "IsoSrcCodeComboBox";
			this.IsoSrcCodeComboBox.Size = new System.Drawing.Size(65, 24);
			this.IsoSrcCodeComboBox.TabIndex = 13;
			this.toolTip1.SetToolTip(this.IsoSrcCodeComboBox, resources.GetString("IsoSrcCodeComboBox.ToolTip"));
			this.IsoSrcCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IsoSrcCodeComboBox_SelectedIndexChanged);
			// 
			// IsotopicsIdLabel
			// 
			this.IsotopicsIdLabel.AutoSize = true;
			this.IsotopicsIdLabel.Location = new System.Drawing.Point(43, 97);
			this.IsotopicsIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IsotopicsIdLabel.Name = "IsotopicsIdLabel";
			this.IsotopicsIdLabel.Size = new System.Drawing.Size(148, 17);
			this.IsotopicsIdLabel.TabIndex = 15;
			this.IsotopicsIdLabel.Text = "Composite isotopics id";
			// 
			// ReferenceDateLabel
			// 
			this.ReferenceDateLabel.AutoSize = true;
			this.ReferenceDateLabel.Location = new System.Drawing.Point(85, 129);
			this.ReferenceDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ReferenceDateLabel.Name = "ReferenceDateLabel";
			this.ReferenceDateLabel.Size = new System.Drawing.Size(106, 17);
			this.ReferenceDateLabel.TabIndex = 16;
			this.ReferenceDateLabel.Text = "Reference date";
			// 
			// IsoSrcCodeLabel
			// 
			this.IsoSrcCodeLabel.AutoSize = true;
			this.IsoSrcCodeLabel.Location = new System.Drawing.Point(205, 166);
			this.IsoSrcCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IsoSrcCodeLabel.Name = "IsoSrcCodeLabel";
			this.IsoSrcCodeLabel.Size = new System.Drawing.Size(145, 17);
			this.IsoSrcCodeLabel.TabIndex = 17;
			this.IsoSrcCodeLabel.Text = "Isotopics source code";
			// 
			// CalculateBtn
			// 
			this.CalculateBtn.BackColor = System.Drawing.Color.Lime;
			this.CalculateBtn.Location = new System.Drawing.Point(208, 33);
			this.CalculateBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CalculateBtn.Name = "CalculateBtn";
			this.CalculateBtn.Size = new System.Drawing.Size(215, 46);
			this.CalculateBtn.TabIndex = 18;
			this.CalculateBtn.Text = "Calculate and Store Isotopics";
			this.toolTip1.SetToolTip(this.CalculateBtn, "Clicking here will cause a composite set of isotopics to be calculated and displa" +
        "yed. If you then click on OK, the composite isotopics will be stored in the isot" +
        "opics database");
			this.CalculateBtn.UseVisualStyleBackColor = false;
			this.CalculateBtn.Click += new System.EventHandler(this.CalculateBtn_Click);
			// 
			// ReferenceDateTimePicker
			// 
			this.ReferenceDateTimePicker.Location = new System.Drawing.Point(220, 129);
			this.ReferenceDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
			this.ReferenceDateTimePicker.Name = "ReferenceDateTimePicker";
			this.ReferenceDateTimePicker.Size = new System.Drawing.Size(203, 22);
			this.ReferenceDateTimePicker.TabIndex = 19;
			this.toolTip1.SetToolTip(this.ReferenceDateTimePicker, "The date the isotopics were determined");
			this.ReferenceDateTimePicker.ValueChanged += new System.EventHandler(this.ReferenceDateTimePicker_ValueChanged);
			// 
			// IsoDataGrid
			// 
			this.IsoDataGrid.AllowUserToDeleteRows = false;
			this.IsoDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.IsoDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PuMass,
            this.Pu238,
            this.Pu239,
            this.Pu240,
            this.Pu241,
            this.Pu242,
            this.PuDate,
            this.Am241,
            this.AmDate});
			this.IsoDataGrid.Location = new System.Drawing.Point(0, 229);
			this.IsoDataGrid.Margin = new System.Windows.Forms.Padding(4);
			this.IsoDataGrid.Name = "IsoDataGrid";
			this.IsoDataGrid.RowHeadersVisible = false;
			this.IsoDataGrid.Size = new System.Drawing.Size(1025, 295);
			this.IsoDataGrid.TabIndex = 20;
			// 
			// PuMass
			// 
			this.PuMass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle1.Format = "N3";
			dataGridViewCellStyle1.NullValue = null;
			this.PuMass.DefaultCellStyle = dataGridViewCellStyle1;
			this.PuMass.HeaderText = "Pu Mass (g)";
			this.PuMass.Name = "PuMass";
			this.PuMass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Pu238
			// 
			this.Pu238.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "N6";
			this.Pu238.DefaultCellStyle = dataGridViewCellStyle2;
			this.Pu238.HeaderText = "% Pu238";
			this.Pu238.Name = "Pu238";
			this.Pu238.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Pu239
			// 
			this.Pu239.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "N6";
			this.Pu239.DefaultCellStyle = dataGridViewCellStyle3;
			this.Pu239.HeaderText = "% Pu239";
			this.Pu239.Name = "Pu239";
			this.Pu239.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Pu240
			// 
			this.Pu240.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "N6";
			this.Pu240.DefaultCellStyle = dataGridViewCellStyle4;
			this.Pu240.HeaderText = "% Pu240";
			this.Pu240.Name = "Pu240";
			this.Pu240.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Pu241
			// 
			this.Pu241.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "N6";
			this.Pu241.DefaultCellStyle = dataGridViewCellStyle5;
			this.Pu241.HeaderText = "% Pu241";
			this.Pu241.Name = "Pu241";
			this.Pu241.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Pu242
			// 
			this.Pu242.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "N6";
			this.Pu242.DefaultCellStyle = dataGridViewCellStyle6;
			this.Pu242.HeaderText = "% Pu242";
			this.Pu242.Name = "Pu242";
			this.Pu242.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// PuDate
			// 
			this.PuDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PuDate.HeaderText = "Pu Date";
			this.PuDate.Name = "PuDate";
			// 
			// Am241
			// 
			this.Am241.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Am241.DefaultCellStyle = dataGridViewCellStyle6;
			this.Am241.HeaderText = "% Am241";
			this.Am241.Name = "Am241";
			this.Am241.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// AmDate
			// 
			this.AmDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.AmDate.HeaderText = "Am Date";
			this.AmDate.Name = "AmDate";
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.HeaderText = "Pu Mass (g)";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.HeaderText = "% Pu238";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn3.HeaderText = "% Pu239";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.HeaderText = "% Pu240";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn5.HeaderText = "% Pu241";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn6.HeaderText = "% Pu242";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn7.HeaderText = "% Am241";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// IDDCompositeIsotopics
			// 
			this.AccessibleDescription = "c";
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1025, 525);
			this.Controls.Add(this.IsoDataGrid);
			this.Controls.Add(this.ReferenceDateTimePicker);
			this.Controls.Add(this.CalculateBtn);
			this.Controls.Add(this.IsoSrcCodeLabel);
			this.Controls.Add(this.ReferenceDateLabel);
			this.Controls.Add(this.IsotopicsIdLabel);
			this.Controls.Add(this.IsoSrcCodeComboBox);
			this.Controls.Add(this.IsotopicsIdComboBox);
			this.Controls.Add(this.InstructionsLabel);
			this.Controls.Add(this.DeleteBtn);
			this.Controls.Add(this.EditBtn);
			this.Controls.Add(this.SaveBtn);
			this.Controls.Add(this.AddBtn);
			this.Controls.Add(this.WriteBtn);
			this.Controls.Add(this.ReadBtn);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IDDCompositeIsotopics";
			this.Text = "Composite Isotopics";
			((System.ComponentModel.ISupportInitialize)(this.IsoDataGrid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button ReadBtn;
        private System.Windows.Forms.Button WriteBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Label InstructionsLabel;
        private System.Windows.Forms.ComboBox IsotopicsIdComboBox;
        private System.Windows.Forms.ComboBox IsoSrcCodeComboBox;
        private System.Windows.Forms.Label IsotopicsIdLabel;
        private System.Windows.Forms.Label ReferenceDateLabel;
        private System.Windows.Forms.Label IsoSrcCodeLabel;
        private System.Windows.Forms.Button CalculateBtn;
        private System.Windows.Forms.DateTimePicker ReferenceDateTimePicker;
		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView IsoDataGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private CalendarColumn AmDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn Am241;
		private CalendarColumn PuDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn Pu242;
		private System.Windows.Forms.DataGridViewTextBoxColumn Pu241;
		private System.Windows.Forms.DataGridViewTextBoxColumn Pu240;
		private System.Windows.Forms.DataGridViewTextBoxColumn Pu239;
		private System.Windows.Forms.DataGridViewTextBoxColumn Pu238;
		private System.Windows.Forms.DataGridViewTextBoxColumn PuMass;
	}
}