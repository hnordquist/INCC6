namespace NewUI
{
    partial class LMConnectionParams
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
            this.ALMMIPLabel = new System.Windows.Forms.Label();
            this.ALMMIPAddressTextBox = new System.Windows.Forms.TextBox();
            this.ALMMPortLabel = new System.Windows.Forms.Label();
            this.ALMMPortTextBox = new System.Windows.Forms.TextBox();
            this.ALMMBufferLabel = new System.Windows.Forms.Label();
            this.ALMMBufferTextBox = new System.Windows.Forms.TextBox();
            this.ALMMOKBtn = new System.Windows.Forms.Button();
            this.ALMMCancelBtn = new System.Windows.Forms.Button();
            this.ALMMHelpBtn = new System.Windows.Forms.Button();
            this.ALMMPerformanceTuningGroupBox = new System.Windows.Forms.GroupBox();
            this.CommandWaitTimemsLabel = new System.Windows.Forms.Label();
            this.ALMMCommandWaitTimeTextBox = new System.Windows.Forms.TextBox();
            this.ALMMCommandWaitTimeLabel = new System.Windows.Forms.Label();
            this.LongWaitTimemsLabel = new System.Windows.Forms.Label();
            this.ALMMLongWaitTextBox = new System.Windows.Forms.TextBox();
            this.ALMMLongWaitTimeLabel = new System.Windows.Forms.Label();
            this.ALMMNetworkSettings = new System.Windows.Forms.GroupBox();
            this.ALMMPanel = new System.Windows.Forms.Panel();
            this.ALMMHWConfig = new System.Windows.Forms.GroupBox();
            this.ALMMHVTextBox = new System.Windows.Forms.TextBox();
            this.ALMMHVLabel = new System.Windows.Forms.Label();
            this.ALMMBackBtn = new System.Windows.Forms.Button();
            this.ALMMLabel = new System.Windows.Forms.Label();
            this.SelectorPanel = new System.Windows.Forms.Panel();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.AddDetectorBtn = new System.Windows.Forms.Button();
            this.HelpButt = new System.Windows.Forms.Button();
            this.CancelButt = new System.Windows.Forms.Button();
            this.EditBtn = new System.Windows.Forms.Button();
            this.DetectorTypeLabel = new System.Windows.Forms.Label();
            this.DetectorComboBox = new System.Windows.Forms.ComboBox();
            this.PTR32Panel = new System.Windows.Forms.Panel();
            this.MCAComboBox = new System.Windows.Forms.ComboBox();
            this.MCANameLabel = new System.Windows.Forms.Label();
            this.MCAName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.VoltageTolerance = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.check_HV_set = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.VoltageTimeout = new System.Windows.Forms.TextBox();
            this.connIdLabel = new System.Windows.Forms.Label();
            this.ConnIdField = new System.Windows.Forms.TextBox();
            this.PTR32Back = new System.Windows.Forms.Button();
            this.PTR32Help = new System.Windows.Forms.Button();
            this.PTR32Cancel = new System.Windows.Forms.Button();
            this.PTR32Ok = new System.Windows.Forms.Button();
            this.connLabel = new System.Windows.Forms.Label();
            this.AddDetectorTypePanel = new System.Windows.Forms.Panel();
            this.AddDetectorNameTextBox = new System.Windows.Forms.TextBox();
            this.AddDetectorNameLabel = new System.Windows.Forms.Label();
            this.AddBackBtn = new System.Windows.Forms.Button();
            this.AddNextBtn = new System.Windows.Forms.Button();
            this.AddDetectorTypeComboBox = new System.Windows.Forms.ComboBox();
            this.AddDetectorTypeLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ALMMPerformanceTuningGroupBox.SuspendLayout();
            this.ALMMNetworkSettings.SuspendLayout();
            this.ALMMPanel.SuspendLayout();
            this.ALMMHWConfig.SuspendLayout();
            this.SelectorPanel.SuspendLayout();
            this.PTR32Panel.SuspendLayout();
            this.AddDetectorTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ALMMIPLabel
            // 
            this.ALMMIPLabel.AutoSize = true;
            this.ALMMIPLabel.Location = new System.Drawing.Point(25, 32);
            this.ALMMIPLabel.Name = "ALMMIPLabel";
            this.ALMMIPLabel.Size = new System.Drawing.Size(58, 13);
            this.ALMMIPLabel.TabIndex = 0;
            this.ALMMIPLabel.Text = "IP Address";
            // 
            // ALMMIPAddressTextBox
            // 
            this.ALMMIPAddressTextBox.Location = new System.Drawing.Point(115, 32);
            this.ALMMIPAddressTextBox.Name = "ALMMIPAddressTextBox";
            this.ALMMIPAddressTextBox.Size = new System.Drawing.Size(92, 20);
            this.ALMMIPAddressTextBox.TabIndex = 1;
            this.ALMMIPAddressTextBox.Text = "169.254.30.30";
            this.toolTip1.SetToolTip(this.ALMMIPAddressTextBox, "subnet mask for LM network");
            this.ALMMIPAddressTextBox.Leave += new System.EventHandler(this.ALMMIPAddressTextBox_Leave);
            // 
            // ALMMPortLabel
            // 
            this.ALMMPortLabel.AutoSize = true;
            this.ALMMPortLabel.Location = new System.Drawing.Point(25, 58);
            this.ALMMPortLabel.Name = "ALMMPortLabel";
            this.ALMMPortLabel.Size = new System.Drawing.Size(26, 13);
            this.ALMMPortLabel.TabIndex = 2;
            this.ALMMPortLabel.Text = "Port";
            // 
            // ALMMPortTextBox
            // 
            this.ALMMPortTextBox.Location = new System.Drawing.Point(115, 58);
            this.ALMMPortTextBox.Name = "ALMMPortTextBox";
            this.ALMMPortTextBox.Size = new System.Drawing.Size(49, 20);
            this.ALMMPortTextBox.TabIndex = 3;
            this.ALMMPortTextBox.Text = "5011";
            this.toolTip1.SetToolTip(this.ALMMPortTextBox, "port number for this app\'s LM TCP/IP listener");
            this.ALMMPortTextBox.Leave += new System.EventHandler(this.ALMMPortTextBox_Leave);
            // 
            // ALMMBufferLabel
            // 
            this.ALMMBufferLabel.AutoSize = true;
            this.ALMMBufferLabel.Location = new System.Drawing.Point(12, 23);
            this.ALMMBufferLabel.Name = "ALMMBufferLabel";
            this.ALMMBufferLabel.Size = new System.Drawing.Size(71, 13);
            this.ALMMBufferLabel.TabIndex = 19;
            this.ALMMBufferLabel.Text = "Socket buffer";
            // 
            // ALMMBufferTextBox
            // 
            this.ALMMBufferTextBox.Location = new System.Drawing.Point(147, 23);
            this.ALMMBufferTextBox.Name = "ALMMBufferTextBox";
            this.ALMMBufferTextBox.Size = new System.Drawing.Size(48, 20);
            this.ALMMBufferTextBox.TabIndex = 20;
            this.ALMMBufferTextBox.Text = "8192";
            this.toolTip1.SetToolTip(this.ALMMBufferTextBox, "(note:  must match hardware configuration)");
            this.ALMMBufferTextBox.Leave += new System.EventHandler(this.ALMMBufferTextBox_Leave);
            // 
            // ALMMOKBtn
            // 
            this.ALMMOKBtn.Location = new System.Drawing.Point(509, 48);
            this.ALMMOKBtn.Name = "ALMMOKBtn";
            this.ALMMOKBtn.Size = new System.Drawing.Size(75, 23);
            this.ALMMOKBtn.TabIndex = 28;
            this.ALMMOKBtn.Text = "OK";
            this.ALMMOKBtn.UseVisualStyleBackColor = true;
            this.ALMMOKBtn.Click += new System.EventHandler(this.ALMMOKBtn_Click);
            // 
            // ALMMCancelBtn
            // 
            this.ALMMCancelBtn.Location = new System.Drawing.Point(509, 106);
            this.ALMMCancelBtn.Name = "ALMMCancelBtn";
            this.ALMMCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.ALMMCancelBtn.TabIndex = 29;
            this.ALMMCancelBtn.Text = "Cancel";
            this.ALMMCancelBtn.UseVisualStyleBackColor = true;
            this.ALMMCancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ALMMHelpBtn
            // 
            this.ALMMHelpBtn.Location = new System.Drawing.Point(509, 135);
            this.ALMMHelpBtn.Name = "ALMMHelpBtn";
            this.ALMMHelpBtn.Size = new System.Drawing.Size(75, 23);
            this.ALMMHelpBtn.TabIndex = 30;
            this.ALMMHelpBtn.Text = "Help";
            this.ALMMHelpBtn.UseVisualStyleBackColor = true;
            // 
            // ALMMPerformanceTuningGroupBox
            // 
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.CommandWaitTimemsLabel);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.ALMMCommandWaitTimeTextBox);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.ALMMCommandWaitTimeLabel);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.LongWaitTimemsLabel);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.ALMMLongWaitTextBox);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.ALMMLongWaitTimeLabel);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.ALMMBufferTextBox);
            this.ALMMPerformanceTuningGroupBox.Controls.Add(this.ALMMBufferLabel);
            this.ALMMPerformanceTuningGroupBox.Location = new System.Drawing.Point(11, 177);
            this.ALMMPerformanceTuningGroupBox.Name = "ALMMPerformanceTuningGroupBox";
            this.ALMMPerformanceTuningGroupBox.Size = new System.Drawing.Size(281, 223);
            this.ALMMPerformanceTuningGroupBox.TabIndex = 31;
            this.ALMMPerformanceTuningGroupBox.TabStop = false;
            this.ALMMPerformanceTuningGroupBox.Text = "Performance tuning";
            // 
            // CommandWaitTimemsLabel
            // 
            this.CommandWaitTimemsLabel.AutoSize = true;
            this.CommandWaitTimemsLabel.Location = new System.Drawing.Point(201, 82);
            this.CommandWaitTimemsLabel.Name = "CommandWaitTimemsLabel";
            this.CommandWaitTimemsLabel.Size = new System.Drawing.Size(20, 13);
            this.CommandWaitTimemsLabel.TabIndex = 27;
            this.CommandWaitTimemsLabel.Text = "ms";
            this.CommandWaitTimemsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.CommandWaitTimemsLabel, " ");
            // 
            // ALMMCommandWaitTimeTextBox
            // 
            this.ALMMCommandWaitTimeTextBox.Location = new System.Drawing.Point(147, 79);
            this.ALMMCommandWaitTimeTextBox.Name = "ALMMCommandWaitTimeTextBox";
            this.ALMMCommandWaitTimeTextBox.Size = new System.Drawing.Size(48, 20);
            this.ALMMCommandWaitTimeTextBox.TabIndex = 26;
            this.ALMMCommandWaitTimeTextBox.Text = "30";
            this.toolTip1.SetToolTip(this.ALMMCommandWaitTimeTextBox, "(note:  must match hardware configuration)");
            this.ALMMCommandWaitTimeTextBox.Leave += new System.EventHandler(this.ALMMCommandWaitTimeTextBox_Leave);
            // 
            // ALMMCommandWaitTimeLabel
            // 
            this.ALMMCommandWaitTimeLabel.AutoSize = true;
            this.ALMMCommandWaitTimeLabel.Location = new System.Drawing.Point(12, 79);
            this.ALMMCommandWaitTimeLabel.Name = "ALMMCommandWaitTimeLabel";
            this.ALMMCommandWaitTimeLabel.Size = new System.Drawing.Size(105, 13);
            this.ALMMCommandWaitTimeLabel.TabIndex = 25;
            this.ALMMCommandWaitTimeLabel.Text = "Command Wait Time";
            // 
            // LongWaitTimemsLabel
            // 
            this.LongWaitTimemsLabel.AutoSize = true;
            this.LongWaitTimemsLabel.Location = new System.Drawing.Point(201, 56);
            this.LongWaitTimemsLabel.Name = "LongWaitTimemsLabel";
            this.LongWaitTimemsLabel.Size = new System.Drawing.Size(20, 13);
            this.LongWaitTimemsLabel.TabIndex = 24;
            this.LongWaitTimemsLabel.Text = "ms";
            this.LongWaitTimemsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.LongWaitTimemsLabel, " ");
            // 
            // ALMMLongWaitTextBox
            // 
            this.ALMMLongWaitTextBox.Location = new System.Drawing.Point(147, 53);
            this.ALMMLongWaitTextBox.Name = "ALMMLongWaitTextBox";
            this.ALMMLongWaitTextBox.Size = new System.Drawing.Size(48, 20);
            this.ALMMLongWaitTextBox.TabIndex = 23;
            this.ALMMLongWaitTextBox.Text = "250";
            this.toolTip1.SetToolTip(this.ALMMLongWaitTextBox, "(note:  must match hardware configuration)");
            this.ALMMLongWaitTextBox.Leave += new System.EventHandler(this.ALMMLongWaitTextBox_Leave);
            // 
            // ALMMLongWaitTimeLabel
            // 
            this.ALMMLongWaitTimeLabel.AutoSize = true;
            this.ALMMLongWaitTimeLabel.Location = new System.Drawing.Point(12, 53);
            this.ALMMLongWaitTimeLabel.Name = "ALMMLongWaitTimeLabel";
            this.ALMMLongWaitTimeLabel.Size = new System.Drawing.Size(82, 13);
            this.ALMMLongWaitTimeLabel.TabIndex = 22;
            this.ALMMLongWaitTimeLabel.Text = "Long Wait Time";
            // 
            // ALMMNetworkSettings
            // 
            this.ALMMNetworkSettings.Controls.Add(this.ALMMPortTextBox);
            this.ALMMNetworkSettings.Controls.Add(this.ALMMPortLabel);
            this.ALMMNetworkSettings.Controls.Add(this.ALMMIPAddressTextBox);
            this.ALMMNetworkSettings.Controls.Add(this.ALMMIPLabel);
            this.ALMMNetworkSettings.Location = new System.Drawing.Point(11, 43);
            this.ALMMNetworkSettings.Name = "ALMMNetworkSettings";
            this.ALMMNetworkSettings.Size = new System.Drawing.Size(237, 128);
            this.ALMMNetworkSettings.TabIndex = 33;
            this.ALMMNetworkSettings.TabStop = false;
            this.ALMMNetworkSettings.Text = "Network";
            // 
            // ALMMPanel
            // 
            this.ALMMPanel.Controls.Add(this.ALMMHWConfig);
            this.ALMMPanel.Controls.Add(this.ALMMBackBtn);
            this.ALMMPanel.Controls.Add(this.ALMMLabel);
            this.ALMMPanel.Controls.Add(this.ALMMNetworkSettings);
            this.ALMMPanel.Controls.Add(this.ALMMPerformanceTuningGroupBox);
            this.ALMMPanel.Controls.Add(this.ALMMHelpBtn);
            this.ALMMPanel.Controls.Add(this.ALMMCancelBtn);
            this.ALMMPanel.Controls.Add(this.ALMMOKBtn);
            this.ALMMPanel.Location = new System.Drawing.Point(12, 130);
            this.ALMMPanel.Name = "ALMMPanel";
            this.ALMMPanel.Size = new System.Drawing.Size(601, 416);
            this.ALMMPanel.TabIndex = 34;
            this.ALMMPanel.Visible = false;
            // 
            // ALMMHWConfig
            // 
            this.ALMMHWConfig.Controls.Add(this.ALMMHVTextBox);
            this.ALMMHWConfig.Controls.Add(this.ALMMHVLabel);
            this.ALMMHWConfig.Location = new System.Drawing.Point(315, 177);
            this.ALMMHWConfig.Name = "ALMMHWConfig";
            this.ALMMHWConfig.Size = new System.Drawing.Size(237, 223);
            this.ALMMHWConfig.TabIndex = 34;
            this.ALMMHWConfig.TabStop = false;
            this.ALMMHWConfig.Text = "ALMMHW Config";
            // 
            // ALMMHVTextBox
            // 
            this.ALMMHVTextBox.Location = new System.Drawing.Point(123, 46);
            this.ALMMHVTextBox.Name = "ALMMHVTextBox";
            this.ALMMHVTextBox.Size = new System.Drawing.Size(48, 20);
            this.ALMMHVTextBox.TabIndex = 30;
            this.ALMMHVTextBox.Text = "1680";
            this.toolTip1.SetToolTip(this.ALMMHVTextBox, "Expected high voltage");
            this.ALMMHVTextBox.Leave += new System.EventHandler(this.ALMMHV_Leave);
            // 
            // ALMMHVLabel
            // 
            this.ALMMHVLabel.AutoSize = true;
            this.ALMMHVLabel.Location = new System.Drawing.Point(46, 49);
            this.ALMMHVLabel.Name = "ALMMHVLabel";
            this.ALMMHVLabel.Size = new System.Drawing.Size(67, 13);
            this.ALMMHVLabel.TabIndex = 29;
            this.ALMMHVLabel.Text = "High voltage";
            this.toolTip1.SetToolTip(this.ALMMHVLabel, "Expected high voltage");
            // 
            // ALMMBackBtn
            // 
            this.ALMMBackBtn.Location = new System.Drawing.Point(509, 77);
            this.ALMMBackBtn.Name = "ALMMBackBtn";
            this.ALMMBackBtn.Size = new System.Drawing.Size(75, 23);
            this.ALMMBackBtn.TabIndex = 35;
            this.ALMMBackBtn.Text = "Back";
            this.ALMMBackBtn.UseVisualStyleBackColor = true;
            this.ALMMBackBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // ALMMLabel
            // 
            this.ALMMLabel.AutoSize = true;
            this.ALMMLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ALMMLabel.Location = new System.Drawing.Point(11, 11);
            this.ALMMLabel.Name = "ALMMLabel";
            this.ALMMLabel.Size = new System.Drawing.Size(252, 20);
            this.ALMMLabel.TabIndex = 34;
            this.ALMMLabel.Text = "ALMM Connection Parameters";
            // 
            // SelectorPanel
            // 
            this.SelectorPanel.Controls.Add(this.DeleteBtn);
            this.SelectorPanel.Controls.Add(this.AddDetectorBtn);
            this.SelectorPanel.Controls.Add(this.HelpButt);
            this.SelectorPanel.Controls.Add(this.CancelButt);
            this.SelectorPanel.Controls.Add(this.EditBtn);
            this.SelectorPanel.Controls.Add(this.DetectorTypeLabel);
            this.SelectorPanel.Controls.Add(this.DetectorComboBox);
            this.SelectorPanel.Location = new System.Drawing.Point(1, 2);
            this.SelectorPanel.Name = "SelectorPanel";
            this.SelectorPanel.Size = new System.Drawing.Size(617, 108);
            this.SelectorPanel.TabIndex = 35;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Enabled = false;
            this.DeleteBtn.Location = new System.Drawing.Point(350, 40);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(147, 23);
            this.DeleteBtn.TabIndex = 6;
            this.DeleteBtn.Text = "Delete selected definition";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // AddDetectorBtn
            // 
            this.AddDetectorBtn.Location = new System.Drawing.Point(350, 11);
            this.AddDetectorBtn.Name = "AddDetectorBtn";
            this.AddDetectorBtn.Size = new System.Drawing.Size(147, 23);
            this.AddDetectorBtn.TabIndex = 5;
            this.AddDetectorBtn.Text = "Add new detector definition";
            this.AddDetectorBtn.UseVisualStyleBackColor = true;
            this.AddDetectorBtn.Click += new System.EventHandler(this.AddDetectorBtn_Click);
            // 
            // HelpButt
            // 
            this.HelpButt.Location = new System.Drawing.Point(526, 40);
            this.HelpButt.Name = "HelpButt";
            this.HelpButt.Size = new System.Drawing.Size(75, 23);
            this.HelpButt.TabIndex = 4;
            this.HelpButt.Text = "Help";
            this.HelpButt.UseVisualStyleBackColor = true;
            this.HelpButt.Click += new System.EventHandler(this.HelpButt_Click);
            // 
            // CancelButt
            // 
            this.CancelButt.Location = new System.Drawing.Point(526, 11);
            this.CancelButt.Name = "CancelButt";
            this.CancelButt.Size = new System.Drawing.Size(75, 23);
            this.CancelButt.TabIndex = 3;
            this.CancelButt.Text = "Cancel";
            this.CancelButt.UseVisualStyleBackColor = true;
            this.CancelButt.Click += new System.EventHandler(this.CancelButt_Click);
            // 
            // EditBtn
            // 
            this.EditBtn.Enabled = false;
            this.EditBtn.Location = new System.Drawing.Point(350, 69);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(147, 23);
            this.EditBtn.TabIndex = 2;
            this.EditBtn.Text = "Edit selected definition";
            this.EditBtn.UseVisualStyleBackColor = true;
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // DetectorTypeLabel
            // 
            this.DetectorTypeLabel.AutoSize = true;
            this.DetectorTypeLabel.Location = new System.Drawing.Point(15, 16);
            this.DetectorTypeLabel.Name = "DetectorTypeLabel";
            this.DetectorTypeLabel.Size = new System.Drawing.Size(135, 13);
            this.DetectorTypeLabel.TabIndex = 1;
            this.DetectorTypeLabel.Text = "Existing detector definitions";
            // 
            // DetectorComboBox
            // 
            this.DetectorComboBox.FormattingEnabled = true;
            this.DetectorComboBox.Items.AddRange(new object[] {
            "ALMM",
            "PTR-32"});
            this.DetectorComboBox.Location = new System.Drawing.Point(156, 13);
            this.DetectorComboBox.Name = "DetectorComboBox";
            this.DetectorComboBox.Size = new System.Drawing.Size(180, 21);
            this.DetectorComboBox.TabIndex = 0;
            this.DetectorComboBox.SelectedIndexChanged += new System.EventHandler(this.DetectorTypeComboBox_SelectedIndexChanged);
            // 
            // PTR32Panel
            // 
            this.PTR32Panel.Controls.Add(this.MCAComboBox);
            this.PTR32Panel.Controls.Add(this.MCANameLabel);
            this.PTR32Panel.Controls.Add(this.MCAName);
            this.PTR32Panel.Controls.Add(this.label8);
            this.PTR32Panel.Controls.Add(this.VoltageTolerance);
            this.PTR32Panel.Controls.Add(this.label7);
            this.PTR32Panel.Controls.Add(this.check_HV_set);
            this.PTR32Panel.Controls.Add(this.label6);
            this.PTR32Panel.Controls.Add(this.label5);
            this.PTR32Panel.Controls.Add(this.VoltageTimeout);
            this.PTR32Panel.Controls.Add(this.connIdLabel);
            this.PTR32Panel.Controls.Add(this.ConnIdField);
            this.PTR32Panel.Controls.Add(this.PTR32Back);
            this.PTR32Panel.Controls.Add(this.PTR32Help);
            this.PTR32Panel.Controls.Add(this.PTR32Cancel);
            this.PTR32Panel.Controls.Add(this.PTR32Ok);
            this.PTR32Panel.Controls.Add(this.connLabel);
            this.PTR32Panel.Location = new System.Drawing.Point(659, 130);
            this.PTR32Panel.Name = "PTR32Panel";
            this.PTR32Panel.Size = new System.Drawing.Size(596, 416);
            this.PTR32Panel.TabIndex = 36;
            this.PTR32Panel.Visible = false;
            // 
            // MCAComboBox
            // 
            this.MCAComboBox.FormattingEnabled = true;
            this.MCAComboBox.Location = new System.Drawing.Point(192, 128);
            this.MCAComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.MCAComboBox.Name = "MCAComboBox";
            this.MCAComboBox.Size = new System.Drawing.Size(271, 21);
            this.MCAComboBox.TabIndex = 52;
            this.toolTip1.SetToolTip(this.MCAComboBox, "Identified MCA527 devices on the current network");
            this.MCAComboBox.SelectedIndexChanged += new System.EventHandler(this.MCAComboBox_SelectedIndexChanged);
            // 
            // MCANameLabel
            // 
            this.MCANameLabel.AutoSize = true;
            this.MCANameLabel.Location = new System.Drawing.Point(41, 82);
            this.MCANameLabel.Name = "MCANameLabel";
            this.MCANameLabel.Size = new System.Drawing.Size(125, 13);
            this.MCANameLabel.TabIndex = 51;
            this.MCANameLabel.Text = "MCA-527 detector name ";
            this.toolTip1.SetToolTip(this.MCANameLabel, "Enter the identifier of your PTR-32 instrument here. ");
            // 
            // MCAName
            // 
            this.MCAName.Location = new System.Drawing.Point(192, 79);
            this.MCAName.Name = "MCAName";
            this.MCAName.ReadOnly = true;
            this.MCAName.Size = new System.Drawing.Size(100, 20);
            this.MCAName.TabIndex = 50;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(268, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 49;
            this.label8.Text = "volts";
            // 
            // VoltageTolerance
            // 
            this.VoltageTolerance.Location = new System.Drawing.Point(192, 198);
            this.VoltageTolerance.Name = "VoltageTolerance";
            this.VoltageTolerance.Size = new System.Drawing.Size(61, 20);
            this.VoltageTolerance.TabIndex = 48;
            this.VoltageTolerance.Leave += new System.EventHandler(this.VoltageTolerance_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(75, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 47;
            this.label7.Text = "Voltage tolerance +-:";
            // 
            // check_HV_set
            // 
            this.check_HV_set.AutoSize = true;
            this.check_HV_set.Location = new System.Drawing.Point(44, 139);
            this.check_HV_set.Name = "check_HV_set";
            this.check_HV_set.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.check_HV_set.Size = new System.Drawing.Size(115, 17);
            this.check_HV_set.TabIndex = 46;
            this.check_HV_set.Text = "Disable HV Setting";
            this.check_HV_set.UseVisualStyleBackColor = true;
            this.check_HV_set.CheckedChanged += new System.EventHandler(this.check_HV_set_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(265, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 44;
            this.label6.Text = "sec.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(97, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 43;
            this.label5.Text = "Voltage timeout:";
            this.toolTip1.SetToolTip(this.label5, "Voltage setting timeout in seconds ");
            // 
            // VoltageTimeout
            // 
            this.VoltageTimeout.Location = new System.Drawing.Point(192, 166);
            this.VoltageTimeout.Name = "VoltageTimeout";
            this.VoltageTimeout.Size = new System.Drawing.Size(61, 20);
            this.VoltageTimeout.TabIndex = 42;
            this.VoltageTimeout.Leave += new System.EventHandler(this.VoltageTimeout_Leave);
            // 
            // connIdLabel
            // 
            this.connIdLabel.AutoSize = true;
            this.connIdLabel.Location = new System.Drawing.Point(41, 108);
            this.connIdLabel.Name = "connIdLabel";
            this.connIdLabel.Size = new System.Drawing.Size(143, 13);
            this.connIdLabel.TabIndex = 41;
            this.connIdLabel.Text = "PTR-32 instrument identifier: ";
            this.toolTip1.SetToolTip(this.connIdLabel, "Enter the identifier of your PTR-32 instrument here. ");
            // 
            // ConnIdField
            // 
            this.ConnIdField.Location = new System.Drawing.Point(192, 105);
            this.ConnIdField.Name = "ConnIdField";
            this.ConnIdField.Size = new System.Drawing.Size(100, 20);
            this.ConnIdField.TabIndex = 40;
            this.ConnIdField.Leave += new System.EventHandler(this.PTR32Id_Leave);
            // 
            // PTR32Back
            // 
            this.PTR32Back.Location = new System.Drawing.Point(504, 77);
            this.PTR32Back.Name = "PTR32Back";
            this.PTR32Back.Size = new System.Drawing.Size(75, 23);
            this.PTR32Back.TabIndex = 39;
            this.PTR32Back.Text = "Back";
            this.PTR32Back.UseVisualStyleBackColor = true;
            this.PTR32Back.Click += new System.EventHandler(this.PTR32Back_Click);
            // 
            // PTR32Help
            // 
            this.PTR32Help.Location = new System.Drawing.Point(504, 135);
            this.PTR32Help.Name = "PTR32Help";
            this.PTR32Help.Size = new System.Drawing.Size(75, 23);
            this.PTR32Help.TabIndex = 38;
            this.PTR32Help.Text = "Help";
            this.PTR32Help.UseVisualStyleBackColor = true;
            this.PTR32Help.Click += new System.EventHandler(this.PTR32Help_Click);
            // 
            // PTR32Cancel
            // 
            this.PTR32Cancel.Location = new System.Drawing.Point(504, 106);
            this.PTR32Cancel.Name = "PTR32Cancel";
            this.PTR32Cancel.Size = new System.Drawing.Size(75, 23);
            this.PTR32Cancel.TabIndex = 37;
            this.PTR32Cancel.Text = "Cancel";
            this.PTR32Cancel.UseVisualStyleBackColor = true;
            this.PTR32Cancel.Click += new System.EventHandler(this.PTR32Cancel_Click);
            // 
            // PTR32Ok
            // 
            this.PTR32Ok.Location = new System.Drawing.Point(504, 48);
            this.PTR32Ok.Name = "PTR32Ok";
            this.PTR32Ok.Size = new System.Drawing.Size(75, 23);
            this.PTR32Ok.TabIndex = 36;
            this.PTR32Ok.Text = "OK";
            this.PTR32Ok.UseVisualStyleBackColor = true;
            this.PTR32Ok.Click += new System.EventHandler(this.PTR32Ok_Click);
            // 
            // connLabel
            // 
            this.connLabel.AutoSize = true;
            this.connLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connLabel.Location = new System.Drawing.Point(7, 16);
            this.connLabel.Name = "connLabel";
            this.connLabel.Size = new System.Drawing.Size(262, 20);
            this.connLabel.TabIndex = 0;
            this.connLabel.Text = "PTR-32 Connection Parameters";
            // 
            // AddDetectorTypePanel
            // 
            this.AddDetectorTypePanel.Controls.Add(this.AddDetectorNameTextBox);
            this.AddDetectorTypePanel.Controls.Add(this.AddDetectorNameLabel);
            this.AddDetectorTypePanel.Controls.Add(this.AddBackBtn);
            this.AddDetectorTypePanel.Controls.Add(this.AddNextBtn);
            this.AddDetectorTypePanel.Controls.Add(this.AddDetectorTypeComboBox);
            this.AddDetectorTypePanel.Controls.Add(this.AddDetectorTypeLabel);
            this.AddDetectorTypePanel.Location = new System.Drawing.Point(722, 20);
            this.AddDetectorTypePanel.Name = "AddDetectorTypePanel";
            this.AddDetectorTypePanel.Size = new System.Drawing.Size(382, 80);
            this.AddDetectorTypePanel.TabIndex = 37;
            this.AddDetectorTypePanel.Visible = false;
            // 
            // AddDetectorNameTextBox
            // 
            this.AddDetectorNameTextBox.Location = new System.Drawing.Point(123, 16);
            this.AddDetectorNameTextBox.Name = "AddDetectorNameTextBox";
            this.AddDetectorNameTextBox.Size = new System.Drawing.Size(153, 20);
            this.AddDetectorNameTextBox.TabIndex = 5;
            this.AddDetectorNameTextBox.TextChanged += new System.EventHandler(this.AddDetectorNameTextBox_TextChanged);
            this.AddDetectorNameTextBox.Leave += new System.EventHandler(this.AddDetectorNameTextBox_Leave);
            // 
            // AddDetectorNameLabel
            // 
            this.AddDetectorNameLabel.AutoSize = true;
            this.AddDetectorNameLabel.Location = new System.Drawing.Point(40, 19);
            this.AddDetectorNameLabel.Name = "AddDetectorNameLabel";
            this.AddDetectorNameLabel.Size = new System.Drawing.Size(77, 13);
            this.AddDetectorNameLabel.TabIndex = 4;
            this.AddDetectorNameLabel.Text = "Detector name";
            // 
            // AddBackBtn
            // 
            this.AddBackBtn.Location = new System.Drawing.Point(297, 40);
            this.AddBackBtn.Name = "AddBackBtn";
            this.AddBackBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBackBtn.TabIndex = 3;
            this.AddBackBtn.Text = "Back";
            this.AddBackBtn.UseVisualStyleBackColor = true;
            this.AddBackBtn.Click += new System.EventHandler(this.AddBackBtn_Click);
            // 
            // AddNextBtn
            // 
            this.AddNextBtn.Enabled = false;
            this.AddNextBtn.Location = new System.Drawing.Point(297, 11);
            this.AddNextBtn.Name = "AddNextBtn";
            this.AddNextBtn.Size = new System.Drawing.Size(75, 23);
            this.AddNextBtn.TabIndex = 2;
            this.AddNextBtn.Text = "Next";
            this.AddNextBtn.UseVisualStyleBackColor = true;
            this.AddNextBtn.Click += new System.EventHandler(this.AddNextBtn_Click);
            // 
            // AddDetectorTypeComboBox
            // 
            this.AddDetectorTypeComboBox.FormattingEnabled = true;
            this.AddDetectorTypeComboBox.Location = new System.Drawing.Point(123, 42);
            this.AddDetectorTypeComboBox.Name = "AddDetectorTypeComboBox";
            this.AddDetectorTypeComboBox.Size = new System.Drawing.Size(153, 21);
            this.AddDetectorTypeComboBox.TabIndex = 1;
            this.AddDetectorTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.AddDetectorTypeComboBox_SelectedIndexChanged);
            // 
            // AddDetectorTypeLabel
            // 
            this.AddDetectorTypeLabel.AutoSize = true;
            this.AddDetectorTypeLabel.Location = new System.Drawing.Point(13, 45);
            this.AddDetectorTypeLabel.Name = "AddDetectorTypeLabel";
            this.AddDetectorTypeLabel.Size = new System.Drawing.Size(104, 13);
            this.AddDetectorTypeLabel.TabIndex = 0;
            this.AddDetectorTypeLabel.Text = "Detector type to add";
            // 
            // LMConnectionParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1197, 750);
            this.Controls.Add(this.AddDetectorTypePanel);
            this.Controls.Add(this.PTR32Panel);
            this.Controls.Add(this.SelectorPanel);
            this.Controls.Add(this.ALMMPanel);
            this.Name = "LMConnectionParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List Mode Connection Parameters";
            this.TopMost = true;
            this.ALMMPerformanceTuningGroupBox.ResumeLayout(false);
            this.ALMMPerformanceTuningGroupBox.PerformLayout();
            this.ALMMNetworkSettings.ResumeLayout(false);
            this.ALMMNetworkSettings.PerformLayout();
            this.ALMMPanel.ResumeLayout(false);
            this.ALMMPanel.PerformLayout();
            this.ALMMHWConfig.ResumeLayout(false);
            this.ALMMHWConfig.PerformLayout();
            this.SelectorPanel.ResumeLayout(false);
            this.SelectorPanel.PerformLayout();
            this.PTR32Panel.ResumeLayout(false);
            this.PTR32Panel.PerformLayout();
            this.AddDetectorTypePanel.ResumeLayout(false);
            this.AddDetectorTypePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ALMMIPLabel;
        private System.Windows.Forms.TextBox ALMMIPAddressTextBox;
        private System.Windows.Forms.Label ALMMPortLabel;
        private System.Windows.Forms.TextBox ALMMPortTextBox;
        private System.Windows.Forms.Label ALMMBufferLabel;
        private System.Windows.Forms.TextBox ALMMBufferTextBox;
        private System.Windows.Forms.GroupBox ALMMHWConfig;
        private System.Windows.Forms.TextBox ALMMHVTextBox;
        private System.Windows.Forms.Label ALMMHVLabel;
        private System.Windows.Forms.Button ALMMOKBtn;
        private System.Windows.Forms.Button ALMMCancelBtn;
        private System.Windows.Forms.Button ALMMHelpBtn;
        private System.Windows.Forms.GroupBox ALMMPerformanceTuningGroupBox;
        private System.Windows.Forms.GroupBox ALMMNetworkSettings;
        private System.Windows.Forms.Panel ALMMPanel;
        private System.Windows.Forms.Label ALMMLabel;
        private System.Windows.Forms.Label CommandWaitTimemsLabel;
        private System.Windows.Forms.TextBox ALMMCommandWaitTimeTextBox;
        private System.Windows.Forms.Label ALMMCommandWaitTimeLabel;
        private System.Windows.Forms.Label LongWaitTimemsLabel;
        private System.Windows.Forms.TextBox ALMMLongWaitTextBox;
        private System.Windows.Forms.Label ALMMLongWaitTimeLabel;
        private System.Windows.Forms.Panel SelectorPanel;
        private System.Windows.Forms.Button HelpButt;
        private System.Windows.Forms.Button CancelButt;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.Label DetectorTypeLabel;
        private System.Windows.Forms.ComboBox DetectorComboBox;
        private System.Windows.Forms.Button ALMMBackBtn;
        private System.Windows.Forms.Panel PTR32Panel;
        private System.Windows.Forms.Label connLabel;
        private System.Windows.Forms.Button AddDetectorBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Panel AddDetectorTypePanel;
        private System.Windows.Forms.ComboBox AddDetectorTypeComboBox;
        private System.Windows.Forms.Label AddDetectorTypeLabel;
        private System.Windows.Forms.Button AddBackBtn;
        private System.Windows.Forms.Button AddNextBtn;
        private System.Windows.Forms.TextBox AddDetectorNameTextBox;
        private System.Windows.Forms.Label AddDetectorNameLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button PTR32Back;
        private System.Windows.Forms.Button PTR32Help;
        private System.Windows.Forms.Button PTR32Cancel;
        private System.Windows.Forms.Button PTR32Ok;
        private System.Windows.Forms.Label connIdLabel;
        private System.Windows.Forms.TextBox ConnIdField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox VoltageTimeout;
        private System.Windows.Forms.CheckBox check_HV_set;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox VoltageTolerance;
		private System.Windows.Forms.Label MCANameLabel;
		private System.Windows.Forms.TextBox MCAName;
		private System.Windows.Forms.ComboBox MCAComboBox;

    }
}