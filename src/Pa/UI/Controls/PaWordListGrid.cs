using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Localization;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;
using SIL.Pa.UI.Dialogs;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public interface IWaterMarkHost
	{
		bool AreResultsStale { get; set; }
	}

	/// ----------------------------------------------------------------------------------------
	public class PaWordListGrid : DataGridView, IxCoreColleague, IWaterMarkHost
	{
		private const int kPopupSidePadding = 30;

		private static bool s_showTopRightHotState;
		private static bool s_showBottomRightHotState;
		private WordListCache _cache;
		private WordListCache _backupCache;
		private SortOptions _sortOptions;
		private string _phoneticColName;
		private string _audioFileColName;
		private WordCacheEntry _currPaintingCellEntry;
		private bool _currPaintingCellSelected;
		private int _defaultRowHeight;
		private Dictionary<int, int> _customRowHeights;
		private bool _paintWaterMark;
		private bool _playbackInProgress;
		private bool _playbackAborted;
		private int _currPlaybackRow = -2;
		private AudioPlayer _audioPlayer;
		private int _playbackSpeed;
		private PaField _groupByField;
		private Label _noCIEResultsMsg;
		private ToolTip _audioFilePathToolTip;

		private bool _toggleGroupExpansion;

		private LocalWindowsHook _kbHook;
		private readonly bool _drawFocusRectAroundCurrCell;
		private readonly Keys _stopPlaybackKey = Keys.None;
		private readonly GridCellInfoPopup _cellInfoPopup;
		private readonly Bitmap _spkrImage;
		private readonly int _widthOfWrdBoundarySrchRsltMatch;

		private Color _uncertainPhoneForeColor;
		private Color _searchItemBackColor;
		private Color _searchItemForeColor;
		
		//private Color m_selectedFocusedRowBackColor;
		//private Color m_selectedFocusedRowForeColor;
		
		//private Color m_selectedCellBackColor;
		//private Color m_selectedCellForeColor;
		
		//private Color m_selectedUnFocusedRowBackColor;
		//private Color m_selectedUnFocusedRowForeColor;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView using the specified query source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid(WordListCache cache) : this(cache, null)
		{
			base.Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView using the specified cache as a data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid(WordListCache cache, Type owningViewType) :
			this(cache, owningViewType, true)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView using the specified cache as a data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid(WordListCache cache, Type owningViewType, bool performInitialSort)
			: this()
		{
			Cache = cache;
			OwningViewType = owningViewType;
			_spkrImage = Properties.Resources.kimidSpeaker;
			OnSortingOptionsChanged(performInitialSort);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView used for find phone results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid()
		{
			AllGroupsExpanded = true;
			base.DoubleBuffered = true;
			ReadOnly = true;
			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AllowUserToOrderColumns = true;
			AutoGenerateColumns = false;
			ShowCellToolTips = false;
			base.Dock = DockStyle.Fill;
			base.Font = FontHelper.UIFont;
			BorderStyle = BorderStyle.Fixed3D;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			VirtualMode = true;
			RowHeadersWidth = 25;
			RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			OnSystemColorsChanged(null);
			BuildColumns();
			OnWordListOptionsChanged(null);
			App.AddMediatorColleague(this);

			// Create the popup for the uncertain phone possibilities
			// list and the experimental transcriptions list.
			_cellInfoPopup = new GridCellInfoPopup();
			_cellInfoPopup.AssociatedGrid = this;
			_cellInfoPopup.HeadingPanel.Font = FontHelper.MakeFont(App.PhoneticFont, FontStyle.Bold);
			_cellInfoPopup.Paint += m_cellInfoPopup_Paint;
			_cellInfoPopup.CommandLink.Click += PopupsCommandLink_Click;

			_drawFocusRectAroundCurrCell = Settings.Default.WordListDrawFocusRectangle;
			_widthOfWrdBoundarySrchRsltMatch = Settings.Default.WordListSrchResultWidthOnWordBoundaryMatch;

			if (App.TMAdapter != null)
			{
				App.TMAdapter.SetContextMenuForControl(this, "cmnuWordListGrid");
				var itemProps = App.TMAdapter.GetItemProperties("mnuStopPlayback");
				if (itemProps != null)
					_stopPlaybackKey = itemProps.ShortcutKey;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the columns to the grid based on the collection of PA fields in the current
		/// project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void BuildColumns()
		{
			foreach (var field in App.Project.GetMappedFields().Where(f => f.DisplayIndexInGrid >= 0))
				AddNewColumn(field);

			RefreshColumnFonts(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column to the grid using the information from the specified PaFieldInfo
		/// object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual DataGridViewColumn AddNewColumn(PaField field)
		{
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(field.Name);
			col.HeaderText = field.DisplayName;
			col.DataPropertyName = field.Name;
			col.DefaultCellStyle.Font = field.Font;

			// Allow right to left display for any field but phonetic.
			if (field.RightToLeft && field.Type != FieldType.Phonetic)
			{
				col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			col.SortMode = (field.Type == FieldType.Phonetic ?
				DataGridViewColumnSortMode.Programmatic :
				DataGridViewColumnSortMode.Automatic);

			int i = field.DisplayIndexInGrid;
			if (i < ColumnCount)
			{
				while (i < ColumnCount && Columns[i].Frozen)
					i++;
			}

			col.DisplayIndex = i;
			col.Visible = field.VisibleInGrid;
			if (field.WidthInGrid > -1)
				col.Width = field.WidthInGrid;

			Columns.Add(col);

			// Save this because we'll need it later.
			if (field.Type == FieldType.Phonetic)
				_phoneticColName = field.Name;
			else if (field.Type == FieldType.AudioFilePath)
				_audioFileColName = field.Name;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnStringsLocalized(object args)
		{
			OnUserInterfaceLangaugeChanged(null);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUserInterfaceLangaugeChanged(object args)
		{
			foreach (DataGridViewColumn col in Columns)
			{
				var field = App.Project.GetFieldForName(col.Name);
				if (field != null)
					col.HeaderText = field.DisplayName;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			ForeColor = SystemColors.WindowText;
			BackgroundColor = SystemColors.Window;
			GridColor = App.GridColor;

			_uncertainPhoneForeColor = Settings.Default.UncertainPhoneForeColor;
			_searchItemBackColor = Settings.Default.QuerySearchItemBackColor;
			_searchItemForeColor = Settings.Default.QuerySearchItemForeColor;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				try
				{
					App.Project.GridLayoutInfo.Save(this);
				}
				catch { }

				if (GroupHeadingFont != null)
					GroupHeadingFont.Dispose();

				if (base.ContextMenuStrip != null)
					base.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

				if (_cellInfoPopup != null && !_cellInfoPopup.IsDisposed)
				{
					_cellInfoPopup.Paint -= m_cellInfoPopup_Paint;
					_cellInfoPopup.CommandLink.Click -= PopupsCommandLink_Click;
					_cellInfoPopup.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Loading and saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load settings for this grid from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadSettings()
		{
			if (App.Project.GridLayoutInfo.ColHeaderHeight >= 10)
				ColumnHeadersHeight = App.Project.GridLayoutInfo.ColHeaderHeight;
			else
			{
				AutoResizeColumnHeadersHeight();
				ColumnHeadersHeight += 6;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save grid query to query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current cache record in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry GetRecord()
		{
			return (CurrentRow == null || _cache == null || _cache.IsEmpty ?
				null : GetRecord(CurrentRow.Index));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache record for the specified grid row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry GetRecord(int rowIndex)
		{
			WordListCacheEntry entry = GetWordEntry(rowIndex);
			return (entry == null ? null : entry.WordCacheEntry.RecordEntry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current cache word entry in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCacheEntry GetWordEntry()
		{
			return (CurrentRow == null ? null : GetWordEntry(CurrentRow.Index));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache word entry for the specified index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCacheEntry GetWordEntry(int rowIndex)
		{
			if (_cache != null && rowIndex >= 0 && rowIndex < RowCount)
			{
				var row = Rows[rowIndex] as PaCacheGridRow;
				if (row != null)
				{
					int i = (row.ParentRow == null ? row.CacheEntryIndex :
						row.ParentRow.GetCacheIndex(rowIndex));
					
					if (i >= 0 && i < _cache.Count)
						return _cache[i];
				}
			}

			return null;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the cache entries that can play audio files, represented by
		/// selected grid rows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedList<int, WordCacheEntry> GetAudioEntriesInSelectedRows()
		{
			var entries = new SortedList<int, WordCacheEntry>();

			// Go through the selected rows collection.
			if (SelectedRows.Count > 0)
			{
				foreach (var row in SelectedRows.Cast<DataGridViewRow>()
					.Where(row => CanRowPlayAudio(row.Index)))
				{
					entries[row.Index] = GetWordEntry(row.Index).WordCacheEntry;
				}
			}

			// Now add the current row if it's not already in the collection.
			int currRow = CurrentCellAddress.Y;
			if (CanRowPlayAudio(currRow) &&	!entries.ContainsKey(currRow))
				entries[currRow] = GetWordEntry(currRow).WordCacheEntry;

			return (entries.Count == 0 ? null : entries);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the row specified by rowIndex has data to playback an
		/// audio file (or a portion thereof).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CanRowPlayAudio(int rowIndex)
		{
			var wlentry = GetWordEntry(rowIndex);
			if (wlentry == null)
				return false;

			var audioFilePath = wlentry.WordCacheEntry.GetAudioFileUsingFallBackIfNecessary();
			return (audioFilePath != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a list of menu items to be put on a menu drop-down. The items are added to
		/// the specified parentItem's drop down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void BuildGroupByMenu(string parentItem, ITMAdapter tmAdapter)
		{
			// Clear all the previous items in case the grid's columns have changed.
			tmAdapter.RemoveMenuSubItems(parentItem);

			// Add the "None" item first.
			var itemProps = new TMItemProperties();
			itemProps.CommandId = "CmdGroupByField";
			itemProps.Text = LocalizationManager.GetString("Views.WordLists.GroupByNoneFieldName", "None",
				"The item in the group by fields menu (and toolbar menu) indicating no field is being grouped on.");
			
			itemProps.Name = null;
			itemProps.Checked = !IsGroupedByField;
			tmAdapter.AddMenuItem(itemProps, parentItem, null);

			var sortedCols = new SortedList<int, DataGridViewColumn>();

			// Sort the items in column display order.
			foreach (DataGridViewColumn col in Columns)
			{
				if (col.Visible && !string.IsNullOrEmpty(col.HeaderText))
					sortedCols[col.DisplayIndex] = col;
			}

			// Add them to the specified parent menu's drop-down list.
			foreach (DataGridViewColumn col in sortedCols.Values)
			{
				itemProps = new TMItemProperties();
				itemProps.CommandId = "CmdGroupByField";
				itemProps.Text = col.HeaderText;
				itemProps.Name = col.Name;
				itemProps.Checked = (GroupByField != null && GroupByField.Name == col.Name);
				tmAdapter.AddMenuItem(itemProps, parentItem, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnGroupByField(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !Focused ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			GroupByField = App.Project.GetFieldForName(itemProps.Name);
			return true;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the column that's marked as the phonetic column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewColumn PhoneticColumn
		{
			get { return (Columns.Contains(_phoneticColName) ? Columns[_phoneticColName] : null); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets assigned to a new font when the grid is grouped by a field or minimal
		/// pairs. When the grid is ungrouped, then it gets disposed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Font GroupHeadingFont { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the AllGroupsCollapsed for the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllGroupsCollapsed { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the AllGroupsCollapsed for the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllGroupsExpanded { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of groups in a grouped word list. If the word list is not grouped,
		/// then zero is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GroupCount
		{
			get { return Rows.Cast<DataGridViewRow>().Count(x => x is SilHierarchicalGridRow); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the cache associated with the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache Cache
		{
			get { return _cache; }
			set
			{
				_cache = value;
				RefreshRows(false);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the adapter for the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the type of the view that owns the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Type OwningViewType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the grid's border style.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new DataGridViewCellBorderStyle CellBorderStyle
		{
			get { return base.CellBorderStyle; }
			set
			{
				base.CellBorderStyle = value;
				TMItemProperties itemProps;

				switch (value)
				{
					case DataGridViewCellBorderStyle.None:
						itemProps = App.TMAdapter.GetItemProperties("tbbGridLinesNone");
						break;
					case DataGridViewCellBorderStyle.SingleHorizontal:
						itemProps = App.TMAdapter.GetItemProperties("tbbGridLinesHorizontal");
						break;
					case DataGridViewCellBorderStyle.SingleVertical:
						itemProps = App.TMAdapter.GetItemProperties("tbbGridLinesVertical");
						break;
					default:
						itemProps = App.TMAdapter.GetItemProperties("tbbGridLinesBoth");
						break;
				}

				if (itemProps != null)
				{
					Image img = itemProps.Image;
					itemProps = App.TMAdapter.GetItemProperties("tbbShowGridLines");
					if (itemProps != null)
					{
						itemProps.Image = img;
						itemProps.Update = true;
						App.TMAdapter.SetItemProperties("tbbShowGridLines", itemProps);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the PhoneticSortOptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual SortOptions SortOptions
		{
			get { return _sortOptions; }
			set
			{
				if (value == null)
					return;

				_sortOptions = value;

				// Turn off the glyph on all the columns.
				foreach (DataGridViewColumn col in Columns)
					col.HeaderCell.SortGlyphDirection = SortOrder.None;

				// Add the sortGlyph direction
				if (_sortOptions.SortFields.Count > 0)
				{
					// Toss out sort fields that don't have columns. This should never happen,
					// but it did once after a migration to a new version. A sort field in an
					// old project must have been renamed during migration.
					for (int i = _sortOptions.SortFields.Count - 1; i >= 0; i--)
					{
						if (Columns[_sortOptions.SortFields[i].Field.Name] == null)
							_sortOptions.SortFields.RemoveAt(i);
					}

					if (_sortOptions.SortFields.Count > 0)
					{
						string colName = _sortOptions.SortFields[0].Field.Name;

						Columns[colName].HeaderCell.SortGlyphDirection =
							(_sortOptions.SortFields[0].Ascending ?
							SortOrder.Ascending : SortOrder.Descending);

						if (_groupByField != null)
							_groupByField = _sortOptions.SortFields[0].Field;
					}
				}

				_cache.Sort(_sortOptions);
				WordListGroupingBuilder.Group(this);

				// Make the grid update its display
				Invalidate();
				App.MsgMediator.SendMessage("WordListGridSorted", this);
			}
		}

		/// ------------------------------------------------------------------------------------
		public DataGridViewColumn GroupByColumn
		{
			get { return (_groupByField == null ? null : Columns[_groupByField.Name]); }
		}
		
		/// ------------------------------------------------------------------------------------
		public virtual PaField GroupByField
		{
			get	{return _groupByField;}
			set
			{
				if (_groupByField == value)
					return;

				if (value != null)
				{
					_groupByField = value;
					WordListGroupingBuilder.Group(this);
				}
				else
				{
					// Make sure all groups are expanded before ungrouping. This will prevent
					// rows in collapsed groups from remaining invisible after the ungrouping
					// process.
					ToggleGroupExpansion(true);
					_groupByField = null;
					WordListGroupingBuilder.UnGroup(this);
					return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the word list grid is grouped by a field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsGroupedByField
		{
			get { return _groupByField != null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The minimal pairs options for this grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the grid should be considered
		/// the one whose entries will be played back (or stopped) when the user clicks
		/// on a playback button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCurrentPlaybackGrid { get; set; }

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Right)
				Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseDoubleClick(e);

			if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && Settings.Default.WordListEditSourceOnDoublClick)
				OnEditSourceRecord(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// In order to achieve double buffering without the problem that arises from having
		/// double buffering on while sizing rows and columns or dragging columns around,
		/// monitor when the mouse goes down and turn off double buffering when it goes down
		/// on a column heading or over the dividers between rows or the dividers between
		/// columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex == -1 || (e.ColumnIndex == -1 && Cursor == Cursors.SizeNS) ||
				(e.RowIndex == 0 && Cursor == Cursors.SizeNS))
			{
				DoubleBuffered = false;
			}

			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Button == MouseButtons.Right)
				CurrentCell = this[e.ColumnIndex, e.RowIndex];

			base.OnCellMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When double buffering is off, it means it was turned off in the cell mouse down
		/// event. Therefore, turn it back on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (!DoubleBuffered)
				DoubleBuffered = true;

			base.OnCellMouseUp(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar accordingly. I use this override instead of OnLeave because
		/// when using OnLeave, I found some edge cases when the current row wasn't getting
		/// invalidated properly. Overriding this event fixed it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLostFocus(EventArgs e)
		{
			App.StatusBarLabel.Text = string.Empty;
			base.OnLostFocus(e);
			if (_audioFilePathToolTip != null)
			{
				_audioFilePathToolTip.Dispose();
				_audioFilePathToolTip = null;
			}

			if (CurrentRow != null)
				InvalidateRow(CurrentRow.Index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar accordingly. I use this override instead of OnEnter because
		/// when using OnEnter, I found some edge cases when the current row wasn't getting
		/// invalidated properly. Overriding this event fixed it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnGotFocus(EventArgs e)
		{
			SetStatusBarText();

			if (CurrentRow != null)
				InvalidateRow(CurrentRow.Index);

			base.OnGotFocus(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowEnter(DataGridViewCellEventArgs e)
		{
			SetStatusBarText(e.RowIndex);
			base.OnRowEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText()
		{
			SetStatusBarText(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText(ToolStripStatusLabel sblbl)
		{
			SetStatusBarText(CurrentCellAddress.Y, sblbl);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText(int rowIndex)
		{
			SetStatusBarText(rowIndex, App.StatusBarLabel);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText(int rowIndex, ToolStripStatusLabel sblbl)
		{
			if (sblbl == null)
				return;

			if (rowIndex < 0 || rowIndex >= RowCount)
				sblbl.Text = string.Empty;
			else
			{
				var row = Rows[rowIndex] as PaCacheGridRow;
				sblbl.Text = (row == null ? string.Empty :
					string.Format(LocalizationManager.GetString("Views.WordLists.StatusBarText", "Record {0} of {1}"),
					row.CacheEntryIndex + 1, _cache.Count));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the height of the row when it's requested.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowHeightInfoNeeded(DataGridViewRowHeightInfoNeededEventArgs e)
		{
			int rowHeight;

			e.Height = (_customRowHeights != null &&
				_customRowHeights.TryGetValue(e.RowIndex, out rowHeight) ?	rowHeight : _defaultRowHeight);

			e.MinimumHeight = 10;
			base.OnRowHeightInfoNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the custom height just set for the specified row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowHeightInfoPushed(DataGridViewRowHeightInfoPushedEventArgs e)
		{
			if (_customRowHeights == null)
				_customRowHeights = new Dictionary<int, int>();

			_customRowHeights[e.RowIndex] = e.Height;
			base.OnRowHeightInfoPushed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			base.OnCellValueNeeded(e);

			try
			{
				var entry = GetWordEntry(e.RowIndex);

				if (entry == null)
				{
					e.Value = null;
					return;
				}
				
				var fieldName = Columns[e.ColumnIndex].DataPropertyName;

				// When the entry is from a data source that was parsed using the one-to-one
				// option and the field is a parsed field, then handle that case specially.
				// In that case, we don't want to display the record entry's value when the
				// word cache entry's value is null. We just want to display nothing. PA-709
				if (entry.WordCacheEntry.RecordEntry.DataSource.ParseType ==
					DataSourceParseType.OneToOne)
				{
					var field = entry.WordCacheEntry.RecordEntry.DataSource.FieldMappings
						.SingleOrDefault(m => m.Field.Name == fieldName);

					if (field != null && field.IsParsed)
					{
						e.Value = entry.WordCacheEntry.GetField(fieldName, false);
						return;
					}
				}

				e.Value = entry[fieldName];
			}
			catch
			{
				e.Value = LocalizationManager.GetString("Views.WordLists.CellValueErrorMsg", "Error getting data!",
					"Displayed in a word list cell when there was an error retrieving data for the cell. This should never be used, but just in case...");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Play nice and clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			App.RemoveMediatorColleague(this);
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the cells get formatted with the proper font and background color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			e.CellStyle.Font = Columns[e.ColumnIndex].DefaultCellStyle.Font;
			App.SetCellColors(this, e);
			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the grid is assigned a context menu, then make sure to subscribe to its
		/// opening event so we can override showing the context menu when the user clicks
		/// on the phonetic column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override ContextMenuStrip ContextMenuStrip
		{
			get { return base.ContextMenuStrip; }
			set
			{
				if (base.ContextMenuStrip != null)
					base.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

				base.ContextMenuStrip = value;

				if (base.ContextMenuStrip != null)
					base.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the context menu is popping up when the mouse is over the phonetic column
		/// heading, then cancel popping up the context menu so it doesn't get in the way of
		/// the phonetic sort options popup displaying.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			Point pt = PointToClient(MousePosition);
			Rectangle rc = GetCellDisplayRectangle(Columns[_phoneticColName].Index, -1, false);

			// Check if the mouse location is over the phonetic column heading. Normally, we
			// will be in this code when the user right-clicks somewhere on the grid. However,
			// we could also be here because he pressed the context menu button on the
			// keyboard. In that case, it's a little strange showing the phonetic sort options
			// just because the mouse happens to be resting over the phonetic column heading.
			// But, oh well. Stranger things happen in the world.
			if (rc.Contains(pt))
			{
				e.Cancel = true;
				ShowPhoneticSortOptionsPopup();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort the cache based on the column clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (_cache != null)
			{
				string colName = Columns[e.ColumnIndex].Name;

				if (e.Button == MouseButtons.Left && Columns[colName].SortMode != DataGridViewColumnSortMode.NotSortable)
					Sort(colName, true); // Sort using the SortOptions and the column clicked
				else if (colName == _phoneticColName)
					ShowPhoneticSortOptionsPopup();
			}

			base.OnColumnHeaderMouseClick(e);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void ShowPhoneticSortOptionsPopup()
		{
			if (!Focused)
			{
				Focus();
				Application.DoEvents();
			}

			Rectangle rc = GetCellDisplayRectangle(Columns[_phoneticColName].Index, -1, false);
			Point pt = new Point(rc.X, rc.Bottom);
			pt = PointToScreen(pt);
			TMAdapter.PopupMenu("tbbPhoneticSort", pt.X, pt.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnColumnDisplayIndexChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnDisplayIndexChanged(e);

			// Force users to restart Find when moving column positions
			FindInfo.CanFindAgain = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When word list is grouped and the row divider is double-clicked, it causes a crash
		/// in the DataGridViewCell.MeasureTextSize() method because some object's font
		/// is being referenced and the font is null. Fix for PA-207
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowDividerDoubleClick(DataGridViewRowDividerDoubleClickEventArgs e)
		{
			try
			{
				base.OnRowDividerDoubleClick(e);
			}
			catch { }
		}

		#endregion

		#region Methods for handling the uncertain phones and experimental transcription popup lists.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user moves over a phonetic cell whose cache entry contains uncertain
		/// phones a list of words is popped-up to the right of of the cell. The words are
		/// all the possibilties given the specified uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (App.IsViewOrFormActive(OwningViewType, FindForm()) &&
				e.ColumnIndex >= 0 && e.RowIndex >= 0 && _cache != null)
			{
				string colName = Columns[e.ColumnIndex].Name;

				if (colName == _phoneticColName || colName == _audioFileColName)
				{
					WordListCacheEntry wlEntry = GetWordEntry(e.RowIndex);
					if (wlEntry == null)
						return;

					WordCacheEntry entry = wlEntry.WordCacheEntry;

					if (colName == _phoneticColName)
					{
						// Don't popup either of the popups in this method if the cell has
						// both indicators. In that case the popup is handled in OnCellMouseMove.
						if (entry.AppliedExperimentalTranscriptions != null &&
							entry.ContiansUncertainties)
						{
							return;
						}

						if (entry.ContiansUncertainties)
							ShowUncertainDataPopup(e.RowIndex, e.ColumnIndex);
						else if (entry.AppliedExperimentalTranscriptions != null)
							ShowExperimentTranscriptionsPopup(e.RowIndex, e.ColumnIndex);
					}
					else
					{
						string audioFilePath = entry[_audioFileColName];

						// Show a tooltip with the full audio file path if the user has
						// moved the mouse over the audio file cell of an entry from a
						// FieldWorks data source.
						if (audioFilePath != null &&
							entry.RecordEntry.DataSource.FwSourceDirectFromDB &&
							!Path.IsPathRooted(audioFilePath))
						{
							audioFilePath =	Path.Combine(FwDBUtils.FwRootDataDir, audioFilePath);
							Point pt = FindForm().PointToClient(MousePosition);
							pt.Y += (int)(Cursor.Size.Height * 1.3);
							_audioFilePathToolTip = new ToolTip();
							_audioFilePathToolTip.Show(audioFilePath, FindForm(), pt);
						}
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hide the pop-up if it's showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseLeave(e);
			ClearIndicatorHotspot(e.RowIndex, e.ColumnIndex);

			if (_audioFilePathToolTip != null)
			{
				_audioFilePathToolTip.Dispose();
				_audioFilePathToolTip = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the cell the mouse is moving over contains an entry that has both uncertain
		/// phones and experimental transcriptions, then those popups will popup only when the
		/// mouse moves over certain portions of the cell. When the mouse is over the top right
		/// corner, then the list of all uncertain possibilities is shown. When the mosue is
		/// over the bottom right corner of the cell, the list of experimental transcription
		/// conversions is shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseMove(e);

			if (!App.IsViewOrFormActive(OwningViewType, FindForm()) ||
				e.ColumnIndex < 0 || e.RowIndex < 0 || _cache == null ||
				Columns[e.ColumnIndex].Name != _phoneticColName)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = false;
				return;
			}

			WordListCacheEntry wlentry = GetWordEntry(e.RowIndex);
			if (wlentry == null)
				return;
	
			WordCacheEntry entry = wlentry.WordCacheEntry;

			// If the entry doesn't have both indicators then don't show either
			// popup in this method. It's done in the OnCellMouseEnter method.
			if (!entry.ContiansUncertainties || entry.AppliedExperimentalTranscriptions == null)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = false;
				return;
			}

			if (IsMouseOverUncertainDataPopupHotSpot(e.RowIndex, e.ColumnIndex, e.Location))
				HandleMouseOverUncertainDataPopupHotSpot(e.RowIndex, e.ColumnIndex, entry);
			else if (IsMouseOverExperimentalTransPopupHotSpot(e.RowIndex, e.ColumnIndex, e.Location))
				HandleMouseOverExperimentalTransPopupHotSpot(e.RowIndex, e.ColumnIndex, entry);
			else
			{
				ClearIndicatorHotspot(e.RowIndex, e.ColumnIndex);

				// The mouse isn't over either corner so make sure the popups aren't showing.
				if (!_cellInfoPopup.IsMouseOver)
					_cellInfoPopup.Hide();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes the mouse moving over the hotspot area that triggers displaying the
		/// uncertain phone possibility list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseOverUncertainDataPopupHotSpot(int row, int col, WordCacheEntry entry)
		{
			if (!s_showTopRightHotState || s_showBottomRightHotState)
			{
				s_showTopRightHotState = true;
				s_showBottomRightHotState = false;
				InvalidateCell(col, row);
			}

			// If the popup is active and it's for experimental transcriptions, then hide it.
			_cellInfoPopup.Hide(GridCellInfoPopup.Purpose.ExperimentalTranscription);

			// The mouse is over the top right so, if it's not already shown,
			// show the uncertainty popup.
			if (!_cellInfoPopup.Active(GridCellInfoPopup.Purpose.UncertainPossibilities) &&
				entry.ContiansUncertainties)
			{
				ShowUncertainDataPopup(row, col);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes the mouse moving over the hotspot area that triggers displaying the
		/// experimental transcriptions list .
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseOverExperimentalTransPopupHotSpot(int row, int col, WordCacheEntry entry)
		{
			if (s_showTopRightHotState || !s_showBottomRightHotState)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = true;
				InvalidateCell(col, row);
			}

			// If the popup is active and it's for uncertain items, then hide it.
			_cellInfoPopup.Hide(GridCellInfoPopup.Purpose.UncertainPossibilities);

			// The mouse is over the bottom right so, if it's not already shown,
			// show the experimental transcriptions popup.
			if (!_cellInfoPopup.Active(GridCellInfoPopup.Purpose.ExperimentalTranscription)
				&& entry.AppliedExperimentalTranscriptions != null)
			{
				ShowExperimentTranscriptionsPopup(row, col);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the mouse is over the top right corner of the cell
		/// specified by row and col.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsMouseOverUncertainDataPopupHotSpot(int row, int col, Point mouseLocation)
		{
			Rectangle rc = GetCellDisplayRectangle(col, row, false);

			// Because the mouse's location is relative to the cell (not the entire
			// grid), we need to adjust the origin of the cell's rectangle. 
			rc.X = rc.Y = 0;

			int rightEdge = rc.Right - 1;
			rc.Height /= 2;
			rc.Width = (int)(rc.Height * 1.5);
			rc.X = rightEdge - rc.Width;
			return rc.Contains(mouseLocation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the mouse is over the bottom right corner of the cell
		/// specified by row and col.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsMouseOverExperimentalTransPopupHotSpot(int row, int col, Point mouseLocation)
		{
			Rectangle rc = GetCellDisplayRectangle(col, row, false);

			// Because the mouse's location is relative to the cell (not the entire
			// grid), we need to adjust the origin of the cell's rectangle. 
			rc.X = rc.Y = 0;

			int rightEdge = rc.Right - 1;
			int bottomEdge = rc.Bottom - 1;
			rc.Height /= 2;
			rc.Width = (int)(rc.Height * 1.5);
			rc.X = rightEdge - rc.Width;
			rc.Y = bottomEdge - rc.Height;
			return rc.Contains(mouseLocation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the specified popup at using a cell's row and column coordinates to
		/// determine where the popup should be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowUncertainDataPopup(int row, int col)
		{
			s_showTopRightHotState = true;
			s_showBottomRightHotState = false;
			InvalidateCell(col, row);

			WordListCacheEntry wlentry = GetWordEntry(row);
			if (wlentry == null)
				return;

			WordCacheEntry entry = wlentry.WordCacheEntry;
			if (entry == null)
				return;

			string[][] possibleWords = entry.GetAllPossibleUncertainWords(true);
			if (possibleWords == null)
				return;
			
			int hdgWidth;
			using (Font fnt = FontHelper.MakeFont(App.PhoneticFont, FontStyle.Bold))
				hdgWidth = _cellInfoPopup.SetHeadingText(entry.PhoneticValue, fnt);

			_cellInfoPopup.PurposeIndicator = GridCellInfoPopup.Purpose.UncertainPossibilities;
			_cellInfoPopup.CacheEntry = entry;
			_cellInfoPopup.MeasureBodyHeight(App.PhoneticFont, possibleWords.Length);
			_cellInfoPopup.Width = hdgWidth + kPopupSidePadding;
			_cellInfoPopup.AssociatedCell = this[col, row];

			// Calculate a point just to the right and even with the top of
			// the cell to which the popup belongs. Then show the popup there.
			Rectangle rc = GetCellDisplayRectangle(col, row, false);
			Point pt = new Point(rc.Right - 1, rc.Y);
			_cellInfoPopup.Show(this, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the specified popup at using a cell's row and column coordinates to
		/// determine where the popup should be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowExperimentTranscriptionsPopup(int row, int col)
		{
			s_showTopRightHotState = false;
			s_showBottomRightHotState = true;
			InvalidateCell(col, row);

			var wlentry = GetWordEntry(row);
			if (wlentry == null)
				return;

			var entry = wlentry.WordCacheEntry;
			if (entry == null)
				return;

			var experimentalTrans = entry.AppliedExperimentalTranscriptions;
			if (experimentalTrans == null)
				return;

			int widestExperimentalTrans = GetWidestExperimentalTrancription(experimentalTrans);

			int hdgWidth;
			string hdgText = LocalizationManager.GetString(
				"Views.WordLists.CellInfoPopup.TranscriptionChanges.HeadingText",
				"Transcription changes\nmade in this entry:",
				"Heading text on transcription changes popup in word lists.");
			
			using (var fnt = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold))
				hdgWidth = _cellInfoPopup.SetHeadingText(hdgText, fnt);

			_cellInfoPopup.CacheEntry = entry;
			_cellInfoPopup.PurposeIndicator = GridCellInfoPopup.Purpose.ExperimentalTranscription;
			_cellInfoPopup.MeasureBodyHeight(App.PhoneticFont, experimentalTrans.Count);
			_cellInfoPopup.Width = Math.Max(hdgWidth, widestExperimentalTrans) + kPopupSidePadding;
			_cellInfoPopup.AssociatedCell = this[col, row];
			_cellInfoPopup.HeadingTextSidePadding = (kPopupSidePadding / 2 - 3);

			_cellInfoPopup.HeadingTextFomatFlags = TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.Left |
				TextFormatFlags.LeftAndRightPadding;

			// Calculate a point just to the right and even with the top of
			// the cell to which the popup belongs. Then show the popup there.
			var rc = GetCellDisplayRectangle(col, row, false);
			var pt = new Point(rc.Right - 1, rc.Y);
			_cellInfoPopup.Show(this, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Measure how big the experimental transcription list must be.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static int GetWidestExperimentalTrancription(
			Dictionary<string, string> experimentalTrans)
		{
			const TextFormatFlags kFlags = TextFormatFlags.NoPrefix |
				TextFormatFlags.LeftAndRightPadding;

			// Determine the width of each experimental transcription and the width of the
			// string it was converted to. Determine the maximum of all the widths.
			int maxWidth = 0;
			foreach (var item in experimentalTrans)
			{
				int itemWidth = TextRenderer.MeasureText(item.Key,
					App.PhoneticFont, Size.Empty, kFlags).Width;

				itemWidth += TextRenderer.MeasureText(item.Value,
					App.PhoneticFont, Size.Empty, kFlags).Width;

				maxWidth = Math.Max(itemWidth, maxWidth);
			}

			// Add some for the arrow glyph with some space on either side of it.
			return maxWidth + 15;
		}

		/// ------------------------------------------------------------------------------------
		void m_cellInfoPopup_Paint(object sender, PaintEventArgs e)
		{
			if (_cellInfoPopup.CacheEntry == null)
				return;

			if (_cellInfoPopup.PurposeIndicator == GridCellInfoPopup.Purpose.UncertainPossibilities)
				DrawUncertaintyPopupList(e.Graphics);
			else
				DrawExperimentalTransPopupList(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the list of possible words in the uncertainty popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawUncertaintyPopupList(IDeviceContext g)
		{
			string[][] possibleWords = _cellInfoPopup.CacheEntry.GetAllPossibleUncertainWords(true);
			if (possibleWords == null)
				return;

			Rectangle rc = _cellInfoPopup.FirstItemRectangle;

			const TextFormatFlags kFlags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.NoPadding;

			// Draw each word.
			foreach (string[] word in possibleWords)
			{
				rc.X = 25;

				// Draw each phone in the word
				foreach (string phone in word)
				{
					string ph = phone;
					Color clrText = Color.Black;

                    if (ph.StartsWith("|", StringComparison.Ordinal))
					{
						clrText = _uncertainPhoneForeColor;
						ph = ph.Substring(1);
					}

					TextRenderer.DrawText(g, ph, App.PhoneticFont, rc, clrText, kFlags);

					// Figure out where the next phone should be drawn.
					int phoneWidth = TextRenderer.MeasureText(g, ph,
						App.PhoneticFont, Size.Empty, kFlags).Width;

					rc.X += phoneWidth;
				}

				rc.Y += rc.Height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the list of experimental transcription conversions performed on the
		/// cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawExperimentalTransPopupList(Graphics g)
		{
			Dictionary<string, string> experimentalTrans =
				_cellInfoPopup.CacheEntry.AppliedExperimentalTranscriptions;

			if (experimentalTrans == null)
				return;

			const TextFormatFlags kFlags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.NoPadding |
				TextFormatFlags.LeftAndRightPadding;

			Rectangle rc = _cellInfoPopup.FirstItemRectangle;
			rc.X += 25;

			foreach (KeyValuePair<string, string> item in experimentalTrans)
			{
				TextRenderer.DrawText(g, item.Key, App.PhoneticFont, rc,
					Color.Black, kFlags);

				Size sz = TextRenderer.MeasureText(g, item.Key, App.PhoneticFont);

				// Draw an arrow that points to what experimental transcription the
				// phone was converted to.
				using (SolidBrush br = new SolidBrush(Color.Black))
				{
					Point pt1 = new Point(rc.X + sz.Width + 12, rc.Y + (sz.Height / 2));
					Point pt2 = new Point(pt1.X - 5, pt1.Y - 5);
					Point pt3 = new Point(pt1.X - 5, pt1.Y + 5);
					g.FillPolygon(br, new[] { pt1, pt2, pt3 });
				}

				Rectangle rcTmp = rc;
				rcTmp.X += sz.Width + 15;
				TextRenderer.DrawText(g, item.Value, App.PhoneticFont, rcTmp,
					Color.Black, kFlags);

				rc.Y += rc.Height;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ClearIndicatorHotspot(int row, int col)
		{
			if (s_showTopRightHotState || s_showBottomRightHotState)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = false;
				InvalidateCell(col, row);
			}
		}

		/// ------------------------------------------------------------------------------------
		void PopupsCommandLink_Click(object sender, EventArgs e)
		{
			if (_cellInfoPopup.PurposeIndicator == GridCellInfoPopup.Purpose.ExperimentalTranscription)
				App.MsgMediator.SendMessage("ExperimentalTranscriptions", null);
			else if (_cellInfoPopup.AssociatedCell != null)
			{
				if (!Focused)
					Focus();

				OnEditSourceRecord(_cellInfoPopup.AssociatedCell.RowIndex);
			}
		}

		#endregion

		#region Watermark handling methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the results in the grid are stale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreResultsStale
		{
			get { return _paintWaterMark; }
			set
			{
				if (_paintWaterMark != value)
				{
					_paintWaterMark = value;
					Invalidate(WaterMarkRectangle);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle in which the watermark is drawn.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle WaterMarkRectangle
		{
			get
			{
				var rc = ClientRectangle;
				rc.Width = (int)(rc.Width * 0.5f);
				rc.Height = (int)(rc.Height * 0.5f);
				rc.X = (ClientRectangle.Width - rc.Width) / 2;
				rc.Y = (ClientRectangle.Height - rc.Height) / 2;
				return rc;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid changes size.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			bool paintWaterMark = _paintWaterMark;

			if (_paintWaterMark)
			{
				// Clear previous water mark.
				_paintWaterMark = false;
				Invalidate();
			}

			base.OnSizeChanged(e);

			_paintWaterMark = paintWaterMark;
			if (_paintWaterMark)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid scrolls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			bool paintWaterMark = _paintWaterMark;

			if (_paintWaterMark)
			{
				// Clear previous water mark.
				_paintWaterMark = false;
				Invalidate();
			}

			base.OnScroll(e);

			_paintWaterMark = paintWaterMark;
			if (_paintWaterMark)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints a water mark when the results are stale (i.e. the query settings have been
		/// changed since the results were shown).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (_paintWaterMark)
				DrawStaleResultsWaterMark(e.Graphics, WaterMarkRectangle, DefaultCellStyle.ForeColor);
		}

		/// ------------------------------------------------------------------------------------
		public static void DrawStaleResultsWaterMark(Graphics g, Rectangle rc, Color clr)
		{
			var path = new GraphicsPath();
			var family = FontFamily.GenericSerif;

			// Find the first font size equal to or smaller than 256 that
			// fits in the water mark rectangle.
			for (int size = 256; size >= 0; size -= 2)
			{
				using (var fnt = FontHelper.MakeFont(family.Name, size, FontStyle.Bold))
				{
					int height = TextRenderer.MeasureText("!", fnt).Height;
					if (height < rc.Height)
					{
						using (var sf = Utils.GetStringFormat(true))
							path.AddString("!", family, (int)FontStyle.Bold, size, rc, sf);

						break;
					}
				}
			}

			path.AddEllipse(rc);

			using (var br = new SolidBrush(Color.FromArgb(35, clr)))
				g.FillRegion(br, new Region(path));
		}

		#endregion

		#region Drawing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles special painting for the phonetic column when the grid is displaying
		/// find phone results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0 || _cache == null)
			{
				if (e.ColumnIndex < 0 && e.RowIndex == _currPlaybackRow)
					DrawRowHeaderForPlayingRow(e);
				else
					base.OnCellPainting(e);

				if (e.RowIndex == -1 && e.ColumnIndex == 0)
					DrawGapFiller(e);

				return;
			}

			var wlentry = GetWordEntry(e.RowIndex);
			var field = App.Project.GetFieldForName(Columns[e.ColumnIndex].Name);

			if (wlentry == null || field == null)
			{
				base.OnCellPainting(e);
				return;
			}

			_currPaintingCellEntry = wlentry.WordCacheEntry;
			_currPaintingCellSelected =
				((e.State & DataGridViewElementStates.Selected) > 0);

			if (field.Type == FieldType.AudioFilePath || field.Type == FieldType.GeneralFilePath)
			{
				DrawFilePath(e);
				e.Handled = true;
				return;
			}

			if (Columns[e.ColumnIndex].Name == _phoneticColName)
			{
				if (_cache.IsForSearchResults)
				{
					DrawPhoneticSearchResult(e);
					e.Handled = true;
					return;
				}

				if (_currPaintingCellEntry.Phones != null)
				{
					if (_currPaintingCellEntry.ContiansUncertainties &&
						_currPaintingCellEntry.UncertainPhones != null)
					{
						DrawPhoneticUncertainty(e);
						e.Handled = true;
						return;
					}

					if (_currPaintingCellEntry.AppliedExperimentalTranscriptions != null)
					{
						PaintCell(e, true);
						DrawIndicatorCornerGlyphs(e.Graphics, e.CellBounds,
							_currPaintingCellEntry);
						
						e.Handled = true;
						return;
					}
				}
			}

			PaintCell(e, true);
			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the foreground color for the current cell being painted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Color GetCurrentCellDrawForeColor(DataGridViewCellPaintingEventArgs e)
		{
			return (_currPaintingCellSelected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints a cell using default values, except for the focus rectangle and content
		/// when paintContent is false.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaintCell(DataGridViewCellPaintingEventArgs e, bool paintContent)
		{
			Rectangle rc = e.CellBounds;

			var parts = DataGridViewPaintParts.All;
			parts &= ~DataGridViewPaintParts.Focus;

			// Draw default everything but text if paintContent is false.
			if (!paintContent)
				parts &= ~DataGridViewPaintParts.ContentForeground;

			// Determine whether or not we're painting the current cell.
			bool isCurrentCell = (_currPaintingCellSelected && Focused &&
				CurrentCellAddress.X == e.ColumnIndex);

			if (!isCurrentCell)
				e.Paint(rc, parts);
			else
			{
				Color clr = e.CellStyle.SelectionForeColor;
				e.CellStyle.SelectionForeColor = GetCurrentCellDrawForeColor(e);
				e.Paint(rc, parts);
				e.CellStyle.SelectionForeColor = clr;

				if (_drawFocusRectAroundCurrCell)
				{
					rc.Width--;
					rc.Height--;
					ControlPaint.DrawFocusRectangle(e.Graphics, rc);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method fills in the gap between the header of the last visible column and the
		/// right edge of the grid. That gap is filled with something that looks like one long
		/// empty column header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawGapFiller(DataGridViewCellPaintingEventArgs e)
		{
			int colWidths = Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
			if (colWidths > ClientSize.Width)
				return;

			Rectangle rc = e.CellBounds;
			rc.Width = ClientSize.Width;

			VisualStyleElement element = VisualStyleElement.Header.Item.Normal;
			if (PaintingHelper.CanPaintVisualStyle(element))
			{
				if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
					rc.Height--;

				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(e.Graphics, rc);
			}
			else
			{
				// Draw this when themes aren't supported.
				ControlPaint.DrawButton(e.Graphics, rc, ButtonState.Normal);
				ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Flat,
					Border3DSide.Bottom);
			}

			if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
			{
				ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Etched,
					Border3DSide.Top);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the row header cell for the row currently playing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawRowHeaderForPlayingRow(DataGridViewCellPaintingEventArgs e)
		{
			e.PaintBackground(e.CellBounds, false);

			// Figure out the size and location of the image so it's centered in the cell.
			Rectangle rc = e.CellBounds;
			rc.X += (rc.Width - _spkrImage.Width) / 2;
			rc.Y += (rc.Height - _spkrImage.Height) / 2;
			rc.Size = _spkrImage.Size;
			e.Graphics.DrawImage(_spkrImage, rc);
			e.Handled = true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints text in a cell using path with path ellipsis formatting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawFilePath(DataGridViewCellPaintingEventArgs e)
		{
			PaintCell(e, false);

			string soundFilePath = e.Value as string;
			if (string.IsNullOrEmpty(soundFilePath))
				return;

			const TextFormatFlags kFlags = TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
				TextFormatFlags.PreserveGraphicsClipping;

			TextRenderer.DrawText(e.Graphics, soundFilePath, e.CellStyle.Font, e.CellBounds,
				GetCurrentCellDrawForeColor(e), kFlags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the phonetic column when the grid is displaying find phone results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawPhoneticSearchResult(DataGridViewCellPaintingEventArgs e)
		{
			WordListCacheEntry wlentry = GetWordEntry(e.RowIndex);
			if (wlentry == null)
				return;

			Rectangle rc = e.CellBounds;
			PaintCell(e, false);

			int srchItemOffset = wlentry.SearchItemOffset;
			int srchItemLength = wlentry.SearchItemLength;
			int envAfterOffset = srchItemOffset + srchItemLength;

			// Make sure the search item offset + length doesn't exceed the number of
			// phones in the entry. This could happen with some searches (see PA-755).
			if (srchItemOffset + srchItemLength > wlentry.Phones.Length)
				srchItemLength = Math.Abs(wlentry.Phones.Length - srchItemOffset);

			string srchItem;

			try
			{
				// Get the text that makes up the search item.
				// This is used only to measure it's width.
				srchItem = string.Join(string.Empty, wlentry.Phones,
					srchItemOffset, srchItemLength);
			}
			catch
			{
				srchItem = (wlentry.SearchItem ?? string.Empty);
			}

			const TextFormatFlags kFlags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
				TextFormatFlags.PreserveGraphicsClipping;

			// Calculate the width of the search item.
			int itemWidth = TextRenderer.MeasureText(e.Graphics, srchItem,
				App.PhoneticFont, Size.Empty, kFlags).Width;

			if (itemWidth == 0)
				itemWidth = _widthOfWrdBoundarySrchRsltMatch;

			// Calculate the center of the cell less half the width of the search item.
			int itemLeft = rc.X + (rc.Width / 2) - (itemWidth / 2);

			// Draw the background color for the search item and the search item's phones.
			DrawSearchItemBackground(e.Graphics, rc, itemWidth, itemLeft);
			DrawSearchItemPhones(e.Graphics, wlentry.Phones, rc, srchItemOffset,
				envAfterOffset, kFlags, itemLeft);

			Color clrText = GetCurrentCellDrawForeColor(e);

			// Draw the phones in the environment after.
			rc.X = itemLeft + itemWidth;
			rc.Width = e.CellBounds.Right - (itemLeft + itemWidth);
			DrawPhones(e.Graphics, wlentry.Phones, envAfterOffset, wlentry.Phones.Length,
				rc, clrText, kFlags, true);

			if (itemLeft > e.CellBounds.X)
			{
				// Draw the phones in the environment before.
				rc.X = e.CellBounds.X;
				rc.Width = itemLeft - e.CellBounds.X;
				DrawPhones(e.Graphics, wlentry.Phones, srchItemOffset - 1, -1,
					rc, clrText, kFlags, false);
			}

			DrawIndicatorCornerGlyphs(e.Graphics, e.CellBounds, wlentry.WordCacheEntry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the background color for the search item's rectangle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawSearchItemBackground(Graphics g, Rectangle rc, int width, int left)
		{
			// Take into consideration when the cell is selected. In that case, the highlighted
			// background is made a little transparent so the row's selection color shows through.
			Rectangle rcBackground = new Rectangle(left, rc.Y, width, rc.Height - 1);
			
			if (rcBackground.X < rc.X)
			{
				rcBackground.X = rc.X;
				rcBackground.Width -= (rc.X - left);
				if (rcBackground.Width >= rc.Width)
					rcBackground.Width = rc.Width - 1;
			}

			Color clrBack = (_currPaintingCellSelected ?
				Color.FromArgb(90, _searchItemBackColor) :	_searchItemBackColor);

			using (SolidBrush br = new SolidBrush(clrBack))
				g.FillRectangle(br, rcBackground);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawSearchItemPhones(IDeviceContext g, string[] phones, Rectangle cellBounds,
			int begin, int end, TextFormatFlags flags, int left)
		{
			Color clrText = (_currPaintingCellSelected && !Focused ?
				 Settings.Default.GridRowUnfocusedSelectionForeColor : _searchItemForeColor);
			
			Rectangle rc = new Rectangle(left, cellBounds.Y,
				cellBounds.Right - left, cellBounds.Height);

			// I think there's a bug in .Net because the graphics clipping area seems to
			// always be the entire grid. Therefore, using PreserveGraphicsClipping in the
			// flags doesn't seem to do any good when the rectangle is outside the cell's
			// border.
			if (left >= cellBounds.X)
				DrawPhones(g, phones, begin, end, rc, clrText, flags, true);
			else
			{
				// At this point, we know the cell isn't big enough for everything
				// and the search item's left edge is beyond the left edge of the
				// cell. So draw an ellipsis.
				rc.X = cellBounds.X;
				rc.Width -= (cellBounds.X - left);
				if (rc.Width >= cellBounds.Width)
					rc.Width = cellBounds.Width - 1;

				DrawPhones(g, new[] { "..." }, 0, 1, rc, clrText, flags, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the phonetic cell when the entry contains uncertain query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawPhoneticUncertainty(DataGridViewCellPaintingEventArgs e)
		{
			Rectangle rc = e.CellBounds;
			PaintCell(e, false);

			const TextFormatFlags kFlags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
				TextFormatFlags.Left | TextFormatFlags.PreserveGraphicsClipping;

			Color clrText = GetCurrentCellDrawForeColor(e);

			rc.X += 4;
			rc.Width -= 4;
			DrawPhones(e.Graphics, _currPaintingCellEntry.Phones, 0,
				_currPaintingCellEntry.Phones.Length, rc, clrText, kFlags, true);

			DrawIndicatorCornerGlyphs(e.Graphics, e.CellBounds, _currPaintingCellEntry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the specified number of phones (via begin and end) in the specified
		/// direction, using the specified color, in the specified rectangle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawPhones(IDeviceContext g, string[] phones, int begin, int end,
			Rectangle rc, Color clrText, TextFormatFlags flags, bool drawForward)
		{
			if (begin == end)
				return;

			if (!drawForward)
				flags |= TextFormatFlags.Right;

			int i = begin;
			while (i != end && i < phones.Length)
			{
				Color clr = (_currPaintingCellEntry.ContiansUncertainties &&
					_currPaintingCellEntry.UncertainPhones.ContainsKey(i) ?
					_uncertainPhoneForeColor : clrText);

				TextRenderer.DrawText(g, phones[i], App.PhoneticFont, rc, clr, flags);

				int phoneWidth = TextRenderer.MeasureText(g, phones[i],
					App.PhoneticFont, Size.Empty, flags).Width;

				// If the phones are being drawn from L to R (which should always be true unless
				// we're drawing the environment before in a find phone search result grid) then
				// move the left edge of the rectangle forward by the width of the phone just
				// drawn. Otherwise move the right edge back by the width of the phone just drawn.
				rc.Width -= phoneWidth;

				if (!drawForward)
					i--;
				else
				{
					rc.X += phoneWidth;
					i++;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there are uncertain phones, draw a small red triangle in the top right corner
		/// of the phonetic cells. When there are experimental transcription conversions on an
		/// entry, draw a small green triangle in the bottom right corner of the phonetic cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DrawIndicatorCornerGlyphs(Graphics g, Rectangle rc,
			WordCacheEntry entry)
		{
			if (entry.Phones == null)
				return;

			// If their are uncertain phones, draw the little red
			// triangle in the top right corner.
			if (entry.ContiansUncertainties)
				DrawUncertaintyCornerGlyph(g, rc);
			
			// If their are experimental transcription conversions, draw the little green
			// triangle in the bottom right corner.
			if (entry.AppliedExperimentalTranscriptions != null)
				DrawExperimentalTransCornerGlyphs(g, rc);
		}

		/// ------------------------------------------------------------------------------------
		private static void DrawUncertaintyCornerGlyph(Graphics g, Rectangle rc)
		{
			Point pt1 = new Point(rc.Right - 7, rc.Y);
			Point pt2 = new Point(rc.Right - 1, rc.Y + 6);
			Point ptCorner = new Point(rc.Right - 1, rc.Top);

			using (var br = new LinearGradientBrush(pt1, pt2, Color.Red, Color.DarkRed))
				g.FillPolygon(br, new[] { pt1, pt2, ptCorner });

			if (s_showTopRightHotState)
			{
				pt2.Y += 9;
				pt1.X -= 10;
				using (var sbr = new SolidBrush(Color.FromArgb(100, Color.DarkRed)))
					g.FillPolygon(sbr, new[] { pt1, pt2, ptCorner });
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void DrawExperimentalTransCornerGlyphs(Graphics g, Rectangle rc)
		{
			Point pt1 = new Point(rc.Right - 8, rc.Bottom - 1);
			Point pt2 = new Point(rc.Right - 1, rc.Bottom - 8);
			Point ptCorner = new Point(rc.Right - 1, rc.Bottom - 1);

			using (var br = new LinearGradientBrush(pt1, pt2, Color.LightGreen, Color.DarkGreen))
				g.FillPolygon(br, new[] { pt1, pt2, ptCorner });

			if (s_showBottomRightHotState)
			{
				pt2.Y -= 10;
				pt1.X -= 10;
				using (var sbr = new SolidBrush(Color.FromArgb(100, Color.DarkGreen)))
					g.FillPolygon(sbr, new[] { pt1, pt2, ptCorner });
			}
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetCurrentCell(int col, int row)
		{
			if (row < 0 || row >= RowCount)
				row = 0;

			while (col < ColumnCount && !Columns[col].Visible)
				col++;

			if (col < ColumnCount && Columns[col].Visible && Rows[row].Visible)
				CurrentCell = this[col, row];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When minimal pairs view is off, this will turn it on, otherwise, the existing
		/// minimal pairs cache is just refreshed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CIEViewRefresh()
		{
			if (_cache.IsCIEList)
			{
				_cache = _backupCache;
				_backupCache = null;

				if (!CIEViewOn())
				{
					ManageNoCIEResultsMessage(false);
					_cache.Sort(_sortOptions);
					RefreshRows(true);
					return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turns on showing minimal pairs from the grid's cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CIEViewOn()
		{
			if (_cache.IsCIEList)
				return false;

			if (CIEOptions == null)
				CIEOptions = App.Project.CIEOptions;

			var builder = new CIEBuilder(_cache, _sortOptions, CIEOptions);
			var cieCache = builder.FindMinimalPairs();

			// This should never happen.
			if (cieCache == null)
			{
				Utils.MsgBox(LocalizationManager.GetString("Views.WordLists.NoMinimalPairsPopupMsg", "No minimal pairs to display."));
				return false;
			}

			if (cieCache.Count > 1)
			{
				if (_noCIEResultsMsg != null)
					ManageNoCIEResultsMessage(false);
			}
			else
			{
				cieCache.Clear();
				if (_noCIEResultsMsg == null)
					ManageNoCIEResultsMessage(true);
			}

			if (_cellInfoPopup != null)
				_cellInfoPopup.Hide();

			_backupCache = _cache;
			_cache = cieCache;
			RefreshRows(true);
			App.MsgMediator.SendMessage("AfterWordListGroupingByCIE", this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turns off showing minimal pairs from the grid's cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CIEViewOff()
		{
			if (_backupCache != null)
			{
				if (_cellInfoPopup != null)
					_cellInfoPopup.Hide();

				ManageNoCIEResultsMessage(false);
				_cache = _backupCache;
				_backupCache = null;
				_cache.Sort(_sortOptions);
				RefreshRows(true);
				CIEOptions = null;
				App.MsgMediator.SendMessage("AfterWordListUnGroupingByCIE", this);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ManageNoCIEResultsMessage(bool show)
		{
			// If the message isn't showing and we're supposed to
			// turn it off, then there's nothing to be done.
			if (!show && _noCIEResultsMsg == null)
				return;
			
			Utils.SetWindowRedraw(this, false, false);

			if (!show)
			{
				RowHeadersVisible = true;
				ColumnHeadersVisible = true;
				BackgroundColor = SystemColors.Window;
				Controls.Remove(_noCIEResultsMsg);
				_noCIEResultsMsg.Dispose();
				_noCIEResultsMsg = null;
			}
			else
			{
				RowHeadersVisible = false;
				ColumnHeadersVisible = false;
				BackgroundColor = SystemColors.Control;
				_noCIEResultsMsg = new Label();
				_noCIEResultsMsg.Font = new Font(FontHelper.UIFont, FontStyle.Bold);
				_noCIEResultsMsg.AutoSize = false;
				_noCIEResultsMsg.Dock = DockStyle.Fill;
				_noCIEResultsMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				_noCIEResultsMsg.BackColor = Color.Transparent;
				_noCIEResultsMsg.MouseDown += delegate { Focus(); };
				_noCIEResultsMsg.Text = Utils.ConvertLiteralNewLines(LocalizationManager.GetString("Views.WordLists.NoMinimalPairsMsg",
					"No minimal pairs to display.\nChange the minimal pairs options and try again.",
					"Shows in place of a word list grid when the user has turned on minimal pairs and there aren't any to show."));

				Controls.Add(_noCIEResultsMsg);
				_noCIEResultsMsg.BringToFront();
				App.MsgMediator.SendMessage("NoCIEResultsShowing", this);
			}

			Utils.SetWindowRedraw(this, true, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the grid's rows and rebuilds them from the grid's cache, grouping
		/// records if the grid is currently grouped on the primary sort field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshRows()
		{
			RefreshRows(IsGroupedByField);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the grid's rows and rebuilds them from the grid's cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshRows(bool groupRecords)
		{
			Rows.Clear();

			if (_cache == null || _cache.IsEmpty)
				return;

			for (int i = 0; i < _cache.Count; i++)
			{
				if (!_cache.IsCIEList)
					_cache[i].CIEGroupId = -1;

				Rows.Add(new PaCacheGridRow(i));
			}

			if (groupRecords)
				WordListGroupingBuilder.Group(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort phonetically or based on the column clicked.
		/// </summary>
		/// <param name="colName">string</param>
		/// <param name="changeSortDirection">bool</param>
		/// ------------------------------------------------------------------------------------
		public virtual void Sort(string colName, bool changeSortDirection)
		{
			if (Cache == null || Cache.IsEmpty)
				return;

			var args = new object[] {colName, changeSortDirection};
			if (App.MsgMediator.SendMessage("BeforeWordListSorted", args))
				return;
			
			// Do this just in case the message mediator call changed the values.
			colName = args[0] as string;
			changeSortDirection = (bool)args[1];

			// Remove the SortGlyph from the previous sort column header
			foreach (DataGridViewColumn col in Columns)
				col.HeaderCell.SortGlyphDirection = SortOrder.None;

			bool ascending = _cache.Sort(SortOptions, colName, changeSortDirection);

			// Add the sortGlyphDirection to the current column
			Columns[colName].HeaderCell.SortGlyphDirection =
				(ascending ? SortOrder.Ascending : SortOrder.Descending);

			// UnGroup and ReGroup by new sort column
			bool allGroupCollapsed = false;
			if (IsGroupedByField && !Cache.IsCIEList)
			{
				allGroupCollapsed = AllGroupsCollapsed;
				var groupByField = _groupByField;
				//ToggleGroupExpansion(true);
				_groupByField = null;
				WordListGroupingBuilder.UnGroup(this);
				_groupByField = groupByField;

				// This code is necessary for correctly changing the Group Headings
				if (SortOptions.SortFields != null &&
					SortOptions.SortFields.Count > 0)
				{
					_groupByField = SortOptions.SortFields[0].Field;
				}
			}

			// Can't be Grouped By Field and Minimal Pairs at the same time.
			if (IsGroupedByField && !Cache.IsCIEList)
				WordListGroupingBuilder.Group(this);

			// Recollapse all groups if needed
			if (allGroupCollapsed)
				ToggleGroupExpansion(false);
			
			// Make the grid update its display
			Invalidate();
			App.MsgMediator.SendMessage("WordListGridSorted", this);

			if (Cursor == Cursors.WaitCursor)
				Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ToggleGroupExpansion(bool expand)
		{
			ToggleGroupExpansion(expand, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to expand or collapse all groups shown in the grid (when the
		/// primary sort field is not displayed in groups, this method will do nothing).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ToggleGroupExpansion(bool expand, bool forceStateChange)
		{
			if (!IsGroupedByField && (!_cache.IsCIEList || _cache.IsEmpty))
				return;

			Utils.SetWindowRedraw(this, false, false);
	
			// All the Sorted row groups were either expanded or collapsed
			_toggleGroupExpansion = true;
			AllGroupsCollapsed = !expand;
			AllGroupsExpanded = expand;

			App.InitializeProgressBar(expand ?
				LocalizationManager.GetString("Views.WordLists.ExpandingGroupsProgressStatusMsg", "Expanding groups...") :
				LocalizationManager.GetString("Views.WordLists.CollapsingGroupsProgressStatusMsg", "Collapsing groups..."), RowCount);

			foreach (var row in Rows.Cast<DataGridViewRow>().Where(r => r is SilHierarchicalGridRow))
			{
				((SilHierarchicalGridRow)row).SetExpandedState(expand, forceStateChange, false);
				App.IncProgressBar(((SilHierarchicalGridRow)row).ChildCount);
			}

			// Force users to restart Find when collapsing all rows
			if (!expand)
				FindInfo.CanFindAgain = false;

			SetCurrentCell(0, 0);
			App.IncProgressBar(RowCount);
			App.UninitializeProgressBar();
			_toggleGroupExpansion = false;
			Utils.SetWindowRedraw(this, true, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the font used for the columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void RefreshColumnFonts(bool updateColumnFonts)
		{
			_defaultRowHeight = 0;

			foreach (DataGridViewColumn col in Columns)
			{
				if (updateColumnFonts)
				{
					var field = App.Project.GetFieldForName(col.Name);
					if (field != null)
						col.DefaultCellStyle.Font = field.Font;
				}

				if (col.Visible && col.DefaultCellStyle.Font != null)
					_defaultRowHeight = Math.Max(_defaultRowHeight, col.DefaultCellStyle.Font.Height);
			}

			// Add a little vertical padding.
			_defaultRowHeight += Settings.Default.WordListVerticalRowPadding;

			// Get rid of all custom row heights (i.e. row heights adjusted by the user).
			_customRowHeights = null;

			if (RowCount > 0)
			{
				// Reset all the row heights to the new height. I used to update the row
				// heights using a call to AutoResizeRows. But that caused a bug (see
				// PA-193 and PA-81). Calling Updating row heights this way fixes it.
				UpdateRowHeightInfo(0, true);
				
				// Make sure the selected row is scrolled into view.
				if (CurrentRow != null && !CurrentRow.Displayed)
					ScrollRowToMiddleOfGrid(CurrentRow.Index);

				// Invalidate the row headers because they don't always paint
				// correctly right away after a call to UpdateRowHeightInfo.
				Invalidate(new Rectangle(0, 0, RowHeadersWidth, ClientSize.Height));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the current row, sort options and first visible row to those specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void PostDataSourceModifiedRestore(int row, int column,
			int firstRow, SortOptions savedSortOptions)
		{
			PostDataSourceModifiedRestore(row, column, firstRow, savedSortOptions, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the current row, sort options and first visible row to those specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void PostDataSourceModifiedRestore(int row, int column, 
			int firstRow, SortOptions savedSortOptions, CIEOptions cieOptions)
		{
			// Restore and apply the saved sort options
			if (savedSortOptions != null)
				SortOptions = savedSortOptions;

			if (cieOptions != null)
			{
				CIEOptions = cieOptions;
				CIEViewOn();
			}

			// Restore the first visible row.
			if (firstRow < Rows.Count && firstRow >= 0 && Rows[firstRow].Visible)
				FirstDisplayedScrollingRowIndex = firstRow;
			
			// Restore the selected row and column.
			if (Rows.Count > row)
			{
				SetCurrentCell(column, row);
				CurrentRow.Selected = true;
				InvalidateRow(row);
			}

			Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the specified cell's row to the middle of the screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void ScrollRowToMiddleOfGrid(int rowIndex)
		{
			// Number of visible rows that will be displayed above the row with the matched cell
			int backupRowCount = DisplayedRowCount(true) / 2;
			int firstDisplayRowIndex = rowIndex - 1;

			while (firstDisplayRowIndex > 0)
			{
				// firstDisplayRowIndex should never be >= RowCount, but just in case.
				if (firstDisplayRowIndex < RowCount)
				{
					if (Rows[firstDisplayRowIndex].Visible)
						backupRowCount--;

					if (backupRowCount == 0)
						break;
				}

				firstDisplayRowIndex--;
			}

			try
			{
				// Sometimes this fails with a "No room is available to display rows"
				// exception, but almost never. I'm not sure what June did, but I think it
				// had something to do with running in 120dpi. So wrap in a try/catch.
				FirstDisplayedScrollingRowIndex = firstDisplayRowIndex;
			}
			catch
			{
				try
				{
					if (FirstDisplayedCell != null)
						FirstDisplayedScrollingRowIndex = 0;
				}
				catch { }
			}
		}

		#endregion

		#region Message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets called whenever the sort options change from modifications made on the
		/// sorting tab of the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnSortingOptionsChanged(object args)
		{
			bool performSort = (args != null && args is bool && (bool)args);

			SortOptions sortOptions = null;

			// Load the sort options appropriate for the grid's view type. Can't compare
			// types because the m_owningViewType's are declared in PaDll which cannot
			// be referenced by PaControls since PaControls is referenced by PaDll.
			if (OwningViewType.Name == "DataCorpusVw")
				sortOptions = App.Project.DataCorpusVwSortOptions;
			else if (OwningViewType.Name == "SearchVw")
				sortOptions = App.Project.SearchVwSortOptions.Copy();
			else if (OwningViewType.Name == "XYChartVw")
				sortOptions = App.Project.DistributionChartVwSortOptions.Copy();

			if (sortOptions == null)
				sortOptions = new SortOptions(true, App.Project);

			// If the default sort options should not change as the user clicks headings or
			// changes phonetic sort options from the phonetic sort option drop-down, then
			// clone the sort options stored in the project so when the user does click
			// headings and change phonetic sort options, the changes won't get saved to
			// the sort options object in the project.
			if (!sortOptions.SaveManuallySetSortOptions)
				sortOptions = sortOptions.Copy();

			if (performSort)
				SortOptions = sortOptions;
			else
				_sortOptions = sortOptions;

			FindInfo.ResetStartSearchCell(false);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnDataSourcesModified(object args)
		{
			var project = args as PaProject;
			var mappedFields = project.GetMappedFields().ToList();

			// Hide the first column (collapse/expand group) so it won't mess up
			// the calculations for the other columns
			if (IsGroupedByField)
				Columns[0].Visible = false;

			int hierarchicalGridColumnCount = 0;

			// Make sure there are no columns for fields that no longer exist in the project.
			for (int i = ColumnCount - 1; i >= 0; i--)
			{
				if (Columns[i] is SilHierarchicalGridColumn)
					hierarchicalGridColumnCount++;
				else
				{
					// If there's no longer a field for the column, then remove it.
					if (!mappedFields.Any(f => f.Name == Columns[i].Name))
						Columns.RemoveAt(i);
				}
			}

			// Make sure there are columns for new fields
			// that may have been added to the project.
			foreach (var field in mappedFields)
			{
				var col = Columns[field.Name];
				if (col == null)
				{
					if (field.DisplayIndexInGrid < 0 || !project.LastNewlyMappedFields.Contains(field.Name))
						continue;

					field.VisibleInGrid = true;
					col = AddNewColumn(field);
				}

				col.HeaderText = field.DisplayName;

				col.DefaultCellStyle.Alignment = (field.RightToLeft && field.Type != FieldType.Phonetic ?
					DataGridViewContentAlignment.MiddleRight : DataGridViewContentAlignment.MiddleLeft);

				col.HeaderCell.Style.Alignment = (field.RightToLeft && field.Type != FieldType.Phonetic ?
					DataGridViewContentAlignment.MiddleRight : DataGridViewContentAlignment.MiddleLeft);
				
				if (field.DisplayIndexInGrid < 0)
					col.Visible = false;
				else
				{
					try
					{
						// Any SilHierarchicalGridColumns are hidden above, and setting the
						// column's dislpay index below will cause the hierarchical columns
						// to be bumped to the end of the column's display list -- and since
						// they are frozen columns, that leads to bad things. Therefore,
						// adding hierarchicalGridColumnCount to DisplayIndexInGrid will
						// make sure the hierarchical grid columns stay at the beginning of
						// the displayed columns.
						col.DisplayIndex = field.DisplayIndexInGrid + hierarchicalGridColumnCount;
					}
					catch
					{
						col.DisplayIndex = Columns.Count - 1;
					}

					col.Visible = field.VisibleInGrid;
				}
			}

			CleanupColumns();

			// Restore the first column's (collapse/expand group) visibility 
			// and reset its display index to 0.
			if (IsGroupedByField)
			{
				Columns[0].DisplayIndex = 0;
				Columns[0].Visible = true;
			}

			if (CurrentRow != null)
				InvalidateRow(CurrentRow.Index);

			RefreshColumnFonts(true);
			Cursor = Cursors.Default;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the CV patterns get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnCVPatternsChanged(object args)
		{
			// Check if the CV pattern is one of the fields on which the list
			// is sorted. If it is, then resort the word list. This will also
			// regroup the list if it's grouped.
			if (_sortOptions != null && _sortOptions.SortFields != null &&
				_sortOptions.SortFields.Any(si => si.Field.Name == PaField.kCVPatternFieldName))
			{
				Sort(_sortOptions.SortFields[0].Field.Name, false);
			}
			else
			{
				DataGridViewColumn col = Columns[PaField.kCVPatternFieldName];
				if (col != null)
					InvalidateColumn(col.Index);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnWordListOptionsChanged(object args)
		{
			App.Project.GridLayoutInfo.InitializeGrid(this);

			// Force users to restart Find when adding or removing columns
			FindInfo.CanFindAgain = false;

			// This will make sure that if a column was made visible or hidden,
			// that the row height is recalculated based on the heights of the
			// fonts in the visible rows.
			RefreshColumnFonts(false);

			CleanupColumns();

			// Return false so everyone who cares gets a crack at handling the message.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// After word list options have changed, or the project's fields list has changed
		/// (e.g. custom fields change), it's possible the grid is grouped on a column that was
		/// removed or hidden. Therefore, ungroup the list. In addition, there could be columns
		/// that were removed or hidden and are in the sort options. If that's the case, they
		/// are removed from the sort options and the list is resorted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CleanupColumns()
		{
			// If the column that was grouped on is no longer visible, then ungroup.
			if (_groupByField != null)
			{
				if (!Columns.Contains(_groupByField.Name) ||
					!Columns[_groupByField.Name].Visible)
				{
					GroupByField = null;
				}
			}

			if (SortOptions == null)
				return;

			// If the sort options contain any fields whose column was removed, then
			// remove the field from the sort options and resort the list.
			bool reSort = false;
			for (int i = SortOptions.SortFields.Count - 1; i >= 0; i--)
			{
				string fldName = SortOptions.SortFields[i].Field.Name;

				if (!Columns.Contains(fldName) || !Columns[fldName].Visible)
				{
					SortOptions.SortFields.RemoveAt(i);
					reSort = true;
				}
			}

			if (!reSort)
				return;

			if (SortOptions.SortFields.Count > 0)
			{
				// Sort on the first column in the sort option's field list.
				Sort(SortOptions.SortFields[0].Field.Name, false);
				return;
			}

			// There isn't a column left on which to sort, so pick the first visible column.
			foreach (var col in Columns.Cast<DataGridViewColumn>().Where(c => c.Visible))
			{
				Sort(col.Name, false);
				return;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnEditSourceRecord(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			int row = -1;
			if (args != null && args is int)
				row = (int)args;
			else if (CurrentRow != null)
				row = CurrentRow.Index;

			var wentry = GetWordEntry(row);
			if (wentry != null)
				new DataSourceEditor(wentry.WordCacheEntry, FindForm().Text);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A hidden feature is that when the user presses Ctrl+Shift+F2 on an entry that
		/// came from a FW database, a dialog with the jump url will be shown before jumping
		/// to Flex.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			// Ctrl+Up and Ctrl+Down inherently move the active cell to places that are
			// useless to us in PA and can also be accomplished with other key combinations.
			// Eat the sequence in case a menu accelerator wants to use this combination.
			if (e.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
			{
				e.Handled = true;
				base.OnKeyDown(e);
				return;
			}

			if (e.KeyCode == Keys.Enter && Settings.Default.WordListEditSourceOnEnterKey)
			{
				e.Handled = true;
				base.OnKeyDown(e);
				OnEditSourceRecord(null);
				return;
			}

			base.OnKeyDown(e);

			// A hidden feature is that when the user presses Ctrl+Shift+F2 on an
			// entry that came from a FW database, a dialog with the jump url will
			// be shown before jumping to Flex.
			if (e.Control && e.Shift && e.KeyCode == Keys.F2 && GetWordEntry() != null)
				new DataSourceEditor(GetWordEntry().WordCacheEntry, null, true);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Uncomment if the desire is to have the menu item disabled when the user cannot
		///// edit the source record. As it is now, if a record cannot be edited, a dialog
		///// tells the user this.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateEditSourceRecord(object args)
		//{
		//    TMItemProperties itemProps = args as TMItemProperties;

		//    if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
		//        itemProps == null || CurrentRow == null || CurrentRow.Index < 0 ||
		//        CurrentRow.Index >= m_cache.Count)
		//        return false;

		//    itemProps.Update = true;
		//    itemProps.Visible = true;
		//    itemProps.Enabled = PaApp.IsToolboxRunning &&
		//        m_cache[CurrentRow.Index].WordCacheEntry.RecordEntry.CanBeEditedInToolbox;

		//    return true;
		//}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownShowGridColumns(object args)
		{
			//if (!Focused || !App.IsViewOrFormActive(m_owningViewType, FindForm()))
			//    return false;

			//ToolBarPopupInfo itemProps = args as ToolBarPopupInfo;
			//if (itemProps == null)
			//    return false;

			//// Create a popup control with checkboxes used to turn column's on and off.
			//GridColumnVisibilitySetter gcvs = new GridColumnVisibilitySetter(this);
			//gcvs.Dock = DockStyle.Fill;

			//SizableDropDownPanel gcvsHost =
			//    new SizableDropDownPanel("savedshowhidecolumndropdownsize", gcvs.DefltSize);
			//gcvsHost.BackColor = SystemColors.Menu;
			//gcvsHost.Padding = new Padding(0, 0, 0, gcvsHost.Padding.Bottom);
			//gcvsHost.Controls.Add(gcvs);

			//itemProps.Control = gcvsHost;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the settings after the user changes the visible columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedShowGridColumns(object args)
		{
			SaveSettings();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowGridColumns(object args)
		{
			Form parentForm = FindForm();

			if (!Focused || !App.IsViewOrFormActive(OwningViewType, parentForm))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = parentForm.ContainsFocus;
			itemProps.Update = true;
			return itemProps.Enabled;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesNone(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.None;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesBoth(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.Single;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesHorizontal(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesVertical(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			RefreshColumnFonts(true);

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool OnPlayback(object args)
		{
			if ((!Focused && !IsCurrentPlaybackGrid) ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			PlaybackAllSelectedRows(false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnPlaybackRepeatedly(object args)
		{
			if ((!Focused && !IsCurrentPlaybackGrid) ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			PlaybackAllSelectedRows(true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || (!Focused && !IsCurrentPlaybackGrid) ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			bool enable = (GetAudioEntriesInSelectedRows() != null && !_playbackInProgress);
			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAdjustPlaybackSpeed(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAdjustPlaybackSpeedParent(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnStopPlayback(object args)
		{
			if ((!Focused && !IsCurrentPlaybackGrid) ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			_playbackInProgress = false;
			_playbackAborted = true;
			
			if (_audioPlayer != null)
				_audioPlayer.Stop();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnStopAllPlayback(object args)
		{
			if (_playbackInProgress)
			{
				_playbackInProgress = false;
				_playbackAborted = true;
				
				if (_audioPlayer != null)
					_audioPlayer.Stop();
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || (!Focused && !IsCurrentPlaybackGrid) ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			itemProps.Visible = true;
			itemProps.Enabled = AudioPlayer.IsPlaybackInProgress || _playbackInProgress;
			itemProps.Update = true;
			return true;
		}
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle groups expanding and collapsing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void GroupExpandedChangedHandler(SilHierarchicalGridRow row)
		{
			if (!row.Expanded && row.IsRowOwned(FindInfo.FirstMatchedRow))
			{
				SetCurrentCell(0, (row.Index >= 0 ? row.Index : 0));

				// Move the cell's row to the screen's top if it is not on the screen
				if (CurrentCell != null && !CurrentCell.Displayed)
					FirstDisplayedScrollingRowIndex = (row.Index < 1 ? 0 : row.Index);

				FindInfo.ResetStartSearchCell(false);
			}

			// Enable/disable the 'Collapse all groups' and 'Expand all groups' toolbar 
			// buttons and menu selections.

			// All the Sorted row groups were either expanded or collapsed.
			// This logic is handled in the method ToggleGroupExpansion()
			if (_toggleGroupExpansion)
				return;

			// Single hierarchical row expanded
			if (row.Expanded && AllGroupsCollapsed)
			{
				AllGroupsCollapsed = false;
				return;
			}

			// Single hierarchical row collapsed
			if (!row.Expanded && AllGroupsExpanded)
			{
				AllGroupsExpanded = false;
				return;
			}

			// Loop through all the SilHierarchicalGridRow's and see if any
			// of them are expanded. If so, set AllGroupsCollapsed to false.
			AllGroupsCollapsed = true;
			AllGroupsExpanded = true;
			foreach (DataGridViewRow rw in Rows)
			{
				var shgr = rw as SilHierarchicalGridRow;
				if (shgr != null)
				{
					if (shgr.Expanded)
					{
						AllGroupsCollapsed = false;
						if (!AllGroupsExpanded)
							return;
					}
					else
					{
						AllGroupsExpanded = false;
						if (!AllGroupsCollapsed)
							return;
					}
				}
			}
		}

		#region Find Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the FindDlg form.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFind(object args)
		{
			bool isGridCurrent =
				App.MsgMediator.SendMessage("CompareGrid", this) || Focused;

			if (FindInfo.FindDlgIsOpen || !isGridCurrent ||
				!App.IsViewOrFormActive(OwningViewType, FindForm()))
			{
				return false;
			}

			new FindDlg(this).Show(); // Not modal
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycle through the cell rows that matched the Find search criteria.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFindNext(object args)
		{
			bool isGridCurrent =
				App.MsgMediator.SendMessage("CompareGrid", this) || Focused;

			if (!isGridCurrent || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			FindInfo.Grid = this;
			FindInfo.Find(false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycle backwards through the cell rows that matched the Find search criteria.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFindPrevious(object args)
		{
			bool isGridCurrent =
				App.MsgMediator.SendMessage("CompareGrid", this) || Focused;

			if (!isGridCurrent || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			FindInfo.Grid = this;
			FindInfo.Find(true);
			return true;
		}
		#endregion

		#region Playback methods
		/// ------------------------------------------------------------------------------------
		private void PlaybackAllSelectedRows(bool repeatedly)
		{
			if (_playbackInProgress)
				return;

			_playbackAborted = false;
			_playbackInProgress = true;
			PreparePlaybackControlButtonsForPlayback();

			_audioPlayer = new AudioPlayer();

			do
			{
				// Loop through the selected items and playback their utterances.
				SortedList<int, WordCacheEntry> selectedWords = GetAudioEntriesInSelectedRows();
				if (selectedWords == null)
					break;
				
				foreach (var entry in selectedWords)
				{
					_currPlaybackRow = entry.Key;
					InvalidateRow(entry.Key);
					Application.DoEvents();
					PlaybackSingleEntry(entry.Value);
					_currPlaybackRow = -2;
					InvalidateRow(entry.Key);
					Application.DoEvents();

					if (_playbackAborted)
						break;
				}
			}
			while (!_playbackAborted && repeatedly);

			_playbackInProgress = false;
			_audioPlayer = null;
			App.MsgMediator.SendMessage("PlaybackEnded", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the playback control buttons to their appropriate state just before playback
		/// begins. Do this explicitly because these buttons won't get an update messages
		/// until after playback is complete. By that time, it's too late.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PreparePlaybackControlButtonsForPlayback()
		{
			// Update these buttons since they won't get an update
			// message until after playback is complete.
			var itemProps = TMAdapter.GetItemProperties("tbbStopPlayback");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = true;
				itemProps.Update = true;
				TMAdapter.SetItemProperties("tbbStopPlayback", itemProps);
			}

			itemProps = TMAdapter.GetItemProperties("tbbPlaybackOnMenu");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				TMAdapter.SetItemProperties("tbbPlaybackOnMenu", itemProps);
			}

			itemProps = TMAdapter.GetItemProperties("tbbPlaybackRepeatedly");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				TMAdapter.SetItemProperties("tbbPlaybackRepeatedly", itemProps);
			}

			itemProps = TMAdapter.GetItemProperties("tbbAdjustPlaybackSpeedParent");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				TMAdapter.SetItemProperties("tbbAdjustPlaybackSpeedParent", itemProps);
			}

			// Inform the main window what's happening.
			App.MsgMediator.SendMessage("PlaybackBeginning", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Plays back the audio for the specified word cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PlaybackSingleEntry(WordCacheEntry entry)
		{
			var recEntry = entry.RecordEntry;
			if (recEntry == null || _audioPlayer == null)
				return;

			var offset = entry.AudioOffset;
			var length = entry.AudioLength;
			string audioFile = entry.GetAudioFileUsingFallBackIfNecessary();

			// Get the playback speed for the Control grid
			_playbackSpeed = App.GetPlaybackSpeedForVwType(OwningViewType);
			
			// If the speed is not 100% then use Speech Analyzer to playback the utterance.
			if (_playbackSpeed != 100)
			{
				PlaybackEntryUsingSA(recEntry.DataSource.Type, audioFile, offset, length);
				return;
			}

			// For SA wave files, the offset and length are in bytes, not milliseconds.
			// Therefore, we need to calculate what those values are in milliseconds.
			if (recEntry.DataSource.Type == DataSourceType.SA)
			{
				offset = AudioPlayer.ByteValueToMilliseconds(offset, recEntry.Channels,
					recEntry.SamplesPerSecond, recEntry.BitsPerSample);

				length = AudioPlayer.ByteValueToMilliseconds(length, recEntry.Channels,
					recEntry.SamplesPerSecond, recEntry.BitsPerSample);
			}

			_audioPlayer.Play(audioFile, offset, offset + length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Uses Speech Analyzer to play back the specified audio file from the specified
		/// offset, for the specified length;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PlaybackEntryUsingSA(DataSourceType dataSourceType, string audioFile,
			long offset, long length)
		{
			// If the entry is not from an SA audio file, the units for the offset
			// and length are assumed to be in milliseconds. Since SA will perform
			// the altered speed playback and it needs the values in bytes convert
			// the millisecond values to bytes.
			if (dataSourceType != DataSourceType.SA)
			{
				offset = AudioPlayer.MillisecondValueToBytes(offset, audioFile);
				length = AudioPlayer.MillisecondValueToBytes(length, audioFile);
			}

			// If AlteredSpeedPlayback returns null it means SA couldn't be found.
			// Therefore, abort trying to playback anymore utterances.
			var saPrs = _audioPlayer.AlteredSpeedPlayback(FindForm().Text,
				audioFile, offset, offset + length, _playbackSpeed);

			if (saPrs == null)
			{
				_playbackAborted = true;
				return;
			}

			if (_stopPlaybackKey != Keys.None)
			{
				// Create a global hook for the key that stops playback.
				_kbHook = new LocalWindowsHook(HookType.WH_KEYBOARD);
				_kbHook.HookInvoked += m_kbHook_HookInvoked;
				_kbHook.Install();
			}

			while (!saPrs.HasExited)
			{
				// It appears as though there are some times when HasExited returns false and
				// calling Kill throws an exception because the process has already exited.
				// I think it is when pressing F8 multiple times in quick succession.
				// Therefore, wrap it all in a try/catch and go on our merry way.
				try
				{
					Application.DoEvents();
					if (_playbackAborted && !saPrs.HasExited)
						saPrs.Kill();
				}
				catch { }
			}

			if (_kbHook != null)
			{
				_kbHook.Uninstall();
				_kbHook = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		void m_kbHook_HookInvoked(object sender, HookEventArgs e)
		{
			if (e.wParam.ToInt32() == (int)_stopPlaybackKey)
				_playbackAborted = true;
		}

		#endregion

		#region Update handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesNone(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.None);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesBoth(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.Single);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesHorizontal(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesVertical(object args)
		{
			if (!Focused || !App.IsViewOrFormActive(OwningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical);
			itemProps.Update = true;
			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}

	#region PaCacheGridRow class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaCacheGridRow : DataGridViewRow
	{
		private int m_cacheEntryIndex = -1;
		private SilHierarchicalGridRow m_parentRow;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaCacheGridRow()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaCacheGridRow(int cacheEntryIndex) : this()
		{
			m_cacheEntryIndex = cacheEntryIndex;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the index of the row's corresponding cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CacheEntryIndex
		{
			get { return m_cacheEntryIndex; }
			set { m_cacheEntryIndex = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the SilHierarchicalGridRow to which this row is subordinate.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow ParentRow
		{
			get { return m_parentRow; }
			set { m_parentRow = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an exact copy of the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			object clone = base.Clone();

			if (clone is PaCacheGridRow)
			{
				((PaCacheGridRow)clone).m_cacheEntryIndex = m_cacheEntryIndex;
				((PaCacheGridRow)clone).m_parentRow = m_parentRow;
			}

			return clone;
		}
	}

	#endregion
}
