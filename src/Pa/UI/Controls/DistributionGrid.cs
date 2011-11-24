using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class DistributionGrid : SilGrid, IxCoreColleague
	{
		private bool m_paintDropValidEffect;
		private bool m_mouseDownOnCornerCell;
		private string m_preEditCellValue;
		private int m_defaultRowHeight;
		private Point m_prevCurrCellAddress = new Point(-1, -1);
		private Label m_lblName;
		private ToolTip m_tooltip;
		private IMessageFilter m_editControlMsgFilter;
		private SearchOptionsDropDown m_searchOptionsDropDown;
		//private readonly Image m_dirtyIndicator;
		private readonly Bitmap m_errorInCell;
		private readonly DistributionChartCellInfoPopup m_cellInfoPopup;

		/// ------------------------------------------------------------------------------------
		public DistributionGrid()
		{
			OnPaFontsChanged(null);

			//m_dirtyIndicator = Properties.Resources.kimidXYChartDirtyIndicator;
			m_errorInCell = Properties.Resources.kimidXYChartCellError;
			m_errorInCell.MakeTransparent(m_errorInCell.GetPixel(0, 0));

			base.Font = FontHelper.UIFont;
			DefaultCellStyle.Font = FontHelper.UIFont;
			DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			DefaultCellStyle.ForeColor = SystemColors.ControlText;
			AllowUserToAddRows = true;
			AllowUserToDeleteRows = true;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
	 		BackgroundColor = SystemColors.Window;
			SelectionMode = DataGridViewSelectionMode.CellSelect;
			ColumnHeadersVisible = false;
			RowHeadersVisible = false;
			base.AllowDrop = true;
			ShowCellToolTips = false;
			ShowWaterMarkWhenDirty = true;

			Reset();
			App.AddMediatorColleague(this);
			m_cellInfoPopup = new DistributionChartCellInfoPopup(this);

			SetToolTips();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// June M. discovered that if you choose to load a different project while you're
				// in the edit mode of an XY chart cell, the program crashes when the XYGrid is
				// disposed. This will prevent that. I don't know how she finds these things.
				EndEdit();

				if (m_tooltip != null)
					m_tooltip.Dispose();

				if (m_searchOptionsDropDown != null)
					m_searchOptionsDropDown.Dispose();

				if (m_cellInfoPopup != null)
					m_cellInfoPopup.Dispose();

				if (m_errorInCell != null)
					m_errorInCell.Dispose();

				App.RemoveMediatorColleague(this);
			}
			
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFromLayout(DistributionChart layout)
		{
			if (layout == null)
				return;

			Reset();

			ChartLayout = layout.Clone();

			// Add columns for the environments.
			if (ChartLayout.SearchQueries.Count > 0)
			{
				foreach (SearchQuery query in ChartLayout.SearchQueries)
					AddColumn(query, false);
			}

			// Add the search items in the first column of the grid.
			if (ChartLayout.SearchItems.Count > 0)
			{
				int i = 1;
				Rows.Add(ChartLayout.SearchItems.Count);
				foreach (string srchItem in ChartLayout.SearchItems)
					this[0, i++].Value = srchItem;
			}

			// Set the column sizes.
			if (ChartLayout.ColumnWidths.Count > 0 && Columns.Count >= ChartLayout.ColumnWidths.Count)
			{
				for (int i = 0; i < ChartLayout.ColumnWidths.Count; i++)
					Columns[i].Width = ChartLayout.ColumnWidths[i];
			}

			if (m_lblName != null)
				m_lblName.Text = ChartLayout.NameOrNone;

			IsDirty = false;
			LoadedLayout = true;

			App.MsgMediator.SendMessage("DistributionChartLoadedFromLayout", this);

			// Go ahead and fill the chart after loading it.
			if (!IsEmpty)
				FillChart();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateLayout()
		{
			if (ChartLayout != null)
				ChartLayout.UpdateFromDistributionGrid(this);
			else
			{
				ChartLayout = DistributionChart.NewFromDistributionGrid(this);
				LoadedLayout = false;
			}

			if (m_lblName != null)
				m_lblName.Text = ChartLayout.NameOrNone;
			
			IsDirty = true;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the search options drop-down control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchOptionsDropDown SearchOptionsDropDown
		{
			get
			{
				if (m_searchOptionsDropDown == null)
				{
					m_searchOptionsDropDown = new SearchOptionsDropDown();
					m_searchOptionsDropDown.ShowApplyToAll = true;
					m_searchOptionsDropDown.ApplyToAllLinkLabel.Click += ApplyToAllLinkLabel_Click;
					m_searchOptionsDropDown._linkHelp.Click += SearchDropDownHelpLink_Click;
					m_searchOptionsDropDown.Disposed += m_searchOptionsDropDown_Disposed;
				}
				
				return m_searchOptionsDropDown;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The only time this will be disposed before the program terminates is when the
		/// view is redocked after being undocked. That is because the toolbar/menu adapter
		/// is disposed and recreated when the view is being redocked. And when the TMAdapter
		/// is disposed, so are the custom controls it hosts in drop-downs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_searchOptionsDropDown_Disposed(object sender, EventArgs e)
		{
			m_searchOptionsDropDown.ApplyToAllLinkLabel.Click -= ApplyToAllLinkLabel_Click;
			m_searchOptionsDropDown._linkHelp.Click -= SearchDropDownHelpLink_Click;
			m_searchOptionsDropDown.Disposed -= m_searchOptionsDropDown_Disposed;
			m_searchOptionsDropDown = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the chart's owning form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITabView OwningView { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the current layout was loaded using the
		/// LoadFromLayout() method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool LoadedLayout { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the chart's toolbar/menu adapter. This should be set by the chart's
		/// owning form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the chart layout that has been loaded in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DistributionChart ChartLayout { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the label control in which the chart displays the name of the layout.
		/// This should be set by the chart's owning form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Label OwnersNameLabelControl
		{
			get { return m_lblName; }
			set
			{
				m_lblName = value;
				if (m_lblName != null && ChartLayout != null)
					m_lblName.Text = ChartLayout.NameOrNone;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the chart that's currently loaded in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ChartName
		{
			get { return ChartLayout == null ? null : ChartLayout.Name; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a default name for the chart based on the search items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DefaultName
		{
			get
			{
				StringBuilder bldr = new StringBuilder();
				for (int i = 1; i < Rows.Count; i++)
				{
					string value = this[0, i].Value as string;
					if (value != null)
					{
						bldr.Append(Rows[i].Cells[0].Value as string);
						if (i < Rows.Count - 2)
							bldr.Append(',');
					}
				}

				return bldr.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the current cell contains a search result
		/// count.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCurrentCellValidForSearch
		{
			get
			{
				Point cell = CurrentCellAddress;
				return (cell.X > 0 && cell.Y > 0 && this[cell.X, cell.Y].Value != null); 
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full search query (i.e. search item plus environments) for the 
		/// current cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery CurrentCellsFullSearchQuery
		{
			get
			{
				return (CurrentCell == null ? null :
					GetCellsFullSearchQuery(CurrentCell.RowIndex, CurrentCell.ColumnIndex));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the chart contains any search items or
		/// environments.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsEmpty
		{
			get
			{
				for (int r = 1; r < Rows.Count; r++)
				{
					// Find a row with a search item.
					if (r == NewRowIndex || string.IsNullOrEmpty(this[0, r].Value as string))
						continue;

					// We've found a row with a search item, so
					// try to find a column with an environment.
					for (int c = 1; c < Columns.Count; c++)
					{
						if (!string.IsNullOrEmpty(this[c, 0].Value as string))
							return false;
					}
				}

				return true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the chart can be cleared.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CanClear
		{
			get {return (Columns.Count > 2 || Rows.Count > 2);}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void SetToolTips()
		{
			m_tooltip = new ToolTip();
			m_tooltip.IsBalloon = true;
			m_tooltip.UseFading = true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnViewDocked(object args)
		{
			SetToolTips();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			SetToolTips();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the search query for the specified column index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery GetColumnsSearchQuery(int col)
		{
			return (col <= 0 || col >= Columns.Count ? null :
				(Columns[col].Tag as SearchQuery));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full search query (i.e. search item plus environments) for the specified
		/// row and column. This method returns a clone of the query stored in the column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery GetCellsFullSearchQuery(int row, int col)
		{
			SearchQuery query = GetColumnsSearchQuery(col);
			if (query == null || row <= 0 || row >= Rows.Count)
				return null;

			query = query.Clone();
			query.Pattern = this[0, row].Value as string;
			query.Pattern += "/" + this[col, 0].Value;
			return query;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether not the cell specified by row and col contains
		/// a search result.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCellValidForSearch(int row, int col)
		{
			return (row > 0 && col > 0 && row < Rows.Count && col < Columns.Count &&
				this[col, row].Value != null);
		}

		#region Adding Column methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a column with a new query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddColumn(bool updateLayout)
		{
			DataGridViewColumn col = CreateTextBoxColumn(string.Empty);
			col.Tag = new SearchQuery();
			Columns.Add(col);

			if (updateLayout)
				UpdateLayout();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a column with the specified query. The column will always be inserted before
		/// the last empty column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddColumn(SearchQuery query, bool updateLayout)
		{
			DataGridViewColumn col = CreateTextBoxColumn(string.Empty);
			col.Tag = query;

			int newColIndex = Columns.Count - 1;
			SearchQuery lastColQuery = Columns[newColIndex].Tag as SearchQuery;
		
			// If the last column is empty then insert this column before it.
			// Otherwise, add this column to the end of the column collection.
			if (lastColQuery == null || string.IsNullOrEmpty(lastColQuery.Pattern))
				Columns.Insert(newColIndex, col);
			else
			{
				Columns.Add(col);
				newColIndex = Columns.Count - 1;
			}

			// When the environment row is present, then make sure the environment
			// cell for the new column contains the environment string.
			if (Rows.Count > 0)
				this[newColIndex, 0].Value = query.Pattern;

			if (updateLayout)
				UpdateLayout();
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnWidthChanged(e);

			if (ChartLayout != null && ChartLayout.ColumnWidths != null &&
				e.Column.Index < ChartLayout.ColumnWidths.Count)
			{
				//IsDirty = true;
				ChartLayout.ColumnWidths[e.Column.Index] = e.Column.Width;
				Invalidate(GetCellDisplayRectangle(0, 0, true));
			}
		}
		
		/// ------------------------------------------------------------------------------------
		protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
		{
			// Save the value before editing begins.
			m_preEditCellValue = this[e.ColumnIndex, e.RowIndex].Value as string;
			base.OnCellBeginEdit(e);
		}

		#region EditControlKeyPressHook class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This class is only for trapping the Ctrl-0 keypress for inserting diacritic
		/// placeholders into cells. See comments in OnEditingControlShowing for more details.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal class EditControlKeyPressHook : IMessageFilter
		{
			readonly TextBox m_txtBox;
			readonly DataGridView m_grid;

			/// --------------------------------------------------------------------------------
			internal EditControlKeyPressHook(TextBox txtBox, DataGridView grid)
			{
				m_txtBox = txtBox;
				m_grid = grid;
			}

			/// --------------------------------------------------------------------------------
			/// <summary>
			/// Hook the Ctrl+0 key press to insert a diacritic placeholder. We might as well
			/// trap Esc too, to get rid of that insessent beep that would otherwise be heard
			/// when the user leaves edit mode via pressing escape.
			/// </summary>
			/// --------------------------------------------------------------------------------
			public bool PreFilterMessage(ref Message m)
			{
				if (m.Msg == 0x0100)
				{
					if ((ModifierKeys & Keys.Control) == Keys.Control &&
						m.WParam.ToInt32() == (int)Keys.D0)
					{
						PatternTextBox.Insert(m_txtBox, App.DiacriticPlaceholder);
						return true;
					}
					
					if (m.WParam.ToInt32() == (int)Keys.Escape)
					{
						m_grid.EndEdit();
						return true;
					}
				}

				return false;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
		{
			base.OnEditingControlShowing(e);

			TextBox txt = e.Control as TextBox;
			if (txt != null)
			{
				txt.TextChanged += CellTextBoxTextChanged;
				txt.KeyPress += CellTextBoxKeyPress;
				txt.VisibleChanged += CellTextBoxVisibleChanged;

				// This is a lot of trouble just to trap Ctrl+0 in order to insert diacritic
				// placeholders, but it's the only way I found to accomplish it, short of
				// subclassing the grid cell. For some reason, Ctrl+0 doesn't fire the
				// edit box's KeyDown event. Argh!
				m_editControlMsgFilter = new EditControlKeyPressHook(txt, this);
				Application.AddMessageFilter(m_editControlMsgFilter);
			}
		}

		/// ------------------------------------------------------------------------------------
		void CellTextBoxVisibleChanged(object sender, EventArgs e)
		{
			TextBox txt = sender as TextBox;
			if (txt != null && !txt.Visible)
			{
				txt.TextChanged -= CellTextBoxTextChanged;
				txt.KeyPress -= CellTextBoxKeyPress;
				txt.VisibleChanged -= CellTextBoxVisibleChanged;

				if (m_editControlMsgFilter != null)
				{
					Application.RemoveMessageFilter(m_editControlMsgFilter);
					m_editControlMsgFilter = null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		static void CellTextBoxTextChanged(object sender, EventArgs e)
		{
			// For some reason, it didn't work to directly assign the static delegate
			// from PatternTextBox to the text box event. So I call it this way.
			PatternTextBox.HandlePatternTextBoxTextChanged(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		static void CellTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			// For some reason, it didn't work to directly assign the static delegate
			// from PatternTextBox to the text box event. So I call it this way.
			PatternTextBox.HandlePatternTextBoxKeyPress(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
		{
			base.OnCellEndEdit(e);

			int row = e.RowIndex;
			int col = e.ColumnIndex;

			string value = (this[col, row].Value == null ? null : this[col, row].Value as string);
			string prevValue = m_preEditCellValue;
			m_preEditCellValue = null;

			// Check if the previous value is the same as the new
			// value. If so, we don't need to do anything more here.
			if (value == prevValue)
				return;

			// Add a new column if the last column contains an environment.
			if (Columns.Count > 0 && !string.IsNullOrEmpty(this[Columns.Count - 1, 0].Value as string))
				AddColumn(true);

			// When an edited search item or environment is different from the previous value
			// (which we know is the case if we've gotten this far) clear out the result cells
			// associated with it.
			if (col == 0)
				ClearRow(row);
			else if (row == 0)
			{
				ClearColumn(col);
		
				// Update the column's query.
				SearchQuery query = Columns[col].Tag as SearchQuery;
				if (query != null)
					query.Pattern = this[col, 0].Value as string;
			}

			UpdateLayout();
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Uncomment this if it's desired to have the cell go into edit mode as soon
		///// the user clicks in a search item or environment cell.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override void OnCellClick(DataGridViewCellEventArgs e)
		//{
		//    base.OnCellClick(e);

		//    if (e.RowIndex == 0 || e.ColumnIndex == 0)
		//    {
		//        // When the user clicks on a search item or an environment
		//        // cell, put them in the edit mode in that cell.
		//        BeginEdit(true);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prevent the top, left cell from ever becoming the current by clicking on it and
		/// cause right-clicking on a cell to make that cell current.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (m_mouseDownOnCornerCell)
				return;

			if (e.Button == MouseButtons.Right)
			{
				CurrentCell = this[e.ColumnIndex, e.RowIndex];
				Focus();
			}

			base.OnCellMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void WndProc(ref Message m)
		{
			// Monitor a mouse down and eat any that are on the top, left cell.
			if (m.HWnd == Handle && m.Msg == 0x201 || m.Msg == 0x204 || m.Msg == 0x207)
			{
				int x = (m.LParam.ToInt32() & 0x0000FFFF);
				int y = (int)(m.LParam.ToInt32() & 0xFFFF0000) / 0x10000;

				m_mouseDownOnCornerCell = false;

				// Don't ignore the click if the sizing cursor is showing (since clicks
				// when the sizing cursor is showing don't make the cell active).
				if (Cursor != Cursors.SizeWE)
				{
					HitTestInfo hinfo = HitTest(x, y);
					if (hinfo.ColumnIndex == 0 && hinfo.RowIndex == 0)
					{
						m_mouseDownOnCornerCell = true;
						m.Result = IntPtr.Zero;
						m.Msg = 0;
					}
				}
			}

			base.WndProc(ref m);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
		{
			m_tooltip.Hide(this);
			base.OnCellMouseLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (IsCurrentCellInEditMode)
				return;

			int row = e.RowIndex;
			int col = e.ColumnIndex;

			if (col < 0 || row < 0 || (col == 0 && row == 0))
				return;

			// Show helpful information when the mouse is over an
			// empty search item or environment cell.
			if (col == 0 || row == 0)
			{
				if (this[col, row].Value == null)
				{
					string text;

					if (col == 0)
					{
						m_tooltip.ToolTipTitle = App.GetString(
							"DistributionGrid.AddSearchItemCellToolTipTitle",
							"Search Item Column:", "Views");
						
						text = App.GetString(
							"DistributionGrid.AddSearchItemCellToolTip",
							"Add a search item in this cell", "Views");
					}
					else
					{
						m_tooltip.ToolTipTitle = App.GetString(
							"DistributionGrid.AddEnvironmentCellToolTipTitle",
							"Environment Row:", "Views");

						text = App.GetString(
							"DistributionGrid.AddEnvironmentCellToolTip",
							"Add a search environment\nin this cell", "Views");
					}

					var rc = GetCellDisplayRectangle(col, row, false);
					rc.X = rc.Right - 7;
					rc.Y = rc.Bottom - 7;
					m_tooltip.Show(Utils.ConvertLiteralNewLines(text), this, rc.Location);
				}

				return;
			}

			var query = GetCellsFullSearchQuery(row, col);
			var pattern = (query == null ? "?" : query.Pattern);
			var exception = this[col, row].Value as SearchQueryException;

			if (exception != null)
			{
				// When the cell's value is an exception, it is because the
				// query generated an error when searching took place.
				m_cellInfoPopup.Initialize(pattern, this[col, row], exception.QueryErrorMessage);
				m_cellInfoPopup.Show();
			}
			else if (this[col, row].Tag is string[] || this[col, row].Tag is char[])
			{
				// When cell's tag is an array of strings it must contain a list of phones
				// found in the query that are not found in the project's phone inventory.
				// When the cell's tag is an array of characters it must contain a list of
				// characters found in the query that are not in PA's phonetic character
				// inventory (i.e. undefined phonetic characters).
				m_cellInfoPopup.Initialize(pattern, this[col, row]);
				m_cellInfoPopup.Show();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will do three things: 1) prevent the top, left cell from becoming current
		/// using the keyboard, 2) allow the user delete a column or row when the first cell
		/// in that column or row is the current cell, and 3) drop down a column's query
		/// options when the user presses Alt-Down while the current cell isn't one of the
		/// search item cells.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Modifiers == Keys.None &&
				(e.KeyCode == Keys.Left && CurrentCellAddress == new Point(1, 0)) ||
				(e.KeyCode == Keys.Up && CurrentCellAddress == new Point(0, 1)))
			{
				e.Handled = true;
				return;
			}

			if (e.Modifiers == Keys.None && e.KeyCode == Keys.Delete &&
				(CurrentCellAddress.X == 0 || CurrentCellAddress.Y == 0) &&
				CurrentCellAddress.X < Columns.Count - 1 &&	CurrentCellAddress.Y < NewRowIndex &&
				CurrentCellAddress != Point.Empty)
			{
				if (CurrentCellAddress.X == 0)
				{
					Rows.RemoveAt(CurrentCellAddress.Y);
					if (CurrentCellAddress == Point.Empty)
						CurrentCell = this[0, 1];
				}
				else
				{
					Columns.RemoveAt(CurrentCellAddress.X);
					if (CurrentCellAddress == Point.Empty)
						CurrentCell = this[1, 0];
				}

				e.Handled = true;
				UpdateLayout();
				return;
			}

			if (e.Alt && e.KeyCode == Keys.Down && CurrentCellAddress.X > 0 &&
			   Columns[CurrentCellAddress.X].Tag is SearchQuery &&
			   !string.IsNullOrEmpty((Columns[CurrentCellAddress.X].Tag as SearchQuery).Pattern))
			{
				// TODO: show options drop-down
			}

			base.OnKeyDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the current cell changes to a search item cell, highlight the entire row.
		/// If the current cell changes to an environment cell, highlight the entire column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCurrentCellChanged(EventArgs e)
		{
			if (m_prevCurrCellAddress.X >= 0)
				InvalidateColumn(m_prevCurrCellAddress.X);

			if (m_prevCurrCellAddress.Y >= 0)
				InvalidateRow(m_prevCurrCellAddress.Y);

			if (CurrentCellAddress.X == 0 && CurrentCellAddress.Y > 0)
				InvalidateRow(CurrentCellAddress.Y);

			if (CurrentCellAddress.Y == 0 && CurrentCellAddress.X > 0)
				InvalidateColumn(CurrentCellAddress.X);

			m_prevCurrCellAddress = CurrentCellAddress;
			
			base.OnCurrentCellChanged(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the cells get formatted with the proper font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			int row = e.RowIndex;
			int col = e.ColumnIndex;

			// Items in the first row and column use the phonetic font.
			if ((row == 0 && col > 0) || (col == 0 && row > 0))
				e.CellStyle.Font = App.PhoneticFont;

			// Make cells with zero in them a different color from those
			if (col > 0 && row > 0 && col < ColumnCount && row < RowCount &&
				this[col, row].Value != null && this[col, row].Value is int)
			{
				int val = (int)this[col, row].Value;
				if (val > -1)
				{
					e.CellStyle.BackColor = (val > 0 ?
						Settings.Default.DistChartNonZeroBackColor :
						Settings.Default.DistChartZeroBackColor);
						
					e.CellStyle.ForeColor = (val > 0 ?
						Settings.Default.DistChartNonZeroForeColor :
						Settings.Default.DistChartZeroForeColor);
				}
			}

			if (row > 0 && col > 0)
				this[col, row].ReadOnly = true;

			base.OnCellFormatting(e);
		}

		#endregion

		#region Methods related to dragging and dropping
		/// ------------------------------------------------------------------------------------
		protected override void OnDragOver(DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			DistributionChart data = e.Data.GetData(typeof(DistributionChart)) as DistributionChart;
			if (data != null)
				e.Effect = e.AllowedEffect;
			else
			{
				// Check if what's being dragged is plain text.
				string text = e.Data.GetData(typeof(string)) as string;
				if (!string.IsNullOrEmpty(text))
				{
					// Determine what cell is being dragged over. Only allow
					// text to be dropped on search item or environment cells.
					Point pt = PointToClient(new Point(e.X, e.Y));
					HitTestInfo hinfo = HitTest(pt.X, pt.Y);
					if ((hinfo.RowIndex == 0 || hinfo.ColumnIndex == 0) &&
						(hinfo.RowIndex + hinfo.ColumnIndex) > 0)
					{
						e.Effect = e.AllowedEffect;
					}
				}
			}
			
			base.OnDragOver(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			DistributionChart data = e.Data.GetData(typeof(DistributionChart)) as DistributionChart;
			if (data == null)
				return;

			// At this point we know a saved chart layout is being dragged over the grid.
			// In that case, force the grid to be painted so it looks like it's ready to
			// accept the dropping of that saved chart. Normally, setting the double-
			// buffered property for a grid is not a good thing due to some issues related
			// to painting while resizing columns. However, if that's not done here,
			// the painting of the drop effect is way too slow. Therefore turn it on, force
			// the painting, then turn it off.
			m_paintDropValidEffect = true;
			DoubleBuffered = true;
			Invalidate();
			Utils.UpdateWindow(Handle);
			DoubleBuffered = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove the visual drop effect if it has been turned on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);

			if (m_paintDropValidEffect)
			{
				m_paintDropValidEffect = false;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If a saved chart was dropped on the grid, then load it into the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragDrop(DragEventArgs e)
		{
			if (e.Effect != e.AllowedEffect)
				return;
			
			m_paintDropValidEffect = false;
			Invalidate();

			DistributionChart data = e.Data.GetData(typeof(DistributionChart)) as DistributionChart;
			if (data != null)
				LoadFromLayout(data);
			else
				ProcessDroppedText(e.Data.GetData(typeof(string)) as string, e.X, e.Y);

			base.OnDragDrop(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles processing dropping of text in a search item or environment cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ProcessDroppedText(string dragText, int x, int y)
		{
			if (string.IsNullOrEmpty(dragText))
				return;

			// Get the cell dropped on.
			Point pt = PointToClient(new Point(x, y));
			HitTestInfo hinfo = HitTest(pt.X, pt.Y);
			DataGridViewCell cell = this[hinfo.ColumnIndex, hinfo.RowIndex];

			// When dropping on the new row or new column, then make sure to add a new,
			// new one.
			if (cell.RowIndex == 0 && cell.ColumnIndex == Columns.Count - 1)
				AddColumn(true);
			else if (cell.ColumnIndex == 0 && cell.RowIndex == NewRowIndex)
			{
				Rows.Add();
				
				// When the cell referenced the first cell in the new row index,
				// after adding a row, its row index moves to the new, new row index.
				// This will put it back where it belongs.
				cell = this[cell.ColumnIndex, hinfo.RowIndex];
			}

			// Give the grid focus and insert the text in the specified cell.
			Focus();
			InsertTextInCell(cell, dragText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only do painting here when the user is dragging a saved chart layout over the
		/// grid. In that case, the entire grid is painted with a hatch pattern and 
		/// semi-transparent gray indicating it's a valid drop target.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			if (m_paintDropValidEffect)
			{
				using (HatchBrush br = new HatchBrush(HatchStyle.Percent50,
					Color.FromArgb(50, Color.LightGray)))
				{
					e.Graphics.FillRectangle(br, ClientRectangle);
				}
			}
		}

		#endregion

		#region Cell painting methods
		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			bool selected = ((e.State & DataGridViewElementStates.Selected) > 0);

			if (e.ColumnIndex > 0 && e.RowIndex > 0)
				DrawResultCell(e, selected);
			else
				DrawSearchItemOrEnvironmentCell(e, selected);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws a result cell, which is a cell that's not a search item nor an environment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawResultCell(DataGridViewCellPaintingEventArgs e, bool selected)
		{
			// First assume the cell being drawn is in the row of a search item cell or column
			// of a environment cell that is the current cell.
			Color clrBack = e.CellStyle.SelectionBackColor;

			Point currCell = CurrentCellAddress;

			if ((currCell.X > 0 && currCell.Y > 0) ||
				(currCell.X != e.ColumnIndex && currCell.Y != e.RowIndex))
			{
				clrBack = (!selected ? e.CellStyle.BackColor :
					ColorHelper.CalculateColor(e.CellStyle.SelectionBackColor,
					e.CellStyle.BackColor, 60));
			}

			using (SolidBrush br = new SolidBrush(clrBack))
				e.Graphics.FillRectangle(br, e.CellBounds);

			e.Paint(e.CellBounds, DataGridViewPaintParts.Border);

			if (!(e.Value is SearchQueryException))
				e.PaintContent(e.CellBounds);
			else
			{
				Rectangle rc = new Rectangle(new Point(0, 0), m_errorInCell.Size);
				rc.X = e.CellBounds.Left + ((e.CellBounds.Width - rc.Width) / 2);
				rc.Y = e.CellBounds.Top + ((e.CellBounds.Height - rc.Height) / 2);
				e.Graphics.DrawImageUnscaledAndClipped(m_errorInCell, rc);
			}

			if (this[e.ColumnIndex, e.RowIndex].Tag is string[] ||
				this[e.ColumnIndex, e.RowIndex].Tag is char[])
				DrawInfoCornerGlyph(e.Graphics, e.CellBounds);

			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		private static void DrawInfoCornerGlyph(Graphics g, Rectangle rc)
		{
			Point pt1 = new Point(rc.Right - 7, rc.Y);
			Point pt2 = new Point(rc.Right - 1, rc.Y + 6);
			Point ptCorner = new Point(rc.Right - 1, rc.Top);

			using (LinearGradientBrush br =
				new LinearGradientBrush(pt1, pt2, Color.Red, Color.DarkRed))
			{
				g.FillPolygon(br, new[] { pt1, pt2, ptCorner });
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws a search item or environment cell (i.e. any cell in the first row or first
		/// column).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawSearchItemOrEnvironmentCell(DataGridViewCellPaintingEventArgs e,
			bool selected)
		{
			Rectangle rc = e.CellBounds;
			Color clrBack = (!selected ? SystemColors.Control :
							ColorHelper.CalculateColor(e.CellStyle.SelectionBackColor,
							ColorHelper.LightLightHighlight, 60));

			using (SolidBrush br = new SolidBrush(clrBack))
			using (Pen pen = new Pen(ColorHelper.LightHighlight))
			{
				e.Graphics.FillRectangle(br, rc);
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Top, rc.Right - 1, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.Left, rc.Bottom - 1, rc.Right - 1, rc.Bottom - 1);
			}

			if (e.ColumnIndex != 0 || e.RowIndex != 0)
				e.PaintContent(e.CellBounds);
			//else if (IsDirty)
			//{
			//    // Draw the "chart is dirty" indicator in the top, left cell.
			//    Rectangle rc2 = new Rectangle(new Point(0, 0), m_dirtyIndicator.Size);
			//    rc2.X = e.CellBounds.Left + ((rc.Width - rc2.Width) / 2);
			//    rc2.Y = e.CellBounds.Top + ((rc.Height - rc2.Height) / 2);
			//    e.Graphics.DrawImageUnscaledAndClipped(m_dirtyIndicator, rc2);
			//}

			e.Handled = true;
		}

		#endregion

		#region Methods for clearing and resetting the grid
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the results for the specified row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ClearRow(int row)
		{
			if (row < 1 || row >= Rows.Count)
				return;

			for (int i = 1; i < Columns.Count; i++)
				this[i, row].Value = this[i, row].Tag = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the results for the specified row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ClearColumn(int col)
		{
			if (col < 1 || col >= Columns.Count)
				return;

			for (int i = 1; i < Rows.Count; i++)
				this[col, i].Value = this[col, i].Tag = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resets the grid so it contains no search items, environments or results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			if (m_lblName != null)
				m_lblName.Text = App.GetString("DistributionGrid.EmptyName", "(none)", "Views");

			Rows.Clear();
			Columns.Clear();
			ChartLayout = null;

			DataGridViewColumn col = CreateTextBoxColumn("srchitem");
			col.Frozen = true;
			col.Width = 45;
			Columns.Add(col);
			AddColumn(false);
			Rows.Add(1);
			this[0, 0].ReadOnly = true;
			Rows[0].Frozen = true;
			CurrentCell = this[0, 1];

			IsDirty = false;
			LoadedLayout = false;
			App.MsgMediator.SendMessage("DistributionChartReset", this);
		}

		#endregion

		#region Method for filling the grid with results from searching
		/// ------------------------------------------------------------------------------------
		public void FillChart()
		{
			// Force any changes that may be pending.
			if (IsCurrentCellInEditMode)
				EndEdit();

			if (RowCount <= 1 || ColumnCount <= 1)
				return;

			if (App.MsgMediator.SendMessage("BeforeDistributionChartFilled", this))
				return;

			int progBarMax = (RowCount - 2) * (ColumnCount - 2);
			App.InitializeProgressBar(App.kstidQuerySearchingMsg, progBarMax);
			FixEnvironments();

			foreach (var row in GetRows())
			{
				if (row.Index == 0 || row.Index == NewRowIndex)
					continue;

				for (int i = 1; i < Columns.Count; i++)
				{
					App.IncProgressBar();
					GetResultsForCell(row.Cells[i],
						row.Cells[0].Value as string, Columns[i].Tag as SearchQuery);
				}
			}

			IsDirty = false;
			App.UninitializeProgressBar();
			Cursor = Cursors.Default;

			App.MsgMediator.SendMessage("DistributionChartFilled", this);
		}

		/// ------------------------------------------------------------------------------------
		public static string PopupSyntaxErrorsMsg
		{
			get
			{
				return App.GetString("ChartPopupInfoSyntaxErrorsMsg",
					"This search pattern contains the following error(s):",
					"Views.Distribution Charts");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string PopupUndefinedSymbolsMsg
		{
			get
			{
				return App.GetString("ChartPopupInfoUndefinedSymbolsMsg",
					"This search pattern contains the following undefined phonetic symbol(s).",
					"Views.Distribution Charts");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string PopupInvalidPhonesMsg
		{
			get
			{
				return App.GetString("ChartPopupInfoInvalidPhonesMsg",
					"This search pattern contains the following phone(s) not found in the data.",
					"Views.Distribution Charts");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the environment cells and cleans them up a bit if the user didn't
		/// enter them completely. Fixes PA-712
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FixEnvironments()
		{
			if (App.MsgMediator.SendMessage("DistributionChartBeginEnvironmentFixes", this))
				return;

			if (RowCount > 0 && NewRowIndex > 0)
			{
				for (int i = 1; i < Columns.Count; i++)
				{
					string value = this[i, 0].Value as string;
					if (value == null)
						continue;
					
					value = value.Trim();

					// Remove any slashes.
					value = value.Replace("/", string.Empty);

					if (value == "*" || value == "_" || value == string.Empty)
						value = "*_*";
					else if (value == "+")
						value = "+_+";
					else if (value.EndsWith("_"))
						value += "*";
					else if (value.IndexOf('_') < 0)
						value += "_*";

					this[i, 0].Value = value;

					// Update the column's query.
					SearchQuery query = Columns[i].Tag as SearchQuery;
					if (query != null)
						query.Pattern = this[i, 0].Value as string;
				}
			}

			UpdateLayout();
			App.MsgMediator.SendMessage("DistributionChartAfterEnvironmentFixes", this);
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshCellValue(int row, int col)
		{
			if (IsDirty && row >= 0 && row < RowCount && col >= 0 && col < ColumnCount)
			{
				GetResultsForCell(Rows[row].Cells[col],
					Rows[row].Cells[0].Value as string, Columns[col].Tag as SearchQuery);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void GetResultsForCell(DataGridViewCell cell, string srchItem,
		    SearchQuery qryEnvironment)
		{
			SearchQuery query = null;

			try
			{
				cell.Value = cell.Tag = null;

				// If there is an environment and a search item, then get search results.
				if (!string.IsNullOrEmpty(qryEnvironment.Pattern) && !string.IsNullOrEmpty(srchItem))
				{
					int count;
					query = qryEnvironment.Clone();
					query.Pattern = srchItem + "/" + qryEnvironment.Pattern;
					App.Search(query, false, true, false, 0, out count);

					if (count < 0)
						cell.Value = new SearchQueryException(query);
					else
					{
						cell.Value = count;
						cell.Tag = (query.GetPhonesNotInCache() ?? query.GetSymbolsNotInInventory());
					}
				}
			}
			catch (Exception e)
			{
				cell.Value = new SearchQueryException(e, query);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			m_defaultRowHeight = App.PhoneticFont.Height + 10;
			RowTemplate.MinimumHeight = m_defaultRowHeight;
			RowTemplate.Height = m_defaultRowHeight;

			foreach (DataGridViewRow row in Rows)
			{
				row.MinimumHeight = m_defaultRowHeight;
				row.Height = m_defaultRowHeight;
			}

			Invalidate();

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the grid when one or more data sources has changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			if (!IsEmpty)
			{
				//bool wasDirty = IsDirty;
				FillChart();
				//IsDirty = wasDirty;
			}

			return false;
		}

		#endregion

		#region Methods that insert text in cells
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified text in the current cell when the cell is a valid cell
		/// in which to insert text. The cell is also put in the edit mode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InsertTextInCell(string text)
		{
			InsertTextInCell(CurrentCell, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified text in the specified cell when the cell is a valid cell
		/// in which to insert text.  The cell is also put in the edit mode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InsertTextInCell(DataGridViewCell cell, string text)
		{
			if (cell == null || string.IsNullOrEmpty(text) ||
			    (cell.RowIndex > 0 && cell.ColumnIndex > 0) ||
			    (cell.RowIndex == 0 && cell.ColumnIndex == 0))
				return;

			if (!cell.IsInEditMode)
			{
				CurrentCell = cell;
				BeginEdit(false);
			}

			// By this time, we know the cell is in the edit mode so get its edit control.
			DataGridViewTextBoxEditingControl txtBox =
				EditingControl as DataGridViewTextBoxEditingControl;

			if (txtBox != null)
			{
				txtBox.Focus();
				PatternTextBox.Insert(txtBox, text);
			}
		}

		#endregion

		#region Message handlers for Inserting
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears out the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnClearChart(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			Reset();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update handler for the option to clear the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateClearChart(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !OwningView.ActiveView)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = CanClear;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertIntoChart(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !OwningView.ActiveView)
				return false;

			return (itemProps.Name.StartsWith("cmnu") || UpdateInsertItem(itemProps));
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertConsonant(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("[C]");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertConsonant(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertVowel(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("[V]");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertVowel(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertZeroOrMore(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("*");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertZeroOrMore(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertOneOrMore(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("+");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertOneOrMore(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertWordBoundary(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("#");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertWordBoundary(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertDiacriticPlaceholder(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell(App.DiacriticPlaceholder);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertDiacriticPlaceholder(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertSyllableBoundary(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell(".");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertSyllableBoundary(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertANDGroup(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("[]");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertANDGroup(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInsertORGroup(object args)
		{
			if (!OwningView.ActiveView)
				return false;

			InsertTextInCell("{}");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertORGroup(object args)
		{
			return UpdateInsertItem(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		private bool UpdateInsertItem(TMItemProperties itemProps)
		{
			if (itemProps == null || !OwningView.ActiveView)
				return false;

			var pt = CurrentCellAddress;
			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Enabled = ((pt.X == 0 || pt.Y == 0) && !m_mouseDownOnCornerCell);

			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSearchOptions(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !OwningView.ActiveView)
				return false;

			var pt = CurrentCellAddress;
			itemProps.Visible = true;
			itemProps.Enabled = (GetColumnsSearchQuery(pt.X) != null && pt.X < Columns.Count - 1);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownSearchOptions(object args)
		{
			var itemProps = args as ToolBarPopupInfo;
			var query = GetColumnsSearchQuery(CurrentCellAddress.X);
			if (query == null || itemProps == null || !OwningView.ActiveView)
				return false;

			var pt = CurrentCellAddress;
			m_searchOptionsDropDown.Enabled =
				(GetColumnsSearchQuery(pt.X) != null && pt.X < Columns.Count - 1);

			m_searchOptionsDropDown.SearchQuery = query;
			itemProps.Control = m_searchOptionsDropDown;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will apply the search options just edited to all the search environments
		/// in the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ApplyToAllLinkLabel_Click(object sender, EventArgs e)
		{
			if (TMAdapter != null)
				TMAdapter.HideBarItemsPopup("cmnuXYChart");

			for (int i = 1; i < Columns.Count - 1; i++)
			{
				var query = m_searchOptionsDropDown.SearchQuery.Clone();
				
				// Get the old query and keep it's pattern.
				var oldQuery = GetColumnsSearchQuery(i);
				if (oldQuery != null)
					query.Pattern = oldQuery.Pattern;
			
				Columns[i].Tag = query;
			}

			UpdateLayout();
			m_searchOptionsDropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		static void SearchDropDownHelpLink_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidSearchOptionsXYChartsView");
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedSearchOptions(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || (itemProps.ParentControl != OwningView))
				return false;

			// First, check if any changes were made.
			if (!m_searchOptionsDropDown.Enabled || !m_searchOptionsDropDown.OptionsChanged)
				return true;

			var pt = CurrentCellAddress;
			if (pt.X > 0 && pt.X < Columns.Count)
				Columns[pt.X].Tag = m_searchOptionsDropDown.SearchQuery.Clone();

			UpdateLayout();
			return true;
		}
		
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
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}
