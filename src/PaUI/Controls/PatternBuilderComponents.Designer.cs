using SilUtils.Controls;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatternBuilderComponents));
			this.tabPatternBlding = new System.Windows.Forms.TabControl();
			this.tpgCons = new System.Windows.Forms.TabPage();
			this.pnlConsonants = new SilUtils.Controls.SilPanel();
			this.tpgVows = new System.Windows.Forms.TabPage();
			this.pnlVowels = new SilUtils.Controls.SilPanel();
			this.tpgOther = new System.Windows.Forms.TabPage();
			this.charExplorer = new SIL.Pa.UI.Controls.IPACharacterExplorer();
			this.tpgClasses = new System.Windows.Forms.TabPage();
			this.lvClasses = new SIL.Pa.UI.Controls.ClassListView();
			this.tpgAFeatures = new System.Windows.Forms.TabPage();
			this.tpgBFeatures = new System.Windows.Forms.TabPage();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
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
			resources.ApplyResources(this.tabPatternBlding, "tabPatternBlding");
			this.tabPatternBlding.HotTrack = true;
			this.tabPatternBlding.Name = "tabPatternBlding";
			this.tabPatternBlding.SelectedIndex = 0;
			this.tabPatternBlding.ClientSizeChanged += new System.EventHandler(this.tabPatternBlding_ClientSizeChanged);
			// 
			// tpgCons
			// 
			this.tpgCons.Controls.Add(this.pnlConsonants);
			this.locExtender.SetLocalizableToolTip(this.tpgCons, null);
			this.locExtender.SetLocalizationComment(this.tpgCons, "Text for consonant tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgCons, "PatternBuilderComponents.tpgCons");
			resources.ApplyResources(this.tpgCons, "tpgCons");
			this.tpgCons.Name = "tpgCons";
			this.tpgCons.UseVisualStyleBackColor = true;
			// 
			// pnlConsonants
			// 
			resources.ApplyResources(this.pnlConsonants, "pnlConsonants");
			this.pnlConsonants.BackColor = System.Drawing.SystemColors.Window;
			this.pnlConsonants.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlConsonants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlConsonants.ClipTextForChildControls = true;
			this.pnlConsonants.ControlReceivingFocusOnMnemonic = null;
			this.pnlConsonants.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlConsonants, null);
			this.locExtender.SetLocalizationComment(this.pnlConsonants, null);
			this.locExtender.SetLocalizationPriority(this.pnlConsonants, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlConsonants, "PatternBuilderComponents.pnlConsonants");
			this.pnlConsonants.MnemonicGeneratesClick = false;
			this.pnlConsonants.Name = "pnlConsonants";
			this.pnlConsonants.PaintExplorerBarBackground = false;
			// 
			// tpgVows
			// 
			this.tpgVows.Controls.Add(this.pnlVowels);
			this.locExtender.SetLocalizableToolTip(this.tpgVows, null);
			this.locExtender.SetLocalizationComment(this.tpgVows, "Text for vowel tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgVows, "PatternBuilderComponents.tpgVows");
			resources.ApplyResources(this.tpgVows, "tpgVows");
			this.tpgVows.Name = "tpgVows";
			this.tpgVows.UseVisualStyleBackColor = true;
			// 
			// pnlVowels
			// 
			resources.ApplyResources(this.pnlVowels, "pnlVowels");
			this.pnlVowels.BackColor = System.Drawing.SystemColors.Window;
			this.pnlVowels.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlVowels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlVowels.ClipTextForChildControls = true;
			this.pnlVowels.ControlReceivingFocusOnMnemonic = null;
			this.pnlVowels.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlVowels, null);
			this.locExtender.SetLocalizationComment(this.pnlVowels, null);
			this.locExtender.SetLocalizationPriority(this.pnlVowels, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlVowels, "PatternBuilderComponents.pnlVowels");
			this.pnlVowels.MnemonicGeneratesClick = false;
			this.pnlVowels.Name = "pnlVowels";
			this.pnlVowels.PaintExplorerBarBackground = false;
			// 
			// tpgOther
			// 
			this.tpgOther.Controls.Add(this.charExplorer);
			this.locExtender.SetLocalizableToolTip(this.tpgOther, null);
			this.locExtender.SetLocalizationComment(this.tpgOther, "Text for other tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgOther, "PatternBuilderComponents.tpgOther");
			resources.ApplyResources(this.tpgOther, "tpgOther");
			this.tpgOther.Name = "tpgOther";
			this.tpgOther.UseVisualStyleBackColor = true;
			// 
			// charExplorer
			// 
			resources.ApplyResources(this.charExplorer, "charExplorer");
			this.charExplorer.BackColor = System.Drawing.SystemColors.Window;
			this.charExplorer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.ClipTextForChildControls = true;
			this.charExplorer.ControlReceivingFocusOnMnemonic = null;
			this.charExplorer.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.charExplorer, null);
			this.locExtender.SetLocalizationComment(this.charExplorer, null);
			this.locExtender.SetLocalizationPriority(this.charExplorer, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.charExplorer, "PatternBuilderComponents.charExplorer");
			this.charExplorer.MnemonicGeneratesClick = false;
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			// 
			// tpgClasses
			// 
			this.tpgClasses.Controls.Add(this.lvClasses);
			this.locExtender.SetLocalizableToolTip(this.tpgClasses, null);
			this.locExtender.SetLocalizationComment(this.tpgClasses, "Text for classes tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgClasses, "PatternBuilderComponents.tpgClasses");
			resources.ApplyResources(this.tpgClasses, "tpgClasses");
			this.tpgClasses.Name = "tpgClasses";
			this.tpgClasses.UseVisualStyleBackColor = true;
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.UI.Controls.ClassListView.ListApplicationType.SearchViewWnd;
			resources.ApplyResources(this.lvClasses, "lvClasses");
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.lvClasses, null);
			this.locExtender.SetLocalizationComment(this.lvClasses, null);
			this.locExtender.SetLocalizationPriority(this.lvClasses, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lvClasses, "PatternBuilderComponents.lvClasses");
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = false;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			// 
			// tpgAFeatures
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgAFeatures, null);
			this.locExtender.SetLocalizationComment(this.tpgAFeatures, "Text for articulatory features tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgAFeatures, "PatternBuilderComponents.tpgAFeatures");
			resources.ApplyResources(this.tpgAFeatures, "tpgAFeatures");
			this.tpgAFeatures.Name = "tpgAFeatures";
			this.tpgAFeatures.UseVisualStyleBackColor = true;
			// 
			// tpgBFeatures
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgBFeatures, null);
			this.locExtender.SetLocalizationComment(this.tpgBFeatures, "Text for binary features tab on side panel in search and XY chart views.");
			this.locExtender.SetLocalizingId(this.tpgBFeatures, "PatternBuilderComponents.tpgBFeatures");
			resources.ApplyResources(this.tpgBFeatures, "tpgBFeatures");
			this.tpgBFeatures.Name = "tpgBFeatures";
			this.tpgBFeatures.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "User Interface Controls";
			// 
			// PatternBuilderComponents
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabPatternBlding);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "PatternBuilderComponents.PatternBuilderComponents");
			this.Name = "PatternBuilderComponents";
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
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}
