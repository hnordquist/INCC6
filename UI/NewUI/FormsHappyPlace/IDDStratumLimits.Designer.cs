namespace NewUI
{
    partial class IDDStratumLimits
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
            this.StrataView = new System.Windows.Forms.DataGridView();
            this.Stratum_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historical_bias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historical_random_uncertainty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historical_sys_uncertainty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StratumId = new System.Windows.Forms.DataGridViewColumn();
            this.HistBias = new System.Windows.Forms.DataGridViewColumn();
            this.RandUnc = new System.Windows.Forms.DataGridViewColumn();
            this.SystUnc = new System.Windows.Forms.DataGridViewColumn();
            ((System.ComponentModel.ISupportInitialize)(this.StrataView)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(640, 15);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(640, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(640, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // StrataView
            // 
            this.StrataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StrataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Stratum_ID,
            this.historical_bias,
            this.historical_random_uncertainty,
            this.historical_sys_uncertainty});
            this.StrataView.Location = new System.Drawing.Point(10, 15);
            this.StrataView.Name = "StrataView";
            this.StrataView.Size = new System.Drawing.Size(614, 238);
            this.StrataView.TabIndex = 4;
            this.StrataView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.StrataView_CellValueChanged);
            // 
            // Stratum_ID
            // 
            this.Stratum_ID.HeaderText = "Stratum ID";
            this.Stratum_ID.Name = "Stratum_ID";
            this.Stratum_ID.ReadOnly = true;
            // 
            // historical_bias
            // 
            this.historical_bias.HeaderText = "Historical Bias Fraction";
            this.historical_bias.Name = "historical_bias";
            this.historical_bias.Width = 160;
            // 
            // historical_random_uncertainty
            // 
            this.historical_random_uncertainty.HeaderText = "Historical Random Uncertainty (fraction)";
            this.historical_random_uncertainty.Name = "historical_random_uncertainty";
            this.historical_random_uncertainty.Width = 160;
            // 
            // historical_sys_uncertainty
            // 
            this.historical_sys_uncertainty.HeaderText = "Historical System Uncertainty (fraction)";
            this.historical_sys_uncertainty.Name = "historical_sys_uncertainty";
            this.historical_sys_uncertainty.Width = 160;
            // 
            // StratumId
            // 
            this.StratumId.Name = "StratumId";
            // 
            // HistBias
            // 
            this.HistBias.Name = "HistBias";
            // 
            // RandUnc
            // 
            this.RandUnc.Name = "RandUnc";
            // 
            // SystUnc
            // 
            this.SystUnc.Name = "SystUnc";
            // 
            // IDDStratumLimits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 264);
            this.Controls.Add(this.StrataView);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDStratumLimits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Historical Stratum Uncertainties";
            ((System.ComponentModel.ISupportInitialize)(this.StrataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.DataGridView StrataView;
        private System.Windows.Forms.DataGridViewColumn StratumId;
        private System.Windows.Forms.DataGridViewColumn HistBias;
        private System.Windows.Forms.DataGridViewColumn RandUnc;
        private System.Windows.Forms.DataGridViewColumn SystUnc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stratum_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn historical_bias;
        private System.Windows.Forms.DataGridViewTextBoxColumn historical_random_uncertainty;
        private System.Windows.Forms.DataGridViewTextBoxColumn historical_sys_uncertainty;
    }
}