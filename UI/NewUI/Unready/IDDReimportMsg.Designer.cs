namespace NewUI
{
    partial class IDDReimportMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDReimportMsg));
            this.LongAssLabel = new System.Windows.Forms.Label();
            this.DoNotDisplayAgainCheckBox = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LongAssLabel
            // 
            this.LongAssLabel.AutoSize = true;
            this.LongAssLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LongAssLabel.Location = new System.Drawing.Point(12, 9);
            this.LongAssLabel.MaximumSize = new System.Drawing.Size(450, 0);
            this.LongAssLabel.Name = "LongAssLabel";
            this.LongAssLabel.Size = new System.Drawing.Size(448, 119);
            this.LongAssLabel.TabIndex = 0;
            this.LongAssLabel.Text = resources.GetString("LongAssLabel.Text");
            // 
            // DoNotDisplayAgainCheckBox
            // 
            this.DoNotDisplayAgainCheckBox.AutoSize = true;
            this.DoNotDisplayAgainCheckBox.Location = new System.Drawing.Point(15, 145);
            this.DoNotDisplayAgainCheckBox.Name = "DoNotDisplayAgainCheckBox";
            this.DoNotDisplayAgainCheckBox.Size = new System.Drawing.Size(186, 17);
            this.DoNotDisplayAgainCheckBox.TabIndex = 1;
            this.DoNotDisplayAgainCheckBox.Text = "Do not display this message again";
            this.DoNotDisplayAgainCheckBox.UseVisualStyleBackColor = true;
            this.DoNotDisplayAgainCheckBox.CheckedChanged += new System.EventHandler(this.DoNotDisplayAgainCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(208, 183);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(51, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // IDDReimportMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 218);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DoNotDisplayAgainCheckBox);
            this.Controls.Add(this.LongAssLabel);
            this.Name = "IDDReimportMsg";
            this.Text = "Re-import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LongAssLabel;
        private System.Windows.Forms.CheckBox DoNotDisplayAgainCheckBox;
        private System.Windows.Forms.Button OKBtn;
    }
}