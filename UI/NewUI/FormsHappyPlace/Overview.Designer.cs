namespace NewUI
{
	partial class Overview
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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Acquisition State");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Detectors");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("QC and Tests");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Items");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Materials");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Isotopics");
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Strata");
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Collar Items");
			System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Composite Isotopics");
			System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Facilities");
			System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("MBAs");
			System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Methods");
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.detCtx = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.FacMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MeasParamMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LMConnMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DetsCtx = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.dFac = new System.Windows.Forms.ToolStripMenuItem();
			this.setAsCurrentDetectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.detCtx.SuspendLayout();
			this.DetsCtx.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.treeView1.Name = "treeView1";
			treeNode1.Name = "Acquisition State";
			treeNode1.Text = "Acquisition State";
			treeNode2.Name = "Detectors";
			treeNode2.Text = "Detectors";
			treeNode3.Name = "QC and Tests";
			treeNode3.Text = "QC and Tests";
			treeNode4.Name = "Items";
			treeNode4.Text = "Items";
			treeNode5.Name = "Materials";
			treeNode5.Text = "Materials";
			treeNode6.Name = "Isotopics";
			treeNode6.Text = "Isotopics";
			treeNode7.Name = "Strata";
			treeNode7.Text = "Strata";
			treeNode8.Name = "Collar Items";
			treeNode8.Text = "Collar Items";
			treeNode9.Name = "Composite Isotopics";
			treeNode9.Text = "Composite Isotopics";
			treeNode10.Name = "Facilities";
			treeNode10.Text = "Facilities";
			treeNode11.Name = "MBAs";
			treeNode11.Text = "MBAs";
			treeNode12.Name = "Methods";
			treeNode12.Text = "Methods";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
			this.treeView1.Size = new System.Drawing.Size(162, 377);
			this.treeView1.TabIndex = 0;
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
			this.splitContainer1.Size = new System.Drawing.Size(379, 377);
			this.splitContainer1.SplitterDistance = 162;
			this.splitContainer1.TabIndex = 1;
			// 
			// richTextBox1
			// 
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(213, 377);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			// 
			// detCtx
			// 
			this.detCtx.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.detCtx.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FacMenuItem,
            this.MeasParamMenuItem,
            this.LMConnMenuItem});
			this.detCtx.Name = "detCtx";
			this.detCtx.Size = new System.Drawing.Size(396, 100);
			// 
			// FacMenuItem
			// 
			this.FacMenuItem.Name = "FacMenuItem";
			this.FacMenuItem.Size = new System.Drawing.Size(395, 32);
			this.FacMenuItem.Text = "Facility/Inspection...";
			this.FacMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// MeasParamMenuItem
			// 
			this.MeasParamMenuItem.Name = "MeasParamMenuItem";
			this.MeasParamMenuItem.Size = new System.Drawing.Size(395, 32);
			this.MeasParamMenuItem.Text = "Measurement Parameters...";
			this.MeasParamMenuItem.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
			// 
			// LMConnMenuItem
			// 
			this.LMConnMenuItem.Name = "LMConnMenuItem";
			this.LMConnMenuItem.Size = new System.Drawing.Size(395, 32);
			this.LMConnMenuItem.Text = "List Mode Connection Parameters...";
			this.LMConnMenuItem.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
			// 
			// DetsCtx
			// 
			this.DetsCtx.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.DetsCtx.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dFac,
            this.setAsCurrentDetectorToolStripMenuItem});
			this.DetsCtx.Name = "detCtx";
			this.DetsCtx.Size = new System.Drawing.Size(293, 68);
			this.DetsCtx.Opening += new System.ComponentModel.CancelEventHandler(this.DetsCtx_Opening);
			// 
			// dFac
			// 
			this.dFac.Name = "dFac";
			this.dFac.Size = new System.Drawing.Size(292, 32);
			this.dFac.Text = "Facility/Inspection...";
			this.dFac.Click += new System.EventHandler(this.dFac_Click);
			// 
			// setAsCurrentDetectorToolStripMenuItem
			// 
			this.setAsCurrentDetectorToolStripMenuItem.Name = "setAsCurrentDetectorToolStripMenuItem";
			this.setAsCurrentDetectorToolStripMenuItem.Size = new System.Drawing.Size(292, 32);
			this.setAsCurrentDetectorToolStripMenuItem.Text = "Set as Current Detector";
			this.setAsCurrentDetectorToolStripMenuItem.Click += new System.EventHandler(this.SetDet_Click);
			// 
			// Overview
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(379, 377);
			this.Controls.Add(this.splitContainer1);
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "Overview";
			this.Text = "Cascade";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.detCtx.ResumeLayout(false);
			this.DetsCtx.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ContextMenuStrip detCtx;
		private System.Windows.Forms.ToolStripMenuItem FacMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MeasParamMenuItem;
		private System.Windows.Forms.ToolStripMenuItem LMConnMenuItem;
		private System.Windows.Forms.ContextMenuStrip DetsCtx;
		private System.Windows.Forms.ToolStripMenuItem dFac;
		private System.Windows.Forms.ToolStripMenuItem setAsCurrentDetectorToolStripMenuItem;
	}
}