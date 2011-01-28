using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a control class combining a heading panel and a grid for entering
	/// transcription changes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TranscriptionChangesControl : SilPanel, IxCoreColleague
	{
		private const int kFirstCnvrtToCol = 2;
		private const int kCnvrtCol = 1;

		private SilGrid m_grid;
		private readonly HeaderLabel m_header;
		private readonly Label lblTargetHdg;
		private readonly Label lblSourceHdg;

		#region Constructing the control, grid and loading the grid
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TranscriptionChangesControl()
		{
			// Create the label over the column containing transcriptions to convert.
			lblSourceHdg = new Label();
			lblSourceHdg.AutoSize = false;
			lblSourceHdg.Left = 3;
			lblSourceHdg.Font = FontHelper.UIFont;
			lblSourceHdg.BackColor = Color.Transparent;
			lblSourceHdg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

			// Create the label that's over the possible experimental
			// transcriptions to convert to.
			lblTargetHdg = new Label();
			lblTargetHdg.AutoSize = false;
			lblTargetHdg.Font = FontHelper.UIFont;
			lblTargetHdg.BackColor = Color.Transparent;
			lblTargetHdg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

			// Create the panel that owns the two labels created above.
			m_header = new HeaderLabel();
			m_header.Dock = DockStyle.Top;
			m_header.ShowWindowBackgroudOnTopAndRightEdge = false;
			m_header.Paint += m_header_Paint;
			m_header.Controls.Add(lblSourceHdg);
			m_header.Controls.Add(lblTargetHdg);
			Controls.Add(m_header);

			// Calculate the height of the labels and their owning panel.
			// The height should accomodate two lines worth of text.
			lblSourceHdg.Font = FontHelper.UIFont;
			lblSourceHdg.Text = "X\nX";
			lblSourceHdg.Height = lblTargetHdg.Height = lblSourceHdg.PreferredHeight;
			m_header.Height = lblSourceHdg.Height + 6;
			lblSourceHdg.Top = lblTargetHdg.Top = (m_header.Height - lblSourceHdg.Height) / 2;

			// Set the heading text.
			App.LocalizeObject(lblSourceHdg, "TranscriptionChangesDlg.Heading1",
				"Transcribed in source as:", "Heading in transcription changes control.",
				App.kLocalizationGroupDialogs);

			App.LocalizeObject(lblTargetHdg, "TranscriptionChangesDlg.Heading2",
				"Replace with one of these options:", 
				"Heading in transcription changes control.", App.kLocalizationGroupDialogs);

			BuildGrid();
			LoadGrid();

			App.MsgMediator.AddColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			App.MsgMediator.RemoveColleague(this);
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the grid control embedded in the experimental transcription panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilGrid Grid
		{
			get { return m_grid; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct the grid that holds all the experimental transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid = new SilGrid();
			m_grid.BorderStyle = BorderStyle.None;
			m_grid.Name = "TranscriptionChangesGrid";
			m_grid.AllowUserToOrderColumns = false;
			m_grid.AllowUserToAddRows = true;
			m_grid.AllowUserToDeleteRows = true;
			m_grid.ColumnHeadersVisible = false;
			m_grid.ColumnHeadersHeight = 12;
			m_grid.ColumnHeadersHeightSizeMode =
				DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			m_grid.RowHeadersVisible = false;
			m_grid.RowsRemoved += m_grid_RowsRemoved;
			m_grid.CellEndEdit += m_grid_CellEndEdit;
			m_grid.CellBeginEdit += m_grid_CellBeginEdit;
			m_grid.CurrentCellDirtyStateChanged += m_grid_CurrentCellDirtyStateChanged;
			m_grid.CellPainting += m_grid_CellPainting;
			m_grid.ColumnWidthChanged += m_grid_ColumnWidthChanged;
			App.SetGridSelectionColors(m_grid, true);

			// The sequence-to-convert column.
			DataGridViewColumn col = new RadioButtonColumn("col0", false, true);
			col.Resizable = DataGridViewTriState.True;
			m_grid.Columns.Add(col);

			// The "None" (don't convert) column.
			col = new RadioButtonColumn(true);
			col.Width = 90;
			col.Resizable = DataGridViewTriState.True;
			m_grid.Columns.Add(col);

			m_grid.Dock = DockStyle.Fill;
			Controls.Add(m_grid);
			m_grid.BringToFront();

			RefreshHeader();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the grid with the experimental transcriptions from the loaded project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			TranscriptionChanges expList = App.IPASymbolCache.TranscriptionChanges;

			if (expList == null || expList.Count == 0)
			{
				m_grid.Columns.Add(new RadioButtonColumn());
				m_grid.IsDirty = false;
				return;
			}

			m_grid.Rows.Add(expList.Count);

			int i = 0;
			foreach (TranscriptionChange info in expList)
			{
				m_grid[0, i].Value = info.WhatToReplace;
				bool noneChecked = false;

				// Load the cell indicating whether or not a
				// replacement will take place for this item.
				RadioButtonCell cell = m_grid[kCnvrtCol, i] as RadioButtonCell;
				if (cell != null)
				{
					cell.Checked = (info.ReplaceWith == null);
					noneChecked = cell.Checked;
				}

				if (info.ReplacementOptions.Count > 0)
				{
					// Now add the possible translation changes list to
					// which the ambiguous item may be converted.
					int col = kFirstCnvrtToCol;
					foreach (string option in info.ReplacementOptions.Select(x => x.Literal))
					{
						// If there aren't enough columns to accomodate the
						// next convert to item, then add a new one.
						if (col == m_grid.Columns.Count)
							m_grid.Columns.Add(new RadioButtonColumn("col" + col));

						m_grid[col, i].Value = option;
						cell = m_grid[col, i] as RadioButtonCell;
						if (cell != null)
						{
							cell.Checked = (!noneChecked &&
								option == info.ReplaceWith);
						}

						col++;
					}
				}

				i++;
			}

			m_grid.Columns.Add(new RadioButtonColumn("col" + m_grid.Columns.Count));
			m_grid.EndEdit();

			if (Settings.Default.TranscriptionChangesGrid != null)
				Settings.Default.TranscriptionChangesGrid.InitializeGrid(m_grid);

			m_grid.Invalidate();

			// Make sure the grid is not considered to be dirty after loading it.
			m_grid.IsDirty = false;
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) gets changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			m_grid.Columns["col0"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_grid.Columns["col0"].DefaultCellStyle.Font = FontHelper.PhoneticFont;

			foreach (DataGridViewRow row in m_grid.Rows)
				row.Cells["col0"].Style.Font = FontHelper.PhoneticFont;

			for (int i = kFirstCnvrtToCol; i < m_grid.ColumnCount; i++)
			{
				m_grid.Columns[i].CellTemplate.Style.Font = FontHelper.PhoneticFont;
				m_grid.Columns[i].DefaultCellStyle.Font = FontHelper.PhoneticFont;

				foreach (DataGridViewRow row in m_grid.Rows)
					row.Cells[i].Style.Font = FontHelper.PhoneticFont;
			}

			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			RefreshHeader();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the experimental transcription grid's headings when it changes sizes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshHeader()
		{
			Rectangle rc = m_grid.GetColumnDisplayRectangle(kCnvrtCol, true);
			Point pt = m_grid.PointToScreen(rc.Location);
			pt = m_header.PointToClient(pt);

			// Adjust the widths of the experimental transcription headings.
			lblSourceHdg.Width = rc.Left - 5;
			lblTargetHdg.Left = pt.X + 3;
			lblTargetHdg.Width = m_header.Width - lblTargetHdg.Left - 4;

			// Repaint the line dividing the two headings.
			m_header.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value of the "Convert?" column in the specified row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetRowsConvertValue(int rowIndex)
		{
			RadioButtonCell cell = m_grid[kCnvrtCol, rowIndex] as RadioButtonCell;
			return (cell == null ? false : !cell.Checked);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the convert to value for the specified row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetRowsConvertToValue(int rowIndex)
		{
			if (rowIndex != m_grid.NewRowIndex && GetRowsConvertValue(rowIndex))
			{
				for (int i = kFirstCnvrtToCol; i < m_grid.Columns.Count; i++)
				{
					RadioButtonCell cell = m_grid[i, rowIndex] as RadioButtonCell;
					if (cell != null && cell.Checked)
						return (cell.Value as string);
				}
			}

			return null;
		}

		#endregion

		#region Grid event methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void m_grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			// When the steps performed in "RowWasRemoved" were done in this method, there
			// was a certain case that would cause a crash because because the grid wasn't
			// finished processing the RowsRemoved event. Therefore, I have moved the steps
			// I need to perform into a post message handler so the grid can completely
			// finish its handling of removed rows before I do my part.
			App.MsgMediator.PostMessage("RowWasRemoved", e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure there are no more columns than are necessary. The message handler is
		/// called after the user deletes a row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRowWasRemoved(object args)
		{
			if (args.GetType() != typeof(int))
				return false;

			int rowIndex = (int)args;

			if (rowIndex > 0 && rowIndex == m_grid.NewRowIndex)
				rowIndex--;

			while (AreLastTwoColumnsEmpty)
				OnRemoveLastColumn(rowIndex);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the cv pattern when the value of the convert check box changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			// Save the edit to the current cell and update the CV pattern.
			DataGridViewCell cell = m_grid.CurrentCell;
			if (cell != null && m_grid.NewRowIndex != cell.RowIndex && cell.ColumnIndex == kCnvrtCol)
				m_grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (e.RowIndex == m_grid.NewRowIndex)
			{
				if (e.ColumnIndex != 0)
					e.Cancel = true;

				// When a new row is beginning to be edited, then check the
				// "None" column. But don't consider it dirty yet.
				RadioButtonCell cell = m_grid[kCnvrtCol, e.RowIndex] as RadioButtonCell;
				if (cell != null)
				{
					bool wasDirty = m_grid.IsDirty;
					cell.Checked = true;

					if (!wasDirty)
						m_grid.IsDirty = false;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == m_grid.NewRowIndex)
				return;

			m_grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			RemoveEmptyCellGaps(e.RowIndex);

			// If, after the cell is edited, its row is now empty, the row can be removed.
			// If, after the cell is edited, all the cells in its row contain data, then a
			// new blank column needs to be added. If, after the cell is edited, the right
			// two columns contain no data, then remove the last one since we only need one
			// empty column (which is for columns as the new row is for rows).
			if (string.IsNullOrEmpty(m_grid.Rows[e.RowIndex].Cells[0].Value as string))
			{
				App.MsgMediator.PostMessage("RemoveRow", e.RowIndex);
				return;
			}
			
			if (IsRowFull(m_grid.Rows[e.RowIndex]))
				m_grid.Columns.Add(new RadioButtonColumn());
			else if (AreLastTwoColumnsEmpty && App.MsgMediator != null)
				App.MsgMediator.PostMessage("RemoveLastColumn", e.RowIndex);

			if (e.ColumnIndex > kCnvrtCol)
			{
				// If the cell just edited is one of the convert to experimentaTransList
				// then check it, on the assumption the user wants the latest of their
				// added/edited experimentaTransList.
				RadioButtonCell cell = m_grid[e.ColumnIndex, e.RowIndex] as RadioButtonCell;
				if (cell != null && !string.IsNullOrEmpty(cell.Value as string))
					cell.Checked = true;

				bool convert = GetRowsConvertValue(e.RowIndex);

				// Check the convert option when there's something in the row and the user
				// user added/edited one of the "convert to" experimentaTransList. If the edit resulted in
				// the row being emptied, then check the None item.
				if (IsRowEmpty(m_grid.Rows[e.RowIndex]))
					((RadioButtonCell)m_grid[kCnvrtCol, e.RowIndex]).Checked = true;
				else if (!convert && cell != null && !string.IsNullOrEmpty(cell.Value as string))
					((RadioButtonCell)m_grid[kCnvrtCol, e.RowIndex]).Checked = false;
			}

			// Normalize ambiguous experimentaTransList and "convert to" experimentaTransList.
			if (e.ColumnIndex > kCnvrtCol || e.ColumnIndex == 0)
			{
				DataGridViewCell cell = m_grid[e.ColumnIndex, e.RowIndex];
				if (!string.IsNullOrEmpty(cell.Value as string))
					cell.Value = FFNormalizer.Normalize((cell.Value as string).Trim());

				// Check if there's no "convert to" experimentaTransList. If not,
				// then make sure the None column is checked.
				if (GetRowsConvertToValue(e.RowIndex) == null)
					((RadioButtonCell)m_grid[kCnvrtCol, e.RowIndex]).Checked = true;

				// Force a repaint of all the cells in the edited column in case
				// the width of the CV pattern area of the cell changed.
				m_grid.InvalidateColumn(e.ColumnIndex);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveRow(object args)
		{
			if (args.GetType() == typeof(int))
			{
				m_grid.Rows.RemoveAt((int)args);
				RemoveEmptyCellGaps((int)args);

				if (AreLastTwoColumnsEmpty)
					OnRemoveLastColumn((int)args);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveLastColumn(object args)
		{
			m_grid.Columns.RemoveAt(m_grid.Columns.Count - 1);

			if (args.GetType() != typeof(int))
				return false;

			int row = (int)args;

			if (row >= 0 && row < m_grid.NewRowIndex)
			{
				// Now make sure at least one convert to item is checked
				RadioButtonCell cell = m_grid[kFirstCnvrtToCol, row] as RadioButtonCell;
				if (GetRowsConvertToValue(row) == null && cell != null &&
					!string.IsNullOrEmpty(cell.Value as string))
				{
					cell.Checked = true;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the "item to convert" column changes width, then redraw the panel above it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			foreach (DataGridViewColumn col in m_grid.Columns)
				col.Name = "col" + col.DisplayIndex;

			Settings.Default.TranscriptionChangesGrid = GridSettings.Create(m_grid);

			if (e.Column.Index == 0)
				RefreshHeader();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Custom draw column headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void m_grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex >= 0)
				return;

			Rectangle rc = e.CellBounds;

			// This is somewhat kludgy. I encountered a problem drawing a line across the
			// bottom of each column heading cell. In most cells it worked, but the line would
			// not get drawn in the furthest right cell nor, in some cases, in the middle of
			// other cells. It was very strange and I think it's a bug in the underlying grid
			// painting. Since there isn't a problem with rectangle filling, I first fill the
			// full cell with the line color, then decrease the height of the rectangle by
			// one pixel and fill with the cell's desired background color. Argh!
			using (SolidBrush br = new SolidBrush(SystemColors.ControlDark))
			{
				e.Graphics.FillRectangle(br, rc);
				rc.Height--;
				br.Color = SystemColors.Control;
				e.Graphics.FillRectangle(br, rc);
			}

			// Draw a vertical line at the right edge of the heading cell.
			using (Pen pen = new Pen(e.ColumnIndex == 0 ?
				SystemColors.ControlText : Color.FromArgb(50, SystemColors.ControlDark)))
			{
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Top, rc.Right - 1, rc.Bottom);
			}

			e.Handled = true;
		}

		#endregion

		#region Painting the heading panel.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		// Draw a vertical line in the heading which, visually, is an extension of
		// the line between the "item to convert" column and the column to its right.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_header_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = m_grid.GetColumnDisplayRectangle(kCnvrtCol, true);
			Point pt = m_grid.PointToScreen(rc.Location);
			pt = m_header.PointToClient(pt);
			e.Graphics.DrawLine(SystemPens.ControlText,
				pt.X - 1, 0, pt.X - 1, m_grid.Top - 1);
		}

		#endregion

		#region Methods for managing/determining rows and columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves all empty cell's in the specified row to the end of the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RemoveEmptyCellGaps(int row)
		{
			int cellCount = m_grid.Rows[row].Cells.Count;

			for (int i = kFirstCnvrtToCol; i < cellCount; i++)
			{
				// If the current cell isn't empty then move on to the next cell.
				if (!string.IsNullOrEmpty(m_grid[i, row].Value as string))
					continue;

				// At this point we know the current cell is empty.
				// So find the next following cell that's not empty.
				int nextCell = i + 1;
				while (nextCell < cellCount &&
					string.IsNullOrEmpty(m_grid[nextCell, row].Value as string))
				{
					nextCell++;
				}

				// If a following cell was found that's not empty,
				// then move its contents to this cell.
				if (nextCell < cellCount)
				{
					m_grid[i, row].Value = m_grid[nextCell, row].Value;
					m_grid[nextCell, row].Value = null;
					m_grid.InvalidateColumn(i);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the relevant cell's in the specified
		/// row are empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsRowEmpty(DataGridViewRow row)
		{
			foreach (DataGridViewCell cell in row.Cells)
			{
				if ((cell.ColumnIndex == 0 || cell.ColumnIndex > kCnvrtCol) &&
					!string.IsNullOrEmpty(cell.Value as string))
				{
					return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the cell's in this cell's row are
		/// filled with query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsRowFull(DataGridViewRow row)
		{
			DataGridViewCell lastCell = row.Cells[row.Cells.Count - 1];
			return !string.IsNullOrEmpty(lastCell.Value as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the last two columns in the grid are empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AreLastTwoColumnsEmpty
		{
			get
			{
				if (m_grid.Columns.Count == kFirstCnvrtToCol + 1)
					return false;

				int col = m_grid.Columns.Count - 2;
				if (col < 0)
					return false;

				foreach (DataGridViewRow row in m_grid.Rows)
				{
					if (row.Index >= 0 && row.Index != m_grid.NewRowIndex)
					{
						if (!string.IsNullOrEmpty(row.Cells[col].Value as string))
							return false;
					}
				}

				col = m_grid.Columns.Count - 1;
				foreach (DataGridViewRow row in m_grid.Rows)
				{
					if (row.Index >= 0 && row.Index != m_grid.NewRowIndex)
					{
						if (!string.IsNullOrEmpty(row.Cells[col].Value as string))
							return false;
					}
				}

				return true;
			}
		}

		#endregion

		#region Method for saving changes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveChanges()
		{
			// Commit pending changes in the grid.
			m_grid.EndEdit();

			TranscriptionChanges transChanges = new TranscriptionChanges();
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row.Index == m_grid.NewRowIndex)
					continue;

				TranscriptionChange change = new TranscriptionChange();
				change.WhatToReplace = (row.Cells[0].Value as string);

				List<string> replacementOptions = new List<string>();
				for (int i = kFirstCnvrtToCol; i < m_grid.Columns.Count; i++)
				{
					RadioButtonCell cell = row.Cells[i] as RadioButtonCell;
					if (cell != null && !string.IsNullOrEmpty(cell.Value as string))
					{
						replacementOptions.Add(cell.Value as string);
						if (cell.Checked)
							change.ReplaceWith = cell.Value as string;
					}
				}

				if (replacementOptions.Count > 0)
					change.SetReplacementOptions(replacementOptions);

				transChanges.Add(change);
			}

			App.MsgMediator.SendMessage("BeforeTranscriptionChangesSaved", transChanges);
			transChanges.Save(App.Project.ProjectPathFilePrefix);
			App.IPASymbolCache.TranscriptionChanges = transChanges;
			m_grid.IsDirty = false;
			App.MsgMediator.SendMessage("AfterTranscriptionChangesSaved", transChanges);
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// Not used in PA.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}

	#region RadioButtonColumn class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class RadioButtonColumn : DataGridViewColumn
	{
		private bool m_includeCVPattern = true;
		private bool m_showRadioButton = true;
		private readonly bool m_forNoConvertColumn;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RadioButtonColumn() : base(new RadioButtonCell())
		{
			base.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			Width = 110;
			HeaderText = string.Empty;
			base.Resizable = DataGridViewTriState.True;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RadioButtonColumn(string name) : this()
		{
			Name = name;
		}

		/// ------------------------------------------------------------------------------------
		public RadioButtonColumn(string name, bool showRadioButton, bool includeCVPattern)
			: this()
		{
			Name = name;
			m_includeCVPattern = includeCVPattern;
			m_showRadioButton = showRadioButton;
		}

		/// ------------------------------------------------------------------------------------
		public RadioButtonColumn(bool forNoConvertColumn) : this()
		{
			Name = "None";
			m_includeCVPattern = false;
			m_showRadioButton = true;
			m_forNoConvertColumn = true;

			base.DefaultCellStyle.Font = FontHelper.UIFont;
			base.CellTemplate.Style.Font = FontHelper.UIFont;
			base.ReadOnly = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ForNoConvertColumn
		{
			get { return m_forNoConvertColumn; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show the radio button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowRadioButton
		{
			get { return m_showRadioButton; }
			set { m_showRadioButton = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cells in this column should
		/// include a CV pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IncludeCVPattern
		{
			get { return m_includeCVPattern; }
			set { m_includeCVPattern = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the template is always a radion button cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				if (value != null && !value.GetType().IsAssignableFrom(typeof(RadioButtonCell)))
					throw new InvalidCastException("Must be a RadioButtonCell");

				base.CellTemplate = value;
			}
		}
	}

	#endregion

	#region RadioButtonCell class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class RadioButtonCell : DataGridViewTextBoxCell
	{
		private const int m_rbDimension = 14;
		private bool m_checked;
		private bool m_mouseOverRadioButton;
		private bool m_enabled = true;
		private readonly Font m_fntCV = FontHelper.UIFont;
		private readonly string m_noneText;

		private const TextFormatFlags kCellFmtFlags =
			TextFormatFlags.LeftAndRightPadding | TextFormatFlags.VerticalCenter |
			TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

		/// ------------------------------------------------------------------------------------
		public RadioButtonCell()
		{
			if (App.Project.FieldInfo.CVPatternField != null)
				m_fntCV = App.Project.FieldInfo.CVPatternField.Font;

			m_noneText = App.LocalizeString(
				"ExperimentalTranscriptionsDlg.DontConvertText", "None",
				"Text in the experimental " + "transcription list of experimental transcription dialog box.",
				App.kLocalizationGroupDialogs);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a cell is checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Checked
		{
			get { return m_checked; }
			set
			{
				if (m_checked == value || !CanBeChecked)
					return;

				m_checked = value;

				// When setting this cell's checked value to true, then go through all the
				// other cells in the same row and uncheck them. Then invalidate the row so
				// all the cells in the row are repainted with their proper checked state.
				if (value)
				{
					foreach (DataGridViewCell cell in DataGridView.Rows[RowIndex].Cells)
					{
						RadioButtonCell rbCell = cell as RadioButtonCell;
						if (rbCell != null && rbCell != this)
							rbCell.m_checked = false;
					}

					DataGridView.InvalidateRow(RowIndex);
				}

				if (DataGridView != null && DataGridView is SilGrid)
					((SilGrid)DataGridView).IsDirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Repaint the cell when it's enabled property changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Enabled
		{
			get { return m_enabled; }
			set
			{
				m_enabled = value;
				DataGridView.InvalidateCell(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the cell can be checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CanBeChecked
		{
			get
			{
				RadioButtonColumn owningCol = OwningColumn as RadioButtonColumn;
				bool forNoConvertCol = (owningCol != null && owningCol.ForNoConvertColumn);
				bool hasRadioButton = (owningCol != null && owningCol.ShowRadioButton);

				return (hasRadioButton && (forNoConvertCol ||
					(RowIndex < DataGridView.NewRowIndex &&
					!string.IsNullOrEmpty(Value as string))));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified point is over the cell's
		/// radio button. The relativeToCell flag is true when the specified point's origin
		/// is relative to the upper right corner of the cell. When false, it's assumed the
		/// point's origin is relative to the cell's owning grid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPointOverRadioButton(Point pt, bool relativeToCell)
		{
			// Get the rectangle for the radion button area.
			Rectangle rc = DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, false);
			Rectangle rcrb;
			Rectangle rcText;
			Rectangle rcCVPattern;
			GetRectangles(null, rc, out rcrb, out rcText, out rcCVPattern);

			if (relativeToCell)
			{
				// Set the radio button's rectangle location
				// relative to the cell instead of the grid.
				rcrb.X -= rc.X;
				rcrb.Y -= rc.Y;
			}

			return rcrb.Contains(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor space-bar keypresses since they serve to check a cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
		{
			if (e.KeyCode == Keys.Space && !m_checked && CanBeChecked && m_enabled)
				Checked = true;
			else
				base.OnKeyDown(e, rowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Normally, space puts the user in the edit mode. We want it to check cells.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override bool KeyEntersEditMode(KeyEventArgs e)
		{
			return (e.KeyCode == Keys.Space ? false : base.KeyEntersEditMode(e));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (!CanBeChecked || !m_enabled)
				return;

			if (!IsPointOverRadioButton(e.Location, true) && m_mouseOverRadioButton)
			{
				m_mouseOverRadioButton = false;
				DataGridView.InvalidateCell(this);
			}
			else if (IsPointOverRadioButton(e.Location, true) && !m_mouseOverRadioButton)
			{
				m_mouseOverRadioButton = true;
				DataGridView.InvalidateCell(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			// Just perform normal processing if checking the cell isn't allowed.
			if (!CanBeChecked || !m_enabled)
			{
				base.OnMouseClick(e);
				return;
			}

			// If the click is over the radio button and the
			// cell isn't already checked, then check it.
			if (!IsPointOverRadioButton(e.Location, true))
				base.OnMouseClick(e);
			else if (!m_checked)
				Checked = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the cell with a radio button and it's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Paint(Graphics g, Rectangle clipBounds,
			Rectangle bounds, int rowIndex, DataGridViewElementStates state,
			object value, object formattedValue, string errorText, DataGridViewCellStyle style,
			DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts parts)
		{
			// Determine whether or not this cell belongs to the "None" column.
			RadioButtonColumn owningCol = OwningColumn as RadioButtonColumn;
			bool forNoConvertCol = (owningCol != null && owningCol.ForNoConvertColumn);
			if (forNoConvertCol)
			{
				formattedValue = m_noneText;
				m_enabled = (rowIndex != DataGridView.NewRowIndex);
			}

			// Allow the default paint behavior if the conditions are correct.
			if (!forNoConvertCol &&
				(rowIndex == DataGridView.NewRowIndex || string.IsNullOrEmpty(value as string)))
			{
				base.Paint(g, clipBounds, bounds, rowIndex, state, value,
					formattedValue, errorText, style, advancedBorderStyle, parts);

				DrawCVPatternSeparatorLines(g, bounds, Rectangle.Empty);
				return;
			}

			// Draw default everything but text
			parts &= ~DataGridViewPaintParts.ContentForeground;
			base.Paint(g, clipBounds, bounds, rowIndex, state, value,
				formattedValue, errorText, style, advancedBorderStyle, parts);

			// Get the rectangles for the two parts of the cell.
			Rectangle rcrb;
			Rectangle rcText;
			Rectangle rcCVPattern;
			GetRectangles(g, bounds, out rcrb, out rcText, out rcCVPattern);
			DrawRadioButton(g, rcrb);

			Color clrText = (m_enabled ? style.ForeColor : SystemColors.GrayText);

			// Draw the CV pattern
			if (rcCVPattern != Rectangle.Empty)
			{
				TextRenderer.DrawText(g, CVPattern, m_fntCV, rcCVPattern,
					clrText, kCellFmtFlags | TextFormatFlags.EndEllipsis);
			}

			// Draw text
			TextRenderer.DrawText(g, formattedValue as string, style.Font,
				rcText, clrText, kCellFmtFlags | TextFormatFlags.EndEllipsis);

			DrawCVPatternSeparatorLines(g, bounds, rcCVPattern);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws a line between the phonetic and its CV pattern and a dark line to the right
		/// of the CV pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawCVPatternSeparatorLines(Graphics g, Rectangle bounds,
			Rectangle rcCVPattern)
		{
			using (Pen pen = new Pen(SystemColors.WindowText))
			{
				// Draw a dark line to the right of the CV pattern.
				g.DrawLine(pen, bounds.Right - 1, bounds.Top,
				   bounds.Right - 1, bounds.Bottom - 1);

				if (rcCVPattern != Rectangle.Empty && ShowCVPattern)
				{
					// Draw line separating the phonetic from its CV pattern.
					pen.Color = DataGridView.GridColor;
					g.DrawLine(pen, rcCVPattern.X, rcCVPattern.Top, rcCVPattern.X,
						rcCVPattern.Bottom - 1);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the radio button in the cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawRadioButton(Graphics g, Rectangle rcrb)
		{
			if (!ShowRadioButton)
				return;

			VisualStyleElement element = GetVisualStyleRadioButton();

			if (PaintingHelper.CanPaintVisualStyle(element))
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(g, rcrb);
			}
			else
			{
				ButtonState buttonState = ButtonState.Flat |
					(m_checked ? ButtonState.Checked : ButtonState.Normal);

				if (m_enabled)
					buttonState |= ButtonState.Inactive;

				ControlPaint.DrawRadioButton(g, rcrb, buttonState);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the correct visual style radio button given the state of the cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private VisualStyleElement GetVisualStyleRadioButton()
		{
			VisualStyleElement element;

			if (!m_enabled)
			{
				element = (m_checked ?
					VisualStyleElement.Button.RadioButton.CheckedDisabled :
					VisualStyleElement.Button.RadioButton.UncheckedDisabled);
			}
			else
			{
				if (m_checked)
				{
					element = (m_mouseOverRadioButton ?
						VisualStyleElement.Button.RadioButton.CheckedHot :
						VisualStyleElement.Button.RadioButton.CheckedNormal);
				}
				else
				{
					element = (m_mouseOverRadioButton ?
						VisualStyleElement.Button.RadioButton.UncheckedHot :
						VisualStyleElement.Button.RadioButton.UncheckedNormal);
				}
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle for the radio button and the text, given the specified cell
		/// bounds.
		/// </summary>
		/// <param name="g">The graphics object</param>
		/// <param name="bounds">The rectangle of the entire cell.</param>
		/// <param name="rcrb">The returned rectangle for the radio button.</param>
		/// <param name="rcText">The returned rectangle for the text.</param>
		/// <param name="rcCVPattern">The rc CV pattern.</param>
		/// ------------------------------------------------------------------------------------
		private void GetRectangles(IDeviceContext g, Rectangle bounds, out Rectangle rcrb,
			out Rectangle rcText, out Rectangle rcCVPattern)
		{
			rcCVPattern = GetCVPatternRectangle(g, bounds);

			// The gap between the left edge of the cell and the text will be the
			// diameter of the radio button plus 7 pixels, plus whatever left padding the
			// text renderer includes when drawing the text.
			int rbAreaWidth = (ShowRadioButton ? m_rbDimension + 7 : 0);
			rcText = bounds;
			rcText.X += rbAreaWidth;
			rcText.Width -= (rbAreaWidth + rcCVPattern.Width);

			// The gap between the left edge of the cell and the radio button is 7 pixels.
			rcrb = new Rectangle(bounds.Left + 7, 0, m_rbDimension, m_rbDimension);
			rcrb.Y = bounds.Top + (int)Math.Ceiling((bounds.Height - m_rbDimension) / 2f);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle in which this cell's CV pattern is to be drawn.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="bounds">The bounding rectangle for the entire cell.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private Rectangle GetCVPatternRectangle(IDeviceContext g, Rectangle bounds)
		{
			if (!ShowCVPattern || DataGridView == null ||
				DataGridView.Rows.Count == 0 || g == null)
			{
				return Rectangle.Empty;
			}

			int maxWidth = 0;

			foreach (DataGridViewRow row in DataGridView.Rows)
			{
				RadioButtonCell cell = row.Cells[ColumnIndex] as RadioButtonCell;
				if (cell != null)
				{
					Size sz = TextRenderer.MeasureText(g, cell.CVPattern,
						m_fntCV, Size.Empty, kCellFmtFlags);

					maxWidth = Math.Max(maxWidth, sz.Width);
				}
			}

			// Just don't show the CV pattern if its with is zero or its width doesn't
			// leave room for at least 20 pixels of the left portion of the cell.
			if (maxWidth == 0 || maxWidth > bounds.Width - 20)
				return Rectangle.Empty;

			return new Rectangle(bounds.Right - maxWidth - 1, bounds.Y, maxWidth, bounds.Height);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not this cell is owned by a RadioButtonColumn
		/// and whose ShowRadioButton property is true;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ShowRadioButton
		{
			get
			{
				RadioButtonColumn owingCol = OwningColumn as RadioButtonColumn;
				return (owingCol != null && owingCol.ShowRadioButton);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not this cell is owned by a RadioButtonColumn
		/// and whose IncludeCVPattern property is true;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ShowCVPattern
		{
			get
			{
				RadioButtonColumn owingCol = OwningColumn as RadioButtonColumn;
				return (owingCol != null && owingCol.IncludeCVPattern);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the cell's phonetic value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CVPattern
		{
			get
			{
				// Make sure that when getting the CV pattern for the item to convert, we
				// first don't convert it using it's own experimental transcription.
				//RadioButtonColumn owningCol = OwningColumn as RadioButtonColumn;
				//bool isCellForItemToConvert = (owningCol != null && !owningCol.ShowRadioButton);
				
				return (!ShowCVPattern || string.IsNullOrEmpty(Value as string) ? string.Empty :
					App.PhoneCache.GetCVPattern(Value as string, false));
			}
		}
	}

	#endregion
}
