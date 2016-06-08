namespace NewUI
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
            this.ItemIdComboBox.Location = new System.Drawing.Point(121, 37);
            this.ItemIdComboBox.Name = "ItemIdComboBox";
            this.ItemIdComboBox.Size = new System.Drawing.Size(269, 21);
            this.ItemIdComboBox.TabIndex = 1;
            this.ItemIdComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIdComboBox_SelectedIndexChanged);
            this.ItemIdComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemIdComboBox_KeyDown);
            this.ItemIdComboBox.Leave += new System.EventHandler(this.ItemIdComboBox_Leave);
            // 
            // StratumIdComboBox
            // 
            this.StratumIdComboBox.FormattingEnabled = true;
            this.StratumIdComboBox.Location = new System.Drawing.Point(121, 63);
            this.StratumIdComboBox.Name = "StratumIdComboBox";
            this.StratumIdComboBox.Size = new System.Drawing.Size(269, 21);
            this.StratumIdComboBox.TabIndex = 2;
            this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
            this.StratumIdComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StratumIdComboBox_KeyPress);
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(121, 90);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(269, 21);
            this.MaterialTypeComboBox.TabIndex = 3;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            this.MaterialTypeComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MaterialTypeComboBox_KeyPress);
            // 
            // InventoryChangeCodeComboBox
            // 
            this.InventoryChangeCodeComboBox.FormattingEnabled = true;
            this.InventoryChangeCodeComboBox.Location = new System.Drawing.Point(474, 6);
            this.InventoryChangeCodeComboBox.Name = "InventoryChangeCodeComboBox";
            this.InventoryChangeCodeComboBox.Size = new System.Drawing.Size(61, 21);
            this.InventoryChangeCodeComboBox.TabIndex = 5;
            this.InventoryChangeCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.InventoryChangeCodeComboBox_SelectedIndexChanged);
            // 
            // IOCodeComboBox
            // 
            this.IOCodeComboBox.FormattingEnabled = true;
            this.IOCodeComboBox.Location = new System.Drawing.Point(474, 33);
            this.IOCodeComboBox.Name = "IOCodeComboBox";
            this.IOCodeComboBox.Size = new System.Drawing.Size(61, 21);
            this.IOCodeComboBox.TabIndex = 6;
            this.IOCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IOCodeComboBox_SelectedIndexChanged);
            // 
            // DeclaredMassTextBox
            // 
            this.DeclaredMassTextBox.Location = new System.Drawing.Point(156, 120);
            this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
            this.DeclaredMassTextBox.Size = new System.Drawing.Size(78, 20);
            this.DeclaredMassTextBox.TabIndex = 7;
            this.DeclaredMassTextBox.Leave += new System.EventHandler(this.DeclaredMassTextBox_Leave);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(121, 191);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(269, 20);
            this.CommentTextBox.TabIndex = 8;
            this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(121, 282);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 16;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(121, 305);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 17;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(121, 213);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 18;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(540, 3);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 20;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(540, 33);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 21;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(540, 63);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 22;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IsotopicsBtn
            // 
            this.IsotopicsBtn.Location = new System.Drawing.Point(453, 111);
            this.IsotopicsBtn.Name = "IsotopicsBtn";
            this.IsotopicsBtn.Size = new System.Drawing.Size(81, 23);
            this.IsotopicsBtn.TabIndex = 23;
            this.IsotopicsBtn.Text = "Isotopics...";
            this.IsotopicsBtn.UseVisualStyleBackColor = true;
            this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
            // 
            // BackgroundBtn
            // 
            this.BackgroundBtn.Location = new System.Drawing.Point(454, 141);
            this.BackgroundBtn.Name = "BackgroundBtn";
            this.BackgroundBtn.Size = new System.Drawing.Size(80, 23);
            this.BackgroundBtn.TabIndex = 24;
            this.BackgroundBtn.Text = "Background...";
            this.BackgroundBtn.UseVisualStyleBackColor = true;
            this.BackgroundBtn.Click += new System.EventHandler(this.BackgroundBtn_Click);
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(73, 40);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(38, 13);
            this.ItemIdLabel.TabIndex = 27;
            this.ItemIdLabel.Text = "Item id";
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Location = new System.Drawing.Point(57, 67);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(54, 13);
            this.StratumIdLabel.TabIndex = 28;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(44, 93);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 29;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // DeclaredMassLabel
            // 
            this.DeclaredMassLabel.AutoSize = true;
            this.DeclaredMassLabel.Location = new System.Drawing.Point(56, 120);
            this.DeclaredMassLabel.Name = "DeclaredMassLabel";
            this.DeclaredMassLabel.Size = new System.Drawing.Size(92, 13);
            this.DeclaredMassLabel.TabIndex = 30;
            this.DeclaredMassLabel.Text = "Declared mass (g)";
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(60, 193);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 31;
            this.CommentLabel.Text = "Comment";
            // 
            // InventoryChangeCodeLabel
            // 
            this.InventoryChangeCodeLabel.AutoSize = true;
            this.InventoryChangeCodeLabel.Location = new System.Drawing.Point(351, 9);
            this.InventoryChangeCodeLabel.Name = "InventoryChangeCodeLabel";
            this.InventoryChangeCodeLabel.Size = new System.Drawing.Size(117, 13);
            this.InventoryChangeCodeLabel.TabIndex = 40;
            this.InventoryChangeCodeLabel.Text = "Inventory change code";
            // 
            // IOCodeLabel
            // 
            this.IOCodeLabel.AutoSize = true;
            this.IOCodeLabel.Location = new System.Drawing.Point(418, 37);
            this.IOCodeLabel.Name = "IOCodeLabel";
            this.IOCodeLabel.Size = new System.Drawing.Size(50, 13);
            this.IOCodeLabel.TabIndex = 41;
            this.IOCodeLabel.Text = "I/O code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Measurement date";
            // 
            // UseCurrentCalibCheckBox
            // 
            this.UseCurrentCalibCheckBox.AutoSize = true;
            this.UseCurrentCalibCheckBox.Enabled = false;
            this.UseCurrentCalibCheckBox.Location = new System.Drawing.Point(121, 236);
            this.UseCurrentCalibCheckBox.Name = "UseCurrentCalibCheckBox";
            this.UseCurrentCalibCheckBox.Size = new System.Drawing.Size(187, 17);
            this.UseCurrentCalibCheckBox.TabIndex = 44;
            this.UseCurrentCalibCheckBox.Text = "Use current calibration parameters";
            this.toolTip1.SetToolTip(this.UseCurrentCalibCheckBox, "Use of original parameters NYI");
            this.UseCurrentCalibCheckBox.UseVisualStyleBackColor = true;
            this.UseCurrentCalibCheckBox.CheckedChanged += new System.EventHandler(this.UseCurrentCalibCheckBox_CheckedChanged);
            // 
            // ReplaceOriginalCheckBox
            // 
            this.ReplaceOriginalCheckBox.AutoSize = true;
            this.ReplaceOriginalCheckBox.Location = new System.Drawing.Point(121, 259);
            this.ReplaceOriginalCheckBox.Name = "ReplaceOriginalCheckBox";
            this.ReplaceOriginalCheckBox.Size = new System.Drawing.Size(222, 17);
            this.ReplaceOriginalCheckBox.TabIndex = 45;
            this.ReplaceOriginalCheckBox.Text = "Replace original verification measurement";
            this.ReplaceOriginalCheckBox.UseVisualStyleBackColor = true;
            this.ReplaceOriginalCheckBox.CheckedChanged += new System.EventHandler(this.ReplaceOriginalCheckBox_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(121, 6);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 46;
            // 
            // NormConstErr
            // 
            this.NormConstErr.Location = new System.Drawing.Point(156, 167);
            this.NormConstErr.Name = "NormConstErr";
            this.NormConstErr.Size = new System.Drawing.Size(78, 20);
            this.NormConstErr.TabIndex = 47;
            this.NormConstErr.Leave += new System.EventHandler(this.NormConstErr_Leave);
            // 
            // NormConst
            // 
            this.NormConst.Location = new System.Drawing.Point(156, 143);
            this.NormConst.Name = "NormConst";
            this.NormConst.Size = new System.Drawing.Size(78, 20);
            this.NormConst.TabIndex = 48;
            this.NormConst.Leave += new System.EventHandler(this.NormConst_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Normalization constant error";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 50;
            this.label3.Text = "Normalization constant";
            // 
            // IDDReanalysisAssay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 329);
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