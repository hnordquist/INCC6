namespace NewUI
{
    partial class IDDIsotopics
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDIsotopics));
			this.IsotopicsIdComboBox = new System.Windows.Forms.ComboBox();
			this.IsotopicsSourceCodeComboBox = new System.Windows.Forms.ComboBox();
			this.IsotopicsEntryFormatGroupBox = new System.Windows.Forms.GroupBox();
			this.PuPlusAmRadioButton = new System.Windows.Forms.RadioButton();
			this.AmPercentOfPuRadioButton = new System.Windows.Forms.RadioButton();
			this.Pu238PercentTextBox = new System.Windows.Forms.TextBox();
			this.Pu239PercentTextBox = new System.Windows.Forms.TextBox();
			this.Pu240PercentTextBox = new System.Windows.Forms.TextBox();
			this.Pu241PercentTextBox = new System.Windows.Forms.TextBox();
			this.Pu242PercentTextBox = new System.Windows.Forms.TextBox();
			this.Am241PercentTextBox = new System.Windows.Forms.TextBox();
			this.Pu238ErrorTextBox = new System.Windows.Forms.TextBox();
			this.Pu239ErrorTextBox = new System.Windows.Forms.TextBox();
			this.Pu240ErrorTextBox = new System.Windows.Forms.TextBox();
			this.Pu241ErrorTextBox = new System.Windows.Forms.TextBox();
			this.Pu242ErrorTextBox = new System.Windows.Forms.TextBox();
			this.Am241ErrorTextBox = new System.Windows.Forms.TextBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.ReadFromFileBtn = new System.Windows.Forms.Button();
			this.WriteToFileBtn = new System.Windows.Forms.Button();
			this.SaveSetBtn = new System.Windows.Forms.Button();
			this.EditIdBtn = new System.Windows.Forms.Button();
			this.DeleteSetBtn = new System.Windows.Forms.Button();
			this.AddNewSetBtn = new System.Windows.Forms.Button();
			this.IsotopicsIdLabel = new System.Windows.Forms.Label();
			this.IsotopicsSourceCodeLabel = new System.Windows.Forms.Label();
			this.Pu238PercentLabel = new System.Windows.Forms.Label();
			this.Pu239PercentLabel = new System.Windows.Forms.Label();
			this.Pu240PercentLabel = new System.Windows.Forms.Label();
			this.Pu241PercentLabel = new System.Windows.Forms.Label();
			this.Pu242PrecentLabel = new System.Windows.Forms.Label();
			this.PuDateLabel = new System.Windows.Forms.Label();
			this.Am241PercentLabel = new System.Windows.Forms.Label();
			this.AmDateLabel = new System.Windows.Forms.Label();
			this.Pu238ErrorLabel = new System.Windows.Forms.Label();
			this.Pu239ErrorLabel = new System.Windows.Forms.Label();
			this.Pu240ErrorLabel = new System.Windows.Forms.Label();
			this.Pu241ErrorLabel = new System.Windows.Forms.Label();
			this.Pu242ErrorLabel = new System.Windows.Forms.Label();
			this.Am241ErrorLabel = new System.Windows.Forms.Label();
			this.PuDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.AmDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.IsoList = new System.Windows.Forms.Button();
			this.IsotopicsEntryFormatGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// IsotopicsIdComboBox
			// 
			this.IsotopicsIdComboBox.FormattingEnabled = true;
			this.IsotopicsIdComboBox.Location = new System.Drawing.Point(107, 15);
			this.IsotopicsIdComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IsotopicsIdComboBox.Name = "IsotopicsIdComboBox";
			this.IsotopicsIdComboBox.Size = new System.Drawing.Size(219, 24);
			this.IsotopicsIdComboBox.TabIndex = 0;
			this.toolTip1.SetToolTip(this.IsotopicsIdComboBox, "Select the desired isotopics data set from the picklist. To add a new isotopics d" +
        "ata set enter the desired isotopics and then click on the \'Add new isotopics dat" +
        "a set\' button.");
			this.IsotopicsIdComboBox.SelectedIndexChanged += new System.EventHandler(this.IsotopicsIdComboBox_SelectedIndexChanged);
			// 
			// IsotopicsSourceCodeComboBox
			// 
			this.IsotopicsSourceCodeComboBox.FormattingEnabled = true;
			this.IsotopicsSourceCodeComboBox.Location = new System.Drawing.Point(616, 15);
			this.IsotopicsSourceCodeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IsotopicsSourceCodeComboBox.Name = "IsotopicsSourceCodeComboBox";
			this.IsotopicsSourceCodeComboBox.Size = new System.Drawing.Size(65, 24);
			this.IsotopicsSourceCodeComboBox.TabIndex = 1;
			this.toolTip1.SetToolTip(this.IsotopicsSourceCodeComboBox, resources.GetString("IsotopicsSourceCodeComboBox.ToolTip"));
			this.IsotopicsSourceCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IsotopicsSourceCodeComboBox_SelectedIndexChanged);
			// 
			// IsotopicsEntryFormatGroupBox
			// 
			this.IsotopicsEntryFormatGroupBox.Controls.Add(this.PuPlusAmRadioButton);
			this.IsotopicsEntryFormatGroupBox.Controls.Add(this.AmPercentOfPuRadioButton);
			this.IsotopicsEntryFormatGroupBox.Location = new System.Drawing.Point(23, 66);
			this.IsotopicsEntryFormatGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IsotopicsEntryFormatGroupBox.Name = "IsotopicsEntryFormatGroupBox";
			this.IsotopicsEntryFormatGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IsotopicsEntryFormatGroupBox.Size = new System.Drawing.Size(399, 110);
			this.IsotopicsEntryFormatGroupBox.TabIndex = 2;
			this.IsotopicsEntryFormatGroupBox.TabStop = false;
			this.IsotopicsEntryFormatGroupBox.Text = "Isotopics entry format";
			// 
			// PuPlusAmRadioButton
			// 
			this.PuPlusAmRadioButton.AutoSize = true;
			this.PuPlusAmRadioButton.Location = new System.Drawing.Point(28, 65);
			this.PuPlusAmRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.PuPlusAmRadioButton.Name = "PuPlusAmRadioButton";
			this.PuPlusAmRadioButton.Size = new System.Drawing.Size(211, 21);
			this.PuPlusAmRadioButton.TabIndex = 1;
			this.PuPlusAmRadioButton.TabStop = true;
			this.PuPlusAmRadioButton.Text = "Mass of Pu + Am241 = 100%";
			this.toolTip1.SetToolTip(this.PuPlusAmRadioButton, "Mass of Pu + Am = 100%");
			this.PuPlusAmRadioButton.UseVisualStyleBackColor = true;
			// 
			// AmPercentOfPuRadioButton
			// 
			this.AmPercentOfPuRadioButton.AutoSize = true;
			this.AmPercentOfPuRadioButton.Location = new System.Drawing.Point(28, 37);
			this.AmPercentOfPuRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.AmPercentOfPuRadioButton.Name = "AmPercentOfPuRadioButton";
			this.AmPercentOfPuRadioButton.Size = new System.Drawing.Size(344, 21);
			this.AmPercentOfPuRadioButton.TabIndex = 0;
			this.AmPercentOfPuRadioButton.TabStop = true;
			this.AmPercentOfPuRadioButton.Text = "Mass of Pu = 100% and Am241 = % total Pu mass";
			this.toolTip1.SetToolTip(this.AmPercentOfPuRadioButton, "Mass of Pu = 100% and Am is a percentage of the total Pu mass.");
			this.AmPercentOfPuRadioButton.UseVisualStyleBackColor = true;
			// 
			// Pu238PercentTextBox
			// 
			this.Pu238PercentTextBox.Location = new System.Drawing.Point(92, 204);
			this.Pu238PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu238PercentTextBox.Name = "Pu238PercentTextBox";
			this.Pu238PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu238PercentTextBox.TabIndex = 3;
			this.toolTip1.SetToolTip(this.Pu238PercentTextBox, "The Pu238 content is expressed as weight percent of the total plutonium.");
			this.Pu238PercentTextBox.Leave += new System.EventHandler(this.Pu238PercentTextBox_Leave);
			// 
			// Pu239PercentTextBox
			// 
			this.Pu239PercentTextBox.Location = new System.Drawing.Point(92, 236);
			this.Pu239PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu239PercentTextBox.Name = "Pu239PercentTextBox";
			this.Pu239PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu239PercentTextBox.TabIndex = 4;
			this.toolTip1.SetToolTip(this.Pu239PercentTextBox, "The Pu239 content is expressed as weight percent of the total plutonium.");
			this.Pu239PercentTextBox.Leave += new System.EventHandler(this.Pu239PercentTextBox_Leave);
			// 
			// Pu240PercentTextBox
			// 
			this.Pu240PercentTextBox.Location = new System.Drawing.Point(92, 268);
			this.Pu240PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu240PercentTextBox.Name = "Pu240PercentTextBox";
			this.Pu240PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu240PercentTextBox.TabIndex = 5;
			this.toolTip1.SetToolTip(this.Pu240PercentTextBox, "The Pu240 content is expressed as weight percent of the total plutonium.");
			this.Pu240PercentTextBox.Leave += new System.EventHandler(this.Pu240PercentTextBox_Leave);
			// 
			// Pu241PercentTextBox
			// 
			this.Pu241PercentTextBox.Location = new System.Drawing.Point(92, 300);
			this.Pu241PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu241PercentTextBox.Name = "Pu241PercentTextBox";
			this.Pu241PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu241PercentTextBox.TabIndex = 6;
			this.toolTip1.SetToolTip(this.Pu241PercentTextBox, "The Pu241 content is expressed as weight percent of the total plutonium.");
			this.Pu241PercentTextBox.Leave += new System.EventHandler(this.Pu241PercentTextBox_Leave);
			// 
			// Pu242PercentTextBox
			// 
			this.Pu242PercentTextBox.Location = new System.Drawing.Point(92, 332);
			this.Pu242PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu242PercentTextBox.Name = "Pu242PercentTextBox";
			this.Pu242PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu242PercentTextBox.TabIndex = 7;
			this.toolTip1.SetToolTip(this.Pu242PercentTextBox, "The Pu242 content is expressed as weight percent of the total plutonium.");
			this.Pu242PercentTextBox.Leave += new System.EventHandler(this.Pu242PercentTextBox_Leave);
			// 
			// Am241PercentTextBox
			// 
			this.Am241PercentTextBox.Location = new System.Drawing.Point(91, 418);
			this.Am241PercentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Am241PercentTextBox.Name = "Am241PercentTextBox";
			this.Am241PercentTextBox.Size = new System.Drawing.Size(132, 22);
			this.Am241PercentTextBox.TabIndex = 9;
			this.toolTip1.SetToolTip(this.Am241PercentTextBox, "The Am241 content is expressed as weight percent of the total plutonium. To conve" +
        "rt from parts per million (ppm) to percent, divide by 10,000.");
			this.Am241PercentTextBox.Leave += new System.EventHandler(this.Am241PercentTextBox_Leave);
			// 
			// Pu238ErrorTextBox
			// 
			this.Pu238ErrorTextBox.Location = new System.Drawing.Point(365, 204);
			this.Pu238ErrorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu238ErrorTextBox.Name = "Pu238ErrorTextBox";
			this.Pu238ErrorTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu238ErrorTextBox.TabIndex = 11;
			this.toolTip1.SetToolTip(this.Pu238ErrorTextBox, "The Pu238 error is expressed as weight percent of the total plutonium.");
			this.Pu238ErrorTextBox.Leave += new System.EventHandler(this.Pu238ErrorTextBox_Leave);
			// 
			// Pu239ErrorTextBox
			// 
			this.Pu239ErrorTextBox.Location = new System.Drawing.Point(365, 236);
			this.Pu239ErrorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu239ErrorTextBox.Name = "Pu239ErrorTextBox";
			this.Pu239ErrorTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu239ErrorTextBox.TabIndex = 12;
			this.toolTip1.SetToolTip(this.Pu239ErrorTextBox, "The Pu239 error is expressed as weight percent of the total plutonium.");
			this.Pu239ErrorTextBox.Leave += new System.EventHandler(this.Pu239ErrorTextBox_Leave);
			// 
			// Pu240ErrorTextBox
			// 
			this.Pu240ErrorTextBox.Location = new System.Drawing.Point(365, 268);
			this.Pu240ErrorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu240ErrorTextBox.Name = "Pu240ErrorTextBox";
			this.Pu240ErrorTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu240ErrorTextBox.TabIndex = 13;
			this.toolTip1.SetToolTip(this.Pu240ErrorTextBox, "The Pu240 error is expressed as weight percent of the total plutonium.");
			this.Pu240ErrorTextBox.Leave += new System.EventHandler(this.Pu240ErrorTextBox_Leave);
			// 
			// Pu241ErrorTextBox
			// 
			this.Pu241ErrorTextBox.Location = new System.Drawing.Point(365, 300);
			this.Pu241ErrorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu241ErrorTextBox.Name = "Pu241ErrorTextBox";
			this.Pu241ErrorTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu241ErrorTextBox.TabIndex = 14;
			this.toolTip1.SetToolTip(this.Pu241ErrorTextBox, "The Pu241 error is expressed as weight percent of the total plutonium.");
			this.Pu241ErrorTextBox.Leave += new System.EventHandler(this.Pu241ErrorTextBox_Leave);
			// 
			// Pu242ErrorTextBox
			// 
			this.Pu242ErrorTextBox.Location = new System.Drawing.Point(365, 332);
			this.Pu242ErrorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Pu242ErrorTextBox.Name = "Pu242ErrorTextBox";
			this.Pu242ErrorTextBox.Size = new System.Drawing.Size(132, 22);
			this.Pu242ErrorTextBox.TabIndex = 15;
			this.toolTip1.SetToolTip(this.Pu242ErrorTextBox, "The Pu242 error is expressed as weight percent of the total plutonium.");
			this.Pu242ErrorTextBox.Leave += new System.EventHandler(this.Pu242ErrorTextBox_Leave);
			// 
			// Am241ErrorTextBox
			// 
			this.Am241ErrorTextBox.Location = new System.Drawing.Point(365, 422);
			this.Am241ErrorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Am241ErrorTextBox.Name = "Am241ErrorTextBox";
			this.Am241ErrorTextBox.Size = new System.Drawing.Size(132, 22);
			this.Am241ErrorTextBox.TabIndex = 16;
			this.toolTip1.SetToolTip(this.Am241ErrorTextBox, "The Am241 error is expressed as weight percent of the total plutonium.");
			this.Am241ErrorTextBox.Leave += new System.EventHandler(this.Am241ErrorTextBox_Leave);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(704, 12);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 17;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(704, 48);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 18;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(704, 84);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 19;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// ReadFromFileBtn
			// 
			this.ReadFromFileBtn.Location = new System.Drawing.Point(555, 202);
			this.ReadFromFileBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ReadFromFileBtn.Name = "ReadFromFileBtn";
			this.ReadFromFileBtn.Size = new System.Drawing.Size(249, 28);
			this.ReadFromFileBtn.TabIndex = 20;
			this.ReadFromFileBtn.Text = "Read isotopics from file...";
			this.toolTip1.SetToolTip(this.ReadFromFileBtn, resources.GetString("ReadFromFileBtn.ToolTip"));
			this.ReadFromFileBtn.UseVisualStyleBackColor = true;
			this.ReadFromFileBtn.Click += new System.EventHandler(this.ReadFromFileBtn_Click);
			// 
			// WriteToFileBtn
			// 
			this.WriteToFileBtn.Enabled = false;
			this.WriteToFileBtn.Location = new System.Drawing.Point(555, 238);
			this.WriteToFileBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.WriteToFileBtn.Name = "WriteToFileBtn";
			this.WriteToFileBtn.Size = new System.Drawing.Size(249, 28);
			this.WriteToFileBtn.TabIndex = 21;
			this.WriteToFileBtn.Text = "Write isotopics to file...";
			this.toolTip1.SetToolTip(this.WriteToFileBtn, resources.GetString("WriteToFileBtn.ToolTip"));
			this.WriteToFileBtn.UseVisualStyleBackColor = true;
			this.WriteToFileBtn.Click += new System.EventHandler(this.WriteToFileBtn_Click);
			// 
			// SaveSetBtn
			// 
			this.SaveSetBtn.Location = new System.Drawing.Point(555, 309);
			this.SaveSetBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SaveSetBtn.Name = "SaveSetBtn";
			this.SaveSetBtn.Size = new System.Drawing.Size(249, 28);
			this.SaveSetBtn.TabIndex = 23;
			this.SaveSetBtn.Text = "Save isotopics data set...";
			this.toolTip1.SetToolTip(this.SaveSetBtn, "Clicking here will save the currently displayed isotopic values for the currently" +
        " selected isotopic id.");
			this.SaveSetBtn.UseVisualStyleBackColor = true;
			this.SaveSetBtn.Click += new System.EventHandler(this.SaveSetBtn_Click);
			// 
			// EditIdBtn
			// 
			this.EditIdBtn.Location = new System.Drawing.Point(555, 345);
			this.EditIdBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.EditIdBtn.Name = "EditIdBtn";
			this.EditIdBtn.Size = new System.Drawing.Size(249, 28);
			this.EditIdBtn.TabIndex = 24;
			this.EditIdBtn.Text = "Edit isotopics id...";
			this.toolTip1.SetToolTip(this.EditIdBtn, "Clicking here will bring up a dialog box prompting you to enter a new isotopics i" +
        "d for the currently displayed isotopics data set.");
			this.EditIdBtn.UseVisualStyleBackColor = true;
			this.EditIdBtn.Click += new System.EventHandler(this.EditIdBtn_Click);
			// 
			// DeleteSetBtn
			// 
			this.DeleteSetBtn.Location = new System.Drawing.Point(555, 380);
			this.DeleteSetBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.DeleteSetBtn.Name = "DeleteSetBtn";
			this.DeleteSetBtn.Size = new System.Drawing.Size(249, 28);
			this.DeleteSetBtn.TabIndex = 25;
			this.DeleteSetBtn.Text = "Delete isotopics data set...";
			this.toolTip1.SetToolTip(this.DeleteSetBtn, "Clicking here will bring up a dialog box asking confirmation for deleting the cur" +
        "rently displayed isotopic data set.");
			this.DeleteSetBtn.UseVisualStyleBackColor = true;
			this.DeleteSetBtn.Click += new System.EventHandler(this.DeleteSetBtn_Click);
			// 
			// AddNewSetBtn
			// 
			this.AddNewSetBtn.Location = new System.Drawing.Point(555, 273);
			this.AddNewSetBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.AddNewSetBtn.Name = "AddNewSetBtn";
			this.AddNewSetBtn.Size = new System.Drawing.Size(249, 28);
			this.AddNewSetBtn.TabIndex = 26;
			this.AddNewSetBtn.Text = "Add new isotopics data set...";
			this.toolTip1.SetToolTip(this.AddNewSetBtn, "Clicking here will bring up a dialog box prompting you to enter an isotopics id f" +
        "or a new isotopics data set. The new isotopic data set will consist of the isoto" +
        "pic values currently displayed.");
			this.AddNewSetBtn.UseVisualStyleBackColor = true;
			this.AddNewSetBtn.Click += new System.EventHandler(this.AddNewSetBtn_Click);
			// 
			// IsotopicsIdLabel
			// 
			this.IsotopicsIdLabel.AutoSize = true;
			this.IsotopicsIdLabel.Location = new System.Drawing.Point(19, 18);
			this.IsotopicsIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IsotopicsIdLabel.Name = "IsotopicsIdLabel";
			this.IsotopicsIdLabel.Size = new System.Drawing.Size(78, 17);
			this.IsotopicsIdLabel.TabIndex = 27;
			this.IsotopicsIdLabel.Text = "Isotopics id";
			// 
			// IsotopicsSourceCodeLabel
			// 
			this.IsotopicsSourceCodeLabel.AutoSize = true;
			this.IsotopicsSourceCodeLabel.Location = new System.Drawing.Point(463, 18);
			this.IsotopicsSourceCodeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IsotopicsSourceCodeLabel.Name = "IsotopicsSourceCodeLabel";
			this.IsotopicsSourceCodeLabel.Size = new System.Drawing.Size(145, 17);
			this.IsotopicsSourceCodeLabel.TabIndex = 28;
			this.IsotopicsSourceCodeLabel.Text = "Isotopics source code";
			// 
			// Pu238PercentLabel
			// 
			this.Pu238PercentLabel.AutoSize = true;
			this.Pu238PercentLabel.Location = new System.Drawing.Point(19, 208);
			this.Pu238PercentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu238PercentLabel.Name = "Pu238PercentLabel";
			this.Pu238PercentLabel.Size = new System.Drawing.Size(65, 17);
			this.Pu238PercentLabel.TabIndex = 29;
			this.Pu238PercentLabel.Text = "% Pu238";
			// 
			// Pu239PercentLabel
			// 
			this.Pu239PercentLabel.AutoSize = true;
			this.Pu239PercentLabel.Location = new System.Drawing.Point(19, 240);
			this.Pu239PercentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu239PercentLabel.Name = "Pu239PercentLabel";
			this.Pu239PercentLabel.Size = new System.Drawing.Size(65, 17);
			this.Pu239PercentLabel.TabIndex = 30;
			this.Pu239PercentLabel.Text = "% Pu239";
			// 
			// Pu240PercentLabel
			// 
			this.Pu240PercentLabel.AutoSize = true;
			this.Pu240PercentLabel.Location = new System.Drawing.Point(19, 272);
			this.Pu240PercentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu240PercentLabel.Name = "Pu240PercentLabel";
			this.Pu240PercentLabel.Size = new System.Drawing.Size(65, 17);
			this.Pu240PercentLabel.TabIndex = 31;
			this.Pu240PercentLabel.Text = "% Pu240";
			// 
			// Pu241PercentLabel
			// 
			this.Pu241PercentLabel.AutoSize = true;
			this.Pu241PercentLabel.Location = new System.Drawing.Point(19, 304);
			this.Pu241PercentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu241PercentLabel.Name = "Pu241PercentLabel";
			this.Pu241PercentLabel.Size = new System.Drawing.Size(65, 17);
			this.Pu241PercentLabel.TabIndex = 32;
			this.Pu241PercentLabel.Text = "% Pu241";
			// 
			// Pu242PrecentLabel
			// 
			this.Pu242PrecentLabel.AutoSize = true;
			this.Pu242PrecentLabel.Location = new System.Drawing.Point(19, 336);
			this.Pu242PrecentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu242PrecentLabel.Name = "Pu242PrecentLabel";
			this.Pu242PrecentLabel.Size = new System.Drawing.Size(65, 17);
			this.Pu242PrecentLabel.TabIndex = 33;
			this.Pu242PrecentLabel.Text = "% Pu242";
			// 
			// PuDateLabel
			// 
			this.PuDateLabel.AutoSize = true;
			this.PuDateLabel.Location = new System.Drawing.Point(23, 368);
			this.PuDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PuDateLabel.Name = "PuDateLabel";
			this.PuDateLabel.Size = new System.Drawing.Size(57, 17);
			this.PuDateLabel.TabIndex = 34;
			this.PuDateLabel.Text = "Pu date";
			// 
			// Am241PercentLabel
			// 
			this.Am241PercentLabel.AutoSize = true;
			this.Am241PercentLabel.Location = new System.Drawing.Point(15, 422);
			this.Am241PercentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Am241PercentLabel.Name = "Am241PercentLabel";
			this.Am241PercentLabel.Size = new System.Drawing.Size(68, 17);
			this.Am241PercentLabel.TabIndex = 35;
			this.Am241PercentLabel.Text = "% Am241";
			// 
			// AmDateLabel
			// 
			this.AmDateLabel.AutoSize = true;
			this.AmDateLabel.Location = new System.Drawing.Point(21, 454);
			this.AmDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.AmDateLabel.Name = "AmDateLabel";
			this.AmDateLabel.Size = new System.Drawing.Size(60, 17);
			this.AmDateLabel.TabIndex = 36;
			this.AmDateLabel.Text = "Am date";
			// 
			// Pu238ErrorLabel
			// 
			this.Pu238ErrorLabel.AutoSize = true;
			this.Pu238ErrorLabel.Location = new System.Drawing.Point(275, 208);
			this.Pu238ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu238ErrorLabel.Name = "Pu238ErrorLabel";
			this.Pu238ErrorLabel.Size = new System.Drawing.Size(84, 17);
			this.Pu238ErrorLabel.TabIndex = 37;
			this.Pu238ErrorLabel.Text = "Pu238 error";
			// 
			// Pu239ErrorLabel
			// 
			this.Pu239ErrorLabel.AutoSize = true;
			this.Pu239ErrorLabel.Location = new System.Drawing.Point(275, 240);
			this.Pu239ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu239ErrorLabel.Name = "Pu239ErrorLabel";
			this.Pu239ErrorLabel.Size = new System.Drawing.Size(84, 17);
			this.Pu239ErrorLabel.TabIndex = 38;
			this.Pu239ErrorLabel.Text = "Pu239 error";
			// 
			// Pu240ErrorLabel
			// 
			this.Pu240ErrorLabel.AutoSize = true;
			this.Pu240ErrorLabel.Location = new System.Drawing.Point(275, 272);
			this.Pu240ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu240ErrorLabel.Name = "Pu240ErrorLabel";
			this.Pu240ErrorLabel.Size = new System.Drawing.Size(84, 17);
			this.Pu240ErrorLabel.TabIndex = 39;
			this.Pu240ErrorLabel.Text = "Pu240 error";
			// 
			// Pu241ErrorLabel
			// 
			this.Pu241ErrorLabel.AutoSize = true;
			this.Pu241ErrorLabel.Location = new System.Drawing.Point(275, 304);
			this.Pu241ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu241ErrorLabel.Name = "Pu241ErrorLabel";
			this.Pu241ErrorLabel.Size = new System.Drawing.Size(84, 17);
			this.Pu241ErrorLabel.TabIndex = 40;
			this.Pu241ErrorLabel.Text = "Pu241 error";
			// 
			// Pu242ErrorLabel
			// 
			this.Pu242ErrorLabel.AutoSize = true;
			this.Pu242ErrorLabel.Location = new System.Drawing.Point(275, 336);
			this.Pu242ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Pu242ErrorLabel.Name = "Pu242ErrorLabel";
			this.Pu242ErrorLabel.Size = new System.Drawing.Size(84, 17);
			this.Pu242ErrorLabel.TabIndex = 41;
			this.Pu242ErrorLabel.Text = "Pu242 error";
			// 
			// Am241ErrorLabel
			// 
			this.Am241ErrorLabel.AutoSize = true;
			this.Am241ErrorLabel.Location = new System.Drawing.Point(272, 426);
			this.Am241ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.Am241ErrorLabel.Name = "Am241ErrorLabel";
			this.Am241ErrorLabel.Size = new System.Drawing.Size(87, 17);
			this.Am241ErrorLabel.TabIndex = 42;
			this.Am241ErrorLabel.Text = "Am241 error";
			// 
			// PuDateTimePicker
			// 
			this.PuDateTimePicker.Location = new System.Drawing.Point(91, 364);
			this.PuDateTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.PuDateTimePicker.MinDate = new System.DateTime(1952, 1, 1, 0, 0, 0, 0);
			this.PuDateTimePicker.Name = "PuDateTimePicker";
			this.PuDateTimePicker.Size = new System.Drawing.Size(265, 22);
			this.PuDateTimePicker.TabIndex = 43;
			this.toolTip1.SetToolTip(this.PuDateTimePicker, "This is the date Pu isotopic analysis was done.");
			this.PuDateTimePicker.ValueChanged += new System.EventHandler(this.PuDateTimePicker_ValueChanged);
			// 
			// AmDateTimePicker
			// 
			this.AmDateTimePicker.CustomFormat = "yy.MM.dd";
			this.AmDateTimePicker.Location = new System.Drawing.Point(91, 450);
			this.AmDateTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.AmDateTimePicker.MinDate = new System.DateTime(1952, 1, 1, 0, 0, 0, 0);
			this.AmDateTimePicker.Name = "AmDateTimePicker";
			this.AmDateTimePicker.Size = new System.Drawing.Size(265, 22);
			this.AmDateTimePicker.TabIndex = 44;
			this.toolTip1.SetToolTip(this.AmDateTimePicker, "This is date Am content was measured.");
			this.AmDateTimePicker.ValueChanged += new System.EventHandler(this.AmDateTimePicker_ValueChanged);
			// 
			// IsoList
			// 
			this.IsoList.Location = new System.Drawing.Point(364, 16);
			this.IsoList.Name = "IsoList";
			this.IsoList.Size = new System.Drawing.Size(75, 23);
			this.IsoList.TabIndex = 45;
			this.IsoList.Text = "Select";
			this.IsoList.UseVisualStyleBackColor = true;
			this.IsoList.Click += new System.EventHandler(this.IsoList_Click);
			// 
			// IDDIsotopics
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(828, 494);
			this.Controls.Add(this.IsoList);
			this.Controls.Add(this.AmDateTimePicker);
			this.Controls.Add(this.PuDateTimePicker);
			this.Controls.Add(this.Am241ErrorLabel);
			this.Controls.Add(this.Pu242ErrorLabel);
			this.Controls.Add(this.Pu241ErrorLabel);
			this.Controls.Add(this.Pu240ErrorLabel);
			this.Controls.Add(this.Pu239ErrorLabel);
			this.Controls.Add(this.Pu238ErrorLabel);
			this.Controls.Add(this.AmDateLabel);
			this.Controls.Add(this.Am241PercentLabel);
			this.Controls.Add(this.PuDateLabel);
			this.Controls.Add(this.Pu242PrecentLabel);
			this.Controls.Add(this.Pu241PercentLabel);
			this.Controls.Add(this.Pu240PercentLabel);
			this.Controls.Add(this.Pu239PercentLabel);
			this.Controls.Add(this.Pu238PercentLabel);
			this.Controls.Add(this.IsotopicsSourceCodeLabel);
			this.Controls.Add(this.IsotopicsIdLabel);
			this.Controls.Add(this.AddNewSetBtn);
			this.Controls.Add(this.DeleteSetBtn);
			this.Controls.Add(this.EditIdBtn);
			this.Controls.Add(this.SaveSetBtn);
			this.Controls.Add(this.WriteToFileBtn);
			this.Controls.Add(this.ReadFromFileBtn);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.Am241ErrorTextBox);
			this.Controls.Add(this.Pu242ErrorTextBox);
			this.Controls.Add(this.Pu241ErrorTextBox);
			this.Controls.Add(this.Pu240ErrorTextBox);
			this.Controls.Add(this.Pu239ErrorTextBox);
			this.Controls.Add(this.Pu238ErrorTextBox);
			this.Controls.Add(this.Am241PercentTextBox);
			this.Controls.Add(this.Pu242PercentTextBox);
			this.Controls.Add(this.Pu241PercentTextBox);
			this.Controls.Add(this.Pu240PercentTextBox);
			this.Controls.Add(this.Pu239PercentTextBox);
			this.Controls.Add(this.Pu238PercentTextBox);
			this.Controls.Add(this.IsotopicsEntryFormatGroupBox);
			this.Controls.Add(this.IsotopicsSourceCodeComboBox);
			this.Controls.Add(this.IsotopicsIdComboBox);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IDDIsotopics";
			this.Text = "Isotopics";
			this.IsotopicsEntryFormatGroupBox.ResumeLayout(false);
			this.IsotopicsEntryFormatGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox IsotopicsIdComboBox;
        private System.Windows.Forms.ComboBox IsotopicsSourceCodeComboBox;
        private System.Windows.Forms.GroupBox IsotopicsEntryFormatGroupBox;
        private System.Windows.Forms.RadioButton PuPlusAmRadioButton;
        private System.Windows.Forms.RadioButton AmPercentOfPuRadioButton;
        private System.Windows.Forms.TextBox Pu238PercentTextBox;
        private System.Windows.Forms.TextBox Pu239PercentTextBox;
        private System.Windows.Forms.TextBox Pu240PercentTextBox;
        private System.Windows.Forms.TextBox Pu241PercentTextBox;
        private System.Windows.Forms.TextBox Pu242PercentTextBox;
        private System.Windows.Forms.TextBox Am241PercentTextBox;
        private System.Windows.Forms.TextBox Pu238ErrorTextBox;
        private System.Windows.Forms.TextBox Pu239ErrorTextBox;
        private System.Windows.Forms.TextBox Pu240ErrorTextBox;
        private System.Windows.Forms.TextBox Pu241ErrorTextBox;
        private System.Windows.Forms.TextBox Pu242ErrorTextBox;
        private System.Windows.Forms.TextBox Am241ErrorTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button ReadFromFileBtn;
        private System.Windows.Forms.Button WriteToFileBtn;
        private System.Windows.Forms.Button SaveSetBtn;
        private System.Windows.Forms.Button EditIdBtn;
        private System.Windows.Forms.Button DeleteSetBtn;
        private System.Windows.Forms.Button AddNewSetBtn;
        private System.Windows.Forms.Label IsotopicsIdLabel;
        private System.Windows.Forms.Label IsotopicsSourceCodeLabel;
        private System.Windows.Forms.Label Pu238PercentLabel;
        private System.Windows.Forms.Label Pu239PercentLabel;
        private System.Windows.Forms.Label Pu240PercentLabel;
        private System.Windows.Forms.Label Pu241PercentLabel;
        private System.Windows.Forms.Label Pu242PrecentLabel;
        private System.Windows.Forms.Label PuDateLabel;
        private System.Windows.Forms.Label Am241PercentLabel;
        private System.Windows.Forms.Label AmDateLabel;
        private System.Windows.Forms.Label Pu238ErrorLabel;
        private System.Windows.Forms.Label Pu239ErrorLabel;
        private System.Windows.Forms.Label Pu240ErrorLabel;
        private System.Windows.Forms.Label Pu241ErrorLabel;
        private System.Windows.Forms.Label Pu242ErrorLabel;
        private System.Windows.Forms.Label Am241ErrorLabel;
        private System.Windows.Forms.DateTimePicker PuDateTimePicker;
        private System.Windows.Forms.DateTimePicker AmDateTimePicker;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button IsoList;
	}
}