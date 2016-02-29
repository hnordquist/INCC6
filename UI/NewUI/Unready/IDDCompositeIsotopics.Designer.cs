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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDCompositeIsotopics));
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.PuMass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Pu238 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Pu239 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Pu240 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Pu241 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Pu242 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PuDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Am241 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AmDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
			this.ReadBtn.Location = new System.Drawing.Point(712, 122);
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
			this.WriteBtn.Location = new System.Drawing.Point(712, 158);
			this.WriteBtn.Margin = new System.Windows.Forms.Padding(4);
			this.WriteBtn.Name = "WriteBtn";
			this.WriteBtn.Size = new System.Drawing.Size(297, 28);
			this.WriteBtn.TabIndex = 4;
			this.WriteBtn.Text = "Write composite isotopics to file...";
			this.toolTip1.SetToolTip(this.WriteBtn, resources.GetString("WriteBtn.ToolTip"));
			this.WriteBtn.UseVisualStyleBackColor = true;
			this.WriteBtn.Click += new System.EventHandler(this.WriteBtn_Click);
			// 
			// AddBtn
			// 
			this.AddBtn.Location = new System.Drawing.Point(712, 193);
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
			this.SaveBtn.Location = new System.Drawing.Point(712, 229);
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
			this.EditBtn.Location = new System.Drawing.Point(712, 265);
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
			this.DeleteBtn.Location = new System.Drawing.Point(712, 300);
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
			this.IsotopicsIdComboBox.Location = new System.Drawing.Point(169, 138);
			this.IsotopicsIdComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.IsotopicsIdComboBox.Name = "IsotopicsIdComboBox";
			this.IsotopicsIdComboBox.Size = new System.Drawing.Size(265, 24);
			this.IsotopicsIdComboBox.TabIndex = 12;
			this.toolTip1.SetToolTip(this.IsotopicsIdComboBox, "Select the desired composite isotopics data set from the picklist");
			this.IsotopicsIdComboBox.SelectedIndexChanged += new System.EventHandler(this.IsotopicsIdComboBox_SelectedIndexChanged);
			// 
			// IsoSrcCodeComboBox
			// 
			this.IsoSrcCodeComboBox.FormattingEnabled = true;
			this.IsoSrcCodeComboBox.Location = new System.Drawing.Point(169, 220);
			this.IsoSrcCodeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.IsoSrcCodeComboBox.Name = "IsoSrcCodeComboBox";
			this.IsoSrcCodeComboBox.Size = new System.Drawing.Size(265, 24);
			this.IsoSrcCodeComboBox.TabIndex = 13;
			this.toolTip1.SetToolTip(this.IsoSrcCodeComboBox, resources.GetString("IsoSrcCodeComboBox.ToolTip"));
			this.IsoSrcCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IsoSrcCodeComboBox_SelectedIndexChanged);
			// 
			// IsotopicsIdLabel
			// 
			this.IsotopicsIdLabel.AutoSize = true;
			this.IsotopicsIdLabel.Location = new System.Drawing.Point(16, 142);
			this.IsotopicsIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IsotopicsIdLabel.Name = "IsotopicsIdLabel";
			this.IsotopicsIdLabel.Size = new System.Drawing.Size(148, 17);
			this.IsotopicsIdLabel.TabIndex = 15;
			this.IsotopicsIdLabel.Text = "Composite isotopics id";
			// 
			// ReferenceDateLabel
			// 
			this.ReferenceDateLabel.AutoSize = true;
			this.ReferenceDateLabel.Location = new System.Drawing.Point(57, 182);
			this.ReferenceDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ReferenceDateLabel.Name = "ReferenceDateLabel";
			this.ReferenceDateLabel.Size = new System.Drawing.Size(106, 17);
			this.ReferenceDateLabel.TabIndex = 16;
			this.ReferenceDateLabel.Text = "Reference date";
			// 
			// IsoSrcCodeLabel
			// 
			this.IsoSrcCodeLabel.AutoSize = true;
			this.IsoSrcCodeLabel.Location = new System.Drawing.Point(13, 224);
			this.IsoSrcCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IsoSrcCodeLabel.Name = "IsoSrcCodeLabel";
			this.IsoSrcCodeLabel.Size = new System.Drawing.Size(145, 17);
			this.IsoSrcCodeLabel.TabIndex = 17;
			this.IsoSrcCodeLabel.Text = "Isotopics source code";
			// 
			// CalculateBtn
			// 
			this.CalculateBtn.BackColor = System.Drawing.Color.Lime;
			this.CalculateBtn.Location = new System.Drawing.Point(169, 64);
			this.CalculateBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CalculateBtn.Name = "CalculateBtn";
			this.CalculateBtn.Size = new System.Drawing.Size(267, 28);
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
			this.ReferenceDateTimePicker.Location = new System.Drawing.Point(169, 180);
			this.ReferenceDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
			this.ReferenceDateTimePicker.Name = "ReferenceDateTimePicker";
			this.ReferenceDateTimePicker.Size = new System.Drawing.Size(265, 22);
			this.ReferenceDateTimePicker.TabIndex = 19;
			this.toolTip1.SetToolTip(this.ReferenceDateTimePicker, "The date the isotopics were determined");
			this.ReferenceDateTimePicker.ValueChanged += new System.EventHandler(this.ReferenceDateTimePicker_ValueChanged);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PuMass,
            this.Pu238,
            this.Pu239,
            this.Pu240,
            this.Pu241,
            this.Pu242,
            this.PuDate,
            this.Am241,
            this.AmDate});
			this.listView1.Location = new System.Drawing.Point(31, 347);
			this.listView1.Margin = new System.Windows.Forms.Padding(4);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(977, 274);
			this.listView1.TabIndex = 20;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// PuMass
			// 
			this.PuMass.Text = "Pu Mass (g)";
			this.PuMass.Width = 70;
			// 
			// Pu238
			// 
			this.Pu238.Text = "% Pu238";
			this.Pu238.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Pu239
			// 
			this.Pu239.Text = "% Pu239";
			this.Pu239.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Pu240
			// 
			this.Pu240.Text = "% Pu240";
			this.Pu240.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Pu241
			// 
			this.Pu241.Text = "% Pu241";
			this.Pu241.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Pu242
			// 
			this.Pu242.Text = "% Pu242";
			// 
			// PuDate
			// 
			this.PuDate.Text = "Pu Date";
			this.PuDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.PuDate.Width = 150;
			// 
			// Am241
			// 
			this.Am241.Text = "% Am241";
			this.Am241.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// AmDate
			// 
			this.AmDate.Text = "Am Date";
			this.AmDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.AmDate.Width = 150;
			// 
			// IDDCompositeIsotopics
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1025, 636);
			this.Controls.Add(this.listView1);
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
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader PuMass;
        private System.Windows.Forms.ColumnHeader Pu238;
        private System.Windows.Forms.ColumnHeader Pu239;
        private System.Windows.Forms.ColumnHeader Pu240;
        private System.Windows.Forms.ColumnHeader Pu241;
        private System.Windows.Forms.ColumnHeader Pu242;
        private System.Windows.Forms.ColumnHeader PuDate;
        private System.Windows.Forms.ColumnHeader Am241;
        private System.Windows.Forms.ColumnHeader AmDate;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}