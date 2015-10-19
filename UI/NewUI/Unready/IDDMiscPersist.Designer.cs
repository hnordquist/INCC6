namespace NewUI
{
    partial class IDDMiscPersist
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
            this.ControlDBVistaPopUpsGroupBox = new System.Windows.Forms.GroupBox();
            this.EnableDBErrorsRadioButton = new System.Windows.Forms.RadioButton();
            this.SupressDBErrorsRadioButton = new System.Windows.Forms.RadioButton();
            this.ImportProcessingGroupBox = new System.Windows.Forms.GroupBox();
            this.EnableDialogsRadioButton = new System.Windows.Forms.RadioButton();
            this.SupressDialogsRadioButton = new System.Windows.Forms.RadioButton();
            this.EnableLoggingCheckBox = new System.Windows.Forms.CheckBox();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.ClearAskOnceSettingsBtn = new System.Windows.Forms.Button();
            this.EnableSilentFolderCreationCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableAuxRatioReportingCheckBox = new System.Windows.Forms.CheckBox();
            this.ControlDBVistaPopUpsGroupBox.SuspendLayout();
            this.ImportProcessingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlDBVistaPopUpsGroupBox
            // 
            this.ControlDBVistaPopUpsGroupBox.Controls.Add(this.EnableDBErrorsRadioButton);
            this.ControlDBVistaPopUpsGroupBox.Controls.Add(this.SupressDBErrorsRadioButton);
            this.ControlDBVistaPopUpsGroupBox.Location = new System.Drawing.Point(18, 20);
            this.ControlDBVistaPopUpsGroupBox.Name = "ControlDBVistaPopUpsGroupBox";
            this.ControlDBVistaPopUpsGroupBox.Size = new System.Drawing.Size(315, 97);
            this.ControlDBVistaPopUpsGroupBox.TabIndex = 0;
            this.ControlDBVistaPopUpsGroupBox.TabStop = false;
            this.ControlDBVistaPopUpsGroupBox.Text = "Control DB Vista Pop-ups";
            // 
            // EnableDBErrorsRadioButton
            // 
            this.EnableDBErrorsRadioButton.AutoSize = true;
            this.EnableDBErrorsRadioButton.Location = new System.Drawing.Point(25, 55);
            this.EnableDBErrorsRadioButton.Name = "EnableDBErrorsRadioButton";
            this.EnableDBErrorsRadioButton.Size = new System.Drawing.Size(162, 17);
            this.EnableDBErrorsRadioButton.TabIndex = 1;
            this.EnableDBErrorsRadioButton.TabStop = true;
            this.EnableDBErrorsRadioButton.Text = "Enable DB Error Notifications";
            this.EnableDBErrorsRadioButton.UseVisualStyleBackColor = true;
            this.EnableDBErrorsRadioButton.CheckedChanged += new System.EventHandler(this.EnableDBErrorsRadioButton_CheckedChanged);
            // 
            // SupressDBErrorsRadioButton
            // 
            this.SupressDBErrorsRadioButton.AutoSize = true;
            this.SupressDBErrorsRadioButton.Location = new System.Drawing.Point(25, 32);
            this.SupressDBErrorsRadioButton.Name = "SupressDBErrorsRadioButton";
            this.SupressDBErrorsRadioButton.Size = new System.Drawing.Size(173, 17);
            this.SupressDBErrorsRadioButton.TabIndex = 0;
            this.SupressDBErrorsRadioButton.TabStop = true;
            this.SupressDBErrorsRadioButton.Text = "Suppress DB Error Notifications";
            this.SupressDBErrorsRadioButton.UseVisualStyleBackColor = true;
            this.SupressDBErrorsRadioButton.CheckedChanged += new System.EventHandler(this.SupressDBErrorsRadioButton_CheckedChanged);
            // 
            // ImportProcessingGroupBox
            // 
            this.ImportProcessingGroupBox.Controls.Add(this.EnableDialogsRadioButton);
            this.ImportProcessingGroupBox.Controls.Add(this.SupressDialogsRadioButton);
            this.ImportProcessingGroupBox.Controls.Add(this.EnableLoggingCheckBox);
            this.ImportProcessingGroupBox.Location = new System.Drawing.Point(18, 141);
            this.ImportProcessingGroupBox.Name = "ImportProcessingGroupBox";
            this.ImportProcessingGroupBox.Size = new System.Drawing.Size(315, 120);
            this.ImportProcessingGroupBox.TabIndex = 1;
            this.ImportProcessingGroupBox.TabStop = false;
            this.ImportProcessingGroupBox.Text = "IMPORT Processing Dialog and Pop-up Control";
            // 
            // EnableDialogsRadioButton
            // 
            this.EnableDialogsRadioButton.AutoSize = true;
            this.EnableDialogsRadioButton.Location = new System.Drawing.Point(25, 56);
            this.EnableDialogsRadioButton.Name = "EnableDialogsRadioButton";
            this.EnableDialogsRadioButton.Size = new System.Drawing.Size(274, 17);
            this.EnableDialogsRadioButton.TabIndex = 2;
            this.EnableDialogsRadioButton.TabStop = true;
            this.EnableDialogsRadioButton.Text = "Enable Normal Dialogs and Pop-ups During IMPORT";
            this.EnableDialogsRadioButton.UseVisualStyleBackColor = true;
            this.EnableDialogsRadioButton.CheckedChanged += new System.EventHandler(this.EnableDialogsRadioButton_CheckedChanged);
            // 
            // SupressDialogsRadioButton
            // 
            this.SupressDialogsRadioButton.AutoSize = true;
            this.SupressDialogsRadioButton.Location = new System.Drawing.Point(25, 33);
            this.SupressDialogsRadioButton.Name = "SupressDialogsRadioButton";
            this.SupressDialogsRadioButton.Size = new System.Drawing.Size(257, 17);
            this.SupressDialogsRadioButton.TabIndex = 1;
            this.SupressDialogsRadioButton.TabStop = true;
            this.SupressDialogsRadioButton.Text = "Supress All Dialogs and Pop-ups During IMPORT";
            this.SupressDialogsRadioButton.UseVisualStyleBackColor = true;
            this.SupressDialogsRadioButton.CheckedChanged += new System.EventHandler(this.SupressDialogsRadioButton_CheckedChanged);
            // 
            // EnableLoggingCheckBox
            // 
            this.EnableLoggingCheckBox.AutoSize = true;
            this.EnableLoggingCheckBox.Location = new System.Drawing.Point(25, 79);
            this.EnableLoggingCheckBox.Name = "EnableLoggingCheckBox";
            this.EnableLoggingCheckBox.Size = new System.Drawing.Size(132, 17);
            this.EnableLoggingCheckBox.TabIndex = 0;
            this.EnableLoggingCheckBox.Text = "Enable Import Logging";
            this.EnableLoggingCheckBox.UseVisualStyleBackColor = true;
            this.EnableLoggingCheckBox.CheckedChanged += new System.EventHandler(this.EnableLoggingCheckBox_CheckedChanged);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(351, 37);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 2;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(351, 69);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // ClearAskOnceSettingsBtn
            // 
            this.ClearAskOnceSettingsBtn.Location = new System.Drawing.Point(91, 337);
            this.ClearAskOnceSettingsBtn.Name = "ClearAskOnceSettingsBtn";
            this.ClearAskOnceSettingsBtn.Size = new System.Drawing.Size(143, 23);
            this.ClearAskOnceSettingsBtn.TabIndex = 4;
            this.ClearAskOnceSettingsBtn.Text = "Clear \'Ask Once\' Settings";
            this.ClearAskOnceSettingsBtn.UseVisualStyleBackColor = true;
            this.ClearAskOnceSettingsBtn.Click += new System.EventHandler(this.ClearAskOnceSettingsBtn_Click);
            // 
            // EnableSilentFolderCreationCheckBox
            // 
            this.EnableSilentFolderCreationCheckBox.AutoSize = true;
            this.EnableSilentFolderCreationCheckBox.Location = new System.Drawing.Point(43, 276);
            this.EnableSilentFolderCreationCheckBox.Name = "EnableSilentFolderCreationCheckBox";
            this.EnableSilentFolderCreationCheckBox.Size = new System.Drawing.Size(200, 17);
            this.EnableSilentFolderCreationCheckBox.TabIndex = 5;
            this.EnableSilentFolderCreationCheckBox.Text = "Enable Silent Missing Folder Creation";
            this.EnableSilentFolderCreationCheckBox.UseVisualStyleBackColor = true;
            this.EnableSilentFolderCreationCheckBox.CheckedChanged += new System.EventHandler(this.EnableSilentFolderCreationCheckBox_CheckedChanged);
            // 
            // EnableAuxRatioReportingCheckBox
            // 
            this.EnableAuxRatioReportingCheckBox.AutoSize = true;
            this.EnableAuxRatioReportingCheckBox.Location = new System.Drawing.Point(43, 299);
            this.EnableAuxRatioReportingCheckBox.Name = "EnableAuxRatioReportingCheckBox";
            this.EnableAuxRatioReportingCheckBox.Size = new System.Drawing.Size(157, 17);
            this.EnableAuxRatioReportingCheckBox.TabIndex = 6;
            this.EnableAuxRatioReportingCheckBox.Text = "Enable Aux Ratio Reporting";
            this.EnableAuxRatioReportingCheckBox.UseVisualStyleBackColor = true;
            this.EnableAuxRatioReportingCheckBox.CheckedChanged += new System.EventHandler(this.EnableAuxRatioReportingCheckBox_CheckedChanged);
            // 
            // IDDMiscPersist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 378);
            this.Controls.Add(this.EnableAuxRatioReportingCheckBox);
            this.Controls.Add(this.EnableSilentFolderCreationCheckBox);
            this.Controls.Add(this.ClearAskOnceSettingsBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.ImportProcessingGroupBox);
            this.Controls.Add(this.ControlDBVistaPopUpsGroupBox);
            this.Name = "IDDMiscPersist";
            this.Text = "Settings";
            this.ControlDBVistaPopUpsGroupBox.ResumeLayout(false);
            this.ControlDBVistaPopUpsGroupBox.PerformLayout();
            this.ImportProcessingGroupBox.ResumeLayout(false);
            this.ImportProcessingGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ControlDBVistaPopUpsGroupBox;
        private System.Windows.Forms.RadioButton EnableDBErrorsRadioButton;
        private System.Windows.Forms.RadioButton SupressDBErrorsRadioButton;
        private System.Windows.Forms.GroupBox ImportProcessingGroupBox;
        private System.Windows.Forms.RadioButton EnableDialogsRadioButton;
        private System.Windows.Forms.RadioButton SupressDialogsRadioButton;
        private System.Windows.Forms.CheckBox EnableLoggingCheckBox;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button ClearAskOnceSettingsBtn;
        private System.Windows.Forms.CheckBox EnableSilentFolderCreationCheckBox;
        private System.Windows.Forms.CheckBox EnableAuxRatioReportingCheckBox;
    }
}