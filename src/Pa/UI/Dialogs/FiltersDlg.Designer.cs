using SilTools.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class FiltersDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiltersDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lvFilters = new System.Windows.Forms.ListView();
			this.hdrFilter = new System.Windows.Forms.ColumnHeader();
			this.splitFilters = new System.Windows.Forms.SplitContainer();
			this.pnlFilters = new SilTools.Controls.SilPanel();
			this.btnApplyNow = new System.Windows.Forms.Button();
			this.hlblFilters = new SilTools.Controls.HeaderLabel();
			this.flwLayoutFilterButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnDeleteFilter = new System.Windows.Forms.Button();
			this.pnlExpressions = new SilTools.Controls.SilPanel();
			this.m_grid = new SilTools.SilGrid();
			this.hlblExpressions = new SilTools.Controls.HeaderLabel();
			this.pnlFilterOptions = new System.Windows.Forms.Panel();
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.rbMatchAll = new System.Windows.Forms.RadioButton();
			this.chkIncludeInList = new System.Windows.Forms.CheckBox();
			this.rbMatchAny = new System.Windows.Forms.RadioButton();
			this.btnRemoveExp = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.splitFilters.Panel1.SuspendLayout();
			this.splitFilters.Panel2.SuspendLayout();
			this.splitFilters.SuspendLayout();
			this.pnlFilters.SuspendLayout();
			this.flwLayoutFilterButtons.SuspendLayout();
			this.pnlExpressions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.pnlFilterOptions.SuspendLayout();
			this.tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lvFilters
			// 
			this.lvFilters.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvFilters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrFilter});
			resources.ApplyResources(this.lvFilters, "lvFilters");
			this.lvFilters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvFilters.HideSelection = false;
			this.lvFilters.LabelEdit = true;
			this.lvFilters.MultiSelect = false;
			this.lvFilters.Name = "lvFilters";
			this.lvFilters.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvFilters.UseCompatibleStateImageBehavior = false;
			this.lvFilters.View = System.Windows.Forms.View.Details;
			this.lvFilters.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvFilters_AfterLabelEdit);
			this.lvFilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFilters_KeyDown);
			// 
			// hdrFilter
			// 
			this.locExtender.SetLocalizableToolTip(this.hdrFilter, null);
			this.locExtender.SetLocalizationComment(this.hdrFilter, null);
			this.locExtender.SetLocalizingId(this.hdrFilter, "FiltersDlg.lvFiltersColhdrFilter");
			resources.ApplyResources(this.hdrFilter, "hdrFilter");
			// 
			// splitFilters
			// 
			resources.ApplyResources(this.splitFilters, "splitFilters");
			this.splitFilters.Name = "splitFilters";
			// 
			// splitFilters.Panel1
			// 
			this.splitFilters.Panel1.Controls.Add(this.pnlFilters);
			this.splitFilters.Panel1.Controls.Add(this.flwLayoutFilterButtons);
			// 
			// splitFilters.Panel2
			// 
			this.splitFilters.Panel2.Controls.Add(this.pnlExpressions);
			this.splitFilters.Panel2.Controls.Add(this.pnlFilterOptions);
			this.splitFilters.TabStop = false;
			this.splitFilters.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitFilter_SplitterMoved);
			// 
			// pnlFilters
			// 
			this.pnlFilters.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFilters.ClipTextForChildControls = true;
			this.pnlFilters.ControlReceivingFocusOnMnemonic = null;
			this.pnlFilters.Controls.Add(this.btnApplyNow);
			this.pnlFilters.Controls.Add(this.lvFilters);
			this.pnlFilters.Controls.Add(this.hlblFilters);
			resources.ApplyResources(this.pnlFilters, "pnlFilters");
			this.pnlFilters.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlFilters, null);
			this.locExtender.SetLocalizationComment(this.pnlFilters, null);
			this.locExtender.SetLocalizingId(this.pnlFilters, "FiltersDlg.pnlFilters");
			this.pnlFilters.MnemonicGeneratesClick = false;
			this.pnlFilters.Name = "pnlFilters";
			this.pnlFilters.PaintExplorerBarBackground = false;
			// 
			// btnApplyNow
			// 
			resources.ApplyResources(this.btnApplyNow, "btnApplyNow");
			this.locExtender.SetLocalizableToolTip(this.btnApplyNow, null);
			this.locExtender.SetLocalizationComment(this.btnApplyNow, null);
			this.locExtender.SetLocalizingId(this.btnApplyNow, "FiltersDlg.btnApplyNow");
			this.btnApplyNow.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnApplyNow.Name = "btnApplyNow";
			this.btnApplyNow.UseVisualStyleBackColor = true;
			this.btnApplyNow.Click += new System.EventHandler(this.btnApplyNow_Click);
			// 
			// hlblFilters
			// 
			this.hlblFilters.ClipTextForChildControls = false;
			this.hlblFilters.ControlReceivingFocusOnMnemonic = this.lvFilters;
			resources.ApplyResources(this.hlblFilters, "hlblFilters");
			this.locExtender.SetLocalizableToolTip(this.hlblFilters, null);
			this.locExtender.SetLocalizationComment(this.hlblFilters, null);
			this.locExtender.SetLocalizingId(this.hlblFilters, "FiltersDlg.hlblFilters");
			this.hlblFilters.MnemonicGeneratesClick = true;
			this.hlblFilters.Name = "hlblFilters";
			this.hlblFilters.ShowWindowBackgroudOnTopAndRightEdge = false;
			// 
			// flwLayoutFilterButtons
			// 
			resources.ApplyResources(this.flwLayoutFilterButtons, "flwLayoutFilterButtons");
			this.flwLayoutFilterButtons.Controls.Add(this.btnAdd);
			this.flwLayoutFilterButtons.Controls.Add(this.btnCopy);
			this.flwLayoutFilterButtons.Controls.Add(this.btnDeleteFilter);
			this.flwLayoutFilterButtons.Name = "flwLayoutFilterButtons";
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.locExtender.SetLocalizableToolTip(this.btnAdd, null);
			this.locExtender.SetLocalizationComment(this.btnAdd, null);
			this.locExtender.SetLocalizingId(this.btnAdd, "FiltersDlg.btnAdd");
			this.btnAdd.MinimumSize = new System.Drawing.Size(70, 26);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnCopy
			// 
			resources.ApplyResources(this.btnCopy, "btnCopy");
			this.locExtender.SetLocalizableToolTip(this.btnCopy, null);
			this.locExtender.SetLocalizationComment(this.btnCopy, null);
			this.locExtender.SetLocalizingId(this.btnCopy, "FiltersDlg.btnCopy");
			this.btnCopy.MinimumSize = new System.Drawing.Size(70, 26);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnDeleteFilter
			// 
			resources.ApplyResources(this.btnDeleteFilter, "btnDeleteFilter");
			this.locExtender.SetLocalizableToolTip(this.btnDeleteFilter, null);
			this.locExtender.SetLocalizationComment(this.btnDeleteFilter, null);
			this.locExtender.SetLocalizingId(this.btnDeleteFilter, "FiltersDlg.btnDeleteFilter");
			this.btnDeleteFilter.MinimumSize = new System.Drawing.Size(70, 26);
			this.btnDeleteFilter.Name = "btnDeleteFilter";
			this.btnDeleteFilter.UseVisualStyleBackColor = true;
			this.btnDeleteFilter.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// pnlExpressions
			// 
			this.pnlExpressions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlExpressions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlExpressions.ClipTextForChildControls = true;
			this.pnlExpressions.ControlReceivingFocusOnMnemonic = null;
			this.pnlExpressions.Controls.Add(this.m_grid);
			this.pnlExpressions.Controls.Add(this.hlblExpressions);
			resources.ApplyResources(this.pnlExpressions, "pnlExpressions");
			this.pnlExpressions.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlExpressions, null);
			this.locExtender.SetLocalizationComment(this.pnlExpressions, null);
			this.locExtender.SetLocalizingId(this.pnlExpressions, "FiltersDlg.pnlExpressions");
			this.pnlExpressions.MnemonicGeneratesClick = false;
			this.pnlExpressions.Name = "pnlExpressions";
			this.pnlExpressions.PaintExplorerBarBackground = false;
			// 
			// m_grid
			// 
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToOrderColumns = true;
			this.m_grid.AllowUserToResizeColumns = false;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.m_grid, "m_grid");
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_grid, null);
			this.locExtender.SetLocalizationComment(this.m_grid, null);
			this.locExtender.SetLocalizingId(this.m_grid, "FiltersDlg.m_grid");
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.PaintHeaderAcrossFullGridWidth = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.WaterMark = "!";
			this.m_grid.Enter += new System.EventHandler(this.m_grid_Enter);
			this.m_grid.ColumnHeadersHeightChanged += new System.EventHandler(this.m_grid_ColumnHeadersHeightChanged);
			this.m_grid.Leave += new System.EventHandler(this.m_grid_Leave);
			this.m_grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.m_grid_CellFormatting);
			this.m_grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.m_grid_DefaultValuesNeeded);
			this.m_grid.CurrentRowChanged += new System.EventHandler(this.m_grid_CurrentRowChanged);
			this.m_grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_grid_KeyDown);
			// 
			// hlblExpressions
			// 
			this.hlblExpressions.ClipTextForChildControls = false;
			this.hlblExpressions.ControlReceivingFocusOnMnemonic = this.lvFilters;
			resources.ApplyResources(this.hlblExpressions, "hlblExpressions");
			this.locExtender.SetLocalizableToolTip(this.hlblExpressions, null);
			this.locExtender.SetLocalizationComment(this.hlblExpressions, null);
			this.locExtender.SetLocalizingId(this.hlblExpressions, "FiltersDlg.hlblExpressions");
			this.hlblExpressions.MnemonicGeneratesClick = true;
			this.hlblExpressions.Name = "hlblExpressions";
			this.hlblExpressions.ShowWindowBackgroudOnTopAndRightEdge = false;
			// 
			// pnlFilterOptions
			// 
			this.pnlFilterOptions.Controls.Add(this.tableLayout);
			resources.ApplyResources(this.pnlFilterOptions, "pnlFilterOptions");
			this.pnlFilterOptions.Name = "pnlFilterOptions";
			this.pnlFilterOptions.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFilterOptions_Paint);
			// 
			// tableLayout
			// 
			resources.ApplyResources(this.tableLayout, "tableLayout");
			this.tableLayout.BackColor = System.Drawing.Color.Transparent;
			this.tableLayout.Controls.Add(this.rbMatchAll, 0, 1);
			this.tableLayout.Controls.Add(this.chkIncludeInList, 1, 0);
			this.tableLayout.Controls.Add(this.rbMatchAny, 0, 0);
			this.tableLayout.Controls.Add(this.btnRemoveExp, 1, 1);
			this.tableLayout.Name = "tableLayout";
			// 
			// rbMatchAll
			// 
			resources.ApplyResources(this.rbMatchAll, "rbMatchAll");
			this.rbMatchAll.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbMatchAll, null);
			this.locExtender.SetLocalizationComment(this.rbMatchAll, null);
			this.locExtender.SetLocalizingId(this.rbMatchAll, "FiltersDlg.rbMatchAll");
			this.rbMatchAll.Name = "rbMatchAll";
			this.rbMatchAll.TabStop = true;
			this.rbMatchAll.UseVisualStyleBackColor = false;
			// 
			// chkIncludeInList
			// 
			resources.ApplyResources(this.chkIncludeInList, "chkIncludeInList");
			this.chkIncludeInList.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkIncludeInList, "\"Check to display this filter in the\\nfilter toolbar button’s drop-down list.\"");
			this.locExtender.SetLocalizationComment(this.chkIncludeInList, null);
			this.locExtender.SetLocalizingId(this.chkIncludeInList, "FiltersDlg.chkIncludeInList");
			this.chkIncludeInList.Name = "chkIncludeInList";
			this.chkIncludeInList.UseVisualStyleBackColor = false;
			// 
			// rbMatchAny
			// 
			resources.ApplyResources(this.rbMatchAny, "rbMatchAny");
			this.rbMatchAny.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.rbMatchAny, null);
			this.locExtender.SetLocalizationComment(this.rbMatchAny, null);
			this.locExtender.SetLocalizingId(this.rbMatchAny, "FiltersDlg.rbMatchAny");
			this.rbMatchAny.Name = "rbMatchAny";
			this.rbMatchAny.TabStop = true;
			this.rbMatchAny.UseVisualStyleBackColor = false;
			// 
			// btnRemoveExp
			// 
			resources.ApplyResources(this.btnRemoveExp, "btnRemoveExp");
			this.locExtender.SetLocalizableToolTip(this.btnRemoveExp, null);
			this.locExtender.SetLocalizationComment(this.btnRemoveExp, null);
			this.locExtender.SetLocalizingId(this.btnRemoveExp, "FiltersDlg.btnRemoveExp");
			this.btnRemoveExp.MinimumSize = new System.Drawing.Size(175, 26);
			this.btnRemoveExp.Name = "btnRemoveExp";
			this.btnRemoveExp.UseVisualStyleBackColor = true;
			this.btnRemoveExp.Click += new System.EventHandler(this.btnRemoveExp_Click);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// DefineFiltersDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitFilters);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FiltersDlg.WindowTitle");
			this.Name = "FiltersDlg";
			this.Controls.SetChildIndex(this.splitFilters, 0);
			this.splitFilters.Panel1.ResumeLayout(false);
			this.splitFilters.Panel1.PerformLayout();
			this.splitFilters.Panel2.ResumeLayout(false);
			this.splitFilters.ResumeLayout(false);
			this.pnlFilters.ResumeLayout(false);
			this.pnlFilters.PerformLayout();
			this.flwLayoutFilterButtons.ResumeLayout(false);
			this.flwLayoutFilterButtons.PerformLayout();
			this.pnlExpressions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.pnlFilterOptions.ResumeLayout(false);
			this.pnlFilterOptions.PerformLayout();
			this.tableLayout.ResumeLayout(false);
			this.tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvFilters;
		private System.Windows.Forms.SplitContainer splitFilters;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Button btnDeleteFilter;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ColumnHeader hdrFilter;
		private SilTools.SilGrid m_grid;
		private SilPanel pnlFilters;
		private HeaderLabel hlblFilters;
		private System.Windows.Forms.Panel pnlFilterOptions;
		private System.Windows.Forms.RadioButton rbMatchAny;
		private System.Windows.Forms.RadioButton rbMatchAll;
		private System.Windows.Forms.CheckBox chkIncludeInList;
		private System.Windows.Forms.Button btnApplyNow;
		private System.Windows.Forms.Button btnRemoveExp;
		private SilPanel pnlExpressions;
		private HeaderLabel hlblExpressions;
		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.FlowLayoutPanel flwLayoutFilterButtons;
	}
}