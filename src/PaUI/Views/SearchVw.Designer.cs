using SIL.Pa.UI.Controls;
using SilUtils.Controls;

namespace SIL.Pa.UI.Views
{
	partial class SearchVw
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchVw));
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitSideBarOuter = new System.Windows.Forms.SplitContainer();
			this.pnlTabClassDef = new System.Windows.Forms.Panel();
			this.pnlSideBarCaption = new SilUtils.Controls.SilGradientPanel();
			this.splitSideBarInner = new System.Windows.Forms.SplitContainer();
			this.pnlRecentPatterns = new SilUtils.Controls.SilPanel();
			this.lstRecentPatterns = new System.Windows.Forms.ListBox();
			this.pnlSavedPatterns = new SilUtils.Controls.SilPanel();
			this.splitResults = new System.Windows.Forms.SplitContainer();
			this.pnlRecView = new SilUtils.Controls.SilPanel();
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.btnRefresh = new System.Windows.Forms.Button();
			this.pnlSliderPlaceholder = new System.Windows.Forms.Panel();
			this.pnlCurrPattern = new System.Windows.Forms.Panel();
			this.lblCurrPattern = new System.Windows.Forms.Label();
			this.pnlOuter = new System.Windows.Forms.Panel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.ptrnBldrComponent = new SIL.Pa.UI.Controls.PatternBuilderComponents();
			this.btnDock = new SIL.Pa.UI.Controls.AutoHideDockButton();
			this.btnAutoHide = new SIL.Pa.UI.Controls.AutoHideDockButton();
			this.hlblRecentPatterns = new SIL.Pa.UI.Controls.HeaderLabel();
			this.btnClearRecentList = new SIL.Pa.UI.Controls.XButton();
			this.btnRemoveFromRecentList = new SIL.Pa.UI.Controls.XButton();
			this.tvSavedPatterns = new SIL.Pa.UI.Controls.SearchPatternTreeView();
			this.hlblSavedPatterns = new SIL.Pa.UI.Controls.HeaderLabel();
			this.btnCategoryNew = new SIL.Pa.UI.Controls.XButton();
			this.btnCategoryCut = new SIL.Pa.UI.Controls.XButton();
			this.btnCategoryPaste = new SIL.Pa.UI.Controls.XButton();
			this.btnCategoryCopy = new SIL.Pa.UI.Controls.XButton();
			this.rtfRecVw = new SIL.Pa.UI.Controls.RtfRecordView();
			this.ptrnTextBox = new SIL.Pa.UI.Controls.PatternTextBox();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitSideBarOuter.Panel1.SuspendLayout();
			this.splitSideBarOuter.Panel2.SuspendLayout();
			this.splitSideBarOuter.SuspendLayout();
			this.pnlTabClassDef.SuspendLayout();
			this.pnlSideBarCaption.SuspendLayout();
			this.splitSideBarInner.Panel1.SuspendLayout();
			this.splitSideBarInner.Panel2.SuspendLayout();
			this.splitSideBarInner.SuspendLayout();
			this.pnlRecentPatterns.SuspendLayout();
			this.pnlSavedPatterns.SuspendLayout();
			this.splitResults.Panel2.SuspendLayout();
			this.splitResults.SuspendLayout();
			this.pnlRecView.SuspendLayout();
			this.pnlCurrPattern.SuspendLayout();
			this.pnlOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.hlblRecentPatterns.SuspendLayout();
			this.hlblSavedPatterns.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.splitSideBarOuter);
			resources.ApplyResources(this.splitOuter.Panel1, "splitOuter.Panel1");
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitResults);
			this.splitOuter.TabStop = false;
			// 
			// splitSideBarOuter
			// 
			resources.ApplyResources(this.splitSideBarOuter, "splitSideBarOuter");
			this.splitSideBarOuter.Name = "splitSideBarOuter";
			// 
			// splitSideBarOuter.Panel1
			// 
			this.splitSideBarOuter.Panel1.Controls.Add(this.pnlTabClassDef);
			this.splitSideBarOuter.Panel1.Controls.Add(this.pnlSideBarCaption);
			// 
			// splitSideBarOuter.Panel2
			// 
			this.splitSideBarOuter.Panel2.Controls.Add(this.splitSideBarInner);
			this.splitSideBarOuter.TabStop = false;
			// 
			// pnlTabClassDef
			// 
			this.pnlTabClassDef.Controls.Add(this.ptrnBldrComponent);
			resources.ApplyResources(this.pnlTabClassDef, "pnlTabClassDef");
			this.pnlTabClassDef.Name = "pnlTabClassDef";
			// 
			// pnlSideBarCaption
			// 
			this.pnlSideBarCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSideBarCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSideBarCaption.ClipTextForChildControls = true;
			this.pnlSideBarCaption.ControlReceivingFocusOnMnemonic = null;
			this.pnlSideBarCaption.Controls.Add(this.btnDock);
			this.pnlSideBarCaption.Controls.Add(this.btnAutoHide);
			resources.ApplyResources(this.pnlSideBarCaption, "pnlSideBarCaption");
			this.pnlSideBarCaption.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlSideBarCaption, null);
			this.locExtender.SetLocalizationComment(this.pnlSideBarCaption, "Caption at the top of  the side bar in the Search view.");
			this.locExtender.SetLocalizingId(this.pnlSideBarCaption, "SearchVw.pnlSideBarCaption");
			this.pnlSideBarCaption.MakeDark = false;
			this.pnlSideBarCaption.MnemonicGeneratesClick = false;
			this.pnlSideBarCaption.Name = "pnlSideBarCaption";
			this.pnlSideBarCaption.PaintExplorerBarBackground = false;
			// 
			// splitSideBarInner
			// 
			resources.ApplyResources(this.splitSideBarInner, "splitSideBarInner");
			this.splitSideBarInner.Name = "splitSideBarInner";
			// 
			// splitSideBarInner.Panel1
			// 
			this.splitSideBarInner.Panel1.Controls.Add(this.pnlRecentPatterns);
			// 
			// splitSideBarInner.Panel2
			// 
			this.splitSideBarInner.Panel2.Controls.Add(this.pnlSavedPatterns);
			this.splitSideBarInner.TabStop = false;
			// 
			// pnlRecentPatterns
			// 
			this.pnlRecentPatterns.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlRecentPatterns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlRecentPatterns.ClipTextForChildControls = true;
			this.pnlRecentPatterns.ControlReceivingFocusOnMnemonic = null;
			this.pnlRecentPatterns.Controls.Add(this.lstRecentPatterns);
			this.pnlRecentPatterns.Controls.Add(this.hlblRecentPatterns);
			resources.ApplyResources(this.pnlRecentPatterns, "pnlRecentPatterns");
			this.pnlRecentPatterns.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlRecentPatterns, null);
			this.locExtender.SetLocalizationComment(this.pnlRecentPatterns, null);
			this.locExtender.SetLocalizationPriority(this.pnlRecentPatterns, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlRecentPatterns, "SearchVw.pnlRecentPatterns");
			this.pnlRecentPatterns.MnemonicGeneratesClick = false;
			this.pnlRecentPatterns.Name = "pnlRecentPatterns";
			this.pnlRecentPatterns.PaintExplorerBarBackground = false;
			// 
			// lstRecentPatterns
			// 
			this.lstRecentPatterns.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.lstRecentPatterns, "lstRecentPatterns");
			this.lstRecentPatterns.FormattingEnabled = true;
			this.lstRecentPatterns.Name = "lstRecentPatterns";
			this.lstRecentPatterns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstRecentPatterns_MouseUp);
			this.lstRecentPatterns.Enter += new System.EventHandler(this.lstRecentPatterns_Enter);
			this.lstRecentPatterns.DoubleClick += new System.EventHandler(this.lstRecentPatterns_DoubleClick);
			this.lstRecentPatterns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstRecentPatterns_MouseMove);
			this.lstRecentPatterns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstRecentPatterns_MouseDown);
			this.lstRecentPatterns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstRecentPatterns_KeyDown);
			// 
			// pnlSavedPatterns
			// 
			this.pnlSavedPatterns.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSavedPatterns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSavedPatterns.ClipTextForChildControls = true;
			this.pnlSavedPatterns.ControlReceivingFocusOnMnemonic = null;
			this.pnlSavedPatterns.Controls.Add(this.tvSavedPatterns);
			this.pnlSavedPatterns.Controls.Add(this.hlblSavedPatterns);
			resources.ApplyResources(this.pnlSavedPatterns, "pnlSavedPatterns");
			this.pnlSavedPatterns.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlSavedPatterns, null);
			this.locExtender.SetLocalizationComment(this.pnlSavedPatterns, null);
			this.locExtender.SetLocalizationPriority(this.pnlSavedPatterns, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSavedPatterns, "SearchVw.pnlSavedPatterns");
			this.pnlSavedPatterns.MnemonicGeneratesClick = false;
			this.pnlSavedPatterns.Name = "pnlSavedPatterns";
			this.pnlSavedPatterns.PaintExplorerBarBackground = false;
			// 
			// splitResults
			// 
			resources.ApplyResources(this.splitResults, "splitResults");
			this.splitResults.Name = "splitResults";
			// 
			// splitResults.Panel1
			// 
			this.splitResults.Panel1.AllowDrop = true;
			this.splitResults.Panel1.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.splitResults.Panel1, "splitResults.Panel1");
			this.splitResults.Panel1.Tag = "";
			this.splitResults.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitResults_Panel1_Paint);
			this.splitResults.Panel1.DragOver += new System.Windows.Forms.DragEventHandler(this.splitResults_Panel1_DragOver);
			this.splitResults.Panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.splitResults_Panel1_DragDrop);
			this.splitResults.Panel1.SizeChanged += new System.EventHandler(this.splitResults_Panel1_SizeChanged);
			// 
			// splitResults.Panel2
			// 
			this.splitResults.Panel2.Controls.Add(this.pnlRecView);
			// 
			// pnlRecView
			// 
			this.pnlRecView.BackColor = System.Drawing.SystemColors.Window;
			this.pnlRecView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlRecView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlRecView.ClipTextForChildControls = true;
			this.pnlRecView.ControlReceivingFocusOnMnemonic = null;
			this.pnlRecView.Controls.Add(this.rtfRecVw);
			resources.ApplyResources(this.pnlRecView, "pnlRecView");
			this.pnlRecView.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlRecView, null);
			this.locExtender.SetLocalizationComment(this.pnlRecView, null);
			this.locExtender.SetLocalizationPriority(this.pnlRecView, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlRecView, "SearchVw.pnlRecView");
			this.pnlRecView.MnemonicGeneratesClick = false;
			this.pnlRecView.Name = "pnlRecView";
			this.pnlRecView.PaintExplorerBarBackground = false;
			// 
			// btnRefresh
			// 
			resources.ApplyResources(this.btnRefresh, "btnRefresh");
			this.btnRefresh.Image = global::SIL.Pa.Properties.Resources.kimidRefresh;
			this.locExtender.SetLocalizableToolTip(this.btnRefresh, "Refresh Results");
			this.locExtender.SetLocalizationComment(this.btnRefresh, "Button to refresh search results. The button is to the right of the search patter" +
					"n text box at the top of the search view.");
			this.locExtender.SetLocalizationPriority(this.btnRefresh, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnRefresh, "SearchVw.btnRefresh");
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// pnlSliderPlaceholder
			// 
			resources.ApplyResources(this.pnlSliderPlaceholder, "pnlSliderPlaceholder");
			this.pnlSliderPlaceholder.Name = "pnlSliderPlaceholder";
			// 
			// pnlCurrPattern
			// 
			this.pnlCurrPattern.Controls.Add(this.btnRefresh);
			this.pnlCurrPattern.Controls.Add(this.ptrnTextBox);
			this.pnlCurrPattern.Controls.Add(this.lblCurrPattern);
			resources.ApplyResources(this.pnlCurrPattern, "pnlCurrPattern");
			this.pnlCurrPattern.Name = "pnlCurrPattern";
			this.pnlCurrPattern.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCurrPattern_Paint);
			this.pnlCurrPattern.Resize += new System.EventHandler(this.pnlCurrPattern_Resize);
			// 
			// lblCurrPattern
			// 
			resources.ApplyResources(this.lblCurrPattern, "lblCurrPattern");
			this.lblCurrPattern.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblCurrPattern, null);
			this.locExtender.SetLocalizationComment(this.lblCurrPattern, "Text next to search pattern text box in search view.");
			this.locExtender.SetLocalizingId(this.lblCurrPattern, "SearchVw.lblCurrPattern");
			this.lblCurrPattern.Name = "lblCurrPattern";
			// 
			// pnlOuter
			// 
			this.pnlOuter.Controls.Add(this.splitOuter);
			resources.ApplyResources(this.pnlOuter, "pnlOuter");
			this.pnlOuter.Name = "pnlOuter";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			// 
			// ptrnBldrComponent
			// 
			resources.ApplyResources(this.ptrnBldrComponent, "ptrnBldrComponent");
			this.locExtender.SetLocalizableToolTip(this.ptrnBldrComponent, null);
			this.locExtender.SetLocalizationComment(this.ptrnBldrComponent, null);
			this.locExtender.SetLocalizationPriority(this.ptrnBldrComponent, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.ptrnBldrComponent, "SearchVw.PatternBuilderComponents");
			this.ptrnBldrComponent.Name = "ptrnBldrComponent";
			// 
			// btnDock
			// 
			resources.ApplyResources(this.btnDock, "btnDock");
			this.btnDock.BackColor = System.Drawing.Color.Transparent;
			this.btnDock.CanBeChecked = false;
			this.btnDock.Checked = false;
			this.btnDock.DrawEmpty = false;
			this.btnDock.DrawLeftArrowButton = false;
			this.btnDock.DrawRightArrowButton = false;
			this.locExtender.SetLocalizableToolTip(this.btnDock, "Dock");
			this.locExtender.SetLocalizationComment(this.btnDock, "For docking button on the side bar in search view.");
			this.locExtender.SetLocalizationPriority(this.btnDock, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnDock, "SearchVw.btnDock");
			this.btnDock.Name = "btnDock";
			this.btnDock.Click += new System.EventHandler(this.btnDock_Click);
			// 
			// btnAutoHide
			// 
			resources.ApplyResources(this.btnAutoHide, "btnAutoHide");
			this.btnAutoHide.BackColor = System.Drawing.Color.Transparent;
			this.btnAutoHide.CanBeChecked = false;
			this.btnAutoHide.Checked = false;
			this.btnAutoHide.DrawEmpty = false;
			this.btnAutoHide.DrawLeftArrowButton = false;
			this.btnAutoHide.DrawRightArrowButton = false;
			this.locExtender.SetLocalizableToolTip(this.btnAutoHide, "Automatically Hide");
			this.locExtender.SetLocalizationComment(this.btnAutoHide, "For docking button on the side bar in search view.");
			this.locExtender.SetLocalizationPriority(this.btnAutoHide, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnAutoHide, "SearchVw.btnAutoHide");
			this.btnAutoHide.Name = "btnAutoHide";
			this.btnAutoHide.Click += new System.EventHandler(this.btnAutoHide_Click);
			// 
			// hlblRecentPatterns
			// 
			this.hlblRecentPatterns.ClipTextForChildControls = true;
			this.hlblRecentPatterns.ControlReceivingFocusOnMnemonic = null;
			this.hlblRecentPatterns.Controls.Add(this.btnClearRecentList);
			this.hlblRecentPatterns.Controls.Add(this.btnRemoveFromRecentList);
			resources.ApplyResources(this.hlblRecentPatterns, "hlblRecentPatterns");
			this.locExtender.SetLocalizableToolTip(this.hlblRecentPatterns, null);
			this.locExtender.SetLocalizationComment(this.hlblRecentPatterns, "Heading above list of recent patterns in search view.");
			this.locExtender.SetLocalizingId(this.hlblRecentPatterns, "SearchVw.hlblRecentPatterns");
			this.hlblRecentPatterns.MnemonicGeneratesClick = false;
			this.hlblRecentPatterns.Name = "hlblRecentPatterns";
			this.hlblRecentPatterns.ShowWindowBackgroudOnTopAndRightEdge = true;
			// 
			// btnClearRecentList
			// 
			resources.ApplyResources(this.btnClearRecentList, "btnClearRecentList");
			this.btnClearRecentList.BackColor = System.Drawing.Color.Transparent;
			this.btnClearRecentList.CanBeChecked = false;
			this.btnClearRecentList.Checked = false;
			this.btnClearRecentList.DrawEmpty = false;
			this.btnClearRecentList.DrawLeftArrowButton = false;
			this.btnClearRecentList.DrawRightArrowButton = false;
			this.btnClearRecentList.Image = global::SIL.Pa.Properties.Resources.kimidClearList;
			this.locExtender.SetLocalizableToolTip(this.btnClearRecentList, "Clear List");
			this.locExtender.SetLocalizationComment(this.btnClearRecentList, null);
			this.locExtender.SetLocalizationPriority(this.btnClearRecentList, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnClearRecentList, "SearchVw.btnClearRecentList");
			this.btnClearRecentList.Name = "btnClearRecentList";
			this.btnClearRecentList.Click += new System.EventHandler(this.btnClearRecentList_Click);
			// 
			// btnRemoveFromRecentList
			// 
			resources.ApplyResources(this.btnRemoveFromRecentList, "btnRemoveFromRecentList");
			this.btnRemoveFromRecentList.BackColor = System.Drawing.Color.Transparent;
			this.btnRemoveFromRecentList.CanBeChecked = false;
			this.btnRemoveFromRecentList.Checked = false;
			this.btnRemoveFromRecentList.DrawEmpty = false;
			this.btnRemoveFromRecentList.DrawLeftArrowButton = false;
			this.btnRemoveFromRecentList.DrawRightArrowButton = false;
			this.btnRemoveFromRecentList.Image = global::SIL.Pa.Properties.Resources.kimidDelete;
			this.locExtender.SetLocalizableToolTip(this.btnRemoveFromRecentList, "Remove Selected Pattern");
			this.locExtender.SetLocalizationComment(this.btnRemoveFromRecentList, null);
			this.locExtender.SetLocalizationPriority(this.btnRemoveFromRecentList, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnRemoveFromRecentList, "SearchVw.btnRemoveFromRecentList");
			this.btnRemoveFromRecentList.Name = "btnRemoveFromRecentList";
			this.btnRemoveFromRecentList.Click += new System.EventHandler(this.btnRemoveFromRecentList_Click);
			// 
			// tvSavedPatterns
			// 
			this.tvSavedPatterns.AllowDataModifications = true;
			this.tvSavedPatterns.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tvSavedPatterns, "tvSavedPatterns");
			this.tvSavedPatterns.HideSelection = false;
			this.tvSavedPatterns.IsForToolbarPopup = false;
			this.locExtender.SetLocalizableToolTip(this.tvSavedPatterns, null);
			this.locExtender.SetLocalizationComment(this.tvSavedPatterns, null);
			this.locExtender.SetLocalizationPriority(this.tvSavedPatterns, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tvSavedPatterns, "SearchVw.tvSavedPatterns");
			this.tvSavedPatterns.Name = "tvSavedPatterns";
			this.tvSavedPatterns.TMAdapter = null;
			this.tvSavedPatterns.DoubleClick += new System.EventHandler(this.tvSavedPatterns_DoubleClick);
			this.tvSavedPatterns.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvSavedPatterns_KeyPress);
			// 
			// hlblSavedPatterns
			// 
			this.hlblSavedPatterns.ClipTextForChildControls = true;
			this.hlblSavedPatterns.ControlReceivingFocusOnMnemonic = null;
			this.hlblSavedPatterns.Controls.Add(this.btnCategoryNew);
			this.hlblSavedPatterns.Controls.Add(this.btnCategoryCut);
			this.hlblSavedPatterns.Controls.Add(this.btnCategoryPaste);
			this.hlblSavedPatterns.Controls.Add(this.btnCategoryCopy);
			resources.ApplyResources(this.hlblSavedPatterns, "hlblSavedPatterns");
			this.locExtender.SetLocalizableToolTip(this.hlblSavedPatterns, null);
			this.locExtender.SetLocalizationComment(this.hlblSavedPatterns, "Heading above list of saved patterns in search  view.");
			this.locExtender.SetLocalizingId(this.hlblSavedPatterns, "SearchVw.hlblSavedPatterns");
			this.hlblSavedPatterns.MnemonicGeneratesClick = false;
			this.hlblSavedPatterns.Name = "hlblSavedPatterns";
			this.hlblSavedPatterns.ShowWindowBackgroudOnTopAndRightEdge = true;
			// 
			// btnCategoryNew
			// 
			resources.ApplyResources(this.btnCategoryNew, "btnCategoryNew");
			this.btnCategoryNew.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryNew.CanBeChecked = false;
			this.btnCategoryNew.Checked = false;
			this.btnCategoryNew.DrawEmpty = false;
			this.btnCategoryNew.DrawLeftArrowButton = false;
			this.btnCategoryNew.DrawRightArrowButton = false;
			this.btnCategoryNew.Image = global::SIL.Pa.Properties.Resources.kimidNewPatternCategory;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryNew, "Create New Saved Pattern Category");
			this.locExtender.SetLocalizationComment(this.btnCategoryNew, "Button to create a new saved patterns folder. The button is in the heading above " +
					"the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryNew, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryNew, "SearchVw.btnCategoryNew");
			this.btnCategoryNew.Name = "btnCategoryNew";
			this.btnCategoryNew.Click += new System.EventHandler(this.btnCategoryNew_Click);
			// 
			// btnCategoryCut
			// 
			resources.ApplyResources(this.btnCategoryCut, "btnCategoryCut");
			this.btnCategoryCut.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryCut.CanBeChecked = false;
			this.btnCategoryCut.Checked = false;
			this.btnCategoryCut.DrawEmpty = false;
			this.btnCategoryCut.DrawLeftArrowButton = false;
			this.btnCategoryCut.DrawRightArrowButton = false;
			this.btnCategoryCut.Image = global::SIL.Pa.Properties.Resources.kimidCut;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryCut, "Cut Saved Pattern");
			this.locExtender.SetLocalizationComment(this.btnCategoryCut, "Button to cut a saved pattern to the clipboard. The button is in the heading abov" +
					"e the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryCut, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryCut, "SearchVw.btnCategoryCut");
			this.btnCategoryCut.Name = "btnCategoryCut";
			this.btnCategoryCut.Click += new System.EventHandler(this.btnCategoryCut_Click);
			// 
			// btnCategoryPaste
			// 
			resources.ApplyResources(this.btnCategoryPaste, "btnCategoryPaste");
			this.btnCategoryPaste.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryPaste.CanBeChecked = false;
			this.btnCategoryPaste.Checked = false;
			this.btnCategoryPaste.DrawEmpty = false;
			this.btnCategoryPaste.DrawLeftArrowButton = false;
			this.btnCategoryPaste.DrawRightArrowButton = false;
			this.btnCategoryPaste.Image = global::SIL.Pa.Properties.Resources.kimidPaste;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryPaste, "Paste Saved Pattern");
			this.locExtender.SetLocalizationComment(this.btnCategoryPaste, "Button to paste a saved pattern from the clipboard. The button is in the heading " +
					"above the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryPaste, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryPaste, "SearchVw.btnCategoryPaste");
			this.btnCategoryPaste.Name = "btnCategoryPaste";
			this.btnCategoryPaste.Click += new System.EventHandler(this.btnCategoryPaste_Click);
			// 
			// btnCategoryCopy
			// 
			resources.ApplyResources(this.btnCategoryCopy, "btnCategoryCopy");
			this.btnCategoryCopy.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryCopy.CanBeChecked = false;
			this.btnCategoryCopy.Checked = false;
			this.btnCategoryCopy.DrawEmpty = false;
			this.btnCategoryCopy.DrawLeftArrowButton = false;
			this.btnCategoryCopy.DrawRightArrowButton = false;
			this.btnCategoryCopy.Image = global::SIL.Pa.Properties.Resources.kimidCopy;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryCopy, "Copy Saved Pattern");
			this.locExtender.SetLocalizationComment(this.btnCategoryCopy, "Button to copy a saved pattern to the clipboard. The button is in the heading abo" +
					"ve the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryCopy, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryCopy, "SearchVw.btnCategoryCopy");
			this.btnCategoryCopy.Name = "btnCategoryCopy";
			this.btnCategoryCopy.Click += new System.EventHandler(this.btnCategoryCopy_Click);
			// 
			// rtfRecVw
			// 
			this.rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtfRecVw, "rtfRecVw");
			this.locExtender.SetLocalizableToolTip(this.rtfRecVw, null);
			this.locExtender.SetLocalizationComment(this.rtfRecVw, null);
			this.locExtender.SetLocalizationPriority(this.rtfRecVw, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rtfRecVw, "SearchVw.rtfRecVw");
			this.rtfRecVw.Name = "rtfRecVw";
			this.rtfRecVw.ReadOnly = true;
			this.rtfRecVw.TabStop = false;
			// 
			// ptrnTextBox
			// 
			resources.ApplyResources(this.ptrnTextBox, "ptrnTextBox");
			this.ptrnTextBox.BackColor = System.Drawing.Color.Transparent;
			this.ptrnTextBox.ClassDisplayBehaviorChanged = false;
			this.locExtender.SetLocalizableToolTip(this.ptrnTextBox, null);
			this.locExtender.SetLocalizationComment(this.ptrnTextBox, null);
			this.locExtender.SetLocalizationPriority(this.ptrnTextBox, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.ptrnTextBox, "SearchVw.PatternTextBox");
			this.ptrnTextBox.Name = "ptrnTextBox";
			this.ptrnTextBox.OwningView = null;
			this.ptrnTextBox.SearchQueryCategory = null;
			this.ptrnTextBox.SearchOptionsChanged += new System.EventHandler(this.ptrnTextBox_SearchOptionsChanged);
			this.ptrnTextBox.PatternTextChanged += new System.EventHandler(this.ptrnTextBox_PatternTextChanged);
			this.ptrnTextBox.SizeChanged += new System.EventHandler(this.ptrnTextBox_SizeChanged);
			// 
			// SearchVw
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlOuter);
			this.Controls.Add(this.pnlSliderPlaceholder);
			this.Controls.Add(this.pnlCurrPattern);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SearchVw");
			this.Name = "SearchVw";
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitSideBarOuter.Panel1.ResumeLayout(false);
			this.splitSideBarOuter.Panel2.ResumeLayout(false);
			this.splitSideBarOuter.ResumeLayout(false);
			this.pnlTabClassDef.ResumeLayout(false);
			this.pnlSideBarCaption.ResumeLayout(false);
			this.splitSideBarInner.Panel1.ResumeLayout(false);
			this.splitSideBarInner.Panel2.ResumeLayout(false);
			this.splitSideBarInner.ResumeLayout(false);
			this.pnlRecentPatterns.ResumeLayout(false);
			this.pnlSavedPatterns.ResumeLayout(false);
			this.splitResults.Panel2.ResumeLayout(false);
			this.splitResults.ResumeLayout(false);
			this.pnlRecView.ResumeLayout(false);
			this.pnlCurrPattern.ResumeLayout(false);
			this.pnlOuter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.hlblRecentPatterns.ResumeLayout(false);
			this.hlblSavedPatterns.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip m_tooltip;
		private SearchPatternTreeView tvSavedPatterns;
		private System.Windows.Forms.SplitContainer splitSideBarOuter;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitSideBarInner;
		private System.Windows.Forms.ListBox lstRecentPatterns;
		private SilPanel pnlRecentPatterns;
		private SilPanel pnlSavedPatterns;
		private System.Windows.Forms.SplitContainer splitResults;
		private SilPanel pnlRecView;
		private RtfRecordView rtfRecVw;
		private System.Windows.Forms.Panel pnlSliderPlaceholder;
		private SilGradientPanel pnlSideBarCaption;
		private XButton btnCategoryNew;
		private XButton btnCategoryCut;
		private XButton btnCategoryCopy;
		private XButton btnCategoryPaste;
		private System.Windows.Forms.Panel pnlTabClassDef;
		private System.Windows.Forms.Panel pnlCurrPattern;
		private System.Windows.Forms.Label lblCurrPattern;
		private System.Windows.Forms.Panel pnlOuter;
		private HeaderLabel hlblRecentPatterns;
		private AutoHideDockButton btnDock;
		private AutoHideDockButton btnAutoHide;
		private HeaderLabel hlblSavedPatterns;
		private XButton btnRemoveFromRecentList;
		private PatternBuilderComponents ptrnBldrComponent;
		private PatternTextBox ptrnTextBox;
		private System.Windows.Forms.Button btnRefresh;
		private XButton btnClearRecentList;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}