namespace NewUI
{
    partial class IDDManualDataEntry
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
            this.CountTimeTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.CountTimeLabel = new System.Windows.Forms.Label();
            this.MeasurementDateLabel = new System.Windows.Forms.Label();
            this.MeausurementDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.UseCurrent = new System.Windows.Forms.Button();
            this.cyclesGridView = new System.Windows.Forms.DataGridView();
            this.Cycle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Accidentals = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CountTimeTextBox
            // 
            this.CountTimeTextBox.Location = new System.Drawing.Point(103, 19);
            this.CountTimeTextBox.Name = "CountTimeTextBox";
            this.CountTimeTextBox.Size = new System.Drawing.Size(53, 20);
            this.CountTimeTextBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.CountTimeTextBox, "The length of time in seconds for each cycle in this measurement.");
            this.CountTimeTextBox.Leave += new System.EventHandler(this.CountTimeTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(410, 129);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(410, 158);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(410, 187);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(410, 216);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(75, 23);
            this.PrintBtn.TabIndex = 6;
            this.PrintBtn.Text = "Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // CountTimeLabel
            // 
            this.CountTimeLabel.AutoSize = true;
            this.CountTimeLabel.Location = new System.Drawing.Point(9, 22);
            this.CountTimeLabel.Name = "CountTimeLabel";
            this.CountTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.CountTimeLabel.TabIndex = 7;
            this.CountTimeLabel.Text = "Count time (secs)";
            // 
            // MeasurementDateLabel
            // 
            this.MeasurementDateLabel.AutoSize = true;
            this.MeasurementDateLabel.Location = new System.Drawing.Point(180, 22);
            this.MeasurementDateLabel.Name = "MeasurementDateLabel";
            this.MeasurementDateLabel.Size = new System.Drawing.Size(95, 13);
            this.MeasurementDateLabel.TabIndex = 8;
            this.MeasurementDateLabel.Text = "Measurement date";
            // 
            // MeausurementDateTimePicker
            // 
            this.MeausurementDateTimePicker.Location = new System.Drawing.Point(285, 19);
            this.MeausurementDateTimePicker.Name = "MeausurementDateTimePicker";
            this.MeausurementDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.MeausurementDateTimePicker.TabIndex = 9;
            this.toolTip.SetToolTip(this.MeausurementDateTimePicker, "Enter the date to be used for this measurement. The measurement time will always " +
        "be the current day\'s time.");
            this.MeausurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeausurementDateTimePicker_ValueChanged);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(410, 52);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 11;
            this.ClearBtn.Text = "Clear data";
            this.toolTip.SetToolTip(this.ClearBtn, "Click on this button to reset all the manually entered data values to zero.");
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // UseCurrent
            // 
            this.UseCurrent.Location = new System.Drawing.Point(410, 82);
            this.UseCurrent.Name = "UseCurrent";
            this.UseCurrent.Size = new System.Drawing.Size(75, 23);
            this.UseCurrent.TabIndex = 13;
            this.UseCurrent.Text = "Use current";
            this.toolTip.SetToolTip(this.UseCurrent, "//todo: (since Acquire OK resets measurement) Load current cycles from current me" +
        "asurement");
            this.UseCurrent.UseVisualStyleBackColor = true;
            this.UseCurrent.Click += new System.EventHandler(this.UseCurrent_Click);
            // 
            // cyclesGridView
            // 
            this.cyclesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cyclesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Cycle,
            this.Column2,
            this.RealAcc,
            this.Accidentals});
            this.cyclesGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.cyclesGridView.Location = new System.Drawing.Point(3, 52);
            this.cyclesGridView.Name = "cyclesGridView";
            this.cyclesGridView.Size = new System.Drawing.Size(401, 436);
            this.cyclesGridView.TabIndex = 12;
            this.cyclesGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.cyclesGridView_CellEndEdit);
            this.cyclesGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.cyclesGridView_CellValidating_1);
            this.cyclesGridView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cyclesGridView_KeyPress);
            // 
            // Cycle
            // 
            this.Cycle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Cycle.HeaderText = "Cycle";
            this.Cycle.Name = "Cycle";
            this.Cycle.ReadOnly = true;
            this.Cycle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cycle.Width = 39;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Totals";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RealAcc
            // 
            this.RealAcc.HeaderText = "Reals + Accidentals";
            this.RealAcc.Name = "RealAcc";
            this.RealAcc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Accidentals
            // 
            this.Accidentals.HeaderText = "Accidentals";
            this.Accidentals.Name = "Accidentals";
            this.Accidentals.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IDDManualDataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 500);
            this.Controls.Add(this.UseCurrent);
            this.Controls.Add(this.cyclesGridView);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.MeausurementDateTimePicker);
            this.Controls.Add(this.MeasurementDateLabel);
            this.Controls.Add(this.CountTimeLabel);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CountTimeTextBox);
            this.Name = "IDDManualDataEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Raw Data";
            ((System.ComponentModel.ISupportInitialize)(this.cyclesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CountTimeTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.Label MeasurementDateLabel;
        private System.Windows.Forms.DateTimePicker MeausurementDateTimePicker;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridView cyclesGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cycle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Accidentals;
        private System.Windows.Forms.Button UseCurrent;
    }
}