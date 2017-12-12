namespace UI
{
    partial class IDDReanalysisAssay    
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
			this.ItemIdComboBox = new System.Windows.Forms.ComboBox();
			this.StratumIdComboBox = new System.Windows.Forms.ComboBox();
			this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
			this.InventoryChangeCodeComboBox = new System.Windows.Forms.ComboBox();
			this.IOCodeComboBox = new System.Windows.Forms.ComboBox();
			this.DeclaredMassTextBox = new System.Windows.Forms.TextBox();
			this.CommentTextBox = new System.Windows.Forms.TextBox();
			this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
			this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.IsotopicsBtn = new System.Windows.Forms.Button();
			this.BackgroundBtn = new System.Windows.Forms.Button();
			this.ItemIdLabel = new System.Windows.Forms.Label();
			this.StratumIdLabel = new System.Windows.Forms.Label();
			this.MaterialTypeLabel = new System.Windows.Forms.Label();
			this.DeclaredMassLabel = new System.Windows.Forms.Label();
			this.CommentLabel = new System.Windows.Forms.Label();
			this.InventoryChangeCodeLabel = new System.Windows.Forms.Label();
			this.IOCodeLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.UseCurrentCalibCheckBox = new System.Windows.Forms.CheckBox();
			this.ReplaceOriginalCheckBox = new System.Windows.Forms.CheckBox();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.NormConstErr = new System.Windows.Forms.TextBox();
			this.NormConst = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// ItemIdComboBox
			// 
			this.ItemIdComboBox.FormattingEnabled = true;
			this.ItemIdComboBox.Location = new System.Drawing.Point(161, 46);
			this.ItemIdComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.ItemIdComboBox.Name = "ItemIdComboBox";
			this.ItemIdComboBox.Size = new System.Drawing.Size(357, 24);
			this.ItemIdComboBox.TabIndex = 1;
			this.ItemIdComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIdComboBox_SelectedIndexChanged);
			this.ItemIdComboBox.Leave += new System.EventHandler(this.ItemIdComboBox_Leave);
			// 
			// StratumIdComboBox
			// 
			this.StratumIdComboBox.FormattingEnabled = true;
			this.StratumIdComboBox.Location = new System.Drawing.Point(161, 78);
			this.StratumIdComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.StratumIdComboBox.Name = "StratumIdComboBox";
			this.StratumIdComboBox.Size = new System.Drawing.Size(357, 24);
			this.StratumIdComboBox.TabIndex = 2;
			this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
			// 
			// MaterialTypeComboBox
			// 
			this.MaterialTypeComboBox.FormattingEnabled = true;
			this.MaterialTypeComboBox.Location = new System.Drawing.Point(161, 111);
			this.MaterialTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
			this.MaterialTypeComboBox.Size = new System.Drawing.Size(357, 24);
			this.MaterialTypeComboBox.TabIndex = 3;
			this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
			// 
			// InventoryChangeCodeComboBox
			// 
			this.InventoryChangeCodeComboBox.FormattingEnabled = true;
			this.InventoryChangeCodeComboBox.Location = new System.Drawing.Point(632, 7);
			this.InventoryChangeCodeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.InventoryChangeCodeComboBox.Name = "InventoryChangeCodeComboBox";
			this.InventoryChangeCodeComboBox.Size = new System.Drawing.Size(80, 24);
			this.InventoryChangeCodeComboBox.TabIndex = 5;
			this.InventoryChangeCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.InventoryChangeCodeComboBox_SelectedIndexChanged);
			// 
			// IOCodeComboBox
			// 
			this.IOCodeComboBox.FormattingEnabled = true;
			this.IOCodeComboBox.Location = new System.Drawing.Point(632, 41);
			this.IOCodeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.IOCodeComboBox.Name = "IOCodeComboBox";
			this.IOCodeComboBox.Size = new System.Drawing.Size(80, 24);
			this.IOCodeComboBox.TabIndex = 6;
			this.IOCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IOCodeComboBox_SelectedIndexChanged);
			// 
			// DeclaredMassTextBox
			// 
			this.DeclaredMassTextBox.Location = new System.Drawing.Point(208, 148);
			this.DeclaredMassTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
			this.DeclaredMassTextBox.Size = new System.Drawing.Size(103, 22);
			this.DeclaredMassTextBox.TabIndex = 7;
			this.DeclaredMassTextBox.Leave += new System.EventHandler(this.DeclaredMassTextBox_Leave);
			// 
			// CommentTextBox
			// 
			this.CommentTextBox.Location = new System.Drawing.Point(161, 235);
			this.CommentTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.CommentTextBox.Name = "CommentTextBox";
			this.CommentTextBox.Size = new System.Drawing.Size(357, 22);
			this.CommentTextBox.TabIndex = 8;
			this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
			// 
			// QCTestsCheckBox
			// 
			this.QCTestsCheckBox.AutoSize = true;
			this.QCTestsCheckBox.Location = new System.Drawing.Point(161, 347);
			this.QCTestsCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.QCTestsCheckBox.Name = "QCTestsCheckBox";
			this.QCTestsCheckBox.Size = new System.Drawing.Size(84, 21);
			this.QCTestsCheckBox.TabIndex = 16;
			this.QCTestsCheckBox.Text = "QC tests";
			this.QCTestsCheckBox.UseVisualStyleBackColor = true;
			this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
			// 
			// PrintResultsCheckBox
			// 
			this.PrintResultsCheckBox.AutoSize = true;
			this.PrintResultsCheckBox.Location = new System.Drawing.Point(161, 375);
			this.PrintResultsCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
			this.PrintResultsCheckBox.Size = new System.Drawing.Size(105, 21);
			this.PrintResultsCheckBox.TabIndex = 17;
			this.PrintResultsCheckBox.Text = "Print results";
			this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
			this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
			// 
			// CommentAtEndCheckBox
			// 
			this.CommentAtEndCheckBox.AutoSize = true;
			this.CommentAtEndCheckBox.Location = new System.Drawing.Point(161, 262);
			this.CommentAtEndCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
			this.CommentAtEndCheckBox.Size = new System.Drawing.Size(239, 21);
			this.CommentAtEndCheckBox.TabIndex = 18;
			this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
			this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
			this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(720, 4);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 20;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(720, 41);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 21;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(720, 78);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 22;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// IsotopicsBtn
			// 
			this.IsotopicsBtn.Location = new System.Drawing.Point(604, 137);
			this.IsotopicsBtn.Margin = new System.Windows.Forms.Padding(4);
			this.IsotopicsBtn.Name = "IsotopicsBtn";
			this.IsotopicsBtn.Size = new System.Drawing.Size(108, 28);
			this.IsotopicsBtn.TabIndex = 23;
			this.IsotopicsBtn.Text = "Isotopics...";
			this.IsotopicsBtn.UseVisualStyleBackColor = true;
			this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
			// 
			// BackgroundBtn
			// 
			this.BackgroundBtn.Location = new System.Drawing.Point(605, 174);
			this.BackgroundBtn.Margin = new System.Windows.Forms.Padding(4);
			this.BackgroundBtn.Name = "BackgroundBtn";
			this.BackgroundBtn.Size = new System.Drawing.Size(107, 28);
			this.BackgroundBtn.TabIndex = 24;
			this.BackgroundBtn.Text = "Background...";
			this.BackgroundBtn.UseVisualStyleBackColor = true;
			this.BackgroundBtn.Click += new System.EventHandler(this.BackgroundBtn_Click);
			// 
			// ItemIdLabel
			// 
			this.ItemIdLabel.AutoSize = true;
			this.ItemIdLabel.Location = new System.Drawing.Point(97, 49);
			this.ItemIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ItemIdLabel.Name = "ItemIdLabel";
			this.ItemIdLabel.Size = new System.Drawing.Size(49, 17);
			this.ItemIdLabel.TabIndex = 27;
			this.ItemIdLabel.Text = "Item id";
			// 
			// StratumIdLabel
			// 
			this.StratumIdLabel.AutoSize = true;
			this.StratumIdLabel.Location = new System.Drawing.Point(76, 82);
			this.StratumIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.StratumIdLabel.Name = "StratumIdLabel";
			this.StratumIdLabel.Size = new System.Drawing.Size(72, 17);
			this.StratumIdLabel.TabIndex = 28;
			this.StratumIdLabel.Text = "Stratum id";
			// 
			// MaterialTypeLabel
			// 
			this.MaterialTypeLabel.AutoSize = true;
			this.MaterialTypeLabel.Location = new System.Drawing.Point(59, 114);
			this.MaterialTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MaterialTypeLabel.Name = "MaterialTypeLabel";
			this.MaterialTypeLabel.Size = new System.Drawing.Size(89, 17);
			this.MaterialTypeLabel.TabIndex = 29;
			this.MaterialTypeLabel.Text = "Material type";
			// 
			// DeclaredMassLabel
			// 
			this.DeclaredMassLabel.AutoSize = true;
			this.DeclaredMassLabel.Location = new System.Drawing.Point(75, 148);
			this.DeclaredMassLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DeclaredMassLabel.Name = "DeclaredMassLabel";
			this.DeclaredMassLabel.Size = new System.Drawing.Size(124, 17);
			this.DeclaredMassLabel.TabIndex = 30;
			this.DeclaredMassLabel.Text = "Declared mass (g)";
			// 
			// CommentLabel
			// 
			this.CommentLabel.AutoSize = true;
			this.CommentLabel.Location = new System.Drawing.Point(80, 238);
			this.CommentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.CommentLabel.Name = "CommentLabel";
			this.CommentLabel.Size = new System.Drawing.Size(67, 17);
			this.CommentLabel.TabIndex = 31;
			this.CommentLabel.Text = "Comment";
			// 
			// InventoryChangeCodeLabel
			// 
			this.InventoryChangeCodeLabel.AutoSize = true;
			this.InventoryChangeCodeLabel.Location = new System.Drawing.Point(468, 11);
			this.InventoryChangeCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InventoryChangeCodeLabel.Name = "InventoryChangeCodeLabel";
			this.InventoryChangeCodeLabel.Size = new System.Drawing.Size(152, 17);
			this.InventoryChangeCodeLabel.TabIndex = 40;
			this.InventoryChangeCodeLabel.Text = "Inventory change code";
			// 
			// IOCodeLabel
			// 
			this.IOCodeLabel.AutoSize = true;
			this.IOCodeLabel.Location = new System.Drawing.Point(557, 46);
			this.IOCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IOCodeLabel.Name = "IOCodeLabel";
			this.IOCodeLabel.Size = new System.Drawing.Size(61, 17);
			this.IOCodeLabel.TabIndex = 41;
			this.IOCodeLabel.Text = "I/O code";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 15);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(126, 17);
			this.label1.TabIndex = 43;
			this.label1.Text = "Measurement date";
			// 
			// UseCurrentCalibCheckBox
			// 
			this.UseCurrentCalibCheckBox.AutoSize = true;
			this.UseCurrentCalibCheckBox.Location = new System.Drawing.Point(161, 290);
			this.UseCurrentCalibCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.UseCurrentCalibCheckBox.Name = "UseCurrentCalibCheckBox";
			this.UseCurrentCalibCheckBox.Size = new System.Drawing.Size(249, 21);
			this.UseCurrentCalibCheckBox.TabIndex = 44;
			this.UseCurrentCalibCheckBox.Text = "Use current calibration parameters";
			this.toolTip1.SetToolTip(this.UseCurrentCalibCheckBox, "x = use the current calibration parameters for the current material type.\r\nblank " +
        "= use the calibration parameters from the original measurement");
			this.UseCurrentCalibCheckBox.UseVisualStyleBackColor = true;
			// 
			// ReplaceOriginalCheckBox
			// 
			this.ReplaceOriginalCheckBox.AutoSize = true;
			this.ReplaceOriginalCheckBox.Enabled = false;
			this.ReplaceOriginalCheckBox.Location = new System.Drawing.Point(161, 319);
			this.ReplaceOriginalCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.ReplaceOriginalCheckBox.Name = "ReplaceOriginalCheckBox";
			this.ReplaceOriginalCheckBox.Size = new System.Drawing.Size(294, 21);
			this.ReplaceOriginalCheckBox.TabIndex = 45;
			this.ReplaceOriginalCheckBox.Text = "Replace original verification measurement";
			this.ReplaceOriginalCheckBox.UseVisualStyleBackColor = true;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(161, 7);
			this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(265, 22);
			this.dateTimePicker1.TabIndex = 46;
			// 
			// NormConstErr
			// 
			this.NormConstErr.Location = new System.Drawing.Point(208, 206);
			this.NormConstErr.Margin = new System.Windows.Forms.Padding(4);
			this.NormConstErr.Name = "NormConstErr";
			this.NormConstErr.Size = new System.Drawing.Size(103, 22);
			this.NormConstErr.TabIndex = 47;
			this.NormConstErr.Leave += new System.EventHandler(this.NormConstErr_Leave);
			// 
			// NormConst
			// 
			this.NormConst.Location = new System.Drawing.Point(208, 176);
			this.NormConst.Margin = new System.Windows.Forms.Padding(4);
			this.NormConst.Name = "NormConst";
			this.NormConst.Size = new System.Drawing.Size(103, 22);
			this.NormConst.TabIndex = 48;
			this.NormConst.Leave += new System.EventHandler(this.NormConst_Leave);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 206);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(187, 17);
			this.label2.TabIndex = 49;
			this.label2.Text = "Normalization constant error";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(47, 176);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 17);
			this.label3.TabIndex = 50;
			this.label3.Text = "Normalization constant";
			// 
			// IDDReanalysisAssay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(825, 405);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.NormConst);
			this.Controls.Add(this.NormConstErr);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.ReplaceOriginalCheckBox);
			this.Controls.Add(this.UseCurrentCalibCheckBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.IOCodeLabel);
			this.Controls.Add(this.InventoryChangeCodeLabel);
			this.Controls.Add(this.CommentLabel);
			this.Controls.Add(this.DeclaredMassLabel);
			this.Controls.Add(this.MaterialTypeLabel);
			this.Controls.Add(this.StratumIdLabel);
			this.Controls.Add(this.ItemIdLabel);
			this.Controls.Add(this.BackgroundBtn);
			this.Controls.Add(this.IsotopicsBtn);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.CommentAtEndCheckBox);
			this.Controls.Add(this.PrintResultsCheckBox);
			this.Controls.Add(this.QCTestsCheckBox);
			this.Controls.Add(this.CommentTextBox);
			this.Controls.Add(this.DeclaredMassTextBox);
			this.Controls.Add(this.IOCodeComboBox);
			this.Controls.Add(this.InventoryChangeCodeComboBox);
			this.Controls.Add(this.MaterialTypeComboBox);
			this.Controls.Add(this.StratumIdComboBox);
			this.Controls.Add(this.ItemIdComboBox);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IDDReanalysisAssay";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " ";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox ItemIdComboBox;
        private System.Windows.Forms.ComboBox StratumIdComboBox;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox InventoryChangeCodeComboBox;
        private System.Windows.Forms.ComboBox IOCodeComboBox;
        private System.Windows.Forms.TextBox DeclaredMassTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button BackgroundBtn;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label DeclaredMassLabel;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Label InventoryChangeCodeLabel;
        private System.Windows.Forms.Label IOCodeLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox UseCurrentCalibCheckBox;
		private System.Windows.Forms.CheckBox ReplaceOriginalCheckBox;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.TextBox NormConstErr;
		private System.Windows.Forms.TextBox NormConst;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}