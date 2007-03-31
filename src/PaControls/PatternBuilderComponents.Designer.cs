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
			this.tabPatternBlding.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPatternBlding.HotTrack = true;
			this.tabPatternBlding.Location = new System.Drawing.Point(0, 0);
			this.tabPatternBlding.Name = "tabPatternBlding";
			this.tabPatternBlding.SelectedIndex = 0;
			this.tabPatternBlding.ShowToolTips = true;
			this.tabPatternBlding.Size = new System.Drawing.Size(216, 214);
			this.tabPatternBlding.TabIndex = 1;
			this.tabPatternBlding.ClientSizeChanged += new System.EventHandler(this.tabPatternBlding_ClientSizeChanged);
			// 
			// tpgCons
			// 
			this.tpgCons.Controls.Add(this.pnlConsonants);
			this.tpgCons.Location = new System.Drawing.Point(4, 24);
			this.tpgCons.Name = "tpgCons";
			this.tpgCons.Padding = new System.Windows.Forms.Padding(5);
			this.tpgCons.Size = new System.Drawing.Size(208, 186);
			this.tpgCons.TabIndex = 1;
			this.tpgCons.Text = "Con.";
			this.tpgCons.ToolTipText = "Consonants";
			this.tpgCons.UseVisualStyleBackColor = true;
			// 
			// pnlConsonants
			// 
			this.pnlConsonants.AutoScroll = true;
			this.pnlConsonants.BackColor = System.Drawing.SystemColors.Window;
			this.pnlConsonants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlConsonants.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlConsonants.DoubleBuffered = false;
			this.pnlConsonants.Location = new System.Drawing.Point(5, 5);
			this.pnlConsonants.Name = "pnlConsonants";
			this.pnlConsonants.PaintExplorerBarBackground = false;
			this.pnlConsonants.Size = new System.Drawing.Size(198, 176);
			this.pnlConsonants.TabIndex = 0;
			// 
			// tpgVows
			// 
			this.tpgVows.Controls.Add(this.pnlVowels);
			this.tpgVows.Location = new System.Drawing.Point(4, 24);
			this.tpgVows.Name = "tpgVows";
			this.tpgVows.Padding = new System.Windows.Forms.Padding(3);
			this.tpgVows.Size = new System.Drawing.Size(208, 186);
			this.tpgVows.TabIndex = 4;
			this.tpgVows.Text = "Vow.";
			this.tpgVows.ToolTipText = "Vowels";
			this.tpgVows.UseVisualStyleBackColor = true;
			// 
			// pnlVowels
			// 
			this.pnlVowels.AutoScroll = true;
			this.pnlVowels.BackColor = System.Drawing.SystemColors.Window;
			this.pnlVowels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlVowels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlVowels.DoubleBuffered = false;
			this.pnlVowels.Location = new System.Drawing.Point(3, 3);
			this.pnlVowels.Name = "pnlVowels";
			this.pnlVowels.PaintExplorerBarBackground = false;
			this.pnlVowels.Size = new System.Drawing.Size(202, 180);
			this.pnlVowels.TabIndex = 2;
			// 
			// tpgOther
			// 
			this.tpgOther.Controls.Add(this.charExplorer);
			this.tpgOther.Location = new System.Drawing.Point(4, 24);
			this.tpgOther.Name = "tpgOther";
			this.tpgOther.Padding = new System.Windows.Forms.Padding(3);
			this.tpgOther.Size = new System.Drawing.Size(208, 186);
			this.tpgOther.TabIndex = 5;
			this.tpgOther.Text = "Other";
			this.tpgOther.ToolTipText = "Suprasegmentals and diacritics";
			this.tpgOther.UseVisualStyleBackColor = true;
			// 
			// charExplorer
			// 
			this.charExplorer.AutoScroll = true;
			this.charExplorer.BackColor = System.Drawing.SystemColors.Window;
			this.charExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.charExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.charExplorer.DoubleBuffered = false;
			this.charExplorer.Location = new System.Drawing.Point(3, 3);
			this.charExplorer.Name = "charExplorer";
			this.charExplorer.PaintExplorerBarBackground = false;
			this.charExplorer.Size = new System.Drawing.Size(202, 180);
			this.charExplorer.TabIndex = 0;
			// 
			// tpgClasses
			// 
			this.tpgClasses.Controls.Add(this.lvClasses);
			this.tpgClasses.Location = new System.Drawing.Point(4, 24);
			this.tpgClasses.Name = "tpgClasses";
			this.tpgClasses.Padding = new System.Windows.Forms.Padding(5);
			this.tpgClasses.Size = new System.Drawing.Size(208, 186);
			this.tpgClasses.TabIndex = 0;
			this.tpgClasses.Text = "Classes";
			this.tpgClasses.UseVisualStyleBackColor = true;
			// 
			// lvClasses
			// 
			this.lvClasses.AppliesTo = SIL.Pa.Controls.ClassListView.ListApplicationType.FindPhoneWindow;
			this.lvClasses.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvClasses.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.lvClasses.FullRowSelect = true;
			this.lvClasses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvClasses.HideSelection = false;
			this.lvClasses.Location = new System.Drawing.Point(5, 5);
			this.lvClasses.MultiSelect = false;
			this.lvClasses.Name = "lvClasses";
			this.lvClasses.OwnerDraw = true;
			this.lvClasses.ShowMembersAndBasedOn = false;
			this.lvClasses.Size = new System.Drawing.Size(198, 176);
			this.lvClasses.TabIndex = 0;
			this.lvClasses.UseCompatibleStateImageBehavior = false;
			this.lvClasses.View = System.Windows.Forms.View.Details;
			// 
			// tpgAFeatures
			// 
			this.tpgAFeatures.Location = new System.Drawing.Point(4, 24);
			this.tpgAFeatures.Name = "tpgAFeatures";
			this.tpgAFeatures.Padding = new System.Windows.Forms.Padding(5);
			this.tpgAFeatures.Size = new System.Drawing.Size(208, 186);
			this.tpgAFeatures.TabIndex = 2;
			this.tpgAFeatures.Text = "Art. Features";
			this.tpgAFeatures.ToolTipText = "Articulatory Features";
			this.tpgAFeatures.UseVisualStyleBackColor = true;
			// 
			// tpgBFeatures
			// 
			this.tpgBFeatures.Location = new System.Drawing.Point(4, 24);
			this.tpgBFeatures.Name = "tpgBFeatures";
			this.tpgBFeatures.Padding = new System.Windows.Forms.Padding(5);
			this.tpgBFeatures.Size = new System.Drawing.Size(208, 186);
			this.tpgBFeatures.TabIndex = 3;
			this.tpgBFeatures.Text = "Bin. Features";
			this.tpgBFeatures.ToolTipText = "Binary Features";
			this.tpgBFeatures.UseVisualStyleBackColor = true;
			// 
			// PatternBuilderComponents
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabPatternBlding);
			this.Name = "PatternBuilderComponents";
			this.Size = new System.Drawing.Size(216, 214);
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
