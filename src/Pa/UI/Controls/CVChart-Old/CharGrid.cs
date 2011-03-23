using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a character grid (for vowel and consonants) with special headers (for
	/// columns and rows) that can span multiple columns in the DataGridView.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CharGrid : UserControl, IxCoreColleague
	{
		private const string kDropTargetCell = "dtc";

		internal static Color kGridColor =
				ColorHelper.CalculateColor(SystemColors.Window, SystemColors.GrayText, 70);

		public event ItemDragEventHandler ItemDrag;

		private const int kMinHdrSize = 22;

		private readonly int m_cellHeight; // = 60; //34;
		private CharGridHeader m_currentHeader;
		private Point m_mouseDownGridLocation = Point.Empty;
		private DataGridViewCell m_cellDraggedOver;
		private string m_phoneBeingDragged;
		private ITMAdapter m_tmAdapter;
		private CharGridHeader m_currentRowHeader;
		private CharGridHeader m_currentColHeader;
		private CellKBMovingCellHelper m_phoneMovingHelper;
		private CharGridHeaderCollectionPanel m_pnlColHeaders;
		private PhoneInfoPopup m_phoneInfoPopup;

		/// ------------------------------------------------------------------------------------
		public CharGrid()
		{
			CellWidth = 38;
			SearchWhenPhoneDoubleClicked = true;
			SupraSegsToIgnore = PhoneCache.kDefaultChartSupraSegsToIgnore;
			m_cellHeight = Settings.Default.CVChartsCellHeight;
			RowHeaders = new List<CharGridHeader>();
			ColumnHeaders = new List<CharGridHeader>();
			ChartFont = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 14);
			m_pnlColHeaders = new CharGridHeaderCollectionPanel(true);
			RowHeadersCollectionPanel = new CharGridHeaderCollectionPanel(false);

			InitializeComponent();

			if (App.DesignMode)
				return;

			SuspendLayout();
			Grid.OwningPanel = pnlGrid;
			Grid.Font = ChartFont;
			Grid.GridColor = kGridColor;
			pnlColHeaderOuter.Controls.Add(m_pnlColHeaders);
			pnlRowHeaderOuter.Controls.Add(RowHeadersCollectionPanel);
			m_phoneInfoPopup = new PhoneInfoPopup(Grid);
			ResumeLayout(true);

			AdjustRowHeadingLocation();
			AdjustColumnHeadingLocation();

			Disposed += CharGrid_Disposed;
		}

		/// ------------------------------------------------------------------------------------
		void CharGrid_Disposed(object sender, EventArgs e)
		{
			Disposed -= CharGrid_Disposed;

			RowHeaders = null;
			ColumnHeaders = null;

			if (ChartFont != null)
				ChartFont.Dispose();

			if (m_pnlColHeaders != null && !m_pnlColHeaders.IsDisposed)
			{
				m_pnlColHeaders.Dispose();
				m_pnlColHeaders = null;
			}

			if (RowHeadersCollectionPanel != null && !RowHeadersCollectionPanel.IsDisposed)
			{
				RowHeadersCollectionPanel.Dispose();
				RowHeadersCollectionPanel = null;
			}

			if (m_phoneInfoPopup != null && !m_phoneInfoPopup.IsDisposed)
			{
				m_phoneInfoPopup.Dispose();
				m_phoneInfoPopup = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear's the grid and all the headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			m_currentColHeader = null;
			m_currentRowHeader = null;
			Grid.Rows.Clear();
			Grid.Columns.Clear();
			RowHeadersCollectionPanel.Controls.Clear();
			RowHeaders.Clear();
			m_pnlColHeaders.Controls.Clear();
			ColumnHeaders.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calls the grid's CurrentCellChanged event even if the current cell hasn't changed.
		/// This is useful to force a selection painting of the current cell's row and column
		/// heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ForceCurrentCellUpdate()
		{
			m_grid_CurrentCellChanged(null, null);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font used in the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Font ChartFont { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the column and row headers are
		/// visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool HeadersVisible
		{
			get { return pnlColHeaderOuter.Visible; }
			set
			{
				pnlColHeaderOuter.Visible = value;
				pnlRowHeaderOuter.Visible = value;
				m_vsplitter.Visible = value;
				m_hsplitter.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the width of each cell in the grid. This should be set before the
		/// chart is loaded with phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CellWidth { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a default search is performed for
		/// a phone when the cell it's in is double-clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SearchWhenPhoneDoubleClicked { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the toolbar/menu adapter for the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
			set
			{
				App.RemoveMediatorColleague(this);

				m_tmAdapter = value;

				if (m_tmAdapter != null)
				{
					m_tmAdapter.SetContextMenuForControl(Grid, "cmnuCharChartGrid");
					if (Grid.ContextMenuStrip != null)
					{
						Grid.ContextMenuStrip.Opening += ((sender, args) => m_phoneInfoPopup.Enabled = false);
						Grid.ContextMenuStrip.Closed += ((sender, args) => m_phoneInfoPopup.Enabled = true);
					}
				}

				App.AddMediatorColleague(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the owning view type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Type OwningViewType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the chart's grid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CharGridView Grid { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current phone in the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CurrentPhone
		{
			get
			{
				CharGridCell cell = (Grid.CurrentCell == null ?
					null : Grid.CurrentCell.Value as CharGridCell);

				return (cell == null || string.IsNullOrEmpty(cell.Phone) ? null : cell.Phone);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of selected phones phone in the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] SelectedPhones
		{
			get
			{
				var phones = new List<string>();

				if (Grid.SelectedCells.Count == 0)
				{
					string currPhone = CurrentPhone;
					if (!string.IsNullOrEmpty(currPhone))
						phones.Add(CurrentPhone);
				}
				else
				{
					foreach (DataGridViewCell dgvCell in Grid.SelectedCells)
					{
						var cell = dgvCell.Value as CharGridCell;
						if (cell != null && !string.IsNullOrEmpty(cell.Phone))
							phones.Add(cell.Phone);
					}
				}

				return (phones.Count == 0 ? null : phones.ToArray());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the panel that owns the collection of column header controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CharGridHeaderCollectionPanel ColumnHeadersCollectionPanel
		{
			get { return m_pnlColHeaders; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the panel that owns the collection of row header controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CharGridHeaderCollectionPanel RowHeadersCollectionPanel { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of row headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<CharGridHeader> RowHeaders { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of column headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<CharGridHeader> ColumnHeaders { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the row header width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int RowHeaderWidth
		{
			get { return pnlRowHeaderOuter.Width; }
			set
			{
				pnlRowHeaderOuter.Width = (value < kMinHdrSize ? kMinHdrSize : value);
				RowHeadersCollectionPanel.Width = pnlRowHeaderOuter.Width;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the column header height.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ColumnHeaderHeight
		{
			get { return pnlColHeaderOuter.Height; }
			set
			{
				pnlColHeaderOuter.Height = (value < kMinHdrSize ? kMinHdrSize : value);
				m_pnlColHeaders.Height = pnlColHeaderOuter.Height;
			}
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show uncertain phones in the chart.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ShowUncertainPhones { get; set; }

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of suprasegmentals to ignore.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SupraSegsToIgnore { get; set; }

		#endregion

		#region Save/Restore settings
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || App.DesignMode)
				return;

			RowHeadersCollectionPanel.Width = pnlRowHeaderOuter.Width;
			m_pnlColHeaders.Height = pnlColHeaderOuter.Height;
			Adjust();

			m_grid_CurrentCellChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the header sizes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			App.RemoveMediatorColleague(this);
			base.OnHandleDestroyed(e);
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the group the specified row is contained in.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetRowsGroup(int rowIndex)
		{
			var hdr = GetRowsHeader(rowIndex);
			return (hdr == null ? -1 : hdr.Group);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets header that owns the row specified by the supplied row index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader GetRowsHeader(int rowIndex)
		{
			if (rowIndex >= 0 && rowIndex < Grid.Rows.Count)
			{
				foreach (var hdr in RowHeaders)
				{
					if (hdr.OwnedRows.Any(row => row.Index == rowIndex))
						return hdr;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets header that owns the column specified by the supplied column index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader GetColumnsHeader(int colIndex)
		{
			if (colIndex >= 0 && colIndex < Grid.Columns.Count)
			{
				foreach (CharGridHeader hdr in ColumnHeaders)
				{
					foreach (DataGridViewColumn col in hdr.OwnedColumns)
					{
						if (col.Index == colIndex)
							return hdr;
					}
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a single row from the CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveRow(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex >= Grid.Rows.Count)
				return;

			CharGridHeader hdr = Grid.Rows[rowIndex].Tag as CharGridHeader;
			if (hdr == null)
				return;

			// If there's only one row left then remove the entire heading too.
			if (hdr.OwnedRows.Count == 1)
				RemoveRowHeader(hdr);
			else
			{
				hdr.RemoveRow(Grid.Rows[rowIndex]);
				Grid.Rows.RemoveAt(rowIndex);
				Grid.Refresh();
				CalcHeights();
				AdjustRowHeadingLocation();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a single column from the CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveColumn(int colIndex)
		{
			if (colIndex < 0 || colIndex >= Grid.Columns.Count)
				return;

			CharGridHeader hdr = Grid.Columns[colIndex].Tag as CharGridHeader;
			if (hdr == null)
				return;

			// If there's only one column left then remove the entire heading too.
			if (hdr.OwnedColumns.Count == 1)
				RemoveColumnHeader(hdr);
			else
			{
				hdr.RemoveColumn(Grid.Columns[colIndex]);
				Grid.Columns.RemoveAt(colIndex);
				Grid.Refresh();
				CalcWidths();
				AdjustColumnHeadingLocation();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the specified row header from the CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveRowHeader(CharGridHeader hdr)
		{
			if (hdr != null)
			{
				m_currentRowHeader = null;
				hdr.RemoveOwnedRows();
				RowHeaders.Remove(hdr);
				RowHeadersCollectionPanel.Controls.Remove(hdr);
				Grid.Refresh();
				CalcHeights();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the specified column header from the CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveColumnHeader(CharGridHeader hdr)
		{
			if (hdr != null)
			{
				m_currentColHeader = null;
				hdr.RemoveOwnedColumns();
				ColumnHeaders.Remove(hdr);
				m_pnlColHeaders.Controls.Remove(hdr);
				Grid.Refresh();
				CalcWidths();
			}
		}

		#endregion

		#region Adding/Creating Rows
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row without a label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader AddRowHeader()
		{
			return AddRowHeader(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row with a label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader AddRowHeader(string text)
		{
			return AddRowHeader(text, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row with a label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader AddRowHeader(string text, string subheadtext)
		{
			if (m_currentHeader != null)
				m_currentHeader.EndEditLabel();

			CharGridHeader hdr = CreateRowHeader(text, -1);
			AddRowToHeading(hdr, subheadtext);
			return hdr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row to the grid under the specified heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewRow AddRowToHeading(CharGridHeader hdr)
		{
			return AddRowToHeading(hdr, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row to the grid under the specified heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewRow AddRowToHeading(CharGridHeader hdr, string subheadtext)
		{
			if (hdr == null)
				return null;

			int insertRowIndex = (hdr.LastRow == null ? Grid.Rows.Count : hdr.LastRow.Index + 1);
			Grid.Rows.Insert(insertRowIndex, new DataGridViewRow());
			DataGridViewRow newRow = Grid.Rows[insertRowIndex];
			newRow.Height = m_cellHeight;
			hdr.AddRow(newRow, subheadtext);
			CalcHeights();
			return newRow;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new row header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridHeader CreateRowHeader(string text, int insertIndex)
		{
			RowHeadersCollectionPanel.SuspendLayout();

			CharGridHeader newHdr = new CharGridHeader(text, false);
			newHdr.Height = m_cellHeight;

			if (insertIndex == -1)
			{
				RowHeaders.Add(newHdr);
				RowHeadersCollectionPanel.Controls.Add(newHdr);
				newHdr.BringToFront();
			}
			else
			{
				RowHeaders.Insert(insertIndex, newHdr);
				RowHeadersCollectionPanel.Controls.Clear();
				foreach (CharGridHeader hdr in RowHeaders)
				{
					RowHeadersCollectionPanel.Controls.Add(hdr);
					hdr.BringToFront();
				}
			}

			CalcHeights();
			RowHeadersCollectionPanel.ResumeLayout();
			return newHdr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate what the height should be of the panel that owns all the row headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CalcHeights()
		{
			int height = 0;
			foreach (Control ctrl in RowHeadersCollectionPanel.Controls)
				height += ctrl.Height;

			RowHeadersCollectionPanel.Height = height;
			Grid.Height = height + 1;
		}

		#endregion

		#region Adding/Creating Columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column header without a label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader AddColumnHeader()
		{
			return AddColumnHeader(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column header without a label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader AddColumnHeader(string text)
		{
			return AddColumnHeader(text, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column header label and a single grid column it owns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader AddColumnHeader(string text, string subheadtext)
		{
			if (m_currentHeader != null)
				m_currentHeader.EndEditLabel();

			CharGridHeader hdr = CreateColumnHeader(text, -1);
			AddColumnToHeading(hdr, subheadtext);
			return hdr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column to the grid under the specified heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewColumn AddColumnToHeading(CharGridHeader hdr)
		{
			return AddColumnToHeading(hdr, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column to the grid under the specified heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewColumn AddColumnToHeading(CharGridHeader hdr, string subheadtext)
		{
			if (hdr == null)
				return null;

			int insertColIndex = (hdr.LastColumn == null ?
				Grid.Columns.Count : hdr.LastColumn.Index + 1);

			DataGridViewColumn newCol = CreateColumn();
			hdr.AddColumn(newCol, subheadtext);
			CalcWidths();
			Grid.Columns.Insert(insertColIndex, newCol);
			return Grid.Columns[insertColIndex];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridHeader CreateColumnHeader(string text, int insertIndex)
		{
			m_pnlColHeaders.SuspendLayout();
			CharGridHeader newHdr = new CharGridHeader(text, true);
			newHdr.Width = CellWidth;

			if (insertIndex == -1)
			{
				ColumnHeaders.Add(newHdr);
				m_pnlColHeaders.Controls.Add(newHdr);
				newHdr.BringToFront();
			}
			else
			{
				ColumnHeaders.Insert(insertIndex, newHdr);
				m_pnlColHeaders.Controls.Clear();
				foreach (CharGridHeader hdr in ColumnHeaders)
				{
					m_pnlColHeaders.Controls.Add(hdr);
					hdr.BringToFront();
				}
			}

			CalcWidths();
			m_pnlColHeaders.ResumeLayout();
			return newHdr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a new grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewColumn CreateColumn()
		{
			string colName = string.Format("col{0}", Grid.Columns.Count);
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(colName);
			col.CellTemplate.Style.Font = ChartFont;
			col.Width = CellWidth;
			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate what the width should be of the panel that owns all the column headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CalcWidths()
		{
			int width = 0;
			foreach (Control ctrl in m_pnlColHeaders.Controls)
				width += ctrl.Width;

			m_pnlColHeaders.Width = width;
			Grid.Width = width + 1;

			if (Grid.Rows.Count > 0)
				Grid.Height = (Grid.Rows.Count * m_cellHeight) + 1;
		}

		#endregion

		#region Methods for adjusting the size and location of the heading panels
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the grid to the proper location relative to the splitters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Adjust()
		{
			if (pnlCorner.Width != pnlRowHeaderOuter.Width)
			{
				pnlCorner.Width = pnlRowHeaderOuter.Width + m_vsplitter.Width;
				pnlCorner.Invalidate();
			}

			RowHeadersCollectionPanel.Width = pnlRowHeaderOuter.Width;
			m_pnlColHeaders.Height = pnlColHeaderOuter.Height;

			Point ptv = pnlWrapper.PointToScreen(new Point(m_vsplitter.SplitPosition, 0));
			ptv = pnlGrid.PointToClient(ptv);

			Point pth = pnlWrapper.PointToScreen(new Point(0, m_hsplitter.SplitPosition));
			pth = pnlGrid.PointToClient(pth);

			Grid.Location = new Point(ptv.X + m_vsplitter.Width - 1,
				pth.Y + m_hsplitter.Height - 1);

			AdjustColumnHeadingLocation();
			AdjustRowHeadingLocation();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustColumnHeadingLocation()
		{
			Point pt = pnlGrid.PointToScreen(Grid.Location);
			pt = pnlColHeaderOuter.PointToClient(pt);
			m_pnlColHeaders.Left = pt.X + 1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustRowHeadingLocation()
		{
			Point pt = pnlGrid.PointToScreen(Grid.Location);
			pt = pnlRowHeaderOuter.PointToClient(pt);
			RowHeadersCollectionPanel.Top = pt.Y + 1;
		}

		#endregion

		#region Misc. event methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the height of the panel that owns the column header is adjust as the
		/// splitter below it moves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSplitterMoved(object sender, SplitterEventArgs e)
		{
			Adjust();
			m_hsplitter.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure new rows have their heights set properly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			foreach (DataGridViewRow row in Grid.Rows)
			{
				if (row.Height != m_cellHeight)
					row.Height = m_cellHeight;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the proper row and column header is selected as the current cell changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CurrentCellChanged(object sender, EventArgs e)
		{
			if (Grid.CurrentCell == null)
				return;

			// This will need to be added back in when moving rows and columns is added.
			//bool allowed = (m_grid.CurrentCell != null &&
			//    m_grid.CurrentCell.RowIndex < m_grid.Rows.Count - 2);
			////PaApp.MsgMediator.SendMessage("UpdateMoveCharChartRowDown", allowed);

			//allowed = (m_grid.CurrentCell != null &&
			//    m_grid.CurrentCell.RowIndex > 0);
			////PaApp.MsgMediator.SendMessage("UpdateMoveCharChartRowUp", allowed);

			// Check if the current row header changed.
			CharGridHeader hdr = GetRowsHeader(Grid.CurrentCell.RowIndex);
			if (hdr != m_currentRowHeader)
			{
				if (m_currentRowHeader != null)
					m_currentRowHeader.Selected = false;

				m_currentRowHeader = hdr;
				m_currentRowHeader.Selected = true;
			}

			// Check if the current column header changed.
			hdr = GetColumnsHeader(Grid.CurrentCell.ColumnIndex);
			if (hdr != m_currentColHeader)
			{
				if (m_currentColHeader != null)
					m_currentColHeader.Selected = false;

				m_currentColHeader = hdr;
				m_currentColHeader.Selected = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform a search on the phone when it's double-clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (SearchWhenPhoneDoubleClicked && !string.IsNullOrEmpty(CurrentPhone))
				App.MsgMediator.SendMessage("ChartPhoneSearchAnywhere", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all empty rows and columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveAllEmptyRowsAndColumns()
		{
			// Remove empty rows.
			for (int r = Grid.Rows.Count - 1; r >= 0; r--)
			{
				if (IsRowEmtpy(r))
					RemoveRow(r);
			}

			// Remove empty columns.
			for (int c = Grid.Columns.Count - 1; c >= 0; c--)
			{
				if (IsColumnEmtpy(c))
					RemoveColumn(c);
			}
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Custom paint cells being dragged over when dragging phones around.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			// Only paint the row headings and the first row.
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0 &&
				(Grid[e.ColumnIndex, e.RowIndex].Tag as string) == kDropTargetCell)
			{
				Color clr = ColorHelper.CalculateColor(SystemColors.WindowText,
					SystemColors.Window, 65);

				const TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
					TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
					TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsClipping;

				TextRenderer.DrawText(e.Graphics, m_phoneBeingDragged,
					ChartFont, e.CellBounds, clr, flags);

				e.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlCorner_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = pnlCorner.ClientRectangle;
			Point pt1 = new Point(rc.Width - 4, 0);
			Point pt2 = new Point(rc.Width - 4, rc.Bottom - 1);

			// Draw a double vertical line to the left of the first column heading
			// (which is the right edge of the top, left corner panel).
			using (LinearGradientBrush br = new LinearGradientBrush(pt1, pt2,
				kGridColor, SystemColors.GrayText))
			{
				using (Pen pen = new Pen(br))
				{
					e.Graphics.DrawLine(pen, pt1, pt2);
					pt1.X += 3;
					pt2.X += 3;
					e.Graphics.DrawLine(pen, pt1, pt2);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint double lines on the horizontal splitter. From the left edge of the splitter
		/// to where the vertical splitter begins will only be a single line (i.e. the line
		/// the separates the row headers from the empty area (corner panel) above the row
		/// headers and to the left of the column headers).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_hsplitter_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = m_hsplitter.ClientRectangle;
			int left = m_vsplitter.SplitPosition;

			// Draw the top line of the double dark line on the bottom edge.
			e.Graphics.DrawLine(SystemPens.GrayText,
				new Point(0, 0), new Point(left, 0));

			e.Graphics.DrawLine(SystemPens.GrayText,
				new Point(left + 3, 0), new Point(rc.Right - 1, 0));

			// Draw the bottom line of the double dark line on the bottom edge.
			e.Graphics.DrawLine(SystemPens.GrayText,
				new Point(0, rc.Bottom - 1), new Point(left, rc.Bottom - 1));

			e.Graphics.DrawLine(SystemPens.GrayText,
				new Point(left + 3, rc.Bottom - 1), new Point(rc.Right - 1, rc.Bottom - 1));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_vsplitter_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = m_vsplitter.ClientRectangle;

			// Draw the top line of the double dark line on the bottom edge.
			e.Graphics.DrawLine(SystemPens.GrayText,
				new Point(0, 0), new Point(0, rc.Bottom - 1));

			// Draw the bottom line of the double dark line on the bottom edge.
			e.Graphics.DrawLine(SystemPens.GrayText,
				new Point(rc.Right - 1, 0), new Point(rc.Right - 1, rc.Bottom - 1));
		}

		#endregion

		#region Methods for grid's phone information popup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			// This will not be empty when the mouse button is down.
			if (!m_mouseDownGridLocation.IsEmpty || e.ColumnIndex < 0 || e.RowIndex < 0 ||
				(Grid[e.ColumnIndex, e.RowIndex].Value as CharGridCell) == null ||
				!App.IsFormActive(FindForm()))
			{
				return;
			}

			Rectangle rc = Grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			if (m_phoneInfoPopup.Initialize(Grid[e.ColumnIndex, e.RowIndex]))
				m_phoneInfoPopup.Show(rc);
		}

		#endregion

		#region Grid Drag/Drop Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				m_mouseDownGridLocation = e.Location;
			else if (e.Button == MouseButtons.Right && e.ColumnIndex >= 0 && e.RowIndex >= 0)
			{
				Grid.CurrentCell = Grid[e.ColumnIndex, e.RowIndex];
				if (!Grid.Focused)
					Grid.Focus();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			m_mouseDownGridLocation = Point.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			// This will be empty when the mouse button is not down.
			if (m_mouseDownGridLocation.IsEmpty || e.ColumnIndex < 0 || e.RowIndex < 0 ||
				(Grid[e.ColumnIndex, e.RowIndex].Value as CharGridCell) == null)
			{
				return;
			}

			// Begin draging a cell when the mouse is held down
			// and has moved 4 or more pixels in any direction.
			int dx = Math.Abs(m_mouseDownGridLocation.X - e.X);
			int dy = Math.Abs(m_mouseDownGridLocation.Y - e.Y);
			if (dx >= 4 || dy >= 4)
			{
				m_mouseDownGridLocation = Point.Empty;
				DataGridViewCell cell = Grid[e.ColumnIndex, e.RowIndex];

				// When someone has subscribed to the drag event (which is really more like
				// a begin drag event) then call that. Otherwise, start a drag, drop event
				// within the grid.
				if (ItemDrag != null)
				{
					ItemDragEventArgs args = new ItemDragEventArgs(e.Button, cell.Value);
					ItemDrag(this, args);
				}
				else
				{
					m_phoneBeingDragged = ((CharGridCell)cell.Value).Phone;
					m_cellDraggedOver = cell;
					Grid.DoDragDrop(Grid[e.ColumnIndex, e.RowIndex], DragDropEffects.Move);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_DragOver(object sender, DragEventArgs e)
		{
			// Get the cell being dragged.
			DataGridViewTextBoxCell draggedCell =
				e.Data.GetData(typeof(DataGridViewTextBoxCell)) as DataGridViewTextBoxCell;

			Point pt = Grid.PointToClient(new Point(e.X, e.Y));
			DataGridView.HitTestInfo hinfo = Grid.HitTest(pt.X, pt.Y);

			// Drop is not allowed if we can't determine what cell we're over.
			if (hinfo.ColumnIndex < 0 || hinfo.ColumnIndex >= Grid.Columns.Count ||
				hinfo.RowIndex < 0 || hinfo.RowIndex >= Grid.Rows.Count || draggedCell == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			// Can't drop on a cell that already has data in it.
			DataGridViewTextBoxCell cellOver = Grid[hinfo.ColumnIndex, hinfo.RowIndex] as
				DataGridViewTextBoxCell;

			// If we're dragging over a cell that hasn't been given a drag-over background,
			// then repaint the cell that used to have a drag-over background so it has a
			// regular background.
			if (m_cellDraggedOver != null && m_cellDraggedOver != cellOver)
			{
				m_cellDraggedOver.Tag = null;
				Grid.InvalidateCell(m_cellDraggedOver);
			}

			if (cellOver == null || (cellOver.Value as CharGridCell) != null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			e.Effect = e.AllowedEffect;

			// If we're dragging over a cell that hasn't been given a drag-over background,
			// then repaint the cell being dragged over so it has a drag-over background.
			if (m_cellDraggedOver != cellOver)
			{
				m_cellDraggedOver = cellOver;
				m_cellDraggedOver.Tag = kDropTargetCell;
				Grid.InvalidateCell(m_cellDraggedOver);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_DragLeave(object sender, EventArgs e)
		{
			if (m_cellDraggedOver != null)
			{
				m_cellDraggedOver.Tag = null;
				Grid.InvalidateCell(m_cellDraggedOver);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_DragDrop(object sender, DragEventArgs e)
		{
			if (m_cellDraggedOver != null)
			{
				m_cellDraggedOver.Tag = null;
				Grid.InvalidateCell(m_cellDraggedOver);
			}

			m_cellDraggedOver = null;

			// Get the cell being dragged.
			DataGridViewTextBoxCell draggedCell =
				e.Data.GetData(typeof(DataGridViewTextBoxCell)) as DataGridViewTextBoxCell;

			Point pt = Grid.PointToClient(new Point(e.X, e.Y));
			DataGridView.HitTestInfo hinfo = Grid.HitTest(pt.X, pt.Y);

			// Get the cell we're dropping on.
			DataGridViewTextBoxCell droppedOnCell = null;
			if (hinfo.ColumnIndex >= 0 && hinfo.ColumnIndex < Grid.Columns.Count &&
				hinfo.RowIndex >= 0 && hinfo.RowIndex < Grid.Rows.Count && draggedCell != null)
			{
				droppedOnCell = Grid[hinfo.ColumnIndex, hinfo.RowIndex] as
					DataGridViewTextBoxCell;
			}

			if (droppedOnCell != null)
			{
				droppedOnCell.Value = draggedCell.Value;
				draggedCell.Value = null;
				Grid.CurrentCell = droppedOnCell;

				App.MsgMediator.SendMessage("ChartPhoneMoved",
					new object[] { this, droppedOnCell.Value as CharGridCell,
					draggedCell, droppedOnCell });
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the user wants to begin moving a phone using the keyboard. Check for an
		/// Alt, plus up, down, left or right arrow on a cell that contains a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Alt || Grid.CurrentCell == null || (m_phoneMovingHelper != null &&
				m_phoneMovingHelper.MovingInProgress))
			{
				return;
			}

			var cgc = Grid.CurrentCell.Value as CharGridCell;
			if (cgc == null)
				return;

			bool beginMove;

			switch (e.KeyCode)
			{
				case Keys.Up:
					beginMove = (Grid.CurrentCellAddress.Y > 0);
					break;

				case Keys.Down:
					beginMove = (Grid.CurrentCellAddress.Y < Grid.RowCount - 1);
					break;

				case Keys.Left:
					beginMove = (Grid.CurrentCellAddress.X > 0);
					break;

				case Keys.Right:
					beginMove = (Grid.CurrentCellAddress.X < Grid.ColumnCount - 1);
					break;

				default:
					return;
			}

			if (beginMove)
			{
				if (m_phoneMovingHelper == null)
					m_phoneMovingHelper = new CellKBMovingCellHelper(this);

				m_phoneMovingHelper.Reset(cgc, Grid.CurrentCell as DataGridViewTextBoxCell);
			}
		}

		#region Methods for grid panel's scrolling and resizing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the headers scroll with the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_LocationChanged(object sender, EventArgs e)
		{
			AdjustRowHeadingLocation();
			AdjustColumnHeadingLocation();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the location of the panel that owns all the column headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlGrid_Resize(object sender, EventArgs e)
		{
			AdjustRowHeadingLocation();
			AdjustColumnHeadingLocation();
		}

		#endregion

		#region Message for editing heading labels
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put the user in the edit mode for column header labels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditCharChartLabel(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			m_currentHeader.EditLabel();
			return true;
		}

		#endregion

		#region Message/update handlers for showing sub headings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle the visible state of a row heading's sub-headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowChartRowSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle the visible state of a column heading's sub-headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowChartColSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle the visible state of a heading's sub-headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowCharChartSubHeadings(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			m_currentHeader.SubHeadingsVisible = !m_currentHeader.SubHeadingsVisible;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the "Show sub-headings" menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowChartRowSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnUpdateShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the "Show sub-headings" menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowChartColSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnUpdateShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the "Show sub-headings" menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCharChartSubHeadings(object args)
		{
			var itemProps = args as TMItemProperties;

			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				(m_currentHeader == null && Visible) || itemProps == null)
			{
				return false;
			}

			int ownedCount = 0;

			if (Visible && m_currentHeader != null)
			{
				ownedCount = (m_currentHeader.IsForColumnHeadings ?
					m_currentHeader.OwnedColumns.Count : m_currentHeader.OwnedRows.Count);
			}

			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Enabled = (ownedCount > 1);
			itemProps.Checked = (Visible && m_currentHeader != null &&
				m_currentHeader.SubHeadingsVisible);

			return true;
		}

		#endregion

		#region Methods for adding new rows
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row above the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowBeforeTBMenu(object args)
		{
			return AddNewRow(true);
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row above the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowBefore(object args)
		{
			return AddNewRow(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row below the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowAfterTBMenu(object args)
		{
			return AddNewRow(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row below the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowAfter(object args)
		{
			return AddNewRow(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row to the grid (and to the current row's header) either before
		/// or after the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AddNewRow(bool beforeCurrRow)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				Grid.CurrentCell == null || Grid.CurrentCell.RowIndex < 0)
			{
				return false;
			}

			int currRowIndex = Grid.CurrentCell.RowIndex;
			DataGridViewRow currRow = Grid.Rows[currRowIndex];
			if (currRow.Tag == null || currRow.Tag.GetType() != typeof(CharGridHeader))
				return false;

			if (!beforeCurrRow)
				currRowIndex++;

			var newRow = new DataGridViewRow();
			newRow.Height = m_cellHeight;
			Grid.Rows.Insert(currRowIndex, newRow);
			((CharGridHeader)currRow.Tag).AddRow(Grid.Rows[currRowIndex]);
			CalcHeights();
			return true;
		}

		#endregion

		#region Methods for adding new columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added before the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColBeforeTBMenu(object args)
		{
			return AddNewColumn(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added after the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColAfterTBMenu(object args)
		{
			return AddNewColumn(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added before the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColBefore(object args)
		{
			return AddNewColumn(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added after the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColAfter(object args)
		{
			return AddNewColumn(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column to the grid (and to the current column's header) either before
		/// or after the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AddNewColumn(bool beforeCurrColumn)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				Grid.CurrentCell == null || Grid.CurrentCell.ColumnIndex < 0)
			{
				return false;
			}

			int currColIndex = Grid.CurrentCell.ColumnIndex;
			DataGridViewColumn currCol = Grid.Columns[currColIndex];
			if (currCol.Tag == null || currCol.Tag.GetType() != typeof(CharGridHeader))
				return false;

			DataGridViewColumn newCol = CreateColumn();
			((CharGridHeader)currCol.Tag).AddColumn(newCol);
			CalcWidths();
			Grid.Columns.Insert(currColIndex + (beforeCurrColumn ? 0 : 1), newCol);
			return true;
		}

		#endregion

		#region Methods for adding new row headings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowHeadingBeforeTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnAddCharChartRowHeadingBefore(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowHeadingBefore(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertRowHeader(true, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowHeadingAfterTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnAddCharChartRowHeadingAfter(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowHeadingAfter(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertRowHeader(false, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts a row header before the specified header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader InsertRowHeaderBefore(CharGridHeader hdr)
		{
			if (hdr != null)
			{
				m_currentHeader = hdr;
				return InsertRowHeader(true, false);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private CharGridHeader InsertRowHeader(bool beforeClickedOnRow, bool editLabel)
		{
			if (m_currentHeader != null)
				m_currentHeader.EndEditLabel();

			// Get the index of where in the grid we're going to
			// insert the new header's first grid column.
			int newGridRowIndex = (beforeClickedOnRow ?
				m_currentHeader.OwnedRows[0].Index :
				m_currentHeader.LastRow.Index + 1);

			// Get the index of where in the header collection we'll insert the new header.
			int newHdrRowIndex = RowHeaders.IndexOf(m_currentHeader);
			if (!beforeClickedOnRow)
				newHdrRowIndex++;

			var newHdr = CreateRowHeader(string.Empty, newHdrRowIndex);
			var newRow = new DataGridViewRow();
			newRow.Height = m_cellHeight;

			Grid.Rows.Insert(newGridRowIndex, newRow);
			newHdr.AddRow(Grid.Rows[newGridRowIndex]);
			Grid.CurrentCell = Grid[0, newGridRowIndex];

			if (editLabel)
				newHdr.EditLabel();

			return newHdr;
		}

		#endregion

		#region Methods for adding new column headings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColHeadingBeforeTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnAddCharChartColHeadingBefore(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColHeadingBefore(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertColumnHeader(true, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColHeadingAfterTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnAddCharChartColHeadingAfter(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColHeadingAfter(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertColumnHeader(false, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts a column header before the specified header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader InsertColumnHeaderBefore(CharGridHeader hdr)
		{
			if (hdr != null)
			{
				m_currentHeader = hdr;
				return InsertColumnHeader(true, false);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private CharGridHeader InsertColumnHeader(bool insertBeforeCurr, bool editLabel)
		{
			if (m_currentHeader != null)
				m_currentHeader.EndEditLabel();

			// Get the index of where in the grid we're going to
			// insert the new header's first grid column.
			int newGridColIndex = (insertBeforeCurr ?
				m_currentHeader.OwnedColumns[0].Index :
				m_currentHeader.LastColumn.Index + 1);

			// Get the index of where in the header collection we'll insert the new header.
			int newHdrColIndex = ColumnHeaders.IndexOf(m_currentHeader);
			if (!insertBeforeCurr)
				newHdrColIndex++;

			var newHdr = CreateColumnHeader(string.Empty, newHdrColIndex);
			var newCol = CreateColumn();

			newHdr.AddColumn(newCol);
			Grid.Columns.Insert(newGridColIndex, newCol);
			Grid.CurrentCell = Grid[newCol.Index, 0];

			if (editLabel)
				newHdr.EditLabel();

			return newHdr;
		}

		#endregion

		#region Methods for removing rows and columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row from the grid (toolbar menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartRowTBMenu(object args)
		{
			return OnRemoveCharChartRow(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes row toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartRowTBMenu(object args)
		{
			return OnUpdateRemoveCharChartRow(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column from the grid (toolbar menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartColTBMenu(object args)
		{
			return OnRemoveCharChartCol(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes column toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartColTBMenu(object args)
		{
			return OnUpdateRemoveCharChartCol(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row from the grid (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartRow(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || Grid.CurrentCell == null)
				return false;

			RemoveRow(Grid.CurrentCell.RowIndex);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes row context menu item (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartRow(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				itemProps == null || Grid.CurrentCell == null)
			{
				return false;
			}

			itemProps.Enabled = (Visible && IsRowEmtpy(Grid.CurrentCell.RowIndex));
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column from the grid (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartCol(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || Grid.CurrentCell == null)
				return false;

			RemoveColumn(Grid.CurrentCell.ColumnIndex);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes column context menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartCol(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				itemProps == null || Grid.CurrentCell == null)
			{
				return false;
			}

			itemProps.Enabled = (Visible && IsColumnEmtpy(Grid.CurrentCell.ColumnIndex));
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not the specified row is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsRowEmtpy(int rowIndex)
		{
			for (int i = 0; i < Grid.Columns.Count; i++)
			{
				var cgc = Grid[i, rowIndex].Value as CharGridCell;
				if (cgc != null && cgc.Visible && !string.IsNullOrEmpty(cgc.Phone))
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not the specified column is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsColumnEmtpy(int colIndex)
		{
			for (int i = 0; i < Grid.Rows.Count; i++)
			{
				var cgc = Grid[colIndex, i].Value as CharGridCell;
				if (cgc != null && cgc.Visible && !string.IsNullOrEmpty(cgc.Phone))
					return false;
			}

			return true;
		}

		#endregion

		#region Message handlers for removing all empty rows and columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all empty rows and columns in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveAllEmptyChartRowsAndColsTBMenu(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			RemoveAllEmptyRowsAndColumns();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the toolbar menu item for removing all empty rows and columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveAllEmptyChartRowsAndColsTBMenu(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || itemProps == null)
				return false;

			itemProps.Enabled = false;

			if (Visible)
			{
				// Check if any row headers have empty rows.
				if (RowHeaders.Any(hdr => hdr.IsAnyOwnedRowEmpty))
					itemProps.Enabled = true;

				// If none of the row headers had any empty rows, then
				// check if any columns headers have empty columns.
				if (!itemProps.Enabled)
				{
					if (ColumnHeaders.Any(hdr => hdr.IsAnyOwnedColumnEmpty))
						itemProps.Enabled = true;
				}
			}

			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		#endregion

		#region Message handlers and update handlers for removing row and column headings.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row heading and all its grid rows (toolbar button menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartRowHeadingTBMenu(object args)
		{
			m_currentHeader = (Grid.CurrentCell == null ? null :
				GetRowsHeader(Grid.CurrentCell.RowIndex));

			return OnRemoveCharChartRowHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove row heading for current cell toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartRowHeadingTBMenu(object args)
		{
			m_currentHeader = (Grid.CurrentCell == null ? null :
				GetRowsHeader(Grid.CurrentCell.RowIndex));

			return OnUpdateRemoveCharChartRowHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row heading and all its grid rows (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartRowHeading(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			RemoveRowHeader(m_currentHeader);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove row heading context menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartRowHeading(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				itemProps == null || (m_currentHeader == null && Visible))
			{
				return false;
			}

			itemProps.Visible = true;
			itemProps.Enabled = (Visible && m_currentHeader.AreAllOwnedRowsEmpty);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column heading and all its grid columns (toolbar item menu).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartColHeadingTBMenu(object args)
		{
			m_currentHeader = (Grid.CurrentCell == null ? null :
				GetColumnsHeader(Grid.CurrentCell.ColumnIndex));

			return OnRemoveCharChartColHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove column heading for current cell toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartColHeadingTBMenu(object args)
		{
			m_currentHeader = (Grid.CurrentCell == null ? null :
				GetColumnsHeader(Grid.CurrentCell.ColumnIndex));

			return OnUpdateRemoveCharChartColHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column heading and all its grid columns (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartColHeading(object args)
		{
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			RemoveColumnHeader(m_currentHeader);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove row heading context menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartColHeading(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				itemProps == null || (m_currentHeader == null && Visible))
			{
				return false;
			}

			itemProps.Visible = true;
			itemProps.Enabled = (Visible && m_currentHeader.AreAllOwnedColumnsEmpty);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves a row down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMoveCharChartRowDown(object args)
		{
			//if (!PaApp.IsFormActive(FindForm()) || m_currentHeader.GetType() != typeof(int))
			//    return false;

			//SwapRows((int)m_currentHeader, (int)m_currentHeader + 1);
			return true;
		}

		#endregion

		#region Other message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When a header is clicked, then make sure the one clicked becomes current and
		/// that the current cell tracks with it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCharGridHeaderClicked(object args)
		{
			var hdr = args as CharGridHeader;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || hdr == null)
				return false;

			if (hdr.IsForColumnHeadings)
			{
				if (hdr != m_currentColHeader)
				{
					int currRow = Grid.CurrentCell.RowIndex;
					Grid.CurrentCell = Grid[hdr.OwnedColumns[0].Index, currRow];
				}
			}
			else
			{
				if (hdr != m_currentRowHeader)
				{
					int currCol = Grid.CurrentCell.ColumnIndex;
					Grid.CurrentCell = Grid[currCol, hdr.OwnedRows[0].Index];
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Popup a context menu when a header is right-clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCharGridHeaderRightClicked(object args)
		{
			m_currentHeader = args as CharGridHeader;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || m_currentHeader == null)
				return false;

			string menu = (m_currentHeader.IsForColumnHeadings ?
				"cmnuCharChartColHeader" : "cmnuCharChartRowHeader");

			m_tmAdapter.PopupMenu(menu, MousePosition.X, MousePosition.Y);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the move row down option should be enabled. Only enable
		/// when the row heading clicked is not the last row in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateMoveCharChartRowDown(object args)
		{
			//TMItemProperties itemProps = args as TMItemProperties;
			//if (!PaApp.IsFormActive(FindForm()) || itemProps == null)
			//    return false;

			//itemProps.Update = true;

			//if (m_currentHeader.GetType() != typeof(int))
			//    itemProps.Enabled = false;
			//else
			//{
			//    int colIndex = (int)m_currentHeader;
			//    itemProps.Enabled = (colIndex < m_grid.Rows.Count - 2);
			//}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves a row up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMoveCharChartRowUp(object args)
		{
			//if (!PaApp.IsFormActive(FindForm()) || m_currentHeader.GetType() != typeof(int))
			//    return false;

			//SwapRows((int)m_currentHeader, (int)m_currentHeader - 1);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the move row up option should be enabled. Only enable
		/// when the row heading clicked is not the first row in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateMoveCharChartRowUp(object args)
		{
			//TMItemProperties itemProps = args as TMItemProperties;
			//if (!PaApp.IsFormActive(FindForm()) || itemProps == null)
			//    return false;

			//itemProps.Update = true;

			//if (m_currentHeader.GetType() != typeof(int))
			//    itemProps.Enabled = false;
			//else
			//{
			//    int colIndex = (int)m_currentHeader;
			//    itemProps.Enabled = (colIndex > 0);
			//}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartColHeadingBefore(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) || itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = Visible;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartColHeadingAfter(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartRowHeadingBefore(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartRowHeadingAfter(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartRowBefore(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartRowAfter(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartColBefore(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCharChartColAfter(object args)
		{
			return OnUpdateAddCharChartColHeadingBefore(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCVChartRowsColsParent(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = Visible;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAddCVChartRowsColsParent(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = Visible;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			foreach (DataGridViewColumn col in Grid.Columns)
				col.DefaultCellStyle.Font = ChartFont;

			// Return false to allow other windows to update their fonts.
			return false;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Swaps the contents of the specified rows.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void SwapRows(int row1, int row2)
		//{
		//    // Swap the values in the row we're moving down with the
		//    // values in the row below it.
		//    for (int i = 0; i < m_grid.Columns.Count; i++)
		//    {
		//        string tmpPhone = m_grid[i, row2].Value as string;
		//        m_grid[i, row2].Value = m_grid[i, row1].Value;
		//        m_grid[i, row1].Value = tmpPhone;
		//    }

		//    // Now swap the row heading labels.
		//    //string tmpLabel = m_rowHdrTexts[row2];
		//    //m_rowHdrTexts[row2] = m_rowHdrTexts[row1];
		//    //m_rowHdrTexts[row1] = tmpLabel;
		//    m_grid.Refresh();
		//}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}
	
	#region CellKBMovingCellHelper class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class CellKBMovingCellHelper
	{
		private bool m_movingInProgress;
		private DataGridViewTextBoxCell m_originalCell;
		private DataGridViewTextBoxCell m_previousCell;
		private CharGridCell m_cgc;
		private bool m_drawNoDropIcon;
		private Rectangle m_rcNoDropIcon;
		private readonly DataGridView m_grid;
		private readonly CharGrid m_chrGrid;
		private readonly Color m_defaultCellSelectedBackColor;
		private readonly Color m_defaultCellSelectedForeColor;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal CellKBMovingCellHelper(CharGrid chrGrid)
		{
			System.Diagnostics.Debug.Assert(chrGrid != null);
			m_chrGrid = chrGrid;

			if (m_chrGrid.Grid != null)
			{
				m_grid = m_chrGrid.Grid;
				m_grid.Paint += m_grid_Paint;
				m_grid.CellEnter += m_grid_CellEnter;
				m_grid.CellLeave += m_grid_CellLeave;
				m_grid.KeyUp += m_grid_KeyUp;
				m_defaultCellSelectedBackColor = m_grid.DefaultCellStyle.SelectionBackColor;
				m_defaultCellSelectedForeColor = m_grid.DefaultCellStyle.SelectionForeColor;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Reset(CharGridCell cgc, DataGridViewTextBoxCell origCell)
		{
			m_cgc = cgc;
			m_originalCell = origCell;
			m_previousCell = origCell;
			m_movingInProgress = true;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MovingInProgress
		{
			get { return m_movingInProgress; }
			set { m_movingInProgress = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal DataGridViewTextBoxCell OriginalCell
		{
			get { return m_originalCell; }
			set { m_originalCell = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal DataGridViewTextBoxCell PreviousCell
		{
			get { return m_previousCell; }
			set { m_previousCell = value; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_KeyUp(object sender, KeyEventArgs e)
		{
			// When the Alt key goes up, the key data is (Keys.RButton | Keys.ShiftKey).
			// I'm sure that should be obvious to me, but I'm dense enough to wonder why.
			if (!m_movingInProgress || e.KeyData != (Keys.RButton | Keys.ShiftKey))
				return;

			m_movingInProgress = false;
			HideNoDropIndicator(m_rcNoDropIcon);
			DataGridViewTextBoxCell targetCell = m_grid.CurrentCell as DataGridViewTextBoxCell;

			// If the target cell to which the user is moving a phone is not the same
			// as the last one 
			if (targetCell != null)
			{
				CharGridCell currentCellsCgc = targetCell.Value as CharGridCell;

				// Can't leave moved phone on a cell that already contains
				// a phone so put the phone back from whence it came.
				if (currentCellsCgc != null && currentCellsCgc != m_cgc)
				{
					SystemSounds.Beep.Play();
					m_originalCell.Value = m_cgc;
					m_grid.CurrentCell = m_originalCell;
				}
				else
				{
					App.MsgMediator.SendMessage("ChartPhoneMoved",
						new object[] {m_chrGrid, currentCellsCgc, m_originalCell, targetCell});
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (m_movingInProgress)
			{
				DataGridViewTextBoxCell cell = m_grid[e.ColumnIndex, e.RowIndex] as DataGridViewTextBoxCell;
				HideNoDropIndicator(cell);

				// We're leaving a cell so when that cell contains the phone we're moving
				// clear it because the phone will be placed in the cell we're moving to.
				if (m_previousCell != null && (cell != null && cell.Value == m_cgc))
					m_previousCell.Value = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewTextBoxCell cell = m_grid[e.ColumnIndex, e.RowIndex] as DataGridViewTextBoxCell;

			// Do nothing if we're not in the progress of moving or if, for some odd reason,
			// we cell we just entered is not a text box cell. The latter should never happen.
			if (!m_movingInProgress || cell == null)
				return;

			CharGridCell currentCellsCgc = cell.Value as CharGridCell;

			// If the cell just entered is already occupied by another phone then show
			// an indicator in it, telling the user it's off limits. Otherwise, set the
			// cell's value to the moving phone.
			if (currentCellsCgc != null)
				ShowNoDropIndicator(cell);
			else
				cell.Value = m_cgc;

			// Keep track of the cell the phone current cell.
			m_previousCell = cell;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowNoDropIndicator(DataGridViewCell cell)
		{
			if (m_grid == null || cell == null)
				return;

			m_drawNoDropIcon = true;
			m_rcNoDropIcon = m_grid.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);
			Image img = Properties.Resources.kimidPhoneDropNotAllowed;

			// Force the rectangle to the degree that we know it's size is
			// sufficient to hold the image without clipping. Inflating will
			// maintain the original rectangle's center point.
			m_rcNoDropIcon.Inflate(img.Width, img.Height);

			// Now calculate the coordinate where the image would
			// be if it is to be drawn in the center of the rectangle.
			int x = m_rcNoDropIcon.X + (m_rcNoDropIcon.Width - img.Width) / 2;
			int y = m_rcNoDropIcon.Y + (m_rcNoDropIcon.Height - img.Height) / 2;

			// Now shrink the rectangle to the size of the image, while
			// maintaining the drawing coordinate just calculated.
			m_rcNoDropIcon = new Rectangle(x, y, img.Width, img.Height);

			m_grid.DefaultCellStyle.SelectionBackColor = m_grid.BackgroundColor;
			m_grid.DefaultCellStyle.SelectionForeColor = m_grid.ForeColor;
			m_grid.Invalidate(m_rcNoDropIcon);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void HideNoDropIndicator(DataGridViewCell cell)
		{
			if (cell != null && m_grid != null)
			{
				Rectangle rc = m_grid.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);

				// Inflate the rectangle just to make sure we get rid of all residue.
				rc.Inflate(10, 10);
				HideNoDropIndicator(rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void HideNoDropIndicator(Rectangle rc)
		{
			m_drawNoDropIcon = false;

			if (m_grid != null)
			{
				m_grid.DefaultCellStyle.SelectionBackColor = m_defaultCellSelectedBackColor;
				m_grid.DefaultCellStyle.SelectionForeColor = m_defaultCellSelectedForeColor;
				m_grid.Invalidate(rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_Paint(object sender, PaintEventArgs e)
		{
			if (m_drawNoDropIcon && m_movingInProgress)
			{
				e.Graphics.DrawImageUnscaledAndClipped(Properties.Resources.kimidPhoneDropNotAllowed,
					m_rcNoDropIcon);
			}
		}
	}

	#endregion

	#region CharGridCell Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Phone")]
	public class CharGridCell
	{
		private string m_phone;
		private bool m_visible = true;
		private bool m_isUncertain;
		private bool m_isPlacedOnChart;
		private int m_defaultCol = -1;
		private int m_defaultGroup = -1;
		private int m_totalCount;
		private int m_countAsPrimaryUncertainty;
		private int m_countAsNonPrimaryUncertainty;
		private List<string> m_siblingUncertainties = new List<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is for serialization/deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridCell()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridCell(string phone)
		{
			m_phone = phone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridCell(string phone, bool visible) : this(phone)
		{
			m_visible = visible;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the cell's phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (m_visible ? m_phone : null);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("text")]
		public string Phone
		{
			get { return m_phone; }
			set { m_phone = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool Visible
		{
			get { return m_visible; }
			set { m_visible = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Row")]
		public string InternalRow { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Row
		{
			get
			{
				int row;
				return (int.TryParse(InternalRow, out row) ? row : -1);
			}
			set { InternalRow = value.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Column")]
		public string InternalColumn{ get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Column
		{
			get
			{
				int col;
				return (int.TryParse(InternalColumn, out col) ? col : -1);
			}
			set { InternalColumn = value.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Group")]
		public string InternalGroup { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Group
		{
			get
			{
				int grp;
				return (int.TryParse(InternalGroup, out grp) ? grp : -1);
			}
			set { InternalGroup = value.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The number of times the phone occurs when it's neither a primary nor non primary
		/// uncertainty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int TotalCount
		{
			get { return m_totalCount; }
			set { m_totalCount = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The number of times the phone occurs as a primary uncertain phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int CountAsPrimaryUncertainty
		{
			get { return m_countAsPrimaryUncertainty; }
			set { m_countAsPrimaryUncertainty = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The number of times the phone occurs as a non primary uncertain phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int CountAsNonPrimaryUncertainty
		{
			get { return m_countAsNonPrimaryUncertainty; }
			set { m_countAsNonPrimaryUncertainty = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The list of uncertain phones found together with the cell's phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> SiblingUncertainties
		{
			get { return m_siblingUncertainties; }
			set { m_siblingUncertainties = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DefaultColumn
		{
			get { return m_defaultCol; }
			set { m_defaultCol = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DefaultGroup
		{
			get { return m_defaultGroup; }
			set { m_defaultGroup = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsPlacedOnChart
		{
			get { return m_isPlacedOnChart; }
			set { m_isPlacedOnChart = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsUncertain
		{
			get { return m_isUncertain; }
			set { m_isUncertain = value; }
		}

		#endregion
	}

	#endregion

	#region CharGridView class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CharGridView : DataGridView
	{
		delegate void AutoScrollPositionDelegate(ScrollableControl sender, Point p);
		private Panel m_owner;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridView()
		{
			DoubleBuffered = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetDoubleBuffering(bool turnOn)
		{
			DoubleBuffered = turnOn;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal Panel OwningPanel
		{
			set { m_owner = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This prevents the grid's owning panel from scrolling to 0,0 whenever the focus
		/// leaves the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLostFocus(EventArgs e)
		{
			if (m_owner == null)
				return;

			Point pt = m_owner.AutoScrollPosition;
			AutoScrollPositionDelegate del = SetAutoScrollPosition;
			Object[] args = { m_owner, pt };
			BeginInvoke(del, args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This prevents the grid's owning panel from scrolling to 0,0 whenever the focus
		/// leaves the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			if (m_owner == null)
				return;

			try
			{
				Point pt = m_owner.AutoScrollPosition;
				AutoScrollPositionDelegate del = SetAutoScrollPosition;
				Object[] args = { m_owner, pt };

				// This will throw an error when the view this grid is on is disposing. Why
				// the OnLeave event for the control gets called when the outer-most parent
				// is disposing is beyond me. But, it does, which causes a crash if not handled.
				BeginInvoke(del, args);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This prevents the grid's owning panel from scrolling to 0,0 whenever the grid
		/// gains focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnGotFocus(EventArgs e)
		{
			if (m_owner == null)
				return;

			Point pt = m_owner.AutoScrollPosition;
			AutoScrollPositionDelegate del = SetAutoScrollPosition;
			Object[] args = { m_owner, pt };
			BeginInvoke(del, args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This prevents the grid's owning panel from scrolling to 0,0 whenever the grid
		/// gains focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			if (m_owner == null)
				return;

			Point pt = m_owner.AutoScrollPosition;
			AutoScrollPositionDelegate del = SetAutoScrollPosition;
			Object[] args = { m_owner, pt };
			BeginInvoke(del, args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetAutoScrollPosition(ScrollableControl sender, Point pt)
		{
			if (!pt.IsEmpty)
			{
				pt.X = Math.Abs(pt.X);
				pt.Y = Math.Abs(pt.Y);
				sender.AutoScrollPosition = pt;
			}
		}
	}

	#endregion
}
