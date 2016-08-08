namespace NewUI
{
    partial class IDDActiveMultCal
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
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.Thermal1stMomentTextBox = new System.Windows.Forms.TextBox();
            this.Thermal2ndMomentTextBox = new System.Windows.Forms.TextBox();
            this.Thermal3rdMomentTextBox = new System.Windows.Forms.TextBox();
            this.Fast2ndMomentTextBox = new System.Windows.Forms.TextBox();
            this.Fast3rdMomentTextBox = new System.Windows.Forms.TextBox();
            this.Fast1stMomentTextBox = new System.Windows.Forms.TextBox();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.Thermal1stMomentLabel = new System.Windows.Forms.Label();
            this.Thermal2ndMomentLabel = new System.Windows.Forms.Label();
            this.Thermal3rdMomentLabel = new System.Windows.Forms.Label();
            this.Fast1stMomentLabel = new System.Windows.Forms.Label();
            this.Fast2ndMomentLabel = new System.Windows.Forms.Label();
            this.Fast3rdMomentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(83, 12);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(186, 21);
            this.MaterialTypeComboBox.TabIndex = 0;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(302, 10);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(159, 23);
            this.PrintBtn.TabIndex = 1;
            this.PrintBtn.Text = "Print calibration parameters";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(386, 53);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(386, 82);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(386, 111);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // Thermal1stMomentTextBox
            // 
            this.Thermal1stMomentTextBox.Location = new System.Drawing.Point(338, 156);
            this.Thermal1stMomentTextBox.Name = "Thermal1stMomentTextBox";
            this.Thermal1stMomentTextBox.Size = new System.Drawing.Size(135, 20);
            this.Thermal1stMomentTextBox.TabIndex = 5;
            this.Thermal1stMomentTextBox.Leave += new System.EventHandler(this.Thermal1stMomentTextBox_Leave);
            // 
            // Thermal2ndMomentTextBox
            // 
            this.Thermal2ndMomentTextBox.Location = new System.Drawing.Point(338, 182);
            this.Thermal2ndMomentTextBox.Name = "Thermal2ndMomentTextBox";
            this.Thermal2ndMomentTextBox.Size = new System.Drawing.Size(135, 20);
            this.Thermal2ndMomentTextBox.TabIndex = 6;
            this.Thermal2ndMomentTextBox.Leave += new System.EventHandler(this.Thermal2ndMomentTextBox_Leave);
            // 
            // Thermal3rdMomentTextBox
            // 
            this.Thermal3rdMomentTextBox.Location = new System.Drawing.Point(338, 208);
            this.Thermal3rdMomentTextBox.Name = "Thermal3rdMomentTextBox";
            this.Thermal3rdMomentTextBox.Size = new System.Drawing.Size(135, 20);
            this.Thermal3rdMomentTextBox.TabIndex = 7;
            this.Thermal3rdMomentTextBox.Leave += new System.EventHandler(this.Thermal3rdMomentTextBox_Leave);
            // 
            // Fast2ndMomentTextBox
            // 
            this.Fast2ndMomentTextBox.Location = new System.Drawing.Point(338, 260);
            this.Fast2ndMomentTextBox.Name = "Fast2ndMomentTextBox";
            this.Fast2ndMomentTextBox.Size = new System.Drawing.Size(135, 20);
            this.Fast2ndMomentTextBox.TabIndex = 9;
            this.Fast2ndMomentTextBox.Leave += new System.EventHandler(this.Fast2ndMomentTextBox_Leave);
            // 
            // Fast3rdMomentTextBox
            // 
            this.Fast3rdMomentTextBox.Location = new System.Drawing.Point(338, 286);
            this.Fast3rdMomentTextBox.Name = "Fast3rdMomentTextBox";
            this.Fast3rdMomentTextBox.Size = new System.Drawing.Size(135, 20);
            this.Fast3rdMomentTextBox.TabIndex = 10;
            this.Fast3rdMomentTextBox.Leave += new System.EventHandler(this.Fast3rdMomentTextBox_Leave);
            // 
            // Fast1stMomentTextBox
            // 
            this.Fast1stMomentTextBox.Location = new System.Drawing.Point(338, 234);
            this.Fast1stMomentTextBox.Name = "Fast1stMomentTextBox";
            this.Fast1stMomentTextBox.Size = new System.Drawing.Size(135, 20);
            this.Fast1stMomentTextBox.TabIndex = 11;
            this.Fast1stMomentTextBox.Leave += new System.EventHandler(this.Fast1stMomentTextBox_Leave);
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(10, 15);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 12;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // Thermal1stMomentLabel
            // 
            this.Thermal1stMomentLabel.AutoSize = true;
            this.Thermal1stMomentLabel.Location = new System.Drawing.Point(17, 159);
            this.Thermal1stMomentLabel.Name = "Thermal1stMomentLabel";
            this.Thermal1stMomentLabel.Size = new System.Drawing.Size(303, 13);
            this.Thermal1stMomentLabel.TabIndex = 13;
            this.Thermal1stMomentLabel.Text = "1st factorial moment of thermal neutron induced fission of U235";
            // 
            // Thermal2ndMomentLabel
            // 
            this.Thermal2ndMomentLabel.AutoSize = true;
            this.Thermal2ndMomentLabel.Location = new System.Drawing.Point(13, 185);
            this.Thermal2ndMomentLabel.Name = "Thermal2ndMomentLabel";
            this.Thermal2ndMomentLabel.Size = new System.Drawing.Size(307, 13);
            this.Thermal2ndMomentLabel.TabIndex = 14;
            this.Thermal2ndMomentLabel.Text = "2nd factorial moment of thermal neutron induced fission of U235";
            // 
            // Thermal3rdMomentLabel
            // 
            this.Thermal3rdMomentLabel.AutoSize = true;
            this.Thermal3rdMomentLabel.Location = new System.Drawing.Point(16, 211);
            this.Thermal3rdMomentLabel.Name = "Thermal3rdMomentLabel";
            this.Thermal3rdMomentLabel.Size = new System.Drawing.Size(304, 13);
            this.Thermal3rdMomentLabel.TabIndex = 15;
            this.Thermal3rdMomentLabel.Text = "3rd factorial moment of thermal neutron induced fission of U235";
            // 
            // Fast1stMomentLabel
            // 
            this.Fast1stMomentLabel.AutoSize = true;
            this.Fast1stMomentLabel.Location = new System.Drawing.Point(34, 237);
            this.Fast1stMomentLabel.Name = "Fast1stMomentLabel";
            this.Fast1stMomentLabel.Size = new System.Drawing.Size(286, 13);
            this.Fast1stMomentLabel.TabIndex = 16;
            this.Fast1stMomentLabel.Text = "1st factorial moment of fast neutron induced fission of U235";
            // 
            // Fast2ndMomentLabel
            // 
            this.Fast2ndMomentLabel.AutoSize = true;
            this.Fast2ndMomentLabel.Location = new System.Drawing.Point(30, 263);
            this.Fast2ndMomentLabel.Name = "Fast2ndMomentLabel";
            this.Fast2ndMomentLabel.Size = new System.Drawing.Size(290, 13);
            this.Fast2ndMomentLabel.TabIndex = 17;
            this.Fast2ndMomentLabel.Text = "2nd factorial moment of fast neutron induced fission of U235";
            // 
            // Fast3rdMomentLabel
            // 
            this.Fast3rdMomentLabel.AutoSize = true;
            this.Fast3rdMomentLabel.Location = new System.Drawing.Point(33, 289);
            this.Fast3rdMomentLabel.Name = "Fast3rdMomentLabel";
            this.Fast3rdMomentLabel.Size = new System.Drawing.Size(287, 13);
            this.Fast3rdMomentLabel.TabIndex = 18;
            this.Fast3rdMomentLabel.Text = "3rd factorial moment of fast neutron induced fission of U235";
            // 
            // IDDActiveMultCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 320);
            this.Controls.Add(this.Fast3rdMomentLabel);
            this.Controls.Add(this.Fast2ndMomentLabel);
            this.Controls.Add(this.Fast1stMomentLabel);
            this.Controls.Add(this.Thermal3rdMomentLabel);
            this.Controls.Add(this.Thermal2ndMomentLabel);
            this.Controls.Add(this.Thermal1stMomentLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.Fast1stMomentTextBox);
            this.Controls.Add(this.Fast3rdMomentTextBox);
            this.Controls.Add(this.Fast2ndMomentTextBox);
            this.Controls.Add(this.Thermal3rdMomentTextBox);
            this.Controls.Add(this.Thermal2ndMomentTextBox);
            this.Controls.Add(this.Thermal1stMomentTextBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Name = "IDDActiveMultCal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Active Multiplicity Calibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.TextBox Thermal1stMomentTextBox;
        private System.Windows.Forms.TextBox Thermal2ndMomentTextBox;
        private System.Windows.Forms.TextBox Thermal3rdMomentTextBox;
        private System.Windows.Forms.TextBox Fast2ndMomentTextBox;
        private System.Windows.Forms.TextBox Fast3rdMomentTextBox;
        private System.Windows.Forms.TextBox Fast1stMomentTextBox;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label Thermal1stMomentLabel;
        private System.Windows.Forms.Label Thermal2ndMomentLabel;
        private System.Windows.Forms.Label Thermal3rdMomentLabel;
        private System.Windows.Forms.Label Fast1stMomentLabel;
        private System.Windows.Forms.Label Fast2ndMomentLabel;
        private System.Windows.Forms.Label Fast3rdMomentLabel;
    }
}