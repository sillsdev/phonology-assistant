using SilTools.Controls;

namespace SIL.Pa.UI.Views
{
	partial class DistributionChartVw
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributionChartVw));
            this.splitResults = new System.Windows.Forms.SplitContainer();
            this._recView = new SIL.Pa.UI.Controls.RecordViewControls.RecordViewPanel();
            this.splitOuter = new System.Windows.Forms.SplitContainer();
            this.splitSideBarOuter = new System.Windows.Forms.SplitContainer();
            this.pnlTabClassDef = new System.Windows.Forms.Panel();
            this.ptrnBldrComponent = new SIL.Pa.UI.Controls.PatternBuilderComponents();
            this.pnlSideBarCaption = new SilTools.Controls.SilGradientPanel();
            this.btnDock = new SIL.Pa.UI.Controls.AutoHideDockButton();
            this.btnAutoHide = new SIL.Pa.UI.Controls.AutoHideDockButton();
            this.pnlSavedCharts = new SilTools.Controls.SilPanel();
            this.lvSavedCharts = new System.Windows.Forms.ListView();
            this.hdrSavedCharts = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hlblSavedCharts = new SilTools.Controls.HeaderLabel();
            this.btnRestoreDefaultCharts = new SilTools.Controls.XButton();
            this.btnRemoveSavedChart = new SilTools.Controls.XButton();
            this.splitChart = new System.Windows.Forms.SplitContainer();
            this.lblChartName = new System.Windows.Forms.Label();
            this.lblChartNameValue = new System.Windows.Forms.Label();
            this.pnlSliderPlaceholder = new System.Windows.Forms.Panel();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitResults)).BeginInit();
            this.splitResults.Panel2.SuspendLayout();
            this.splitResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitOuter)).BeginInit();
            this.splitOuter.Panel1.SuspendLayout();
            this.splitOuter.Panel2.SuspendLayout();
            this.splitOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSideBarOuter)).BeginInit();
            this.splitSideBarOuter.Panel1.SuspendLayout();
            this.splitSideBarOuter.Panel2.SuspendLayout();
            this.splitSideBarOuter.SuspendLayout();
            this.pnlTabClassDef.SuspendLayout();
            this.pnlSideBarCaption.SuspendLayout();
            this.pnlSavedCharts.SuspendLayout();
            this.hlblSavedCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitChart)).BeginInit();
            this.splitChart.Panel1.SuspendLayout();
            this.splitChart.Panel2.SuspendLayout();
            this.splitChart.SuspendLayout();
            this.pnlOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
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
            this.splitResults.Panel1.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            // 
            // splitResults.Panel2
            // 
            this.splitResults.Panel2.Controls.Add(this._recView);
            this.splitResults.Size = new System.Drawing.Size(431, 275);
            this.splitResults.SplitterDistance = 148;
            this.splitResults.SplitterWidth = 8;
            this.splitResults.TabIndex = 0;
            // 
            // _recView
            // 
            this._recView.BackColor = System.Drawing.SystemColors.Window;
            this._recView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this._recView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._recView.ClipTextForChildControls = true;
            this._recView.ControlReceivingFocusOnMnemonic = null;
            this._recView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recView.DoubleBuffered = true;
            this._recView.DrawOnlyBottomBorder = false;
            this._recView.DrawOnlyTopBorder = false;
            this._recView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._recView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this._recView, null);
            this.locExtender.SetLocalizationComment(this._recView, null);
            this.locExtender.SetLocalizationPriority(this._recView, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._recView, "recordViewPanel1.recordViewPanel1");
            this._recView.Location = new System.Drawing.Point(0, 0);
            this._recView.MnemonicGeneratesClick = false;
            this._recView.Name = "_recView";
            this._recView.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this._recView.PaintExplorerBarBackground = false;
            this._recView.Size = new System.Drawing.Size(431, 119);
            this._recView.TabIndex = 0;
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
            this.splitOuter.Panel2.Controls.Add(this.splitChart);
            this.splitOuter.Size = new System.Drawing.Size(624, 523);
            this.splitOuter.SplitterDistance = 185;
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
            this.splitSideBarOuter.Panel2.Controls.Add(this.pnlSavedCharts);
            this.splitSideBarOuter.Size = new System.Drawing.Size(177, 520);
            this.splitSideBarOuter.SplitterDistance = 248;
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
            this.pnlTabClassDef.Size = new System.Drawing.Size(177, 226);
            this.pnlTabClassDef.TabIndex = 0;
            // 
            // ptrnBldrComponent
            // 
            this.ptrnBldrComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locExtender.SetLocalizableToolTip(this.ptrnBldrComponent, null);
            this.locExtender.SetLocalizationComment(this.ptrnBldrComponent, null);
            this.locExtender.SetLocalizationPriority(this.ptrnBldrComponent, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.ptrnBldrComponent, "DistributionChartVw.PatternBuilderComponents");
            this.ptrnBldrComponent.Location = new System.Drawing.Point(0, 4);
            this.ptrnBldrComponent.Name = "ptrnBldrComponent";
            this.ptrnBldrComponent.Size = new System.Drawing.Size(177, 222);
            this.ptrnBldrComponent.TabIndex = 0;
            // 
            // pnlSideBarCaption
            // 
            this.pnlSideBarCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.pnlSideBarCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSideBarCaption.ClipTextForChildControls = true;
            this.pnlSideBarCaption.ColorBottom = System.Drawing.Color.Empty;
            this.pnlSideBarCaption.ColorTop = System.Drawing.Color.Empty;
            this.pnlSideBarCaption.ControlReceivingFocusOnMnemonic = null;
            this.pnlSideBarCaption.Controls.Add(this.btnDock);
            this.pnlSideBarCaption.Controls.Add(this.btnAutoHide);
            this.pnlSideBarCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSideBarCaption.DoubleBuffered = false;
            this.pnlSideBarCaption.DrawOnlyBottomBorder = false;
            this.pnlSideBarCaption.DrawOnlyTopBorder = false;
            this.pnlSideBarCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.pnlSideBarCaption.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.pnlSideBarCaption, null);
            this.locExtender.SetLocalizationComment(this.pnlSideBarCaption, "Caption at the top of the side bar in the distribution Charts view.");
            this.locExtender.SetLocalizingId(this.pnlSideBarCaption, "Views.DistributionChart.SideBarCaptionLabel");
            this.pnlSideBarCaption.Location = new System.Drawing.Point(0, 0);
            this.pnlSideBarCaption.MakeDark = false;
            this.pnlSideBarCaption.MnemonicGeneratesClick = false;
            this.pnlSideBarCaption.Name = "pnlSideBarCaption";
            this.pnlSideBarCaption.PaintExplorerBarBackground = false;
            this.pnlSideBarCaption.Size = new System.Drawing.Size(177, 22);
            this.pnlSideBarCaption.TabIndex = 1;
            this.pnlSideBarCaption.Text = "Charts && Chart Building";
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
            this.locExtender.SetLocalizationComment(this.btnDock, null);
            this.locExtender.SetLocalizingId(this.btnDock, "Views.DistributionChart.DockButton");
            this.btnDock.Location = new System.Drawing.Point(153, 2);
            this.btnDock.Name = "btnDock";
            this.btnDock.Size = new System.Drawing.Size(16, 16);
            this.btnDock.TabIndex = 1;
            this.btnDock.Click += new System.EventHandler(this.btnDock_Click);
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
            this.locExtender.SetLocalizationComment(this.btnAutoHide, null);
            this.locExtender.SetLocalizingId(this.btnAutoHide, "Views.DistributionChart.AutoHideButton");
            this.btnAutoHide.Location = new System.Drawing.Point(134, 2);
            this.btnAutoHide.Name = "btnAutoHide";
            this.btnAutoHide.Size = new System.Drawing.Size(16, 16);
            this.btnAutoHide.TabIndex = 0;
            this.btnAutoHide.Click += new System.EventHandler(this.btnAutoHide_Click);
            // 
            // pnlSavedCharts
            // 
            this.pnlSavedCharts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.pnlSavedCharts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSavedCharts.ClipTextForChildControls = true;
            this.pnlSavedCharts.ControlReceivingFocusOnMnemonic = null;
            this.pnlSavedCharts.Controls.Add(this.lvSavedCharts);
            this.pnlSavedCharts.Controls.Add(this.hlblSavedCharts);
            this.pnlSavedCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSavedCharts.DoubleBuffered = false;
            this.pnlSavedCharts.DrawOnlyBottomBorder = false;
            this.pnlSavedCharts.DrawOnlyTopBorder = false;
            this.pnlSavedCharts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.pnlSavedCharts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.pnlSavedCharts, null);
            this.locExtender.SetLocalizationComment(this.pnlSavedCharts, null);
            this.locExtender.SetLocalizationPriority(this.pnlSavedCharts, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.pnlSavedCharts, "DistributionChartVw.pnlSavedCharts");
            this.pnlSavedCharts.Location = new System.Drawing.Point(0, 0);
            this.pnlSavedCharts.MnemonicGeneratesClick = false;
            this.pnlSavedCharts.Name = "pnlSavedCharts";
            this.pnlSavedCharts.PaintExplorerBarBackground = false;
            this.pnlSavedCharts.Size = new System.Drawing.Size(177, 264);
            this.pnlSavedCharts.TabIndex = 0;
            this.pnlSavedCharts.Resize += new System.EventHandler(this.pnlSavedCharts_Resize);
            // 
            // lvSavedCharts
            // 
            this.lvSavedCharts.AutoArrange = false;
            this.lvSavedCharts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSavedCharts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrSavedCharts});
            this.lvSavedCharts.FullRowSelect = true;
            this.lvSavedCharts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSavedCharts.HideSelection = false;
            this.lvSavedCharts.LabelEdit = true;
            this.lvSavedCharts.Location = new System.Drawing.Point(0, 24);
            this.lvSavedCharts.MultiSelect = false;
            this.lvSavedCharts.Name = "lvSavedCharts";
            this.lvSavedCharts.ShowGroups = false;
            this.lvSavedCharts.Size = new System.Drawing.Size(132, 160);
            this.lvSavedCharts.TabIndex = 1;
            this.lvSavedCharts.UseCompatibleStateImageBehavior = false;
            this.lvSavedCharts.View = System.Windows.Forms.View.Details;
            this.lvSavedCharts.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvSavedCharts_AfterLabelEdit);
            this.lvSavedCharts.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvSavedCharts_BeforeLabelEdit);
            this.lvSavedCharts.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvSavedCharts_ItemDrag);
            this.lvSavedCharts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvSavedCharts_ItemSelectionChanged);
            this.lvSavedCharts.Enter += new System.EventHandler(this.lvSavedCharts_Enter);
            this.lvSavedCharts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSavedCharts_KeyDown);
            this.lvSavedCharts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvSavedCharts_MouseDoubleClick);
            // 
            // hdrSavedCharts
            // 
            this.locExtender.SetLocalizableToolTip(this.hdrSavedCharts, null);
            this.locExtender.SetLocalizationComment(this.hdrSavedCharts, null);
            this.locExtender.SetLocalizationPriority(this.hdrSavedCharts, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.hdrSavedCharts, "DistributionChartVw.lvSavedCharts");
            // 
            // hlblSavedCharts
            // 
            this.hlblSavedCharts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.hlblSavedCharts.ClipTextForChildControls = true;
            this.hlblSavedCharts.ControlReceivingFocusOnMnemonic = null;
            this.hlblSavedCharts.Controls.Add(this.btnRestoreDefaultCharts);
            this.hlblSavedCharts.Controls.Add(this.btnRemoveSavedChart);
            this.hlblSavedCharts.Dock = System.Windows.Forms.DockStyle.Top;
            this.hlblSavedCharts.DoubleBuffered = true;
            this.hlblSavedCharts.DrawOnlyBottomBorder = false;
            this.hlblSavedCharts.DrawOnlyTopBorder = false;
            this.hlblSavedCharts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.hlblSavedCharts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.hlblSavedCharts, null);
            this.locExtender.SetLocalizationComment(this.hlblSavedCharts, null);
            this.locExtender.SetLocalizingId(this.hlblSavedCharts, "Views.DistributionChart.SavedChartsHeadingLabel");
            this.hlblSavedCharts.Location = new System.Drawing.Point(0, 0);
            this.hlblSavedCharts.MnemonicGeneratesClick = false;
            this.hlblSavedCharts.Name = "hlblSavedCharts";
            this.hlblSavedCharts.PaintExplorerBarBackground = false;
            this.hlblSavedCharts.ShowWindowBackgroudOnTopAndRightEdge = true;
            this.hlblSavedCharts.Size = new System.Drawing.Size(175, 24);
            this.hlblSavedCharts.TabIndex = 0;
            this.hlblSavedCharts.Text = "Save&d Charts";
            // 
            // btnRestoreDefaultCharts
            // 
            this.btnRestoreDefaultCharts.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRestoreDefaultCharts.BackColor = System.Drawing.Color.Transparent;
            this.btnRestoreDefaultCharts.CanBeChecked = false;
            this.btnRestoreDefaultCharts.Checked = false;
            this.btnRestoreDefaultCharts.DrawEmpty = false;
            this.btnRestoreDefaultCharts.DrawLeftArrowButton = false;
            this.btnRestoreDefaultCharts.DrawRightArrowButton = false;
            this.btnRestoreDefaultCharts.Font = new System.Drawing.Font("Marlett", 9F);
            this.btnRestoreDefaultCharts.Image = global::SIL.Pa.Properties.Resources.Restore_default_layout;
            this.btnRestoreDefaultCharts.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.btnRestoreDefaultCharts, "Restore Default Charts");
            this.locExtender.SetLocalizationComment(this.btnRestoreDefaultCharts, "Button to Restore default charts on the distribution charts view. The button is o" +
        "n the right side of the heading over the saved charts list.");
            this.locExtender.SetLocalizationPriority(this.btnRestoreDefaultCharts, Localization.LocalizationPriority.MediumHigh);
            this.locExtender.SetLocalizingId(this.btnRestoreDefaultCharts, "Views.DistributionChart.RestoreDefaultChartsButton");
            this.btnRestoreDefaultCharts.Location = new System.Drawing.Point(127, 2);
            this.btnRestoreDefaultCharts.Name = "btnRestoreDefaultCharts";
            this.btnRestoreDefaultCharts.Size = new System.Drawing.Size(20, 20);
            this.btnRestoreDefaultCharts.TabIndex = 1;
            this.btnRestoreDefaultCharts.Click += new System.EventHandler(this.btnRestoreDefaultCharts_Click);
            // 
            // btnRemoveSavedChart
            // 
            this.btnRemoveSavedChart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRemoveSavedChart.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveSavedChart.CanBeChecked = false;
            this.btnRemoveSavedChart.Checked = false;
            this.btnRemoveSavedChart.DrawEmpty = false;
            this.btnRemoveSavedChart.DrawLeftArrowButton = false;
            this.btnRemoveSavedChart.DrawRightArrowButton = false;
            this.btnRemoveSavedChart.Font = new System.Drawing.Font("Marlett", 9F);
            this.btnRemoveSavedChart.Image = global::SIL.Pa.Properties.Resources.kimidDelete;
            this.btnRemoveSavedChart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.btnRemoveSavedChart, "Remove Saved Chart");
            this.locExtender.SetLocalizationComment(this.btnRemoveSavedChart, "Button to delete saved charts on the distribution charts view. The button is on t" +
        "he right side of the heading over the saved charts list.");
            this.locExtender.SetLocalizationPriority(this.btnRemoveSavedChart, Localization.LocalizationPriority.MediumHigh);
            this.locExtender.SetLocalizingId(this.btnRemoveSavedChart, "Views.DistributionChart.RemoveSavedChartButton");
            this.btnRemoveSavedChart.Location = new System.Drawing.Point(148, 2);
            this.btnRemoveSavedChart.Name = "btnRemoveSavedChart";
            this.btnRemoveSavedChart.Size = new System.Drawing.Size(20, 20);
            this.btnRemoveSavedChart.TabIndex = 0;
            this.btnRemoveSavedChart.Click += new System.EventHandler(this.btnRemoveSavedChart_Click);
            // 
            // splitChart
            // 
            this.splitChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitChart.Location = new System.Drawing.Point(0, 0);
            this.splitChart.Name = "splitChart";
            this.splitChart.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitChart.Panel1
            // 
            this.splitChart.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitChart.Panel1.Controls.Add(this.lblChartName);
            this.splitChart.Panel1.Controls.Add(this.lblChartNameValue);
            this.splitChart.Panel1.Padding = new System.Windows.Forms.Padding(0, 28, 0, 0);
            // 
            // splitChart.Panel2
            // 
            this.splitChart.Panel2.Controls.Add(this.splitResults);
            this.splitChart.Size = new System.Drawing.Size(431, 523);
            this.splitChart.SplitterDistance = 240;
            this.splitChart.SplitterWidth = 8;
            this.splitChart.TabIndex = 0;
            // 
            // lblChartName
            // 
            this.lblChartName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblChartName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.lblChartName, null);
            this.locExtender.SetLocalizationComment(this.lblChartName, null);
            this.locExtender.SetLocalizingId(this.lblChartName, "Views.DistributionChart.ChartNameLabel");
            this.lblChartName.Location = new System.Drawing.Point(3, 5);
            this.lblChartName.Name = "lblChartName";
            this.lblChartName.Size = new System.Drawing.Size(80, 18);
            this.lblChartName.TabIndex = 0;
            this.lblChartName.Text = "Chart Na&me:";
            this.lblChartName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblChartNameValue
            // 
            this.lblChartNameValue.AutoSize = true;
            this.lblChartNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblChartNameValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.lblChartNameValue, null);
            this.locExtender.SetLocalizationComment(this.lblChartNameValue, null);
            this.locExtender.SetLocalizationPriority(this.lblChartNameValue, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.lblChartNameValue, "DistributionChart.ChartNameValueLabel");
            this.lblChartNameValue.Location = new System.Drawing.Point(89, 426);
            this.lblChartNameValue.Name = "lblChartNameValue";
            this.lblChartNameValue.Size = new System.Drawing.Size(18, 20);
            this.lblChartNameValue.TabIndex = 1;
            this.lblChartNameValue.Text = "#";
            this.lblChartNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSliderPlaceholder
            // 
            this.pnlSliderPlaceholder.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSliderPlaceholder.Location = new System.Drawing.Point(0, 0);
            this.pnlSliderPlaceholder.Name = "pnlSliderPlaceholder";
            this.pnlSliderPlaceholder.Size = new System.Drawing.Size(32, 531);
            this.pnlSliderPlaceholder.TabIndex = 0;
            // 
            // pnlOuter
            // 
            this.pnlOuter.Controls.Add(this.splitOuter);
            this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuter.Location = new System.Drawing.Point(32, 0);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Padding = new System.Windows.Forms.Padding(0, 0, 8, 8);
            this.pnlOuter.Size = new System.Drawing.Size(632, 531);
            this.pnlOuter.TabIndex = 3;
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // DistributionChartVw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuter);
            this.Controls.Add(this.pnlSliderPlaceholder);
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this, "DistributionChartVw");
            this.Name = "DistributionChartVw";
            this.Size = new System.Drawing.Size(664, 531);
            this.splitResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitResults)).EndInit();
            this.splitResults.ResumeLayout(false);
            this.splitOuter.Panel1.ResumeLayout(false);
            this.splitOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitOuter)).EndInit();
            this.splitOuter.ResumeLayout(false);
            this.splitSideBarOuter.Panel1.ResumeLayout(false);
            this.splitSideBarOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSideBarOuter)).EndInit();
            this.splitSideBarOuter.ResumeLayout(false);
            this.pnlTabClassDef.ResumeLayout(false);
            this.pnlSideBarCaption.ResumeLayout(false);
            this.pnlSavedCharts.ResumeLayout(false);
            this.hlblSavedCharts.ResumeLayout(false);
            this.splitChart.Panel1.ResumeLayout(false);
            this.splitChart.Panel1.PerformLayout();
            this.splitChart.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitChart)).EndInit();
            this.splitChart.ResumeLayout(false);
            this.pnlOuter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitSideBarOuter;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitResults;
		private System.Windows.Forms.Panel pnlSliderPlaceholder;
		private SilGradientPanel pnlSideBarCaption;
		private System.Windows.Forms.Panel pnlTabClassDef;
		private System.Windows.Forms.Panel pnlOuter;
		private System.Windows.Forms.SplitContainer splitChart;
		private HeaderLabel hlblSavedCharts;
		private SilPanel pnlSavedCharts;
		private System.Windows.Forms.ListView lvSavedCharts;
		private System.Windows.Forms.ColumnHeader hdrSavedCharts;
		private SIL.Pa.UI.Controls.AutoHideDockButton btnAutoHide;
		private SIL.Pa.UI.Controls.AutoHideDockButton btnDock;
		private XButton btnRemoveSavedChart;
		private SIL.Pa.UI.Controls.PatternBuilderComponents ptrnBldrComponent;
		private System.Windows.Forms.Label lblChartName;
		private System.Windows.Forms.Label lblChartNameValue;
		private Localization.UI.LocalizationExtender locExtender;
		private Controls.RecordViewControls.RecordViewPanel _recView;
        private XButton btnRestoreDefaultCharts;
	}
}