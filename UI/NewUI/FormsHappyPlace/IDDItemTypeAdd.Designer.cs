namespace NewUI
{
    partial class IDDItemTypeAdd
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
            this.CurrentMaterialTypesLabel = new System.Windows.Forms.Label();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.CurrentMaterialTypesComboBox = new System.Windows.Forms.ComboBox();
            this.MaterialTypeTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentMaterialTypesLabel
            // 
            this.CurrentMaterialTypesLabel.AutoSize = true;
            this.CurrentMaterialTypesLabel.Location = new System.Drawing.Point(15, 15);
            this.CurrentMaterialTypesLabel.Name = "CurrentMaterialTypesLabel";
            this.CurrentMaterialTypesLabel.Size = new System.Drawing.Size(108, 13);
            this.CurrentMaterialTypesLabel.TabIndex = 0;
            this.CurrentMaterialTypesLabel.Text = "Current material types";
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(56, 73);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 1;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // CurrentMaterialTypesComboBox
            // 
            this.CurrentMaterialTypesComboBox.FormattingEnabled = true;
            this.CurrentMaterialTypesComboBox.Location = new System.Drawing.Point(129, 12);
            this.CurrentMaterialTypesComboBox.Name = "CurrentMaterialTypesComboBox";
            this.CurrentMaterialTypesComboBox.Size = new System.Drawing.Size(121, 21);
            this.CurrentMaterialTypesComboBox.TabIndex = 2;
            // 
            // MaterialTypeTextBox
            // 
            this.MaterialTypeTextBox.Location = new System.Drawing.Point(129, 70);
            this.MaterialTypeTextBox.Name = "MaterialTypeTextBox";
            this.MaterialTypeTextBox.Size = new System.Drawing.Size(121, 20);
            this.MaterialTypeTextBox.TabIndex = 3;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(282, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(282, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(282, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 6;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDItemTypeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 109);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MaterialTypeTextBox);
            this.Controls.Add(this.CurrentMaterialTypesComboBox);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.CurrentMaterialTypesLabel);
            this.Name = "IDDItemTypeAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add a New Material Type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentMaterialTypesLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.ComboBox CurrentMaterialTypesComboBox;
        private System.Windows.Forms.TextBox MaterialTypeTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}