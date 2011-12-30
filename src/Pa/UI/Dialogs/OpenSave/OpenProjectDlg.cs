using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OpenProjectDlg : Form
	{
		private readonly OpenProjectDlgViewModel _viewModel;
		private readonly Font _italicFont;
		private readonly string _prjFoundFormatText;

		/// ------------------------------------------------------------------------------------
		public OpenProjectDlg()
		{
			InitializeComponent();

			DialogResult = DialogResult.None;

			_labelProjectFilesFound.Font = FontHelper.UIFont;
			_linkSelectAdditionalFolderToScan.Font = FontHelper.UIFont;
			_checkBoxOpenInNewWindow.Font = FontHelper.UIFont;
			_checkBoxShowFullProjectPaths.Font = FontHelper.UIFont;

			_grid.AutoResizeColumnHeadersHeight();
			_grid.ColumnHeadersHeight += 4;

			App.SetGridSelectionColors(_grid, false);

			_prjFoundFormatText = _labelProjectFilesFound.Text;

			_italicFont = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Italic);
		
			var linkText = App.GetString("DialogBoxes.OpenProjectDlg.linkSelectSpecificProject.LinkText",
				"select a specific project file");

			_linkSelectAdditionalFolderToScan.Links.Add(
				_linkSelectAdditionalFolderToScan.Text.IndexOf(linkText, StringComparison.Ordinal),
				linkText.Length);

			_checkBoxOpenInNewWindow.Checked = Settings.Default.OpenProjectsInNewWindowCheckedValue;
			_checkBoxShowFullProjectPaths.Checked = Settings.Default.ShowFullProjectFilePathsInOpenDlg;
		}

		/// ------------------------------------------------------------------------------------
		public OpenProjectDlg(OpenProjectDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			LoadGrid();

			_checkBoxShowFullProjectPaths.CheckedChanged += delegate
			{
				Settings.Default.ShowFullProjectFilePathsInOpenDlg = _checkBoxShowFullProjectPaths.Checked;
				_grid.InvalidateColumn(1);
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				_italicFont.Dispose();

				components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.OpenProjectDlg = App.InitializeForm(this, Settings.Default.OpenProjectDlg);
			base.OnLoad(e);
			BringToFront();

			if (Settings.Default.OpenProjectDlgGrid != null)
				Settings.Default.OpenProjectDlgGrid.InitializeGrid(_grid);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Settings.Default.OpenProjectDlgGrid = GridSettings.Create(_grid);
			Settings.Default.OpenProjectsInNewWindowCheckedValue = _checkBoxOpenInNewWindow.Checked;
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			_grid.Rows.Clear();
			_grid.RowCount = _viewModel.GetProjectFileCount();
			_labelProjectFilesFound.Text = string.Format(_prjFoundFormatText, _grid.RowCount);
			_buttonOpen.Enabled = (_grid.RowCount > 0);
			HandleGridCurrentRowChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_viewModel.LetUserSelectSpecificProjectFile(this))
				_buttonOpen.PerformClick();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCurrentRowChanged(object sender, EventArgs e)
		{
			_viewModel.SetCurrentBackupFile(_grid.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridPainting(object sender, PaintEventArgs e)
		{
			if (_grid.RowCount > 0)
				return;

			var msg = App.GetString("DialogBoxes.OpenProjectDlg.SelectProjectFilesPromptInEmptyList",
				"No project files were found.\nClick the link below to scan another folder\n" +
				"or close this dialog box and create a new project.");

			_grid.DrawMessageInCenterOfGrid(e.Graphics, msg, FontHelper.UIFont, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 0 && !_viewModel.GetDoesProjectHaveName(e.RowIndex))
				e.CellStyle.Font = _italicFont;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			switch (e.ColumnIndex)
			{
				case 0: e.Value = _viewModel.GetProjectNameForIndex(e.RowIndex); break;
				case 2: e.Value = _viewModel.GetProjectDataSourceTypesForIndex(e.RowIndex); break;
				case 1: e.Value = (Settings.Default.ShowFullProjectFilePathsInOpenDlg ?
					_viewModel.GetProjectFilePathForIndex(e.RowIndex) :
					_viewModel.GetProjectFileNameForIndex(e.RowIndex));
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
		{
			if (e.ColumnIndex == 1 && e.RowIndex >= 0)
				e.ToolTipText = _viewModel.GetProjectFilePathForIndex(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellDoubleClicked(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridKeyPress(object sender, KeyPressEventArgs e)
		{
			if (_grid.RowCount == 0)
				return;

			var index = _viewModel.FindNextProjectStartingWithLetter(_grid.CurrentCellAddress.Y, e.KeyChar);
			if (index > -1)
				_grid.CurrentCell = _grid[0, index];
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridKeyDown(object sender, KeyEventArgs e)
		{
			if (_grid.RowCount == 0)
				return;

			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex != 1 || !Settings.Default.ShowFullProjectFilePathsInOpenDlg)
				return;

			e.Handled = true;
			var rc = e.CellBounds;
			var parts = DataGridViewPaintParts.All;
			parts &= ~DataGridViewPaintParts.ContentForeground;
			e.Paint(rc, parts);

			var foreColor = ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected ?
				e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor);

			TextRenderer.DrawText(e.Graphics, e.Value as string, e.CellStyle.Font, rc,
				foreColor, TextFormatFlags.VerticalCenter | TextFormatFlags.PathEllipsis);
		}
	}
}
