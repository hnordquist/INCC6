namespace NewUI
{
    partial class IDDGloveboxAdd
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CurrentGloveboxIdsLabel = new System.Windows.Forms.Label();
            this.GloveboxIdLabel = new System.Windows.Forms.Label();
            this.NumRowsLabel = new System.Windows.Forms.Label();
            this.NumColsLabel = new System.Windows.Forms.Label();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.CurrentGloveboxIdsComboBox = new System.Windows.Forms.ComboBox();
            this.GloveboxIdTextBox = new System.Windows.Forms.TextBox();
            this.NumRowsTextBox = new System.Windows.Forms.TextBox();
            this.NumColsTextBox = new System.Windows.Forms.TextBox();
            this.DistanceTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(485, 19);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(485, 48);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(485, 77);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CurrentGloveboxIdsLabel
            // 
            this.CurrentGloveboxIdsLabel.AutoSize = true;
            this.CurrentGloveboxIdsLabel.Location = new System.Drawing.Point(102, 24);
            this.CurrentGloveboxIdsLabel.Name = "CurrentGloveboxIdsLabel";
            this.CurrentGloveboxIdsLabel.Size = new System.Drawing.Size(103, 13);
            this.CurrentGloveboxIdsLabel.TabIndex = 3;
            this.CurrentGloveboxIdsLabel.Text = "Current glovebox ids";
            // 
            // GloveboxIdLabel
            // 
            this.GloveboxIdLabel.AutoSize = true;
            this.GloveboxIdLabel.Location = new System.Drawing.Point(142, 65);
            this.GloveboxIdLabel.Name = "GloveboxIdLabel";
            this.GloveboxIdLabel.Size = new System.Drawing.Size(63, 13);
            this.GloveboxIdLabel.TabIndex = 4;
            this.GloveboxIdLabel.Text = "Glovebox id";
            // 
            // NumRowsLabel
            // 
            this.NumRowsLabel.AutoSize = true;
            this.NumRowsLabel.Location = new System.Drawing.Point(124, 91);
            this.NumRowsLabel.Name = "NumRowsLabel";
            this.NumRowsLabel.Size = new System.Drawing.Size(81, 13);
            this.NumRowsLabel.TabIndex = 5;
            this.NumRowsLabel.Text = "Number of rows";
            // 
            // NumColsLabel
            // 
            this.NumColsLabel.AutoSize = true;
            this.NumColsLabel.Location = new System.Drawing.Point(107, 117);
            this.NumColsLabel.Name = "NumColsLabel";
            this.NumColsLabel.Size = new System.Drawing.Size(98, 13);
            this.NumColsLabel.TabIndex = 6;
            this.NumColsLabel.Text = "Number of columns";
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.AutoSize = true;
            this.DistanceLabel.Location = new System.Drawing.Point(10, 143);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(195, 13);
            this.DistanceLabel.TabIndex = 7;
            this.DistanceLabel.Text = "Distance from detector to glovebox (cm)";
            // 
            // CurrentGloveboxIdsComboBox
            // 
            this.CurrentGloveboxIdsComboBox.FormattingEnabled = true;
            this.CurrentGloveboxIdsComboBox.Location = new System.Drawing.Point(211, 21);
            this.CurrentGloveboxIdsComboBox.Name = "CurrentGloveboxIdsComboBox";
            this.CurrentGloveboxIdsComboBox.Size = new System.Drawing.Size(232, 21);
            this.CurrentGloveboxIdsComboBox.TabIndex = 8;
            this.CurrentGloveboxIdsComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentGloveboxIdsComboBox_SelectedIndexChanged);
            // 
            // GloveboxIdTextBox
            // 
            this.GloveboxIdTextBox.Location = new System.Drawing.Point(211, 62);
            this.GloveboxIdTextBox.Name = "GloveboxIdTextBox";
            this.GloveboxIdTextBox.Size = new System.Drawing.Size(232, 20);
            this.GloveboxIdTextBox.TabIndex = 9;
            this.GloveboxIdTextBox.Leave += new System.EventHandler(this.GloveboxIdTextBox_Leave);
            // 
            // NumRowsTextBox
            // 
            this.NumRowsTextBox.Location = new System.Drawing.Point(211, 88);
            this.NumRowsTextBox.Name = "NumRowsTextBox";
            this.NumRowsTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumRowsTextBox.TabIndex = 10;
            this.NumRowsTextBox.Leave += new System.EventHandler(this.NumRowsTextBox_Leave);
            // 
            // NumColsTextBox
            // 
            this.NumColsTextBox.Location = new System.Drawing.Point(211, 114);
            this.NumColsTextBox.Name = "NumColsTextBox";
            this.NumColsTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumColsTextBox.TabIndex = 11;
            this.NumColsTextBox.Leave += new System.EventHandler(this.NumColsTextBox_Leave);
            // 
            // DistanceTextBox
            // 
            this.DistanceTextBox.Location = new System.Drawing.Point(211, 140);
            this.DistanceTextBox.Name = "DistanceTextBox";
            this.DistanceTextBox.Size = new System.Drawing.Size(100, 20);
            this.DistanceTextBox.TabIndex = 12;
            this.DistanceTextBox.Leave += new System.EventHandler(this.DistanceTextBox_Leave);
            // 
            // IDDGloveboxAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 176);
            this.Controls.Add(this.DistanceTextBox);
            this.Controls.Add(this.NumColsTextBox);
            this.Controls.Add(this.NumRowsTextBox);
            this.Controls.Add(this.GloveboxIdTextBox);
            this.Controls.Add(this.CurrentGloveboxIdsComboBox);
            this.Controls.Add(this.DistanceLabel);
            this.Controls.Add(this.NumColsLabel);
            this.Controls.Add(this.NumRowsLabel);
            this.Controls.Add(this.GloveboxIdLabel);
            this.Controls.Add(this.CurrentGloveboxIdsLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDGloveboxAdd";
            this.Text = "Add a New Glovebox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label CurrentGloveboxIdsLabel;
        private System.Windows.Forms.Label GloveboxIdLabel;
        private System.Windows.Forms.Label NumRowsLabel;
        private System.Windows.Forms.Label NumColsLabel;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.ComboBox CurrentGloveboxIdsComboBox;
        private System.Windows.Forms.TextBox GloveboxIdTextBox;
        private System.Windows.Forms.TextBox NumRowsTextBox;
        private System.Windows.Forms.TextBox NumColsTextBox;
        private System.Windows.Forms.TextBox DistanceTextBox;
    }
}