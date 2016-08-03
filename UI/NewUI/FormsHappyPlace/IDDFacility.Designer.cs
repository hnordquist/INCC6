namespace NewUI
{
    partial class IDDFacility
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDFacility));
            this.FacilityComboBox = new System.Windows.Forms.ComboBox();
            this.MBAComboBox = new System.Windows.Forms.ComboBox();
            this.DetectorIdComboBox = new System.Windows.Forms.ComboBox();
            this.InspectionNumberTextBox = new System.Windows.Forms.TextBox();
            this.InspectorNameTextBox = new System.Windows.Forms.TextBox();
            this.DetectorTypeTextBox = new System.Windows.Forms.TextBox();
            this.ElectronicsIdTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IndividualMultiplicityDistributionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SummedMultiplicityDistributionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SummedRawCoincidenceDataCheckBox = new System.Windows.Forms.CheckBox();
            this.IndividualCycleRateData = new System.Windows.Forms.CheckBox();
            this.IndividualCycleRawDataCheckBox = new System.Windows.Forms.CheckBox();
            this.IsotopicsCheckBox = new System.Windows.Forms.CheckBox();
            this.CalibrationParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.DetectorParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.ChangeDirBtn = new System.Windows.Forms.Button();
            this.FacilityLabel = new System.Windows.Forms.Label();
            this.MBALabel = new System.Windows.Forms.Label();
            this.InspectionNumberLabel = new System.Windows.Forms.Label();
            this.InspectorNameLabel = new System.Windows.Forms.Label();
            this.DetectorIdLabel = new System.Windows.Forms.Label();
            this.DetectorTypeLabel = new System.Windows.Forms.Label();
            this.ElectronicsIdLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SRType = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FacilityComboBox
            // 
            this.FacilityComboBox.FormattingEnabled = true;
            this.FacilityComboBox.Location = new System.Drawing.Point(111, 12);
            this.FacilityComboBox.Name = "FacilityComboBox";
            this.FacilityComboBox.Size = new System.Drawing.Size(164, 21);
            this.FacilityComboBox.TabIndex = 0;
            this.toolTip1.SetToolTip(this.FacilityComboBox, "Select the desired facility here.  \r\nIf you want to create a new facility, \r\ngo i" +
        "nto Maintenance mode and select\r\n\'Facility Add/Delete...\' under \'Maintain.\'");
            this.FacilityComboBox.SelectedIndexChanged += new System.EventHandler(this.FacilityComboBox_SelectedIndexChanged);
            // 
            // MBAComboBox
            // 
            this.MBAComboBox.FormattingEnabled = true;
            this.MBAComboBox.Location = new System.Drawing.Point(111, 39);
            this.MBAComboBox.Name = "MBAComboBox";
            this.MBAComboBox.Size = new System.Drawing.Size(164, 21);
            this.MBAComboBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.MBAComboBox, "Select the material balance area here. \r\nIf you want to create a new MBA, \r\ngo in" +
        "to Maintenance mode and select \r\n\'MBA Add/Delete...\' under \'Maintain.\'");
            this.MBAComboBox.SelectedIndexChanged += new System.EventHandler(this.MBAComboBox_SelectedIndexChanged);
            // 
            // DetectorIdComboBox
            // 
            this.DetectorIdComboBox.FormattingEnabled = true;
            this.DetectorIdComboBox.Location = new System.Drawing.Point(111, 150);
            this.DetectorIdComboBox.Name = "DetectorIdComboBox";
            this.DetectorIdComboBox.Size = new System.Drawing.Size(164, 21);
            this.DetectorIdComboBox.TabIndex = 2;
            this.toolTip1.SetToolTip(this.DetectorIdComboBox, "Select the detector id you want to use to identify this counter.\r\nIf you want to " +
        "create a new detector id, go into maintenance\r\n mode and select \'Detector add/de" +
        "lete\' under \'Maintain.\'");
            this.DetectorIdComboBox.SelectedIndexChanged += new System.EventHandler(this.DetectorIdComboBox_SelectedIndexChanged);
            // 
            // InspectionNumberTextBox
            // 
            this.InspectionNumberTextBox.Location = new System.Drawing.Point(111, 83);
            this.InspectionNumberTextBox.Name = "InspectionNumberTextBox";
            this.InspectionNumberTextBox.Size = new System.Drawing.Size(164, 20);
            this.InspectionNumberTextBox.TabIndex = 3;
            this.toolTip1.SetToolTip(this.InspectionNumberTextBox, resources.GetString("InspectionNumberTextBox.ToolTip"));
            this.InspectionNumberTextBox.Leave += new System.EventHandler(this.InspectionNumberTextBox_Leave);
            // 
            // InspectorNameTextBox
            // 
            this.InspectorNameTextBox.Location = new System.Drawing.Point(111, 109);
            this.InspectorNameTextBox.Name = "InspectorNameTextBox";
            this.InspectorNameTextBox.Size = new System.Drawing.Size(164, 20);
            this.InspectorNameTextBox.TabIndex = 4;
            this.InspectorNameTextBox.Leave += new System.EventHandler(this.InspectorNameTextBox_Leave);
            // 
            // DetectorTypeTextBox
            // 
            this.DetectorTypeTextBox.Location = new System.Drawing.Point(111, 177);
            this.DetectorTypeTextBox.Name = "DetectorTypeTextBox";
            this.DetectorTypeTextBox.Size = new System.Drawing.Size(164, 20);
            this.DetectorTypeTextBox.TabIndex = 5;
            this.toolTip1.SetToolTip(this.DetectorTypeTextBox, "Enter the detector type, such as AWCC or HLNC,\r\nfor the currently selected detect" +
        "or.");
            this.DetectorTypeTextBox.Leave += new System.EventHandler(this.DetectorTypeTextBox_Leave);
            // 
            // ElectronicsIdTextBox
            // 
            this.ElectronicsIdTextBox.Location = new System.Drawing.Point(111, 203);
            this.ElectronicsIdTextBox.Name = "ElectronicsIdTextBox";
            this.ElectronicsIdTextBox.Size = new System.Drawing.Size(164, 20);
            this.ElectronicsIdTextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ElectronicsIdTextBox, "Enter the electronics id for the currently-selected detector.");
            this.ElectronicsIdTextBox.Leave += new System.EventHandler(this.ElectronicsIdTextBox_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.IndividualMultiplicityDistributionsCheckBox);
            this.groupBox1.Controls.Add(this.SummedMultiplicityDistributionsCheckBox);
            this.groupBox1.Controls.Add(this.SummedRawCoincidenceDataCheckBox);
            this.groupBox1.Controls.Add(this.IndividualCycleRateData);
            this.groupBox1.Controls.Add(this.IndividualCycleRawDataCheckBox);
            this.groupBox1.Controls.Add(this.IsotopicsCheckBox);
            this.groupBox1.Controls.Add(this.CalibrationParametersCheckBox);
            this.groupBox1.Controls.Add(this.DetectorParametersCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(420, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 220);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional results to display";
            // 
            // IndividualMultiplicityDistributionsCheckBox
            // 
            this.IndividualMultiplicityDistributionsCheckBox.AutoSize = true;
            this.IndividualMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(22, 189);
            this.IndividualMultiplicityDistributionsCheckBox.Name = "IndividualMultiplicityDistributionsCheckBox";
            this.IndividualMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(179, 17);
            this.IndividualMultiplicityDistributionsCheckBox.TabIndex = 7;
            this.IndividualMultiplicityDistributionsCheckBox.Text = "Individual multiplicity distributions";
            this.toolTip1.SetToolTip(this.IndividualMultiplicityDistributionsCheckBox, "Check this box to include individual cycle multiplicity distributions for reals p" +
        "lus accidentals and accidentals with the displayed results.\r\n");
            this.IndividualMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // SummedMultiplicityDistributionsCheckBox
            // 
            this.SummedMultiplicityDistributionsCheckBox.AutoSize = true;
            this.SummedMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(22, 166);
            this.SummedMultiplicityDistributionsCheckBox.Name = "SummedMultiplicityDistributionsCheckBox";
            this.SummedMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(175, 17);
            this.SummedMultiplicityDistributionsCheckBox.TabIndex = 6;
            this.SummedMultiplicityDistributionsCheckBox.Text = "Summed multiplicity distributions";
            this.toolTip1.SetToolTip(this.SummedMultiplicityDistributionsCheckBox, "Check this box to include summed multiplicity distributions for reals plus accide" +
        "ntals and accidentals with the displayed results.\r\n");
            this.SummedMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // SummedRawCoincidenceDataCheckBox
            // 
            this.SummedRawCoincidenceDataCheckBox.AutoSize = true;
            this.SummedRawCoincidenceDataCheckBox.Location = new System.Drawing.Point(22, 143);
            this.SummedRawCoincidenceDataCheckBox.Name = "SummedRawCoincidenceDataCheckBox";
            this.SummedRawCoincidenceDataCheckBox.Size = new System.Drawing.Size(172, 17);
            this.SummedRawCoincidenceDataCheckBox.TabIndex = 5;
            this.SummedRawCoincidenceDataCheckBox.Text = "Summed raw coincidence data";
            this.toolTip1.SetToolTip(this.SummedRawCoincidenceDataCheckBox, "Check this box to include summed shift register singles, reals plus accidentals, " +
        "accidentals and scalers with the displayed results.\r\n");
            this.SummedRawCoincidenceDataCheckBox.UseVisualStyleBackColor = true;
            // 
            // IndividualCycleRateData
            // 
            this.IndividualCycleRateData.AutoSize = true;
            this.IndividualCycleRateData.Location = new System.Drawing.Point(22, 120);
            this.IndividualCycleRateData.Name = "IndividualCycleRateData";
            this.IndividualCycleRateData.Size = new System.Drawing.Size(144, 17);
            this.IndividualCycleRateData.TabIndex = 4;
            this.IndividualCycleRateData.Text = "Individual cycle rate data";
            this.toolTip1.SetToolTip(this.IndividualCycleRateData, "Check this box to include individual cycle singles, doubles, triples, masses (if " +
        "applicable) and QC test status with the displayed results.");
            this.IndividualCycleRateData.UseVisualStyleBackColor = true;
            // 
            // IndividualCycleRawDataCheckBox
            // 
            this.IndividualCycleRawDataCheckBox.AutoSize = true;
            this.IndividualCycleRawDataCheckBox.Location = new System.Drawing.Point(22, 97);
            this.IndividualCycleRawDataCheckBox.Name = "IndividualCycleRawDataCheckBox";
            this.IndividualCycleRawDataCheckBox.Size = new System.Drawing.Size(143, 17);
            this.IndividualCycleRawDataCheckBox.TabIndex = 3;
            this.IndividualCycleRawDataCheckBox.Text = "Individual cycle raw data";
            this.toolTip1.SetToolTip(this.IndividualCycleRawDataCheckBox, "Check this box to include individual cycle singles, reals plus accidentals, accid" +
        "entals, scalers and QC test status with the displayed results.\r\n");
            this.IndividualCycleRawDataCheckBox.UseVisualStyleBackColor = true;
            // 
            // IsotopicsCheckBox
            // 
            this.IsotopicsCheckBox.AutoSize = true;
            this.IsotopicsCheckBox.Location = new System.Drawing.Point(22, 74);
            this.IsotopicsCheckBox.Name = "IsotopicsCheckBox";
            this.IsotopicsCheckBox.Size = new System.Drawing.Size(68, 17);
            this.IsotopicsCheckBox.TabIndex = 2;
            this.IsotopicsCheckBox.Text = "Isotopics";
            this.toolTip1.SetToolTip(this.IsotopicsCheckBox, "Check this box to include original and updated isotopics with the displayed resul" +
        "ts.\r\n");
            this.IsotopicsCheckBox.UseVisualStyleBackColor = true;
            // 
            // CalibrationParametersCheckBox
            // 
            this.CalibrationParametersCheckBox.AutoSize = true;
            this.CalibrationParametersCheckBox.Location = new System.Drawing.Point(22, 51);
            this.CalibrationParametersCheckBox.Name = "CalibrationParametersCheckBox";
            this.CalibrationParametersCheckBox.Size = new System.Drawing.Size(130, 17);
            this.CalibrationParametersCheckBox.TabIndex = 1;
            this.CalibrationParametersCheckBox.Text = "Calibration parameters";
            this.toolTip1.SetToolTip(this.CalibrationParametersCheckBox, "Check this box to include calibration parameters with the displayed results.");
            this.CalibrationParametersCheckBox.UseVisualStyleBackColor = true;
            // 
            // DetectorParametersCheckBox
            // 
            this.DetectorParametersCheckBox.AutoSize = true;
            this.DetectorParametersCheckBox.Location = new System.Drawing.Point(22, 28);
            this.DetectorParametersCheckBox.Name = "DetectorParametersCheckBox";
            this.DetectorParametersCheckBox.Size = new System.Drawing.Size(122, 17);
            this.DetectorParametersCheckBox.TabIndex = 0;
            this.DetectorParametersCheckBox.Text = "Detector parameters";
            this.toolTip1.SetToolTip(this.DetectorParametersCheckBox, "Check this box to include detector and shift \r\nregister parameters with the displ" +
        "ayed results.");
            this.DetectorParametersCheckBox.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(563, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(563, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(563, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 10;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Visible = false;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // ChangeDirBtn
            // 
            this.ChangeDirBtn.Enabled = false;
            this.ChangeDirBtn.Location = new System.Drawing.Point(164, 297);
            this.ChangeDirBtn.Name = "ChangeDirBtn";
            this.ChangeDirBtn.Size = new System.Drawing.Size(218, 23);
            this.ChangeDirBtn.TabIndex = 12;
            this.ChangeDirBtn.Text = "Change ASCII results data folder...";
            this.toolTip1.SetToolTip(this.ChangeDirBtn, "Click on this button to change the folder where all ASCII results files are store" +
        "d. You will be prompted for the disk drive and full path.");
            this.ChangeDirBtn.UseVisualStyleBackColor = true;
            this.ChangeDirBtn.Click += new System.EventHandler(this.ChangeDirBtn_Click);
            // 
            // FacilityLabel
            // 
            this.FacilityLabel.AutoSize = true;
            this.FacilityLabel.Location = new System.Drawing.Point(66, 15);
            this.FacilityLabel.Name = "FacilityLabel";
            this.FacilityLabel.Size = new System.Drawing.Size(39, 13);
            this.FacilityLabel.TabIndex = 13;
            this.FacilityLabel.Text = "Facility";
            // 
            // MBALabel
            // 
            this.MBALabel.AutoSize = true;
            this.MBALabel.Location = new System.Drawing.Point(75, 42);
            this.MBALabel.Name = "MBALabel";
            this.MBALabel.Size = new System.Drawing.Size(30, 13);
            this.MBALabel.TabIndex = 14;
            this.MBALabel.Text = "MBA";
            // 
            // InspectionNumberLabel
            // 
            this.InspectionNumberLabel.AutoSize = true;
            this.InspectionNumberLabel.Location = new System.Drawing.Point(11, 86);
            this.InspectionNumberLabel.Name = "InspectionNumberLabel";
            this.InspectionNumberLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumberLabel.TabIndex = 15;
            this.InspectionNumberLabel.Text = "Inspection number";
            // 
            // InspectorNameLabel
            // 
            this.InspectorNameLabel.AutoSize = true;
            this.InspectorNameLabel.Location = new System.Drawing.Point(25, 112);
            this.InspectorNameLabel.Name = "InspectorNameLabel";
            this.InspectorNameLabel.Size = new System.Drawing.Size(80, 13);
            this.InspectorNameLabel.TabIndex = 16;
            this.InspectorNameLabel.Text = "Inspector name";
            // 
            // DetectorIdLabel
            // 
            this.DetectorIdLabel.AutoSize = true;
            this.DetectorIdLabel.Location = new System.Drawing.Point(46, 153);
            this.DetectorIdLabel.Name = "DetectorIdLabel";
            this.DetectorIdLabel.Size = new System.Drawing.Size(59, 13);
            this.DetectorIdLabel.TabIndex = 17;
            this.DetectorIdLabel.Text = "Detector id";
            // 
            // DetectorTypeLabel
            // 
            this.DetectorTypeLabel.AutoSize = true;
            this.DetectorTypeLabel.Location = new System.Drawing.Point(34, 180);
            this.DetectorTypeLabel.Name = "DetectorTypeLabel";
            this.DetectorTypeLabel.Size = new System.Drawing.Size(71, 13);
            this.DetectorTypeLabel.TabIndex = 18;
            this.DetectorTypeLabel.Text = "Detector type";
            // 
            // ElectronicsIdLabel
            // 
            this.ElectronicsIdLabel.AutoSize = true;
            this.ElectronicsIdLabel.Location = new System.Drawing.Point(35, 206);
            this.ElectronicsIdLabel.Name = "ElectronicsIdLabel";
            this.ElectronicsIdLabel.Size = new System.Drawing.Size(70, 13);
            this.ElectronicsIdLabel.TabIndex = 19;
            this.ElectronicsIdLabel.Text = "Electronics id";

            // 
            // IDDFacility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 350);
            this.Controls.Add(this.SRType);
            this.Controls.Add(this.ElectronicsIdLabel);
            this.Controls.Add(this.DetectorTypeLabel);
            this.Controls.Add(this.DetectorIdLabel);
            this.Controls.Add(this.InspectorNameLabel);
            this.Controls.Add(this.InspectionNumberLabel);
            this.Controls.Add(this.MBALabel);
            this.Controls.Add(this.FacilityLabel);
            this.Controls.Add(this.ChangeDirBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ElectronicsIdTextBox);
            this.Controls.Add(this.DetectorTypeTextBox);
            this.Controls.Add(this.InspectorNameTextBox);
            this.Controls.Add(this.InspectionNumberTextBox);
            this.Controls.Add(this.DetectorIdComboBox);
            this.Controls.Add(this.MBAComboBox);
            this.Controls.Add(this.FacilityComboBox);
            this.Name = "IDDFacility";
            this.Text = "Facility and Inspection Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox FacilityComboBox;
        private System.Windows.Forms.ComboBox MBAComboBox;
        private System.Windows.Forms.ComboBox DetectorIdComboBox;
        private System.Windows.Forms.TextBox InspectionNumberTextBox;
        private System.Windows.Forms.TextBox InspectorNameTextBox;
        private System.Windows.Forms.TextBox DetectorTypeTextBox;
        private System.Windows.Forms.TextBox ElectronicsIdTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox IndividualMultiplicityDistributionsCheckBox;
        private System.Windows.Forms.CheckBox SummedMultiplicityDistributionsCheckBox;
        private System.Windows.Forms.CheckBox SummedRawCoincidenceDataCheckBox;
        private System.Windows.Forms.CheckBox IndividualCycleRateData;
        private System.Windows.Forms.CheckBox IndividualCycleRawDataCheckBox;
        private System.Windows.Forms.CheckBox IsotopicsCheckBox;
        private System.Windows.Forms.CheckBox CalibrationParametersCheckBox;
        private System.Windows.Forms.CheckBox DetectorParametersCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button ChangeDirBtn;
        private System.Windows.Forms.Label FacilityLabel;
        private System.Windows.Forms.Label MBALabel;
        private System.Windows.Forms.Label InspectionNumberLabel;
        private System.Windows.Forms.Label InspectorNameLabel;
        private System.Windows.Forms.Label DetectorIdLabel;
        private System.Windows.Forms.Label DetectorTypeLabel;
        private System.Windows.Forms.Label ElectronicsIdLabel;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TextBox SRType;
	}
}