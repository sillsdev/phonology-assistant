namespace SIL.Pa.UI.Controls
{
	partial class PatternBuilderBar
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
			this.m_menuStrip = new System.Windows.Forms.MenuStrip();
			this.m_mnuPhones = new System.Windows.Forms.ToolStripMenuItem();
			this.m_mnuSpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.cAnyConsonantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vAnyVowelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.zeroOrMorePhonesOrDiacriticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oneOrMorePhonesOrDiacriticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_mnuClasses = new System.Windows.Forms.ToolStripMenuItem();
			this.m_mnuFeatures = new System.Windows.Forms.ToolStripMenuItem();
			this.spaceWordBoundaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.diacriticPlaceholderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.aNDGroupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.oRGroupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_menuStrip
			// 
			this.m_menuStrip.AutoSize = false;
			this.m_menuStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_mnuPhones,
            this.m_mnuClasses,
            this.m_mnuFeatures,
            this.m_mnuSpecial});
			this.m_menuStrip.Location = new System.Drawing.Point(1, 1);
			this.m_menuStrip.Name = "m_menuStrip";
			this.m_menuStrip.ShowItemToolTips = true;
			this.m_menuStrip.Size = new System.Drawing.Size(413, 22);
			this.m_menuStrip.TabIndex = 1;
			// 
			// m_mnuPhones
			// 
			this.m_mnuPhones.Image = global::SIL.Pa.Properties.Resources.DropDownArrowWithPadding;
			this.m_mnuPhones.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_mnuPhones.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.m_mnuPhones.Name = "m_mnuPhones";
			this.m_mnuPhones.Size = new System.Drawing.Size(124, 18);
			this.m_mnuPhones.Text = "Phones && Symbols";
			this.m_mnuPhones.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			// 
			// m_mnuSpecial
			// 
			this.m_mnuSpecial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cAnyConsonantToolStripMenuItem,
            this.vAnyVowelToolStripMenuItem,
            this.toolStripMenuItem4,
            this.zeroOrMorePhonesOrDiacriticsToolStripMenuItem,
            this.oneOrMorePhonesOrDiacriticsToolStripMenuItem,
            this.spaceWordBoundaryToolStripMenuItem,
            this.diacriticPlaceholderToolStripMenuItem1,
            this.toolStripMenuItem5,
            this.aNDGroupToolStripMenuItem1,
            this.oRGroupToolStripMenuItem1});
			this.m_mnuSpecial.Image = global::SIL.Pa.Properties.Resources.DropDownArrowWithPadding;
			this.m_mnuSpecial.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_mnuSpecial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.m_mnuSpecial.Name = "m_mnuSpecial";
			this.m_mnuSpecial.Size = new System.Drawing.Size(65, 18);
			this.m_mnuSpecial.Text = "Special";
			this.m_mnuSpecial.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			// 
			// cAnyConsonantToolStripMenuItem
			// 
			this.cAnyConsonantToolStripMenuItem.Name = "cAnyConsonantToolStripMenuItem";
			this.cAnyConsonantToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.cAnyConsonantToolStripMenuItem.Text = "[C] Any Consonant";
			// 
			// vAnyVowelToolStripMenuItem
			// 
			this.vAnyVowelToolStripMenuItem.Name = "vAnyVowelToolStripMenuItem";
			this.vAnyVowelToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.vAnyVowelToolStripMenuItem.Text = "[V] Any Vowel";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(252, 6);
			// 
			// zeroOrMorePhonesOrDiacriticsToolStripMenuItem
			// 
			this.zeroOrMorePhonesOrDiacriticsToolStripMenuItem.Name = "zeroOrMorePhonesOrDiacriticsToolStripMenuItem";
			this.zeroOrMorePhonesOrDiacriticsToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.zeroOrMorePhonesOrDiacriticsToolStripMenuItem.Text = "* Zero or More Phones or Diacritics";
			// 
			// oneOrMorePhonesOrDiacriticsToolStripMenuItem
			// 
			this.oneOrMorePhonesOrDiacriticsToolStripMenuItem.Name = "oneOrMorePhonesOrDiacriticsToolStripMenuItem";
			this.oneOrMorePhonesOrDiacriticsToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.oneOrMorePhonesOrDiacriticsToolStripMenuItem.Text = "+ One or More Phones or Diacritics";
			// 
			// m_mnuClasses
			// 
			this.m_mnuClasses.Image = global::SIL.Pa.Properties.Resources.DropDownArrowWithPadding;
			this.m_mnuClasses.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_mnuClasses.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.m_mnuClasses.Name = "m_mnuClasses";
			this.m_mnuClasses.Size = new System.Drawing.Size(66, 18);
			this.m_mnuClasses.Text = "Classes";
			this.m_mnuClasses.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			// 
			// m_mnuFeatures
			// 
			this.m_mnuFeatures.Image = global::SIL.Pa.Properties.Resources.DropDownArrowWithPadding;
			this.m_mnuFeatures.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_mnuFeatures.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.m_mnuFeatures.Name = "m_mnuFeatures";
			this.m_mnuFeatures.Size = new System.Drawing.Size(73, 18);
			this.m_mnuFeatures.Text = "Features";
			this.m_mnuFeatures.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			// 
			// spaceWordBoundaryToolStripMenuItem
			// 
			this.spaceWordBoundaryToolStripMenuItem.Name = "spaceWordBoundaryToolStripMenuItem";
			this.spaceWordBoundaryToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.spaceWordBoundaryToolStripMenuItem.Text = "# Space/Word Boundary";
			// 
			// diacriticPlaceholderToolStripMenuItem1
			// 
			this.diacriticPlaceholderToolStripMenuItem1.Name = "diacriticPlaceholderToolStripMenuItem1";
			this.diacriticPlaceholderToolStripMenuItem1.Size = new System.Drawing.Size(255, 22);
			this.diacriticPlaceholderToolStripMenuItem1.Text = "Diacritic Placeholder";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(252, 6);
			// 
			// aNDGroupToolStripMenuItem1
			// 
			this.aNDGroupToolStripMenuItem1.Name = "aNDGroupToolStripMenuItem1";
			this.aNDGroupToolStripMenuItem1.Size = new System.Drawing.Size(255, 22);
			this.aNDGroupToolStripMenuItem1.Text = "[  ] AND Group";
			// 
			// oRGroupToolStripMenuItem1
			// 
			this.oRGroupToolStripMenuItem1.Name = "oRGroupToolStripMenuItem1";
			this.oRGroupToolStripMenuItem1.Size = new System.Drawing.Size(255, 22);
			this.oRGroupToolStripMenuItem1.Text = "{  } OR Group";
			// 
			// PatternBuilderBar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_menuStrip);
			this.Name = "PatternBuilderBar";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(415, 39);
			this.m_menuStrip.ResumeLayout(false);
			this.m_menuStrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.MenuStrip m_menuStrip;
		private System.Windows.Forms.ToolStripMenuItem m_mnuPhones;
		private System.Windows.Forms.ToolStripMenuItem m_mnuSpecial;
		private System.Windows.Forms.ToolStripMenuItem cAnyConsonantToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem vAnyVowelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem zeroOrMorePhonesOrDiacriticsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem oneOrMorePhonesOrDiacriticsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem m_mnuClasses;
		private System.Windows.Forms.ToolStripMenuItem m_mnuFeatures;
		private System.Windows.Forms.ToolStripMenuItem spaceWordBoundaryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem diacriticPlaceholderToolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem aNDGroupToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem oRGroupToolStripMenuItem1;
	}
}
