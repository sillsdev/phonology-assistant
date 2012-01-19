using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	partial class PatternBuilderComponents
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tabPatternBlding = new System.Windows.Forms.TabControl();
			this.tpgCons = new System.Windows.Forms.TabPage();
			this.pnlConsonants = new SilTools.Controls.SilPanel();
			this.tpgVows = new System.Windows.Forms.TabPage();
			this.pnlVowels = new SilTools.Controls.SilPanel();
			this.tpgOther = new System.Windows.Forms.TabPage();
			this.charExplorer = new SIL.Pa.UI.Controls.IPACharacterExplorer();
			this.tpgClasses = new System.Windows.Forms.TabPage();
			this.lvClasses = new SIL.Pa.UI.Controls.ClassListView();
			this.tpgAFeatures = new System.Windows.Forms.TabPage();
			this.tpgBFeatures = new System.Windows.Forms.TabPage();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.tabPatternBlding.SuspendLayout();
			this.tpgCons.SuspendLayout();
			this.tpgVows.SuspendLayout();
			this.tpgOther.SuspendLayout();
			this.tpgClasses.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// tabPatternBlding
			// 
			this.tabPatternBlding.Controls.Add(this.tpgCons);
			this.tabPatternBlding.Controls.Add(this.tpgVows);
			this.tabPatternBlding.Controls.Add(this.tpgOther);
			this.tabPatternBlding.Controls.Add(this.tpgClasses);
			this.tabPatternBlding.Controls.Add(this.tpgAFeatures);
			this.tabPatternBlding.Controls.Add(this.tpgBFeatures);
			this.tabPatternBlding.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPatternBlding.HotTrack = true;
			this.tabPatternBlding.Location = new System.Drawing.Point(0, 0);
			this.tabPatternBlding.Name = "tabPatternBlding";
			this.tabPatternBlding.SelectedIndex = 0;
			this.tabPatternBlding.ShowToolTips = true;
			this.tabPatternBlding.Size = new System.Drawing.Size(391, 214);
			this.tabPatternBlding.TabIndex = 1;
			this.tabPatternBlding.ClientSizeChanged += new System.EventHandler(this.tabPatternBlding_ClientSizeChanged);
			// 
			// tpgCons
			// 
			this.tpgCons.Controls.Add(this.pnlConsonants);
			this.locExtender.SetLocalizableToolTip(this.tpgCons, null);
			this.locExtender.SetLocalizationComment(this.tpgCons, "Text for consonant tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgCons, "CommonControls.PatternBuilderComponents.ConsonantTabText");
			this.tpgCons.Location = new System.Drawing.Point(4, 22);
			this.tpgCons.Name = "tpgCons";
			this.tpgCons.Padding = new System.Windows.Forms.Padding(5);
			this.tpgCons.Size = new System.Drawing.Size(383, 188);
			this.tpgCons.TabIndex = 1;
			this.tpgCons.Text = "Con.";
			this.tpgCons.ToolTipText = "Consonants";
			this.tpgCons.UseVisualStyleBackColor = true;
			// 
			// pnlConsonants
			// 
			this.pnlConsonants.AutoScroll = true;
			this.pnlConsonants.BackColor = System.Drawing.SystemColors.Window;
			this.pnlConsonants.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlConsonants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlConsonants.ClipTextForChildControls = true;
			this.pnlConsonants.ControlReceivingFocusOnMnemonic = null;
			this.pnlConsonants.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlConsonants.DoubleBuffered = false;
			this.pnlConsonants.DrawOnlyBottomBorder = false;
			this.pnlConsonants.DrawOnlyTopBorder = false;
			this.pnlConsonants.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlConsonants.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlConsonants, null);
			this.locExtender.SetLocalizationComment(this.pnlConsonants, null);
			this.locExtender.SetLocalizationPriority(this.pnlConsonants, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlConsonants, "PatternBuilderComponents.pnlConsonants");
			this.pnlConsonants.Location = new System.Drawing.Point(5, 5);
			this.pnlConsonants.MnemonicGeneratesClick = false;
			this.pnlConsonants.Name = "pnlConsonants";
			this.pnlConsonants.PaintExplorerBarBackground = false;
			this.pnlConsonants.Size = new System.Drawing.Size(373, 178);
			this.pnlConsonants.TabIndex = 0;
			// 
			// tpgVows
			// 
			this.tpgVows.Controls.Add(this.pnlVowels);
			this.locExtender.SetLocalizableToolTip(this.tpgVows, null);
			this.locExtender.SetLocalizationComment(this.tpgVows, "Text for vowel tab on side panel in search and distribution chart views.");
			this.locExtender.SetLocalizingId(this.tpgVows, "CommonControls.PatternBuilderComponents.VowelTabText");
			this.tpgVows.Location = new System.Drawing.Point(4, 22);
			this.tpgVows.Name = "tpgVows";
			this.tpgVows.Padding = new System.Windows.Forms.Padding(3);
			this.tpgVows.Size = new System.Drawing.Size(208, 188);
			this.tpgVows.TabIndex = 4;
			this.tpgVows.Text = "Vow.";
			this.tpgVows.ToolTipText = "Vowels";
			this.tpgVows.UseVisualStyleBackColor = true;
			// 
			// pnlVowels
			// 
			this.pnlVowels.AutoScroll = true;
			this.pnlVowels.BackColor = System.Drawing.SystemColors.Window;
			this.pnlVowels.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlVowels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlVowels.ClipTextForChildControls = true;
			this.pnlVowels.ControlReceivingFocusOnMnemonic = null;
			this.pnlVowels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlVowels.DoubleBuffered = false;
			this.pnlVowels.DrawOnlyBottomBorder = false;
			this.pnlVowels.DrawOnlyTopBorder = false;
			this.pnlVowels.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlVowels.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlVowels, null);
			this.locExtender.SetLocalizationComment(this.pnlVowels, null);
			this.locExtender.SetLocalizationPriority(this.pnlVowels, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlVowels, "PatternBuilderComponents.pnlVowels");
			this.pnlVowels.Location = new System.Drawing.Point(3, 3);
			this.pnlVowels.MnemonicGeneratesClick = false;
			this.pnlVowels.Name = "pnlVowels";
			this.pnlVowels.PaintExplorerBarBackground = false;
			this.pnlVowels.Size = new System.Drawing.Size(202, 182);
			this.pnlVowels.TabIndex = 2;
			// 
			// tpgOther
			// 
			this.tpgOther.Controls.Add(this.charExplorer);
			this.locExtender.SetLocalizableToolTip(this.tpgOther, null);
			this.locExtender.SetLocalizationComment(this.tpgOther, "Text for other tab on side panel in search and distribution chart views.");
			this.locExtender.SetLocalizingId(this.tpgOther, "CommonControls.PatternBuilderComponents.OtherSymbolsTabText");
			this.tpgOther.Location = new System.Drawing.Point(4, 22);
			this.tpgOther.Name = "tpgOther";
			this.tpgOther.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOther.Size = new System.Drawing.Size(208, 188);
			this.tpgOther.TabIndex = 5;
			this.tpgOther.Text = "Other";
			this.tpgOther.ToolTipText = "Suprasegmentals and diacritics";
			this.tpgOther.UseVisualStyleBackColor = true;
			// 
			// charExplorer
			// 
			this.charExplorer.AutoScroll = true;
			this.charExplorer.BackColor = System.Drawing.SystemColors.Window;
			this.charExplorer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.ClipTextForChildControls = true;
			this.charExplorer.ControlReceivingFocusOnMnemonic = null;
			this.charExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.charExplorer.DoubleBuffered = false;
			this.charExplorer.DrawOnlyBottomBorder = false;
			this.charExplorer.DrawOnlyTopBorder = false;
			this.charExplorer.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.charExplorer.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.charExplorer, null);
			this.locExtender.SetLocalizationComment(this.charExplorer, null);
			this.locExtender.SetLocalizationPriority(this.charExplorer, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.charExplorer, "PatternBuilderComponents.charExplorer");
			this.charExplorer.Location = new System.Drawing.Point(3, 3);
			this.charExplorer.MnemonicGeneratesClick = false;
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			this.charExplorer.Size = new System.Drawing.Size(202, 182);
			this.charExplorer.TabIndex = 0;
			// 
			// tpgClasses
			// 
			this.tpgClasses.Controls.Add(this.lvClasses);
			this.locExtender.SetLocalizableToolTip(this.tpgClasses, null);
			this.locExtender.SetLocalizationComment(this.tpgClasses, "Text for classes tab on side panel in search and distribution chart views.");
			this.locExtender.SetLocalizingId(this.tpgClasses, "CommonControls.PatternBuilderComponents.ClassesTabText");
			this.tpgClasses.Location = new System.Drawing.Point(4, 22);
			this.tpgClasses.Name = "tpgClasses";
			this.tpgClasses.Padding = new System.Windows.Forms.Padding(5);
			this.tpgClasses.Size = new System.Drawing.Size(208, 188);
			this.tpgClasses.TabIndex = 0;
			this.tpgClasses.Text = "Classes";
			this.tpgClasses.UseVisualStyleBackColor = true;
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.SearchViewWnd;
			this.lvClasses.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvClasses.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.lvClasses, null);
			this.locExtender.SetLocalizationComment(this.lvClasses, null);
			this.locExtender.SetLocalizationPriority(this.lvClasses, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lvClasses, "PatternBuilderComponents.lvClasses");
			this.lvClasses.Location = new System.Drawing.Point(5, 5);
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = false;
			this.lvClasses.Size = new System.Drawing.Size(198, 178);
			this.lvClasses.TabIndex = 0;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			// 
			// tpgAFeatures
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgAFeatures, null);
			this.locExtender.SetLocalizationComment(this.tpgAFeatures, null);
			this.locExtender.SetLocalizingId(this.tpgAFeatures, "CommonControls.PatternBuilderComponents.DescriptiveFeaturesTabText");
			this.tpgAFeatures.Location = new System.Drawing.Point(4, 22);
			this.tpgAFeatures.Name = "tpgAFeatures";
			this.tpgAFeatures.Padding = new System.Windows.Forms.Padding(5);
			this.tpgAFeatures.Size = new System.Drawing.Size(383, 188);
			this.tpgAFeatures.TabIndex = 2;
			this.tpgAFeatures.Text = "Descriptive Features";
			this.tpgAFeatures.ToolTipText = "Descriptive Features";
			this.tpgAFeatures.UseVisualStyleBackColor = true;
			// 
			// tpgBFeatures
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgBFeatures, null);
			this.locExtender.SetLocalizationComment(this.tpgBFeatures, "");
			this.locExtender.SetLocalizingId(this.tpgBFeatures, "CommonControls.PatternBuilderComponents.DistinctiveFeaturesTabText");
			this.tpgBFeatures.Location = new System.Drawing.Point(4, 22);
			this.tpgBFeatures.Name = "tpgBFeatures";
			this.tpgBFeatures.Padding = new System.Windows.Forms.Padding(5);
			this.tpgBFeatures.Size = new System.Drawing.Size(383, 188);
			this.tpgBFeatures.TabIndex = 3;
			this.tpgBFeatures.Text = "Distinctive Features";
			this.tpgBFeatures.ToolTipText = "Distinctive Features";
			this.tpgBFeatures.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// PatternBuilderComponents
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabPatternBlding);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "PatternBuilderComponents.PatternBuilderComponents");
			this.Name = "PatternBuilderComponents";
			this.Size = new System.Drawing.Size(391, 214);
			this.tabPatternBlding.ResumeLayout(false);
			this.tpgCons.ResumeLayout(false);
			this.tpgVows.ResumeLayout(false);
			this.tpgOther.ResumeLayout(false);
			this.tpgClasses.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabPatternBlding;
		private System.Windows.Forms.TabPage tpgCons;
		private SilPanel pnlConsonants;
		private System.Windows.Forms.TabPage tpgVows;
		private SilPanel pnlVowels;
		private System.Windows.Forms.TabPage tpgOther;
		private IPACharacterExplorer charExplorer;
		private System.Windows.Forms.TabPage tpgClasses;
		private ClassListView lvClasses;
		private System.Windows.Forms.TabPage tpgAFeatures;
		private System.Windows.Forms.TabPage tpgBFeatures;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
