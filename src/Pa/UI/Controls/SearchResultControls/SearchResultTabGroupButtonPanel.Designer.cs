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
			this.components = new System.ComponentModel.Container();
			this.m_toolstrip = new System.Windows.Forms.ToolStrip();
			this.m_btnAddTab = new System.Windows.Forms.ToolStripSplitButton();
			this.m_mnuAddInSideBySideGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.m_mnuAddInStackedGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.m_btnScrollRight = new System.Windows.Forms.ToolStripButton();
			this.m_btnScrollLeft = new System.Windows.Forms.ToolStripButton();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.m_toolstrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// m_toolstrip
			// 
			this.m_toolstrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_toolstrip.AutoSize = false;
			this.m_toolstrip.BackColor = System.Drawing.Color.Transparent;
			this.m_toolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.m_toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.m_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_btnAddTab,
            this.m_btnScrollRight,
            this.m_btnScrollLeft});
			this.locExtender.SetLocalizableToolTip(this.m_toolstrip, null);
			this.locExtender.SetLocalizationComment(this.m_toolstrip, null);
			this.locExtender.SetLocalizationPriority(this.m_toolstrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_toolstrip, "SearchResultTabGroupButtonPanel.m_toolstrip");
			this.m_toolstrip.Location = new System.Drawing.Point(0, 10);
			this.m_toolstrip.Name = "m_toolstrip";
			this.m_toolstrip.Size = new System.Drawing.Size(194, 25);
			this.m_toolstrip.TabIndex = 1;
			this.m_toolstrip.Text = "toolStrip1";
			// 
			// m_btnAddTab
			// 
			this.m_btnAddTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_btnAddTab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_mnuAddInSideBySideGroup,
            this.m_mnuAddInStackedGroup});
			this.m_btnAddTab.Image = global::SIL.Pa.Properties.Resources.NewTabNormal;
			this.m_btnAddTab.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.locExtender.SetLocalizableToolTip(this.m_btnAddTab, "Add Tab");
			this.locExtender.SetLocalizationComment(this.m_btnAddTab, null);
			this.locExtender.SetLocalizingId(this.m_btnAddTab, "SearchResultTabGroupButtonPanel.btnAddTab");
			this.m_btnAddTab.Margin = new System.Windows.Forms.Padding(3, 0, 0, 1);
			this.m_btnAddTab.Name = "m_btnAddTab";
			this.m_btnAddTab.Size = new System.Drawing.Size(32, 24);
			// 
			// m_mnuAddInSideBySideGroup
			// 
			this.m_mnuAddInSideBySideGroup.Image = global::SIL.Pa.Properties.Resources.AddSideBySideTabGroup;
			this.locExtender.SetLocalizableToolTip(this.m_mnuAddInSideBySideGroup, null);
			this.locExtender.SetLocalizationComment(this.m_mnuAddInSideBySideGroup, null);
			this.locExtender.SetLocalizingId(this.m_mnuAddInSideBySideGroup, "SearchResultTabGroupButtonPanel.mnuAddInSideBySideGroup");
			this.m_mnuAddInSideBySideGroup.Name = "m_mnuAddInSideBySideGroup";
			this.m_mnuAddInSideBySideGroup.Size = new System.Drawing.Size(288, 22);
			this.m_mnuAddInSideBySideGroup.Text = "Add Tab in New Si&de-by-Side Tab Group";
			// 
			// m_mnuAddInStackedGroup
			// 
			this.m_mnuAddInStackedGroup.Image = global::SIL.Pa.Properties.Resources.AddStackedTabGroup;
			this.locExtender.SetLocalizableToolTip(this.m_mnuAddInStackedGroup, null);
			this.locExtender.SetLocalizationComment(this.m_mnuAddInStackedGroup, null);
			this.locExtender.SetLocalizingId(this.m_mnuAddInStackedGroup, "SearchResultTabGroupButtonPanel.mnuAddInStackedGroup");
			this.m_mnuAddInStackedGroup.Name = "m_mnuAddInStackedGroup";
			this.m_mnuAddInStackedGroup.Size = new System.Drawing.Size(288, 22);
			this.m_mnuAddInStackedGroup.Text = "Add Tab in New St&acked Tab Group";
			// 
			// m_btnScrollRight
			// 
			this.m_btnScrollRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.m_btnScrollRight.AutoSize = false;
			this.m_btnScrollRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.locExtender.SetLocalizableToolTip(this.m_btnScrollRight, "Scroll Right");
			this.locExtender.SetLocalizationComment(this.m_btnScrollRight, null);
			this.locExtender.SetLocalizingId(this.m_btnScrollRight, "SearchResultTabGroupButtonPanel.btnRight");
			this.m_btnScrollRight.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.m_btnScrollRight.Name = "m_btnScrollRight";
			this.m_btnScrollRight.Size = new System.Drawing.Size(13, 23);
			this.m_btnScrollRight.Text = "4";
			// 
			// m_btnScrollLeft
			// 
			this.m_btnScrollLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.m_btnScrollLeft.AutoSize = false;
			this.m_btnScrollLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.locExtender.SetLocalizableToolTip(this.m_btnScrollLeft, "Scroll Left");
			this.locExtender.SetLocalizationComment(this.m_btnScrollLeft, null);
			this.locExtender.SetLocalizingId(this.m_btnScrollLeft, "SearchResultTabGroupButtonPanel.btnLeft");
			this.m_btnScrollLeft.Margin = new System.Windows.Forms.Padding(5, 0, 0, 1);
			this.m_btnScrollLeft.Name = "m_btnScrollLeft";
			this.m_btnScrollLeft.Size = new System.Drawing.Size(13, 23);
			this.m_btnScrollLeft.Text = "3";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SearchResultTabGroupButtonPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.m_toolstrip);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SearchResultTabGroupButtonPanel.SearchResultTabGroupButtonPanel");
			this.Name = "SearchResultTabGroupButtonPanel";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.Size = new System.Drawing.Size(194, 36);
			this.m_toolstrip.ResumeLayout(false);
			this.m_toolstrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStrip m_toolstrip;
		private System.Windows.Forms.ToolStripButton m_btnScrollRight;
		private System.Windows.Forms.ToolStripButton m_btnScrollLeft;
		private System.Windows.Forms.ToolStripSplitButton m_btnAddTab;
		private System.Windows.Forms.ToolStripMenuItem m_mnuAddInSideBySideGroup;
		private System.Windows.Forms.ToolStripMenuItem m_mnuAddInStackedGroup;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
