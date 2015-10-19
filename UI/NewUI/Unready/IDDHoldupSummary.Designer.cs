namespace NewUI
{
    partial class IDDHoldupSummary
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
            this.SortGroupBox = new System.Windows.Forms.GroupBox();
            this.DateTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.SortByStratumRadioBtn = new System.Windows.Forms.RadioButton();
            this.SortByDetectorRadioBtn = new System.Windows.Forms.RadioButton();
            this.CurrentDateRadioBtn = new System.Windows.Forms.RadioButton();
            this.LastEnteredDateRadioBtn = new System.Windows.Forms.RadioButton();
            this.PrintCheckBox = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.InspectionNumLabel = new System.Windows.Forms.Label();
            this.StartDateLabel = new System.Windows.Forms.Label();
            this.EndDateLabel = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SortGroupBox.SuspendLayout();
            this.DateTimeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // InspectionNumComboBox
            // 
            this.InspectionNumComboBox.FormattingEnabled = true;
            this.InspectionNumComboBox.Location = new System.Drawing.Point(112, 17);
            this.InspectionNumComboBox.Name = "InspectionNumComboBox";
            this.InspectionNumComboBox.Size = new System.Drawing.Size(140, 21);
            this.InspectionNumComboBox.TabIndex = 0;
            this.InspectionNumComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumComboBox_SelectedIndexChanged);
            // 
            // SortGroupBox
            // 
            this.SortGroupBox.Controls.Add(this.SortByDetectorRadioBtn);
            this.SortGroupBox.Controls.Add(this.SortByStratumRadioBtn);
            this.SortGroupBox.Location = new System.Drawing.Point(15, 53);
            this.SortGroupBox.Name = "SortGroupBox";
            this.SortGroupBox.Size = new System.Drawing.Size(237, 73);
            this.SortGroupBox.TabIndex = 1;
            this.SortGroupBox.TabStop = false;
            // 
            // DateTimeGroupBox
            // 
            this.DateTimeGroupBox.Controls.Add(this.LastEnteredDateRadioBtn);
            this.DateTimeGroupBox.Controls.Add(this.CurrentDateRadioBtn);
            this.DateTimeGroupBox.Location = new System.Drawing.Point(15, 132);
            this.DateTimeGroupBox.Name = "DateTimeGroupBox";
            this.DateTimeGroupBox.Size = new System.Drawing.Size(237, 77);
            this.DateTimeGroupBox.TabIndex = 2;
            this.DateTimeGroupBox.TabStop = false;
            // 
            // SortByStratumRadioBtn
            // 
            this.SortByStratumRadioBtn.AutoSize = true;
            this.SortByStratumRadioBtn.Location = new System.Drawing.Point(15, 19);
            this.SortByStratumRadioBtn.Name = "SortByStratumRadioBtn";
            this.SortByStratumRadioBtn.Size = new System.Drawing.Size(106, 17);
            this.SortByStratumRadioBtn.TabIndex = 0;
            this.SortByStratumRadioBtn.TabStop = true;
            this.SortByStratumRadioBtn.Text = "Sort by stratum id";
            this.SortByStratumRadioBtn.UseVisualStyleBackColor = true;
            this.SortByStratumRadioBtn.CheckedChanged += new System.EventHandler(this.SortByStratumRadioBtn_CheckedChanged);
            // 
            // SortByDetectorRadioBtn
            // 
            this.SortByDetectorRadioBtn.AutoSize = true;
            this.SortByDetectorRadioBtn.Location = new System.Drawing.Point(15, 42);
            this.SortByDetectorRadioBtn.Name = "SortByDetectorRadioBtn";
            this.SortByDetectorRadioBtn.Size = new System.Drawing.Size(111, 17);
            this.SortByDetectorRadioBtn.TabIndex = 1;
            this.SortByDetectorRadioBtn.TabStop = true;
            this.SortByDetectorRadioBtn.Text = "Sort by detector id";
            this.SortByDetectorRadioBtn.UseVisualStyleBackColor = true;
            this.SortByDetectorRadioBtn.CheckedChanged += new System.EventHandler(this.SortByDetectorRadioBtn_CheckedChanged);
            // 
            // CurrentDateRadioBtn
            // 
            this.CurrentDateRadioBtn.AutoSize = true;
            this.CurrentDateRadioBtn.Location = new System.Drawing.Point(15, 19);
            this.CurrentDateRadioBtn.Name = "CurrentDateRadioBtn";
            this.CurrentDateRadioBtn.Size = new System.Drawing.Size(195, 17);
            this.CurrentDateRadioBtn.TabIndex = 0;
            this.CurrentDateRadioBtn.TabStop = true;
            this.CurrentDateRadioBtn.Text = "Default to current end date and time";
            this.CurrentDateRadioBtn.UseVisualStyleBackColor = true;
            this.CurrentDateRadioBtn.CheckedChanged += new System.EventHandler(this.CurrentDateRadioBtn_CheckedChanged);
            // 
            // LastEnteredDateRadioBtn
            // 
            this.LastEnteredDateRadioBtn.AutoSize = true;
            this.LastEnteredDateRadioBtn.Location = new System.Drawing.Point(15, 42);
            this.LastEnteredDateRadioBtn.Name = "LastEnteredDateRadioBtn";
            this.LastEnteredDateRadioBtn.Size = new System.Drawing.Size(217, 17);
            this.LastEnteredDateRadioBtn.TabIndex = 1;
            this.LastEnteredDateRadioBtn.TabStop = true;
            this.LastEnteredDateRadioBtn.Text = "Default to last entered end date and time";
            this.LastEnteredDateRadioBtn.UseVisualStyleBackColor = true;
            this.LastEnteredDateRadioBtn.CheckedChanged += new System.EventHandler(this.LastEnteredDateRadioBtn_CheckedChanged);
            // 
            // PrintCheckBox
            // 
            this.PrintCheckBox.AutoSize = true;
            this.PrintCheckBox.Location = new System.Drawing.Point(363, 89);
            this.PrintCheckBox.Name = "PrintCheckBox";
            this.PrintCheckBox.Size = new System.Drawing.Size(47, 17);
            this.PrintCheckBox.TabIndex = 7;
            this.PrintCheckBox.Text = "Print";
            this.PrintCheckBox.UseVisualStyleBackColor = true;
            this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(598, 15);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(598, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(598, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 10;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // InspectionNumLabel
            // 
            this.InspectionNumLabel.AutoSize = true;
            this.InspectionNumLabel.Location = new System.Drawing.Point(12, 20);
            this.InspectionNumLabel.Name = "InspectionNumLabel";
            this.InspectionNumLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumLabel.TabIndex = 11;
            this.InspectionNumLabel.Text = "Inspection number";
            // 
            // StartDateLabel
            // 
            this.StartDateLabel.AutoSize = true;
            this.StartDateLabel.Location = new System.Drawing.Point(274, 20);
            this.StartDateLabel.Name = "StartDateLabel";
            this.StartDateLabel.Size = new System.Drawing.Size(83, 13);
            this.StartDateLabel.TabIndex = 12;
            this.StartDateLabel.Text = "Start Date/Time";
            // 
            // EndDateLabel
            // 
            this.EndDateLabel.AutoSize = true;
            this.EndDateLabel.Location = new System.Drawing.Point(277, 54);
            this.EndDateLabel.Name = "EndDateLabel";
            this.EndDateLabel.Size = new System.Drawing.Size(80, 13);
            this.EndDateLabel.TabIndex = 13;
            this.EndDateLabel.Text = "End Date/Time";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 224);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(658, 368);
            this.listBox1.TabIndex = 16;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Location = new System.Drawing.Point(363, 16);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 17;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Location = new System.Drawing.Point(363, 52);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 19;
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // IDDHoldupSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 605);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.EndDateLabel);
            this.Controls.Add(this.StartDateLabel);
            this.Controls.Add(this.InspectionNumLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintCheckBox);
            this.Controls.Add(this.DateTimeGroupBox);
            this.Controls.Add(this.SortGroupBox);
            this.Controls.Add(this.InspectionNumComboBox);
            this.Name = "IDDHoldupSummary";
            this.Text = "Holdup Summary Setup";
            this.SortGroupBox.ResumeLayout(false);
            this.SortGroupBox.PerformLayout();
            this.DateTimeGroupBox.ResumeLayout(false);
            this.DateTimeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox InspectionNumComboBox;
        private System.Windows.Forms.GroupBox SortGroupBox;
        private System.Windows.Forms.RadioButton SortByDetectorRadioBtn;
        private System.Windows.Forms.RadioButton SortByStratumRadioBtn;
        private System.Windows.Forms.GroupBox DateTimeGroupBox;
        private System.Windows.Forms.RadioButton LastEnteredDateRadioBtn;
        private System.Windows.Forms.RadioButton CurrentDateRadioBtn;
        private System.Windows.Forms.CheckBox PrintCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label InspectionNumLabel;
        private System.Windows.Forms.Label StartDateLabel;
        private System.Windows.Forms.Label EndDateLabel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
    }
}