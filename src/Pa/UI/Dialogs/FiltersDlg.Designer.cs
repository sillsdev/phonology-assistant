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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitFilters = new System.Windows.Forms.SplitContainer();
			this.pnlFilters = new SilTools.Controls.SilPanel();
			this.m_gridFilters = new SilTools.SilGrid();
			this.flwLayoutFilterButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnDeleteFilter = new System.Windows.Forms.Button();
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlExpressionMatch = new SilTools.Controls.SilGradientPanel();
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.lblExpressionMatchMsgPart1 = new System.Windows.Forms.Label();
			this.cboExpressionMatch = new System.Windows.Forms.ComboBox();
			this.lblExpressionMatchMsgPart2 = new System.Windows.Forms.Label();
			this.pnlExpressions = new SilTools.Controls.SilPanel();
			this.btnApplyNow = new System.Windows.Forms.Button();
			this.m_gridExpressions = new SilTools.SilGrid();
			this.hlblExpressions = new SilTools.Controls.HeaderLabel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.splitFilters.Panel1.SuspendLayout();
			this.splitFilters.Panel2.SuspendLayout();
			this.splitFilters.SuspendLayout();
			this.pnlFilters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridFilters)).BeginInit();
			this.flwLayoutFilterButtons.SuspendLayout();
			this.tableLayout.SuspendLayout();
			this.pnlExpressionMatch.SuspendLayout();
			this.flowLayoutPanel.SuspendLayout();
			this.pnlExpressions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridExpressions)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
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
			this.splitFilters.Panel2.Controls.Add(this.tableLayout);
			this.splitFilters.TabStop = false;
			// 
			// pnlFilters
			// 
			this.pnlFilters.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFilters.ClipTextForChildControls = true;
			this.pnlFilters.ControlReceivingFocusOnMnemonic = null;
			this.pnlFilters.Controls.Add(this.m_gridFilters);
			resources.ApplyResources(this.pnlFilters, "pnlFilters");
			this.pnlFilters.DoubleBuffered = true;
			this.pnlFilters.DrawOnlyBottomBorder = false;
			this.pnlFilters.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlFilters.MnemonicGeneratesClick = false;
			this.pnlFilters.Name = "pnlFilters";
			this.pnlFilters.PaintExplorerBarBackground = false;
			// 
			// m_gridFilters
			// 
			this.m_gridFilters.AllowUserToAddRows = false;
			this.m_gridFilters.AllowUserToDeleteRows = false;
			this.m_gridFilters.AllowUserToOrderColumns = true;
			this.m_gridFilters.AllowUserToResizeRows = false;
			this.m_gridFilters.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_gridFilters.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_gridFilters.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gridFilters.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridFilters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_gridFilters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.m_gridFilters, "m_gridFilters");
			this.m_gridFilters.DrawTextBoxEditControlBorder = false;
			this.m_gridFilters.ExtendFullRowSelectRectangleToEdge = true;
			this.m_gridFilters.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_gridFilters.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.m_gridFilters.IsDirty = true;
			this.m_gridFilters.MultiSelect = false;
			this.m_gridFilters.Name = "m_gridFilters";
			this.m_gridFilters.PaintHeaderAcrossFullGridWidth = true;
			this.m_gridFilters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridFilters.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_gridFilters.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_gridFilters.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_gridFilters.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_gridFilters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridFilters.ShowWaterMarkWhenDirty = false;
			this.m_gridFilters.StandardTab = true;
			this.m_gridFilters.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_gridFilters.WaterMark = "!";
			this.m_gridFilters.CurrentRowChanged += new System.EventHandler(this.HandleFilterGridCurrentRowChanged);
			this.m_gridFilters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleFilterGridCellEndEdit);
			this.m_gridFilters.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.HandleFilterGridCellPainting);
			this.m_gridFilters.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.HandleFilterGridCellToolTipTextNeeded);
			this.m_gridFilters.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.HandleFilterGridCellValidating);
			this.m_gridFilters.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.HandleFilterGridCellValueNeeded);
			this.m_gridFilters.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.HandleFilterGridCellValuePushed);
			this.m_gridFilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleFilterGridKeyDown);
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
			this.btnAdd.MinimumSize = new System.Drawing.Size(70, 26);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.HandleButtonAddClick);
			// 
			// btnCopy
			// 
			resources.ApplyResources(this.btnCopy, "btnCopy");
			this.btnCopy.MinimumSize = new System.Drawing.Size(70, 26);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.HandleButtonCopyClick);
			// 
			// btnDeleteFilter
			// 
			resources.ApplyResources(this.btnDeleteFilter, "btnDeleteFilter");
			this.btnDeleteFilter.MinimumSize = new System.Drawing.Size(70, 26);
			this.btnDeleteFilter.Name = "btnDeleteFilter";
			this.btnDeleteFilter.UseVisualStyleBackColor = true;
			this.btnDeleteFilter.Click += new System.EventHandler(this.HandleButtonRemoveClick);
			// 
			// tableLayout
			// 
			resources.ApplyResources(this.tableLayout, "tableLayout");
			this.tableLayout.BackColor = System.Drawing.Color.Transparent;
			this.tableLayout.Controls.Add(this.pnlExpressions, 0, 1);
			this.tableLayout.Name = "tableLayout";
			// 
			// pnlExpressionMatch
			// 
			resources.ApplyResources(this.pnlExpressionMatch, "pnlExpressionMatch");
			this.pnlExpressionMatch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlExpressionMatch.ClipTextForChildControls = true;
			this.pnlExpressionMatch.ColorBottom = System.Drawing.Color.Empty;
			this.pnlExpressionMatch.ColorTop = System.Drawing.Color.Empty;
			this.pnlExpressionMatch.ControlReceivingFocusOnMnemonic = null;
			this.pnlExpressionMatch.Controls.Add(this.flowLayoutPanel);
			this.pnlExpressionMatch.DoubleBuffered = true;
			this.pnlExpressionMatch.DrawOnlyBottomBorder = true;
			this.pnlExpressionMatch.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlExpressionMatch.MakeDark = false;
			this.pnlExpressionMatch.MnemonicGeneratesClick = false;
			this.pnlExpressionMatch.Name = "pnlExpressionMatch";
			this.pnlExpressionMatch.PaintExplorerBarBackground = false;
			// 
			// flowLayoutPanel
			// 
			resources.ApplyResources(this.flowLayoutPanel, "flowLayoutPanel");
			this.flowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
			this.flowLayoutPanel.Controls.Add(this.lblExpressionMatchMsgPart1);
			this.flowLayoutPanel.Controls.Add(this.cboExpressionMatch);
			this.flowLayoutPanel.Controls.Add(this.lblExpressionMatchMsgPart2);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			// 
			// lblExpressionMatchMsgPart1
			// 
			resources.ApplyResources(this.lblExpressionMatchMsgPart1, "lblExpressionMatchMsgPart1");
			this.lblExpressionMatchMsgPart1.Name = "lblExpressionMatchMsgPart1";
			// 
			// cboExpressionMatch
			// 
			resources.ApplyResources(this.cboExpressionMatch, "cboExpressionMatch");
			this.cboExpressionMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboExpressionMatch.FormattingEnabled = true;
			this.cboExpressionMatch.Name = "cboExpressionMatch";
			this.cboExpressionMatch.SelectedIndexChanged += new System.EventHandler(this.HandleExpressionMatchComboIndexChanged);
			// 
			// lblExpressionMatchMsgPart2
			// 
			resources.ApplyResources(this.lblExpressionMatchMsgPart2, "lblExpressionMatchMsgPart2");
			this.lblExpressionMatchMsgPart2.Name = "lblExpressionMatchMsgPart2";
			// 
			// pnlExpressions
			// 
			this.pnlExpressions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlExpressions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlExpressions.ClipTextForChildControls = true;
			this.pnlExpressions.ControlReceivingFocusOnMnemonic = null;
			this.pnlExpressions.Controls.Add(this.btnApplyNow);
			this.pnlExpressions.Controls.Add(this.m_gridExpressions);
			this.pnlExpressions.Controls.Add(this.hlblExpressions);
			this.pnlExpressions.Controls.Add(this.pnlExpressionMatch);
			resources.ApplyResources(this.pnlExpressions, "pnlExpressions");
			this.pnlExpressions.DoubleBuffered = true;
			this.pnlExpressions.DrawOnlyBottomBorder = false;
			this.pnlExpressions.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlExpressions.MnemonicGeneratesClick = false;
			this.pnlExpressions.Name = "pnlExpressions";
			this.pnlExpressions.PaintExplorerBarBackground = false;
			// 
			// btnApplyNow
			// 
			resources.ApplyResources(this.btnApplyNow, "btnApplyNow");
			this.btnApplyNow.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnApplyNow.Name = "btnApplyNow";
			this.btnApplyNow.UseVisualStyleBackColor = true;
			this.btnApplyNow.Click += new System.EventHandler(this.HandleButtonApplyNowClick);
			// 
			// m_gridExpressions
			// 
			this.m_gridExpressions.AllowUserToAddRows = false;
			this.m_gridExpressions.AllowUserToDeleteRows = false;
			this.m_gridExpressions.AllowUserToOrderColumns = true;
			this.m_gridExpressions.AllowUserToResizeColumns = false;
			this.m_gridExpressions.AllowUserToResizeRows = false;
			this.m_gridExpressions.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_gridExpressions.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_gridExpressions.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gridExpressions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridExpressions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.m_gridExpressions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.m_gridExpressions, "m_gridExpressions");
			this.m_gridExpressions.DrawTextBoxEditControlBorder = false;
			this.m_gridExpressions.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_gridExpressions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridExpressions.IsDirty = false;
			this.m_gridExpressions.MultiSelect = false;
			this.m_gridExpressions.Name = "m_gridExpressions";
			this.m_gridExpressions.PaintHeaderAcrossFullGridWidth = true;
			this.m_gridExpressions.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridExpressions.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_gridExpressions.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_gridExpressions.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_gridExpressions.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_gridExpressions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridExpressions.ShowWaterMarkWhenDirty = false;
			this.m_gridExpressions.TextBoxEditControlBorderColor = System.Drawing.Color.DimGray;
			this.m_gridExpressions.WaterMark = "!";
			this.m_gridExpressions.CurrentRowChanged += new System.EventHandler(this.HandleExpressionsGridCurrentRowChanged);
			this.m_gridExpressions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleExpressionsGridCellContentClicked);
			this.m_gridExpressions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.HandleExpressionsGridCellFormatting);
			this.m_gridExpressions.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleExpressionsGridCellMouseEnter);
			this.m_gridExpressions.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleExpressionsGridCellMouseLeave);
			this.m_gridExpressions.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.HandleExpressionsGridCellPainting);
			this.m_gridExpressions.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.HandleExpressionsGridDefaultValuesNeeded);
			this.m_gridExpressions.Enter += new System.EventHandler(this.HandleExpressionsGridEnterAndLeave);
			this.m_gridExpressions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleExpressionsGridKeyDown);
			this.m_gridExpressions.Leave += new System.EventHandler(this.HandleExpressionsGridEnterAndLeave);
			// 
			// hlblExpressions
			// 
			this.hlblExpressions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.hlblExpressions.ClipTextForChildControls = false;
			this.hlblExpressions.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.hlblExpressions, "hlblExpressions");
			this.hlblExpressions.DoubleBuffered = true;
			this.hlblExpressions.DrawOnlyBottomBorder = false;
			this.hlblExpressions.ForeColor = System.Drawing.SystemColors.ControlText;
			this.hlblExpressions.MnemonicGeneratesClick = true;
			this.hlblExpressions.Name = "hlblExpressions";
			this.hlblExpressions.PaintExplorerBarBackground = false;
			this.hlblExpressions.ShowWindowBackgroudOnTopAndRightEdge = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// FiltersDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitFilters);
			this.Name = "FiltersDlg";
			this.Controls.SetChildIndex(this.splitFilters, 0);
			this.splitFilters.Panel1.ResumeLayout(false);
			this.splitFilters.Panel1.PerformLayout();
			this.splitFilters.Panel2.ResumeLayout(false);
			this.splitFilters.Panel2.PerformLayout();
			this.splitFilters.ResumeLayout(false);
			this.pnlFilters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_gridFilters)).EndInit();
			this.flwLayoutFilterButtons.ResumeLayout(false);
			this.flwLayoutFilterButtons.PerformLayout();
			this.tableLayout.ResumeLayout(false);
			this.pnlExpressionMatch.ResumeLayout(false);
			this.pnlExpressionMatch.PerformLayout();
			this.flowLayoutPanel.ResumeLayout(false);
			this.flowLayoutPanel.PerformLayout();
			this.pnlExpressions.ResumeLayout(false);
			this.pnlExpressions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridExpressions)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitFilters;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Button btnDeleteFilter;
		private System.Windows.Forms.Button btnAdd;
		private SilTools.SilGrid m_gridExpressions;
		private SilPanel pnlFilters;
		private System.Windows.Forms.Button btnApplyNow;
		private SilPanel pnlExpressions;
		private HeaderLabel hlblExpressions;
		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.FlowLayoutPanel flwLayoutFilterButtons;
		private System.Windows.Forms.ToolTip m_tooltip;
		private System.Windows.Forms.Label lblExpressionMatchMsgPart1;
		private System.Windows.Forms.ComboBox cboExpressionMatch;
		private System.Windows.Forms.Label lblExpressionMatchMsgPart2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private SilGradientPanel pnlExpressionMatch;
		private SilTools.SilGrid m_gridFilters;
	}
}