namespace NewUI
{
    partial class IDDK5CollarItemData
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.K5ErrorLabel = new System.Windows.Forms.Label();
            this.K5Label = new System.Windows.Forms.Label();
            this.K5ErrorTextBox = new System.Windows.Forms.TextBox();
            this.K5TextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Use = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CorrType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.K5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.K5Error = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.K5ErrorLabel);
            this.groupBox1.Controls.Add(this.K5Label);
            this.groupBox1.Controls.Add(this.K5ErrorTextBox);
            this.groupBox1.Controls.Add(this.K5TextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "K5 - Total Extra Item Correction Factor";
            // 
            // K5ErrorLabel
            // 
            this.K5ErrorLabel.AutoSize = true;
            this.K5ErrorLabel.Location = new System.Drawing.Point(173, 34);
            this.K5ErrorLabel.Name = "K5ErrorLabel";
            this.K5ErrorLabel.Size = new System.Drawing.Size(44, 13);
            this.K5ErrorLabel.TabIndex = 3;
            this.K5ErrorLabel.Text = "K5 error";
            // 
            // K5Label
            // 
            this.K5Label.AutoSize = true;
            this.K5Label.Location = new System.Drawing.Point(16, 34);
            this.K5Label.Name = "K5Label";
            this.K5Label.Size = new System.Drawing.Size(20, 13);
            this.K5Label.TabIndex = 2;
            this.K5Label.Text = "K5";
            // 
            // K5ErrorTextBox
            // 
            this.K5ErrorTextBox.Location = new System.Drawing.Point(223, 31);
            this.K5ErrorTextBox.Name = "K5ErrorTextBox";
            this.K5ErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.K5ErrorTextBox.TabIndex = 1;
            this.K5ErrorTextBox.TextChanged += new System.EventHandler(this.K5ErrorTextBox_TextChanged);
            // 
            // K5TextBox
            // 
            this.K5TextBox.Location = new System.Drawing.Point(42, 31);
            this.K5TextBox.Name = "K5TextBox";
            this.K5TextBox.Size = new System.Drawing.Size(100, 20);
            this.K5TextBox.TabIndex = 0;
            this.K5TextBox.TextChanged += new System.EventHandler(this.K5TextBox_TextChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(375, 19);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(375, 48);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(375, 77);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Use,
            this.CorrType,
            this.K5,
            this.K5Error});
            this.listView1.Location = new System.Drawing.Point(12, 99);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(314, 590);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Use
            // 
            this.Use.Text = "Use";
            this.Use.Width = 40;
            // 
            // CorrType
            // 
            this.CorrType.Text = "Correction Type";
            this.CorrType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CorrType.Width = 150;
            // 
            // K5
            // 
            this.K5.Text = "K5";
            this.K5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // K5Error
            // 
            this.K5Error.Text = "K5 Error";
            this.K5Error.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IDDK5CollarItemData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 707);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "IDDK5CollarItemData";
            this.Text = "Select K5 Extra Item Correction Factors";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label K5ErrorLabel;
        private System.Windows.Forms.Label K5Label;
        private System.Windows.Forms.TextBox K5ErrorTextBox;
        private System.Windows.Forms.TextBox K5TextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Use;
        private System.Windows.Forms.ColumnHeader CorrType;
        private System.Windows.Forms.ColumnHeader K5;
        private System.Windows.Forms.ColumnHeader K5Error;
    }
}