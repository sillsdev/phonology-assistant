namespace SIL.Pa.Controls
{
	partial class PatternBuilderComponents
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatternBuilderComponents));
			this.tabPatternBlding = new System.Windows.Forms.TabControl();
			this.tpgCons = new System.Windows.Forms.TabPage();
			this.pnlConsonants = new SIL.Pa.Controls.PaPanel();
			this.tpgVows = new System.Windows.Forms.TabPage();
			this.pnlVowels = new SIL.Pa.Controls.PaPanel();
			this.tpgOther = new System.Windows.Forms.TabPage();
			this.charExplorer = new SIL.Pa.Controls.IPACharacterExplorer();
			this.tpgClasses = new System.Windows.Forms.TabPage();
			this.lvClasses = new SIL.Pa.Controls.ClassListView();
			this.tpgAFeatures = new System.Windows.Forms.TabPage();
			this.tpgBFeatures = new System.Windows.Forms.TabPage();
			this.tabPatternBlding.SuspendLayout();
			this.tpgCons.SuspendLayout();
			this.tpgVows.SuspendLayout();
			this.tpgOther.SuspendLayout();
			this.tpgClasses.SuspendLayout();
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
			resources.ApplyResources(this.tpgCons, "tpgCons");
			this.tpgCons.Name = "tpgCons";
			this.tpgCons.UseVisualStyleBackColor = true;
			// 
			// pnlConsonants
			// 
			resources.ApplyResources(this.pnlConsonants, "pnlConsonants");
			this.pnlConsonants.BackColor = System.Drawing.SystemColors.Window;
			this.pnlConsonants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlConsonants.ClipTextForChildControls = true;
			this.pnlConsonants.ControlReceivingFocusOnMnemonic = null;
			this.pnlConsonants.DoubleBuffered = false;
			this.pnlConsonants.MnemonicGeneratesClick = false;
			this.pnlConsonants.Name = "pnlConsonants";
			this.pnlConsonants.PaintExplorerBarBackground = false;
			// 
			// tpgVows
			// 
			this.tpgVows.Controls.Add(this.pnlVowels);
			resources.ApplyResources(this.tpgVows, "tpgVows");
			this.tpgVows.Name = "tpgVows";
			this.tpgVows.UseVisualStyleBackColor = true;
			// 
			// pnlVowels
			// 
			resources.ApplyResources(this.pnlVowels, "pnlVowels");
			this.pnlVowels.BackColor = System.Drawing.SystemColors.Window;
			this.pnlVowels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlVowels.ClipTextForChildControls = true;
			this.pnlVowels.ControlReceivingFocusOnMnemonic = null;
			this.pnlVowels.DoubleBuffered = false;
			this.pnlVowels.MnemonicGeneratesClick = false;
			this.pnlVowels.Name = "pnlVowels";
			this.pnlVowels.PaintExplorerBarBackground = false;
			// 
			// tpgOther
			// 
			this.tpgOther.Controls.Add(this.charExplorer);
			resources.ApplyResources(this.tpgOther, "tpgOther");
			this.tpgOther.Name = "tpgOther";
			this.tpgOther.UseVisualStyleBackColor = true;
			// 
			// charExplorer
			// 
			resources.ApplyResources(this.charExplorer, "charExplorer");
			this.charExplorer.BackColor = System.Drawing.SystemColors.Window;
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.ClipTextForChildControls = true;
			this.charExplorer.ControlReceivingFocusOnMnemonic = null;
			this.charExplorer.DoubleBuffered = false;
			this.charExplorer.MnemonicGeneratesClick = false;
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			// 
			// tpgClasses
			// 
			this.tpgClasses.Controls.Add(this.lvClasses);
			resources.ApplyResources(this.tpgClasses, "tpgClasses");
			this.tpgClasses.Name = "tpgClasses";
			this.tpgClasses.UseVisualStyleBackColor = true;
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.Controls.ClassListView.ListApplicationType.SearchViewWnd;
			resources.ApplyResources(this.lvClasses, "lvClasses");
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HideSelection = false;
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndTypeColumns = false;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			// 
			// tpgAFeatures
			// 
			resources.ApplyResources(this.tpgAFeatures, "tpgAFeatures");
			this.tpgAFeatures.Name = "tpgAFeatures";
			this.tpgAFeatures.UseVisualStyleBackColor = true;
			// 
			// tpgBFeatures
			// 
			resources.ApplyResources(this.tpgBFeatures, "tpgBFeatures");
			this.tpgBFeatures.Name = "tpgBFeatures";
			this.tpgBFeatures.UseVisualStyleBackColor = true;
			// 
			// PatternBuilderComponents
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabPatternBlding);
			this.Name = "PatternBuilderComponents";
			this.tabPatternBlding.ResumeLayout(false);
			this.tpgCons.ResumeLayout(false);
			this.tpgVows.ResumeLayout(false);
			this.tpgOther.ResumeLayout(false);
			this.tpgClasses.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabPatternBlding;
		private System.Windows.Forms.TabPage tpgCons;
		private PaPanel pnlConsonants;
		private System.Windows.Forms.TabPage tpgVows;
		private PaPanel pnlVowels;
		private System.Windows.Forms.TabPage tpgOther;
		private IPACharacterExplorer charExplorer;
		private System.Windows.Forms.TabPage tpgClasses;
		private ClassListView lvClasses;
		private System.Windows.Forms.TabPage tpgAFeatures;
		private System.Windows.Forms.TabPage tpgBFeatures;
	}
}
