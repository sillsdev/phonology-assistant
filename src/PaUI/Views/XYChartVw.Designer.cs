namespace SIL.Pa.UI.Views
{
	partial class XYChartVw
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XYChartVw));
			this.splitResults = new System.Windows.Forms.SplitContainer();
			this.pnlRecView = new SIL.Pa.UI.Controls.PaPanel();
			this.rtfRecVw = new SIL.Pa.UI.Controls.RtfRecordView();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitSideBarOuter = new System.Windows.Forms.SplitContainer();
			this.pnlTabClassDef = new System.Windows.Forms.Panel();
			this.ptrnBldrComponent = new SIL.Pa.UI.Controls.PatternBuilderComponents();
			this.pnlSideBarCaption = new SIL.Pa.UI.Controls.PaGradientPanel();
			this.btnDock = new SIL.Pa.UI.Controls.AutoHideDockButton();
			this.btnAutoHide = new SIL.Pa.UI.Controls.AutoHideDockButton();
			this.pnlSavedCharts = new SIL.Pa.UI.Controls.PaPanel();
			this.lvSavedCharts = new System.Windows.Forms.ListView();
			this.hdrSavedCharts = new System.Windows.Forms.ColumnHeader();
			this.hlblSavedCharts = new SIL.Pa.UI.Controls.HeaderLabel();
			this.btnRemoveSavedChart = new SIL.Pa.UI.Controls.XButton();
			this.splitChart = new System.Windows.Forms.SplitContainer();
			this.lblChartName = new System.Windows.Forms.Label();
			this.lblChartNameValue = new System.Windows.Forms.Label();
			this.pnlSliderPlaceholder = new System.Windows.Forms.Panel();
			this.pnlOuter = new System.Windows.Forms.Panel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.splitResults.Panel2.SuspendLayout();
			this.splitResults.SuspendLayout();
			this.pnlRecView.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitSideBarOuter.Panel1.SuspendLayout();
			this.splitSideBarOuter.Panel2.SuspendLayout();
			this.splitSideBarOuter.SuspendLayout();
			this.pnlTabClassDef.SuspendLayout();
			this.pnlSideBarCaption.SuspendLayout();
			this.pnlSavedCharts.SuspendLayout();
			this.hlblSavedCharts.SuspendLayout();
			this.splitChart.Panel1.SuspendLayout();
			this.splitChart.Panel2.SuspendLayout();
			this.splitChart.SuspendLayout();
			this.pnlOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
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
			// 
			// splitResults.Panel2
			// 
			this.splitResults.Panel2.Controls.Add(this.pnlRecView);
			// 
			// pnlRecView
			// 
			this.pnlRecView.BackColor = System.Drawing.SystemColors.Window;
			this.pnlRecView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlRecView.ClipTextForChildControls = true;
			this.pnlRecView.ControlReceivingFocusOnMnemonic = null;
			this.pnlRecView.Controls.Add(this.rtfRecVw);
			resources.ApplyResources(this.pnlRecView, "pnlRecView");
			this.pnlRecView.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlRecView, null);
			this.locExtender.SetLocalizationComment(this.pnlRecView, null);
			this.locExtender.SetLocalizationPriority(this.pnlRecView, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlRecView, "XYChartVw.pnlRecView");
			this.pnlRecView.MnemonicGeneratesClick = false;
			this.pnlRecView.Name = "pnlRecView";
			this.pnlRecView.PaintExplorerBarBackground = false;
			// 
			// rtfRecVw
			// 
			this.rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtfRecVw, "rtfRecVw");
			this.locExtender.SetLocalizableToolTip(this.rtfRecVw, null);
			this.locExtender.SetLocalizationComment(this.rtfRecVw, null);
			this.locExtender.SetLocalizationPriority(this.rtfRecVw, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.rtfRecVw, "XYChartVw.rtfRecVw");
			this.rtfRecVw.Name = "rtfRecVw";
			this.rtfRecVw.ReadOnly = true;
			this.rtfRecVw.TabStop = false;
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
			this.splitOuter.Panel2.Controls.Add(this.splitChart);
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
			this.splitSideBarOuter.Panel2.Controls.Add(this.pnlSavedCharts);
			this.splitSideBarOuter.TabStop = false;
			// 
			// pnlTabClassDef
			// 
			this.pnlTabClassDef.Controls.Add(this.ptrnBldrComponent);
			resources.ApplyResources(this.pnlTabClassDef, "pnlTabClassDef");
			this.pnlTabClassDef.Name = "pnlTabClassDef";
			// 
			// ptrnBldrComponent
			// 
			resources.ApplyResources(this.ptrnBldrComponent, "ptrnBldrComponent");
			this.locExtender.SetLocalizableToolTip(this.ptrnBldrComponent, null);
			this.locExtender.SetLocalizationComment(this.ptrnBldrComponent, null);
			this.locExtender.SetLocalizationPriority(this.ptrnBldrComponent, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.ptrnBldrComponent, "XYChartVw.PatternBuilderComponents");
			this.ptrnBldrComponent.Name = "ptrnBldrComponent";
			// 
			// pnlSideBarCaption
			// 
			this.pnlSideBarCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSideBarCaption.ClipTextForChildControls = true;
			this.pnlSideBarCaption.ControlReceivingFocusOnMnemonic = null;
			this.pnlSideBarCaption.Controls.Add(this.btnDock);
			this.pnlSideBarCaption.Controls.Add(this.btnAutoHide);
			resources.ApplyResources(this.pnlSideBarCaption, "pnlSideBarCaption");
			this.pnlSideBarCaption.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlSideBarCaption, null);
			this.locExtender.SetLocalizationComment(this.pnlSideBarCaption, null);
			this.locExtender.SetLocalizationPriority(this.pnlSideBarCaption, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSideBarCaption, "XYChartVw.pnlSideBarCaption");
			this.pnlSideBarCaption.MakeDark = false;
			this.pnlSideBarCaption.MnemonicGeneratesClick = false;
			this.pnlSideBarCaption.Name = "pnlSideBarCaption";
			this.pnlSideBarCaption.PaintExplorerBarBackground = false;
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
			this.locExtender.SetLocalizationComment(this.btnDock, null);
			this.locExtender.SetLocalizationPriority(this.btnDock, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnDock, "XYChartVw.btnDock");
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
			this.locExtender.SetLocalizationComment(this.btnAutoHide, null);
			this.locExtender.SetLocalizationPriority(this.btnAutoHide, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnAutoHide, "XYChartVw.btnAutoHide");
			this.btnAutoHide.Name = "btnAutoHide";
			this.btnAutoHide.Click += new System.EventHandler(this.btnAutoHide_Click);
			// 
			// pnlSavedCharts
			// 
			this.pnlSavedCharts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSavedCharts.ClipTextForChildControls = true;
			this.pnlSavedCharts.ControlReceivingFocusOnMnemonic = null;
			this.pnlSavedCharts.Controls.Add(this.lvSavedCharts);
			this.pnlSavedCharts.Controls.Add(this.hlblSavedCharts);
			resources.ApplyResources(this.pnlSavedCharts, "pnlSavedCharts");
			this.pnlSavedCharts.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlSavedCharts, null);
			this.locExtender.SetLocalizationComment(this.pnlSavedCharts, null);
			this.locExtender.SetLocalizationPriority(this.pnlSavedCharts, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlSavedCharts, "XYChartVw.pnlSavedCharts");
			this.pnlSavedCharts.MnemonicGeneratesClick = false;
			this.pnlSavedCharts.Name = "pnlSavedCharts";
			this.pnlSavedCharts.PaintExplorerBarBackground = false;
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
			resources.ApplyResources(this.lvSavedCharts, "lvSavedCharts");
			this.lvSavedCharts.MultiSelect = false;
			this.lvSavedCharts.Name = "lvSavedCharts";
			this.lvSavedCharts.ShowGroups = false;
			this.lvSavedCharts.UseCompatibleStateImageBehavior = false;
			this.lvSavedCharts.View = System.Windows.Forms.View.Details;
			this.lvSavedCharts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvSavedCharts_MouseDoubleClick);
			this.lvSavedCharts.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvSavedCharts_AfterLabelEdit);
			this.lvSavedCharts.Enter += new System.EventHandler(this.lvSavedCharts_Enter);
			this.lvSavedCharts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvSavedCharts_ItemSelectionChanged);
			this.lvSavedCharts.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvSavedCharts_BeforeLabelEdit);
			this.lvSavedCharts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSavedCharts_KeyDown);
			this.lvSavedCharts.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvSavedCharts_ItemDrag);
			// 
			// hdrSavedCharts
			// 
			this.locExtender.SetLocalizableToolTip(this.hdrSavedCharts, null);
			this.locExtender.SetLocalizationComment(this.hdrSavedCharts, null);
			this.locExtender.SetLocalizationPriority(this.hdrSavedCharts, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.hdrSavedCharts, "XYChartVw.lvSavedCharts");
			// 
			// hlblSavedCharts
			// 
			this.hlblSavedCharts.ClipTextForChildControls = true;
			this.hlblSavedCharts.ControlReceivingFocusOnMnemonic = null;
			this.hlblSavedCharts.Controls.Add(this.btnRemoveSavedChart);
			resources.ApplyResources(this.hlblSavedCharts, "hlblSavedCharts");
			this.locExtender.SetLocalizableToolTip(this.hlblSavedCharts, null);
			this.locExtender.SetLocalizationComment(this.hlblSavedCharts, null);
			this.locExtender.SetLocalizingId(this.hlblSavedCharts, "XYChartVw.hlblSavedCharts");
			this.hlblSavedCharts.MnemonicGeneratesClick = false;
			this.hlblSavedCharts.Name = "hlblSavedCharts";
			this.hlblSavedCharts.ShowWindowBackgroudOnTopAndRightEdge = true;
			// 
			// btnRemoveSavedChart
			// 
			resources.ApplyResources(this.btnRemoveSavedChart, "btnRemoveSavedChart");
			this.btnRemoveSavedChart.BackColor = System.Drawing.Color.Transparent;
			this.btnRemoveSavedChart.CanBeChecked = false;
			this.btnRemoveSavedChart.Checked = false;
			this.btnRemoveSavedChart.DrawEmpty = false;
			this.btnRemoveSavedChart.DrawLeftArrowButton = false;
			this.btnRemoveSavedChart.DrawRightArrowButton = false;
			this.btnRemoveSavedChart.Image = global::SIL.Pa.Properties.Resources.kimidDelete;
			this.locExtender.SetLocalizableToolTip(this.btnRemoveSavedChart, "Remove Saved Chart");
			this.locExtender.SetLocalizationComment(this.btnRemoveSavedChart, "Button to delete saved charts on the XY charts view. The button is on the right s" +
					"ide of the heading over the saved charts list.");
			this.locExtender.SetLocalizationPriority(this.btnRemoveSavedChart, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.btnRemoveSavedChart, "XYChartVw.btnRemoveSavedChart");
			this.btnRemoveSavedChart.Name = "btnRemoveSavedChart";
			this.btnRemoveSavedChart.Click += new System.EventHandler(this.btnRemoveSavedChart_Click);
			// 
			// splitChart
			// 
			resources.ApplyResources(this.splitChart, "splitChart");
			this.splitChart.Name = "splitChart";
			// 
			// splitChart.Panel1
			// 
			this.splitChart.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitChart.Panel1.Controls.Add(this.lblChartName);
			this.splitChart.Panel1.Controls.Add(this.lblChartNameValue);
			resources.ApplyResources(this.splitChart.Panel1, "splitChart.Panel1");
			// 
			// splitChart.Panel2
			// 
			this.splitChart.Panel2.Controls.Add(this.splitResults);
			// 
			// lblChartName
			// 
			resources.ApplyResources(this.lblChartName, "lblChartName");
			this.locExtender.SetLocalizableToolTip(this.lblChartName, null);
			this.locExtender.SetLocalizationComment(this.lblChartName, null);
			this.locExtender.SetLocalizingId(this.lblChartName, "XYChartVw.lblChartName");
			this.lblChartName.Name = "lblChartName";
			// 
			// lblChartNameValue
			// 
			resources.ApplyResources(this.lblChartNameValue, "lblChartNameValue");
			this.locExtender.SetLocalizableToolTip(this.lblChartNameValue, null);
			this.locExtender.SetLocalizationComment(this.lblChartNameValue, null);
			this.locExtender.SetLocalizationPriority(this.lblChartNameValue, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblChartNameValue, "XYChartVw.lblChartNameValue");
			this.lblChartNameValue.Name = "lblChartNameValue";
			// 
			// pnlSliderPlaceholder
			// 
			resources.ApplyResources(this.pnlSliderPlaceholder, "pnlSliderPlaceholder");
			this.pnlSliderPlaceholder.Name = "pnlSliderPlaceholder";
			// 
			// pnlOuter
			// 
			this.pnlOuter.Controls.Add(this.splitOuter);
			resources.ApplyResources(this.pnlOuter, "pnlOuter");
			this.pnlOuter.Name = "pnlOuter";
			// 
			// XYChartVw
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlOuter);
			this.Controls.Add(this.pnlSliderPlaceholder);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "XYChartVw.XYChartVw");
			this.Name = "XYChartVw";
			this.splitResults.Panel2.ResumeLayout(false);
			this.splitResults.ResumeLayout(false);
			this.pnlRecView.ResumeLayout(false);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitSideBarOuter.Panel1.ResumeLayout(false);
			this.splitSideBarOuter.Panel2.ResumeLayout(false);
			this.splitSideBarOuter.ResumeLayout(false);
			this.pnlTabClassDef.ResumeLayout(false);
			this.pnlSideBarCaption.ResumeLayout(false);
			this.pnlSavedCharts.ResumeLayout(false);
			this.hlblSavedCharts.ResumeLayout(false);
			this.splitChart.Panel1.ResumeLayout(false);
			this.splitChart.Panel1.PerformLayout();
			this.splitChart.Panel2.ResumeLayout(false);
			this.splitChart.ResumeLayout(false);
			this.pnlOuter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitSideBarOuter;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitResults;
		private SIL.Pa.UI.Controls.PaPanel pnlRecView;
		private SIL.Pa.UI.Controls.RtfRecordView rtfRecVw;
		private System.Windows.Forms.Panel pnlSliderPlaceholder;
		private SIL.Pa.UI.Controls.PaGradientPanel pnlSideBarCaption;
		private System.Windows.Forms.Panel pnlTabClassDef;
		private System.Windows.Forms.Panel pnlOuter;
		private System.Windows.Forms.SplitContainer splitChart;
		private SIL.Pa.UI.Controls.HeaderLabel hlblSavedCharts;
		private SIL.Pa.UI.Controls.PaPanel pnlSavedCharts;
		private System.Windows.Forms.ListView lvSavedCharts;
		private System.Windows.Forms.ColumnHeader hdrSavedCharts;
		private SIL.Pa.UI.Controls.AutoHideDockButton btnAutoHide;
		private SIL.Pa.UI.Controls.AutoHideDockButton btnDock;
		private SIL.Pa.UI.Controls.XButton btnRemoveSavedChart;
		private SIL.Pa.UI.Controls.PatternBuilderComponents ptrnBldrComponent;
		private System.Windows.Forms.Label lblChartName;
		private System.Windows.Forms.Label lblChartNameValue;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}