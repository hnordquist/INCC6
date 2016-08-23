namespace NewUI
{
    partial class IDDCurveType
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDCurveType));
			this.CurveTypeLabel = new System.Windows.Forms.Label();
			this.CurveTypeComboBox = new System.Windows.Forms.ComboBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// CurveTypeLabel
			// 
			this.CurveTypeLabel.AutoSize = true;
			this.CurveTypeLabel.Location = new System.Drawing.Point(28, 27);
			this.CurveTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.CurveTypeLabel.Name = "CurveTypeLabel";
			this.CurveTypeLabel.Size = new System.Drawing.Size(76, 17);
			this.CurveTypeLabel.TabIndex = 0;
			this.CurveTypeLabel.Text = "Curve type";
			// 
			// CurveTypeComboBox
			// 
			this.CurveTypeComboBox.FormattingEnabled = true;
			this.CurveTypeComboBox.Location = new System.Drawing.Point(113, 23);
			this.CurveTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.CurveTypeComboBox.Name = "CurveTypeComboBox";
			this.CurveTypeComboBox.Size = new System.Drawing.Size(316, 24);
			this.CurveTypeComboBox.TabIndex = 1;
			this.toolTip1.SetToolTip(this.CurveTypeComboBox, "Select the curve type");
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(463, 21);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 2;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(463, 57);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 3;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(463, 92);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 4;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(113, 89);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(130, 42);
			this.button1.TabIndex = 5;
			this.button1.Text = "Get curve-fitting results";
			this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// IDDCurveType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(587, 143);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.CurveTypeComboBox);
			this.Controls.Add(this.CurveTypeLabel);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IDDCurveType";
			this.Text = "Select Curve Type";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurveTypeLabel;
        private System.Windows.Forms.ComboBox CurveTypeComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}