namespace UI
{
    partial class IDDAcquireAssay
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
			this.MBAComboBox = new System.Windows.Forms.ComboBox();
			this.ItemIdComboBox = new System.Windows.Forms.ComboBox();
			this.StratumIdComboBox = new System.Windows.Forms.ComboBox();
			this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
			this.DataSourceComboBox = new System.Windows.Forms.ComboBox();
			this.InventoryChangeCodeComboBox = new System.Windows.Forms.ComboBox();
			this.IOCodeComboBox = new System.Windows.Forms.ComboBox();
			this.DeclaredMassTextBox = new System.Windows.Forms.TextBox();
			this.CommentTextBox = new System.Windows.Forms.TextBox();
			this.CountTimeTextBox = new System.Windows.Forms.TextBox();
			this.NumPassiveCyclesTextBox = new System.Windows.Forms.TextBox();
			this.MeasPrecisionTextBox = new System.Windows.Forms.TextBox();
			this.MinNumCyclesTextBox = new System.Windows.Forms.TextBox();
			this.MaxNumCyclesTextBox = new System.Windows.Forms.TextBox();
			this.NumActiveCyclesTextBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.UsePu240eRadioButton = new System.Windows.Forms.RadioButton();
			this.UseTriplesRadioButton = new System.Windows.Forms.RadioButton();
			this.UseDoublesRadioButton = new System.Windows.Forms.RadioButton();
			this.UseNumCyclesRadioButton = new System.Windows.Forms.RadioButton();
			this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
			this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
			this.DrumWeightTextBox = new System.Windows.Forms.TextBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.IsotopicsBtn = new System.Windows.Forms.Button();
			this.CompositeIsotopicsBtn = new System.Windows.Forms.Button();
			this.Pu240eCoeffBtn = new System.Windows.Forms.Button();
			this.MBALabel = new System.Windows.Forms.Label();
			this.ItemIdLabel = new System.Windows.Forms.Label();
			this.StratumIdLabel = new System.Windows.Forms.Label();
			this.MaterialTypeLabel = new System.Windows.Forms.Label();
			this.DeclaredMassLabel = new System.Windows.Forms.Label();
			this.CommentLabel = new System.Windows.Forms.Label();
			this.CountTimeLabel = new System.Windows.Forms.Label();
			this.NumPassiveCyclesLabel = new System.Windows.Forms.Label();
			this.MeasPrecisionLabel = new System.Windows.Forms.Label();
			this.MinNumCyclesLabel = new System.Windows.Forms.Label();
			this.MaxNumCyclesLabel = new System.Windows.Forms.Label();
			this.NumActiveCyclesLabel = new System.Windows.Forms.Label();
			this.DrumWeightLabel = new System.Windows.Forms.Label();
			this.DataSourceLabel = new System.Windows.Forms.Label();
			this.InventoryChangeCodeLabel = new System.Windows.Forms.Label();
			this.IOCodeLabel = new System.Windows.Forms.Label();
			this.MaterialTypeHelpBtn = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// MBAComboBox
			// 
			this.MBAComboBox.FormattingEnabled = true;
			this.MBAComboBox.Location = new System.Drawing.Point(195, 16);
			this.MBAComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.MBAComboBox.Name = "MBAComboBox";
			this.MBAComboBox.Size = new System.Drawing.Size(357, 24);
			this.MBAComboBox.TabIndex = 0;
			this.MBAComboBox.SelectedIndexChanged += new System.EventHandler(this.MBAComboBox_SelectedIndexChanged);
			// 
			// ItemIdComboBox
			// 
			this.ItemIdComboBox.FormattingEnabled = true;
			this.ItemIdComboBox.Location = new System.Drawing.Point(195, 49);
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
			this.StratumIdComboBox.Location = new System.Drawing.Point(195, 82);
			this.StratumIdComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.StratumIdComboBox.Name = "StratumIdComboBox";
			this.StratumIdComboBox.Size = new System.Drawing.Size(357, 24);
			this.StratumIdComboBox.TabIndex = 2;
			this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
			// 
			// MaterialTypeComboBox
			// 
			this.MaterialTypeComboBox.FormattingEnabled = true;
			this.MaterialTypeComboBox.Location = new System.Drawing.Point(195, 115);
			this.MaterialTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
			this.MaterialTypeComboBox.Size = new System.Drawing.Size(357, 24);
			this.MaterialTypeComboBox.TabIndex = 3;
			this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
			// 
			// DataSourceComboBox
			// 
			this.DataSourceComboBox.FormattingEnabled = true;
			this.DataSourceComboBox.Location = new System.Drawing.Point(760, 342);
			this.DataSourceComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.DataSourceComboBox.Name = "DataSourceComboBox";
			this.DataSourceComboBox.Size = new System.Drawing.Size(187, 24);
			this.DataSourceComboBox.TabIndex = 4;
			this.DataSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.DataSourceComboBox_SelectedIndexChanged);
			// 
			// InventoryChangeCodeComboBox
			// 
			this.InventoryChangeCodeComboBox.FormattingEnabled = true;
			this.InventoryChangeCodeComboBox.Location = new System.Drawing.Point(731, 16);
			this.InventoryChangeCodeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.InventoryChangeCodeComboBox.Name = "InventoryChangeCodeComboBox";
			this.InventoryChangeCodeComboBox.Size = new System.Drawing.Size(80, 24);
			this.InventoryChangeCodeComboBox.TabIndex = 5;
			this.InventoryChangeCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.InventoryChangeCodeComboBox_SelectedIndexChanged);
			// 
			// IOCodeComboBox
			// 
			this.IOCodeComboBox.FormattingEnabled = true;
			this.IOCodeComboBox.Location = new System.Drawing.Point(731, 49);
			this.IOCodeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.IOCodeComboBox.Name = "IOCodeComboBox";
			this.IOCodeComboBox.Size = new System.Drawing.Size(80, 24);
			this.IOCodeComboBox.TabIndex = 6;
			this.IOCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IOCodeComboBox_SelectedIndexChanged);
			// 
			// DeclaredMassTextBox
			// 
			this.DeclaredMassTextBox.Location = new System.Drawing.Point(195, 149);
			this.DeclaredMassTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
			this.DeclaredMassTextBox.Size = new System.Drawing.Size(95, 22);
			this.DeclaredMassTextBox.TabIndex = 7;
			this.DeclaredMassTextBox.Leave += new System.EventHandler(this.DeclaredMassTextBox_Leave);
			// 
			// CommentTextBox
			// 
			this.CommentTextBox.Location = new System.Drawing.Point(195, 181);
			this.CommentTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.CommentTextBox.Name = "CommentTextBox";
			this.CommentTextBox.Size = new System.Drawing.Size(357, 22);
			this.CommentTextBox.TabIndex = 8;
			this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
			// 
			// CountTimeTextBox
			// 
			this.CountTimeTextBox.Location = new System.Drawing.Point(195, 213);
			this.CountTimeTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.CountTimeTextBox.Name = "CountTimeTextBox";
			this.CountTimeTextBox.Size = new System.Drawing.Size(95, 22);
			this.CountTimeTextBox.TabIndex = 9;
			this.CountTimeTextBox.Leave += new System.EventHandler(this.CountTimeTextBox_Leave);
			// 
			// NumPassiveCyclesTextBox
			// 
			this.NumPassiveCyclesTextBox.Location = new System.Drawing.Point(196, 392);
			this.NumPassiveCyclesTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.NumPassiveCyclesTextBox.Name = "NumPassiveCyclesTextBox";
			this.NumPassiveCyclesTextBox.Size = new System.Drawing.Size(132, 22);
			this.NumPassiveCyclesTextBox.TabIndex = 10;
			this.NumPassiveCyclesTextBox.Leave += new System.EventHandler(this.NumPassiveCyclesTextBox_Leave);
			// 
			// MeasPrecisionTextBox
			// 
			this.MeasPrecisionTextBox.Location = new System.Drawing.Point(196, 424);
			this.MeasPrecisionTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.MeasPrecisionTextBox.Name = "MeasPrecisionTextBox";
			this.MeasPrecisionTextBox.Size = new System.Drawing.Size(132, 22);
			this.MeasPrecisionTextBox.TabIndex = 11;
			this.MeasPrecisionTextBox.Leave += new System.EventHandler(this.MeasPrecisionTextBox_Leave);
			// 
			// MinNumCyclesTextBox
			// 
			this.MinNumCyclesTextBox.Location = new System.Drawing.Point(196, 456);
			this.MinNumCyclesTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.MinNumCyclesTextBox.Name = "MinNumCyclesTextBox";
			this.MinNumCyclesTextBox.Size = new System.Drawing.Size(132, 22);
			this.MinNumCyclesTextBox.TabIndex = 12;
			this.MinNumCyclesTextBox.Leave += new System.EventHandler(this.MinNumCyclesTextBox_Leave);
			// 
			// MaxNumCyclesTextBox
			// 
			this.MaxNumCyclesTextBox.Location = new System.Drawing.Point(196, 488);
			this.MaxNumCyclesTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.MaxNumCyclesTextBox.Name = "MaxNumCyclesTextBox";
			this.MaxNumCyclesTextBox.Size = new System.Drawing.Size(132, 22);
			this.MaxNumCyclesTextBox.TabIndex = 13;
			this.MaxNumCyclesTextBox.Leave += new System.EventHandler(this.MaxNumCyclesTextBox_Leave);
			// 
			// NumActiveCyclesTextBox
			// 
			this.NumActiveCyclesTextBox.Location = new System.Drawing.Point(496, 392);
			this.NumActiveCyclesTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.NumActiveCyclesTextBox.Name = "NumActiveCyclesTextBox";
			this.NumActiveCyclesTextBox.Size = new System.Drawing.Size(132, 22);
			this.NumActiveCyclesTextBox.TabIndex = 14;
			this.NumActiveCyclesTextBox.Leave += new System.EventHandler(this.NumActiveCyclesTextBox_Leave);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.UsePu240eRadioButton);
			this.groupBox1.Controls.Add(this.UseTriplesRadioButton);
			this.groupBox1.Controls.Add(this.UseDoublesRadioButton);
			this.groupBox1.Controls.Add(this.UseNumCyclesRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(195, 239);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(299, 144);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			// 
			// UsePu240eRadioButton
			// 
			this.UsePu240eRadioButton.AutoSize = true;
			this.UsePu240eRadioButton.Location = new System.Drawing.Point(20, 108);
			this.UsePu240eRadioButton.Margin = new System.Windows.Forms.Padding(4);
			this.UsePu240eRadioButton.Name = "UsePu240eRadioButton";
			this.UsePu240eRadioButton.Size = new System.Drawing.Size(258, 21);
			this.UsePu240eRadioButton.TabIndex = 3;
			this.UsePu240eRadioButton.TabStop = true;
			this.UsePu240eRadioButton.Text = "Use Pu240e measurement precision";
			this.UsePu240eRadioButton.UseVisualStyleBackColor = true;
			this.UsePu240eRadioButton.CheckedChanged += new System.EventHandler(this.UsePu240eRadioButton_CheckedChanged);
			// 
			// UseTriplesRadioButton
			// 
			this.UseTriplesRadioButton.AutoSize = true;
			this.UseTriplesRadioButton.Location = new System.Drawing.Point(20, 80);
			this.UseTriplesRadioButton.Margin = new System.Windows.Forms.Padding(4);
			this.UseTriplesRadioButton.Name = "UseTriplesRadioButton";
			this.UseTriplesRadioButton.Size = new System.Drawing.Size(247, 21);
			this.UseTriplesRadioButton.TabIndex = 2;
			this.UseTriplesRadioButton.TabStop = true;
			this.UseTriplesRadioButton.Text = "Use triples measurement precision";
			this.UseTriplesRadioButton.UseVisualStyleBackColor = true;
			this.UseTriplesRadioButton.CheckedChanged += new System.EventHandler(this.UseTriplesRadioButton_CheckedChanged);
			// 
			// UseDoublesRadioButton
			// 
			this.UseDoublesRadioButton.AutoSize = true;
			this.UseDoublesRadioButton.Location = new System.Drawing.Point(20, 52);
			this.UseDoublesRadioButton.Margin = new System.Windows.Forms.Padding(4);
			this.UseDoublesRadioButton.Name = "UseDoublesRadioButton";
			this.UseDoublesRadioButton.Size = new System.Drawing.Size(259, 21);
			this.UseDoublesRadioButton.TabIndex = 1;
			this.UseDoublesRadioButton.TabStop = true;
			this.UseDoublesRadioButton.Text = "Use doubles measurement precision";
			this.UseDoublesRadioButton.UseVisualStyleBackColor = true;
			this.UseDoublesRadioButton.CheckedChanged += new System.EventHandler(this.UseDoublesRadioButton_CheckedChanged);
			// 
			// UseNumCyclesRadioButton
			// 
			this.UseNumCyclesRadioButton.AutoSize = true;
			this.UseNumCyclesRadioButton.Location = new System.Drawing.Point(20, 23);
			this.UseNumCyclesRadioButton.Margin = new System.Windows.Forms.Padding(4);
			this.UseNumCyclesRadioButton.Name = "UseNumCyclesRadioButton";
			this.UseNumCyclesRadioButton.Size = new System.Drawing.Size(165, 21);
			this.UseNumCyclesRadioButton.TabIndex = 0;
			this.UseNumCyclesRadioButton.TabStop = true;
			this.UseNumCyclesRadioButton.Text = "Use number of cycles";
			this.UseNumCyclesRadioButton.UseVisualStyleBackColor = true;
			this.UseNumCyclesRadioButton.CheckedChanged += new System.EventHandler(this.UseNumCyclesRadioButton_CheckedChanged);
			// 
			// QCTestsCheckBox
			// 
			this.QCTestsCheckBox.AutoSize = true;
			this.QCTestsCheckBox.Location = new System.Drawing.Point(380, 426);
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
			this.PrintResultsCheckBox.Location = new System.Drawing.Point(380, 454);
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
			this.CommentAtEndCheckBox.Location = new System.Drawing.Point(380, 483);
			this.CommentAtEndCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
			this.CommentAtEndCheckBox.Size = new System.Drawing.Size(239, 21);
			this.CommentAtEndCheckBox.TabIndex = 18;
			this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
			this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
			this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
			// 
			// DrumWeightTextBox
			// 
			this.DrumWeightTextBox.Location = new System.Drawing.Point(864, 298);
			this.DrumWeightTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DrumWeightTextBox.Name = "DrumWeightTextBox";
			this.DrumWeightTextBox.Size = new System.Drawing.Size(83, 22);
			this.DrumWeightTextBox.TabIndex = 19;
			this.DrumWeightTextBox.Leave += new System.EventHandler(this.DrumWeightTextBox_Leave);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(849, 13);
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
			this.CancelBtn.Location = new System.Drawing.Point(849, 49);
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
			this.HelpBtn.Location = new System.Drawing.Point(849, 85);
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
			this.IsotopicsBtn.Location = new System.Drawing.Point(760, 210);
			this.IsotopicsBtn.Margin = new System.Windows.Forms.Padding(4);
			this.IsotopicsBtn.Name = "IsotopicsBtn";
			this.IsotopicsBtn.Size = new System.Drawing.Size(188, 28);
			this.IsotopicsBtn.TabIndex = 23;
			this.IsotopicsBtn.Text = "Isotopics...";
			this.IsotopicsBtn.UseVisualStyleBackColor = true;
			this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
			// 
			// CompositeIsotopicsBtn
			// 
			this.CompositeIsotopicsBtn.Location = new System.Drawing.Point(762, 246);
			this.CompositeIsotopicsBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CompositeIsotopicsBtn.Name = "CompositeIsotopicsBtn";
			this.CompositeIsotopicsBtn.Size = new System.Drawing.Size(185, 28);
			this.CompositeIsotopicsBtn.TabIndex = 24;
			this.CompositeIsotopicsBtn.Text = "Composite Isotopics...";
			this.CompositeIsotopicsBtn.UseVisualStyleBackColor = true;
			this.CompositeIsotopicsBtn.Click += new System.EventHandler(this.CompositeIsotopicsBtn_Click);
			// 
			// Pu240eCoeffBtn
			// 
			this.Pu240eCoeffBtn.Enabled = false;
			this.Pu240eCoeffBtn.Location = new System.Drawing.Point(722, 389);
			this.Pu240eCoeffBtn.Margin = new System.Windows.Forms.Padding(4);
			this.Pu240eCoeffBtn.Name = "Pu240eCoeffBtn";
			this.Pu240eCoeffBtn.Size = new System.Drawing.Size(225, 28);
			this.Pu240eCoeffBtn.TabIndex = 25;
			this.Pu240eCoeffBtn.Text = "Pu240e Coefficients";
			this.Pu240eCoeffBtn.UseVisualStyleBackColor = true;
			this.Pu240eCoeffBtn.Click += new System.EventHandler(this.Pu240eCoeffBtn_Click);
			// 
			// MBALabel
			// 
			this.MBALabel.AutoSize = true;
			this.MBALabel.Location = new System.Drawing.Point(142, 19);
			this.MBALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MBALabel.Name = "MBALabel";
			this.MBALabel.Size = new System.Drawing.Size(37, 17);
			this.MBALabel.TabIndex = 26;
			this.MBALabel.Text = "MBA";
			// 
			// ItemIdLabel
			// 
			this.ItemIdLabel.AutoSize = true;
			this.ItemIdLabel.Location = new System.Drawing.Point(131, 53);
			this.ItemIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ItemIdLabel.Name = "ItemIdLabel";
			this.ItemIdLabel.Size = new System.Drawing.Size(49, 17);
			this.ItemIdLabel.TabIndex = 27;
			this.ItemIdLabel.Text = "Item id";
			// 
			// StratumIdLabel
			// 
			this.StratumIdLabel.AutoSize = true;
			this.StratumIdLabel.Location = new System.Drawing.Point(110, 86);
			this.StratumIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.StratumIdLabel.Name = "StratumIdLabel";
			this.StratumIdLabel.Size = new System.Drawing.Size(72, 17);
			this.StratumIdLabel.TabIndex = 28;
			this.StratumIdLabel.Text = "Stratum id";
			// 
			// MaterialTypeLabel
			// 
			this.MaterialTypeLabel.AutoSize = true;
			this.MaterialTypeLabel.Location = new System.Drawing.Point(92, 119);
			this.MaterialTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MaterialTypeLabel.Name = "MaterialTypeLabel";
			this.MaterialTypeLabel.Size = new System.Drawing.Size(89, 17);
			this.MaterialTypeLabel.TabIndex = 29;
			this.MaterialTypeLabel.Text = "Material type";
			// 
			// DeclaredMassLabel
			// 
			this.DeclaredMassLabel.AutoSize = true;
			this.DeclaredMassLabel.Location = new System.Drawing.Point(59, 152);
			this.DeclaredMassLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DeclaredMassLabel.Name = "DeclaredMassLabel";
			this.DeclaredMassLabel.Size = new System.Drawing.Size(124, 17);
			this.DeclaredMassLabel.TabIndex = 30;
			this.DeclaredMassLabel.Text = "Declared mass (g)";
			// 
			// CommentLabel
			// 
			this.CommentLabel.AutoSize = true;
			this.CommentLabel.Location = new System.Drawing.Point(114, 184);
			this.CommentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.CommentLabel.Name = "CommentLabel";
			this.CommentLabel.Size = new System.Drawing.Size(67, 17);
			this.CommentLabel.TabIndex = 31;
			this.CommentLabel.Text = "Comment";
			// 
			// CountTimeLabel
			// 
			this.CountTimeLabel.AutoSize = true;
			this.CountTimeLabel.Location = new System.Drawing.Point(64, 216);
			this.CountTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.CountTimeLabel.Name = "CountTimeLabel";
			this.CountTimeLabel.Size = new System.Drawing.Size(118, 17);
			this.CountTimeLabel.TabIndex = 32;
			this.CountTimeLabel.Text = "Count time (secs)";
			// 
			// NumPassiveCyclesLabel
			// 
			this.NumPassiveCyclesLabel.AutoSize = true;
			this.NumPassiveCyclesLabel.Location = new System.Drawing.Point(27, 395);
			this.NumPassiveCyclesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.NumPassiveCyclesLabel.Name = "NumPassiveCyclesLabel";
			this.NumPassiveCyclesLabel.Size = new System.Drawing.Size(153, 17);
			this.NumPassiveCyclesLabel.TabIndex = 33;
			this.NumPassiveCyclesLabel.Text = "Number passive cycles";
			// 
			// MeasPrecisionLabel
			// 
			this.MeasPrecisionLabel.AutoSize = true;
			this.MeasPrecisionLabel.Location = new System.Drawing.Point(4, 427);
			this.MeasPrecisionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MeasPrecisionLabel.Name = "MeasPrecisionLabel";
			this.MeasPrecisionLabel.Size = new System.Drawing.Size(181, 17);
			this.MeasPrecisionLabel.TabIndex = 34;
			this.MeasPrecisionLabel.Text = "Measurement precision (%)";
			// 
			// MinNumCyclesLabel
			// 
			this.MinNumCyclesLabel.AutoSize = true;
			this.MinNumCyclesLabel.Location = new System.Drawing.Point(55, 459);
			this.MinNumCyclesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MinNumCyclesLabel.Name = "MinNumCyclesLabel";
			this.MinNumCyclesLabel.Size = new System.Drawing.Size(125, 17);
			this.MinNumCyclesLabel.TabIndex = 35;
			this.MinNumCyclesLabel.Text = "Min number cycles";
			// 
			// MaxNumCyclesLabel
			// 
			this.MaxNumCyclesLabel.AutoSize = true;
			this.MaxNumCyclesLabel.Location = new System.Drawing.Point(51, 491);
			this.MaxNumCyclesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MaxNumCyclesLabel.Name = "MaxNumCyclesLabel";
			this.MaxNumCyclesLabel.Size = new System.Drawing.Size(128, 17);
			this.MaxNumCyclesLabel.TabIndex = 36;
			this.MaxNumCyclesLabel.Text = "Max number cycles";
			// 
			// NumActiveCyclesLabel
			// 
			this.NumActiveCyclesLabel.AutoSize = true;
			this.NumActiveCyclesLabel.Location = new System.Drawing.Point(343, 395);
			this.NumActiveCyclesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.NumActiveCyclesLabel.Name = "NumActiveCyclesLabel";
			this.NumActiveCyclesLabel.Size = new System.Drawing.Size(142, 17);
			this.NumActiveCyclesLabel.TabIndex = 37;
			this.NumActiveCyclesLabel.Text = "Number active cycles";
			// 
			// DrumWeightLabel
			// 
			this.DrumWeightLabel.AutoSize = true;
			this.DrumWeightLabel.Location = new System.Drawing.Point(698, 301);
			this.DrumWeightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DrumWeightLabel.Name = "DrumWeightLabel";
			this.DrumWeightLabel.Size = new System.Drawing.Size(157, 17);
			this.DrumWeightLabel.TabIndex = 38;
			this.DrumWeightLabel.Text = "Drum empty weight (kg)";
			// 
			// DataSourceLabel
			// 
			this.DataSourceLabel.AutoSize = true;
			this.DataSourceLabel.Location = new System.Drawing.Point(665, 346);
			this.DataSourceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DataSourceLabel.Name = "DataSourceLabel";
			this.DataSourceLabel.Size = new System.Drawing.Size(85, 17);
			this.DataSourceLabel.TabIndex = 39;
			this.DataSourceLabel.Text = "Data source";
			// 
			// InventoryChangeCodeLabel
			// 
			this.InventoryChangeCodeLabel.AutoSize = true;
			this.InventoryChangeCodeLabel.Location = new System.Drawing.Point(567, 19);
			this.InventoryChangeCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InventoryChangeCodeLabel.Name = "InventoryChangeCodeLabel";
			this.InventoryChangeCodeLabel.Size = new System.Drawing.Size(152, 17);
			this.InventoryChangeCodeLabel.TabIndex = 40;
			this.InventoryChangeCodeLabel.Text = "Inventory change code";
			// 
			// IOCodeLabel
			// 
			this.IOCodeLabel.AutoSize = true;
			this.IOCodeLabel.Location = new System.Drawing.Point(656, 53);
			this.IOCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IOCodeLabel.Name = "IOCodeLabel";
			this.IOCodeLabel.Size = new System.Drawing.Size(61, 17);
			this.IOCodeLabel.TabIndex = 41;
			this.IOCodeLabel.Text = "I/O code";
			// 
			// MaterialTypeHelpBtn
			// 
			this.MaterialTypeHelpBtn.Location = new System.Drawing.Point(560, 113);
			this.MaterialTypeHelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.MaterialTypeHelpBtn.Name = "MaterialTypeHelpBtn";
			this.MaterialTypeHelpBtn.Size = new System.Drawing.Size(163, 28);
			this.MaterialTypeHelpBtn.TabIndex = 42;
			this.MaterialTypeHelpBtn.Text = "Material type help...";
			this.MaterialTypeHelpBtn.UseVisualStyleBackColor = true;
			this.MaterialTypeHelpBtn.Click += new System.EventHandler(this.MaterialTypeHelpBtn_Click);
			// 
			// IDDAcquireAssay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(961, 519);
			this.Controls.Add(this.MaterialTypeHelpBtn);
			this.Controls.Add(this.IOCodeLabel);
			this.Controls.Add(this.InventoryChangeCodeLabel);
			this.Controls.Add(this.DataSourceLabel);
			this.Controls.Add(this.DrumWeightLabel);
			this.Controls.Add(this.NumActiveCyclesLabel);
			this.Controls.Add(this.MaxNumCyclesLabel);
			this.Controls.Add(this.MinNumCyclesLabel);
			this.Controls.Add(this.MeasPrecisionLabel);
			this.Controls.Add(this.NumPassiveCyclesLabel);
			this.Controls.Add(this.CountTimeLabel);
			this.Controls.Add(this.CommentLabel);
			this.Controls.Add(this.DeclaredMassLabel);
			this.Controls.Add(this.MaterialTypeLabel);
			this.Controls.Add(this.StratumIdLabel);
			this.Controls.Add(this.ItemIdLabel);
			this.Controls.Add(this.MBALabel);
			this.Controls.Add(this.Pu240eCoeffBtn);
			this.Controls.Add(this.CompositeIsotopicsBtn);
			this.Controls.Add(this.IsotopicsBtn);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.DrumWeightTextBox);
			this.Controls.Add(this.CommentAtEndCheckBox);
			this.Controls.Add(this.PrintResultsCheckBox);
			this.Controls.Add(this.QCTestsCheckBox);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.NumActiveCyclesTextBox);
			this.Controls.Add(this.MaxNumCyclesTextBox);
			this.Controls.Add(this.MinNumCyclesTextBox);
			this.Controls.Add(this.MeasPrecisionTextBox);
			this.Controls.Add(this.NumPassiveCyclesTextBox);
			this.Controls.Add(this.CountTimeTextBox);
			this.Controls.Add(this.CommentTextBox);
			this.Controls.Add(this.DeclaredMassTextBox);
			this.Controls.Add(this.IOCodeComboBox);
			this.Controls.Add(this.InventoryChangeCodeComboBox);
			this.Controls.Add(this.DataSourceComboBox);
			this.Controls.Add(this.MaterialTypeComboBox);
			this.Controls.Add(this.StratumIdComboBox);
			this.Controls.Add(this.ItemIdComboBox);
			this.Controls.Add(this.MBAComboBox);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IDDAcquireAssay";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Verification Measurement";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MBAComboBox;
        private System.Windows.Forms.ComboBox ItemIdComboBox;
        private System.Windows.Forms.ComboBox StratumIdComboBox;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox DataSourceComboBox;
        private System.Windows.Forms.ComboBox InventoryChangeCodeComboBox;
        private System.Windows.Forms.ComboBox IOCodeComboBox;
        private System.Windows.Forms.TextBox DeclaredMassTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.TextBox CountTimeTextBox;
        private System.Windows.Forms.TextBox NumPassiveCyclesTextBox;
        private System.Windows.Forms.TextBox MeasPrecisionTextBox;
        private System.Windows.Forms.TextBox MinNumCyclesTextBox;
        private System.Windows.Forms.TextBox MaxNumCyclesTextBox;
        private System.Windows.Forms.TextBox NumActiveCyclesTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton UsePu240eRadioButton;
        private System.Windows.Forms.RadioButton UseTriplesRadioButton;
        private System.Windows.Forms.RadioButton UseDoublesRadioButton;
        private System.Windows.Forms.RadioButton UseNumCyclesRadioButton;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.TextBox DrumWeightTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button CompositeIsotopicsBtn;
        private System.Windows.Forms.Button Pu240eCoeffBtn;
        private System.Windows.Forms.Label MBALabel;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label DeclaredMassLabel;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.Label NumPassiveCyclesLabel;
        private System.Windows.Forms.Label MeasPrecisionLabel;
        private System.Windows.Forms.Label MinNumCyclesLabel;
        private System.Windows.Forms.Label MaxNumCyclesLabel;
        private System.Windows.Forms.Label NumActiveCyclesLabel;
        private System.Windows.Forms.Label DrumWeightLabel;
        private System.Windows.Forms.Label DataSourceLabel;
        private System.Windows.Forms.Label InventoryChangeCodeLabel;
        private System.Windows.Forms.Label IOCodeLabel;
        private System.Windows.Forms.Button MaterialTypeHelpBtn;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}