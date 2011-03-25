using SilTools.Controls;

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
			this.pnlGrid = new SilTools.Controls.SilPanel();
			this.m_grid = new SilTools.SilGrid();
			this.chkShowGenerated = new System.Windows.Forms.CheckBox();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this.m_grid);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = false;
			this.pnlGrid.DrawOnlyBottomBorder = false;
			this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizationPriority(this.pnlGrid, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlGrid, "AmbiguousSequencesDlg.pnlGrid");
			this.pnlGrid.Location = new System.Drawing.Point(10, 10);
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.pnlGrid.Size = new System.Drawing.Size(443, 274);
			this.pnlGrid.TabIndex = 102;
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
			this.m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grid.DrawTextBoxEditControlBorder = false;
			this.m_grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_grid, null);
			this.locExtender.SetLocalizationComment(this.m_grid, null);
			this.locExtender.SetLocalizationPriority(this.m_grid, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_grid, "AmbiguousSequencesDlg.m_grid");
			this.m_grid.Location = new System.Drawing.Point(0, 0);
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.PaintHeaderAcrossFullGridWidth = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersWidth = 22;
			this.m_grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.Size = new System.Drawing.Size(441, 272);
			this.m_grid.TabIndex = 2;
			this.m_grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_grid.WaterMark = "!";
			this.m_grid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.m_grid_CellBeginEdit);
			this.m_grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellEndEdit);
			this.m_grid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.m_grid_CellValidating);
			this.m_grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.m_grid_DefaultValuesNeeded);
			this.m_grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.m_grid_RowsAdded);
			this.m_grid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.m_grid_UserDeletingRow);
			// 
			// chkShowGenerated
			// 
			this.chkShowGenerated.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkShowGenerated.AutoSize = true;
			this.chkShowGenerated.BackColor = System.Drawing.Color.Transparent;
			this.chkShowGenerated.Checked = true;
			this.chkShowGenerated.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowGenerated.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkShowGenerated, null);
			this.locExtender.SetLocalizationComment(this.chkShowGenerated, null);
			this.locExtender.SetLocalizingId(this.chkShowGenerated, "AmbiguousSequencesDlg.chkShowGenerated");
			this.chkShowGenerated.Location = new System.Drawing.Point(1, 12);
			this.chkShowGenerated.Name = "chkShowGenerated";
			this.chkShowGenerated.Size = new System.Drawing.Size(163, 17);
			this.chkShowGenerated.TabIndex = 4;
			this.chkShowGenerated.Text = "Show &Generated Sequences";
			this.chkShowGenerated.UseVisualStyleBackColor = false;
			this.chkShowGenerated.Visible = false;
			this.chkShowGenerated.CheckedChanged += new System.EventHandler(this.HandleShowGeneratedCheckedChanged);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// AmbiguousSequencesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(463, 324);
			this.Controls.Add(this.pnlGrid);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "AmbiguousSequencesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(455, 225);
			this.Name = "AmbiguousSequencesDlg";
			this.Text = "Ambiguous Sequences";
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilPanel pnlGrid;
		private SilTools.SilGrid m_grid;
		private System.Windows.Forms.CheckBox chkShowGenerated;
		private Localization.UI.LocalizationExtender locExtender;
	}
}