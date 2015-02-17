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
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this._chkBoundary = new System.Windows.Forms.CheckBox();
            this._groupBoundary = new System.Windows.Forms.GroupBox();
            this._pickerBoundary = new SIL.Pa.UI.Controls.CharPicker();
            this.lnkCancel = new System.Windows.Forms.LinkLabel();
            this.lnkOk = new System.Windows.Forms.LinkLabel();
            this._panelStress = new System.Windows.Forms.Panel();
            this._panelLength = new System.Windows.Forms.Panel();
            this._panelTone = new System.Windows.Forms.Panel();
            this._panelBoundary = new System.Windows.Forms.Panel();
            this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this._groupStress.SuspendLayout();
            this._groupTone.SuspendLayout();
            this._groupLength.SuspendLayout();
            this._groupUncertainties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this._groupBoundary.SuspendLayout();
            this._panelStress.SuspendLayout();
            this._panelLength.SuspendLayout();
            this._panelTone.SuspendLayout();
            this._panelBoundary.SuspendLayout();
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
            this._chkIgnoreDiacritics.Location = new System.Drawing.Point(10, 27);
            this._chkIgnoreDiacritics.Margin = new System.Windows.Forms.Padding(10, 5, 3, 3);
            this._chkIgnoreDiacritics.Name = "_chkIgnoreDiacritics";
            this._chkIgnoreDiacritics.Size = new System.Drawing.Size(114, 19);
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
            this._chkShowAllWords.Location = new System.Drawing.Point(10, 0);
            this._chkShowAllWords.Margin = new System.Windows.Forms.Padding(10, 0, 3, 3);
            this._chkShowAllWords.Name = "_chkShowAllWords";
            this._chkShowAllWords.Size = new System.Drawing.Size(212, 19);
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
            this._chkStress.Location = new System.Drawing.Point(10, 0);
            this._chkStress.Name = "_chkStress";
            this._chkStress.Size = new System.Drawing.Size(98, 19);
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
            this.locExtender.SetLocalizationPriority(this._groupStress, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._groupStress, "SearchOptionsDropDown.grpStress");
            this._groupStress.Location = new System.Drawing.Point(0, 2);
            this._groupStress.Margin = new System.Windows.Forms.Padding(0);
            this._groupStress.Name = "_groupStress";
            this._groupStress.Padding = new System.Windows.Forms.Padding(7);
            this._groupStress.Size = new System.Drawing.Size(250, 40);
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
            this._pickerStress.ItemSize = new System.Drawing.Size(30, 32);
            this._pickerStress.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.locExtender.SetLocalizableToolTip(this._pickerStress, null);
            this.locExtender.SetLocalizationComment(this._pickerStress, null);
            this.locExtender.SetLocalizationPriority(this._pickerStress, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._pickerStress, "SearchOptionsDropDown.stressPicker");
            this._pickerStress.Location = new System.Drawing.Point(7, 20);
            this._pickerStress.Name = "_pickerStress";
            this._pickerStress.Padding = new System.Windows.Forms.Padding(0);
            this._pickerStress.Size = new System.Drawing.Size(236, 13);
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
            this._chkTone.Location = new System.Drawing.Point(10, 0);
            this._chkTone.Name = "_chkTone";
            this._chkTone.Size = new System.Drawing.Size(92, 19);
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
            this.locExtender.SetLocalizationPriority(this._groupTone, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._groupTone, "SearchOptionsDropDown.grpTone");
            this._groupTone.Location = new System.Drawing.Point(0, 2);
            this._groupTone.Margin = new System.Windows.Forms.Padding(0);
            this._groupTone.Name = "_groupTone";
            this._groupTone.Padding = new System.Windows.Forms.Padding(7);
            this._groupTone.Size = new System.Drawing.Size(250, 47);
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
            this._pickerTone.ItemSize = new System.Drawing.Size(30, 32);
            this._pickerTone.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.locExtender.SetLocalizableToolTip(this._pickerTone, null);
            this.locExtender.SetLocalizationComment(this._pickerTone, null);
            this.locExtender.SetLocalizationPriority(this._pickerTone, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._pickerTone, "SearchOptionsDropDown.tonePicker");
            this._pickerTone.Location = new System.Drawing.Point(7, 20);
            this._pickerTone.Name = "_pickerTone";
            this._pickerTone.Padding = new System.Windows.Forms.Padding(0);
            this._pickerTone.Size = new System.Drawing.Size(236, 20);
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
            this._chkLength.Location = new System.Drawing.Point(10, 0);
            this._chkLength.Name = "_chkLength";
            this._chkLength.Size = new System.Drawing.Size(102, 19);
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
            this.locExtender.SetLocalizationPriority(this._groupLength, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._groupLength, "SearchOptionsDropDown.grpLength");
            this._groupLength.Location = new System.Drawing.Point(0, 3);
            this._groupLength.Margin = new System.Windows.Forms.Padding(0);
            this._groupLength.Name = "_groupLength";
            this._groupLength.Padding = new System.Windows.Forms.Padding(7);
            this._groupLength.Size = new System.Drawing.Size(250, 49);
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
            this._pickerLength.ItemSize = new System.Drawing.Size(30, 32);
            this._pickerLength.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.locExtender.SetLocalizableToolTip(this._pickerLength, null);
            this.locExtender.SetLocalizationComment(this._pickerLength, null);
            this.locExtender.SetLocalizationPriority(this._pickerLength, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._pickerLength, "SearchOptionsDropDown.lengthPicker");
            this._pickerLength.Location = new System.Drawing.Point(7, 20);
            this._pickerLength.Name = "_pickerLength";
            this._pickerLength.Padding = new System.Windows.Forms.Padding(0);
            this._pickerLength.Size = new System.Drawing.Size(236, 22);
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
            this._linkApplyToAll.Location = new System.Drawing.Point(0, 409);
            this._linkApplyToAll.Margin = new System.Windows.Forms.Padding(0, 4, 3, 0);
            this._linkApplyToAll.Name = "_linkApplyToAll";
            this._linkApplyToAll.Size = new System.Drawing.Size(115, 15);
            this._linkApplyToAll.TabIndex = 7;
            this._linkApplyToAll.TabStop = true;
            this._linkApplyToAll.Text = "Apply to all columns";
            // 
            // _linkHelp
            // 
            this._linkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._linkHelp.AutoSize = true;
            this._linkHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this._linkHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this._linkHelp, null);
            this.locExtender.SetLocalizationComment(this._linkHelp, null);
            this.locExtender.SetLocalizingId(this._linkHelp, "Views.WordLists.SearchResults.SearchOptionsPopup.HelpLink");
            this._linkHelp.Location = new System.Drawing.Point(239, 409);
            this._linkHelp.Margin = new System.Windows.Forms.Padding(3, 4, 0, 0);
            this._linkHelp.Name = "_linkHelp";
            this._linkHelp.Size = new System.Drawing.Size(33, 15);
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
            this._groupUncertainties.Location = new System.Drawing.Point(0, 277);
            this._groupUncertainties.Margin = new System.Windows.Forms.Padding(0, 6, 0, 3);
            this._groupUncertainties.Name = "_groupUncertainties";
            this._groupUncertainties.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
            this._groupUncertainties.Size = new System.Drawing.Size(250, 114);
            this._groupUncertainties.TabIndex = 6;
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
            this.rbAllUncertainties.Location = new System.Drawing.Point(10, 59);
            this.rbAllUncertainties.Name = "rbAllUncertainties";
            this.rbAllUncertainties.Size = new System.Drawing.Size(233, 52);
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
            this.rbPrimaryOnly.Location = new System.Drawing.Point(10, 21);
            this.rbPrimaryOnly.Name = "rbPrimaryOnly";
            this.rbPrimaryOnly.Size = new System.Drawing.Size(233, 39);
            this.rbPrimaryOnly.TabIndex = 0;
            this.rbPrimaryOnly.TabStop = true;
            this.rbPrimaryOnly.Text = "Search Transcriptions Derived Only from &Primary Uncertainties";
            this.rbPrimaryOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbPrimaryOnly.UseVisualStyleBackColor = true;
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
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
            this._chkBoundary.Location = new System.Drawing.Point(10, 0);
            this._chkBoundary.Name = "_chkBoundary";
            this._chkBoundary.Size = new System.Drawing.Size(116, 19);
            this._chkBoundary.TabIndex = 0;
            this._chkBoundary.Text = "Ignore &Boundary";
            this._chkBoundary.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this._chkBoundary.ThreeState = true;
            this._chkBoundary.UseVisualStyleBackColor = false;
            // 
            // _groupBoundary
            // 
            this._groupBoundary.Controls.Add(this._pickerBoundary);
            this.locExtender.SetLocalizableToolTip(this._groupBoundary, null);
            this.locExtender.SetLocalizationComment(this._groupBoundary, null);
            this.locExtender.SetLocalizationPriority(this._groupBoundary, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._groupBoundary, "SearchOptionsDropDown.grpLength");
            this._groupBoundary.Location = new System.Drawing.Point(0, 2);
            this._groupBoundary.Margin = new System.Windows.Forms.Padding(0);
            this._groupBoundary.Name = "_groupBoundary";
            this._groupBoundary.Padding = new System.Windows.Forms.Padding(7);
            this._groupBoundary.Size = new System.Drawing.Size(250, 49);
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
            this._pickerBoundary.ItemSize = new System.Drawing.Size(30, 32);
            this._pickerBoundary.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.locExtender.SetLocalizableToolTip(this._pickerBoundary, null);
            this.locExtender.SetLocalizationComment(this._pickerBoundary, null);
            this.locExtender.SetLocalizationPriority(this._pickerBoundary, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._pickerBoundary, "SearchOptionsDropDown.lengthPicker");
            this._pickerBoundary.Location = new System.Drawing.Point(7, 20);
            this._pickerBoundary.Name = "_pickerBoundary";
            this._pickerBoundary.Padding = new System.Windows.Forms.Padding(0);
            this._pickerBoundary.Size = new System.Drawing.Size(236, 22);
            this._pickerBoundary.TabIndex = 0;
            this._pickerBoundary.Text = "charPicker1";
            // 
            // lnkCancel
            // 
            this.lnkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkCancel.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.lnkCancel, null);
            this.locExtender.SetLocalizationComment(this.lnkCancel, null);
            this.locExtender.SetLocalizingId(this.lnkCancel, "CommonControls.ChartOptionsPopup.CancelLink");
            this.lnkCancel.Location = new System.Drawing.Point(192, 411);
            this.lnkCancel.Margin = new System.Windows.Forms.Padding(3, 4, 0, 0);
            this.lnkCancel.Name = "lnkCancel";
            this.lnkCancel.Size = new System.Drawing.Size(40, 13);
            this.lnkCancel.TabIndex = 10;
            this.lnkCancel.TabStop = true;
            this.lnkCancel.Text = "Cancel";
            this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleCloseClicked);
            // 
            // lnkOk
            // 
            this.lnkOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkOk.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.lnkOk, null);
            this.locExtender.SetLocalizationComment(this.lnkOk, null);
            this.locExtender.SetLocalizingId(this.lnkOk, "CommonControls.ChartOptionsPopup.OKLink");
            this.lnkOk.Location = new System.Drawing.Point(160, 411);
            this.lnkOk.Margin = new System.Windows.Forms.Padding(3, 4, 0, 0);
            this.lnkOk.Name = "lnkOk";
            this.lnkOk.Size = new System.Drawing.Size(22, 13);
            this.lnkOk.TabIndex = 9;
            this.lnkOk.TabStop = true;
            this.lnkOk.Text = "OK";
            // 
            // _panelStress
            // 
            this._tableLayout.SetColumnSpan(this._panelStress, 4);
            this._panelStress.Controls.Add(this._chkStress);
            this._panelStress.Controls.Add(this._groupStress);
            this._panelStress.Location = new System.Drawing.Point(0, 53);
            this._panelStress.Margin = new System.Windows.Forms.Padding(0, 4, 0, 3);
            this._panelStress.Name = "_panelStress";
            this._panelStress.Size = new System.Drawing.Size(250, 42);
            this._panelStress.TabIndex = 2;
            // 
            // _panelLength
            // 
            this._tableLayout.SetColumnSpan(this._panelLength, 4);
            this._panelLength.Controls.Add(this._chkLength);
            this._panelLength.Controls.Add(this._groupLength);
            this._panelLength.Location = new System.Drawing.Point(0, 102);
            this._panelLength.Margin = new System.Windows.Forms.Padding(0, 4, 0, 3);
            this._panelLength.Name = "_panelLength";
            this._panelLength.Size = new System.Drawing.Size(250, 52);
            this._panelLength.TabIndex = 3;
            // 
            // _panelTone
            // 
            this._tableLayout.SetColumnSpan(this._panelTone, 4);
            this._panelTone.Controls.Add(this._chkTone);
            this._panelTone.Controls.Add(this._groupTone);
            this._panelTone.Location = new System.Drawing.Point(0, 161);
            this._panelTone.Margin = new System.Windows.Forms.Padding(0, 4, 0, 3);
            this._panelTone.Name = "_panelTone";
            this._panelTone.Size = new System.Drawing.Size(250, 49);
            this._panelTone.TabIndex = 4;
            // 
            // _panelBoundary
            // 
            this._tableLayout.SetColumnSpan(this._panelBoundary, 4);
            this._panelBoundary.Controls.Add(this._chkBoundary);
            this._panelBoundary.Controls.Add(this._groupBoundary);
            this._panelBoundary.Location = new System.Drawing.Point(0, 217);
            this._panelBoundary.Margin = new System.Windows.Forms.Padding(0, 4, 0, 3);
            this._panelBoundary.Name = "_panelBoundary";
            this._panelBoundary.Size = new System.Drawing.Size(250, 51);
            this._panelBoundary.TabIndex = 5;
            // 
            // _tableLayout
            // 
            this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayout.ColumnCount = 4;
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._tableLayout.Controls.Add(this.lnkCancel, 2, 10);
            this._tableLayout.Controls.Add(this.lnkOk, 1, 10);
            this._tableLayout.Controls.Add(this._chkIgnoreDiacritics, 0, 4);
            this._tableLayout.Controls.Add(this._chkShowAllWords, 0, 3);
            this._tableLayout.Controls.Add(this._panelStress, 0, 5);
            this._tableLayout.Controls.Add(this._panelTone, 0, 7);
            this._tableLayout.Controls.Add(this._panelLength, 0, 6);
            this._tableLayout.Controls.Add(this._groupUncertainties, 0, 9);
            this._tableLayout.Controls.Add(this._linkApplyToAll, 0, 10);
            this._tableLayout.Controls.Add(this._panelBoundary, 0, 8);
            this._tableLayout.Controls.Add(this._linkHelp, 3, 10);
            this._tableLayout.Location = new System.Drawing.Point(4, 4);
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
            this._tableLayout.Size = new System.Drawing.Size(272, 424);
            this._tableLayout.TabIndex = 0;
            // 
            // SearchOptionsDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this._tableLayout);
            this.DoubleBuffered = true;
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this, "SearchOptionsDropDown.SearchOptionsDropDown");
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SearchOptionsDropDown";
            this.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.Size = new System.Drawing.Size(283, 442);
            this._groupStress.ResumeLayout(false);
            this._groupTone.ResumeLayout(false);
            this._groupLength.ResumeLayout(false);
            this._groupUncertainties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this._groupBoundary.ResumeLayout(false);
            this._panelStress.ResumeLayout(false);
            this._panelStress.PerformLayout();
            this._panelLength.ResumeLayout(false);
            this._panelLength.PerformLayout();
            this._panelTone.ResumeLayout(false);
            this._panelTone.PerformLayout();
            this._panelBoundary.ResumeLayout(false);
            this._panelBoundary.PerformLayout();
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
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Panel _panelStress;
		private System.Windows.Forms.Panel _panelLength;
		private System.Windows.Forms.Panel _panelTone;
		private System.Windows.Forms.Panel _panelBoundary;
		protected System.Windows.Forms.CheckBox _chkBoundary;
		protected System.Windows.Forms.GroupBox _groupBoundary;
		private CharPicker _pickerBoundary;
        protected System.Windows.Forms.TableLayoutPanel _tableLayout;
        public System.Windows.Forms.LinkLabel lnkCancel;
        public System.Windows.Forms.LinkLabel lnkOk;
	}
}
