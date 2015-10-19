namespace NewUI
{
    partial class IDDAssaySummary
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
            this.InspectionNumComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SortByDetectorRadioBtn = new System.Windows.Forms.RadioButton();
            this.SortByStratRadioBtn = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DefaultLastEnteredRadioBtn = new System.Windows.Forms.RadioButton();
            this.DefaultCurrentRadioBtn = new System.Windows.Forms.RadioButton();
            this.PrintCheckBox = new System.Windows.Forms.CheckBox();
            this.InspectionNumLabel = new System.Windows.Forms.Label();
            this.StartDateLabel = new System.Windows.Forms.Label();
            this.EndDateLabel = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Sel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // InspectionNumComboBox
            // 
            this.InspectionNumComboBox.FormattingEnabled = true;
            this.InspectionNumComboBox.Location = new System.Drawing.Point(116, 12);
            this.InspectionNumComboBox.Name = "InspectionNumComboBox";
            this.InspectionNumComboBox.Size = new System.Drawing.Size(154, 21);
            this.InspectionNumComboBox.TabIndex = 0;
            this.InspectionNumComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SortByDetectorRadioBtn);
            this.groupBox1.Controls.Add(this.SortByStratRadioBtn);
            this.groupBox1.Location = new System.Drawing.Point(19, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 74);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // SortByDetectorRadioBtn
            // 
            this.SortByDetectorRadioBtn.AutoSize = true;
            this.SortByDetectorRadioBtn.Location = new System.Drawing.Point(23, 42);
            this.SortByDetectorRadioBtn.Name = "SortByDetectorRadioBtn";
            this.SortByDetectorRadioBtn.Size = new System.Drawing.Size(111, 17);
            this.SortByDetectorRadioBtn.TabIndex = 1;
            this.SortByDetectorRadioBtn.TabStop = true;
            this.SortByDetectorRadioBtn.Text = "Sort by detector id";
            this.SortByDetectorRadioBtn.UseVisualStyleBackColor = true;
            this.SortByDetectorRadioBtn.CheckedChanged += new System.EventHandler(this.SortByDetectorRadioBtn_CheckedChanged);
            // 
            // SortByStratRadioBtn
            // 
            this.SortByStratRadioBtn.AutoSize = true;
            this.SortByStratRadioBtn.Location = new System.Drawing.Point(23, 19);
            this.SortByStratRadioBtn.Name = "SortByStratRadioBtn";
            this.SortByStratRadioBtn.Size = new System.Drawing.Size(106, 17);
            this.SortByStratRadioBtn.TabIndex = 0;
            this.SortByStratRadioBtn.TabStop = true;
            this.SortByStratRadioBtn.Text = "Sort by stratum id";
            this.SortByStratRadioBtn.UseVisualStyleBackColor = true;
            this.SortByStratRadioBtn.CheckedChanged += new System.EventHandler(this.SortByStratRadioBtn_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DefaultLastEnteredRadioBtn);
            this.groupBox2.Controls.Add(this.DefaultCurrentRadioBtn);
            this.groupBox2.Location = new System.Drawing.Point(19, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 73);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // DefaultLastEnteredRadioBtn
            // 
            this.DefaultLastEnteredRadioBtn.AutoSize = true;
            this.DefaultLastEnteredRadioBtn.Location = new System.Drawing.Point(23, 42);
            this.DefaultLastEnteredRadioBtn.Name = "DefaultLastEnteredRadioBtn";
            this.DefaultLastEnteredRadioBtn.Size = new System.Drawing.Size(217, 17);
            this.DefaultLastEnteredRadioBtn.TabIndex = 1;
            this.DefaultLastEnteredRadioBtn.TabStop = true;
            this.DefaultLastEnteredRadioBtn.Text = "Default to last entered end date and time";
            this.DefaultLastEnteredRadioBtn.UseVisualStyleBackColor = true;
            this.DefaultLastEnteredRadioBtn.CheckedChanged += new System.EventHandler(this.DefaultLastEnteredRadioBtn_CheckedChanged);
            // 
            // DefaultCurrentRadioBtn
            // 
            this.DefaultCurrentRadioBtn.AutoSize = true;
            this.DefaultCurrentRadioBtn.Location = new System.Drawing.Point(23, 19);
            this.DefaultCurrentRadioBtn.Name = "DefaultCurrentRadioBtn";
            this.DefaultCurrentRadioBtn.Size = new System.Drawing.Size(195, 17);
            this.DefaultCurrentRadioBtn.TabIndex = 0;
            this.DefaultCurrentRadioBtn.TabStop = true;
            this.DefaultCurrentRadioBtn.Text = "Default to current end date and time";
            this.DefaultCurrentRadioBtn.UseVisualStyleBackColor = true;
            this.DefaultCurrentRadioBtn.CheckedChanged += new System.EventHandler(this.DefaultCurrentRadioBtn_CheckedChanged);
            // 
            // PrintCheckBox
            // 
            this.PrintCheckBox.AutoSize = true;
            this.PrintCheckBox.Location = new System.Drawing.Point(389, 91);
            this.PrintCheckBox.Name = "PrintCheckBox";
            this.PrintCheckBox.Size = new System.Drawing.Size(47, 17);
            this.PrintCheckBox.TabIndex = 7;
            this.PrintCheckBox.Text = "Print";
            this.PrintCheckBox.UseVisualStyleBackColor = true;
            this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
            // 
            // InspectionNumLabel
            // 
            this.InspectionNumLabel.AutoSize = true;
            this.InspectionNumLabel.Location = new System.Drawing.Point(16, 15);
            this.InspectionNumLabel.Name = "InspectionNumLabel";
            this.InspectionNumLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumLabel.TabIndex = 8;
            this.InspectionNumLabel.Text = "Inspection number";
            // 
            // StartDateLabel
            // 
            this.StartDateLabel.AutoSize = true;
            this.StartDateLabel.Location = new System.Drawing.Point(288, 15);
            this.StartDateLabel.Name = "StartDateLabel";
            this.StartDateLabel.Size = new System.Drawing.Size(96, 13);
            this.StartDateLabel.TabIndex = 9;
            this.StartDateLabel.Text = "Start date and time";
            // 
            // EndDateLabel
            // 
            this.EndDateLabel.AutoSize = true;
            this.EndDateLabel.Location = new System.Drawing.Point(291, 53);
            this.EndDateLabel.Name = "EndDateLabel";
            this.EndDateLabel.Size = new System.Drawing.Size(93, 13);
            this.EndDateLabel.TabIndex = 10;
            this.EndDateLabel.Text = "End date and time";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(609, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 13;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(609, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 14;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(609, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 15;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Location = new System.Drawing.Point(389, 12);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 16;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Location = new System.Drawing.Point(389, 51);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 17;
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Sel,
            this.Description});
            this.listView1.Location = new System.Drawing.Point(19, 208);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(665, 335);
            this.listView1.TabIndex = 18;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Sel
            // 
            this.Sel.Text = "Select";
            this.Sel.Width = 45;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Description.Width = 616;
            // 
            // IDDAssaySummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 567);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.EndDateLabel);
            this.Controls.Add(this.StartDateLabel);
            this.Controls.Add(this.InspectionNumLabel);
            this.Controls.Add(this.PrintCheckBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.InspectionNumComboBox);
            this.Name = "IDDAssaySummary";
            this.Text = "Summary Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox InspectionNumComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SortByDetectorRadioBtn;
        private System.Windows.Forms.RadioButton SortByStratRadioBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton DefaultLastEnteredRadioBtn;
        private System.Windows.Forms.RadioButton DefaultCurrentRadioBtn;
        private System.Windows.Forms.CheckBox PrintCheckBox;
        private System.Windows.Forms.Label InspectionNumLabel;
        private System.Windows.Forms.Label StartDateLabel;
        private System.Windows.Forms.Label EndDateLabel;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Sel;
        private System.Windows.Forms.ColumnHeader Description;
    }
}