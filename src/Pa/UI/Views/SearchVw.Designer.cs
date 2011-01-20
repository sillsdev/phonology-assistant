using SIL.Pa.UI.Controls;
using SilTools.Controls;

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
			this.ptrnBldrComponent = new SIL.Pa.UI.Controls.PatternBuilderComponents();
			this.pnlSideBarCaption = new SilTools.Controls.SilGradientPanel();
			this.btnDock = new SIL.Pa.UI.Controls.AutoHideDockButton();
			this.btnAutoHide = new SIL.Pa.UI.Controls.AutoHideDockButton();
			this.splitSideBarInner = new System.Windows.Forms.SplitContainer();
			this.pnlRecentPatterns = new SilTools.Controls.SilPanel();
			this.lstRecentPatterns = new System.Windows.Forms.ListBox();
			this.hlblRecentPatterns = new SilTools.Controls.HeaderLabel();
			this.btnClearRecentList = new SilTools.Controls.XButton();
			this.btnRemoveFromRecentList = new SilTools.Controls.XButton();
			this.pnlSavedPatterns = new SilTools.Controls.SilPanel();
			this.tvSavedPatterns = new SIL.Pa.UI.Controls.SearchPatternTreeView();
			this.hlblSavedPatterns = new SilTools.Controls.HeaderLabel();
			this.btnCategoryNew = new SilTools.Controls.XButton();
			this.btnCategoryCut = new SilTools.Controls.XButton();
			this.btnCategoryPaste = new SilTools.Controls.XButton();
			this.btnCategoryCopy = new SilTools.Controls.XButton();
			this.splitResults = new System.Windows.Forms.SplitContainer();
			this.pnlRecView = new SilTools.Controls.SilPanel();
			this.rtfRecVw = new SIL.Pa.UI.Controls.RtfRecordView();
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.btnRefresh = new System.Windows.Forms.Button();
			this.pnlSliderPlaceholder = new System.Windows.Forms.Panel();
			this.lblCurrPattern = new System.Windows.Forms.Label();
			this.pnlOuter = new System.Windows.Forms.Panel();
			this.m_patternTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.ptrnTextBox = new SIL.Pa.UI.Controls.PatternTextBox();
			this.m_patternBuilderBar = new SIL.Pa.UI.Controls.PatternBuilderBar();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
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
			this.hlblRecentPatterns.SuspendLayout();
			this.pnlSavedPatterns.SuspendLayout();
			this.hlblSavedPatterns.SuspendLayout();
			this.splitResults.Panel2.SuspendLayout();
			this.splitResults.SuspendLayout();
			this.pnlRecView.SuspendLayout();
			this.pnlOuter.SuspendLayout();
			this.m_patternTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.Location = new System.Drawing.Point(0, 0);
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.splitSideBarOuter);
			this.splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 0);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitResults);
			this.splitOuter.Size = new System.Drawing.Size(541, 457);
			this.splitOuter.SplitterDistance = 230;
			this.splitOuter.SplitterWidth = 8;
			this.splitOuter.TabIndex = 0;
			this.splitOuter.TabStop = false;
			// 
			// splitSideBarOuter
			// 
			this.splitSideBarOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitSideBarOuter.Location = new System.Drawing.Point(8, 3);
			this.splitSideBarOuter.Name = "splitSideBarOuter";
			this.splitSideBarOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitSideBarOuter.Panel1
			// 
			this.splitSideBarOuter.Panel1.Controls.Add(this.pnlTabClassDef);
			this.splitSideBarOuter.Panel1.Controls.Add(this.pnlSideBarCaption);
			// 
			// splitSideBarOuter.Panel2
			// 
			this.splitSideBarOuter.Panel2.Controls.Add(this.splitSideBarInner);
			this.splitSideBarOuter.Size = new System.Drawing.Size(222, 454);
			this.splitSideBarOuter.SplitterDistance = 193;
			this.splitSideBarOuter.SplitterWidth = 8;
			this.splitSideBarOuter.TabIndex = 0;
			this.splitSideBarOuter.TabStop = false;
			// 
			// pnlTabClassDef
			// 
			this.pnlTabClassDef.Controls.Add(this.ptrnBldrComponent);
			this.pnlTabClassDef.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTabClassDef.Location = new System.Drawing.Point(0, 22);
			this.pnlTabClassDef.Name = "pnlTabClassDef";
			this.pnlTabClassDef.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.pnlTabClassDef.Size = new System.Drawing.Size(222, 171);
			this.pnlTabClassDef.TabIndex = 1;
			// 
			// ptrnBldrComponent
			// 
			this.ptrnBldrComponent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this.ptrnBldrComponent, null);
			this.locExtender.SetLocalizationComment(this.ptrnBldrComponent, null);
			this.locExtender.SetLocalizationPriority(this.ptrnBldrComponent, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.ptrnBldrComponent, "SearchVw.PatternBuilderComponents");
			this.ptrnBldrComponent.Location = new System.Drawing.Point(0, 4);
			this.ptrnBldrComponent.Name = "ptrnBldrComponent";
			this.ptrnBldrComponent.Size = new System.Drawing.Size(222, 167);
			this.ptrnBldrComponent.TabIndex = 0;
			// 
			// pnlSideBarCaption
			// 
			this.pnlSideBarCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSideBarCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSideBarCaption.ClipTextForChildControls = true;
			this.pnlSideBarCaption.ColorBottom = System.Drawing.Color.Empty;
			this.pnlSideBarCaption.ColorTop = System.Drawing.Color.Empty;
			this.pnlSideBarCaption.ControlReceivingFocusOnMnemonic = null;
			this.pnlSideBarCaption.Controls.Add(this.btnDock);
			this.pnlSideBarCaption.Controls.Add(this.btnAutoHide);
			this.pnlSideBarCaption.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSideBarCaption.DoubleBuffered = false;
			this.pnlSideBarCaption.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSideBarCaption.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSideBarCaption, null);
			this.locExtender.SetLocalizationComment(this.pnlSideBarCaption, "Caption at the top of  the side bar in the Search view.");
			this.locExtender.SetLocalizingId(this.pnlSideBarCaption, "SearchVw.pnlSideBarCaption");
			this.pnlSideBarCaption.Location = new System.Drawing.Point(0, 0);
			this.pnlSideBarCaption.MakeDark = false;
			this.pnlSideBarCaption.MnemonicGeneratesClick = false;
			this.pnlSideBarCaption.Name = "pnlSideBarCaption";
			this.pnlSideBarCaption.PaintExplorerBarBackground = false;
			this.pnlSideBarCaption.Size = new System.Drawing.Size(222, 22);
			this.pnlSideBarCaption.TabIndex = 0;
			this.pnlSideBarCaption.Text = "Patterns && Pattern Building";
			// 
			// btnDock
			// 
			this.btnDock.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnDock.BackColor = System.Drawing.Color.Transparent;
			this.btnDock.CanBeChecked = false;
			this.btnDock.Checked = false;
			this.btnDock.DrawEmpty = false;
			this.btnDock.DrawLeftArrowButton = false;
			this.btnDock.DrawRightArrowButton = false;
			this.btnDock.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnDock.Image = ((System.Drawing.Image)(resources.GetObject("btnDock.Image")));
			this.btnDock.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnDock, "Dock");
			this.locExtender.SetLocalizationComment(this.btnDock, "For docking button on the side bar in search view.");
			this.locExtender.SetLocalizationPriority(this.btnDock, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnDock, "SearchVw.btnDock");
			this.btnDock.Location = new System.Drawing.Point(198, 2);
			this.btnDock.Name = "btnDock";
			this.btnDock.Size = new System.Drawing.Size(16, 16);
			this.btnDock.TabIndex = 1;
			this.btnDock.Click += new System.EventHandler(this.HandleDockButtonClick);
			// 
			// btnAutoHide
			// 
			this.btnAutoHide.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnAutoHide.BackColor = System.Drawing.Color.Transparent;
			this.btnAutoHide.CanBeChecked = false;
			this.btnAutoHide.Checked = false;
			this.btnAutoHide.DrawEmpty = false;
			this.btnAutoHide.DrawLeftArrowButton = false;
			this.btnAutoHide.DrawRightArrowButton = false;
			this.btnAutoHide.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnAutoHide.Image = ((System.Drawing.Image)(resources.GetObject("btnAutoHide.Image")));
			this.btnAutoHide.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnAutoHide, "Automatically Hide");
			this.locExtender.SetLocalizationComment(this.btnAutoHide, "For docking button on the side bar in search view.");
			this.locExtender.SetLocalizationPriority(this.btnAutoHide, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnAutoHide, "SearchVw.btnAutoHide");
			this.btnAutoHide.Location = new System.Drawing.Point(154, 2);
			this.btnAutoHide.Name = "btnAutoHide";
			this.btnAutoHide.Size = new System.Drawing.Size(16, 16);
			this.btnAutoHide.TabIndex = 0;
			this.btnAutoHide.Click += new System.EventHandler(this.HandleAutoHideButtonClick);
			// 
			// splitSideBarInner
			// 
			this.splitSideBarInner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitSideBarInner.Location = new System.Drawing.Point(0, 0);
			this.splitSideBarInner.Name = "splitSideBarInner";
			this.splitSideBarInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitSideBarInner.Panel1
			// 
			this.splitSideBarInner.Panel1.Controls.Add(this.pnlRecentPatterns);
			// 
			// splitSideBarInner.Panel2
			// 
			this.splitSideBarInner.Panel2.Controls.Add(this.pnlSavedPatterns);
			this.splitSideBarInner.Size = new System.Drawing.Size(222, 253);
			this.splitSideBarInner.SplitterDistance = 97;
			this.splitSideBarInner.SplitterWidth = 8;
			this.splitSideBarInner.TabIndex = 0;
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
			this.pnlRecentPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRecentPatterns.DoubleBuffered = false;
			this.pnlRecentPatterns.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlRecentPatterns.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlRecentPatterns, null);
			this.locExtender.SetLocalizationComment(this.pnlRecentPatterns, null);
			this.locExtender.SetLocalizationPriority(this.pnlRecentPatterns, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlRecentPatterns, "SearchVw.pnlRecentPatterns");
			this.pnlRecentPatterns.Location = new System.Drawing.Point(0, 0);
			this.pnlRecentPatterns.MnemonicGeneratesClick = false;
			this.pnlRecentPatterns.Name = "pnlRecentPatterns";
			this.pnlRecentPatterns.PaintExplorerBarBackground = false;
			this.pnlRecentPatterns.Size = new System.Drawing.Size(222, 97);
			this.pnlRecentPatterns.TabIndex = 0;
			// 
			// lstRecentPatterns
			// 
			this.lstRecentPatterns.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstRecentPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstRecentPatterns.FormattingEnabled = true;
			this.lstRecentPatterns.IntegralHeight = false;
			this.lstRecentPatterns.ItemHeight = 15;
			this.lstRecentPatterns.Location = new System.Drawing.Point(0, 24);
			this.lstRecentPatterns.Name = "lstRecentPatterns";
			this.lstRecentPatterns.Size = new System.Drawing.Size(220, 71);
			this.lstRecentPatterns.TabIndex = 1;
			this.lstRecentPatterns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstRecentPatterns_MouseUp);
			this.lstRecentPatterns.Enter += new System.EventHandler(this.lstRecentPatterns_Enter);
			this.lstRecentPatterns.DoubleClick += new System.EventHandler(this.lstRecentPatterns_DoubleClick);
			this.lstRecentPatterns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstRecentPatterns_MouseMove);
			this.lstRecentPatterns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstRecentPatterns_MouseDown);
			this.lstRecentPatterns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstRecentPatterns_KeyDown);
			// 
			// hlblRecentPatterns
			// 
			this.hlblRecentPatterns.ClipTextForChildControls = true;
			this.hlblRecentPatterns.ControlReceivingFocusOnMnemonic = null;
			this.hlblRecentPatterns.Controls.Add(this.btnClearRecentList);
			this.hlblRecentPatterns.Controls.Add(this.btnRemoveFromRecentList);
			this.hlblRecentPatterns.Dock = System.Windows.Forms.DockStyle.Top;
			this.hlblRecentPatterns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.hlblRecentPatterns.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.hlblRecentPatterns, null);
			this.locExtender.SetLocalizationComment(this.hlblRecentPatterns, "Heading above list of recent patterns in search view.");
			this.locExtender.SetLocalizingId(this.hlblRecentPatterns, "SearchVw.hlblRecentPatterns");
			this.hlblRecentPatterns.Location = new System.Drawing.Point(0, 0);
			this.hlblRecentPatterns.MnemonicGeneratesClick = false;
			this.hlblRecentPatterns.Name = "hlblRecentPatterns";
			this.hlblRecentPatterns.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.hlblRecentPatterns.Size = new System.Drawing.Size(220, 24);
			this.hlblRecentPatterns.TabIndex = 0;
			this.hlblRecentPatterns.Text = "&Recent Patterns";
			// 
			// btnClearRecentList
			// 
			this.btnClearRecentList.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnClearRecentList.BackColor = System.Drawing.Color.Transparent;
			this.btnClearRecentList.CanBeChecked = false;
			this.btnClearRecentList.Checked = false;
			this.btnClearRecentList.DrawEmpty = false;
			this.btnClearRecentList.DrawLeftArrowButton = false;
			this.btnClearRecentList.DrawRightArrowButton = false;
			this.btnClearRecentList.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnClearRecentList.Image = global::SIL.Pa.Properties.Resources.kimidClearList;
			this.btnClearRecentList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnClearRecentList, "Clear List");
			this.locExtender.SetLocalizationComment(this.btnClearRecentList, null);
			this.locExtender.SetLocalizationPriority(this.btnClearRecentList, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnClearRecentList, "SearchVw.btnClearRecentList");
			this.btnClearRecentList.Location = new System.Drawing.Point(193, 2);
			this.btnClearRecentList.Name = "btnClearRecentList";
			this.btnClearRecentList.Size = new System.Drawing.Size(20, 20);
			this.btnClearRecentList.TabIndex = 1;
			this.btnClearRecentList.Click += new System.EventHandler(this.HandleClearRecentListButtonClick);
			// 
			// btnRemoveFromRecentList
			// 
			this.btnRemoveFromRecentList.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnRemoveFromRecentList.BackColor = System.Drawing.Color.Transparent;
			this.btnRemoveFromRecentList.CanBeChecked = false;
			this.btnRemoveFromRecentList.Checked = false;
			this.btnRemoveFromRecentList.DrawEmpty = false;
			this.btnRemoveFromRecentList.DrawLeftArrowButton = false;
			this.btnRemoveFromRecentList.DrawRightArrowButton = false;
			this.btnRemoveFromRecentList.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnRemoveFromRecentList.Image = global::SIL.Pa.Properties.Resources.kimidDelete;
			this.btnRemoveFromRecentList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnRemoveFromRecentList, "Remove Selected Pattern");
			this.locExtender.SetLocalizationComment(this.btnRemoveFromRecentList, null);
			this.locExtender.SetLocalizationPriority(this.btnRemoveFromRecentList, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnRemoveFromRecentList, "SearchVw.btnRemoveFromRecentList");
			this.btnRemoveFromRecentList.Location = new System.Drawing.Point(169, 2);
			this.btnRemoveFromRecentList.Name = "btnRemoveFromRecentList";
			this.btnRemoveFromRecentList.Size = new System.Drawing.Size(20, 20);
			this.btnRemoveFromRecentList.TabIndex = 0;
			this.btnRemoveFromRecentList.Click += new System.EventHandler(this.HandleRemoveFromRecentListButtonClick);
			// 
			// pnlSavedPatterns
			// 
			this.pnlSavedPatterns.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlSavedPatterns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSavedPatterns.ClipTextForChildControls = true;
			this.pnlSavedPatterns.ControlReceivingFocusOnMnemonic = null;
			this.pnlSavedPatterns.Controls.Add(this.tvSavedPatterns);
			this.pnlSavedPatterns.Controls.Add(this.hlblSavedPatterns);
			this.pnlSavedPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSavedPatterns.DoubleBuffered = false;
			this.pnlSavedPatterns.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSavedPatterns.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlSavedPatterns, null);
			this.locExtender.SetLocalizationComment(this.pnlSavedPatterns, null);
			this.locExtender.SetLocalizationPriority(this.pnlSavedPatterns, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSavedPatterns, "SearchVw.pnlSavedPatterns");
			this.pnlSavedPatterns.Location = new System.Drawing.Point(0, 0);
			this.pnlSavedPatterns.MnemonicGeneratesClick = false;
			this.pnlSavedPatterns.Name = "pnlSavedPatterns";
			this.pnlSavedPatterns.PaintExplorerBarBackground = false;
			this.pnlSavedPatterns.Size = new System.Drawing.Size(222, 148);
			this.pnlSavedPatterns.TabIndex = 0;
			// 
			// tvSavedPatterns
			// 
			this.tvSavedPatterns.AllowDataModifications = true;
			this.tvSavedPatterns.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvSavedPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvSavedPatterns.Font = new System.Drawing.Font("Doulos SIL", 13F);
			this.tvSavedPatterns.HideSelection = false;
			this.tvSavedPatterns.ImageIndex = 0;
			this.tvSavedPatterns.IsForToolbarPopup = false;
			this.locExtender.SetLocalizableToolTip(this.tvSavedPatterns, null);
			this.locExtender.SetLocalizationComment(this.tvSavedPatterns, null);
			this.locExtender.SetLocalizationPriority(this.tvSavedPatterns, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tvSavedPatterns, "SearchVw.tvSavedPatterns");
			this.tvSavedPatterns.Location = new System.Drawing.Point(0, 24);
			this.tvSavedPatterns.Name = "tvSavedPatterns";
			this.tvSavedPatterns.SelectedImageIndex = 0;
			this.tvSavedPatterns.Size = new System.Drawing.Size(220, 122);
			this.tvSavedPatterns.TabIndex = 1;
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
			this.hlblSavedPatterns.Dock = System.Windows.Forms.DockStyle.Top;
			this.hlblSavedPatterns.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.hlblSavedPatterns.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.hlblSavedPatterns, null);
			this.locExtender.SetLocalizationComment(this.hlblSavedPatterns, "Heading above list of saved patterns in search  view.");
			this.locExtender.SetLocalizingId(this.hlblSavedPatterns, "SearchVw.hlblSavedPatterns");
			this.hlblSavedPatterns.Location = new System.Drawing.Point(0, 0);
			this.hlblSavedPatterns.MnemonicGeneratesClick = false;
			this.hlblSavedPatterns.Name = "hlblSavedPatterns";
			this.hlblSavedPatterns.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.hlblSavedPatterns.Size = new System.Drawing.Size(220, 24);
			this.hlblSavedPatterns.TabIndex = 0;
			this.hlblSavedPatterns.Text = "Save&d Patterns";
			// 
			// btnCategoryNew
			// 
			this.btnCategoryNew.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnCategoryNew.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryNew.CanBeChecked = false;
			this.btnCategoryNew.Checked = false;
			this.btnCategoryNew.DrawEmpty = false;
			this.btnCategoryNew.DrawLeftArrowButton = false;
			this.btnCategoryNew.DrawRightArrowButton = false;
			this.btnCategoryNew.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnCategoryNew.Image = global::SIL.Pa.Properties.Resources.kimidNewPatternCategory;
			this.btnCategoryNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryNew, "Create New Saved Pattern Category");
			this.locExtender.SetLocalizationComment(this.btnCategoryNew, "Button to create a new saved patterns folder. The button is in the heading above " +
					"the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryNew, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryNew, "SearchVw.btnCategoryNew");
			this.btnCategoryNew.Location = new System.Drawing.Point(193, 2);
			this.btnCategoryNew.Name = "btnCategoryNew";
			this.btnCategoryNew.Size = new System.Drawing.Size(20, 20);
			this.btnCategoryNew.TabIndex = 1;
			this.btnCategoryNew.Click += new System.EventHandler(this.HandleCategoryNewButtonClick);
			// 
			// btnCategoryCut
			// 
			this.btnCategoryCut.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnCategoryCut.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryCut.CanBeChecked = false;
			this.btnCategoryCut.Checked = false;
			this.btnCategoryCut.DrawEmpty = false;
			this.btnCategoryCut.DrawLeftArrowButton = false;
			this.btnCategoryCut.DrawRightArrowButton = false;
			this.btnCategoryCut.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnCategoryCut.Image = global::SIL.Pa.Properties.Resources.kimidCut;
			this.btnCategoryCut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryCut, "Cut Saved Pattern");
			this.locExtender.SetLocalizationComment(this.btnCategoryCut, "Button to cut a saved pattern to the clipboard. The button is in the heading abov" +
					"e the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryCut, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryCut, "SearchVw.btnCategoryCut");
			this.btnCategoryCut.Location = new System.Drawing.Point(121, 2);
			this.btnCategoryCut.Name = "btnCategoryCut";
			this.btnCategoryCut.Size = new System.Drawing.Size(20, 20);
			this.btnCategoryCut.TabIndex = 0;
			this.btnCategoryCut.Click += new System.EventHandler(this.HandleCategoryCutButtonClick);
			// 
			// btnCategoryPaste
			// 
			this.btnCategoryPaste.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnCategoryPaste.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryPaste.CanBeChecked = false;
			this.btnCategoryPaste.Checked = false;
			this.btnCategoryPaste.DrawEmpty = false;
			this.btnCategoryPaste.DrawLeftArrowButton = false;
			this.btnCategoryPaste.DrawRightArrowButton = false;
			this.btnCategoryPaste.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnCategoryPaste.Image = global::SIL.Pa.Properties.Resources.kimidPaste;
			this.btnCategoryPaste.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryPaste, "Paste Saved Pattern");
			this.locExtender.SetLocalizationComment(this.btnCategoryPaste, "Button to paste a saved pattern from the clipboard. The button is in the heading " +
					"above the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryPaste, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryPaste, "SearchVw.btnCategoryPaste");
			this.btnCategoryPaste.Location = new System.Drawing.Point(169, 2);
			this.btnCategoryPaste.Name = "btnCategoryPaste";
			this.btnCategoryPaste.Size = new System.Drawing.Size(20, 20);
			this.btnCategoryPaste.TabIndex = 3;
			this.btnCategoryPaste.Click += new System.EventHandler(this.HandleCategoryPasteButtonClick);
			// 
			// btnCategoryCopy
			// 
			this.btnCategoryCopy.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnCategoryCopy.BackColor = System.Drawing.Color.Transparent;
			this.btnCategoryCopy.CanBeChecked = false;
			this.btnCategoryCopy.Checked = false;
			this.btnCategoryCopy.DrawEmpty = false;
			this.btnCategoryCopy.DrawLeftArrowButton = false;
			this.btnCategoryCopy.DrawRightArrowButton = false;
			this.btnCategoryCopy.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnCategoryCopy.Image = global::SIL.Pa.Properties.Resources.kimidCopy;
			this.btnCategoryCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCategoryCopy, "Copy Saved Pattern");
			this.locExtender.SetLocalizationComment(this.btnCategoryCopy, "Button to copy a saved pattern to the clipboard. The button is in the heading abo" +
					"ve the saved patterns list in search view.");
			this.locExtender.SetLocalizationPriority(this.btnCategoryCopy, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnCategoryCopy, "SearchVw.btnCategoryCopy");
			this.btnCategoryCopy.Location = new System.Drawing.Point(145, 2);
			this.btnCategoryCopy.Name = "btnCategoryCopy";
			this.btnCategoryCopy.Size = new System.Drawing.Size(20, 20);
			this.btnCategoryCopy.TabIndex = 2;
			this.btnCategoryCopy.Click += new System.EventHandler(this.HandleCategoryCopyButtonClick);
			// 
			// splitResults
			// 
			this.splitResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitResults.Location = new System.Drawing.Point(0, 0);
			this.splitResults.Name = "splitResults";
			this.splitResults.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitResults.Panel1
			// 
			this.splitResults.Panel1.AllowDrop = true;
			this.splitResults.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitResults.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.splitResults.Panel1.Tag = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			this.splitResults.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleSplitResultsPanel1Paint);
			this.splitResults.Panel1.DragOver += new System.Windows.Forms.DragEventHandler(this.HandleSplitResultsPanel1DragOver);
			this.splitResults.Panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleSplitResultsPanel1DragDrop);
			this.splitResults.Panel1.SizeChanged += new System.EventHandler(this.HandleSplitResultsPanel1SizeChanged);
			// 
			// splitResults.Panel2
			// 
			this.splitResults.Panel2.Controls.Add(this.pnlRecView);
			this.splitResults.Size = new System.Drawing.Size(303, 457);
			this.splitResults.SplitterDistance = 328;
			this.splitResults.SplitterWidth = 8;
			this.splitResults.TabIndex = 0;
			// 
			// pnlRecView
			// 
			this.pnlRecView.BackColor = System.Drawing.SystemColors.Window;
			this.pnlRecView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlRecView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlRecView.ClipTextForChildControls = true;
			this.pnlRecView.ControlReceivingFocusOnMnemonic = null;
			this.pnlRecView.Controls.Add(this.rtfRecVw);
			this.pnlRecView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRecView.DoubleBuffered = false;
			this.pnlRecView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlRecView.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlRecView, null);
			this.locExtender.SetLocalizationComment(this.pnlRecView, null);
			this.locExtender.SetLocalizationPriority(this.pnlRecView, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlRecView, "SearchVw.pnlRecView");
			this.pnlRecView.Location = new System.Drawing.Point(0, 0);
			this.pnlRecView.MnemonicGeneratesClick = false;
			this.pnlRecView.Name = "pnlRecView";
			this.pnlRecView.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
			this.pnlRecView.PaintExplorerBarBackground = false;
			this.pnlRecView.Size = new System.Drawing.Size(303, 121);
			this.pnlRecView.TabIndex = 0;
			// 
			// rtfRecVw
			// 
			this.rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtfRecVw.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this.rtfRecVw, null);
			this.locExtender.SetLocalizationComment(this.rtfRecVw, null);
			this.locExtender.SetLocalizationPriority(this.rtfRecVw, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rtfRecVw, "SearchVw.rtfRecVw");
			this.rtfRecVw.Location = new System.Drawing.Point(4, 4);
			this.rtfRecVw.Name = "rtfRecVw";
			this.rtfRecVw.ReadOnly = true;
			this.rtfRecVw.Size = new System.Drawing.Size(297, 115);
			this.rtfRecVw.TabIndex = 0;
			this.rtfRecVw.TabStop = false;
			this.rtfRecVw.Text = " ";
			this.rtfRecVw.WordWrap = false;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnRefresh.Image = global::SIL.Pa.Properties.Resources.kimidRefresh;
			this.btnRefresh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnRefresh, "Refresh Results");
			this.locExtender.SetLocalizationComment(this.btnRefresh, "Button to refresh search results. The button is to the right of the search patter" +
					"n text box at the top of the search view.");
			this.locExtender.SetLocalizationPriority(this.btnRefresh, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnRefresh, "SearchVw.btnRefresh");
			this.btnRefresh.Location = new System.Drawing.Point(547, 6);
			this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 8, 0);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(26, 26);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.HandleRefreshButtonClick);
			// 
			// pnlSliderPlaceholder
			// 
			this.pnlSliderPlaceholder.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlSliderPlaceholder.Location = new System.Drawing.Point(0, 66);
			this.pnlSliderPlaceholder.Name = "pnlSliderPlaceholder";
			this.pnlSliderPlaceholder.Size = new System.Drawing.Size(32, 465);
			this.pnlSliderPlaceholder.TabIndex = 1;
			// 
			// lblCurrPattern
			// 
			this.lblCurrPattern.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCurrPattern.AutoSize = true;
			this.lblCurrPattern.BackColor = System.Drawing.Color.Transparent;
			this.lblCurrPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblCurrPattern.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblCurrPattern, null);
			this.locExtender.SetLocalizationComment(this.lblCurrPattern, "Text next to search pattern text box in search view.");
			this.locExtender.SetLocalizingId(this.lblCurrPattern, "SearchVw.lblCurrPattern");
			this.lblCurrPattern.Location = new System.Drawing.Point(8, 11);
			this.lblCurrPattern.Margin = new System.Windows.Forms.Padding(8, 6, 3, 0);
			this.lblCurrPattern.Name = "lblCurrPattern";
			this.lblCurrPattern.Size = new System.Drawing.Size(141, 15);
			this.lblCurrPattern.TabIndex = 0;
			this.lblCurrPattern.Text = "C&urrent\\nSearch Pattern:";
			this.lblCurrPattern.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlOuter
			// 
			this.pnlOuter.Controls.Add(this.splitOuter);
			this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlOuter.Location = new System.Drawing.Point(32, 66);
			this.pnlOuter.Name = "pnlOuter";
			this.pnlOuter.Padding = new System.Windows.Forms.Padding(0, 0, 8, 8);
			this.pnlOuter.Size = new System.Drawing.Size(549, 465);
			this.pnlOuter.TabIndex = 3;
			// 
			// m_patternTableLayoutPanel
			// 
			this.m_patternTableLayoutPanel.AutoSize = true;
			this.m_patternTableLayoutPanel.ColumnCount = 3;
			this.m_patternTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_patternTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_patternTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_patternTableLayoutPanel.Controls.Add(this.btnRefresh, 2, 0);
			this.m_patternTableLayoutPanel.Controls.Add(this.ptrnTextBox, 1, 0);
			this.m_patternTableLayoutPanel.Controls.Add(this.lblCurrPattern, 0, 0);
			this.m_patternTableLayoutPanel.Controls.Add(this.m_patternBuilderBar, 1, 1);
			this.m_patternTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_patternTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.m_patternTableLayoutPanel.Name = "m_patternTableLayoutPanel";
			this.m_patternTableLayoutPanel.RowCount = 2;
			this.m_patternTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_patternTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_patternTableLayoutPanel.Size = new System.Drawing.Size(581, 66);
			this.m_patternTableLayoutPanel.TabIndex = 4;
			this.m_patternTableLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePatternTableLayoutPanelPaint);
			// 
			// ptrnTextBox
			// 
			this.ptrnTextBox.AcceptsReturn = true;
			this.ptrnTextBox.AllowDrop = true;
			this.ptrnTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.ptrnTextBox.ClassDisplayBehaviorChanged = false;
			this.ptrnTextBox.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.ptrnTextBox, null);
			this.locExtender.SetLocalizationComment(this.ptrnTextBox, null);
			this.locExtender.SetLocalizationPriority(this.ptrnTextBox, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.ptrnTextBox, "SearchVw.PatternTextBox");
			this.ptrnTextBox.Location = new System.Drawing.Point(152, 9);
			this.ptrnTextBox.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
			this.ptrnTextBox.Name = "ptrnTextBox";
			this.ptrnTextBox.OwningView = null;
			this.ptrnTextBox.SearchQueryCategory = null;
			this.ptrnTextBox.Size = new System.Drawing.Size(391, 20);
			this.ptrnTextBox.TabIndex = 1;
			this.ptrnTextBox.PatternTextChanged += new System.EventHandler(this.HandlePatternTextBoxPatternTextChanged);
			this.ptrnTextBox.SearchOptionsChanged += new System.EventHandler(this.HandlePatternTextBoxSearchOptionsChanged);
			// 
			// m_patternBuilderBar
			// 
			this.m_patternBuilderBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(130)))), ((int)(((byte)(152)))));
			this.m_patternBuilderBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.locExtender.SetLocalizableToolTip(this.m_patternBuilderBar, null);
			this.locExtender.SetLocalizationComment(this.m_patternBuilderBar, null);
			this.locExtender.SetLocalizationPriority(this.m_patternBuilderBar, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_patternBuilderBar, "SearchVw.PatternBuilderBar");
			this.m_patternBuilderBar.Location = new System.Drawing.Point(152, 33);
			this.m_patternBuilderBar.Margin = new System.Windows.Forms.Padding(0, 1, 0, 6);
			this.m_patternBuilderBar.Name = "m_patternBuilderBar";
			this.m_patternBuilderBar.Padding = new System.Windows.Forms.Padding(1, 1, 1, 4);
			this.m_patternBuilderBar.Size = new System.Drawing.Size(391, 27);
			this.m_patternBuilderBar.TabIndex = 5;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// SearchVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlOuter);
			this.Controls.Add(this.pnlSliderPlaceholder);
			this.Controls.Add(this.m_patternTableLayoutPanel);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SearchVw");
			this.Name = "SearchVw";
			this.Size = new System.Drawing.Size(581, 531);
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
			this.hlblRecentPatterns.ResumeLayout(false);
			this.pnlSavedPatterns.ResumeLayout(false);
			this.hlblSavedPatterns.ResumeLayout(false);
			this.splitResults.Panel2.ResumeLayout(false);
			this.splitResults.ResumeLayout(false);
			this.pnlRecView.ResumeLayout(false);
			this.pnlOuter.ResumeLayout(false);
			this.m_patternTableLayoutPanel.ResumeLayout(false);
			this.m_patternTableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.Label lblCurrPattern;
		private System.Windows.Forms.Panel pnlOuter;
		private HeaderLabel hlblRecentPatterns;
		private AutoHideDockButton btnDock;
		private AutoHideDockButton btnAutoHide;
		private HeaderLabel hlblSavedPatterns;
		private SilTools.Controls.XButton btnRemoveFromRecentList;
		private PatternBuilderComponents ptrnBldrComponent;
		private PatternTextBox ptrnTextBox;
		private System.Windows.Forms.Button btnRefresh;
		private SilTools.Controls.XButton btnClearRecentList;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel m_patternTableLayoutPanel;
		private PatternBuilderBar m_patternBuilderBar;
	}
}