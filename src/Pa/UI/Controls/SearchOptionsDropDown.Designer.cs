namespace SIL.Pa.UI.Controls
{
	partial class SearchOptionsDropDown
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
			this.chkIgnoreDiacritics = new System.Windows.Forms.CheckBox();
			this.chkShowAllWords = new System.Windows.Forms.CheckBox();
			this.chkStress = new System.Windows.Forms.CheckBox();
			this.grpStress = new System.Windows.Forms.GroupBox();
			this.stressPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.chkTone = new System.Windows.Forms.CheckBox();
			this.grpTone = new System.Windows.Forms.GroupBox();
			this.tonePicker = new SIL.Pa.UI.Controls.CharPicker();
			this.chkLength = new System.Windows.Forms.CheckBox();
			this.grpLength = new System.Windows.Forms.GroupBox();
			this.lengthPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.lnkApplyToAll = new System.Windows.Forms.LinkLabel();
			this.lnkHelp = new System.Windows.Forms.LinkLabel();
			this.grpUncertainties = new System.Windows.Forms.GroupBox();
			this.rbAllUncertainties = new System.Windows.Forms.RadioButton();
			this.rbPrimaryOnly = new System.Windows.Forms.RadioButton();
			this.lblUncertainties = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.grpStress.SuspendLayout();
			this.grpTone.SuspendLayout();
			this.grpLength.SuspendLayout();
			this.grpUncertainties.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// chkIgnoreDiacritics
			// 
			this.chkIgnoreDiacritics.AutoSize = true;
			this.chkIgnoreDiacritics.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkIgnoreDiacritics.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkIgnoreDiacritics.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkIgnoreDiacritics, null);
			this.locExtender.SetLocalizationComment(this.chkIgnoreDiacritics, null);
			this.locExtender.SetLocalizingId(this.chkIgnoreDiacritics, "SearchOptionsDropDown.chkIgnoreDiacritics");
			this.chkIgnoreDiacritics.Location = new System.Drawing.Point(24, 8);
			this.chkIgnoreDiacritics.Name = "chkIgnoreDiacritics";
			this.chkIgnoreDiacritics.Size = new System.Drawing.Size(114, 19);
			this.chkIgnoreDiacritics.TabIndex = 0;
			this.chkIgnoreDiacritics.Text = "Ignore &Diacritics";
			this.chkIgnoreDiacritics.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkIgnoreDiacritics.UseVisualStyleBackColor = true;
			// 
			// chkShowAllWords
			// 
			this.chkShowAllWords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkShowAllWords.AutoSize = true;
			this.chkShowAllWords.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowAllWords.Enabled = false;
			this.chkShowAllWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkShowAllWords.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkShowAllWords, null);
			this.locExtender.SetLocalizationComment(this.chkShowAllWords, null);
			this.locExtender.SetLocalizingId(this.chkShowAllWords, "SearchOptionsDropDown.chkShowAllWords");
			this.chkShowAllWords.Location = new System.Drawing.Point(161, -133);
			this.chkShowAllWords.Name = "chkShowAllWords";
			this.chkShowAllWords.Size = new System.Drawing.Size(212, 19);
			this.chkShowAllWords.TabIndex = 10;
			this.chkShowAllWords.Text = "Show &all occurances of each word";
			this.chkShowAllWords.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowAllWords.UseVisualStyleBackColor = true;
			this.chkShowAllWords.Visible = false;
			// 
			// chkStress
			// 
			this.chkStress.AutoSize = true;
			this.chkStress.BackColor = System.Drawing.Color.Transparent;
			this.chkStress.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkStress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkStress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkStress, null);
			this.locExtender.SetLocalizationComment(this.chkStress, null);
			this.locExtender.SetLocalizingId(this.chkStress, "SearchOptionsDropDown.chkStress");
			this.chkStress.Location = new System.Drawing.Point(24, 32);
			this.chkStress.Name = "chkStress";
			this.chkStress.Size = new System.Drawing.Size(144, 19);
			this.chkStress.TabIndex = 1;
			this.chkStress.Text = "Ignore &Stress/Syllable";
			this.chkStress.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkStress.ThreeState = true;
			this.chkStress.UseVisualStyleBackColor = false;
			this.chkStress.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// grpStress
			// 
			this.grpStress.Controls.Add(this.stressPicker);
			this.locExtender.SetLocalizableToolTip(this.grpStress, null);
			this.locExtender.SetLocalizationComment(this.grpStress, null);
			this.locExtender.SetLocalizationPriority(this.grpStress, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpStress, "SearchOptionsDropDown.grpStress");
			this.grpStress.Location = new System.Drawing.Point(14, 34);
			this.grpStress.Name = "grpStress";
			this.grpStress.Padding = new System.Windows.Forms.Padding(7);
			this.grpStress.Size = new System.Drawing.Size(222, 40);
			this.grpStress.TabIndex = 2;
			this.grpStress.TabStop = false;
			// 
			// stressPicker
			// 
			this.stressPicker.AutoSize = false;
			this.stressPicker.AutoSizeItems = false;
			this.stressPicker.BackColor = System.Drawing.Color.Transparent;
			this.stressPicker.CheckItemsOnClick = true;
			this.stressPicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stressPicker.FontSize = 14F;
			this.stressPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.stressPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.stressPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this.stressPicker, null);
			this.locExtender.SetLocalizationComment(this.stressPicker, null);
			this.locExtender.SetLocalizingId(this.stressPicker, "SearchOptionsDropDown.stressPicker");
			this.stressPicker.Location = new System.Drawing.Point(7, 20);
			this.stressPicker.Name = "stressPicker";
			this.stressPicker.Padding = new System.Windows.Forms.Padding(0);
			this.stressPicker.Size = new System.Drawing.Size(208, 13);
			this.stressPicker.TabIndex = 0;
			this.stressPicker.Text = "charPicker1";
			this.stressPicker.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// chkTone
			// 
			this.chkTone.AutoSize = true;
			this.chkTone.BackColor = System.Drawing.Color.Transparent;
			this.chkTone.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkTone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkTone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkTone, null);
			this.locExtender.SetLocalizationComment(this.chkTone, null);
			this.locExtender.SetLocalizingId(this.chkTone, "SearchOptionsDropDown.chkTone");
			this.chkTone.Location = new System.Drawing.Point(24, 82);
			this.chkTone.Name = "chkTone";
			this.chkTone.Size = new System.Drawing.Size(92, 19);
			this.chkTone.TabIndex = 3;
			this.chkTone.Text = "Ignore &Tone";
			this.chkTone.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkTone.ThreeState = true;
			this.chkTone.UseVisualStyleBackColor = false;
			this.chkTone.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// grpTone
			// 
			this.grpTone.Controls.Add(this.tonePicker);
			this.locExtender.SetLocalizableToolTip(this.grpTone, null);
			this.locExtender.SetLocalizationComment(this.grpTone, null);
			this.locExtender.SetLocalizationPriority(this.grpTone, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpTone, "SearchOptionsDropDown.grpTone");
			this.grpTone.Location = new System.Drawing.Point(14, 84);
			this.grpTone.Name = "grpTone";
			this.grpTone.Padding = new System.Windows.Forms.Padding(7);
			this.grpTone.Size = new System.Drawing.Size(222, 47);
			this.grpTone.TabIndex = 4;
			this.grpTone.TabStop = false;
			// 
			// tonePicker
			// 
			this.tonePicker.AutoSize = false;
			this.tonePicker.AutoSizeItems = false;
			this.tonePicker.BackColor = System.Drawing.Color.Transparent;
			this.tonePicker.CheckItemsOnClick = true;
			this.tonePicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tonePicker.FontSize = 14F;
			this.tonePicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tonePicker.ItemSize = new System.Drawing.Size(30, 32);
			this.tonePicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this.tonePicker, null);
			this.locExtender.SetLocalizationComment(this.tonePicker, null);
			this.locExtender.SetLocalizingId(this.tonePicker, "SearchOptionsDropDown.tonePicker");
			this.tonePicker.Location = new System.Drawing.Point(7, 20);
			this.tonePicker.Name = "tonePicker";
			this.tonePicker.Padding = new System.Windows.Forms.Padding(0);
			this.tonePicker.Size = new System.Drawing.Size(208, 20);
			this.tonePicker.TabIndex = 0;
			this.tonePicker.Text = "charPicker1";
			this.tonePicker.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// chkLength
			// 
			this.chkLength.AutoSize = true;
			this.chkLength.BackColor = System.Drawing.Color.Transparent;
			this.chkLength.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkLength, null);
			this.locExtender.SetLocalizationComment(this.chkLength, null);
			this.locExtender.SetLocalizingId(this.chkLength, "SearchOptionsDropDown.chkLength");
			this.chkLength.Location = new System.Drawing.Point(24, 141);
			this.chkLength.Name = "chkLength";
			this.chkLength.Size = new System.Drawing.Size(102, 19);
			this.chkLength.TabIndex = 5;
			this.chkLength.Text = "Ignore &Length";
			this.chkLength.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkLength.ThreeState = true;
			this.chkLength.UseVisualStyleBackColor = false;
			this.chkLength.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// grpLength
			// 
			this.grpLength.Controls.Add(this.lengthPicker);
			this.locExtender.SetLocalizableToolTip(this.grpLength, null);
			this.locExtender.SetLocalizationComment(this.grpLength, null);
			this.locExtender.SetLocalizationPriority(this.grpLength, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpLength, "SearchOptionsDropDown.grpLength");
			this.grpLength.Location = new System.Drawing.Point(14, 144);
			this.grpLength.Name = "grpLength";
			this.grpLength.Padding = new System.Windows.Forms.Padding(7);
			this.grpLength.Size = new System.Drawing.Size(222, 49);
			this.grpLength.TabIndex = 6;
			this.grpLength.TabStop = false;
			// 
			// lengthPicker
			// 
			this.lengthPicker.AutoSize = false;
			this.lengthPicker.AutoSizeItems = false;
			this.lengthPicker.BackColor = System.Drawing.Color.Transparent;
			this.lengthPicker.CheckItemsOnClick = true;
			this.lengthPicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lengthPicker.FontSize = 14F;
			this.lengthPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.lengthPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.lengthPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this.lengthPicker, null);
			this.locExtender.SetLocalizationComment(this.lengthPicker, null);
			this.locExtender.SetLocalizingId(this.lengthPicker, "SearchOptionsDropDown.lengthPicker");
			this.lengthPicker.Location = new System.Drawing.Point(7, 20);
			this.lengthPicker.Name = "lengthPicker";
			this.lengthPicker.Padding = new System.Windows.Forms.Padding(0);
			this.lengthPicker.Size = new System.Drawing.Size(208, 22);
			this.lengthPicker.TabIndex = 0;
			this.lengthPicker.Text = "charPicker1";
			this.lengthPicker.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// lnkApplyToAll
			// 
			this.lnkApplyToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkApplyToAll.AutoSize = true;
			this.lnkApplyToAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lnkApplyToAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lnkApplyToAll, null);
			this.locExtender.SetLocalizationComment(this.lnkApplyToAll, null);
			this.locExtender.SetLocalizingId(this.lnkApplyToAll, "SearchOptionsDropDown.lnkApplyToAll");
			this.lnkApplyToAll.Location = new System.Drawing.Point(11, 361);
			this.lnkApplyToAll.Name = "lnkApplyToAll";
			this.lnkApplyToAll.Size = new System.Drawing.Size(115, 15);
			this.lnkApplyToAll.TabIndex = 8;
			this.lnkApplyToAll.TabStop = true;
			this.lnkApplyToAll.Text = "Apply to all columns";
			// 
			// lnkHelp
			// 
			this.lnkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkHelp.AutoSize = true;
			this.lnkHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lnkHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lnkHelp, null);
			this.locExtender.SetLocalizationComment(this.lnkHelp, null);
			this.locExtender.SetLocalizingId(this.lnkHelp, "SearchOptionsDropDown.lnkHelp");
			this.lnkHelp.Location = new System.Drawing.Point(202, 361);
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.Size = new System.Drawing.Size(33, 15);
			this.lnkHelp.TabIndex = 9;
			this.lnkHelp.TabStop = true;
			this.lnkHelp.Text = "Help";
			this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleHelpClicked);
			// 
			// grpUncertainties
			// 
			this.grpUncertainties.BackColor = System.Drawing.Color.Transparent;
			this.grpUncertainties.Controls.Add(this.rbAllUncertainties);
			this.grpUncertainties.Controls.Add(this.rbPrimaryOnly);
			this.grpUncertainties.Controls.Add(this.lblUncertainties);
			this.grpUncertainties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.grpUncertainties, null);
			this.locExtender.SetLocalizationComment(this.grpUncertainties, null);
			this.locExtender.SetLocalizingId(this.grpUncertainties, "SearchOptionsDropDown.grpUncertainties");
			this.grpUncertainties.Location = new System.Drawing.Point(14, 206);
			this.grpUncertainties.Name = "grpUncertainties";
			this.grpUncertainties.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
			this.grpUncertainties.Size = new System.Drawing.Size(222, 115);
			this.grpUncertainties.TabIndex = 7;
			this.grpUncertainties.TabStop = false;
			// 
			// rbAllUncertainties
			// 
			this.rbAllUncertainties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rbAllUncertainties.AutoEllipsis = true;
			this.rbAllUncertainties.BackColor = System.Drawing.Color.Transparent;
			this.rbAllUncertainties.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbAllUncertainties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.rbAllUncertainties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbAllUncertainties, null);
			this.locExtender.SetLocalizationComment(this.rbAllUncertainties, null);
			this.locExtender.SetLocalizingId(this.rbAllUncertainties, "SearchOptionsDropDown.rbAllUncertainties");
			this.rbAllUncertainties.Location = new System.Drawing.Point(10, 59);
			this.rbAllUncertainties.Name = "rbAllUncertainties";
			this.rbAllUncertainties.Size = new System.Drawing.Size(209, 52);
			this.rbAllUncertainties.TabIndex = 2;
			this.rbAllUncertainties.Text = "Search Transcriptions Derived from Primary and &Non Primary Uncertainties";
			this.rbAllUncertainties.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbAllUncertainties.UseVisualStyleBackColor = false;
			// 
			// rbPrimaryOnly
			// 
			this.rbPrimaryOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rbPrimaryOnly.AutoEllipsis = true;
			this.rbPrimaryOnly.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbPrimaryOnly.Checked = true;
			this.rbPrimaryOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.rbPrimaryOnly.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbPrimaryOnly, null);
			this.locExtender.SetLocalizationComment(this.rbPrimaryOnly, null);
			this.locExtender.SetLocalizingId(this.rbPrimaryOnly, "SearchOptionsDropDown.rbPrimaryOnly");
			this.rbPrimaryOnly.Location = new System.Drawing.Point(10, 21);
			this.rbPrimaryOnly.Name = "rbPrimaryOnly";
			this.rbPrimaryOnly.Size = new System.Drawing.Size(208, 39);
			this.rbPrimaryOnly.TabIndex = 1;
			this.rbPrimaryOnly.TabStop = true;
			this.rbPrimaryOnly.Text = "Search Transcriptions Derived Only from &Primary Uncertainties";
			this.rbPrimaryOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbPrimaryOnly.UseVisualStyleBackColor = true;
			// 
			// lblUncertainties
			// 
			this.lblUncertainties.AutoSize = true;
			this.lblUncertainties.BackColor = System.Drawing.Color.White;
			this.lblUncertainties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblUncertainties, null);
			this.locExtender.SetLocalizationComment(this.lblUncertainties, null);
			this.locExtender.SetLocalizingId(this.lblUncertainties, "SearchOptionsDropDown.lblUncertainties");
			this.lblUncertainties.Location = new System.Drawing.Point(10, 0);
			this.lblUncertainties.Name = "lblUncertainties";
			this.lblUncertainties.Size = new System.Drawing.Size(179, 15);
			this.lblUncertainties.TabIndex = 0;
			this.lblUncertainties.Text = "Records with Uncertain Phones";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SearchOptionsDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.lnkApplyToAll);
			this.Controls.Add(this.chkLength);
			this.Controls.Add(this.grpLength);
			this.Controls.Add(this.chkTone);
			this.Controls.Add(this.grpTone);
			this.Controls.Add(this.chkShowAllWords);
			this.Controls.Add(this.chkStress);
			this.Controls.Add(this.chkIgnoreDiacritics);
			this.Controls.Add(this.grpStress);
			this.Controls.Add(this.grpUncertainties);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SearchOptionsDropDown.SearchOptionsDropDown");
			this.MinimumSize = new System.Drawing.Size(250, 2);
			this.Name = "SearchOptionsDropDown";
			this.Size = new System.Drawing.Size(248, 381);
			this.grpStress.ResumeLayout(false);
			this.grpTone.ResumeLayout(false);
			this.grpLength.ResumeLayout(false);
			this.grpUncertainties.ResumeLayout(false);
			this.grpUncertainties.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private CharPicker tonePicker;
		private CharPicker stressPicker;
		private CharPicker lengthPicker;
		protected System.Windows.Forms.CheckBox chkShowAllWords;
		protected System.Windows.Forms.GroupBox grpStress;
		protected System.Windows.Forms.GroupBox grpTone;
		protected System.Windows.Forms.GroupBox grpLength;
		protected System.Windows.Forms.CheckBox chkIgnoreDiacritics;
		protected System.Windows.Forms.CheckBox chkStress;
		protected System.Windows.Forms.CheckBox chkTone;
		protected System.Windows.Forms.CheckBox chkLength;
		private System.Windows.Forms.LinkLabel lnkApplyToAll;
		public System.Windows.Forms.LinkLabel lnkHelp;
		protected System.Windows.Forms.GroupBox grpUncertainties;
		private System.Windows.Forms.RadioButton rbAllUncertainties;
		private System.Windows.Forms.RadioButton rbPrimaryOnly;
		protected System.Windows.Forms.Label lblUncertainties;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
