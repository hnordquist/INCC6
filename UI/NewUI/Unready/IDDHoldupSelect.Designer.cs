namespace NewUI
{
    partial class IDDHoldupSelect
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.InstructionsLabel = new System.Windows.Forms.Label();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.MeasureBtn = new System.Windows.Forms.Button();
            this.QuitBtn = new System.Windows.Forms.Button();
            this.AbortBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 50);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(409, 212);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // InstructionsLabel
            // 
            this.InstructionsLabel.AutoSize = true;
            this.InstructionsLabel.Location = new System.Drawing.Point(79, 20);
            this.InstructionsLabel.Name = "InstructionsLabel";
            this.InstructionsLabel.Size = new System.Drawing.Size(275, 13);
            this.InstructionsLabel.TabIndex = 1;
            this.InstructionsLabel.Text = "Click on the glovebox position you want to measure next.";
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.AutoSize = true;
            this.DistanceLabel.Location = new System.Drawing.Point(89, 283);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(265, 13);
            this.DistanceLabel.TabIndex = 2;
            this.DistanceLabel.Text = "Distance from detector to glovebox should be xx.x (cm)";
            // 
            // MeasureBtn
            // 
            this.MeasureBtn.Location = new System.Drawing.Point(113, 330);
            this.MeasureBtn.Name = "MeasureBtn";
            this.MeasureBtn.Size = new System.Drawing.Size(208, 23);
            this.MeasureBtn.TabIndex = 3;
            this.MeasureBtn.Text = "Measure selected glovebox position";
            this.MeasureBtn.UseVisualStyleBackColor = true;
            this.MeasureBtn.Click += new System.EventHandler(this.MeasureBtn_Click);
            // 
            // QuitBtn
            // 
            this.QuitBtn.Location = new System.Drawing.Point(113, 375);
            this.QuitBtn.Name = "QuitBtn";
            this.QuitBtn.Size = new System.Drawing.Size(208, 23);
            this.QuitBtn.TabIndex = 4;
            this.QuitBtn.Text = "Quit measurement with results";
            this.QuitBtn.UseVisualStyleBackColor = true;
            this.QuitBtn.Click += new System.EventHandler(this.QuitBtn_Click);
            // 
            // AbortBtn
            // 
            this.AbortBtn.Location = new System.Drawing.Point(113, 420);
            this.AbortBtn.Name = "AbortBtn";
            this.AbortBtn.Size = new System.Drawing.Size(208, 23);
            this.AbortBtn.TabIndex = 5;
            this.AbortBtn.Text = "Abort measurement";
            this.AbortBtn.UseVisualStyleBackColor = true;
            this.AbortBtn.Click += new System.EventHandler(this.AbortBtn_Click);
            // 
            // IDDHoldupSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 468);
            this.Controls.Add(this.AbortBtn);
            this.Controls.Add(this.QuitBtn);
            this.Controls.Add(this.MeasureBtn);
            this.Controls.Add(this.DistanceLabel);
            this.Controls.Add(this.InstructionsLabel);
            this.Controls.Add(this.listBox1);
            this.Name = "IDDHoldupSelect";
            this.Text = "Select Next Holdup Measurement Position";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label InstructionsLabel;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.Button MeasureBtn;
        private System.Windows.Forms.Button QuitBtn;
        private System.Windows.Forms.Button AbortBtn;
    }
}