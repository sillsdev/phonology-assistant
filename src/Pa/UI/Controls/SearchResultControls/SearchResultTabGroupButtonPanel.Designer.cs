namespace SIL.Pa.UI.Controls
{
	partial class SearchResultTabGroupButtonPanel
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.m_btnAddTab = new System.Windows.Forms.ToolStripSplitButton();
			this.m_mnuAddInSideBySideGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.m_mnuAddInStackedGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.m_btnRight = new System.Windows.Forms.ToolStripButton();
			this.m_btnLeft = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_btnAddTab,
            this.m_btnRight,
            this.m_btnLeft});
			this.toolStrip1.Location = new System.Drawing.Point(0, 10);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(194, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// m_btnAddTab
			// 
			this.m_btnAddTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_btnAddTab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_mnuAddInSideBySideGroup,
            this.m_mnuAddInStackedGroup});
			this.m_btnAddTab.Image = global::SIL.Pa.Properties.Resources.NewTabNormal;
			this.m_btnAddTab.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.m_btnAddTab.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_btnAddTab.Margin = new System.Windows.Forms.Padding(3, 0, 0, 1);
			this.m_btnAddTab.Name = "m_btnAddTab";
			this.m_btnAddTab.Size = new System.Drawing.Size(32, 24);
			// 
			// m_mnuAddInSideBySideGroup
			// 
			this.m_mnuAddInSideBySideGroup.Name = "m_mnuAddInSideBySideGroup";
			this.m_mnuAddInSideBySideGroup.Size = new System.Drawing.Size(288, 22);
			this.m_mnuAddInSideBySideGroup.Text = "Add Tab in New Si&de-by-Side Tab Group";
			// 
			// m_mnuAddInStackedGroup
			// 
			this.m_mnuAddInStackedGroup.Name = "m_mnuAddInStackedGroup";
			this.m_mnuAddInStackedGroup.Size = new System.Drawing.Size(288, 22);
			this.m_mnuAddInStackedGroup.Text = "Add Tab in New St&acked Tab Group";
			// 
			// m_btnRight
			// 
			this.m_btnRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.m_btnRight.AutoSize = false;
			this.m_btnRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.m_btnRight.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_btnRight.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.m_btnRight.Name = "m_btnRight";
			this.m_btnRight.Size = new System.Drawing.Size(13, 23);
			this.m_btnRight.Text = "4";
			// 
			// m_btnLeft
			// 
			this.m_btnLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.m_btnLeft.AutoSize = false;
			this.m_btnLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.m_btnLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_btnLeft.Margin = new System.Windows.Forms.Padding(5, 0, 0, 1);
			this.m_btnLeft.Name = "m_btnLeft";
			this.m_btnLeft.Size = new System.Drawing.Size(13, 23);
			this.m_btnLeft.Text = "3";
			// 
			// SearchResultTabGroupButtonPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.toolStrip1);
			this.Name = "SearchResultTabGroupButtonPanel";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.Size = new System.Drawing.Size(194, 36);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton m_btnRight;
		private System.Windows.Forms.ToolStripButton m_btnLeft;
		private System.Windows.Forms.ToolStripSplitButton m_btnAddTab;
		private System.Windows.Forms.ToolStripMenuItem m_mnuAddInSideBySideGroup;
		private System.Windows.Forms.ToolStripMenuItem m_mnuAddInStackedGroup;
	}
}
