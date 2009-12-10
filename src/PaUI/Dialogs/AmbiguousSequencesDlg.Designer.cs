using SilUtils.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class AmbiguousSequencesDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmbiguousSequencesDlg));
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.m_grid = new SilUtils.SilGrid();
			this.chkShowDefaults = new System.Windows.Forms.CheckBox();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.chkShowDefaults);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.chkShowDefaults, 0);
			// 
			// btnCancel
			// 
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in Base Class");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in Base Class");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in Base Class");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this.m_grid);
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizationPriority(this.pnlGrid, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlGrid, "AmbiguousSequencesDlg.pnlGrid");
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			// 
			// m_grid
			// 
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToOrderColumns = true;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
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
			this.locExtender.SetLocalizationPriority(this.m_grid, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_grid, "AmbiguousSequencesDlg.m_grid");
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.WaterMark = "!";
			this.m_grid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.m_grid_UserDeletingRow);
			this.m_grid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.m_grid_CellBeginEdit);
			this.m_grid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.m_grid_CellValidating);
			this.m_grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.m_grid_RowsAdded);
			this.m_grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellEndEdit);
			this.m_grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.m_grid_DefaultValuesNeeded);
			// 
			// chkShowDefaults
			// 
			resources.ApplyResources(this.chkShowDefaults, "chkShowDefaults");
			this.chkShowDefaults.BackColor = System.Drawing.Color.Transparent;
			this.chkShowDefaults.Checked = true;
			this.chkShowDefaults.CheckState = System.Windows.Forms.CheckState.Checked;
			this.locExtender.SetLocalizableToolTip(this.chkShowDefaults, null);
			this.locExtender.SetLocalizationComment(this.chkShowDefaults, null);
			this.locExtender.SetLocalizationPriority(this.chkShowDefaults, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.chkShowDefaults, "AmbiguousSequencesDlg.chkShowDefaults");
			this.chkShowDefaults.Name = "chkShowDefaults";
			this.chkShowDefaults.UseVisualStyleBackColor = false;
			this.chkShowDefaults.CheckedChanged += new System.EventHandler(this.chkShowDefaults_CheckedChanged);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// AmbiguousSequencesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlGrid);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "AmbiguousSequencesDlg.WindowTitle");
			this.Name = "AmbiguousSequencesDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilPanel pnlGrid;
		private SilUtils.SilGrid m_grid;
		private System.Windows.Forms.CheckBox chkShowDefaults;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}