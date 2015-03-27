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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitFilters = new System.Windows.Forms.SplitContainer();
            this.pnlFilters = new SilTools.Controls.SilPanel();
            this.btnApplyNow = new System.Windows.Forms.Button();
            this.m_gridFilters = new SilTools.SilGrid();
            this.flwLayoutFilterButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDeleteFilter = new System.Windows.Forms.Button();
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlExpressions = new SilTools.Controls.SilPanel();
            this.m_gridExpressions = new SilTools.SilGrid();
            this.pnlExpressionMatch = new SilTools.Controls.SilGradientPanel();
            this.tableLayoutExpressionMatching = new System.Windows.Forms.TableLayoutPanel();
            this.lblExpressionMatchMsgPart = new System.Windows.Forms.Label();
            this.rbAllExpressions = new System.Windows.Forms.RadioButton();
            this.rbAnyExpression = new System.Windows.Forms.RadioButton();
            this.hlblExpressions = new SilTools.Controls.HeaderLabel();
            this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
            this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitFilters)).BeginInit();
            this.splitFilters.Panel1.SuspendLayout();
            this.splitFilters.Panel2.SuspendLayout();
            this.splitFilters.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridFilters)).BeginInit();
            this.flwLayoutFilterButtons.SuspendLayout();
            this.tableLayout.SuspendLayout();
            this.pnlExpressions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridExpressions)).BeginInit();
            this.pnlExpressionMatch.SuspendLayout();
            this.tableLayoutExpressionMatching.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
            // 
            // splitFilters
            // 
            this.splitFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFilters.Location = new System.Drawing.Point(10, 10);
            this.splitFilters.Name = "splitFilters";
            // 
            // splitFilters.Panel1
            // 
            this.splitFilters.Panel1.Controls.Add(this.pnlFilters);
            this.splitFilters.Panel1.Controls.Add(this.flwLayoutFilterButtons);
            this.splitFilters.Panel1MinSize = 150;
            // 
            // splitFilters.Panel2
            // 
            this.splitFilters.Panel2.Controls.Add(this.tableLayout);
            this.splitFilters.Size = new System.Drawing.Size(685, 316);
            this.splitFilters.SplitterDistance = 225;
            this.splitFilters.SplitterWidth = 6;
            this.splitFilters.TabIndex = 0;
            this.splitFilters.TabStop = false;
            // 
            // pnlFilters
            // 
            this.pnlFilters.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilters.ClipTextForChildControls = true;
            this.pnlFilters.ControlReceivingFocusOnMnemonic = null;
            this.pnlFilters.Controls.Add(this.btnApplyNow);
            this.pnlFilters.Controls.Add(this.m_gridFilters);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFilters.DoubleBuffered = true;
            this.pnlFilters.DrawOnlyBottomBorder = false;
            this.pnlFilters.DrawOnlyTopBorder = false;
            this.pnlFilters.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlFilters.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.pnlFilters, null);
            this.locExtender.SetLocalizationComment(this.pnlFilters, null);
            this.locExtender.SetLocalizationPriority(this.pnlFilters, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.pnlFilters, "FiltersDlg.pnlFilters");
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.MnemonicGeneratesClick = false;
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.PaintExplorerBarBackground = false;
            this.pnlFilters.Size = new System.Drawing.Size(225, 286);
            this.pnlFilters.TabIndex = 0;
            // 
            // btnApplyNow
            // 
            this.btnApplyNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyNow.AutoSize = true;
            this.btnApplyNow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApplyNow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.btnApplyNow, null);
            this.locExtender.SetLocalizationComment(this.btnApplyNow, null);
            this.locExtender.SetLocalizingId(this.btnApplyNow, "DialogBoxes.FiltersDlg.ApplyNowButton");
            this.btnApplyNow.Location = new System.Drawing.Point(53, 128);
            this.btnApplyNow.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.btnApplyNow.MinimumSize = new System.Drawing.Size(95, 26);
            this.btnApplyNow.Name = "btnApplyNow";
            this.btnApplyNow.Size = new System.Drawing.Size(95, 26);
            this.btnApplyNow.TabIndex = 1;
            this.btnApplyNow.Text = "Apply No&w";
            this.btnApplyNow.UseVisualStyleBackColor = true;
            this.btnApplyNow.Click += new System.EventHandler(this.HandleButtonApplyNowClick);
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
            this.m_gridFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_gridFilters.DrawTextBoxEditControlBorder = false;
            this.m_gridFilters.ExtendFullRowSelectRectangleToEdge = true;
            this.m_gridFilters.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.m_gridFilters.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
            this.m_gridFilters.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.m_gridFilters.IsDirty = true;
            this.locExtender.SetLocalizableToolTip(this.m_gridFilters, null);
            this.locExtender.SetLocalizationComment(this.m_gridFilters, null);
            this.locExtender.SetLocalizationPriority(this.m_gridFilters, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.m_gridFilters, "FiltersDlg.m_gridFilters");
            this.m_gridFilters.Location = new System.Drawing.Point(0, 0);
            this.m_gridFilters.MultiSelect = false;
            this.m_gridFilters.Name = "m_gridFilters";
            this.m_gridFilters.PaintHeaderAcrossFullGridWidth = true;
            this.m_gridFilters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.m_gridFilters.RowHeadersWidth = 22;
            this.m_gridFilters.SelectedCellBackColor = System.Drawing.Color.Empty;
            this.m_gridFilters.SelectedCellForeColor = System.Drawing.Color.Empty;
            this.m_gridFilters.SelectedRowBackColor = System.Drawing.Color.Empty;
            this.m_gridFilters.SelectedRowForeColor = System.Drawing.Color.Empty;
            this.m_gridFilters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_gridFilters.ShowWaterMarkWhenDirty = false;
            this.m_gridFilters.Size = new System.Drawing.Size(223, 284);
            this.m_gridFilters.StandardTab = true;
            this.m_gridFilters.TabIndex = 0;
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
            this.flwLayoutFilterButtons.AutoSize = true;
            this.flwLayoutFilterButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flwLayoutFilterButtons.Controls.Add(this.btnAdd);
            this.flwLayoutFilterButtons.Controls.Add(this.btnCopy);
            this.flwLayoutFilterButtons.Controls.Add(this.btnDeleteFilter);
            this.flwLayoutFilterButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flwLayoutFilterButtons.Location = new System.Drawing.Point(0, 286);
            this.flwLayoutFilterButtons.Name = "flwLayoutFilterButtons";
            this.flwLayoutFilterButtons.Size = new System.Drawing.Size(225, 30);
            this.flwLayoutFilterButtons.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.AutoSize = true;
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.btnAdd, null);
            this.locExtender.SetLocalizationComment(this.btnAdd, null);
            this.locExtender.SetLocalizingId(this.btnAdd, "DialogBoxes.FiltersDlg.AddButton");
            this.btnAdd.Location = new System.Drawing.Point(0, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 4, 5, 0);
            this.btnAdd.MinimumSize = new System.Drawing.Size(70, 26);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 26);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.HandleButtonAddClick);
            // 
            // btnCopy
            // 
            this.btnCopy.AutoSize = true;
            this.btnCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.btnCopy, null);
            this.locExtender.SetLocalizationComment(this.btnCopy, null);
            this.locExtender.SetLocalizingId(this.btnCopy, "DialogBoxes.FiltersDlg.CopyButton");
            this.btnCopy.Location = new System.Drawing.Point(75, 4);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(0, 4, 5, 0);
            this.btnCopy.MinimumSize = new System.Drawing.Size(70, 26);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(70, 26);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.HandleButtonCopyClick);
            // 
            // btnDeleteFilter
            // 
            this.btnDeleteFilter.AutoSize = true;
            this.btnDeleteFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDeleteFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.btnDeleteFilter, null);
            this.locExtender.SetLocalizationComment(this.btnDeleteFilter, null);
            this.locExtender.SetLocalizingId(this.btnDeleteFilter, "DialogBoxes.FiltersDlg.DeleteFilterButton");
            this.btnDeleteFilter.Location = new System.Drawing.Point(150, 4);
            this.btnDeleteFilter.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.btnDeleteFilter.MinimumSize = new System.Drawing.Size(70, 26);
            this.btnDeleteFilter.Name = "btnDeleteFilter";
            this.btnDeleteFilter.Size = new System.Drawing.Size(70, 26);
            this.btnDeleteFilter.TabIndex = 2;
            this.btnDeleteFilter.Text = "&Delete";
            this.btnDeleteFilter.UseVisualStyleBackColor = true;
            this.btnDeleteFilter.Click += new System.EventHandler(this.HandleButtonRemoveClick);
            // 
            // tableLayout
            // 
            this.tableLayout.AutoSize = true;
            this.tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayout.BackColor = System.Drawing.Color.Transparent;
            this.tableLayout.ColumnCount = 1;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.pnlExpressions, 0, 1);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 2;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Size = new System.Drawing.Size(454, 316);
            this.tableLayout.TabIndex = 0;
            // 
            // pnlExpressions
            // 
            this.pnlExpressions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.pnlExpressions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExpressions.ClipTextForChildControls = true;
            this.pnlExpressions.ControlReceivingFocusOnMnemonic = null;
            this.pnlExpressions.Controls.Add(this.m_gridExpressions);
            this.pnlExpressions.Controls.Add(this.pnlExpressionMatch);
            this.pnlExpressions.Controls.Add(this.hlblExpressions);
            this.pnlExpressions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExpressions.DoubleBuffered = true;
            this.pnlExpressions.DrawOnlyBottomBorder = false;
            this.pnlExpressions.DrawOnlyTopBorder = false;
            this.pnlExpressions.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.pnlExpressions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.pnlExpressions, null);
            this.locExtender.SetLocalizationComment(this.pnlExpressions, null);
            this.locExtender.SetLocalizationPriority(this.pnlExpressions, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.pnlExpressions, "FiltersDlg.pnlExpressions");
            this.pnlExpressions.Location = new System.Drawing.Point(0, 0);
            this.pnlExpressions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlExpressions.MnemonicGeneratesClick = false;
            this.pnlExpressions.Name = "pnlExpressions";
            this.pnlExpressions.PaintExplorerBarBackground = false;
            this.pnlExpressions.Size = new System.Drawing.Size(454, 316);
            this.pnlExpressions.TabIndex = 0;
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
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_gridExpressions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_gridExpressions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_gridExpressions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_gridExpressions.DrawTextBoxEditControlBorder = false;
            this.m_gridExpressions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.m_gridExpressions.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
            this.m_gridExpressions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
            this.m_gridExpressions.IsDirty = false;
            this.locExtender.SetLocalizableToolTip(this.m_gridExpressions, null);
            this.locExtender.SetLocalizationComment(this.m_gridExpressions, null);
            this.locExtender.SetLocalizationPriority(this.m_gridExpressions, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.m_gridExpressions, "FiltersDlg.m_gridExpressions");
            this.m_gridExpressions.Location = new System.Drawing.Point(0, 27);
            this.m_gridExpressions.MultiSelect = false;
            this.m_gridExpressions.Name = "m_gridExpressions";
            this.m_gridExpressions.PaintHeaderAcrossFullGridWidth = true;
            this.m_gridExpressions.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.m_gridExpressions.RowHeadersWidth = 22;
            this.m_gridExpressions.SelectedCellBackColor = System.Drawing.Color.Empty;
            this.m_gridExpressions.SelectedCellForeColor = System.Drawing.Color.Empty;
            this.m_gridExpressions.SelectedRowBackColor = System.Drawing.Color.Empty;
            this.m_gridExpressions.SelectedRowForeColor = System.Drawing.Color.Empty;
            this.m_gridExpressions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_gridExpressions.ShowWaterMarkWhenDirty = false;
            this.m_gridExpressions.Size = new System.Drawing.Size(452, 217);
            this.m_gridExpressions.TabIndex = 1;
            this.m_gridExpressions.TextBoxEditControlBorderColor = System.Drawing.Color.DimGray;
            this.m_gridExpressions.WaterMark = "!";
            this.m_gridExpressions.CurrentRowChanged += new System.EventHandler(this.HandleExpressionsGridCurrentRowChanged);
            this.m_gridExpressions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleExpressionsGridCellContentClicked);
            this.m_gridExpressions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.HandleExpressionsGridCellFormatting);
            this.m_gridExpressions.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleExpressionsGridCellMouseEnter);
            this.m_gridExpressions.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleExpressionsGridCellMouseLeave);
            this.m_gridExpressions.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.HandleExpressionsGridCellPainting);
            this.m_gridExpressions.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.m_gridExpressions_DataError);
            this.m_gridExpressions.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.HandleExpressionsGridDefaultValuesNeeded);
            this.m_gridExpressions.Enter += new System.EventHandler(this.HandleExpressionsGridEnterAndLeave);
            this.m_gridExpressions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleExpressionsGridKeyDown);
            this.m_gridExpressions.Leave += new System.EventHandler(this.HandleExpressionsGridEnterAndLeave);
            // 
            // pnlExpressionMatch
            // 
            this.pnlExpressionMatch.AutoSize = true;
            this.pnlExpressionMatch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlExpressionMatch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.pnlExpressionMatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExpressionMatch.ClipTextForChildControls = true;
            this.pnlExpressionMatch.ColorBottom = System.Drawing.Color.Empty;
            this.pnlExpressionMatch.ColorTop = System.Drawing.Color.Empty;
            this.pnlExpressionMatch.ControlReceivingFocusOnMnemonic = null;
            this.pnlExpressionMatch.Controls.Add(this.tableLayoutExpressionMatching);
            this.pnlExpressionMatch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlExpressionMatch.DoubleBuffered = true;
            this.pnlExpressionMatch.DrawOnlyBottomBorder = true;
            this.pnlExpressionMatch.DrawOnlyTopBorder = false;
            this.pnlExpressionMatch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.pnlExpressionMatch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.pnlExpressionMatch, null);
            this.locExtender.SetLocalizationComment(this.pnlExpressionMatch, null);
            this.locExtender.SetLocalizationPriority(this.pnlExpressionMatch, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this.pnlExpressionMatch, "FiltersDlg.pnlExpressionMatch");
            this.pnlExpressionMatch.Location = new System.Drawing.Point(0, 244);
            this.pnlExpressionMatch.MakeDark = false;
            this.pnlExpressionMatch.Margin = new System.Windows.Forms.Padding(0);
            this.pnlExpressionMatch.MnemonicGeneratesClick = false;
            this.pnlExpressionMatch.Name = "pnlExpressionMatch";
            this.pnlExpressionMatch.PaintExplorerBarBackground = false;
            this.pnlExpressionMatch.Size = new System.Drawing.Size(452, 70);
            this.pnlExpressionMatch.TabIndex = 2;
            // 
            // tableLayoutExpressionMatching
            // 
            this.tableLayoutExpressionMatching.AutoSize = true;
            this.tableLayoutExpressionMatching.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutExpressionMatching.ColumnCount = 1;
            this.tableLayoutExpressionMatching.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutExpressionMatching.Controls.Add(this.lblExpressionMatchMsgPart, 0, 0);
            this.tableLayoutExpressionMatching.Controls.Add(this.rbAllExpressions, 0, 2);
            this.tableLayoutExpressionMatching.Controls.Add(this.rbAnyExpression, 0, 1);
            this.tableLayoutExpressionMatching.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutExpressionMatching.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutExpressionMatching.Name = "tableLayoutExpressionMatching";
            this.tableLayoutExpressionMatching.RowCount = 3;
            this.tableLayoutExpressionMatching.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutExpressionMatching.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutExpressionMatching.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutExpressionMatching.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutExpressionMatching.Size = new System.Drawing.Size(288, 68);
            this.tableLayoutExpressionMatching.TabIndex = 0;
            // 
            // lblExpressionMatchMsgPart
            // 
            this.lblExpressionMatchMsgPart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblExpressionMatchMsgPart.AutoSize = true;
            this.lblExpressionMatchMsgPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblExpressionMatchMsgPart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.locExtender.SetLocalizableToolTip(this.lblExpressionMatchMsgPart, null);
            this.locExtender.SetLocalizationComment(this.lblExpressionMatchMsgPart, null);
            this.locExtender.SetLocalizingId(this.lblExpressionMatchMsgPart, "DialogBoxes.FiltersDlg.ExpressionMatchMsgPartLabel");
            this.lblExpressionMatchMsgPart.Location = new System.Drawing.Point(10, 5);
            this.lblExpressionMatchMsgPart.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblExpressionMatchMsgPart.Name = "lblExpressionMatchMsgPart";
            this.lblExpressionMatchMsgPart.Size = new System.Drawing.Size(268, 13);
            this.lblExpressionMatchMsgPart.TabIndex = 0;
            this.lblExpressionMatchMsgPart.Text = "For the \'{0}\' filter, what must be true to display a record?";
            // 
            // rbAllExpressions
            // 
            this.rbAllExpressions.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.rbAllExpressions, null);
            this.locExtender.SetLocalizationComment(this.rbAllExpressions, null);
            this.locExtender.SetLocalizingId(this.rbAllExpressions, "DialogBoxes.FiltersDlg.AllExpressionsRadioButton");
            this.rbAllExpressions.Location = new System.Drawing.Point(15, 44);
            this.rbAllExpressions.Margin = new System.Windows.Forms.Padding(15, 0, 10, 5);
            this.rbAllExpressions.Name = "rbAllExpressions";
            this.rbAllExpressions.Size = new System.Drawing.Size(207, 19);
            this.rbAllExpressions.TabIndex = 2;
            this.rbAllExpressions.TabStop = true;
            this.rbAllExpressions.Text = "A&ll expressions above must be true";
            this.rbAllExpressions.UseVisualStyleBackColor = true;
            this.rbAllExpressions.CheckedChanged += new System.EventHandler(this.HandleExpressionMatchTypeCheckedChanged);
            // 
            // rbAnyExpression
            // 
            this.rbAnyExpression.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this.rbAnyExpression, null);
            this.locExtender.SetLocalizationComment(this.rbAnyExpression, null);
            this.locExtender.SetLocalizingId(this.rbAnyExpression, "DialogBoxes.FiltersDlg.AnyExpressionRadioButton");
            this.rbAnyExpression.Location = new System.Drawing.Point(15, 23);
            this.rbAnyExpression.Margin = new System.Windows.Forms.Padding(15, 0, 10, 2);
            this.rbAnyExpression.Name = "rbAnyExpression";
            this.rbAnyExpression.Size = new System.Drawing.Size(250, 19);
            this.rbAnyExpression.TabIndex = 1;
            this.rbAnyExpression.TabStop = true;
            this.rbAnyExpression.Text = "At least &one expression above must be true";
            this.rbAnyExpression.UseVisualStyleBackColor = true;
            this.rbAnyExpression.CheckedChanged += new System.EventHandler(this.HandleExpressionMatchTypeCheckedChanged);
            // 
            // hlblExpressions
            // 
            this.hlblExpressions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.hlblExpressions.ClipTextForChildControls = false;
            this.hlblExpressions.ControlReceivingFocusOnMnemonic = null;
            this.hlblExpressions.Dock = System.Windows.Forms.DockStyle.Top;
            this.hlblExpressions.DoubleBuffered = true;
            this.hlblExpressions.DrawOnlyBottomBorder = false;
            this.hlblExpressions.DrawOnlyTopBorder = false;
            this.hlblExpressions.Font = new System.Drawing.Font("Arial", 9F);
            this.hlblExpressions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this.hlblExpressions, null);
            this.locExtender.SetLocalizationComment(this.hlblExpressions, null);
            this.locExtender.SetLocalizingId(this.hlblExpressions, "DialogBoxes.FiltersDlg.ExpressionsHeadingLabel");
            this.hlblExpressions.Location = new System.Drawing.Point(0, 0);
            this.hlblExpressions.MnemonicGeneratesClick = true;
            this.hlblExpressions.Name = "hlblExpressions";
            this.hlblExpressions.PaintExplorerBarBackground = false;
            this.hlblExpressions.ShowWindowBackgroudOnTopAndRightEdge = false;
            this.hlblExpressions.Size = new System.Drawing.Size(452, 27);
            this.hlblExpressions.TabIndex = 0;
            this.hlblExpressions.Text = "&Expressions";
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // FiltersDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 366);
            this.Controls.Add(this.splitFilters);
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizingId(this, "DialogBoxes.FiltersDlg.WindowTitle");
            this.MinimumSize = new System.Drawing.Size(715, 230);
            this.Name = "FiltersDlg";
            this.Text = "Filters";
            this.Controls.SetChildIndex(this.splitFilters, 0);
            this.splitFilters.Panel1.ResumeLayout(false);
            this.splitFilters.Panel1.PerformLayout();
            this.splitFilters.Panel2.ResumeLayout(false);
            this.splitFilters.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitFilters)).EndInit();
            this.splitFilters.ResumeLayout(false);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridFilters)).EndInit();
            this.flwLayoutFilterButtons.ResumeLayout(false);
            this.flwLayoutFilterButtons.PerformLayout();
            this.tableLayout.ResumeLayout(false);
            this.pnlExpressions.ResumeLayout(false);
            this.pnlExpressions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridExpressions)).EndInit();
            this.pnlExpressionMatch.ResumeLayout(false);
            this.pnlExpressionMatch.PerformLayout();
            this.tableLayoutExpressionMatching.ResumeLayout(false);
            this.tableLayoutExpressionMatching.PerformLayout();
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
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.FlowLayoutPanel flwLayoutFilterButtons;
		private System.Windows.Forms.ToolTip m_tooltip;
		private System.Windows.Forms.Label lblExpressionMatchMsgPart;
		private SilGradientPanel pnlExpressionMatch;
		private SilTools.SilGrid m_gridFilters;
		private System.Windows.Forms.TableLayoutPanel tableLayoutExpressionMatching;
		private System.Windows.Forms.RadioButton rbAllExpressions;
		private System.Windows.Forms.RadioButton rbAnyExpression;
	}
}