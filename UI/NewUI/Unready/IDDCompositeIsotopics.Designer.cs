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
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(682, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(682, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(682, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // ReadBtn
            // 
            this.ReadBtn.Location = new System.Drawing.Point(534, 99);
            this.ReadBtn.Name = "ReadBtn";
            this.ReadBtn.Size = new System.Drawing.Size(223, 23);
            this.ReadBtn.TabIndex = 3;
            this.ReadBtn.Text = "Read compisite isotopics from file";
            this.ReadBtn.UseVisualStyleBackColor = true;
            this.ReadBtn.Click += new System.EventHandler(this.ReadBtn_Click);
            // 
            // WriteBtn
            // 
            this.WriteBtn.Location = new System.Drawing.Point(534, 128);
            this.WriteBtn.Name = "WriteBtn";
            this.WriteBtn.Size = new System.Drawing.Size(223, 23);
            this.WriteBtn.TabIndex = 4;
            this.WriteBtn.Text = "Write composite isotopics to file...";
            this.WriteBtn.UseVisualStyleBackColor = true;
            this.WriteBtn.Click += new System.EventHandler(this.WriteBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(534, 157);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(223, 23);
            this.AddBtn.TabIndex = 5;
            this.AddBtn.Text = "Add new composite isotopics data set";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(534, 186);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(223, 23);
            this.SaveBtn.TabIndex = 6;
            this.SaveBtn.Text = "Save compiosite isotopics data set";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // EditBtn
            // 
            this.EditBtn.Location = new System.Drawing.Point(534, 215);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(223, 23);
            this.EditBtn.TabIndex = 7;
            this.EditBtn.Text = "Edit composite isotopics id";
            this.EditBtn.UseVisualStyleBackColor = true;
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(534, 244);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(223, 23);
            this.DeleteBtn.TabIndex = 9;
            this.DeleteBtn.Text = "Delete composite isotopics data set";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // InstructionsLabel
            // 
            this.InstructionsLabel.AutoSize = true;
            this.InstructionsLabel.Location = new System.Drawing.Point(12, 9);
            this.InstructionsLabel.Name = "InstructionsLabel";
            this.InstructionsLabel.Size = new System.Drawing.Size(433, 13);
            this.InstructionsLabel.TabIndex = 10;
            this.InstructionsLabel.Text = "When all isotopic data sets are entered, click on the \'Calculate and Store Isotop" +
    "ics\' button.";
            // 
            // IsotopicsIdComboBox
            // 
            this.IsotopicsIdComboBox.FormattingEnabled = true;
            this.IsotopicsIdComboBox.Location = new System.Drawing.Point(127, 112);
            this.IsotopicsIdComboBox.Name = "IsotopicsIdComboBox";
            this.IsotopicsIdComboBox.Size = new System.Drawing.Size(200, 21);
            this.IsotopicsIdComboBox.TabIndex = 12;
            this.IsotopicsIdComboBox.SelectedIndexChanged += new System.EventHandler(this.IsotopicsIdComboBox_SelectedIndexChanged);
            // 
            // IsoSrcCodeComboBox
            // 
            this.IsoSrcCodeComboBox.FormattingEnabled = true;
            this.IsoSrcCodeComboBox.Location = new System.Drawing.Point(127, 179);
            this.IsoSrcCodeComboBox.Name = "IsoSrcCodeComboBox";
            this.IsoSrcCodeComboBox.Size = new System.Drawing.Size(200, 21);
            this.IsoSrcCodeComboBox.TabIndex = 13;
            this.IsoSrcCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IsoSrcCodeComboBox_SelectedIndexChanged);
            // 
            // IsotopicsIdLabel
            // 
            this.IsotopicsIdLabel.AutoSize = true;
            this.IsotopicsIdLabel.Location = new System.Drawing.Point(12, 115);
            this.IsotopicsIdLabel.Name = "IsotopicsIdLabel";
            this.IsotopicsIdLabel.Size = new System.Drawing.Size(111, 13);
            this.IsotopicsIdLabel.TabIndex = 15;
            this.IsotopicsIdLabel.Text = "Composite isotopics id";
            // 
            // ReferenceDateLabel
            // 
            this.ReferenceDateLabel.AutoSize = true;
            this.ReferenceDateLabel.Location = new System.Drawing.Point(43, 148);
            this.ReferenceDateLabel.Name = "ReferenceDateLabel";
            this.ReferenceDateLabel.Size = new System.Drawing.Size(81, 13);
            this.ReferenceDateLabel.TabIndex = 16;
            this.ReferenceDateLabel.Text = "Reference date";
            // 
            // IsoSrcCodeLabel
            // 
            this.IsoSrcCodeLabel.AutoSize = true;
            this.IsoSrcCodeLabel.Location = new System.Drawing.Point(10, 182);
            this.IsoSrcCodeLabel.Name = "IsoSrcCodeLabel";
            this.IsoSrcCodeLabel.Size = new System.Drawing.Size(111, 13);
            this.IsoSrcCodeLabel.TabIndex = 17;
            this.IsoSrcCodeLabel.Text = "Isotopics source code";
            // 
            // CalculateBtn
            // 
            this.CalculateBtn.BackColor = System.Drawing.Color.Lime;
            this.CalculateBtn.Location = new System.Drawing.Point(127, 52);
            this.CalculateBtn.Name = "CalculateBtn";
            this.CalculateBtn.Size = new System.Drawing.Size(200, 23);
            this.CalculateBtn.TabIndex = 18;
            this.CalculateBtn.Text = "Calculate and Store Isotopics";
            this.CalculateBtn.UseVisualStyleBackColor = false;
            this.CalculateBtn.Click += new System.EventHandler(this.CalculateBtn_Click);
            // 
            // ReferenceDateTimePicker
            // 
            this.ReferenceDateTimePicker.Location = new System.Drawing.Point(127, 146);
            this.ReferenceDateTimePicker.Name = "ReferenceDateTimePicker";
            this.ReferenceDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.ReferenceDateTimePicker.TabIndex = 19;
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
            this.listView1.Location = new System.Drawing.Point(23, 282);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(734, 223);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 517);
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
    }
}