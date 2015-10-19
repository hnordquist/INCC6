namespace NewUI
{
    partial class IDDGloveboxEdit
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
            this.GloveboxIdLabel = new System.Windows.Forms.Label();
            this.NumRowsLabel = new System.Windows.Forms.Label();
            this.NumColsLabel = new System.Windows.Forms.Label();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.GloveboxIdComboBox = new System.Windows.Forms.ComboBox();
            this.NumRowsTextBox = new System.Windows.Forms.TextBox();
            this.NumColsTextBox = new System.Windows.Forms.TextBox();
            this.DistanceTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GloveboxIdLabel
            // 
            this.GloveboxIdLabel.AutoSize = true;
            this.GloveboxIdLabel.Location = new System.Drawing.Point(28, 25);
            this.GloveboxIdLabel.Name = "GloveboxIdLabel";
            this.GloveboxIdLabel.Size = new System.Drawing.Size(63, 13);
            this.GloveboxIdLabel.TabIndex = 0;
            this.GloveboxIdLabel.Text = "Glovebox id";
            // 
            // NumRowsLabel
            // 
            this.NumRowsLabel.AutoSize = true;
            this.NumRowsLabel.Location = new System.Drawing.Point(130, 108);
            this.NumRowsLabel.Name = "NumRowsLabel";
            this.NumRowsLabel.Size = new System.Drawing.Size(81, 13);
            this.NumRowsLabel.TabIndex = 1;
            this.NumRowsLabel.Text = "Number of rows";
            // 
            // NumColsLabel
            // 
            this.NumColsLabel.AutoSize = true;
            this.NumColsLabel.Location = new System.Drawing.Point(113, 134);
            this.NumColsLabel.Name = "NumColsLabel";
            this.NumColsLabel.Size = new System.Drawing.Size(98, 13);
            this.NumColsLabel.TabIndex = 2;
            this.NumColsLabel.Text = "Number of columns";
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.AutoSize = true;
            this.DistanceLabel.Location = new System.Drawing.Point(16, 160);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(195, 13);
            this.DistanceLabel.TabIndex = 3;
            this.DistanceLabel.Text = "Distance from detector to glovebox (cm)";
            // 
            // GloveboxIdComboBox
            // 
            this.GloveboxIdComboBox.FormattingEnabled = true;
            this.GloveboxIdComboBox.Location = new System.Drawing.Point(97, 22);
            this.GloveboxIdComboBox.Name = "GloveboxIdComboBox";
            this.GloveboxIdComboBox.Size = new System.Drawing.Size(188, 21);
            this.GloveboxIdComboBox.TabIndex = 4;
            this.GloveboxIdComboBox.SelectedIndexChanged += new System.EventHandler(this.GloveboxIdComboBox_SelectedIndexChanged);
            // 
            // NumRowsTextBox
            // 
            this.NumRowsTextBox.Location = new System.Drawing.Point(217, 105);
            this.NumRowsTextBox.Name = "NumRowsTextBox";
            this.NumRowsTextBox.Size = new System.Drawing.Size(68, 20);
            this.NumRowsTextBox.TabIndex = 5;
            this.NumRowsTextBox.TextChanged += new System.EventHandler(this.NumRowsTextBox_TextChanged);
            // 
            // NumColsTextBox
            // 
            this.NumColsTextBox.Location = new System.Drawing.Point(217, 131);
            this.NumColsTextBox.Name = "NumColsTextBox";
            this.NumColsTextBox.Size = new System.Drawing.Size(68, 20);
            this.NumColsTextBox.TabIndex = 6;
            this.NumColsTextBox.TextChanged += new System.EventHandler(this.NumColsTextBox_TextChanged);
            // 
            // DistanceTextBox
            // 
            this.DistanceTextBox.Location = new System.Drawing.Point(217, 157);
            this.DistanceTextBox.Name = "DistanceTextBox";
            this.DistanceTextBox.Size = new System.Drawing.Size(68, 20);
            this.DistanceTextBox.TabIndex = 7;
            this.DistanceTextBox.TextChanged += new System.EventHandler(this.DistanceTextBox_TextChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(326, 20);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(326, 49);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(326, 78);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 10;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDGloveboxEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 194);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DistanceTextBox);
            this.Controls.Add(this.NumColsTextBox);
            this.Controls.Add(this.NumRowsTextBox);
            this.Controls.Add(this.GloveboxIdComboBox);
            this.Controls.Add(this.DistanceLabel);
            this.Controls.Add(this.NumColsLabel);
            this.Controls.Add(this.NumRowsLabel);
            this.Controls.Add(this.GloveboxIdLabel);
            this.Name = "IDDGloveboxEdit";
            this.Text = "Edit An Existing Glovebox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label GloveboxIdLabel;
        private System.Windows.Forms.Label NumRowsLabel;
        private System.Windows.Forms.Label NumColsLabel;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.ComboBox GloveboxIdComboBox;
        private System.Windows.Forms.TextBox NumRowsTextBox;
        private System.Windows.Forms.TextBox NumColsTextBox;
        private System.Windows.Forms.TextBox DistanceTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}