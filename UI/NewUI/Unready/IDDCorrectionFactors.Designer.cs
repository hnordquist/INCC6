namespace NewUI
{
    partial class IDDCorrectionFactors
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "derp"}, -1);
            this.listView1 = new System.Windows.Forms.ListView();
            this.RodType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AbsFact = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.A = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.B = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.C = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MDMGroupBox = new System.Windows.Forms.GroupBox();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.DetectorLabel = new System.Windows.Forms.Label();
            this.MaterialLabel = new System.Windows.Forms.Label();
            this.K4GroupBox = new System.Windows.Forms.GroupBox();
            this.BErrorLabel = new System.Windows.Forms.Label();
            this.AErrorLabel = new System.Windows.Forms.Label();
            this.BLabel = new System.Windows.Forms.Label();
            this.ALabel = new System.Windows.Forms.Label();
            this.BErrorTextBox = new System.Windows.Forms.TextBox();
            this.AErrorTextBox = new System.Windows.Forms.TextBox();
            this.BTextBox = new System.Windows.Forms.TextBox();
            this.ATextBox = new System.Windows.Forms.TextBox();
            this.NextBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.NumRodsTextBox = new System.Windows.Forms.TextBox();
            this.NumRodsLabel = new System.Windows.Forms.Label();
            this.K3Label = new System.Windows.Forms.Label();
            this.MDMGroupBox.SuspendLayout();
            this.K4GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RodType,
            this.AbsFact,
            this.A,
            this.AError,
            this.B,
            this.BError,
            this.C,
            this.CError});
            listViewItem1.Checked = true;
            listViewItem1.StateImageIndex = 1;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(12, 123);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(569, 208);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // RodType
            // 
            this.RodType.Text = "Poison Rod Type";
            this.RodType.Width = 95;
            // 
            // AbsFact
            // 
            this.AbsFact.Text = "Poison Absrp. Factor";
            this.AbsFact.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AbsFact.Width = 110;
            // 
            // A
            // 
            this.A.Text = "a";
            this.A.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AError
            // 
            this.AError.DisplayIndex = 4;
            this.AError.Text = "a error";
            this.AError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // B
            // 
            this.B.DisplayIndex = 3;
            this.B.Text = "b";
            this.B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BError
            // 
            this.BError.Text = "b error";
            this.BError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // C
            // 
            this.C.Text = "c";
            this.C.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CError
            // 
            this.CError.Text = "c error";
            this.CError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MDMGroupBox
            // 
            this.MDMGroupBox.Controls.Add(this.ModeLabel);
            this.MDMGroupBox.Controls.Add(this.DetectorLabel);
            this.MDMGroupBox.Controls.Add(this.MaterialLabel);
            this.MDMGroupBox.Location = new System.Drawing.Point(438, 15);
            this.MDMGroupBox.Name = "MDMGroupBox";
            this.MDMGroupBox.Size = new System.Drawing.Size(143, 90);
            this.MDMGroupBox.TabIndex = 1;
            this.MDMGroupBox.TabStop = false;
            this.MDMGroupBox.Text = "Material/Detector/Mode";
            // 
            // ModeLabel
            // 
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new System.Drawing.Point(17, 66);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new System.Drawing.Size(82, 13);
            this.ModeLabel.TabIndex = 2;
            this.ModeLabel.Text = "Thermal (no Cd)";
            // 
            // DetectorLabel
            // 
            this.DetectorLabel.AutoSize = true;
            this.DetectorLabel.Location = new System.Drawing.Point(19, 44);
            this.DetectorLabel.Name = "DetectorLabel";
            this.DetectorLabel.Size = new System.Drawing.Size(80, 13);
            this.DetectorLabel.TabIndex = 1;
            this.DetectorLabel.Text = "XXXX/XXX/YY";
            // 
            // MaterialLabel
            // 
            this.MaterialLabel.AutoSize = true;
            this.MaterialLabel.Location = new System.Drawing.Point(18, 22);
            this.MaterialLabel.Name = "MaterialLabel";
            this.MaterialLabel.Size = new System.Drawing.Size(29, 13);
            this.MaterialLabel.TabIndex = 0;
            this.MaterialLabel.Text = "TBD";
            // 
            // K4GroupBox
            // 
            this.K4GroupBox.Controls.Add(this.BErrorLabel);
            this.K4GroupBox.Controls.Add(this.AErrorLabel);
            this.K4GroupBox.Controls.Add(this.BLabel);
            this.K4GroupBox.Controls.Add(this.ALabel);
            this.K4GroupBox.Controls.Add(this.BErrorTextBox);
            this.K4GroupBox.Controls.Add(this.AErrorTextBox);
            this.K4GroupBox.Controls.Add(this.BTextBox);
            this.K4GroupBox.Controls.Add(this.ATextBox);
            this.K4GroupBox.Location = new System.Drawing.Point(15, 350);
            this.K4GroupBox.Name = "K4GroupBox";
            this.K4GroupBox.Size = new System.Drawing.Size(338, 101);
            this.K4GroupBox.TabIndex = 2;
            this.K4GroupBox.TabStop = false;
            this.K4GroupBox.Text = "K4 - Uranium Mass Correction Factor";
            // 
            // BErrorLabel
            // 
            this.BErrorLabel.AutoSize = true;
            this.BErrorLabel.Location = new System.Drawing.Point(175, 67);
            this.BErrorLabel.Name = "BErrorLabel";
            this.BErrorLabel.Size = new System.Drawing.Size(37, 13);
            this.BErrorLabel.TabIndex = 8;
            this.BErrorLabel.Text = "b error";
            // 
            // AErrorLabel
            // 
            this.AErrorLabel.AutoSize = true;
            this.AErrorLabel.Location = new System.Drawing.Point(175, 28);
            this.AErrorLabel.Name = "AErrorLabel";
            this.AErrorLabel.Size = new System.Drawing.Size(37, 13);
            this.AErrorLabel.TabIndex = 7;
            this.AErrorLabel.Text = "a error";
            // 
            // BLabel
            // 
            this.BLabel.AutoSize = true;
            this.BLabel.Location = new System.Drawing.Point(13, 67);
            this.BLabel.Name = "BLabel";
            this.BLabel.Size = new System.Drawing.Size(13, 13);
            this.BLabel.TabIndex = 6;
            this.BLabel.Text = "b";
            // 
            // ALabel
            // 
            this.ALabel.AutoSize = true;
            this.ALabel.Location = new System.Drawing.Point(13, 28);
            this.ALabel.Name = "ALabel";
            this.ALabel.Size = new System.Drawing.Size(13, 13);
            this.ALabel.TabIndex = 5;
            this.ALabel.Text = "a";
            // 
            // BErrorTextBox
            // 
            this.BErrorTextBox.Location = new System.Drawing.Point(218, 64);
            this.BErrorTextBox.Name = "BErrorTextBox";
            this.BErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.BErrorTextBox.TabIndex = 4;
            this.BErrorTextBox.TextChanged += new System.EventHandler(this.BErrorTextBox_TextChanged);
            // 
            // AErrorTextBox
            // 
            this.AErrorTextBox.Location = new System.Drawing.Point(218, 25);
            this.AErrorTextBox.Name = "AErrorTextBox";
            this.AErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.AErrorTextBox.TabIndex = 2;
            this.AErrorTextBox.TextChanged += new System.EventHandler(this.AErrorTextBox_TextChanged);
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(32, 64);
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.TabIndex = 1;
            this.BTextBox.TextChanged += new System.EventHandler(this.BTextBox_TextChanged);
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(32, 25);
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.TabIndex = 0;
            this.ATextBox.TextChanged += new System.EventHandler(this.ATextBox_TextChanged);
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(506, 350);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 23);
            this.NextBtn.TabIndex = 3;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(506, 379);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(506, 408);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // NumRodsTextBox
            // 
            this.NumRodsTextBox.Location = new System.Drawing.Point(147, 12);
            this.NumRodsTextBox.Name = "NumRodsTextBox";
            this.NumRodsTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumRodsTextBox.TabIndex = 6;
            this.NumRodsTextBox.TextChanged += new System.EventHandler(this.NumRodsTextBox_TextChanged);
            // 
            // NumRodsLabel
            // 
            this.NumRodsLabel.AutoSize = true;
            this.NumRodsLabel.Location = new System.Drawing.Point(11, 15);
            this.NumRodsLabel.Name = "NumRodsLabel";
            this.NumRodsLabel.Size = new System.Drawing.Size(130, 13);
            this.NumRodsLabel.TabIndex = 7;
            this.NumRodsLabel.Text = "Number of calibration rods";
            // 
            // K3Label
            // 
            this.K3Label.AutoSize = true;
            this.K3Label.Location = new System.Drawing.Point(12, 107);
            this.K3Label.Name = "K3Label";
            this.K3Label.Size = new System.Drawing.Size(145, 13);
            this.K3Label.TabIndex = 8;
            this.K3Label.Text = "K3 - Poison Correction Factor";
            // 
            // IDDCorrectionFactors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 463);
            this.Controls.Add(this.K3Label);
            this.Controls.Add(this.NumRodsLabel);
            this.Controls.Add(this.NumRodsTextBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.K4GroupBox);
            this.Controls.Add(this.MDMGroupBox);
            this.Controls.Add(this.listView1);
            this.Name = "IDDCorrectionFactors";
            this.Text = "Correction Factors";
            this.MDMGroupBox.ResumeLayout(false);
            this.MDMGroupBox.PerformLayout();
            this.K4GroupBox.ResumeLayout(false);
            this.K4GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox MDMGroupBox;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.Label DetectorLabel;
        private System.Windows.Forms.Label MaterialLabel;
        private System.Windows.Forms.GroupBox K4GroupBox;
        private System.Windows.Forms.Label BErrorLabel;
        private System.Windows.Forms.Label AErrorLabel;
        private System.Windows.Forms.Label BLabel;
        private System.Windows.Forms.Label ALabel;
        private System.Windows.Forms.TextBox BErrorTextBox;
        private System.Windows.Forms.TextBox AErrorTextBox;
        private System.Windows.Forms.TextBox BTextBox;
        private System.Windows.Forms.TextBox ATextBox;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.TextBox NumRodsTextBox;
        private System.Windows.Forms.Label NumRodsLabel;
        private System.Windows.Forms.Label K3Label;
        private System.Windows.Forms.ColumnHeader RodType;
        private System.Windows.Forms.ColumnHeader AbsFact;
        private System.Windows.Forms.ColumnHeader A;
        private System.Windows.Forms.ColumnHeader AError;
        private System.Windows.Forms.ColumnHeader B;
        private System.Windows.Forms.ColumnHeader BError;
        private System.Windows.Forms.ColumnHeader C;
        private System.Windows.Forms.ColumnHeader CError;
    }
}