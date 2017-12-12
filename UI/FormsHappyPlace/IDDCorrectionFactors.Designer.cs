namespace UI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MDMGroupBox = new System.Windows.Forms.GroupBox();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.DetectorLabel = new System.Windows.Forms.Label();
            this.MaterialLabel = new System.Windows.Forms.Label();
            this.K4GroupBox = new System.Windows.Forms.GroupBox();
            this.BErrorLabel = new System.Windows.Forms.Label();
            this.AErrorLabel = new System.Windows.Forms.Label();
            this.BLabel = new System.Windows.Forms.Label();
            this.ALabel = new System.Windows.Forms.Label();
            this.BErrorTextBox = new UI.NumericTextBox();
            this.AErrorTextBox = new UI.NumericTextBox();
            this.BTextBox = new UI.NumericTextBox();
            this.ATextBox = new UI.NumericTextBox();
            this.NextBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.K3Label = new System.Windows.Forms.Label();
            this.BackBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NumRods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AbsFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.a = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aerr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.b = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.berr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cerr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.provider = new System.Windows.Forms.HelpProvider();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MDMGroupBox.SuspendLayout();
            this.K4GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // MDMGroupBox
            // 
            this.MDMGroupBox.Controls.Add(this.ModeLabel);
            this.MDMGroupBox.Controls.Add(this.DetectorLabel);
            this.MDMGroupBox.Controls.Add(this.MaterialLabel);
            this.MDMGroupBox.Location = new System.Drawing.Point(674, 15);
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
            this.DetectorLabel.Location = new System.Drawing.Point(17, 44);
            this.DetectorLabel.Name = "DetectorLabel";
            this.DetectorLabel.Size = new System.Drawing.Size(80, 13);
            this.DetectorLabel.TabIndex = 1;
            this.DetectorLabel.Text = "XXXX/XXX/YY";
            // 
            // MaterialLabel
            // 
            this.MaterialLabel.AutoSize = true;
            this.MaterialLabel.Location = new System.Drawing.Point(17, 22);
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
            this.K4GroupBox.Location = new System.Drawing.Point(15, 233);
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
            this.BErrorTextBox.Max = 1.7976931348623157E+308D;
            this.BErrorTextBox.Min = 0D;
            this.BErrorTextBox.Name = "BErrorTextBox";
            this.BErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.BErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.BErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.BErrorTextBox.Steps = -1D;
            this.BErrorTextBox.TabIndex = 4;
            this.BErrorTextBox.Text = "0.000000E+000";
            this.BErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.BErrorTextBox.Value = 0D;
            // 
            // AErrorTextBox
            // 
            this.AErrorTextBox.Location = new System.Drawing.Point(218, 25);
            this.AErrorTextBox.Max = 1.7976931348623157E+308D;
            this.AErrorTextBox.Min = 0D;
            this.AErrorTextBox.Name = "AErrorTextBox";
            this.AErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.AErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.AErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.AErrorTextBox.Steps = -1D;
            this.AErrorTextBox.TabIndex = 2;
            this.AErrorTextBox.Text = "0.000000E+000";
            this.AErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.AErrorTextBox.Value = 0D;
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(32, 64);
            this.BTextBox.Max = 1.7976931348623157E+308D;
            this.BTextBox.Min = 0D;
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.BTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.Steps = -1D;
            this.BTextBox.TabIndex = 3;
            this.BTextBox.Text = "0.000000E+000";
            this.BTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.BTextBox.Value = 0D;
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(32, 25);
            this.ATextBox.Max = 1.7976931348623157E+308D;
            this.ATextBox.Min = 0D;
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.Steps = -1D;
            this.ATextBox.TabIndex = 1;
            this.ATextBox.Text = "0.000000E+000";
            this.ATextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ATextBox.Value = 0D;
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(745, 251);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 23);
            this.NextBtn.TabIndex = 3;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(745, 280);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(745, 309);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
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
            // BackBtn
            // 
            this.BackBtn.Location = new System.Drawing.Point(745, 222);
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(75, 23);
            this.BackBtn.TabIndex = 9;
            this.BackBtn.Text = "Back";
            this.BackBtn.UseVisualStyleBackColor = true;
            this.BackBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.NumRods,
            this.AbsFactor,
            this.a,
            this.aerr,
            this.b,
            this.berr,
            this.c,
            this.cerr});
            this.dataGridView1.Location = new System.Drawing.Point(12, 107);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(817, 98);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            // 
            // Type
            // 
            this.Type.HeaderText = "Poison Rod Type";
            this.Type.Name = "Type";
            this.Type.Width = 50;
            // 
            // NumRods
            // 
            this.NumRods.HeaderText = "# of Rods";
            this.NumRods.Name = "NumRods";
            this.NumRods.Width = 50;
            // 
            // AbsFactor
            // 
            dataGridViewCellStyle1.Format = "E3";
            this.AbsFactor.DefaultCellStyle = dataGridViewCellStyle1;
            this.AbsFactor.HeaderText = "Poison Rod Absorption Factor";
            this.AbsFactor.Name = "AbsFactor";
            this.AbsFactor.ReadOnly = true;
            this.AbsFactor.Width = 75;
            // 
            // a
            // 
            dataGridViewCellStyle2.Format = "E3";
            this.a.DefaultCellStyle = dataGridViewCellStyle2;
            this.a.HeaderText = "a";
            this.a.Name = "a";
            // 
            // aerr
            // 
            dataGridViewCellStyle3.Format = "E3";
            this.aerr.DefaultCellStyle = dataGridViewCellStyle3;
            this.aerr.HeaderText = "a error";
            this.aerr.Name = "aerr";
            // 
            // b
            // 
            dataGridViewCellStyle4.Format = "E3";
            this.b.DefaultCellStyle = dataGridViewCellStyle4;
            this.b.HeaderText = "b";
            this.b.Name = "b";
            // 
            // berr
            // 
            dataGridViewCellStyle5.Format = "E3";
            this.berr.DefaultCellStyle = dataGridViewCellStyle5;
            this.berr.HeaderText = "b error";
            this.berr.Name = "berr";
            // 
            // c
            // 
            dataGridViewCellStyle6.Format = "E3";
            this.c.DefaultCellStyle = dataGridViewCellStyle6;
            this.c.HeaderText = "c";
            this.c.Name = "c";
            // 
            // cerr
            // 
            dataGridViewCellStyle7.Format = "E3";
            this.cerr.DefaultCellStyle = dataGridViewCellStyle7;
            this.cerr.HeaderText = "c error";
            this.cerr.Name = "cerr";
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle8.Format = "E3";
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn1.HeaderText = "Poison Rod Absorption Factor";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn2.HeaderText = "a";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 75;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn3.HeaderText = "a error";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn4.HeaderText = "b";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn5.HeaderText = "b error";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn6.HeaderText = "c";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn7.HeaderText = "c error";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle9.Format = "E3";
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn8.HeaderText = "c error";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // IDDCorrectionFactors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 352);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.BackBtn);
            this.Controls.Add(this.K3Label);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.K4GroupBox);
            this.Controls.Add(this.MDMGroupBox);
            this.Name = "IDDCorrectionFactors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Correction Factors";
            this.MDMGroupBox.ResumeLayout(false);
            this.MDMGroupBox.PerformLayout();
            this.K4GroupBox.ResumeLayout(false);
            this.K4GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox MDMGroupBox;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.Label DetectorLabel;
        private System.Windows.Forms.Label MaterialLabel;
        private System.Windows.Forms.GroupBox K4GroupBox;
        private System.Windows.Forms.Label BErrorLabel;
        private System.Windows.Forms.Label AErrorLabel;
        private System.Windows.Forms.Label BLabel;
        private System.Windows.Forms.Label ALabel;
        private NumericTextBox BErrorTextBox;
        private NumericTextBox AErrorTextBox;
        private NumericTextBox BTextBox;
        private NumericTextBox ATextBox;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label K3Label;
        private System.Windows.Forms.Button BackBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.HelpProvider provider;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumRods;
        private System.Windows.Forms.DataGridViewTextBoxColumn AbsFactor;
        private System.Windows.Forms.DataGridViewTextBoxColumn a;
        private System.Windows.Forms.DataGridViewTextBoxColumn aerr;
        private System.Windows.Forms.DataGridViewTextBoxColumn b;
        private System.Windows.Forms.DataGridViewTextBoxColumn berr;
        private System.Windows.Forms.DataGridViewTextBoxColumn c;
        private System.Windows.Forms.DataGridViewTextBoxColumn cerr;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    }
}