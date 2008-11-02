namespace SIL.Pa.FiltersAddOn
{
	partial class DefineFiltersDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefineFiltersDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lvFilters = new System.Windows.Forms.ListView();
			this.hdrFilter = new System.Windows.Forms.ColumnHeader();
			this.splitFilters = new System.Windows.Forms.SplitContainer();
			this.pnlFilters = new SIL.Pa.Controls.PaPanel();
			this.hlblFilters = new SIL.Pa.Controls.HeaderLabel();
			this.pnlButtons2 = new System.Windows.Forms.Panel();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.m_grid = new SIL.SpeechTools.Utils.SilGrid();
			this.pnlFilterOptions = new System.Windows.Forms.Panel();
			this.chkShowHide = new System.Windows.Forms.CheckBox();
			this.lblAndOr = new System.Windows.Forms.Label();
			this.rbOr = new System.Windows.Forms.RadioButton();
			this.rbAnd = new System.Windows.Forms.RadioButton();
			this.m_tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.pnlButtons.SuspendLayout();
			this.splitFilters.Panel1.SuspendLayout();
			this.splitFilters.Panel2.SuspendLayout();
			this.splitFilters.SuspendLayout();
			this.pnlFilters.SuspendLayout();
			this.pnlButtons2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.pnlFilterOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
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
			this.splitFilters.Panel1.Controls.Add(this.pnlButtons2);
			// 
			// splitFilters.Panel2
			// 
			this.splitFilters.Panel2.Controls.Add(this.m_grid);
			this.splitFilters.Panel2.Controls.Add(this.pnlFilterOptions);
			this.splitFilters.TabStop = false;
			this.splitFilters.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitFilter_SplitterMoved);
			// 
			// pnlFilters
			// 
			this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFilters.ClipTextForChildControls = true;
			this.pnlFilters.ControlReceivingFocusOnMnemonic = null;
			this.pnlFilters.Controls.Add(this.lvFilters);
			this.pnlFilters.Controls.Add(this.hlblFilters);
			resources.ApplyResources(this.pnlFilters, "pnlFilters");
			this.pnlFilters.DoubleBuffered = true;
			this.pnlFilters.MnemonicGeneratesClick = false;
			this.pnlFilters.Name = "pnlFilters";
			this.pnlFilters.PaintExplorerBarBackground = false;
			// 
			// hlblFilters
			// 
			this.hlblFilters.ClipTextForChildControls = false;
			this.hlblFilters.ControlReceivingFocusOnMnemonic = this.lvFilters;
			resources.ApplyResources(this.hlblFilters, "hlblFilters");
			this.hlblFilters.MnemonicGeneratesClick = true;
			this.hlblFilters.Name = "hlblFilters";
			this.hlblFilters.ShowWindowBackgroudOnTopAndRightEdge = false;
			// 
			// pnlButtons2
			// 
			this.pnlButtons2.Controls.Add(this.btnCopy);
			this.pnlButtons2.Controls.Add(this.btnRemove);
			this.pnlButtons2.Controls.Add(this.btnAdd);
			resources.ApplyResources(this.pnlButtons2, "pnlButtons2");
			this.pnlButtons2.Name = "pnlButtons2";
			// 
			// btnCopy
			// 
			resources.ApplyResources(this.btnCopy, "btnCopy");
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnRemove
			// 
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.WaterMark = "!";
			this.m_grid.ColumnHeadersHeightChanged += new System.EventHandler(this.m_grid_ColumnHeadersHeightChanged);
			this.m_grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.m_grid_CellFormatting);
			this.m_grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.m_grid_DefaultValuesNeeded);
			this.m_grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_grid_KeyDown);
			// 
			// pnlFilterOptions
			// 
			this.pnlFilterOptions.Controls.Add(this.chkShowHide);
			this.pnlFilterOptions.Controls.Add(this.lblAndOr);
			this.pnlFilterOptions.Controls.Add(this.rbOr);
			this.pnlFilterOptions.Controls.Add(this.rbAnd);
			resources.ApplyResources(this.pnlFilterOptions, "pnlFilterOptions");
			this.pnlFilterOptions.Name = "pnlFilterOptions";
			// 
			// chkShowHide
			// 
			resources.ApplyResources(this.chkShowHide, "chkShowHide");
			this.chkShowHide.BackColor = System.Drawing.Color.Transparent;
			this.chkShowHide.Name = "chkShowHide";
			this.chkShowHide.UseVisualStyleBackColor = false;
			// 
			// lblAndOr
			// 
			resources.ApplyResources(this.lblAndOr, "lblAndOr");
			this.lblAndOr.BackColor = System.Drawing.Color.Transparent;
			this.lblAndOr.Name = "lblAndOr";
			// 
			// rbOr
			// 
			resources.ApplyResources(this.rbOr, "rbOr");
			this.rbOr.BackColor = System.Drawing.Color.Transparent;
			this.rbOr.Name = "rbOr";
			this.rbOr.TabStop = true;
			this.rbOr.UseVisualStyleBackColor = false;
			// 
			// rbAnd
			// 
			resources.ApplyResources(this.rbAnd, "rbAnd");
			this.rbAnd.BackColor = System.Drawing.Color.Transparent;
			this.rbAnd.Name = "rbAnd";
			this.rbAnd.TabStop = true;
			this.rbAnd.UseVisualStyleBackColor = false;
			// 
			// DefineFiltersDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitFilters);
			this.Name = "DefineFiltersDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.splitFilters, 0);
			this.pnlButtons.ResumeLayout(false);
			this.splitFilters.Panel1.ResumeLayout(false);
			this.splitFilters.Panel2.ResumeLayout(false);
			this.splitFilters.ResumeLayout(false);
			this.pnlFilters.ResumeLayout(false);
			this.pnlButtons2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.pnlFilterOptions.ResumeLayout(false);
			this.pnlFilterOptions.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvFilters;
		private System.Windows.Forms.SplitContainer splitFilters;
		private System.Windows.Forms.Panel pnlButtons2;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ColumnHeader hdrFilter;
		private SIL.SpeechTools.Utils.SilGrid m_grid;
		private SIL.Pa.Controls.PaPanel pnlFilters;
		private SIL.Pa.Controls.HeaderLabel hlblFilters;
		private System.Windows.Forms.Panel pnlFilterOptions;
		private System.Windows.Forms.Label lblAndOr;
		private System.Windows.Forms.RadioButton rbOr;
		private System.Windows.Forms.RadioButton rbAnd;
		private System.Windows.Forms.CheckBox chkShowHide;
		private System.Windows.Forms.ToolTip m_tooltip;
	}
}