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
			this._grid = new SilTools.SilGrid();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this._grid);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = false;
			this.pnlGrid.DrawOnlyBottomBorder = false;
			this.pnlGrid.DrawOnlyTopBorder = false;
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
			// _grid
			// 
			this._grid.AllowUserToAddRows = false;
			this._grid.AllowUserToDeleteRows = false;
			this._grid.AllowUserToOrderColumns = true;
			this._grid.AllowUserToResizeRows = false;
			this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._grid.DrawTextBoxEditControlBorder = false;
			this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this._grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this._grid, null);
			this.locExtender.SetLocalizationComment(this._grid, null);
			this.locExtender.SetLocalizationPriority(this._grid, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._grid, "AmbiguousSequencesDlg.m_grid");
			this._grid.Location = new System.Drawing.Point(0, 0);
			this._grid.MultiSelect = false;
			this._grid.Name = "_grid";
			this._grid.PaintHeaderAcrossFullGridWidth = true;
			this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.RowHeadersWidth = 22;
			this._grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._grid.ShowWaterMarkWhenDirty = false;
			this._grid.Size = new System.Drawing.Size(441, 272);
			this._grid.TabIndex = 2;
			this._grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._grid.WaterMark = "!";
			this._grid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.HandleGridCellBeginEdit);
			this._grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleGridCellContentClick);
			this._grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleGridCellEndEdit);
			this._grid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.HandleGridCellValidating);
			this._grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.HandleGridDefaultValuesNeeded);
			this._grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.HandleGridRowsAdded);
			this._grid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.HandleGridUserDeletingRow);
			// 
			// locExtender
			// 
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
			this.locExtender.SetLocalizingId(this, "DialogBoxes.AmbiguousSequencesDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(455, 225);
			this.Name = "AmbiguousSequencesDlg";
			this.Text = "Ambiguous Sequences";
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilPanel pnlGrid;
		private SilTools.SilGrid _grid;
		private Localization.UI.LocalizationExtender locExtender;
	}
}