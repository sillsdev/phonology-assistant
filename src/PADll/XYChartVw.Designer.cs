namespace SIL.Pa
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
			this.pnlRecView = new SIL.Pa.Controls.PaPanel();
			this.rtfRecVw = new SIL.Pa.Controls.RtfRecordView();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitSideBarOuter = new System.Windows.Forms.SplitContainer();
			this.pnlTabClassDef = new System.Windows.Forms.Panel();
			this.ptrnBldrComponent = new SIL.Pa.Controls.PatternBuilderComponents();
			this.pnlSideBarCaption = new SIL.Pa.Controls.PaGradientPanel();
			this.btnDock = new SIL.Pa.Controls.DockButton();
			this.btnAutoHide = new SIL.Pa.Controls.AutoHideButton();
			this.pnlSavedCharts = new SIL.Pa.Controls.PaPanel();
			this.lvSavedCharts = new System.Windows.Forms.ListView();
			this.hdrSavedCharts = new System.Windows.Forms.ColumnHeader();
			this.hlblSavedCharts = new SIL.Pa.Controls.HeaderLabel();
			this.btnRemoveSavedChart = new SIL.Pa.Controls.XButton();
			this.splitChart = new System.Windows.Forms.SplitContainer();
			this.lblChartName = new System.Windows.Forms.Label();
			this.lblChartNameValue = new System.Windows.Forms.Label();
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.pnlSliderPlaceholder = new System.Windows.Forms.Panel();
			this.pnlOuter = new System.Windows.Forms.Panel();
			this.sblblMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.sblblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbProgress = new System.Windows.Forms.ToolStripProgressBar();
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
			this.pnlRecView.MnemonicGeneratesClick = false;
			this.pnlRecView.Name = "pnlRecView";
			this.pnlRecView.PaintExplorerBarBackground = false;
			// 
			// rtfRecVw
			// 
			this.rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtfRecVw, "rtfRecVw");
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
			this.pnlSavedCharts.MnemonicGeneratesClick = false;
			this.pnlSavedCharts.Name = "pnlSavedCharts";
			this.pnlSavedCharts.PaintExplorerBarBackground = false;
			// 
			// lvSavedCharts
			// 
			this.lvSavedCharts.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvSavedCharts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrSavedCharts});
			resources.ApplyResources(this.lvSavedCharts, "lvSavedCharts");
			this.lvSavedCharts.FullRowSelect = true;
			this.lvSavedCharts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvSavedCharts.HideSelection = false;
			this.lvSavedCharts.LabelEdit = true;
			this.lvSavedCharts.MultiSelect = false;
			this.lvSavedCharts.Name = "lvSavedCharts";
			this.lvSavedCharts.UseCompatibleStateImageBehavior = false;
			this.lvSavedCharts.View = System.Windows.Forms.View.Details;
			this.lvSavedCharts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvSavedCharts_MouseDoubleClick);
			this.lvSavedCharts.Resize += new System.EventHandler(this.lvSavedCharts_Resize);
			this.lvSavedCharts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSavedCharts_KeyDown);
			this.lvSavedCharts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvSavedCharts_ItemSelectionChanged);
			this.lvSavedCharts.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvSavedCharts_AfterLabelEdit);
			this.lvSavedCharts.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvSavedCharts_ItemDrag);
			// 
			// hlblSavedCharts
			// 
			this.hlblSavedCharts.ClipTextForChildControls = true;
			this.hlblSavedCharts.ControlReceivingFocusOnMnemonic = null;
			this.hlblSavedCharts.Controls.Add(this.btnRemoveSavedChart);
			resources.ApplyResources(this.hlblSavedCharts, "hlblSavedCharts");
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
			this.btnRemoveSavedChart.Name = "btnRemoveSavedChart";
			this.m_tooltip.SetToolTip(this.btnRemoveSavedChart, resources.GetString("btnRemoveSavedChart.ToolTip"));
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
			this.lblChartName.Name = "lblChartName";
			// 
			// lblChartNameValue
			// 
			resources.ApplyResources(this.lblChartNameValue, "lblChartNameValue");
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
			// sblblMain
			// 
			resources.ApplyResources(this.sblblMain, "sblblMain");
			this.sblblMain.Name = "sblblMain";
			this.sblblMain.Spring = true;
			// 
			// sblblProgress
			// 
			this.sblblProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.sblblProgress.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.sblblProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sblblProgress.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
			this.sblblProgress.Name = "sblblProgress";
			resources.ApplyResources(this.sblblProgress, "sblblProgress");
			// 
			// sbProgress
			// 
			resources.ApplyResources(this.sbProgress, "sbProgress");
			this.sbProgress.Name = "sbProgress";
			// 
			// XYChartVw
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlOuter);
			this.Controls.Add(this.pnlSliderPlaceholder);
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
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip m_tooltip;
		private System.Windows.Forms.SplitContainer splitSideBarOuter;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitResults;
		private SIL.Pa.Controls.PaPanel pnlRecView;
		private SIL.Pa.Controls.RtfRecordView rtfRecVw;
		private System.Windows.Forms.Panel pnlSliderPlaceholder;
		private SIL.Pa.Controls.PaGradientPanel pnlSideBarCaption;
		private System.Windows.Forms.Panel pnlTabClassDef;
		private System.Windows.Forms.Panel pnlOuter;
		private System.Windows.Forms.SplitContainer splitChart;
		private SIL.Pa.Controls.HeaderLabel hlblSavedCharts;
		private SIL.Pa.Controls.PaPanel pnlSavedCharts;
		private System.Windows.Forms.ListView lvSavedCharts;
		private System.Windows.Forms.ColumnHeader hdrSavedCharts;
		private SIL.Pa.Controls.AutoHideButton btnAutoHide;
		private SIL.Pa.Controls.DockButton btnDock;
		private SIL.Pa.Controls.XButton btnRemoveSavedChart;
		private SIL.Pa.Controls.PatternBuilderComponents ptrnBldrComponent;
		private System.Windows.Forms.Label lblChartName;
		private System.Windows.Forms.ToolStripStatusLabel sblblMain;
		private System.Windows.Forms.ToolStripStatusLabel sblblProgress;
		private System.Windows.Forms.ToolStripProgressBar sbProgress;
		private System.Windows.Forms.Label lblChartNameValue;
	}
}