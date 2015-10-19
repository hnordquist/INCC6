namespace NewUI
{
    partial class IDDCollarCal
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
            this.CurveTypeComboBox = new System.Windows.Forms.ComboBox();
            this.ATextBox = new System.Windows.Forms.TextBox();
            this.BTextBox = new System.Windows.Forms.TextBox();
            this.CTextBox = new System.Windows.Forms.TextBox();
            this.DTextBox = new System.Windows.Forms.TextBox();
            this.VarianceATextBox = new System.Windows.Forms.TextBox();
            this.VarianceBTextBox = new System.Windows.Forms.TextBox();
            this.VarianceCTextBox = new System.Windows.Forms.TextBox();
            this.VarianceDTextBox = new System.Windows.Forms.TextBox();
            this.LowerMassLimitTextBox = new System.Windows.Forms.TextBox();
            this.UpperMassLimitTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceABTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceACTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceADTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceBCTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceBDTextBox = new System.Windows.Forms.TextBox();
            this.SigmaXTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceCDTextBox = new System.Windows.Forms.TextBox();
            this.NextBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CurveTypeLabel = new System.Windows.Forms.Label();
            this.ALabel = new System.Windows.Forms.Label();
            this.BLabel = new System.Windows.Forms.Label();
            this.CLabel = new System.Windows.Forms.Label();
            this.DLabel = new System.Windows.Forms.Label();
            this.VarALabel = new System.Windows.Forms.Label();
            this.VarBLabel = new System.Windows.Forms.Label();
            this.VarClabel = new System.Windows.Forms.Label();
            this.VarDLabel = new System.Windows.Forms.Label();
            this.LowerMassLimitLabel = new System.Windows.Forms.Label();
            this.UpperMassLimitLabel = new System.Windows.Forms.Label();
            this.CovarABLabel = new System.Windows.Forms.Label();
            this.CovarACLabel = new System.Windows.Forms.Label();
            this.CovarADLabel = new System.Windows.Forms.Label();
            this.CovarBCLabel = new System.Windows.Forms.Label();
            this.CovarBDLabel = new System.Windows.Forms.Label();
            this.CovarCDLabel = new System.Windows.Forms.Label();
            this.SigmaXLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CurveTypeComboBox
            // 
            this.CurveTypeComboBox.FormattingEnabled = true;
            this.CurveTypeComboBox.Location = new System.Drawing.Point(74, 15);
            this.CurveTypeComboBox.Name = "CurveTypeComboBox";
            this.CurveTypeComboBox.Size = new System.Drawing.Size(223, 21);
            this.CurveTypeComboBox.TabIndex = 0;
            this.CurveTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.CurveTypeComboBox_SelectedIndexChanged);
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(44, 88);
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.TabIndex = 1;
            this.ATextBox.Leave += new System.EventHandler(this.ATextBox_Leave);
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(44, 114);
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.TabIndex = 2;
            this.BTextBox.Leave += new System.EventHandler(this.BTextBox_Leave);
            // 
            // CTextBox
            // 
            this.CTextBox.Location = new System.Drawing.Point(44, 140);
            this.CTextBox.Name = "CTextBox";
            this.CTextBox.Size = new System.Drawing.Size(100, 20);
            this.CTextBox.TabIndex = 3;
            this.CTextBox.Leave += new System.EventHandler(this.CTextBox_Leave);
            // 
            // DTextBox
            // 
            this.DTextBox.Location = new System.Drawing.Point(44, 166);
            this.DTextBox.Name = "DTextBox";
            this.DTextBox.Size = new System.Drawing.Size(100, 20);
            this.DTextBox.TabIndex = 4;
            this.DTextBox.Leave += new System.EventHandler(this.DTextBox_Leave);
            // 
            // VarATextBox
            // 
            this.VarianceATextBox.Location = new System.Drawing.Point(254, 88);
            this.VarianceATextBox.Name = "VarATextBox";
            this.VarianceATextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceATextBox.TabIndex = 5;
            this.VarianceATextBox.Leave += new System.EventHandler(this.VarianceATextBox_Leave);
            // 
            // VarBTextBox
            // 
            this.VarianceBTextBox.Location = new System.Drawing.Point(254, 114);
            this.VarianceBTextBox.Name = "VarBTextBox";
            this.VarianceBTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceBTextBox.TabIndex = 6;
            this.VarianceBTextBox.Leave += new System.EventHandler(this.VarianceBTextBox_Leave);
            // 
            // VarCTextBox
            // 
            this.VarianceCTextBox.Location = new System.Drawing.Point(254, 140);
            this.VarianceCTextBox.Name = "VarCTextBox";
            this.VarianceCTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceCTextBox.TabIndex = 7;
            this.VarianceCTextBox.Leave += new System.EventHandler(this.VarianceCTextBox_Leave);
            // 
            // VarDTextBox
            // 
            this.VarianceDTextBox.Location = new System.Drawing.Point(254, 166);
            this.VarianceDTextBox.Name = "VarDTextBox";
            this.VarianceDTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceDTextBox.TabIndex = 8;
            this.VarianceDTextBox.Leave += new System.EventHandler(this.VarianceDTextBox_Leave);
            // 
            // LowerMassLimitTextBox
            // 
            this.LowerMassLimitTextBox.Location = new System.Drawing.Point(470, 15);
            this.LowerMassLimitTextBox.Name = "LowerMassLimitTextBox";
            this.LowerMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.LowerMassLimitTextBox.TabIndex = 9;
            this.LowerMassLimitTextBox.Leave += new System.EventHandler(this.LowerMassLimitTextBox_Leave);
            // 
            // UpperMassLimitTextBox
            // 
            this.UpperMassLimitTextBox.Location = new System.Drawing.Point(470, 41);
            this.UpperMassLimitTextBox.Name = "UpperMassLimitTextBox";
            this.UpperMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.UpperMassLimitTextBox.TabIndex = 10;
            this.UpperMassLimitTextBox.Leave += new System.EventHandler(this.UpperMassLimitTextBox_Leave);
            // 
            // CovarABTextBox
            // 
            this.CovarianceABTextBox.Location = new System.Drawing.Point(470, 88);
            this.CovarianceABTextBox.Name = "CovarABTextBox";
            this.CovarianceABTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceABTextBox.TabIndex = 11;
            this.CovarianceABTextBox.Leave += new System.EventHandler(this.CovarianceABTextBox_Leave);
            // 
            // CovarACTextBox
            // 
            this.CovarianceACTextBox.Location = new System.Drawing.Point(470, 114);
            this.CovarianceACTextBox.Name = "CovarACTextBox";
            this.CovarianceACTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceACTextBox.TabIndex = 12;
            this.CovarianceACTextBox.Leave += new System.EventHandler(this.CovarianceACTextBox_Leave);
            // 
            // CovarADTextBox
            // 
            this.CovarianceADTextBox.Location = new System.Drawing.Point(470, 140);
            this.CovarianceADTextBox.Name = "CovarADTextBox";
            this.CovarianceADTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceADTextBox.TabIndex = 13;
            this.CovarianceADTextBox.Leave += new System.EventHandler(this.CovarianceADTextBox_Leave);
            // 
            // CovarBCTextBox
            // 
            this.CovarianceBCTextBox.Location = new System.Drawing.Point(470, 166);
            this.CovarianceBCTextBox.Name = "CovarBCTextBox";
            this.CovarianceBCTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBCTextBox.TabIndex = 14;
            this.CovarianceBCTextBox.Leave += new System.EventHandler(this.CovarianceBCTextBox_Leave);
            // 
            // CobarBDTextBox
            // 
            this.CovarianceBDTextBox.Location = new System.Drawing.Point(470, 192);
            this.CovarianceBDTextBox.Name = "CobarBDTextBox";
            this.CovarianceBDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBDTextBox.TabIndex = 15;
            this.CovarianceBDTextBox.Leave += new System.EventHandler(this.CovarianceBDTextBox_Leave);
            // 
            // SigmaXTextBox
            // 
            this.SigmaXTextBox.Location = new System.Drawing.Point(470, 244);
            this.SigmaXTextBox.Name = "SigmaXTextBox";
            this.SigmaXTextBox.Size = new System.Drawing.Size(100, 20);
            this.SigmaXTextBox.TabIndex = 17;
            this.SigmaXTextBox.Leave += new System.EventHandler(this.SigmaXTextBox_Leave);
            // 
            // CovarCDTextBox
            // 
            this.CovarianceCDTextBox.Location = new System.Drawing.Point(470, 218);
            this.CovarianceCDTextBox.Name = "CovarCDTextBox";
            this.CovarianceCDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceCDTextBox.TabIndex = 19;
            this.CovarianceCDTextBox.Leave += new System.EventHandler(this.CovarianceCDTextBox_Leave);
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(601, 13);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 23);
            this.NextBtn.TabIndex = 20;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(601, 42);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 21;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(601, 71);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 22;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CurveTypeLabel
            // 
            this.CurveTypeLabel.AutoSize = true;
            this.CurveTypeLabel.Location = new System.Drawing.Point(12, 18);
            this.CurveTypeLabel.Name = "CurveTypeLabel";
            this.CurveTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.CurveTypeLabel.TabIndex = 23;
            this.CurveTypeLabel.Text = "Curve type";
            // 
            // ALabel
            // 
            this.ALabel.AutoSize = true;
            this.ALabel.Location = new System.Drawing.Point(25, 91);
            this.ALabel.Name = "ALabel";
            this.ALabel.Size = new System.Drawing.Size(13, 13);
            this.ALabel.TabIndex = 24;
            this.ALabel.Text = "a";
            // 
            // BLabel
            // 
            this.BLabel.AutoSize = true;
            this.BLabel.Location = new System.Drawing.Point(25, 117);
            this.BLabel.Name = "BLabel";
            this.BLabel.Size = new System.Drawing.Size(13, 13);
            this.BLabel.TabIndex = 25;
            this.BLabel.Text = "b";
            // 
            // CLabel
            // 
            this.CLabel.AutoSize = true;
            this.CLabel.Location = new System.Drawing.Point(25, 143);
            this.CLabel.Name = "CLabel";
            this.CLabel.Size = new System.Drawing.Size(13, 13);
            this.CLabel.TabIndex = 26;
            this.CLabel.Text = "c";
            // 
            // DLabel
            // 
            this.DLabel.AutoSize = true;
            this.DLabel.Location = new System.Drawing.Point(25, 169);
            this.DLabel.Name = "DLabel";
            this.DLabel.Size = new System.Drawing.Size(13, 13);
            this.DLabel.TabIndex = 27;
            this.DLabel.Text = "d";
            // 
            // VarALabel
            // 
            this.VarALabel.AutoSize = true;
            this.VarALabel.Location = new System.Drawing.Point(190, 91);
            this.VarALabel.Name = "VarALabel";
            this.VarALabel.Size = new System.Drawing.Size(58, 13);
            this.VarALabel.TabIndex = 28;
            this.VarALabel.Text = "Variance a";
            // 
            // VarBLabel
            // 
            this.VarBLabel.AutoSize = true;
            this.VarBLabel.Location = new System.Drawing.Point(190, 117);
            this.VarBLabel.Name = "VarBLabel";
            this.VarBLabel.Size = new System.Drawing.Size(58, 13);
            this.VarBLabel.TabIndex = 29;
            this.VarBLabel.Text = "Variance b";
            // 
            // VarClabel
            // 
            this.VarClabel.AutoSize = true;
            this.VarClabel.Location = new System.Drawing.Point(190, 143);
            this.VarClabel.Name = "VarClabel";
            this.VarClabel.Size = new System.Drawing.Size(58, 13);
            this.VarClabel.TabIndex = 30;
            this.VarClabel.Text = "Variance c";
            // 
            // VarDLabel
            // 
            this.VarDLabel.AutoSize = true;
            this.VarDLabel.Location = new System.Drawing.Point(190, 169);
            this.VarDLabel.Name = "VarDLabel";
            this.VarDLabel.Size = new System.Drawing.Size(58, 13);
            this.VarDLabel.TabIndex = 31;
            this.VarDLabel.Text = "Variance d";
            // 
            // LowerMassLimitLabel
            // 
            this.LowerMassLimitLabel.AutoSize = true;
            this.LowerMassLimitLabel.Location = new System.Drawing.Point(337, 18);
            this.LowerMassLimitLabel.Name = "LowerMassLimitLabel";
            this.LowerMassLimitLabel.Size = new System.Drawing.Size(127, 13);
            this.LowerMassLimitLabel.TabIndex = 32;
            this.LowerMassLimitLabel.Text = "Lower U235 mass limit (g)";
            // 
            // UpperMassLimitLabel
            // 
            this.UpperMassLimitLabel.AutoSize = true;
            this.UpperMassLimitLabel.Location = new System.Drawing.Point(337, 44);
            this.UpperMassLimitLabel.Name = "UpperMassLimitLabel";
            this.UpperMassLimitLabel.Size = new System.Drawing.Size(127, 13);
            this.UpperMassLimitLabel.TabIndex = 33;
            this.UpperMassLimitLabel.Text = "Upper U235 mass limit (g)";
            // 
            // CovarABLabel
            // 
            this.CovarABLabel.AutoSize = true;
            this.CovarABLabel.Location = new System.Drawing.Point(388, 91);
            this.CovarABLabel.Name = "CovarABLabel";
            this.CovarABLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarABLabel.TabIndex = 34;
            this.CovarABLabel.Text = "Covariance ab";
            // 
            // CovarACLabel
            // 
            this.CovarACLabel.AutoSize = true;
            this.CovarACLabel.Location = new System.Drawing.Point(388, 117);
            this.CovarACLabel.Name = "CovarACLabel";
            this.CovarACLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarACLabel.TabIndex = 35;
            this.CovarACLabel.Text = "Covariance ac";
            // 
            // CovarADLabel
            // 
            this.CovarADLabel.AutoSize = true;
            this.CovarADLabel.Location = new System.Drawing.Point(388, 143);
            this.CovarADLabel.Name = "CovarADLabel";
            this.CovarADLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarADLabel.TabIndex = 36;
            this.CovarADLabel.Text = "Covariance ad";
            // 
            // CovarBCLabel
            // 
            this.CovarBCLabel.AutoSize = true;
            this.CovarBCLabel.Location = new System.Drawing.Point(388, 169);
            this.CovarBCLabel.Name = "CovarBCLabel";
            this.CovarBCLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarBCLabel.TabIndex = 37;
            this.CovarBCLabel.Text = "Covariance bc";
            // 
            // CovarBDLabel
            // 
            this.CovarBDLabel.AutoSize = true;
            this.CovarBDLabel.Location = new System.Drawing.Point(388, 195);
            this.CovarBDLabel.Name = "CovarBDLabel";
            this.CovarBDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarBDLabel.TabIndex = 38;
            this.CovarBDLabel.Text = "Covariance bd";
            // 
            // CovarCDLabel
            // 
            this.CovarCDLabel.AutoSize = true;
            this.CovarCDLabel.Location = new System.Drawing.Point(388, 221);
            this.CovarCDLabel.Name = "CovarCDLabel";
            this.CovarCDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarCDLabel.TabIndex = 39;
            this.CovarCDLabel.Text = "Covariance cd";
            // 
            // SigmaXLabel
            // 
            this.SigmaXLabel.AutoSize = true;
            this.SigmaXLabel.Location = new System.Drawing.Point(403, 247);
            this.SigmaXLabel.Name = "SigmaXLabel";
            this.SigmaXLabel.Size = new System.Drawing.Size(61, 13);
            this.SigmaXLabel.TabIndex = 40;
            this.SigmaXLabel.Text = "Sigma x (%)";
            // 
            // IDDCollarCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 279);
            this.Controls.Add(this.SigmaXLabel);
            this.Controls.Add(this.CovarCDLabel);
            this.Controls.Add(this.CovarBDLabel);
            this.Controls.Add(this.CovarBCLabel);
            this.Controls.Add(this.CovarADLabel);
            this.Controls.Add(this.CovarACLabel);
            this.Controls.Add(this.CovarABLabel);
            this.Controls.Add(this.UpperMassLimitLabel);
            this.Controls.Add(this.LowerMassLimitLabel);
            this.Controls.Add(this.VarDLabel);
            this.Controls.Add(this.VarClabel);
            this.Controls.Add(this.VarBLabel);
            this.Controls.Add(this.VarALabel);
            this.Controls.Add(this.DLabel);
            this.Controls.Add(this.CLabel);
            this.Controls.Add(this.BLabel);
            this.Controls.Add(this.ALabel);
            this.Controls.Add(this.CurveTypeLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.CovarianceCDTextBox);
            this.Controls.Add(this.SigmaXTextBox);
            this.Controls.Add(this.CovarianceBDTextBox);
            this.Controls.Add(this.CovarianceBCTextBox);
            this.Controls.Add(this.CovarianceADTextBox);
            this.Controls.Add(this.CovarianceACTextBox);
            this.Controls.Add(this.CovarianceABTextBox);
            this.Controls.Add(this.UpperMassLimitTextBox);
            this.Controls.Add(this.LowerMassLimitTextBox);
            this.Controls.Add(this.VarianceDTextBox);
            this.Controls.Add(this.VarianceCTextBox);
            this.Controls.Add(this.VarianceBTextBox);
            this.Controls.Add(this.VarianceATextBox);
            this.Controls.Add(this.DTextBox);
            this.Controls.Add(this.CTextBox);
            this.Controls.Add(this.BTextBox);
            this.Controls.Add(this.ATextBox);
            this.Controls.Add(this.CurveTypeComboBox);
            this.Name = "IDDCollarCal";
            this.Text = "Collar Calibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CurveTypeComboBox;
        private System.Windows.Forms.TextBox ATextBox;
        private System.Windows.Forms.TextBox BTextBox;
        private System.Windows.Forms.TextBox CTextBox;
        private System.Windows.Forms.TextBox DTextBox;
        private System.Windows.Forms.TextBox VarianceATextBox;
        private System.Windows.Forms.TextBox VarianceBTextBox;
        private System.Windows.Forms.TextBox VarianceCTextBox;
        private System.Windows.Forms.TextBox VarianceDTextBox;
        private System.Windows.Forms.TextBox LowerMassLimitTextBox;
        private System.Windows.Forms.TextBox UpperMassLimitTextBox;
        private System.Windows.Forms.TextBox CovarianceABTextBox;
        private System.Windows.Forms.TextBox CovarianceACTextBox;
        private System.Windows.Forms.TextBox CovarianceADTextBox;
        private System.Windows.Forms.TextBox CovarianceBCTextBox;
        private System.Windows.Forms.TextBox CovarianceBDTextBox;
        private System.Windows.Forms.TextBox SigmaXTextBox;
        private System.Windows.Forms.TextBox CovarianceCDTextBox;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label CurveTypeLabel;
        private System.Windows.Forms.Label ALabel;
        private System.Windows.Forms.Label BLabel;
        private System.Windows.Forms.Label CLabel;
        private System.Windows.Forms.Label DLabel;
        private System.Windows.Forms.Label VarALabel;
        private System.Windows.Forms.Label VarBLabel;
        private System.Windows.Forms.Label VarClabel;
        private System.Windows.Forms.Label VarDLabel;
        private System.Windows.Forms.Label LowerMassLimitLabel;
        private System.Windows.Forms.Label UpperMassLimitLabel;
        private System.Windows.Forms.Label CovarABLabel;
        private System.Windows.Forms.Label CovarACLabel;
        private System.Windows.Forms.Label CovarADLabel;
        private System.Windows.Forms.Label CovarBCLabel;
        private System.Windows.Forms.Label CovarBDLabel;
        private System.Windows.Forms.Label CovarCDLabel;
        private System.Windows.Forms.Label SigmaXLabel;
    }
}