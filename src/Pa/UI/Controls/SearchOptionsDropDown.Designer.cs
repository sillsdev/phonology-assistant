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
			this._chkIgnoreDiacritics = new System.Windows.Forms.CheckBox();
			this._chkShowAllWords = new System.Windows.Forms.CheckBox();
			this._chkStress = new System.Windows.Forms.CheckBox();
			this._groupStress = new System.Windows.Forms.GroupBox();
			this._pickerStress = new SIL.Pa.UI.Controls.CharPicker();
			this._chkTone = new System.Windows.Forms.CheckBox();
			this._groupTone = new System.Windows.Forms.GroupBox();
			this._pickerTone = new SIL.Pa.UI.Controls.CharPicker();
			this._chkLength = new System.Windows.Forms.CheckBox();
			this._groupLength = new System.Windows.Forms.GroupBox();
			this._pickerLength = new SIL.Pa.UI.Controls.CharPicker();
			this._linkApplyToAll = new System.Windows.Forms.LinkLabel();
			this._linkHelp = new System.Windows.Forms.LinkLabel();
			this._groupUncertainties = new System.Windows.Forms.GroupBox();
			this.rbAllUncertainties = new System.Windows.Forms.RadioButton();
			this.rbPrimaryOnly = new System.Windows.Forms.RadioButton();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._chkBoundary = new System.Windows.Forms.CheckBox();
			this._groupBoundary = new System.Windows.Forms.GroupBox();
			this._pickerBoundary = new SIL.Pa.UI.Controls.CharPicker();
			this._chkPitchPhonation = new System.Windows.Forms.CheckBox();
			this._groupPitchPhonation = new System.Windows.Forms.GroupBox();
			this._pickerPitchPhonation = new SIL.Pa.UI.Controls.CharPicker();
			this.lnkCancel = new System.Windows.Forms.LinkLabel();
			this.lnkOk = new System.Windows.Forms.LinkLabel();
			this._panelStress = new System.Windows.Forms.Panel();
			this._panelLength = new System.Windows.Forms.Panel();
			this._panelTone = new System.Windows.Forms.Panel();
			this._panelBoundary = new System.Windows.Forms.Panel();
			this._panelPitchPhonation = new System.Windows.Forms.Panel();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._groupStress.SuspendLayout();
			this._groupTone.SuspendLayout();
			this._groupLength.SuspendLayout();
			this._groupUncertainties.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._groupBoundary.SuspendLayout();
			this._groupPitchPhonation.SuspendLayout();
			this._panelStress.SuspendLayout();
			this._panelLength.SuspendLayout();
			this._panelTone.SuspendLayout();
			this._panelBoundary.SuspendLayout();
			this._panelPitchPhonation.SuspendLayout();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _chkIgnoreDiacritics
			// 
			this._chkIgnoreDiacritics.AutoSize = true;
			this._chkIgnoreDiacritics.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._tableLayout.SetColumnSpan(this._chkIgnoreDiacritics, 4);
			this._chkIgnoreDiacritics.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkIgnoreDiacritics.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkIgnoreDiacritics, null);
			this.locExtender.SetLocalizationComment(this._chkIgnoreDiacritics, null);
			this.locExtender.SetLocalizingId(this._chkIgnoreDiacritics, "Views.WordLists.SearchResults.SearchOptionsPopup.IgnoreDiacriticsCheckbox");
			this._chkIgnoreDiacritics.Location = new System.Drawing.Point(13, 32);
			this._chkIgnoreDiacritics.Margin = new System.Windows.Forms.Padding(13, 6, 4, 4);
			this._chkIgnoreDiacritics.Name = "_chkIgnoreDiacritics";
			this._chkIgnoreDiacritics.Size = new System.Drawing.Size(136, 22);
			this._chkIgnoreDiacritics.TabIndex = 1;
			this._chkIgnoreDiacritics.Text = "Ignore &Diacritics";
			this._chkIgnoreDiacritics.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkIgnoreDiacritics.UseVisualStyleBackColor = true;
			// 
			// _chkShowAllWords
			// 
			this._chkShowAllWords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._chkShowAllWords.AutoSize = true;
			this._chkShowAllWords.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._tableLayout.SetColumnSpan(this._chkShowAllWords, 4);
			this._chkShowAllWords.Enabled = false;
			this._chkShowAllWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkShowAllWords.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkShowAllWords, null);
			this.locExtender.SetLocalizationComment(this._chkShowAllWords, null);
			this.locExtender.SetLocalizingId(this._chkShowAllWords, "Views.WordLists.SearchResults.SearchOptionsPopup.ShowAllWordsCheckbox");
			this._chkShowAllWords.Location = new System.Drawing.Point(13, 0);
			this._chkShowAllWords.Margin = new System.Windows.Forms.Padding(13, 0, 4, 4);
			this._chkShowAllWords.Name = "_chkShowAllWords";
			this._chkShowAllWords.Size = new System.Drawing.Size(258, 22);
			this._chkShowAllWords.TabIndex = 0;
			this._chkShowAllWords.Text = "Show &all occurances of each word";
			this._chkShowAllWords.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkShowAllWords.UseVisualStyleBackColor = true;
			this._chkShowAllWords.Visible = false;
			// 
			// _chkStress
			// 
			this._chkStress.AutoSize = true;
			this._chkStress.BackColor = System.Drawing.Color.Transparent;
			this._chkStress.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkStress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkStress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkStress, null);
			this.locExtender.SetLocalizationComment(this._chkStress, null);
			this.locExtender.SetLocalizingId(this._chkStress, "Views.WordLists.SearchResults.SearchOptionsPopup.StressCheckbox");
			this._chkStress.Location = new System.Drawing.Point(13, 0);
			this._chkStress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this._chkStress.Name = "_chkStress";
			this._chkStress.Size = new System.Drawing.Size(118, 22);
			this._chkStress.TabIndex = 0;
			this._chkStress.Text = "Ignore &Stress";
			this._chkStress.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkStress.ThreeState = true;
			this._chkStress.UseVisualStyleBackColor = false;
			this._chkStress.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// _groupStress
			// 
			this._groupStress.Controls.Add(this._pickerStress);
			this.locExtender.SetLocalizableToolTip(this._groupStress, null);
			this.locExtender.SetLocalizationComment(this._groupStress, null);
			this.locExtender.SetLocalizationPriority(this._groupStress, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._groupStress, "SearchOptionsDropDown.grpStress");
			this._groupStress.Location = new System.Drawing.Point(0, 2);
			this._groupStress.Margin = new System.Windows.Forms.Padding(0);
			this._groupStress.Name = "_groupStress";
			this._groupStress.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
			this._groupStress.Size = new System.Drawing.Size(333, 49);
			this._groupStress.TabIndex = 1;
			this._groupStress.TabStop = false;
			// 
			// _pickerStress
			// 
			this._pickerStress.AutoSize = false;
			this._pickerStress.AutoSizeItems = false;
			this._pickerStress.BackColor = System.Drawing.Color.Transparent;
			this._pickerStress.CheckItemsOnClick = true;
			this._pickerStress.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pickerStress.FontSize = 14F;
			this._pickerStress.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._pickerStress.ImageScalingSize = new System.Drawing.Size(20, 20);
			this._pickerStress.ItemSize = new System.Drawing.Size(30, 32);
			this._pickerStress.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this._pickerStress, null);
			this.locExtender.SetLocalizationComment(this._pickerStress, null);
			this.locExtender.SetLocalizationPriority(this._pickerStress, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pickerStress, "SearchOptionsDropDown.stressPicker");
			this._pickerStress.Location = new System.Drawing.Point(9, 24);
			this._pickerStress.Name = "_pickerStress";
			this._pickerStress.Padding = new System.Windows.Forms.Padding(0);
			this._pickerStress.Size = new System.Drawing.Size(315, 16);
			this._pickerStress.TabIndex = 0;
			this._pickerStress.Text = "charPicker1";
			this._pickerStress.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// _chkTone
			// 
			this._chkTone.AutoSize = true;
			this._chkTone.BackColor = System.Drawing.Color.Transparent;
			this._chkTone.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkTone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkTone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkTone, null);
			this.locExtender.SetLocalizationComment(this._chkTone, null);
			this.locExtender.SetLocalizingId(this._chkTone, "Views.WordLists.SearchResults.SearchOptionsPopup.ToneCheckbox");
			this._chkTone.Location = new System.Drawing.Point(13, 0);
			this._chkTone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this._chkTone.Name = "_chkTone";
			this._chkTone.Size = new System.Drawing.Size(109, 22);
			this._chkTone.TabIndex = 0;
			this._chkTone.Text = "Ignore &Tone";
			this._chkTone.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkTone.ThreeState = true;
			this._chkTone.UseVisualStyleBackColor = false;
			this._chkTone.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// _groupTone
			// 
			this._groupTone.Controls.Add(this._pickerTone);
			this.locExtender.SetLocalizableToolTip(this._groupTone, null);
			this.locExtender.SetLocalizationComment(this._groupTone, null);
			this.locExtender.SetLocalizationPriority(this._groupTone, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._groupTone, "SearchOptionsDropDown.grpTone");
			this._groupTone.Location = new System.Drawing.Point(0, 2);
			this._groupTone.Margin = new System.Windows.Forms.Padding(0);
			this._groupTone.Name = "_groupTone";
			this._groupTone.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
			this._groupTone.Size = new System.Drawing.Size(333, 58);
			this._groupTone.TabIndex = 4;
			this._groupTone.TabStop = false;
			// 
			// _pickerTone
			// 
			this._pickerTone.AutoSize = false;
			this._pickerTone.AutoSizeItems = false;
			this._pickerTone.BackColor = System.Drawing.Color.Transparent;
			this._pickerTone.CheckItemsOnClick = true;
			this._pickerTone.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pickerTone.FontSize = 14F;
			this._pickerTone.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._pickerTone.ImageScalingSize = new System.Drawing.Size(20, 20);
			this._pickerTone.ItemSize = new System.Drawing.Size(30, 32);
			this._pickerTone.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this._pickerTone, null);
			this.locExtender.SetLocalizationComment(this._pickerTone, null);
			this.locExtender.SetLocalizationPriority(this._pickerTone, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pickerTone, "SearchOptionsDropDown.tonePicker");
			this._pickerTone.Location = new System.Drawing.Point(9, 24);
			this._pickerTone.Name = "_pickerTone";
			this._pickerTone.Padding = new System.Windows.Forms.Padding(0);
			this._pickerTone.Size = new System.Drawing.Size(315, 25);
			this._pickerTone.TabIndex = 0;
			this._pickerTone.Text = "charPicker1";
			this._pickerTone.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// _chkLength
			// 
			this._chkLength.AutoSize = true;
			this._chkLength.BackColor = System.Drawing.Color.Transparent;
			this._chkLength.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkLength, null);
			this.locExtender.SetLocalizationComment(this._chkLength, null);
			this.locExtender.SetLocalizingId(this._chkLength, "Views.WordLists.SearchResults.SearchOptionsPopup.LengthCheckbox");
			this._chkLength.Location = new System.Drawing.Point(13, 0);
			this._chkLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this._chkLength.Name = "_chkLength";
			this._chkLength.Size = new System.Drawing.Size(119, 22);
			this._chkLength.TabIndex = 0;
			this._chkLength.Text = "Ignore &Length";
			this._chkLength.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkLength.ThreeState = true;
			this._chkLength.UseVisualStyleBackColor = false;
			this._chkLength.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// _groupLength
			// 
			this._groupLength.Controls.Add(this._pickerLength);
			this.locExtender.SetLocalizableToolTip(this._groupLength, null);
			this.locExtender.SetLocalizationComment(this._groupLength, null);
			this.locExtender.SetLocalizationPriority(this._groupLength, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._groupLength, "SearchOptionsDropDown.grpLength");
			this._groupLength.Location = new System.Drawing.Point(0, 4);
			this._groupLength.Margin = new System.Windows.Forms.Padding(0);
			this._groupLength.Name = "_groupLength";
			this._groupLength.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
			this._groupLength.Size = new System.Drawing.Size(333, 60);
			this._groupLength.TabIndex = 3;
			this._groupLength.TabStop = false;
			// 
			// _pickerLength
			// 
			this._pickerLength.AutoSize = false;
			this._pickerLength.AutoSizeItems = false;
			this._pickerLength.BackColor = System.Drawing.Color.Transparent;
			this._pickerLength.CheckItemsOnClick = true;
			this._pickerLength.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pickerLength.FontSize = 14F;
			this._pickerLength.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._pickerLength.ImageScalingSize = new System.Drawing.Size(20, 20);
			this._pickerLength.ItemSize = new System.Drawing.Size(30, 32);
			this._pickerLength.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this._pickerLength, null);
			this.locExtender.SetLocalizationComment(this._pickerLength, null);
			this.locExtender.SetLocalizationPriority(this._pickerLength, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pickerLength, "SearchOptionsDropDown.lengthPicker");
			this._pickerLength.Location = new System.Drawing.Point(9, 24);
			this._pickerLength.Name = "_pickerLength";
			this._pickerLength.Padding = new System.Windows.Forms.Padding(0);
			this._pickerLength.Size = new System.Drawing.Size(315, 27);
			this._pickerLength.TabIndex = 0;
			this._pickerLength.Text = "charPicker1";
			this._pickerLength.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// _linkApplyToAll
			// 
			this._linkApplyToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._linkApplyToAll.AutoSize = true;
			this._linkApplyToAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._linkApplyToAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._linkApplyToAll, null);
			this.locExtender.SetLocalizationComment(this._linkApplyToAll, null);
			this.locExtender.SetLocalizingId(this._linkApplyToAll, "Views.WordLists.SearchResults.SearchOptionsPopup.ApplyToAllLink");
			this._linkApplyToAll.Location = new System.Drawing.Point(0, 565);
			this._linkApplyToAll.Margin = new System.Windows.Forms.Padding(0, 5, 4, 0);
			this._linkApplyToAll.Name = "_linkApplyToAll";
			this._linkApplyToAll.Size = new System.Drawing.Size(139, 18);
			this._linkApplyToAll.TabIndex = 7;
			this._linkApplyToAll.TabStop = true;
			this._linkApplyToAll.Text = "Apply to all columns";
			// 
			// _linkHelp
			// 
			this._linkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._linkHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._linkHelp, null);
			this.locExtender.SetLocalizationComment(this._linkHelp, null);
			this.locExtender.SetLocalizingId(this._linkHelp, "Views.WordLists.SearchResults.SearchOptionsPopup.HelpLink");
			this._linkHelp.Location = new System.Drawing.Point(308, 561);
			this._linkHelp.Margin = new System.Windows.Forms.Padding(4, 5, 0, 0);
			this._linkHelp.Name = "_linkHelp";
			this._linkHelp.Size = new System.Drawing.Size(65, 22);
			this._linkHelp.TabIndex = 8;
			this._linkHelp.TabStop = true;
			this._linkHelp.Text = "Help";
			this._linkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleHelpClicked);
			// 
			// _groupUncertainties
			// 
			this._groupUncertainties.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._groupUncertainties, 4);
			this._groupUncertainties.Controls.Add(this.rbAllUncertainties);
			this._groupUncertainties.Controls.Add(this.rbPrimaryOnly);
			this._groupUncertainties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this._groupUncertainties, null);
			this.locExtender.SetLocalizationComment(this._groupUncertainties, null);
			this.locExtender.SetLocalizingId(this._groupUncertainties, "Views.WordLists.SearchResults.SearchOptionsPopup.UncertaintiesGroupBox");
			this._groupUncertainties.Location = new System.Drawing.Point(0, 412);
			this._groupUncertainties.Margin = new System.Windows.Forms.Padding(0, 7, 0, 4);
			this._groupUncertainties.Name = "_groupUncertainties";
			this._groupUncertainties.Padding = new System.Windows.Forms.Padding(9, 12, 9, 9);
			this._groupUncertainties.Size = new System.Drawing.Size(333, 140);
			this._groupUncertainties.TabIndex = 7;
			this._groupUncertainties.TabStop = false;
			this._groupUncertainties.Text = "Records with Uncertain Phones";
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
			this.locExtender.SetLocalizingId(this.rbAllUncertainties, "Views.WordLists.SearchResults.SearchOptionsPopup.AllUncertaintiesRadioButton");
			this.rbAllUncertainties.Location = new System.Drawing.Point(13, 73);
			this.rbAllUncertainties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.rbAllUncertainties.Name = "rbAllUncertainties";
			this.rbAllUncertainties.Size = new System.Drawing.Size(311, 64);
			this.rbAllUncertainties.TabIndex = 1;
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
			this.locExtender.SetLocalizingId(this.rbPrimaryOnly, "Views.WordLists.SearchResults.SearchOptionsPopup.PrimaryOnlyRadioButton");
			this.rbPrimaryOnly.Location = new System.Drawing.Point(13, 26);
			this.rbPrimaryOnly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.rbPrimaryOnly.Name = "rbPrimaryOnly";
			this.rbPrimaryOnly.Size = new System.Drawing.Size(311, 48);
			this.rbPrimaryOnly.TabIndex = 0;
			this.rbPrimaryOnly.TabStop = true;
			this.rbPrimaryOnly.Text = "Search Transcriptions Derived Only from &Primary Uncertainties";
			this.rbPrimaryOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbPrimaryOnly.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			this.locExtender.PrefixForNewItems = null;
			// 
			// _chkBoundary
			// 
			this._chkBoundary.AutoSize = true;
			this._chkBoundary.BackColor = System.Drawing.Color.Transparent;
			this._chkBoundary.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkBoundary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkBoundary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkBoundary, null);
			this.locExtender.SetLocalizationComment(this._chkBoundary, null);
			this.locExtender.SetLocalizingId(this._chkBoundary, "Views.WordLists.SearchResults.SearchOptionsPopup.BoundaryCheckbox");
			this._chkBoundary.Location = new System.Drawing.Point(13, 0);
			this._chkBoundary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this._chkBoundary.Name = "_chkBoundary";
			this._chkBoundary.Size = new System.Drawing.Size(138, 22);
			this._chkBoundary.TabIndex = 0;
			this._chkBoundary.Text = "Ignore &Boundary";
			this._chkBoundary.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkBoundary.ThreeState = true;
			this._chkBoundary.UseVisualStyleBackColor = false;
			this._chkBoundary.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// _groupBoundary
			// 
			this._groupBoundary.Controls.Add(this._pickerBoundary);
			this.locExtender.SetLocalizableToolTip(this._groupBoundary, null);
			this.locExtender.SetLocalizationComment(this._groupBoundary, null);
			this.locExtender.SetLocalizationPriority(this._groupBoundary, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._groupBoundary, "SearchOptionsDropDown.grpLength");
			this._groupBoundary.Location = new System.Drawing.Point(0, 2);
			this._groupBoundary.Margin = new System.Windows.Forms.Padding(0);
			this._groupBoundary.Name = "_groupBoundary";
			this._groupBoundary.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
			this._groupBoundary.Size = new System.Drawing.Size(333, 60);
			this._groupBoundary.TabIndex = 5;
			this._groupBoundary.TabStop = false;
			// 
			// _pickerBoundary
			// 
			this._pickerBoundary.AutoSize = false;
			this._pickerBoundary.AutoSizeItems = false;
			this._pickerBoundary.BackColor = System.Drawing.Color.Transparent;
			this._pickerBoundary.CheckItemsOnClick = true;
			this._pickerBoundary.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pickerBoundary.FontSize = 14F;
			this._pickerBoundary.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._pickerBoundary.ImageScalingSize = new System.Drawing.Size(20, 20);
			this._pickerBoundary.ItemSize = new System.Drawing.Size(30, 32);
			this._pickerBoundary.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this._pickerBoundary, null);
			this.locExtender.SetLocalizationComment(this._pickerBoundary, null);
			this.locExtender.SetLocalizationPriority(this._pickerBoundary, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pickerBoundary, "SearchOptionsDropDown.lengthPicker");
			this._pickerBoundary.Location = new System.Drawing.Point(9, 24);
			this._pickerBoundary.Name = "_pickerBoundary";
			this._pickerBoundary.Padding = new System.Windows.Forms.Padding(0);
			this._pickerBoundary.Size = new System.Drawing.Size(315, 27);
			this._pickerBoundary.TabIndex = 0;
			this._pickerBoundary.Text = "charPicker1";
			this._pickerBoundary.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// _chkPitchPhonation
			// 
			this._chkPitchPhonation.AutoSize = true;
			this._chkPitchPhonation.BackColor = System.Drawing.Color.Transparent;
			this._chkPitchPhonation.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkPitchPhonation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this._chkPitchPhonation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._chkPitchPhonation, null);
			this.locExtender.SetLocalizationComment(this._chkPitchPhonation, null);
			this.locExtender.SetLocalizingId(this._chkPitchPhonation, "Views.WordLists.SearchResults.SearchOptionsPopup.PitchPhonationCheckbox");
			this._chkPitchPhonation.Location = new System.Drawing.Point(13, 0);
			this._chkPitchPhonation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this._chkPitchPhonation.Name = "_chkPitchPhonation";
			this._chkPitchPhonation.Size = new System.Drawing.Size(180, 22);
			this._chkPitchPhonation.TabIndex = 0;
			this._chkPitchPhonation.Text = "Ignore &Pitch-Phonation";
			this._chkPitchPhonation.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._chkPitchPhonation.ThreeState = true;
			this._chkPitchPhonation.UseVisualStyleBackColor = false;
			this._chkPitchPhonation.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// _groupPitchPhonation
			// 
			this._groupPitchPhonation.Controls.Add(this._pickerPitchPhonation);
			this.locExtender.SetLocalizableToolTip(this._groupPitchPhonation, null);
			this.locExtender.SetLocalizationComment(this._groupPitchPhonation, null);
			this.locExtender.SetLocalizationPriority(this._groupPitchPhonation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._groupPitchPhonation, "SearchOptionsDropDown.grpLength");
			this._groupPitchPhonation.Location = new System.Drawing.Point(0, 2);
			this._groupPitchPhonation.Margin = new System.Windows.Forms.Padding(0);
			this._groupPitchPhonation.Name = "_groupPitchPhonation";
			this._groupPitchPhonation.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
			this._groupPitchPhonation.Size = new System.Drawing.Size(333, 60);
			this._groupPitchPhonation.TabIndex = 6;
			this._groupPitchPhonation.TabStop = false;
			// 
			// _pickerPitchPhonation
			// 
			this._pickerPitchPhonation.AutoSize = false;
			this._pickerPitchPhonation.AutoSizeItems = false;
			this._pickerPitchPhonation.BackColor = System.Drawing.Color.Transparent;
			this._pickerPitchPhonation.CheckItemsOnClick = true;
			this._pickerPitchPhonation.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pickerPitchPhonation.FontSize = 14F;
			this._pickerPitchPhonation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._pickerPitchPhonation.ImageScalingSize = new System.Drawing.Size(20, 20);
			this._pickerPitchPhonation.ItemSize = new System.Drawing.Size(30, 32);
			this._pickerPitchPhonation.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this._pickerPitchPhonation, null);
			this.locExtender.SetLocalizationComment(this._pickerPitchPhonation, null);
			this.locExtender.SetLocalizationPriority(this._pickerPitchPhonation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pickerPitchPhonation, "SearchOptionsDropDown.lengthPicker");
			this._pickerPitchPhonation.Location = new System.Drawing.Point(9, 24);
			this._pickerPitchPhonation.Name = "_pickerPitchPhonation";
			this._pickerPitchPhonation.Padding = new System.Windows.Forms.Padding(0);
			this._pickerPitchPhonation.Size = new System.Drawing.Size(315, 27);
			this._pickerPitchPhonation.TabIndex = 0;
			this._pickerPitchPhonation.Text = "charPicker1";
			this._pickerPitchPhonation.CharPicked += new SIL.Pa.UI.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// lnkCancel
			// 
			this.lnkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.lnkCancel, null);
			this.locExtender.SetLocalizationComment(this.lnkCancel, null);
			this.locExtender.SetLocalizingId(this.lnkCancel, "CommonControls.ChartOptionsPopup.CancelLink");
			this.lnkCancel.Location = new System.Drawing.Point(213, 562);
			this.lnkCancel.Margin = new System.Windows.Forms.Padding(4, 5, 0, 0);
			this.lnkCancel.Name = "lnkCancel";
			this.lnkCancel.Size = new System.Drawing.Size(91, 21);
			this.lnkCancel.TabIndex = 11;
			this.lnkCancel.TabStop = true;
			this.lnkCancel.Text = "Cancel";
			this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleCloseClicked);
			// 
			// lnkOk
			// 
			this.lnkOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.lnkOk, null);
			this.locExtender.SetLocalizationComment(this.lnkOk, null);
			this.locExtender.SetLocalizingId(this.lnkOk, "CommonControls.ChartOptionsPopup.OKLink");
			this.lnkOk.Location = new System.Drawing.Point(162, 562);
			this.lnkOk.Margin = new System.Windows.Forms.Padding(4, 5, 0, 0);
			this.lnkOk.Name = "lnkOk";
			this.lnkOk.Size = new System.Drawing.Size(47, 21);
			this.lnkOk.TabIndex = 10;
			this.lnkOk.TabStop = true;
			this.lnkOk.Text = "OK";
			// 
			// _panelStress
			// 
			this._tableLayout.SetColumnSpan(this._panelStress, 4);
			this._panelStress.Controls.Add(this._chkStress);
			this._panelStress.Controls.Add(this._groupStress);
			this._panelStress.Location = new System.Drawing.Point(0, 63);
			this._panelStress.Margin = new System.Windows.Forms.Padding(0, 5, 0, 4);
			this._panelStress.Name = "_panelStress";
			this._panelStress.Size = new System.Drawing.Size(333, 52);
			this._panelStress.TabIndex = 2;
			// 
			// _panelLength
			// 
			this._tableLayout.SetColumnSpan(this._panelLength, 4);
			this._panelLength.Controls.Add(this._chkLength);
			this._panelLength.Controls.Add(this._groupLength);
			this._panelLength.Location = new System.Drawing.Point(0, 124);
			this._panelLength.Margin = new System.Windows.Forms.Padding(0, 5, 0, 4);
			this._panelLength.Name = "_panelLength";
			this._panelLength.Size = new System.Drawing.Size(333, 64);
			this._panelLength.TabIndex = 3;
			// 
			// _panelTone
			// 
			this._tableLayout.SetColumnSpan(this._panelTone, 4);
			this._panelTone.Controls.Add(this._chkTone);
			this._panelTone.Controls.Add(this._groupTone);
			this._panelTone.Location = new System.Drawing.Point(0, 197);
			this._panelTone.Margin = new System.Windows.Forms.Padding(0, 5, 0, 4);
			this._panelTone.Name = "_panelTone";
			this._panelTone.Size = new System.Drawing.Size(333, 60);
			this._panelTone.TabIndex = 4;
			// 
			// _panelBoundary
			// 
			this._tableLayout.SetColumnSpan(this._panelBoundary, 4);
			this._panelBoundary.Controls.Add(this._chkBoundary);
			this._panelBoundary.Controls.Add(this._groupBoundary);
			this._panelBoundary.Location = new System.Drawing.Point(0, 266);
			this._panelBoundary.Margin = new System.Windows.Forms.Padding(0, 5, 0, 4);
			this._panelBoundary.Name = "_panelBoundary";
			this._panelBoundary.Size = new System.Drawing.Size(333, 63);
			this._panelBoundary.TabIndex = 5;
			// 
			// _panelPitchPhonation
			// 
			this._tableLayout.SetColumnSpan(this._panelPitchPhonation, 4);
			this._panelPitchPhonation.Controls.Add(this._chkPitchPhonation);
			this._panelPitchPhonation.Controls.Add(this._groupPitchPhonation);
			this._panelPitchPhonation.Location = new System.Drawing.Point(0, 338);
			this._panelPitchPhonation.Margin = new System.Windows.Forms.Padding(0, 5, 0, 4);
			this._panelPitchPhonation.Name = "_panelPitchPhonation";
			this._panelPitchPhonation.Size = new System.Drawing.Size(333, 63);
			this._panelPitchPhonation.TabIndex = 5;
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.ColumnCount = 4;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
			this._tableLayout.Controls.Add(this.lnkOk, 1, 11);
			this._tableLayout.Controls.Add(this._chkIgnoreDiacritics, 0, 4);
			this._tableLayout.Controls.Add(this._chkShowAllWords, 0, 3);
			this._tableLayout.Controls.Add(this._panelStress, 0, 5);
			this._tableLayout.Controls.Add(this._panelTone, 0, 7);
			this._tableLayout.Controls.Add(this._panelLength, 0, 6);
			this._tableLayout.Controls.Add(this._groupUncertainties, 0, 10);
			this._tableLayout.Controls.Add(this._linkApplyToAll, 0, 11);
			this._tableLayout.Controls.Add(this._panelBoundary, 0, 8);
			this._tableLayout.Controls.Add(this._linkHelp, 3, 11);
			this._tableLayout.Controls.Add(this._panelPitchPhonation, 0, 9);
			this._tableLayout.Controls.Add(this.lnkCancel, 2, 11);
			this._tableLayout.Location = new System.Drawing.Point(5, 5);
			this._tableLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 11;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 151F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
			this._tableLayout.Size = new System.Drawing.Size(373, 583);
			this._tableLayout.TabIndex = 0;
			// 
			// SearchOptionsDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this._tableLayout);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SearchOptionsDropDown.SearchOptionsDropDown");
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SearchOptionsDropDown";
			this.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
			this.Size = new System.Drawing.Size(377, 594);
			this._groupStress.ResumeLayout(false);
			this._groupTone.ResumeLayout(false);
			this._groupLength.ResumeLayout(false);
			this._groupUncertainties.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._groupBoundary.ResumeLayout(false);
			this._groupPitchPhonation.ResumeLayout(false);
			this._panelStress.ResumeLayout(false);
			this._panelStress.PerformLayout();
			this._panelLength.ResumeLayout(false);
			this._panelLength.PerformLayout();
			this._panelTone.ResumeLayout(false);
			this._panelTone.PerformLayout();
			this._panelBoundary.ResumeLayout(false);
			this._panelBoundary.PerformLayout();
			this._panelPitchPhonation.ResumeLayout(false);
			this._panelPitchPhonation.PerformLayout();
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private CharPicker _pickerTone;
		private CharPicker _pickerStress;
		private CharPicker _pickerLength;
		protected System.Windows.Forms.CheckBox _chkShowAllWords;
		protected System.Windows.Forms.GroupBox _groupStress;
		protected System.Windows.Forms.GroupBox _groupTone;
		protected System.Windows.Forms.GroupBox _groupLength;
		protected System.Windows.Forms.CheckBox _chkIgnoreDiacritics;
		protected System.Windows.Forms.CheckBox _chkStress;
		protected System.Windows.Forms.CheckBox _chkTone;
		protected System.Windows.Forms.CheckBox _chkLength;
		private System.Windows.Forms.LinkLabel _linkApplyToAll;
		public System.Windows.Forms.LinkLabel _linkHelp;
		protected System.Windows.Forms.GroupBox _groupUncertainties;
		private System.Windows.Forms.RadioButton rbAllUncertainties;
		private System.Windows.Forms.RadioButton rbPrimaryOnly;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Panel _panelStress;
		private System.Windows.Forms.Panel _panelLength;
		private System.Windows.Forms.Panel _panelTone;
		private System.Windows.Forms.Panel _panelBoundary;
		protected System.Windows.Forms.CheckBox _chkBoundary;
		protected System.Windows.Forms.GroupBox _groupBoundary;
		private CharPicker _pickerBoundary;
        private System.Windows.Forms.Panel _panelPitchPhonation;
        protected System.Windows.Forms.CheckBox _chkPitchPhonation;
        protected System.Windows.Forms.GroupBox _groupPitchPhonation;
        private CharPicker _pickerPitchPhonation;
        protected System.Windows.Forms.TableLayoutPanel _tableLayout;
        public System.Windows.Forms.LinkLabel lnkCancel;
        public System.Windows.Forms.LinkLabel lnkOk;
	}
}
